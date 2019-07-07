using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cognito_token_validator;
using Cognito_token_validator.Exceptions;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/home
        [HttpGet]
        public ActionResult<string> Get(int id)
        {
            try
            {
                string authorizationHeader = Request.Headers["Authorization"];
                var validator = new CognitoValidator(authorizationHeader);
                Console.WriteLine(validator.GetDecodedToken());
                return validator.GetToken();
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
