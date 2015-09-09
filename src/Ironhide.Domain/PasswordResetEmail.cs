using System;

namespace Ironhide.Users.Domain
{
    public class PasswordResetEmail
    {
        public string ResetUrl { get; private set; }

        public PasswordResetEmail(string baseUrl, Guid token)
        {
            ResetUrl = string.Format("{0}/#/reset-password?token={1}", baseUrl, token.ToString().ToUpper());
        }
    }
}