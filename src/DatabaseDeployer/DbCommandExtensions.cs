using System.Data;
using System.IO;

namespace DatabaseDeployer
{
    public static class DbCommandExtensions
    {
        public static void ExecuteSqlFile(this IDbCommand cmd, string filename)
        {
            var tr = new StreamReader(filename);
            string sql = tr.ReadToEnd();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}