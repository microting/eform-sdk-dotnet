/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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
using System.Linq;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers;

public class EformRetrievedHandler(Core core) : IHandleMessages<EformRetrieved>
{
    public async Task Handle(EformRetrieved message)
    {
        try
        {
            await using var dbContext = core.DbContextHelper.GetDbContext();

            var match = await dbContext.Cases.FirstOrDefaultAsync(x => x.MicrotingUid == message.MicrotringUUID);

            if (match != null)
            {
                match.Status = 77;
                await match.Update(dbContext).ConfigureAwait(false);
            }

            var notification = await dbContext.Notifications.FirstAsync(x =>
                x.NotificationUid == message.notificationUId && x.MicrotingUid == message.MicrotringUUID);
            notification.WorkflowState = Constants.WorkflowStates.Processed;
            await notification.Update(dbContext).ConfigureAwait(false);

            if (dbContext.Cases.Count(x => x.MicrotingUid == message.MicrotringUUID) == 1)
            {
                var aCase = await dbContext.Cases.AsNoTracking()
                    .FirstAsync(x => x.MicrotingUid == message.MicrotringUUID);
                var cL = await dbContext.CheckLists.AsNoTracking().FirstAsync(x => x.Id == aCase.CheckListId);

                var stat = "";
                if (aCase.WorkflowState == Constants.WorkflowStates.Created && aCase.Status != 77)
                    stat = "Created";

                stat = aCase.WorkflowState switch
                {
                    Constants.WorkflowStates.Created when aCase.Status == 77 => "Retrived",
                    Constants.WorkflowStates.Retracted => "Completed",
                    Constants.WorkflowStates.Removed => "Deleted",
                    _ => stat
                };

                var remoteSiteId =
                    (int)dbContext.Sites.Where(x => x.Id == (int)aCase.SiteId).Select(x => x.MicrotingUid).First()!;
                var caseDto = new CaseDto
                {
                    CaseId = aCase.Id,
                    Stat = stat,
                    SiteUId = remoteSiteId,
                    CaseType = cL.CaseType,
                    CaseUId = aCase.CaseUid,
                    MicrotingUId = aCase.MicrotingUid,
                    CheckUId = aCase.MicrotingCheckUid,
                    Custom = aCase.Custom,
                    CheckListId = cL.Id,
                    WorkflowState = aCase.WorkflowState
                };
                var unit = await dbContext.Units.AsNoTracking().FirstOrDefaultAsync(x => x.MicrotingUid == aCase.UnitId);
                if (unit != null)
                {
                    await core.Advanced_UnitGet(unit.Id);
                }
                await core.FireHandleCaseRetrived(caseDto);
            }
            else
            {
                var cls = await dbContext.CheckListSites.AsNoTracking()
                    .FirstAsync(x => x.MicrotingUid == message.MicrotringUUID);
                var cL = await dbContext.CheckLists.AsNoTracking().FirstAsync(x => x.Id == cls.CheckListId);

                var stat = cls.WorkflowState switch
                {
                    Constants.WorkflowStates.Created => Constants.WorkflowStates.Created,
                    Constants.WorkflowStates.Removed => "Deleted",
                    _ => ""
                };

                //

                var remoteSiteId = (int) dbContext.Sites.AsNoTracking().First(x => x.Id == (int) cls.SiteId)
                    .MicrotingUid!;
                var cDto = new CaseDto
                {
                    CaseId = null,
                    Stat = stat,
                    SiteUId = remoteSiteId,
                    CaseType = cL.CaseType,
                    CaseUId = "ReversedCase",
                    MicrotingUId = cls.MicrotingUid,
                    CheckUId = cls.LastCheckId,
                    Custom = null,
                    CheckListId = cL.Id,
                    WorkflowState = null
                };
                var unit = await dbContext.Units.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.SiteId == (int) cls.SiteId);
                if (unit != null)
                {
                    await core.Advanced_UnitGet(unit.Id);
                }

                await core.FireHandleCaseRetrived(cDto);
            }
        }
        catch (Exception ex)
        {
            await using var dbContext = core.DbContextHelper.GetDbContext();

            var notification = await dbContext.Notifications.FirstAsync(x =>
                x.NotificationUid == message.notificationUId && x.MicrotingUid == message.MicrotringUUID);
            notification.WorkflowState = Constants.WorkflowStates.NotFound;
            notification.Exception = ex.Message;
            notification.Stacktrace = ex.StackTrace;
            await notification.Update(dbContext).ConfigureAwait(false);
            var noteDto = new NoteDto(message.notificationUId, message.MicrotringUUID,
                Constants.WorkflowStates.NotFound);
            await core.FireHandleNotificationNotFound(noteDto);
        }
    }
}