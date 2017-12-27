using eForm.Messages;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;

namespace eFormCore.Handlers
{
    public class EformRetrievedHandler : IHandleMessages<EformRetrieved>
    {
        public async Task Handle(EformRetrieved message)
        {
            // Write to database

            // Potentially send new message onto local queue
        }
    }
}
