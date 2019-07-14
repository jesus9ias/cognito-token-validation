using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Cognito_token_validator.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class AuthorizeCognitoToken : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
            var validator = new CognitoValidator(authorizationHeader);
            validator.ValidateToken();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }
}
