using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;

namespace Ironhide.Api.Host
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] =
                _ =>
                {
                    IEnumerable<string> modules =
                        AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(x => x.GetTypes())
                            .Where(x => typeof (NancyModule).IsAssignableFrom(x))
                            .Where(x => x.Namespace != null && !x.Namespace.StartsWith("Nancy"))
                            .Where(x => x != GetType())
                            .Select(x => string.Format("<li>{0}</li>", x.Name))
                            .ToList();

                    if (!modules.Any())
                    {
                        modules = new List<string> {"None"};
                    }

                    Version version = GetType().Assembly.GetName().Version;

                    return string.Format("<p>Ironhide API v{0}</p><p>Modules: <ul>{1}</ul></p>",
                        version, string.Join("", modules));
                };
        }
    }
}