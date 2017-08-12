using Autofac;
using AutofacSerilogIntegration;

namespace Console.Infrastructure.Logging
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterLogger();
        }
    }
}