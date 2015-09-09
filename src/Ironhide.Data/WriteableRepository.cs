using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Services;
using NHibernate;

namespace Ironhide.Data
{
    public class WriteableRepository : IWriteableRepository
    {
        readonly ISession _session;

        public WriteableRepository(ISession session)
        {
            _session = session;
        }

        public async Task<T> Create<T>(T itemToCreate) where T : IEntity
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    _session.Save(itemToCreate);
                    return itemToCreate;
                });
        }

        public async Task DeleteAll<T>() where T : class, IEntity
        {
            foreach (T item in _session.QueryOver<T>().List())
            {
                await Delete<T>(item.Id);
            }
        }

        public async Task Delete<T>(Guid itemId) where T : IEntity
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var itemToDelete = _session.Get<T>(itemId);
                    _session.Delete(itemToDelete);
                });
        }

        public async Task<IEnumerable<T>> CreateAll<T>(IEnumerable<T> list) where T : IEntity
        {
            List<T> items = list as List<T> ?? list.ToList();
            foreach (T item in items)
            {
                await Create(item);
            }

            return items;
        }

        public async Task<T> Update<T>(T itemToUpdate) where T : IEntity
        {
            return await Task.Factory.StartNew(
                () =>
                {
                    ISession session = _session;
                    session.Update(itemToUpdate);
                    return itemToUpdate;
                });
        }
    }
}