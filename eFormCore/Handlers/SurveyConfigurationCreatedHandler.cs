using System;
using System.Threading.Tasks;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers;

public class SurveyConfigurationCreatedHandler : IHandleMessages<SurveyConfigurationCreated>
{
    public Task Handle(SurveyConfigurationCreated message)
    {
        throw new NotImplementedException();
    }
}