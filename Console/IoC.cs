using System;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Serilog;

namespace Console
{
    public static class IoC
    {
        public static IContainer LetThereBeIoC(ContainerBuildOptions containerBuildOptions = ContainerBuildOptions.None, params Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(assemblies);

            try
            {
                var sw = Stopwatch.StartNew();
                var container = builder.Build(containerBuildOptions);
                sw.Stop();
                Log.Information("Container built in {Elapsed}", sw.Elapsed);
                return container;
            }
            catch (Exception exc)
            {
                Log.Fatal(exc, "Container failed to build: {Message}", exc.Message);
            }

            return null;
        }
    }
}