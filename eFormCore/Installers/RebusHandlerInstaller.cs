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

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microting.eForm.Handlers;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Installers
{
    public class RebusHandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHandleMessages<EformCompleted>>().ImplementedBy<EformCompletedHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<EformDeleteFromServer>>().ImplementedBy<EformDeleteFromServerHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<EformParsedByServer>>().ImplementedBy<EformParsedByServerHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<EformParsingError>>().ImplementedBy<EformParsingErrorHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<EformRetrieved>>().ImplementedBy<EformRetrievedHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<TranscribeAudioFile>>().ImplementedBy<TranscribeAudioFileHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<TranscriptionCompleted>>().ImplementedBy<TranscriptionCompletedHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<UnitActivated>>().ImplementedBy<UnitActivatedHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<AnswerCompleted>>().ImplementedBy<AnswerCompletedHandler>()
                    .LifestyleTransient(),
                Component.For<IHandleMessages<SurveyConfigurationCreated>>()
                    .ImplementedBy<SurveyConfigurationCreatedHandler>().LifestyleTransient(),
                Component.For<IHandleMessages<SurveyConfigurationChanged>>()
                    .ImplementedBy<SurveyConfigurationChangedHandler>().LifestyleTransient());
        }
    }
}