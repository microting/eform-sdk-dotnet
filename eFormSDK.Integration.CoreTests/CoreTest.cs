using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Integration.CoreTests
{
    [TestFixture]
    public class CoreTest : DbTestFixture
    {
        private Core sut;
//        private TestHelpers testHelpers;
        private TestHelperReturnXML testHelperReturnXml;
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
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
//            testHelpers = new TestHelpers();
            testHelperReturnXml = new TestHelperReturnXML();
            //await sut.StartLog(new CoreBase());
        }
        
        [Test]
        public async Task Core_CheckStatusByMicrotingUid_DoesCreateCaseAndFieldValues()
        {
            // Arrange
            string microtingUuid = await testHelperReturnXml.CreateMultiPictureXMLResult(true);
            
            // Act
            await sut.CheckStatusByMicrotingUid(int.Parse(microtingUuid));

            // Assert
            List<cases> caseMatches = dbContext.cases.AsNoTracking().ToList();
            List<uploaded_data> udMatches = dbContext.uploaded_data.AsNoTracking().ToList();
            List<field_values> fvMatches = dbContext.field_values.AsNoTracking().ToList();

            Assert.NotNull(caseMatches);
            Assert.NotNull(udMatches);
            Assert.NotNull(fvMatches);
            Assert.AreEqual(3, caseMatches.Count());
            Assert.AreEqual(5, udMatches.Count());
            Assert.AreEqual(5, fvMatches.Count());

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
        // Arrange

        // Act

        // Assert
    }
}
