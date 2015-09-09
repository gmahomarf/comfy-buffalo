using System;

namespace Ironhide.Users.Domain.Exceptions
{
    public class ItemNotFoundException<T> : Exception
    {
        public ItemNotFoundException(Guid key)
            : base("The '" + typeof(T).Name + "' item was not found for the key '" + key + "'.")
        {
        }

        public ItemNotFoundException()
            : base("No '" + typeof(T).Name + "' items were found.")
        {
        }
    }
}