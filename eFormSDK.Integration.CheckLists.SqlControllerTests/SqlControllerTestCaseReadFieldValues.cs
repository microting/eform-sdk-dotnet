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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using Case = Microting.eForm.Infrastructure.Data.Entities.Case;
using CheckListValue = Microting.eForm.Infrastructure.Models.CheckListValue;
using Field = Microting.eForm.Infrastructure.Data.Entities.Field;

namespace eFormSDK.Integration.CheckLists.SqlControllerTests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class SqlControllerTestCaseReadFieldValues : DbTestFixture
{
    private SqlController sut;
    private TestHelpers testHelpers;
    private Language language;

    public override async Task DoSetup()
    {
        DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
        SqlController sql = new SqlController(dbContextHelper);
        await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
        await sql.SettingUpdate(Settings.firstRunDone, "true");
        await sql.SettingUpdate(Settings.knownSitesDone, "true");

        sut = new SqlController(dbContextHelper);
        sut.StartLog(new CoreBase());
        testHelpers = new TestHelpers(ConnectionString);
        await testHelpers.GenerateDefaultLanguages();
        language = DbContext.Languages.Single(x => x.LanguageCode == "da");
    }

    [Test]
    public async Task CheckRead_ConcurrentReadsOfCaseWithMissingFieldValues_CreateExactlyOneFieldValuePerField()
    {
        // Arrange
        CheckList cl1 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "A", "D", "CheckList",
            "Template1FolderName", 1, 1);
        CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
        FieldType commentType = DbContext.FieldTypes.First(x => x.Type == "comment");

        List<Field> fields = new List<Field>();
        for (int i = 0; i < 3; i++)
        {
            fields.Add(await testHelpers.CreateField(0, "barcode", cl2, "e2f4fb", "custom", null, "",
                "Comment description", i, 0, commentType, 0, 0, 1, 0, $"Comment {i}", 0, 55, "55", "0", 0, 0,
                null, 0, 0, 0, 0, "", 1));
        }

        Worker worker = await testHelpers.CreateWorker("worker@example.com", "Worker", "A", 21);
        Site site = await testHelpers.CreateSite("Site A", 88);
        Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);
        Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
            worker, 101, 201, site, 66, "caseType", unit, DateTime.UtcNow, 1, worker,
            Constants.WorkflowStates.Created);
        await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

        // Act: the case has no FieldValues yet, so every reader tries to add them at the same time
        await Task.WhenAll(Enumerable.Range(0, 20)
            .Select(_ => Task.Run(() =>
                sut.CheckRead((int)aCase.MicrotingUid, (int)aCase.MicrotingCheckUid, language))));

        // Assert
        foreach (Field field in fields)
        {
            int liveRows = await DbContext.FieldValues.AsNoTracking().CountAsync(x =>
                x.CaseId == aCase.Id
                && x.FieldId == field.Id
                && x.WorkflowState != Constants.WorkflowStates.Removed);
            Assert.That(liveRows, Is.EqualTo(1), $"field {field.Id} has {liveRows} live FieldValues");
        }
    }

    [Test]
    public async Task CheckRead_FieldWithSeveralFieldValues_ReturnsThemOrderedById()
    {
        // Arrange
        CheckList cl1 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "A", "D", "CheckList",
            "Template1FolderName", 1, 1);
        CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
        FieldType commentType = DbContext.FieldTypes.First(x => x.Type == "comment");
        Field field = await testHelpers.CreateField(0, "barcode", cl2, "e2f4fb", "custom", null, "",
            "Comment description", 0, 0, commentType, 0, 0, 1, 0, "Comment", 0, 55, "55", "0", 0, 0,
            null, 0, 0, 0, 0, "", 1);

        Worker worker = await testHelpers.CreateWorker("worker@example.com", "Worker", "A", 21);
        Site site = await testHelpers.CreateSite("Site A", 88);
        Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);
        Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
            worker, 102, 202, site, 66, "caseType", unit, DateTime.UtcNow, 1, worker,
            Constants.WorkflowStates.Created);
        await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);
        await testHelpers.CreateFieldValue(aCase, cl2, field, null, null, "first", 1, worker);
        await testHelpers.CreateFieldValue(aCase, cl2, field, null, null, "second", 1, worker);

        // Act
        ReplyElement match = await sut.CheckRead((int)aCase.MicrotingUid, (int)aCase.MicrotingCheckUid, language);

        // Assert
        CheckListValue clv = (CheckListValue)match.ElementList[0];
        Microting.eForm.Infrastructure.Models.Field readField =
            (Microting.eForm.Infrastructure.Models.Field)clv.DataItemList[0];
        List<int> ids = readField.FieldValues.Select(x => x.Id).ToList();
        Assert.That(ids.Count, Is.EqualTo(2));
        Assert.That(ids, Is.Ordered.Ascending);
    }
}
