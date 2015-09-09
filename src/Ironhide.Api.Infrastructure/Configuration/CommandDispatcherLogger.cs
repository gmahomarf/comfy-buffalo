using System;
using AcklenAvenue.Commands;
using log4net;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class CommandDispatcherLogger : ICommandDispatcherLogger
    {
        readonly ILog _logger;

        public CommandDispatcherLogger(ILog logger)
        {
            _logger = logger;
        }

        public void LogInfo(IUserSession userSession, object sender, DateTime timeStamp, string message)
        {
            _logger.InfoFormat("{0}: {1}", sender.GetType().Name, message);
        }

        public void LogException(IUserSession userSession, object sender, DateTime timeStamp, Exception exception)
        {
            string errorMessage = "1) Error handling command with handler '" + sender.GetType() + "'\n";
            errorMessage += "2) " + exception.Message;
            _logger.Error(errorMessage);
        }
    }
}