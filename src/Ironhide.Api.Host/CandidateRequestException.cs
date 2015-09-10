using System;

namespace Ironhide.Api.Host
{
    public class CandidateRequestException : Exception
    {
        const string FailureMessage =
            "Hello candidate! Thanks for playing. However, something was not right. Please kindly check everything and try again.";

        public CandidateRequestException():base(FailureMessage)
        {
            
        }
    }
}