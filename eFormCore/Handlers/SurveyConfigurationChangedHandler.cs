using System;
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers
{
    public class SurveyConfigurationChangedHandler : IHandleMessages<SurveyConfigurationChanged>
    {
        private readonly SqlController sqlController;
        private readonly Log log;
        private readonly Core core;

        public SurveyConfigurationChangedHandler(SqlController sqlController, Log log, Core core)
        {
            this.sqlController = sqlController;
            this.log = log;
            this.core = core;
        }

        public async Task Handle(SurveyConfigurationChanged message)
        {
            try
            {
                log.LogEverything("SurveyConfigurationChangedHandler.Handle",
                    "Calling GetAllQuestionSets to load all question sets");
                await core.GetAllQuestionSets().ConfigureAwait(false);
                await sqlController.NotificationUpdate(message.NotificationUId,
                    message.MicrotringUUID,
                    Constants.WorkflowStates.Processed,
                    "",
                    "").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogException("SurveyConfigurationChangedHandler.Handle", $"Got exception {ex.Message}", ex);
                await sqlController.NotificationUpdate(message.NotificationUId,
                    message.MicrotringUUID,
                    Constants.WorkflowStates.NotFound,
                    ex.Message,
                    ex.StackTrace).ConfigureAwait(false);
            }
        }
    }
}