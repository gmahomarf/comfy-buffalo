using System;
using Ironhide.Users.Domain.ValueObjects;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class ResetPassword
    {
        public Guid ResetPasswordToken { get; private set; }
        public EncryptedPassword EncryptedPassword { get; private set; }

        public ResetPassword(Guid resetPasswordToken, EncryptedPassword encryptedPassword)
        {
            ResetPasswordToken = resetPasswordToken;
            EncryptedPassword = encryptedPassword;
        }
    }
}