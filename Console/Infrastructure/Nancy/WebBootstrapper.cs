using Autofac;
using Nancy.Bootstrappers.Autofac;

namespace Console.Infrastructure.Nancy
{
    public class WebBootstrapper : AutofacNancyBootstrapper
    {
        private readonly ILifetimeScope _lifetimeScope;

        public WebBootstrapper(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            return _lifetimeScope ?? base.GetApplicationContainer();
        }
    }
}