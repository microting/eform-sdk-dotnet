using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Integration.Case.CoreTests;

/// <summary>
/// Simulates a fresh install on a MariaDB Galera cluster (auto_increment_increment=3), where the runtime
/// seeder hands out FieldTypes ids 3,6,9,...,60 (Text=3, None=9, SaveButton=54) and Languages ids 3,6,...
/// (Dansk=3, English=6). Code must resolve lookups by FieldType.Type / Language.LanguageCode, never by id.
/// See microting/eform-angular-frontend#8141.
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class CoreTestShiftedLookupIds : DbTestFixture
{
    private Core sut;
    private TestHelpers testHelpers;
    private Language language;

    public override async Task DoSetup()
    {
        // The SQL dump re-seeds FieldTypes/Languages with contiguous ids on every Setup, so shift them each time,
        // before anything reads them. Descending order avoids primary-key collisions while multiplying.
        await DbContext.Database.ExecuteSqlRawAsync(
            "SET FOREIGN_KEY_CHECKS = 0;" +
            "UPDATE `FieldTypes` SET `Id` = `Id` * 3 ORDER BY `Id` DESC;" +
            "UPDATE `Languages` SET `Id` = `Id` * 3 ORDER BY `Id` DESC;" +
            "UPDATE `LanguageVersions` SET `LanguageId` = `LanguageId` * 3;" +
            "SET FOREIGN_KEY_CHECKS = 1;");

        if (sut == null)
        {
            sut = new Core();
            await sut.StartSqlOnly(ConnectionString);
        }

        testHelpers = new TestHelpers(ConnectionString);
        await testHelpers.GenerateDefaultLanguages();
        language = DbContext.Languages.AsNoTracking().Single(x => x.LanguageCode == "da");
    }

    [Test]
    public void ShiftedFixture_HasGaleraStyleIds()
    {
        Assert.That(FieldType(Constants.FieldTypes.Text).Id, Is.EqualTo(3));
        Assert.That(FieldType(Constants.FieldTypes.None).Id, Is.EqualTo(9));
        Assert.That(FieldType(Constants.FieldTypes.SaveButton).Id, Is.EqualTo(54));
        Assert.That(language.Id, Is.EqualTo(3));
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task GenerateDataSetFromCases_ShiftedIds_IncludesTextAndNumberFields_ExcludesNoneAndSaveButton(
        bool includeCheckListText)
    {
        CheckList checkList = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "Template",
            "Description", "", "", 1, 1);
        Field textField = await CreateField(checkList, Constants.FieldTypes.Text, "TextLabel", 1);
        await CreateField(checkList, Constants.FieldTypes.None, "NoneLabel", 2);
        Field numberField = await CreateField(checkList, Constants.FieldTypes.Number, "NumberLabel", 3);
        await CreateField(checkList, Constants.FieldTypes.SaveButton, "SaveButtonLabel", 4);

        Worker worker = await testHelpers.CreateWorker("first@example.com", "Arne", "Jensen", 21);
        Site site = await testHelpers.CreateSite("SiteName", 88);
        Unit unit = await testHelpers.CreateUnit(20, 30, site, 348);
        Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", checkList,
            DateTime.UtcNow, "custom", DateTime.UtcNow, worker, 1, 1, site, 66, "caseType", unit,
            DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);
        await testHelpers.CreateFieldValue(aCase, checkList, textField, null, null, "hello", 1, worker);
        await testHelpers.CreateFieldValue(aCase, checkList, numberField, null, null, "42", 1, worker);

        List<List<string>> dataSet = await sut.GenerateDataSetFromCases(checkList.Id, null, null, "", ",", ".",
            true, CultureInfo.InvariantCulture, TimeZoneInfo.Utc, language, includeCheckListText, false);

        string expectedPrefix = includeCheckListText ? "Template / " : "";
        List<string> headers = dataSet.Select(row => row[0]).ToList();
        string fieldHeaders = string.Join(", ", headers);
        Assert.Multiple(() =>
        {
            Assert.That(headers, Does.Contain(expectedPrefix + "TextLabel"), fieldHeaders);
            Assert.That(headers, Does.Contain(expectedPrefix + "NumberLabel"), fieldHeaders);
            Assert.That(headers.Any(h => h.Contains("NoneLabel")), Is.False, fieldHeaders);
            Assert.That(headers.Any(h => h.Contains("SaveButtonLabel")), Is.False, fieldHeaders);
        });

        List<string> textRow = dataSet.Single(row => row[0] == expectedPrefix + "TextLabel");
        Assert.That(textRow[1], Is.EqualTo("hello"));
    }

    private FieldType FieldType(string type) =>
        DbContext.FieldTypes.AsNoTracking().Single(x => x.Type == type);

    private Task<Field> CreateField(CheckList checkList, string type, string label, int displayIndex) =>
        testHelpers.CreateField(1, "", checkList, "e2f4fb", "", null, "", "", displayIndex, 0, FieldType(type),
            0, 0, 0, 0, label, 0, 255, "", "", 0, 0, "", 0, 0, 0, 0, "", 1);
}
