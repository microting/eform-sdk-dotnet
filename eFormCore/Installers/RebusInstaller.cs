/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus.Config;
using Rebus.Logging;

namespace Microting.eForm.Installers
{
    public class RebusInstaller : IWindsorInstaller
    {
        private readonly int _maxParallelism;
        private readonly int _numberOfWorkers;
        private readonly string _rabbitMqUser;
        private readonly string _rabbitMqPassword;
        private readonly string _rabbitMqHost;
        private readonly string _customerNo;

        public RebusInstaller(string customerNo, string connectionString, int maxParallelism, int numberOfWorkers,
            string rabbitMqUser, string rabbitMqPassword, string rabbitMqHost)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            _maxParallelism = maxParallelism;
            _numberOfWorkers = numberOfWorkers;
            _rabbitMqUser = rabbitMqUser;
            _rabbitMqPassword = rabbitMqPassword;
            _rabbitMqHost = rabbitMqHost;
            _customerNo = customerNo;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Configure.With(new CastleWindsorContainerAdapter(container))
                .Logging(l => l.ColoredConsole(LogLevel.Info))
                .Transport(t => t.UseRabbitMq($"amqp://{_rabbitMqUser}:{_rabbitMqPassword}@{_rabbitMqHost}",
                    $"{_customerNo}-eformsdk-input"))
                .Options(o =>
                {
                    o.SetMaxParallelism(_maxParallelism);
                    o.SetNumberOfWorkers(_numberOfWorkers);
                })
                .Start();
        }
    }
}