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
using System.Collections.Generic;
using System.Threading.Tasks;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;

namespace eFormSDK.Base.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class MainElementUTest
{
    #region CoreElement Tests

    [Test]
    public async Task CoreElement_DefaultConstructor_CreatesEmptyElementList()
    {
        await Task.Run(() =>
        {
            // Act
            var coreElement = new CoreElement();

            // Assert
            Assert.That(coreElement.ElementList, Is.Not.Null);
            Assert.That(coreElement.ElementList.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task CoreElement_ParameterizedConstructor_SetsAllProperties()
    {
        await Task.Run(() =>
        {
            // Arrange
            var id = 1;
            var label = "Test Label";
            var displayOrder = 5;
            var checkListFolderName = "TestFolder";
            var repeated = 2;
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            var language = "en";
            var multiApproval = true;
            var fastNavigation = false;
            var downloadEntities = true;
            var manualSync = false;
            var caseType = "TestCase";
            var elementList = new List<Element>();
            var color = "Red";

            // Act
            var coreElement = new CoreElement(id, label, displayOrder, checkListFolderName, repeated,
                startDate, endDate, language, multiApproval, fastNavigation, downloadEntities,
                manualSync, caseType, elementList, color);

            // Assert
            Assert.That(coreElement.Id, Is.EqualTo(id));
            Assert.That(coreElement.Label, Is.EqualTo(label));
            Assert.That(coreElement.DisplayOrder, Is.EqualTo(displayOrder));
            Assert.That(coreElement.CheckListFolderName, Is.EqualTo(checkListFolderName));
            Assert.That(coreElement.Repeated, Is.EqualTo(repeated));
            Assert.That(coreElement.StartDate, Is.EqualTo(startDate));
            Assert.That(coreElement.EndDate, Is.EqualTo(endDate));
            Assert.That(coreElement.Language, Is.EqualTo(language));
            Assert.That(coreElement.MultiApproval, Is.EqualTo(multiApproval));
            Assert.That(coreElement.FastNavigation, Is.EqualTo(fastNavigation));
            Assert.That(coreElement.DownloadEntities, Is.EqualTo(downloadEntities));
            Assert.That(coreElement.ManualSync, Is.EqualTo(manualSync));
            Assert.That(coreElement.CaseType, Is.EqualTo(caseType));
            Assert.That(coreElement.ElementList, Is.EqualTo(elementList));
            Assert.That(coreElement.Color, Is.EqualTo(color));
        });
    }

    [Test]
    public async Task CoreElement_StartDateString_ConvertsDateToString()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement();
            var testDate = new DateTime(2024, 1, 15, 14, 30, 45);

            // Act
            coreElement.StartDate = testDate;

            // Assert
            Assert.That(coreElement.StartDateString, Is.EqualTo("2024-01-15 14:30:45"));
        });
    }

    [Test]
    public async Task CoreElement_StartDateString_ParsesStringToDate()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement();
            var dateString = "2024-01-15 14:30:45";

            // Act
            coreElement.StartDateString = dateString;

            // Assert
            Assert.That(coreElement.StartDate, Is.EqualTo(new DateTime(2024, 1, 15, 14, 30, 45)));
        });
    }

    [Test]
    public async Task CoreElement_EndDateString_ConvertsDateToString()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement();
            var testDate = new DateTime(2024, 2, 20, 16, 45, 30);

            // Act
            coreElement.EndDate = testDate;

            // Assert
            Assert.That(coreElement.EndDateString, Is.EqualTo("2024-02-20 16:45:30"));
        });
    }

    [Test]
    public async Task CoreElement_EndDateString_ParsesStringToDate()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement();
            var dateString = "2024-02-20 16:45:30";

            // Act
            coreElement.EndDateString = dateString;

            // Assert
            Assert.That(coreElement.EndDate, Is.EqualTo(new DateTime(2024, 2, 20, 16, 45, 30)));
        });
    }

    [Test]
    public async Task CoreElement_DataItemGetAll_WithEmptyElementList_ReturnsEmptyList()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement();

            // Act
            var result = coreElement.DataItemGetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task CoreElement_ElementGetAll_WithEmptyElementList_ReturnsEmptyList()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement();

            // Act
            var result = coreElement.ElementGetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        });
    }

    #endregion

    #region MainElement Tests

    [Test]
    public async Task MainElement_DefaultConstructor_CreatesEmptyElementList()
    {
        await Task.Run(() =>
        {
            // Act
            var mainElement = new MainElement();

            // Assert
            Assert.That(mainElement.ElementList, Is.Not.Null);
            Assert.That(mainElement.ElementList.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task MainElement_CoreElementConstructor_CopiesAllProperties()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement
            {
                Id = 1,
                Label = "Test Label",
                DisplayOrder = 5,
                CheckListFolderName = "TestFolder",
                Repeated = 2,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Language = "en",
                MultiApproval = true,
                FastNavigation = false,
                DownloadEntities = true,
                ManualSync = false,
                CaseType = "TestCase",
                ElementList = new List<Element>(),
                EnableQuickSync = true,
                Color = "Blue"
            };

            // Act
            var mainElement = new MainElement(coreElement);

            // Assert
            Assert.That(mainElement.Id, Is.EqualTo(coreElement.Id));
            Assert.That(mainElement.Label, Is.EqualTo(coreElement.Label));
            Assert.That(mainElement.DisplayOrder, Is.EqualTo(coreElement.DisplayOrder));
            Assert.That(mainElement.CheckListFolderName, Is.EqualTo(coreElement.CheckListFolderName));
            Assert.That(mainElement.Repeated, Is.EqualTo(coreElement.Repeated));
            Assert.That(mainElement.StartDate, Is.EqualTo(coreElement.StartDate));
            Assert.That(mainElement.EndDate, Is.EqualTo(coreElement.EndDate));
            Assert.That(mainElement.Language, Is.EqualTo(coreElement.Language));
            Assert.That(mainElement.MultiApproval, Is.EqualTo(coreElement.MultiApproval));
            Assert.That(mainElement.FastNavigation, Is.EqualTo(coreElement.FastNavigation));
            Assert.That(mainElement.DownloadEntities, Is.EqualTo(coreElement.DownloadEntities));
            Assert.That(mainElement.ManualSync, Is.EqualTo(coreElement.ManualSync));
            Assert.That(mainElement.CaseType, Is.EqualTo(coreElement.CaseType));
            Assert.That(mainElement.ElementList, Is.EqualTo(coreElement.ElementList));
            Assert.That(mainElement.EnableQuickSync, Is.EqualTo(coreElement.EnableQuickSync));
            Assert.That(mainElement.Color, Is.EqualTo(coreElement.Color));
            Assert.That(mainElement.PushMessageTitle, Is.EqualTo(""));
            Assert.That(mainElement.PushMessageBody, Is.EqualTo(""));
        });
    }

    [Test]
    public async Task MainElement_ParameterizedConstructor_SetsAllProperties()
    {
        await Task.Run(() =>
        {
            // Arrange
            var id = 1;
            var label = "Test Label";
            var displayOrder = 5;
            var checkListFolderName = "TestFolder";
            var repeated = 2;
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            var language = "en";
            var multiApproval = true;
            var fastNavigation = false;
            var downloadEntities = true;
            var manualSync = false;
            var caseType = "TestCase";
            var pushMessageTitle = "Push Title";
            var pushMessageBody = "Push Body";
            var enableQuickSync = true;
            var elementList = new List<Element>();
            var color = "Green";

            // Act
            var mainElement = new MainElement(id, label, displayOrder, checkListFolderName, repeated,
                startDate, endDate, language, multiApproval, fastNavigation, downloadEntities,
                manualSync, caseType, pushMessageTitle, pushMessageBody, enableQuickSync,
                elementList, color);

            // Assert
            Assert.That(mainElement.Id, Is.EqualTo(id));
            Assert.That(mainElement.Label, Is.EqualTo(label));
            Assert.That(mainElement.DisplayOrder, Is.EqualTo(displayOrder));
            Assert.That(mainElement.CheckListFolderName, Is.EqualTo(checkListFolderName));
            Assert.That(mainElement.Repeated, Is.EqualTo(repeated));
            Assert.That(mainElement.StartDate, Is.EqualTo(startDate));
            Assert.That(mainElement.EndDate, Is.EqualTo(endDate));
            Assert.That(mainElement.Language, Is.EqualTo(language));
            Assert.That(mainElement.MultiApproval, Is.EqualTo(multiApproval));
            Assert.That(mainElement.FastNavigation, Is.EqualTo(fastNavigation));
            Assert.That(mainElement.DownloadEntities, Is.EqualTo(downloadEntities));
            Assert.That(mainElement.ManualSync, Is.EqualTo(manualSync));
            Assert.That(mainElement.CaseType, Is.EqualTo(caseType));
            Assert.That(mainElement.PushMessageTitle, Is.EqualTo(pushMessageTitle));
            Assert.That(mainElement.PushMessageBody, Is.EqualTo(pushMessageBody));
            Assert.That(mainElement.EnableQuickSync, Is.EqualTo(enableQuickSync));
            Assert.That(mainElement.ElementList, Is.EqualTo(elementList));
            Assert.That(mainElement.Color, Is.EqualTo(color));
        });
    }

    [Test]
    public async Task MainElement_PushMessageTitle_WithShortString_ReturnsFullString()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement();
            var shortTitle = "Short Title";

            // Act
            mainElement.PushMessageTitle = shortTitle;

            // Assert
            Assert.That(mainElement.PushMessageTitle, Is.EqualTo(shortTitle));
        });
    }

    [Test]
    public async Task MainElement_PushMessageTitle_WithLongString_TruncatesAt255Characters()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement();
            var longTitle = new string('A', 300); // 300 characters

            // Act
            mainElement.PushMessageTitle = longTitle;

            // Assert
            Assert.That(mainElement.PushMessageTitle.Length, Is.EqualTo(255));
            Assert.That(mainElement.PushMessageTitle, Is.EqualTo(new string('A', 255)));
        });
    }

    [Test]
    public async Task MainElement_PushMessageTitle_WithExactly255Characters_ReturnsFullString()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement();
            var exactTitle = new string('B', 255);

            // Act
            mainElement.PushMessageTitle = exactTitle;

            // Assert
            Assert.That(mainElement.PushMessageTitle.Length, Is.EqualTo(255));
            Assert.That(mainElement.PushMessageTitle, Is.EqualTo(exactTitle));
        });
    }

    [Test]
    public async Task MainElement_XmlToClass_WithValidXml_ReturnsMainElement()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement();
            var xmlStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Main xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Id>1</Id>
  <Label>Test Label</Label>
  <DisplayOrder>5</DisplayOrder>
  <CheckListFolderName>TestFolder</CheckListFolderName>
  <Repeated>2</Repeated>
  <Color>Red</Color>
  <StartDate>2024-01-15 14:30:45</StartDate>
  <EndDate>2024-01-16 14:30:45</EndDate>
  <Language>en</Language>
  <MultiApproval>true</MultiApproval>
  <FastNavigation>false</FastNavigation>
  <DownloadEntities>true</DownloadEntities>
  <ManualSync>false</ManualSync>
  <EnableQuickSync>true</EnableQuickSync>
  <ElementList />
  <PushMessageBody>Test Body</PushMessageBody>
  <BadgeCountEnabled>false</BadgeCountEnabled>
</Main>";

            // Act
            var result = mainElement.XmlToClass(xmlStr);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Label, Is.EqualTo("Test Label"));
            Assert.That(result.DisplayOrder, Is.EqualTo(5));
            Assert.That(result.CheckListFolderName, Is.EqualTo("TestFolder"));
            Assert.That(result.Repeated, Is.EqualTo(2));
            Assert.That(result.Color, Is.EqualTo("Red"));
            Assert.That(result.Language, Is.EqualTo("en"));
            Assert.That(result.MultiApproval, Is.True);
            Assert.That(result.FastNavigation, Is.False);
            Assert.That(result.DownloadEntities, Is.True);
            Assert.That(result.ManualSync, Is.False);
            Assert.That(result.EnableQuickSync, Is.True);
            Assert.That(result.PushMessageBody, Is.EqualTo("Test Body"));
            Assert.That(result.BadgeCountEnabled, Is.False);
        });
    }

    [Test]
    public async Task MainElement_XmlToClass_WithInvalidXml_ThrowsException()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement();
            var invalidXml = "invalid xml content";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => mainElement.XmlToClass(invalidXml));
            Assert.That(ex.Message, Does.Contain("MainElement failed, to convert XML"));
        });
    }

    [Test]
    public async Task MainElement_JsonToClass_WithValidJson_ReturnsMainElement()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement();
            var json = @"{
                    ""Id"": 1,
                    ""Label"": ""Test Label"",
                    ""DisplayOrder"": 5,
                    ""CheckListFolderName"": ""TestFolder"",
                    ""Repeated"": 2,
                    ""Color"": ""Blue"",
                    ""Language"": ""en"",
                    ""MultiApproval"": true,
                    ""FastNavigation"": false,
                    ""DownloadEntities"": true,
                    ""ManualSync"": false,
                    ""EnableQuickSync"": true,
                    ""ElementList"": [],
                    ""PushMessageBody"": ""Test Body"",
                    ""BadgeCountEnabled"": false
                }";

            // Act
            var result = mainElement.JsonToClass(json);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Label, Is.EqualTo("Test Label"));
            Assert.That(result.DisplayOrder, Is.EqualTo(5));
            Assert.That(result.CheckListFolderName, Is.EqualTo("TestFolder"));
            Assert.That(result.Repeated, Is.EqualTo(2));
            Assert.That(result.Color, Is.EqualTo("Blue"));
            Assert.That(result.Language, Is.EqualTo("en"));
            Assert.That(result.MultiApproval, Is.True);
            Assert.That(result.FastNavigation, Is.False);
            Assert.That(result.DownloadEntities, Is.True);
            Assert.That(result.ManualSync, Is.False);
            Assert.That(result.EnableQuickSync, Is.True);
            Assert.That(result.PushMessageBody, Is.EqualTo("Test Body"));
            Assert.That(result.BadgeCountEnabled, Is.False);
        });
    }

    [Test]
    public async Task MainElement_JsonToClass_WithInvalidJson_ThrowsException()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement();
            var invalidJson = "{ invalid json }";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => mainElement.JsonToClass(invalidJson));
            Assert.That(ex.Message, Does.Contain("MainElement failed, to convert Json"));
        });
    }

    [Test]
    public async Task MainElement_ClassToXml_ReturnsValidXmlString()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement
            {
                Id = 1,
                Label = "Test Label",
                DisplayOrder = 5,
                CheckListFolderName = "TestFolder",
                Repeated = 2,
                Color = "Green",
                Language = "en",
                MultiApproval = true,
                FastNavigation = false,
                DownloadEntities = true,
                ManualSync = false,
                EnableQuickSync = true,
                PushMessageBody = "Test Body",
                BadgeCountEnabled = false
            };
            mainElement.PushMessageTitle = "Test Title";

            // Act
            var result = mainElement.ClassToXml();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain("<?xml"));
            Assert.That(result, Does.Contain("<Main"));
            Assert.That(result, Does.Contain("<Id>1</Id>"));
            Assert.That(result, Does.Contain("<Label>Test Label</Label>"));
            Assert.That(result, Does.Contain("<DisplayOrder>5</DisplayOrder>"));
            Assert.That(result, Does.Contain("<CheckListFolderName>TestFolder</CheckListFolderName>"));
            Assert.That(result, Does.Contain("<Color>Green</Color>"));
            Assert.That(result, Does.Contain("<Language>en</Language>"));
            Assert.That(result, Does.Contain("<MultiApproval>true</MultiApproval>"));
            Assert.That(result, Does.Contain("<PushMessageBody>Test Body</PushMessageBody>"));
            Assert.That(result, Does.Contain("</Main>"));
        });
    }

    [Test]
    public async Task MainElement_ClassToJson_ReturnsValidJsonString()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement
            {
                Id = 1,
                Label = "Test Label",
                DisplayOrder = 5,
                CheckListFolderName = "TestFolder",
                Repeated = 2,
                Color = "Yellow",
                Language = "en",
                MultiApproval = true,
                FastNavigation = false,
                DownloadEntities = true,
                ManualSync = false,
                EnableQuickSync = true,
                PushMessageBody = "Test Body",
                BadgeCountEnabled = false
            };
            mainElement.PushMessageTitle = "Test Title";

            // Act
            var result = mainElement.ClassToJson();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain("\"Id\":1"));
            Assert.That(result, Does.Contain("\"Label\":\"Test Label\""));
            Assert.That(result, Does.Contain("\"DisplayOrder\":5"));
            Assert.That(result, Does.Contain("\"CheckListFolderName\":\"TestFolder\""));
            Assert.That(result, Does.Contain("\"Color\":\"Yellow\""));
            Assert.That(result, Does.Contain("\"Language\":\"en\""));
            Assert.That(result, Does.Contain("\"MultiApproval\":true"));
            Assert.That(result, Does.Contain("\"FastNavigation\":false"));
            Assert.That(result, Does.Contain("\"PushMessageBody\":\"Test Body\""));
        });
    }

    #endregion

    #region ReplyElement Tests

    [Test]
    public async Task ReplyElement_DefaultConstructor_CreatesEmptyElementList()
    {
        await Task.Run(() =>
        {
            // Act
            var replyElement = new ReplyElement();

            // Assert
            Assert.That(replyElement.ElementList, Is.Not.Null);
            Assert.That(replyElement.ElementList.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task ReplyElement_CoreElementConstructor_CopiesAllProperties()
    {
        await Task.Run(() =>
        {
            // Arrange
            var coreElement = new CoreElement
            {
                Id = 1,
                Label = "Test Label",
                DisplayOrder = 5,
                CheckListFolderName = "TestFolder",
                Repeated = 2,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Language = "en",
                MultiApproval = true,
                FastNavigation = false,
                DownloadEntities = true,
                ManualSync = false,
                CaseType = "TestCase",
                ElementList = new List<Element>(),
                MicrotingUId = 12345
            };

            // Act
            var replyElement = new ReplyElement(coreElement);

            // Assert
            Assert.That(replyElement.Id, Is.EqualTo(coreElement.Id));
            Assert.That(replyElement.Label, Is.EqualTo(coreElement.Label));
            Assert.That(replyElement.DisplayOrder, Is.EqualTo(coreElement.DisplayOrder));
            Assert.That(replyElement.CheckListFolderName, Is.EqualTo(coreElement.CheckListFolderName));
            Assert.That(replyElement.Repeated, Is.EqualTo(coreElement.Repeated));
            Assert.That(replyElement.StartDate, Is.EqualTo(coreElement.StartDate));
            Assert.That(replyElement.EndDate, Is.EqualTo(coreElement.EndDate));
            Assert.That(replyElement.Language, Is.EqualTo(coreElement.Language));
            Assert.That(replyElement.MultiApproval, Is.EqualTo(coreElement.MultiApproval));
            Assert.That(replyElement.FastNavigation, Is.EqualTo(coreElement.FastNavigation));
            Assert.That(replyElement.DownloadEntities, Is.EqualTo(coreElement.DownloadEntities));
            Assert.That(replyElement.ManualSync, Is.EqualTo(coreElement.ManualSync));
            Assert.That(replyElement.CaseType, Is.EqualTo(coreElement.CaseType));
            Assert.That(replyElement.ElementList, Is.EqualTo(coreElement.ElementList));
            Assert.That(replyElement.MicrotingUId, Is.EqualTo(coreElement.MicrotingUId));
        });
    }

    [Test]
    public async Task ReplyElement_Properties_CanBeSetAndGet()
    {
        await Task.Run(() =>
        {
            // Arrange
            var replyElement = new ReplyElement();
            var custom = "Custom Value";
            var doneAt = DateTime.Now;
            var doneById = 123;
            var unitId = 456;
            var siteMicrotingUuid = 789;
            var jasperExportEnabled = true;
            var docxExportEnabled = false;

            // Act
            replyElement.Custom = custom;
            replyElement.DoneAt = doneAt;
            replyElement.DoneById = doneById;
            replyElement.UnitId = unitId;
            replyElement.SiteMicrotingUuid = siteMicrotingUuid;
            replyElement.JasperExportEnabled = jasperExportEnabled;
            replyElement.DocxExportEnabled = docxExportEnabled;

            // Assert
            Assert.That(replyElement.Custom, Is.EqualTo(custom));
            Assert.That(replyElement.DoneAt, Is.EqualTo(doneAt));
            Assert.That(replyElement.DoneById, Is.EqualTo(doneById));
            Assert.That(replyElement.UnitId, Is.EqualTo(unitId));
            Assert.That(replyElement.SiteMicrotingUuid, Is.EqualTo(siteMicrotingUuid));
            Assert.That(replyElement.JasperExportEnabled, Is.EqualTo(jasperExportEnabled));
            Assert.That(replyElement.DocxExportEnabled, Is.EqualTo(docxExportEnabled));
        });
    }

    #endregion

    #region Protobuf Tests

    [Test]
    public async Task MainElement_ClassToProto_ReturnsValidProtoData()
    {
        await Task.Run(() =>
        {
            // Arrange
            var mainElement = new MainElement
            {
                Id = 1,
                Label = "Test Label",
                DisplayOrder = 5,
                CheckListFolderName = "TestFolder",
                Repeated = 2,
                Color = "Blue",
                Language = "en",
                MultiApproval = true,
                FastNavigation = false,
                DownloadEntities = true,
                ManualSync = false,
                EnableQuickSync = true,
                PushMessageBody = "Test Body",
                BadgeCountEnabled = false
            };
            mainElement.PushMessageTitle = "Test Title";

            // Act
            var result = mainElement.ClassToProto();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<byte[]>());
            Assert.That(result.Length, Is.GreaterThan(0));
        });
    }

    [Test]
    public async Task MainElement_ProtoToClass_ReturnsValidMainElement()
    {
        await Task.Run(() =>
        {
            // Arrange
            var originalMainElement = new MainElement
            {
                Id = 1,
                Label = "Test Label",
                DisplayOrder = 5,
                CheckListFolderName = "TestFolder",
                Repeated = 2,
                Color = "Red",
                Language = "da",
                MultiApproval = false,
                FastNavigation = true,
                DownloadEntities = false,
                ManualSync = true,
                EnableQuickSync = false,
                PushMessageBody = "Body Text",
                BadgeCountEnabled = true
            };
            originalMainElement.PushMessageTitle = "Title Text";

            var protoData = originalMainElement.ClassToProto();

            // Act
            var newMainElement = new MainElement();
            var result = newMainElement.ProtoToClass(protoData);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(originalMainElement.Id));
            Assert.That(result.Label, Is.EqualTo(originalMainElement.Label));
            Assert.That(result.DisplayOrder, Is.EqualTo(originalMainElement.DisplayOrder));
            Assert.That(result.CheckListFolderName, Is.EqualTo(originalMainElement.CheckListFolderName));
            Assert.That(result.Repeated, Is.EqualTo(originalMainElement.Repeated));
            Assert.That(result.Color, Is.EqualTo(originalMainElement.Color));
            Assert.That(result.Language, Is.EqualTo(originalMainElement.Language));
            Assert.That(result.MultiApproval, Is.EqualTo(originalMainElement.MultiApproval));
            Assert.That(result.FastNavigation, Is.EqualTo(originalMainElement.FastNavigation));
            Assert.That(result.DownloadEntities, Is.EqualTo(originalMainElement.DownloadEntities));
            Assert.That(result.ManualSync, Is.EqualTo(originalMainElement.ManualSync));
            Assert.That(result.EnableQuickSync, Is.EqualTo(originalMainElement.EnableQuickSync));
            Assert.That(result.PushMessageTitle, Is.EqualTo(originalMainElement.PushMessageTitle));
            Assert.That(result.PushMessageBody, Is.EqualTo(originalMainElement.PushMessageBody));
            Assert.That(result.BadgeCountEnabled, Is.EqualTo(originalMainElement.BadgeCountEnabled));
        });
    }

    [Test]
    public async Task MainElement_ClassToProto_ThenProtoToClass_RoundTrip()
    {
        await Task.Run(() =>
        {
            // Arrange
            var originalMainElement = new MainElement
            {
                Id = 999,
                Label = "RoundTrip Test",
                DisplayOrder = 10,
                CheckListFolderName = "RoundTripFolder",
                Repeated = 5,
                Color = "Green",
                Language = "sv",
                MultiApproval = true,
                FastNavigation = true,
                DownloadEntities = true,
                ManualSync = false,
                EnableQuickSync = true,
                PushMessageBody = "RoundTrip Body",
                BadgeCountEnabled = true
            };
            originalMainElement.PushMessageTitle = "RoundTrip Title";

            // Act
            var protoData = originalMainElement.ClassToProto();
            var roundTripMainElement = new MainElement();
            var result = roundTripMainElement.ProtoToClass(protoData);

            // Assert - All properties should match
            Assert.That(result.Id, Is.EqualTo(originalMainElement.Id));
            Assert.That(result.Label, Is.EqualTo(originalMainElement.Label));
            Assert.That(result.DisplayOrder, Is.EqualTo(originalMainElement.DisplayOrder));
            Assert.That(result.CheckListFolderName, Is.EqualTo(originalMainElement.CheckListFolderName));
            Assert.That(result.Repeated, Is.EqualTo(originalMainElement.Repeated));
            Assert.That(result.Color, Is.EqualTo(originalMainElement.Color));
            Assert.That(result.Language, Is.EqualTo(originalMainElement.Language));
            Assert.That(result.MultiApproval, Is.EqualTo(originalMainElement.MultiApproval));
            Assert.That(result.FastNavigation, Is.EqualTo(originalMainElement.FastNavigation));
            Assert.That(result.DownloadEntities, Is.EqualTo(originalMainElement.DownloadEntities));
            Assert.That(result.ManualSync, Is.EqualTo(originalMainElement.ManualSync));
            Assert.That(result.EnableQuickSync, Is.EqualTo(originalMainElement.EnableQuickSync));
            Assert.That(result.PushMessageTitle, Is.EqualTo(originalMainElement.PushMessageTitle));
            Assert.That(result.PushMessageBody, Is.EqualTo(originalMainElement.PushMessageBody));
            Assert.That(result.BadgeCountEnabled, Is.EqualTo(originalMainElement.BadgeCountEnabled));
        });
    }

    #endregion
}