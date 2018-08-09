using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus.Handlers;
using System.Reflection;

namespace eFormCore.Installers
{
    public class RebusHandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Classes.FromThisAssembly
            //    .BasedOn(typeof(IHandleMessages<>))
            //    .WithServiceBase()
            //    .LifestyleTransient());

            container.Register(Classes.From()
                .BasedOn(typeof(IHandleMessages<>))
                .WithServiceBase()
                .LifestyleTransient());
        }
    }
}
