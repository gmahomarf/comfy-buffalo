using System.Threading.Tasks;
using AcklenAvenue.Commands;
using NHibernate;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class UnitOfWorkCommandDispatcher : ICommandDispatcher
    {
        readonly ICommandDispatcher _decoratedCommandDispatcher;
        readonly ISession _session;

        public UnitOfWorkCommandDispatcher(ICommandDispatcher decoratedCommandDispatcher, ISession session)
        {
            _decoratedCommandDispatcher = decoratedCommandDispatcher;
            _session = session;
        }

        public async Task Dispatch(IUserSession userSession, object command)
        {
            using (var tx = _session.BeginTransaction())
            {
                await _decoratedCommandDispatcher.Dispatch(userSession, command);
                tx.Commit();
            }
        }
    }
}