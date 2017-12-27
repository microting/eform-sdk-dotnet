using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus.Config;

namespace eFormCore.Installers
{
    public class RebusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Configure.With(new CastleWindsorContainerAdapter(container))
                .Logging(l => l.ColoredConsole())
                .Transport(t => t.UseSqlServer(connectionStringOrConnectionStringName: "", tableName: "Rebus", inputQueueName: "eformsdk-input"))
                .Options(o =>
                {
                    o.SetMaxParallelism(1);
                    o.SetNumberOfWorkers(1);
                })
                .Start();
        }
    }
}
