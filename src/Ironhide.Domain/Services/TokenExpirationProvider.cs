using System;
using System.Configuration;

namespace Ironhide.Users.Domain.Services
{
    public class TokenExpirationProvider : ITokenExpirationProvider
    {
        public DateTime GetExpiration(DateTime now)
        {
            var expirationDays = Convert.ToInt32((string)(ConfigurationManager.AppSettings["PasswordExpirationDays"] ?? "15"));
            return now.AddDays(expirationDays);
        }
    }
}