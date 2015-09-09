using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Ironhide.Data
{
    public class ConnectionStrings
    {
        public static ConnectionStringSettings Get()
        {
            string environment = GetEnvironment();
            List<ConnectionStringSettings> connectionStringSettings = GetConnectionStringSettings();
            ConnectionStringSettings setting = GetMatchingConnectionStringSetting(connectionStringSettings, environment);
            return setting;
        }

        static ConnectionStringSettings GetMatchingConnectionStringSetting(
            List<ConnectionStringSettings> connectionStringSettings, string environment)
        {
            ConnectionStringSettings match = connectionStringSettings
                .FirstOrDefault(x => x.Name.ToLower() == environment.ToLower());

            if (match == null)
            {
                throw new Exception(
                    string.Format(
                        "Connection string for '{0}' not found in the config file. Available connection strings are: {1}",
                        environment, string.Join(", ", connectionStringSettings.Select(x => x.Name))));
            }
            return match;
        }

        static string GetEnvironment()
        {
            string environment =
                (Environment.GetEnvironmentVariable("Environment")
                 ?? ConfigurationManager.AppSettings["Environment"]
                 ?? "local").ToLower();

            if (environment == "remote") environment = "qa";

            return environment;
        }

        static List<ConnectionStringSettings> GetConnectionStringSettings()
        {
            IEnumerable<ConnectionStringSettings> connectionStringSettings =
                ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>();

            if (connectionStringSettings == null)
            {
                throw new Exception("No connection strings were found in the config file.");
            }
            return connectionStringSettings.ToList();
        }
    }
}