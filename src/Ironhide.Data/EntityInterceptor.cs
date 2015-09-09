using System.Diagnostics;
using Ironhide.Users.Domain;
using NHibernate;
using NHibernate.Type;

namespace Ironhide.Data
{
    public class EntityInterceptor : EmptyInterceptor
    {
        public override bool? IsTransient(object entity)
        {
            if (entity is Entity)
            {
                return !((Entity)entity).IsPersisted();
            }
            else
            {
                return null;
            }
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (entity is Entity) ((Entity)entity).OnLoad();
            return false;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (entity is Entity) ((Entity)entity).OnSave();
            return false;
        }

        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            Trace.WriteLine(sql.ToString());
            return sql;
        }
    }
}