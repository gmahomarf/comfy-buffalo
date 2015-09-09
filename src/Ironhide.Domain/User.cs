using System;

namespace Starscream.Domain
{
    public class User : IEntity
    {
        protected User()
        {            
        }

        public User(string name, string emailAddress, EncryptedPassword encryptedPassword)
        {
            Name = name;
            Email = emailAddress;
            EncryptedPassword = encryptedPassword.Password;
        }

        public virtual string Name { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string EncryptedPassword { get; protected set; }

        #region IEntity Members

        public virtual Guid Id { get; protected set; }

        #endregion
    }
}