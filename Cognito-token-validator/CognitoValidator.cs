using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Cognito_token_validator.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;

namespace Cognito_token_validator
{
    public class CognitoValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly string _authorizationHeader;
        private readonly string _token;
        private readonly SecurityToken _webToken;
        private readonly JwtSecurityToken _decodedToken;

        public CognitoValidator(string authorizationHeader)
        {
            _authorizationHeader = authorizationHeader;
            _token = GetBearerToken();
            _tokenHandler = new JwtSecurityTokenHandler();
            _webToken = ProcessWebToken();
            _decodedToken = ProcessToken();
        }

        public string ValidateToken()
        {
            // get kid and jwks url
            var kid = _decodedToken.Header.Kid;
            var jwksUrl = GetJwksUrl();

            // get json web keys from its urls
            var jsonWebKeys = new WebClient().DownloadString(jwksUrl);

            Console.WriteLine(kid);
            Console.WriteLine(jwksUrl);
            Console.WriteLine(_decodedToken.EncodedPayload);

            // creates a json web keys set
            var jwks = new JsonWebKeySet(jsonWebKeys);
            var signedKeys = jwks.GetSigningKeys();
            foreach (var securityKey in signedKeys)
            {
                Console.WriteLine("jwk");
                Console.WriteLine(securityKey.KeyId);
            }

            // validate token
            SecurityToken validatedToken;
            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = _decodedToken.Audiences.First(),
                IssuerSigningKeys = signedKeys,
                ValidIssuer = _decodedToken.Issuer
            };
            _tokenHandler.ValidateToken(_token, validationParameters, out validatedToken);
            Console.WriteLine(validatedToken);

            return "ok";
        }

        public string GetToken()
        {
            return _token;
        }

        public SecurityToken GetDecodedToken()
        {
            return _decodedToken;
        }

        public List<string> GetRoles()
        {
            var claimList = new List<string>();
            var tokenClaims = _decodedToken.Claims;
            foreach (var tokenClaim in tokenClaims)
            {
                Console.WriteLine(tokenClaim.Type);
                if (tokenClaim.Type.Contains("cognito:groups"))
                {
                    claimList.Add(tokenClaim.Value);
                }
            }

            return claimList;
        }

        private SecurityToken ProcessWebToken()
        {
            return _tokenHandler.ReadJwtToken(_token);
        }

        private JwtSecurityToken ProcessToken()
        {
            return _tokenHandler.ReadJwtToken(_token);
        }

        private string GetBearerToken()
        {
            if (string.IsNullOrWhiteSpace(_authorizationHeader) || !_authorizationHeader.Contains("Bearer "))
            {
                throw new InvalidAuthorizationHeader("Invalid Authorization Header");
            }
            return _authorizationHeader.Replace("Bearer ", "");
        }

        private string GetJwksUrl()
        {
            return $"{_decodedToken.Issuer}/.well-known/jwks.json";
        }
    }
}
