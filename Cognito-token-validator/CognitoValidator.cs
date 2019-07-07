using Cognito_token_validator.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace Cognito_token_validator
{
    public class CognitoValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly string _authorizationHeader;
        private readonly string _token;
        private readonly JwtSecurityToken _decodedToken;

        public CognitoValidator(string authorizationHeader)
        {
            _authorizationHeader = authorizationHeader;
            _token = GetBearerToken();
            _tokenHandler = new JwtSecurityTokenHandler();
            _decodedToken = ProcessToken();
        }

        public string ValidateToken()
        {
            return "ok";
        }

        public string GetToken()
        {
            return _token;
        }

        public JwtSecurityToken GetDecodedToken()
        {
            return _decodedToken;
        }

        public string IsAuthorized()
        {
            if (string.IsNullOrWhiteSpace(_authorizationHeader))
            {
                return "not_authorized";
            }
            return "authorized";
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
    }
}
