using System;
using System.Collections.Generic;
using Nancy;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public class RestExceptionConfiguration
    {
        public readonly List<IExceptionRepackager> ErrorHandlers = new List<IExceptionRepackager>();
        public IExceptionRepackager DefaultExceptionRepackager;
        public Action<Response> ResponseAction = response => { };

        public RestExceptionConfiguration WithResponse(Action<Response> action)
        {
            ResponseAction = action;
            return this;
        }

        public RestExceptionConfiguration Except<T>() where T : IExceptionRepackager
        {
            ErrorHandlers.RemoveAll(x => x.GetType() == typeof (T));
            return this;
        }

        public RestExceptionConfiguration WithRepackager(IExceptionRepackager handler)
        {
            ErrorHandlers.Add(handler);
            return this;
        }

        public RestExceptionConfiguration WithDefault(IExceptionRepackager handler)
        {
            DefaultExceptionRepackager = handler;
            return this;
        }
    }
}