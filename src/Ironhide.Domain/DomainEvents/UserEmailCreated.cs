using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserEmailCreated:UserCreated
    {
       
        public string PhoneNumber { get; private set; }

        public UserEmailCreated(Guid userId, string email, string name, string phoneNumber):base(userId,email,name)
        {
           
            PhoneNumber = phoneNumber;
        }
    }
}