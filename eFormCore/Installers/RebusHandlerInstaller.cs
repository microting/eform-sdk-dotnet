using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using eForm.Messages;
using eFormCore.Handlers;
using Rebus.Handlers;
using System.Reflection;

namespace eFormCore.Installers
{
    public class RebusHandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Component.For<IHandleMessages<EformCompleted>>().ImplementedBy<EformCompletedHandler>().LifestyleTransient());
            container.Register(Component.For<IHandleMessages<EformDeleteFromServer>>().ImplementedBy<EformDeleteFromServerHandler>().LifestyleTransient());
            container.Register(Component.For<IHandleMessages<EformParsedByServer>>().ImplementedBy<EformParsedByServerHandler>().LifestyleTransient());
            container.Register(Component.For<IHandleMessages<EformParsingError>>().ImplementedBy<EformParsingErrorHandler>().LifestyleTransient());
            container.Register(Component.For<IHandleMessages<EformRetrieved>>().ImplementedBy<EformRetrievedHandler>().LifestyleTransient());
            container.Register(Component.For<IHandleMessages<TranscribeAudioFile>>().ImplementedBy<TranscribeAudioFileHandler>().LifestyleTransient());
            container.Register(Component.For<IHandleMessages<TranscriptionCompleted>>().ImplementedBy<TranscriptionCompletedHandler>().LifestyleTransient());
            container.Register(Component.For<IHandleMessages<UnitActivated>>().ImplementedBy<UnitActivatedHandler>().LifestyleTransient());
        }
    }
}
