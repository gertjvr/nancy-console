using System;
using System.Configuration;
using Autofac;
using Console.Infrastructure.Nancy;
using Microsoft.Owin.Hosting;
using Nancy;
using Nancy.Routing;
using Owin;
using Serilog;
using Topshelf;

namespace Console
{
    internal class WebService : ServiceControl
    {
        private IContainer _container;
        private IDisposable _webService;

        public bool Start(HostControl hostControl)
        {
            _container = IoC.LetThereBeIoC(assemblies: new[]
            {
                Constants.WebAssembly
            });

            var webServiceUri = ConfigurationManager.AppSettings["WebServiceUri"];

            _webService = WebApp.Start(webServiceUri, app =>
            {
                Log.Information("Listing on {WebServiceUri}", webServiceUri);
                app.UseNancy(options => 
                    options.Bootstrapper = new WebBootstrapper(_container));
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            using (_container)
            using (_webService)
            {
            }

            return true;
        }
    }
}