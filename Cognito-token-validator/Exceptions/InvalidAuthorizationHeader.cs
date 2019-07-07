using System;
using System.Runtime.Serialization;

namespace Cognito_token_validator.Exceptions
{
    public class InvalidAuthorizationHeader : Exception
    {
        public InvalidAuthorizationHeader()
        {
        }

        public InvalidAuthorizationHeader(string message) : base(message)
        {
        }

        public InvalidAuthorizationHeader(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidAuthorizationHeader(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
