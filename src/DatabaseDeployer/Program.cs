using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AcklenAvenue.Data.NHibernate;
using DomainDrivenDatabaseDeployer;
using FluentNHibernate.Cfg.Db;
using Ironhide.Data;
using NHibernate;

namespace DatabaseDeployer
{
    class Program
    {
        static void Main(string[] args)
        {
            args = args.Select(x => x.ToLower()).ToArray();
            bool noArgs = !args.Any();

            ConnectionStringSettings connectionStringSettings = ConnectionStrings.Get();
            MsSqlConfiguration databaseConfiguration =
                MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(
                    x => x.Is(connectionStringSettings.ConnectionString))
                    .Dialect<MsSqlAzureDialect>();

            CreateDatabaseIfNotExists(connectionStringSettings);

            DomainDrivenDatabaseDeployer.DatabaseDeployer dd = null;
            var sessionFactory = new SessionFactoryBuilder(new MappingScheme(), databaseConfiguration, new EntityInterceptor())
                .Build(cfg => { dd = new DomainDrivenDatabaseDeployer.DatabaseDeployer(cfg); });

            if (noArgs || args.Contains("drop"))
            {
                using (ISession sess = sessionFactory.OpenSession())
                {
                    using (IDbCommand cmd = sess.Connection.CreateCommand())
                    {
                        cmd.ExecuteSqlFile("dropForeignKeys.sql");
                        //cmd.ExecuteSqlFile("dropPrimaryKeys.sql");
                        cmd.ExecuteSqlFile("dropTables.sql");
                    }
                }
                dd.Drop();
                Console.WriteLine("");
                Console.WriteLine("Database dropped.");
            }

            if (noArgs || args.Contains("create"))
            {
                dd.Create();
                Console.WriteLine("");
                Console.WriteLine("Database created.");
            }
            else if (args.Contains("update"))
            {
                dd.Update();
                Console.WriteLine("");
                Console.WriteLine("Database updated.");
            }

            if (noArgs || args.Contains("seed"))
            {
                ISession session = sessionFactory.OpenSession();
                using (ITransaction tx = session.BeginTransaction())
                {
                    dd.Seed(new List<IDataSeeder>
                                {
                                    //add data seeders here.
                                    new UserSeeder(session)
                                });
                    tx.Commit();
                }
                session.Close();
                sessionFactory.Close();
                Console.WriteLine("");
                Console.WriteLine("Seed data added.");
            }

            Console.WriteLine("Done");
        }

        static void CreateDatabaseIfNotExists(ConnectionStringSettings connectionStringSettings)
        {
            Dictionary<string, string> settings = GetSettingsFromConnectionString(connectionStringSettings);


            string databaseName = settings.ContainsKey("initial catalog")
                                      ? settings["initial catalog"]
                                      : settings["database"];


            using (var myConn = new SqlConnection(MasterConnectionString(settings)))
            {
                myConn.Open();

                if (!DatabaseExists(databaseName, myConn))
                {
                    CreateDatabase(databaseName, myConn);
                }
            }
        }

        static Dictionary<string, string> GetSettingsFromConnectionString(
            ConnectionStringSettings connectionStringSettings)
        {
            string[] settingsRaw = connectionStringSettings.ConnectionString.Split(';');
            var settings = new Dictionary<string, string>();
            settingsRaw.ToList().ForEach(x =>
                                             {
                                                 string[] parts = x.Split('=');
                                                 if (!parts.Any() || parts.All(string.IsNullOrEmpty)) return;
                                                 settings.Add(parts[0].ToLower(), parts[1]);
                                             });
            return settings;
        }

        static string MasterConnectionString(Dictionary<string, string> settings)
        {
            List<KeyValuePair<string, string>> masterSettings =
                settings.Where(x => x.Key != "database" && x.Key != "initial catalog").ToList();
            masterSettings.Add(new KeyValuePair<string, string>("database", "master"));
            string masterConnectionString = string.Join(";",
                                                        masterSettings.Select(x => string.Join("=", x.Key, x.Value)));
            return masterConnectionString;
        }

        static void CreateDatabase(string databaseName, SqlConnection openConnection)
        {
            string str = "CREATE DATABASE " + databaseName;
            using (var myCommand = new SqlCommand(str, openConnection))
            {
                try
                {
                    myCommand.ExecuteNonQuery();
                    Console.WriteLine("DataBase is Created Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        static bool DatabaseExists(string databaseName, SqlConnection openConnection)
        {
            string queryToCheckIfExists = string.Format("select * from master.dbo.sysdatabases where name=\'{0}\'",
                                                        databaseName);
            using (var sqlCmd = new SqlCommand(queryToCheckIfExists, openConnection))
            {
                object databasesMatchingThatName = sqlCmd.ExecuteScalar();
                return databasesMatchingThatName != null;
            }
        }
    }
}