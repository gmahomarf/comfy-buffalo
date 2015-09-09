using System;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class UserInputPropertyMissingException : Exception
    {
        public UserInputPropertyMissingException(string parameterName)
            : base(string.Format("The json body property '{0}' was missing and is required.", parameterName))
        {

        }
    }
}