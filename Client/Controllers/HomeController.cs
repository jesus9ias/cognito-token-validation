using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cognito_token_validator;
using Cognito_token_validator.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/home
        [Authorize]
        [HttpGet]
        [Route(("validate"))]
        public ActionResult<string> Validate(int id)
        {
            return "Ok, token validated";
        }

        [HttpGet]
        [Route(("auth"))]
        public ActionResult<string> Auth(int id)
        {
            try
            {
                string authorizationHeader = Request.Headers["Authorization"];
                var validator = new CognitoValidator(authorizationHeader);
                Console.WriteLine(validator.GetToken());
                Console.WriteLine(validator.GetDecodedToken());
                //  var roles = validator.GetRoles();
                //  Console.WriteLine(string.Join("-", roles));
                return validator.ValidateToken();
            }
            catch (InvalidAuthorizationHeader e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
