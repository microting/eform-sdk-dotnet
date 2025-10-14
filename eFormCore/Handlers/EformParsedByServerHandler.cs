﻿/*
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

using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers;

public class EformParsedByServerHandler : IHandleMessages<EformParsedByServer>
{
    private readonly SqlController _sqlController;
    private readonly Core _core;
    private readonly Log _log;

    public EformParsedByServerHandler(SqlController sqlController, Core core, Log log)
    {
        _sqlController = sqlController;
        _core = core;
        _log = log;
    }

    public async Task Handle(EformParsedByServer message)
    {
        _log.LogStandard("EformParsedByServer.Handle called", $"NotificationId: {message.NotificationId}, MicrotringUUID: {message.MicrotringUUID}");
        await _sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID,
            Constants.Notifications.EformParsedByServer);

        CaseDto cDto = await _sqlController.CaseReadByMUId(message.MicrotringUUID);
        await _core.FireHandleCaseProcessedByServer(cDto);
        // Potentially send new message onto local queue
    }
}