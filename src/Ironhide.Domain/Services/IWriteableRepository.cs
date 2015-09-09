using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ironhide.Users.Domain.Services
{
    public interface IWriteableRepository
    {
        Task Delete<T>(Guid itemId) where T : IEntity;
        Task<T> Update<T>(T itemToUpdate) where T : IEntity;
        Task<T> Create<T>(T itemToCreate) where T : IEntity;
        Task DeleteAll<T>() where T : class, IEntity;
        Task<IEnumerable<T>> CreateAll<T>(IEnumerable<T> list) where T : IEntity;
    }
}