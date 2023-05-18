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
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers
{
    public class UnitActivatedHandler : IHandleMessages<UnitActivated>
    {
        private readonly SqlController _sqlController;
        private readonly Log _log;
        private readonly Core _core;

        public UnitActivatedHandler(SqlController sqlController, Log log, Core core)
        {
            _sqlController = sqlController;
            _log = log;
            _core = core;
        }

#pragma warning disable 1998
        public async Task Handle(UnitActivated message)
        {
            try
            {
                UnitDto unitDto = await _sqlController.UnitRead(message.MicrotringUUID);
                await _sqlController.UnitUpdate(unitDto.UnitUId, unitDto.CustomerNo, 0, unitDto.SiteUId);
                await _sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.Processed, "", "");

                _log.LogStandard("UnitActivatedHandler.Handle",
                    "Unit with id " + message.MicrotringUUID + " has been activated");

                NoteDto noteDto = new NoteDto(message.notificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.Processed);
                await _core.FireHandleSiteActivated(noteDto);
            }
            catch (Exception ex)
            {
                await _sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.NotFound, ex.Message, ex.StackTrace);
                NoteDto noteDto = new NoteDto(message.notificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.NotFound);
                await _core.FireHandleNotificationNotFound(noteDto);
            }
        }
    }
}