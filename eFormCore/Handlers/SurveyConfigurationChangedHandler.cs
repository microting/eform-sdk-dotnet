using System.Threading.Tasks;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers
{
    public class SurveyConfigurationChangedHandler : IHandleMessages<SurveyConfigurationChanged>
    {
        public Task Handle(SurveyConfigurationChanged message)
        {
            throw new System.NotImplementedException();
        }
    }
}