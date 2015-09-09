using System;

namespace Ironhide.Users.Domain
{
    public abstract class Entity : IEntity
    {
        private bool _saved = false;

        public virtual void OnSave()
        {
            _saved = true;
        }

        public virtual void OnLoad()
        {
            _saved = true;
        }

        public virtual bool IsPersisted()
        {
            return _saved;
        }

        public virtual Guid Id { get; protected set; }
    }
}