using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ironhide.Users.Domain.Services
{
    public interface IReadOnlyRepository
    {
        Task<T> First<T>(Expression<Func<T, bool>> query) where T : class, IEntity;
        Task<T> FirstOrDefault<T>(Expression<Func<T, bool>> query) where T : class, IEntity;
        Task<T> GetById<T>(Guid id) where T : IEntity;
        Task<IEnumerable<T>> GetAll<T>() where T : IEntity;
        Task<IEnumerable<T>> Query<T>(Expression<Func<T, bool>> expression) where T : IEntity;
    }
}