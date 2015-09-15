using System;

namespace Ironhide.Api.Host
{
    public class CandidateRequestException : Exception
    {
        const string FailureMessage =
            "Hello candidate! Thanks for playing. However, something was not right. {0} Please kindly check everything and try again.";

        public CandidateRequestException(string sentence)
            : base(string.Format(FailureMessage, sentence))
        {
            
        }
    }
}