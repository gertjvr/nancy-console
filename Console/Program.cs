using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Topshelf;

namespace Console
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Seq(ConfigurationManager.AppSettings["SeqServerUri"])
                .CreateLogger();
            
            var serviceName = Constants.WebAssembly.GetName().Name;
            var serviceDescription = "Nancy self hosted service.";
            
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += UnobservedTaskException;
            
            return (int) HostFactory.Run(c =>
            {
                c.UseSerilog();

                c.Service<WebService>();

                c.SetDescription(serviceDescription);
                c.SetServiceName(serviceName);
                c.SetDisplayName(serviceName);

                c.RunAsLocalSystem();
            });
        }
        
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var severity = e.IsTerminating ? LogEventLevel.Fatal : LogEventLevel.Error;
            var exception = e.ExceptionObject as Exception;

            if (Debugger.IsAttached) Debugger.Break();
            Log.Write(severity, exception, "An unhandled exception occurred: {Notification}",
                exception?.Message ?? string.Empty);
        }

        private static void UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (Debugger.IsAttached) Debugger.Break();
            Log.Error(e.Exception, "Unobserved task exception");
            e.SetObserved();
        }
    }
}