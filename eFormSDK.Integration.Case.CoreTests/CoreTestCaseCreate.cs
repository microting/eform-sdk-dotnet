using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;

namespace eFormSDK.Integration.Case.CoreTests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class CoreTestCaseCreate : DbTestFixture
{
    private Core sut;
    private TestHelpers testHelpers;
    private string path;

    public override async Task DoSetup()
    {
        if (sut == null)
        {
            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            await sut.StartSqlOnly(ConnectionString);
        }

        path = Assembly.GetExecutingAssembly().Location;
        path = Path.GetDirectoryName(path).Replace(@"file:", "");
        await sut.SetSdkSetting(Settings.fileLocationPicture,
            Path.Combine(path, "output", "dataFolder", "picture"));
        await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
        await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
        testHelpers = new TestHelpers(ConnectionString);
        await testHelpers.GenerateDefaultLanguages();
        //await sut.StartLog(new CoreBase());
    }

    [Test] //needs http mock done
    public async Task Core_Case_CaseCreate_CreatesCase()
    {
        // Arrange

        #region Template1

        DateTime c1_Ca = DateTime.Now;
        DateTime c1_Ua = DateTime.Now;

        CheckList cl1 =
            await testHelpers.CreateTemplate(c1_Ca, c1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

        #endregion

//            #region SubTemplate1
//            check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion

//            #region Fields
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//            #endregion

//            #region Worker
//
//            workers worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
//
//            #endregion

        #region site

        Site site = await testHelpers.CreateSite("SiteName", 88);

        #endregion


        CoreElement CElement = new CoreElement();
        //CElement.ElementList = new List<Element>();

        MainElement main = new MainElement(1, "label1", 1, "FolderWithList",
            1, DateTime.Now, DateTime.Now.AddDays(2),
            "Swahili", false, false, false, false,
            "Type1", "Push", "TextForBody", false,
            CElement.ElementList, "Blue");
        // Act
        var match = await sut.CaseCreate(main, "", (int)site.MicrotingUid, null);
        // Assert
        Assert.That(match, Is.Not.EqualTo(null));
    }


    [Test]
    public async Task Core_Case_CaseCreateLocalOnly_AcceptsEndOfTodayUtcEndDate()
    {
        // Regression for the .ToLongDateString() time-truncation bug: an EndDate at the very
        // end of today UTC must NOT be rejected as "needs to be a future date".

        // Arrange
        CheckList cl1 = await testHelpers.CreateTemplate(
            DateTime.Now, DateTime.Now, "A", "D", "CheckList", "Folder", 1, 1);
        Site site = await testHelpers.CreateSite("SiteName", 88);

        DateTime startUtc = DateTime.UtcNow;
        DateTime endUtc = DateTime.UtcNow.AddDays(1).AddTicks(-1); // end-of-today + 24h - 1tick

        MainElement main = new MainElement(cl1.Id, "label1", 1, "FolderWithList",
            1, startUtc, endUtc,
            "Swahili", false, false, false, false,
            "Type1", "Push", "TextForBody", false,
            new CoreElement().ElementList, "Blue");

        // Act
        var caseId = await sut.CaseCreateLocalOnly(main, "", (int)site.MicrotingUid, null);

        // Assert
        Assert.That(caseId, Is.Not.Null, "CaseCreateLocalOnly should accept end-of-today UTC as EndDate");
        Assert.That(caseId.Value, Is.GreaterThan(0));
    }

    [Test]
    public async Task Core_Case_CaseCreateLocalOnly_InsertsRowWithSyntheticMicrotingUid()
    {
        CheckList cl1 = await testHelpers.CreateTemplate(
            DateTime.Now, DateTime.Now, "A", "D", "CheckList", "Folder", 1, 1);
        Site site = await testHelpers.CreateSite("SiteName", 88);

        MainElement main = new MainElement(cl1.Id, "label1", 1, "FolderWithList",
            1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2),
            "Swahili", false, false, false, false,
            "Type1", "Push", "TextForBody", false,
            new CoreElement().ElementList, "Blue");

        var caseId = await sut.CaseCreateLocalOnly(main, "", (int)site.MicrotingUid, null);

        Assert.That(caseId, Is.Not.Null);
        var row = await DbContext.Cases.FirstOrDefaultAsync(x => x.Id == caseId.Value);
        Assert.That(row, Is.Not.Null, "the Cases row should exist");
        Assert.That(row!.MicrotingUid, Is.Not.Null, "synthetic MicrotingUid should be assigned");
        Assert.That(row.MicrotingUid!.Value, Is.GreaterThanOrEqualTo(2_000_000_000),
            "synthetic MicrotingUids live in the reserved local range");
        Assert.That(row.Id, Is.GreaterThan(0));
    }

    [Test]
    public async Task Core_Case_CaseCreateLocalOnly_RejectsPastEndDate()
    {
        CheckList cl1 = await testHelpers.CreateTemplate(
            DateTime.Now, DateTime.Now, "A", "D", "CheckList", "Folder", 1, 1);
        Site site = await testHelpers.CreateSite("SiteName", 88);

        MainElement main = new MainElement(cl1.Id, "label1", 1, "FolderWithList",
            1, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1),
            "Swahili", false, false, false, false,
            "Type1", "Push", "TextForBody", false,
            new CoreElement().ElementList, "Blue");

        int casesBefore = await DbContext.Cases.CountAsync();

        var caseId = await sut.CaseCreateLocalOnly(main, "", (int)site.MicrotingUid, null);

        Assert.That(caseId, Is.Null, "past EndDate should be rejected");
        int casesAfter = await DbContext.Cases.CountAsync();
        Assert.That(casesAfter, Is.EqualTo(casesBefore), "no Cases row should have been inserted");
    }

    [Test]
    public async Task Core_Case_CaseCreateLocalOnly_AcceptsUnspecifiedKindEndDate()
    {
        // Locks AsUtc("treat Unspecified as UTC") semantics: SDK callers commonly pass
        // Unspecified-Kind DateTimes they mean as UTC; converting via local offset shifts
        // end-of-today back to yesterday and re-introduces the original validation bug.
        CheckList cl1 = await testHelpers.CreateTemplate(
            DateTime.Now, DateTime.Now, "A", "D", "CheckList", "Folder", 1, 1);
        Site site = await testHelpers.CreateSite("SiteName", 88);

        DateTime startUnspec = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        DateTime endUnspec = DateTime.SpecifyKind(
            DateTime.UtcNow.AddDays(1).AddTicks(-1), DateTimeKind.Unspecified);

        MainElement main = new MainElement(cl1.Id, "label1", 1, "FolderWithList",
            1, startUnspec, endUnspec,
            "Swahili", false, false, false, false,
            "Type1", "Push", "TextForBody", false,
            new CoreElement().ElementList, "Blue");

        var caseId = await sut.CaseCreateLocalOnly(main, "", (int)site.MicrotingUid, null);

        Assert.That(caseId, Is.Not.Null,
            "Unspecified-Kind end-of-today must be treated as UTC and accepted");
        Assert.That(caseId.Value, Is.GreaterThan(0));
    }

    [Test]
    public async Task Core_Case_CaseCreate_DoesNotRejectEndOfTodayUtcEndDate()
    {
        // Regression on the public CaseCreate path for the same .ToLongDateString() bug:
        // CaseCreate swallows ArgumentException and returns null, so a non-null match
        // here proves end-of-today UTC EndDate passes validation.
        CheckList cl1 = await testHelpers.CreateTemplate(
            DateTime.Now, DateTime.Now, "A", "D", "CheckList", "Folder", 1, 1);
        Site site = await testHelpers.CreateSite("SiteName", 88);

        DateTime startUtc = DateTime.UtcNow;
        DateTime endUtc = DateTime.UtcNow.AddDays(1).AddTicks(-1);

        MainElement main = new MainElement(cl1.Id, "label1", 1, "FolderWithList",
            1, startUtc, endUtc,
            "Swahili", false, false, false, false,
            "Type1", "Push", "TextForBody", false,
            new CoreElement().ElementList, "Blue");

        var match = await sut.CaseCreate(main, "", (int)site.MicrotingUid, null);

        Assert.That(match, Is.Not.EqualTo(null));
    }

    #region eventhandlers

    public void EventCaseCreated(object sender, EventArgs args)
    {
        // Does nothing for web implementation
    }

    public void EventCaseRetrived(object sender, EventArgs args)
    {
        // Does nothing for web implementation
    }

    public void EventCaseCompleted(object sender, EventArgs args)
    {
        // Does nothing for web implementation
    }

    public void EventCaseDeleted(object sender, EventArgs args)
    {
        // Does nothing for web implementation
    }

    public void EventFileDownloaded(object sender, EventArgs args)
    {
        // Does nothing for web implementation
    }

    public void EventSiteActivated(object sender, EventArgs args)
    {
        // Does nothing for web implementation
    }

    #endregion
}