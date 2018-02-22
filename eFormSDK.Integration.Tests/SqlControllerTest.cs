using eFormData;
using eFormShared;
using eFormSqlController;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTest : DbTestFixture
    {
        private SqlController sut;

        public override void DoSetup()
        {
            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
        }

        #region notification
        [Test]
        public void SQL_Notification_NewNotificationCreateRetrievedForm_DoesStoreNotification()
        {
            // Arrance
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.RetrievedForm);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.notification_uid == notificationId && x.microting_uid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.RetrievedForm, notification.activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.workflow_state);
        }

        [Test]
        public void SQL_Notification_NewNotificationCreateCompletedForm_DoesStoreNotification()
        {
            // Arrance
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.Completed);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.notification_uid == notificationId && x.microting_uid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.workflow_state);
        }

        [Test]
        public void SQL_Notification_NotificationReadFirst_DoesReturnFirstNotification()
        {
            // Arrance
            var notificationId1 = Guid.NewGuid().ToString();
            var microtingUId1 = Guid.NewGuid().ToString();
            var notificationId2 = Guid.NewGuid().ToString();
            var microtingUId2 = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId1, microtingUId1, Constants.Notifications.Completed);
            sut.NotificationCreate(notificationId2, microtingUId2, Constants.Notifications.Completed);

            // Assert
            Note_Dto notification = sut.NotificationReadFirst();

            Assert.NotNull(notification);
            Assert.AreEqual(2, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(microtingUId1, notification.MicrotingUId);
        }

        [Test]
        public void SQL_Notification_NotificationUpdate_DoesUpdateNotification()
        {
            // Arrance
            var notificationUId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationUId, microtingUId, Constants.Notifications.Completed);
            sut.NotificationUpdate(notificationUId, microtingUId, Constants.WorkflowStates.Processed, "");

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.notification_uid == notificationUId && x.microting_uid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.activity);
            Assert.AreEqual(Constants.WorkflowStates.Processed, notification.workflow_state);
        }
        #endregion

        #region uploaded_data
        [Test]
        public void SQL_UploadedData_FileRead_DoesReturnOneUploadedData()
        {
            // Arrance
            string checksum = "";
            string extension = "jpg";
            string currentFile = "Hello.jpg";
            int uploaderId = 1;
            string fileLocation = @"c:\here";
            string fileName = "Hello.jpg";

            // Act
            uploaded_data dU = new uploaded_data();

            dU.created_at = DateTime.Now;
            dU.updated_at = DateTime.Now;
            dU.extension = extension;
            dU.uploader_id = uploaderId;
            dU.uploader_type = Constants.UploaderTypes.System;
            dU.workflow_state = Constants.WorkflowStates.PreCreated;
            dU.version = 1;
            dU.local = 0;
            dU.file_location = fileLocation;
            dU.file_name = fileName;
            dU.current_file = currentFile;
            dU.checksum = checksum;

            DbContext.uploaded_data.Add(dU);
            DbContext.SaveChanges();

            UploadedData ud = sut.FileRead();

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(dU.id, ud.Id);
            Assert.AreEqual(dU.checksum, ud.Checksum);
            Assert.AreEqual(dU.extension, ud.Extension);
            Assert.AreEqual(dU.current_file, ud.CurrentFile);
            Assert.AreEqual(dU.uploader_id, ud.UploaderId);
            Assert.AreEqual(dU.uploader_type, ud.UploaderType);
            Assert.AreEqual(dU.file_location, ud.FileLocation);
            Assert.AreEqual(dU.file_name, ud.FileName);
            //Assert.AreEqual(dU.local, ud.);

        }

        [Test]
        public void SQL_UploadedData_UploadedDataRead_DoesReturnOneUploadedDataClass()
        {
            // Arrance
            string checksum = "";
            string extension = "jpg";
            string currentFile = "Hello.jpg";
            int uploaderId = 1;
            string fileLocation = @"c:\here";
            string fileName = "Hello.jpg";

            // Act
            uploaded_data dU = new uploaded_data();

            dU.created_at = DateTime.Now;
            dU.updated_at = DateTime.Now;
            dU.extension = extension;
            dU.uploader_id = uploaderId;
            dU.uploader_type = Constants.UploaderTypes.System;
            dU.workflow_state = Constants.WorkflowStates.PreCreated;
            dU.version = 1;
            dU.local = 0;
            dU.file_location = fileLocation;
            dU.file_name = fileName;
            dU.current_file = currentFile;
            dU.checksum = checksum;

            DbContext.uploaded_data.Add(dU);
            DbContext.SaveChanges();

            uploaded_data ud = sut.GetUploadedData(dU.id);

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(ud.id, dU.id);
            Assert.AreEqual(ud.extension, dU.extension);
            Assert.AreEqual(ud.uploader_id, dU.uploader_id);
            Assert.AreEqual(ud.uploader_type, dU.uploader_type);
            Assert.AreEqual(ud.workflow_state, dU.workflow_state);
            Assert.AreEqual(ud.version, 1);
            Assert.AreEqual(ud.local, 0);
            Assert.AreEqual(ud.file_location, dU.file_location);
            Assert.AreEqual(ud.file_name, dU.file_name);
            Assert.AreEqual(ud.current_file, dU.current_file);
            Assert.AreEqual(ud.checksum, dU.checksum);

        }
        #endregion

        #region template
        [Test]
        public void SQL_Template_TemplateItemReadAll_DoesSortAccordingly()
        {

            // Arrance

            #region Template1
            check_lists cl1 = new check_lists();
            cl1.created_at = DateTime.Now;
            cl1.updated_at = DateTime.Now;
            cl1.label = "A";
            cl1.description = "D";
            cl1.workflow_state = Constants.WorkflowStates.Created;
            cl1.case_type = "CheckList";
            cl1.folder_name = "Template1FolderName";
            cl1.display_index = 1;
            cl1.repeated = 1;

            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();
            Thread.Sleep(1000);
            #endregion

            #region Template2
            check_lists cl2 = new check_lists();
            cl2.created_at = DateTime.Now;
            cl2.updated_at = DateTime.Now;
            cl2.label = "B";
            cl2.description = "C";
            cl2.workflow_state = Constants.WorkflowStates.Removed;
            cl2.case_type = "CheckList";
            cl2.folder_name = "Template1FolderName";
            cl2.display_index = 1;
            cl2.repeated = 1;

            DbContext.check_lists.Add(cl2);
            DbContext.SaveChanges();
            Thread.Sleep(1000);
            #endregion

            #region Template3
            check_lists cl3 = new check_lists();
            cl3.created_at = DateTime.Now;
            cl3.updated_at = DateTime.Now;
            cl3.label = "D";
            cl3.description = "B";
            cl3.workflow_state = Constants.WorkflowStates.Created;
            cl3.case_type = "CheckList";
            cl3.folder_name = "Template1FolderName";
            cl3.display_index = 1;
            cl3.repeated = 1;

            DbContext.check_lists.Add(cl3);
            DbContext.SaveChanges();
            Thread.Sleep(1000);
            #endregion

            #region Template4
            check_lists cl4 = new check_lists();
            cl4.created_at = DateTime.Now;
            cl4.updated_at = DateTime.Now;
            cl4.label = "C";
            cl4.description = "A";
            cl4.workflow_state = Constants.WorkflowStates.Created;
            cl4.case_type = "CheckList";
            cl4.folder_name = "Template1FolderName";
            cl4.display_index = 1;
            cl4.repeated = 1;

            DbContext.check_lists.Add(cl4);
            DbContext.SaveChanges();
            #endregion
            
            
            // Act
            List<int> emptyList = new List<int>();

            // Default sorting including removed
            List<Template_Dto> templateListId = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, "", emptyList);
            List<Template_Dto> templateListLabel = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescription = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListCreatedAt = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Descending including removed
            List<Template_Dto> templateListDescengingId = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, "", emptyList);
            List<Template_Dto> templateListDescengingLabel = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescengingDescription = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListDescengingCreatedAt = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Default sorting excluding removed
            List<Template_Dto> templateListIdNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, "", emptyList);
            List<Template_Dto> templateListLabelNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescriptionNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListCreatedAtNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Descending excluding removed
            List<Template_Dto> templateListDescengingIdNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, "", emptyList);
            List<Template_Dto> templateListDescengingLabelNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescengingDescriptionNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListDescengingCreatedAtNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.CreatedAt, emptyList);


            // Assert

            #region include removed
            // Default sorting including removed 
            // Id
            Assert.NotNull(templateListId);
            Assert.AreEqual(4, templateListId.Count());
            Assert.AreEqual("A", templateListId[0].Label);
            Assert.AreEqual("B", templateListId[1].Label);
            Assert.AreEqual("D", templateListId[2].Label);
            Assert.AreEqual("C", templateListId[3].Label);
            Assert.AreEqual(0, templateListId[0].Tags.Count());
            Assert.AreEqual(0, templateListId[1].Tags.Count());
            Assert.AreEqual(0, templateListId[2].Tags.Count());
            Assert.AreEqual(0, templateListId[3].Tags.Count());

            // Default sorting including removed 
            // Label
            Assert.NotNull(templateListLabel);
            Assert.AreEqual(4, templateListLabel.Count());
            Assert.AreEqual("A", templateListLabel[0].Label);
            Assert.AreEqual("B", templateListLabel[1].Label);
            Assert.AreEqual("C", templateListLabel[2].Label);
            Assert.AreEqual("D", templateListLabel[3].Label);
            Assert.AreEqual(0, templateListLabel[0].Tags.Count());
            Assert.AreEqual(0, templateListLabel[1].Tags.Count());
            Assert.AreEqual(0, templateListLabel[2].Tags.Count());
            Assert.AreEqual(0, templateListLabel[3].Tags.Count());

            // Default sorting including removed 
            // Description
            Assert.NotNull(templateListDescription);
            Assert.AreEqual(4, templateListDescription.Count());
            Assert.AreEqual("C", templateListDescription[0].Label);
            Assert.AreEqual("D", templateListDescription[1].Label);
            Assert.AreEqual("B", templateListDescription[2].Label);
            Assert.AreEqual("A", templateListDescription[3].Label);
            Assert.AreEqual(0, templateListDescription[0].Tags.Count());
            Assert.AreEqual(0, templateListDescription[1].Tags.Count());
            Assert.AreEqual(0, templateListDescription[2].Tags.Count());
            Assert.AreEqual(0, templateListDescription[3].Tags.Count());

            // Default sorting including removed 
            // Created At
            Assert.NotNull(templateListCreatedAt);
            Assert.AreEqual(4, templateListCreatedAt.Count());
            Assert.AreEqual("A", templateListCreatedAt[0].Label);
            Assert.AreEqual("B", templateListCreatedAt[1].Label);
            Assert.AreEqual("D", templateListCreatedAt[2].Label);
            Assert.AreEqual("C", templateListCreatedAt[3].Label);
            Assert.AreEqual(0, templateListCreatedAt[0].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAt[1].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAt[2].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAt[3].Tags.Count());

            // Descending sorting including removed 
            // Id
            Assert.NotNull(templateListDescengingId);
            Assert.AreEqual(4, templateListDescengingId.Count());
            Assert.AreEqual("C", templateListDescengingId[0].Label);
            Assert.AreEqual("D", templateListDescengingId[1].Label);
            Assert.AreEqual("B", templateListDescengingId[2].Label);
            Assert.AreEqual("A", templateListDescengingId[3].Label);
            Assert.AreEqual(0, templateListDescengingId[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingId[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingId[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingId[3].Tags.Count());

            // Descending sorting including removed 
            // Label
            Assert.NotNull(templateListDescengingLabel);
            Assert.AreEqual(4, templateListDescengingLabel.Count());
            Assert.AreEqual("D", templateListDescengingLabel[0].Label);
            Assert.AreEqual("C", templateListDescengingLabel[1].Label);
            Assert.AreEqual("B", templateListDescengingLabel[2].Label);
            Assert.AreEqual("A", templateListDescengingLabel[3].Label);
            Assert.AreEqual(0, templateListDescengingLabel[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabel[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabel[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabel[3].Tags.Count());

            // Descending sorting including removed 
            // Description
            Assert.NotNull(templateListDescengingDescription);
            Assert.AreEqual(4, templateListDescengingDescription.Count());
            Assert.AreEqual("A", templateListDescengingDescription[0].Label);
            Assert.AreEqual("B", templateListDescengingDescription[1].Label);
            Assert.AreEqual("D", templateListDescengingDescription[2].Label);
            Assert.AreEqual("C", templateListDescengingDescription[3].Label);
            Assert.AreEqual(0, templateListDescengingDescription[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescription[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescription[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescription[3].Tags.Count());

            // Descending sorting including removed 
            // Created At
            Assert.NotNull(templateListDescengingCreatedAt);
            Assert.AreEqual(4, templateListDescengingCreatedAt.Count());
            Assert.AreEqual("C", templateListDescengingCreatedAt[0].Label);
            Assert.AreEqual("D", templateListDescengingCreatedAt[1].Label);
            Assert.AreEqual("B", templateListDescengingCreatedAt[2].Label);
            Assert.AreEqual("A", templateListDescengingCreatedAt[3].Label);
            Assert.AreEqual(0, templateListDescengingCreatedAt[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAt[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAt[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAt[3].Tags.Count());
            #endregion

            #region Exclude removed
            // Default sorting including removed 
            // Id
            Assert.NotNull(templateListIdNr);
            Assert.AreEqual(3, templateListIdNr.Count());
            Assert.AreEqual("A", templateListIdNr[0].Label);
            Assert.AreEqual("D", templateListIdNr[1].Label);
            Assert.AreEqual("C", templateListIdNr[2].Label);
            Assert.AreEqual(0, templateListIdNr[0].Tags.Count());
            Assert.AreEqual(0, templateListIdNr[1].Tags.Count());
            Assert.AreEqual(0, templateListIdNr[2].Tags.Count());

            // Default sorting including removed 
            // Label
            Assert.NotNull(templateListLabelNr);
            Assert.AreEqual(3, templateListLabelNr.Count());
            Assert.AreEqual("A", templateListLabelNr[0].Label);
            Assert.AreEqual("C", templateListLabelNr[1].Label);
            Assert.AreEqual("D", templateListLabelNr[2].Label);
            Assert.AreEqual(0, templateListLabelNr[0].Tags.Count());
            Assert.AreEqual(0, templateListLabelNr[1].Tags.Count());
            Assert.AreEqual(0, templateListLabelNr[2].Tags.Count());

            // Default sorting including removed 
            // Description
            Assert.NotNull(templateListDescriptionNr);
            Assert.AreEqual(3, templateListDescriptionNr.Count());
            Assert.AreEqual("C", templateListDescriptionNr[0].Label);
            Assert.AreEqual("D", templateListDescriptionNr[1].Label);
            Assert.AreEqual("A", templateListDescriptionNr[2].Label);
            Assert.AreEqual(0, templateListDescriptionNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescriptionNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescriptionNr[2].Tags.Count());

            // Default sorting including removed 
            // Created At
            Assert.NotNull(templateListCreatedAtNr);
            Assert.AreEqual(3, templateListCreatedAtNr.Count());
            Assert.AreEqual("A", templateListCreatedAtNr[0].Label);
            Assert.AreEqual("D", templateListCreatedAtNr[1].Label);
            Assert.AreEqual("C", templateListCreatedAtNr[2].Label);
            Assert.AreEqual(0, templateListCreatedAtNr[0].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAtNr[1].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAtNr[2].Tags.Count());

            // Descending sorting including removed 
            // Id
            Assert.NotNull(templateListDescengingIdNr);
            Assert.AreEqual(3, templateListDescengingIdNr.Count());
            Assert.AreEqual("C", templateListDescengingIdNr[0].Label);
            Assert.AreEqual("D", templateListDescengingIdNr[1].Label);
            Assert.AreEqual("A", templateListDescengingIdNr[2].Label);
            Assert.AreEqual(0, templateListDescengingIdNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingIdNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingIdNr[2].Tags.Count());

            // Descending sorting including removed 
            // Label
            Assert.NotNull(templateListDescengingLabelNr);
            Assert.AreEqual(3, templateListDescengingLabelNr.Count());
            Assert.AreEqual("D", templateListDescengingLabelNr[0].Label);
            Assert.AreEqual("C", templateListDescengingLabelNr[1].Label);
            Assert.AreEqual("A", templateListDescengingLabelNr[2].Label);
            Assert.AreEqual(0, templateListDescengingLabelNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabelNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabelNr[2].Tags.Count());

            // Descending sorting including removed 
            // Description
            Assert.NotNull(templateListDescengingDescriptionNr);
            Assert.AreEqual(3, templateListDescengingDescriptionNr.Count());
            Assert.AreEqual("A", templateListDescengingDescriptionNr[0].Label);
            Assert.AreEqual("D", templateListDescengingDescriptionNr[1].Label);
            Assert.AreEqual("C", templateListDescengingDescriptionNr[2].Label);
            Assert.AreEqual(0, templateListDescengingDescriptionNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescriptionNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescriptionNr[2].Tags.Count());

            // Descending sorting including removed 
            // Created At
            Assert.NotNull(templateListDescengingCreatedAtNr);
            Assert.AreEqual(3, templateListDescengingCreatedAtNr.Count());
            Assert.AreEqual("C", templateListDescengingCreatedAtNr[0].Label);
            Assert.AreEqual("D", templateListDescengingCreatedAtNr[1].Label);
            Assert.AreEqual("A", templateListDescengingCreatedAtNr[2].Label);
            Assert.AreEqual(0, templateListDescengingCreatedAtNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAtNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAtNr[2].Tags.Count());

            #endregion

        }
        
        [Test]
        public void SQL_Template_TemplateDelete_DoesMarkTemplateAsRemoved()
        {
            // Arrance
            #region Template1

            check_lists cl1 = new check_lists();
            cl1.created_at = DateTime.Now;
            cl1.updated_at = DateTime.Now;
            cl1.label = "A";
            cl1.description = "D";
            cl1.workflow_state = Constants.WorkflowStates.Created;
            cl1.case_type = "CheckList";
            cl1.folder_name = "Template1FolderName";
            cl1.display_index = 1;
            cl1.repeated = 1;

            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();
            #endregion
            // Act

            sut.TemplateDelete(cl1.id);

            Template_Dto clResult = sut.TemplateItemRead(cl1.id);

            // Assert

            var checkLists = DbContext.check_lists.AsNoTracking().ToList();

            Assert.NotNull(clResult);
            Assert.AreEqual(1, checkLists.Count());
            //Assert.AreEqual(1, cl_results.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkLists[0].workflow_state);
            
        }
        
        
        [Test]
        public void SQL_Template_UpdateCaseFieldValue_DoesUpdateFieldValues()
        {
            // Arrance

            // Act

            // Assert
            Assert.True(true);
        }
        #endregion

        #region public "reply"

        #region check
        //TODO
        [Test]
        public void SQL_Check_ChecksCreate_IsCreated()
        {


            // Arrance
            #region Template1
            /* check_lists cl1 = new check_lists();
             cl1.created_at = DateTime.Now;
             cl1.updated_at = DateTime.Now;
             cl1.label = "A";
             cl1.description = "D";
             cl1.workflow_state = Constants.WorkflowStates.Created;
             cl1.case_type = "CheckList";
             cl1.folder_name = "Template1FolderName";
             cl1.display_index = 1;
             cl1.repeated = 1;

             DbContext.check_lists.Add(cl1);
             DbContext.SaveChanges();
             */
            #endregion

            #region SubTemplate1
            /*check_lists cl2 = new check_lists();
            cl2.created_at = DateTime.Now;
            cl2.updated_at = DateTime.Now;
            cl2.label = "A.1";
            cl2.description = "D.1";
            cl2.workflow_state = Constants.WorkflowStates.Created;
            cl2.case_type = "CheckList";
            cl2.display_index = 1;
            cl2.repeated = 1;
            cl2.parent_id = cl1.id;

            DbContext.check_lists.Add(cl2);
            DbContext.SaveChanges();
            */
            #endregion

            #region Fields
            /*
            field_types ft = DbContext.field_types.Where(x => x.id == 9).First();

            fields f1 = new fields();
            f1.field_type = ft;
            f1.label = "Comment field";
            f1.description = "";
            f1.check_list_id = cl2.id;

            DbContext.fields.Add(f1);
            DbContext.SaveChanges();
            */
            #endregion

            #region Worker
            /*
            workers worker = new workers();
            worker.first_name = "Arne";
            worker.last_name = "Jensen";
            worker.email = "aa@tak.dk";
            worker.created_at = DateTime.Now;
            worker.updated_at = DateTime.Now;
            worker.microting_uid = 21;
            worker.workflow_state = Constants.WorkflowStates.Created;
            worker.version = 69;
            DbContext.workers.Add(worker);
            DbContext.SaveChanges();
            */
            #endregion

            #region site
            /*
            sites site = new sites();
            site.name = "SiteName";
            site.microting_uid = 88;
            site.updated_at = DateTime.Now;
            site.created_at = DateTime.Now;
            site.version = 64;
            site.workflow_state = Constants.WorkflowStates.Created;
            DbContext.sites.Add(site);
            DbContext.SaveChanges();
            */
            #endregion

            #region units

            /*units unit = new units();
            unit.microting_uid = 48;
            unit.otp_code = 49;
            unit.site = site;
            unit.site_id = site.id;
            unit.created_at = DateTime.Now;
            unit.customer_no = 348;
            unit.updated_at = DateTime.Now;
            unit.version = 9;
            unit.workflow_state = Constants.WorkflowStates.Created;

            DbContext.units.Add(unit);
            DbContext.SaveChanges();
            */
            #endregion

            #region site_workers
            /* site_workers site_workers = new site_workers();
             site_workers.created_at = DateTime.Now;
             site_workers.microting_uid = 55;
             site_workers.updated_at = DateTime.Now;
             site_workers.version = 63;
             site_workers.site = site;
             site_workers.site_id = site.id;
             site_workers.worker = worker;
             site_workers.worker_id = worker.id;
             site_workers.workflow_state = Constants.WorkflowStates.Created;
             DbContext.site_workers.Add(site_workers);
             DbContext.SaveChanges();
             */
            #endregion

            #region Case1
            /*
            sites site = new sites();
            site.name = "SiteName";
            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            check_lists cl = new check_lists();
            cl.label = "label";

            DbContext.check_lists.Add(cl);
            DbContext.SaveChanges();



            string caseType = "AAKKAA";
            DateTime createdAt = DateTime.Now;
            int checkListId = 1;
            string microtingUId = "microting_UId";
            string microtingCheckId = "microting_Check_Id";
            string caseUId = "caseUId";
            string custom = "custom";
            cases aCase = new cases();
            aCase.status = 66;
            aCase.type = caseType;
            aCase.created_at = createdAt;
            aCase.updated_at = createdAt;
            aCase.check_list_id = checkListId;
            aCase.microting_uid = microtingUId;
            aCase.microting_check_uid = microtingCheckId;
            aCase.case_uid = caseUId;
            aCase.workflow_state = Constants.WorkflowStates.Created;
            aCase.version = 1;
            aCase.site_id = site.id;

            aCase.custom = custom;

            DbContext.cases.Add(aCase);
            DbContext.SaveChanges();
            */
            #endregion

            #region Check List Values
            /*
            check_list_values check_List_Values = new check_list_values();

            check_List_Values.case_id = aCase.id;
            check_List_Values.check_list_id = cl2.id;
            check_List_Values.created_at = DateTime.Now;
            check_List_Values.status = "completed";
            check_List_Values.updated_at = DateTime.Now;
            check_List_Values.user_id = null;
            check_List_Values.version = 865;
            check_List_Values.workflow_state = Constants.WorkflowStates.Created;

            DbContext.check_list_values.Add(check_List_Values);
            DbContext.SaveChanges();
            */
            #endregion

            #region Field Values
            /*
            field_values field_Values1 = new field_values();
            field_Values1.case_id = aCase.id;
            field_Values1.check_list = cl2;
            field_Values1.check_list_id = cl2.id;
            field_Values1.created_at = DateTime.Now;
            field_Values1.date = DateTime.Now;
            field_Values1.done_at = DateTime.Now;
            field_Values1.field = f1;
            field_Values1.field_id = f1.id;
            field_Values1.updated_at = DateTime.Now;
            field_Values1.user_id = null;
            field_Values1.value = "tomt1";
            field_Values1.version = 61234;
            field_Values1.worker = worker;
            field_Values1.workflow_state = Constants.WorkflowStates.Created;

            DbContext.field_values.Add(field_Values1);
            DbContext.SaveChanges();

            field_values field_Values2 = new field_values();
            field_Values2.case_id = aCase.id;
            field_Values2.check_list = cl2;
            field_Values2.check_list_id = cl2.id;
            field_Values2.created_at = DateTime.Now;
            field_Values2.date = DateTime.Now;
            field_Values2.done_at = DateTime.Now;
            field_Values2.field = f2;
            field_Values2.field_id = f2.id;
            field_Values2.updated_at = DateTime.Now;
            field_Values2.user_id = null;
            field_Values2.value = "tomt2";
            field_Values2.version = 61234;
            field_Values2.worker = worker;
            field_Values2.workflow_state = Constants.WorkflowStates.Created;

            DbContext.field_values.Add(field_Values2);
            DbContext.SaveChanges();

            field_values field_Values3 = new field_values();
            field_Values3.case_id = aCase.id;
            field_Values3.check_list = cl2;
            field_Values3.check_list_id = cl2.id;
            field_Values3.created_at = DateTime.Now;
            field_Values3.date = DateTime.Now;
            field_Values3.done_at = DateTime.Now;
            field_Values3.field = f3;
            field_Values3.field_id = f3.id;
            field_Values3.updated_at = DateTime.Now;
            field_Values3.user_id = null;
            field_Values3.value = "tomt3";
            field_Values3.version = 61234;
            field_Values3.worker = worker;
            field_Values3.workflow_state = Constants.WorkflowStates.Created;

            DbContext.field_values.Add(field_Values3);
            DbContext.SaveChanges();

            field_values field_Values4 = new field_values();
            field_Values4.case_id = aCase.id;
            field_Values4.check_list = cl2;
            field_Values4.check_list_id = cl2.id;
            field_Values4.created_at = DateTime.Now;
            field_Values4.date = DateTime.Now;
            field_Values4.done_at = DateTime.Now;
            field_Values4.field = f4;
            field_Values4.field_id = f4.id;
            field_Values4.updated_at = DateTime.Now;
            field_Values4.user_id = null;
            field_Values4.value = "tomt4";
            field_Values4.version = 61234;
            field_Values4.worker = worker;
            field_Values4.workflow_state = Constants.WorkflowStates.Created;

            DbContext.field_values.Add(field_Values4);
            DbContext.SaveChanges();

            field_values field_Values5 = new field_values();
            field_Values5.case_id = aCase.id;
            field_Values5.check_list = cl2;
            field_Values5.check_list_id = cl2.id;
            field_Values5.created_at = DateTime.Now;
            field_Values5.date = DateTime.Now;
            field_Values5.done_at = DateTime.Now;
            field_Values5.field = f5;
            field_Values5.field_id = f5.id;
            field_Values5.updated_at = DateTime.Now;
            field_Values5.user_id = null;
            field_Values5.value = "tomt5";
            field_Values5.version = 61234;
            field_Values5.worker = worker;
            field_Values5.workflow_state = Constants.WorkflowStates.Created;

            DbContext.field_values.Add(field_Values5);
            DbContext.SaveChanges();
            */
            #endregion

    
            // Act



            // Assert

        }
        [Test]
        public void SQL_Check_CheckRead_ReturnsReplyElement()
        {
            // Arrance
            #region Arrance

            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);
            //    new check_lists();
            //cl1.created_at = DateTime.Now;
            //cl1.updated_at = DateTime.Now;
            //cl1.label = "A";
            //cl1.description = "D";
            //cl1.workflow_state = Constants.WorkflowStates.Created;
            //cl1.case_type = "CheckList";
            //cl1.folder_name = "Template1FolderName";
            //cl1.display_index = 1;
            //cl1.repeated = 1;

            //DbContext.check_lists.Add(cl1);
            //DbContext.SaveChanges();
            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
            //    new check_lists();
            //cl2.created_at = DateTime.Now;
            //cl2.updated_at = DateTime.Now;
            //cl2.label = "A.1";
            //cl2.description = "D.1";
            //cl2.workflow_state = Constants.WorkflowStates.Created;
            //cl2.case_type = "CheckList";
            //cl2.display_index = 1;
            //cl2.repeated = 1;
            //cl2.parent_id = cl1.id;

            //DbContext.check_lists.Add(cl2);
            //DbContext.SaveChanges();

            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description", 
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
            //    new fields();
            //field_types ft1 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f1.field_type = ft1;

            //f1.barcode_enabled = 1;
            //f1.barcode_type = "barcode";
            //f1.check_list_id = cl2.id;
            //f1.color = "e2f4fb";
            //f1.created_at = DateTime.Now;
            //f1.custom = "custom";
            //f1.decimal_count = null;
            //f1.default_value = "";
            //f1.description = "Comment field Description";
            //f1.display_index = 5;
            //f1.dummy = 1;
            //f1.geolocation_enabled = 0;
            //f1.geolocation_forced = 0;
            //f1.geolocation_hidden = 1;
            //f1.is_num = 0;
            //f1.label = "Comment field";
            //f1.mandatory = 1;
            //f1.max_length = 55;
            //f1.max_value = "55";
            //f1.min_value = "0";
            //f1.multi = 0;
            //f1.optional = 0;
            //f1.query_type = null;
            //f1.read_only = 1;
            //f1.selected = 0;
            //f1.split_screen = 0;
            //f1.stop_on_save = 0;
            //f1.unit_name = "";
            //f1.updated_at = DateTime.Now;
            //f1.version = 49;
            //f1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f1);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);
            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
            //    new fields();
            //field_types ft2 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f2.field_type = ft2;

            //f2.barcode_enabled = 1;
            //f2.barcode_type = "barcode";
            //f2.check_list_id = cl2.id;
            //f2.color = "f5eafa";
            //f2.default_value = "";
            //f2.description = "showPDf Description";
            //f2.display_index = 45;
            //f2.dummy = 1;
            //f2.geolocation_enabled = 0;
            //f2.geolocation_forced = 1;
            //f2.geolocation_hidden = 0;
            //f2.is_num = 0;
            //f2.label = "ShowPdf";
            //f2.mandatory = 0;
            //f2.max_length = 5;
            //f2.max_value = "5";
            //f2.min_value = "0";
            //f2.multi = 0;
            //f2.optional = 0;
            //f2.query_type = null;
            //f2.read_only = 0;
            //f2.selected = 0;
            //f2.split_screen = 0;
            //f2.stop_on_save = 0;
            //f2.unit_name = "";
            //f2.updated_at = DateTime.Now;
            //f2.version = 9;
            //f2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f2);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
            //    new fields();
            //field_types ft3 = DbContext.field_types.Where(x => x.field_type == "number").First();

            //f3.field_type = ft3;

            //f3.barcode_enabled = 0;
            //f3.barcode_type = "barcode";
            //f3.check_list_id = cl2.id;
            //f3.color = "f0f8db";
            //f3.created_at = DateTime.Now;
            //f3.custom = "custom";
            //f3.decimal_count = 3;
            //f3.default_value = "";
            //f3.description = "Number Field Description";
            //f3.display_index = 83;
            //f3.dummy = 0;
            //f3.geolocation_enabled = 0;
            //f3.geolocation_forced = 0;
            //f3.geolocation_hidden = 1;
            //f3.is_num = 0;
            //f3.label = "Numberfield";
            //f3.mandatory = 1;
            //f3.max_length = 8;
            //f3.max_value = "4865";
            //f3.min_value = "0";
            //f3.multi = 0;
            //f3.optional = 1;
            //f3.query_type = null;
            //f3.read_only = 1;
            //f3.selected = 0;
            //f3.split_screen = 0;
            //f3.stop_on_save = 0;
            //f3.unit_name = "";
            //f3.updated_at = DateTime.Now;
            //f3.version = 1;
            //f3.workflow_state = Constants.WorkflowStates.Created;



            //DbContext.fields.Add(f3);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft4 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f4.field_type = ft4;

            //f4.barcode_enabled = 1;
            //f4.barcode_type = "barcode";
            //f4.check_list_id = cl2.id;
            //f4.color = "fff6df";
            //f4.created_at = DateTime.Now;
            //f4.custom = "custom";
            //f4.decimal_count = null;
            //f4.default_value = "";
            //f4.description = "date Description";
            //f4.display_index = 84;
            //f4.dummy = 0;
            //f4.geolocation_enabled = 0;
            //f4.geolocation_forced = 0;
            //f4.geolocation_hidden = 1;
            //f4.is_num = 0;
            //f4.label = "Date";
            //f4.mandatory = 1;
            //f4.max_length = 666;
            //f4.max_value = "41153";
            //f4.min_value = "0";
            //f4.multi = 0;
            //f4.optional = 1;
            //f4.query_type = null;
            //f4.read_only = 0;
            //f4.selected = 1;
            //f4.split_screen = 0;
            //f4.stop_on_save = 0;
            //f4.unit_name = "";
            //f4.updated_at = DateTime.Now;
            //f4.version = 1;
            //f4.workflow_state = Constants.WorkflowStates.Created;
        

            //DbContext.fields.Add(f4);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft5 = DbContext.field_types.Where(x => x.field_type == "comment").First();

            //f5.field_type = ft5;
            //f5.barcode_enabled = 0;
            //f5.barcode_type = "barcode";
            //f5.check_list_id = cl2.id;
            //f5.color = "ffe4e4";
            //f5.created_at = DateTime.Now;
            //f5.custom = "custom";
            //f5.decimal_count = null;
            //f5.default_value = "";
            //f5.description = "picture Description";
            //f5.display_index = 85;
            //f5.dummy = 0;
            //f5.geolocation_enabled = 1;
            //f5.geolocation_forced = 0;
            //f5.geolocation_hidden = 1;
            //f5.is_num = 0;
            //f5.label = "Picture";
            //f5.mandatory = 1;
            //f5.max_length = 69;
            //f5.max_value = "69";
            //f5.min_value = "1";
            //f5.multi = 0;
            //f5.optional = 1;
            //f5.query_type = null;
            //f5.read_only = 0;
            //f5.selected = 1;
            //f5.split_screen = 0;
            //f5.stop_on_save = 0;
            //f5.unit_name = "";
            //f5.updated_at = DateTime.Now;
            //f5.version = 1;
            //f5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f5);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            //= new workers();
            //worker.first_name = "Arne";
            //worker.last_name = "Jensen";
            //worker.email = "aa@tak.dk";
            //worker.created_at = DateTime.Now;
            //worker.updated_at = DateTime.Now;
            //worker.microting_uid = 21;
            //worker.workflow_state = Constants.WorkflowStates.Created;
            //worker.version = 69;
            //DbContext.workers.Add(worker);
            //DbContext.SaveChanges();
            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);
            //    new sites();
            //site.name = "SiteName";
            //site.microting_uid = 88;
            //site.updated_at = DateTime.Now;
            //site.created_at = DateTime.Now;
            //site.version = 64;
            //site.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.sites.Add(site);
            //DbContext.SaveChanges();
            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);
            //    new units();
            //unit.microting_uid = 48;
            //unit.otp_code = 49;
            //unit.site = site;
            //unit.site_id = site.id;
            //unit.created_at = DateTime.Now;
            //unit.customer_no = 348;
            //unit.updated_at = DateTime.Now;
            //unit.version = 9;
            //unit.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.units.Add(unit);
            //DbContext.SaveChanges();
            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);
            //    new site_workers();
            //site_workers.created_at = DateTime.Now;
            //site_workers.microting_uid = 55;
            //site_workers.updated_at = DateTime.Now;
            //site_workers.version = 63;
            //site_workers.site = site;
            //site_workers.site_id = site.id;
            //site_workers.worker = worker;
            //site_workers.worker_id = worker.id;
            //site_workers.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.site_workers.Add(site_workers);
            //DbContext.SaveChanges();
            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);
            //    new cases();
            //aCase.case_uid = caseUId;
            //aCase.check_list = cl1;
            //aCase.check_list_id = cl1.id;
            //aCase.created_at = DateTime.Now;
            //aCase.custom = custom;
            //aCase.done_at = DateTime.Now;
            //aCase.done_by_user_id = worker.id;
            //aCase.microting_check_uid = microtingCheckId;
            //aCase.microting_uid = microtingUId;
            //aCase.site = site;
            //aCase.site_id = site.id;
            //aCase.status = 66;
            //aCase.type = caseType;
            //aCase.unit = unit;
            //aCase.unit_id = unit.id;
            //aCase.updated_at = DateTime.Now;
            //aCase.version = 1;
            //aCase.worker = worker;
            //aCase.workflow_state = Constants.WorkflowStates.Created;       
            //DbContext.cases.Add(aCase);
            //DbContext.SaveChanges();
            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);
            //    new check_list_values();

            //check_List_Values.case_id = aCase.id;
            //check_List_Values.check_list_id = cl2.id;
            //check_List_Values.created_at = DateTime.Now;
            //check_List_Values.status = "completed";
            //check_List_Values.updated_at = DateTime.Now;
            //check_List_Values.user_id = null;
            //check_List_Values.version = 865;
            //check_List_Values.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.check_list_values.Add(check_List_Values);
            //DbContext.SaveChanges();

            #endregion

            #region Field Values
            #region fv1
            field_values field_Values1 = CreateFieldValues(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
            //    new field_values();
            //field_Values1.case_id = aCase.id;
            //field_Values1.check_list = cl2;
            //field_Values1.check_list_id = cl2.id;
            //field_Values1.created_at = DateTime.Now;
            //field_Values1.date = DateTime.Now;
            //field_Values1.done_at = DateTime.Now;
            //field_Values1.field = f1;
            //field_Values1.field_id = f1.id;
            //field_Values1.updated_at = DateTime.Now;
            //field_Values1.user_id = null;
            //field_Values1.value = "tomt1";
            //field_Values1.version = 61234;
            //field_Values1.worker = worker;
            //field_Values1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values1);
            //DbContext.SaveChanges();
            #endregion

            #region fv2
            field_values field_Values2 = CreateFieldValues(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
            //    new field_values();
            //field_Values2.case_id = aCase.id;
            //field_Values2.check_list = cl2;
            //field_Values2.check_list_id = cl2.id;
            //field_Values2.created_at = DateTime.Now;
            //field_Values2.date = DateTime.Now;
            //field_Values2.done_at = DateTime.Now;
            //field_Values2.field = f2;
            //field_Values2.field_id = f2.id;
            //field_Values2.updated_at = DateTime.Now;
            //field_Values2.user_id = null;
            //field_Values2.value = "tomt2";
            //field_Values2.version = 61234;
            //field_Values2.worker = worker;
            //field_Values2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values2);
            //DbContext.SaveChanges();
            #endregion

            #region fv3
            field_values field_Values3 = CreateFieldValues(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
            //    new field_values();
            //field_Values3.case_id = aCase.id;
            //field_Values3.check_list = cl2;
            //field_Values3.check_list_id = cl2.id;
            //field_Values3.created_at = DateTime.Now;
            //field_Values3.date = DateTime.Now;
            //field_Values3.done_at = DateTime.Now;
            //field_Values3.field = f3;
            //field_Values3.field_id = f3.id;
            //field_Values3.updated_at = DateTime.Now;
            //field_Values3.user_id = null;
            //field_Values3.value = "tomt3";
            //field_Values3.version = 61234;
            //field_Values3.worker = worker;
            //field_Values3.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values3);
            //DbContext.SaveChanges();
            #endregion

            #region fv4
            field_values field_Values4 = CreateFieldValues(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
            //    new field_values();
            //field_Values4.case_id = aCase.id;
            //field_Values4.check_list = cl2;
            //field_Values4.check_list_id = cl2.id;
            //field_Values4.created_at = DateTime.Now;
            //field_Values4.date = DateTime.Now;
            //field_Values4.done_at = DateTime.Now;
            //field_Values4.field = f4;
            //field_Values4.field_id = f4.id;
            //field_Values4.updated_at = DateTime.Now;
            //field_Values4.user_id = null;
            //field_Values4.value = "tomt4";
            //field_Values4.version = 61234;
            //field_Values4.worker = worker;
            //field_Values4.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values4);
            //DbContext.SaveChanges();
            #endregion

            #region fv5
            field_values field_Values5 = CreateFieldValues(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
            //    new field_values();
            //field_Values5.case_id = aCase.id;
            //field_Values5.check_list = cl2;
            //field_Values5.check_list_id = cl2.id;
            //field_Values5.created_at = DateTime.Now;
            //field_Values5.date = DateTime.Now;
            //field_Values5.done_at = DateTime.Now;
            //field_Values5.field = f5;
            //field_Values5.field_id = f5.id;
            //field_Values5.updated_at = DateTime.Now;
            //field_Values5.user_id = null;
            //field_Values5.value = "tomt5";
            //field_Values5.version = 61234;
            //field_Values5.worker = worker;
            //field_Values5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values5);
            //DbContext.SaveChanges();
            #endregion


            #endregion

            #endregion
            // Act

            ReplyElement match = sut.CheckRead(aCase.microting_uid, aCase.microting_check_uid);

            // Assert
            #region Assert

            Assert.AreEqual(1, match.ElementList.Count());
            CheckListValue clv = (CheckListValue)match.ElementList[0];
            Assert.AreEqual(5, clv.DataItemList.Count);
            #region casts
            Field _f1 = (Field)clv.DataItemList[0];
            Field _f2 = (Field)clv.DataItemList[1];
            Field _f3 = (Field)clv.DataItemList[2];
            Field _f4 = (Field)clv.DataItemList[3];
            Field _f5 = (Field)clv.DataItemList[4];

          
            #endregion
            
            #region Barcode
            Assert.AreEqual(f1.barcode_enabled, 1);
            Assert.AreEqual(f2.barcode_enabled, 1);
            Assert.AreEqual(f3.barcode_enabled, 0);
            Assert.AreEqual(f4.barcode_enabled, 1);
            Assert.AreEqual(f5.barcode_enabled, 0);

            Assert.AreEqual(f1.barcode_type, "barcode");
            Assert.AreEqual(f2.barcode_type, "barcode");
            Assert.AreEqual(f3.barcode_type, "barcode");
            Assert.AreEqual(f4.barcode_type, "barcode");
            Assert.AreEqual(f5.barcode_type, "barcode");
            #endregion

            #region chckl_id

            Assert.AreEqual(f1.check_list_id, cl2.id);
            Assert.AreEqual(f2.check_list_id, cl2.id);
            Assert.AreEqual(f3.check_list_id, cl2.id);
            Assert.AreEqual(f4.check_list_id, cl2.id);
            Assert.AreEqual(f5.check_list_id, cl2.id);


            #endregion

            #region Color
            Assert.AreEqual(f1.color, _f1.FieldValues[0].Color);
            Assert.AreEqual(f2.color, _f2.FieldValues[0].Color);
            Assert.AreEqual(f3.color, _f3.FieldValues[0].Color);
            Assert.AreEqual(f4.color, _f4.FieldValues[0].Color);
            Assert.AreEqual(f5.color, _f5.FieldValues[0].Color);
            #endregion
            
            #region custom
            //  Assert.AreEqual(f1.custom, _f1.FieldValues[0].Id);
            #endregion
            
            #region Decimal_Count
            Assert.AreEqual(f1.decimal_count, null);
            Assert.AreEqual(f2.decimal_count, null);
            Assert.AreEqual(f3.decimal_count, 3);
            Assert.AreEqual(f4.decimal_count, null);
            Assert.AreEqual(f5.decimal_count, null);

            #endregion

            #region Default_value
            Assert.AreEqual(f1.default_value, "");
            Assert.AreEqual(f2.default_value, "");
            Assert.AreEqual(f3.default_value, "");
            Assert.AreEqual(f4.default_value, "");
            Assert.AreEqual(f5.default_value, "");
            #endregion

            #region Description
            CDataValue f1desc = (CDataValue)_f1.Description;
            CDataValue f2desc = (CDataValue)_f2.Description;
            CDataValue f3desc = (CDataValue)_f3.Description;
            CDataValue f4desc = (CDataValue)_f4.Description;
            CDataValue f5desc = (CDataValue)_f5.Description;

            Assert.AreEqual(f1.description, f1desc.InderValue);
            Assert.AreEqual(f2.description, f2desc.InderValue);
            Assert.AreEqual(f3.description, f3desc.InderValue);
            Assert.AreEqual(f4.description, f4desc.InderValue);
            Assert.AreEqual(f5.description, f5desc.InderValue);
            #endregion

            #region Displayindex
            Assert.AreEqual(f1.display_index, _f1.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f2.display_index, _f2.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f3.display_index, _f3.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f4.display_index, _f4.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f5.display_index, _f5.FieldValues[0].DisplayOrder);
            #endregion

            #region Dummy
            Assert.AreEqual(f1.dummy, 1);
            Assert.AreEqual(f2.dummy, 1);
            Assert.AreEqual(f3.dummy, 0);
            Assert.AreEqual(f4.dummy, 0);
            Assert.AreEqual(f5.dummy, 0);
            #endregion

            #region geolocation
            #region enabled
            Assert.AreEqual(f1.geolocation_enabled, 0);
            Assert.AreEqual(f2.geolocation_enabled, 0);
            Assert.AreEqual(f3.geolocation_enabled, 0);
            Assert.AreEqual(f4.geolocation_enabled, 0);
            Assert.AreEqual(f5.geolocation_enabled, 1);
            #endregion
            #region forced
            Assert.AreEqual(f1.geolocation_forced, 0);
            Assert.AreEqual(f2.geolocation_forced, 1);
            Assert.AreEqual(f3.geolocation_forced, 0);
            Assert.AreEqual(f4.geolocation_forced, 0);
            Assert.AreEqual(f5.geolocation_forced, 0);
            #endregion
            #region hidden
            Assert.AreEqual(f1.geolocation_hidden, 1);
            Assert.AreEqual(f2.geolocation_hidden, 0);
            Assert.AreEqual(f3.geolocation_hidden, 1);
            Assert.AreEqual(f4.geolocation_hidden, 1);
            Assert.AreEqual(f5.geolocation_hidden, 1);
            #endregion

            #endregion

            #region isNum
            Assert.AreEqual(f1.is_num, 0);
            Assert.AreEqual(f2.is_num, 0);
            Assert.AreEqual(f3.is_num, 0);
            Assert.AreEqual(f4.is_num, 0);
            Assert.AreEqual(f5.is_num, 0);


            #endregion

            #region Label
            Assert.AreEqual(f1.label, _f1.Label);
            Assert.AreEqual(f2.label, _f2.Label);
            Assert.AreEqual(f3.label, _f3.Label);
            Assert.AreEqual(f4.label, _f4.Label);
            Assert.AreEqual(f5.label, _f5.Label);
            #endregion

            #region Mandatory
            Assert.AreEqual(f1.mandatory, 1);
            Assert.AreEqual(f2.mandatory, 0);
            Assert.AreEqual(f3.mandatory, 1);
            Assert.AreEqual(f4.mandatory, 1);
            Assert.AreEqual(f5.mandatory, 1);
            #endregion

            #region maxLength
            Assert.AreEqual(f1.max_length, 55);
            Assert.AreEqual(f2.max_length, 5);
            Assert.AreEqual(f3.max_length, 8);
            Assert.AreEqual(f4.max_length, 666);
            Assert.AreEqual(f5.max_length, 69);

            #endregion

            #region min/max_Value
            #region max
            Assert.AreEqual(f1.max_value, "55");
            Assert.AreEqual(f2.max_value, "5");
            Assert.AreEqual(f3.max_value, "4865");
            Assert.AreEqual(f4.max_value, "41153");
            Assert.AreEqual(f5.max_value, "69");
            #endregion
            #region min
            Assert.AreEqual(f1.min_value, "0");
            Assert.AreEqual(f2.min_value, "0");
            Assert.AreEqual(f3.min_value, "0");
            Assert.AreEqual(f4.min_value, "0");
            Assert.AreEqual(f5.min_value, "1");
            #endregion
            #endregion

            #region Multi
            Assert.AreEqual(f1.multi, 0);
            Assert.AreEqual(f2.multi, 0);
            Assert.AreEqual(f3.multi, 0);
            Assert.AreEqual(f4.multi, 0);
            Assert.AreEqual(f5.multi, 0);
            #endregion

            #region Optional
            Assert.AreEqual(f1.optional, 0);
            Assert.AreEqual(f2.optional, 0);
            Assert.AreEqual(f3.optional, 1);
            Assert.AreEqual(f4.optional, 1);
            Assert.AreEqual(f5.optional, 1);

            #endregion

            #region Query_Type
            Assert.AreEqual(f1.query_type, null);
            Assert.AreEqual(f2.query_type, null);
            Assert.AreEqual(f3.query_type, null);
            Assert.AreEqual(f4.query_type, null);
            Assert.AreEqual(f5.query_type, null);

            #endregion

            #region Read_Only
            Assert.AreEqual(f1.read_only, 1);
            Assert.AreEqual(f2.read_only, 0);
            Assert.AreEqual(f3.read_only, 1);
            Assert.AreEqual(f4.read_only, 0);
            Assert.AreEqual(f5.read_only, 0);
            #endregion

            #region Selected
            Assert.AreEqual(f1.selected, 0);
            Assert.AreEqual(f2.selected, 0);
            Assert.AreEqual(f3.selected, 0);
            Assert.AreEqual(f4.selected, 1);
            Assert.AreEqual(f5.selected, 1);
            #endregion

            #region Split_Screen
            Assert.AreEqual(f1.split_screen, 0);
            Assert.AreEqual(f2.split_screen, 0);
            Assert.AreEqual(f3.split_screen, 0);
            Assert.AreEqual(f4.split_screen, 0);
            Assert.AreEqual(f5.split_screen, 0);

            #endregion

            #region Stop_On_Save
            Assert.AreEqual(f1.stop_on_save, 0);
            Assert.AreEqual(f2.stop_on_save, 0);
            Assert.AreEqual(f3.stop_on_save, 0);
            Assert.AreEqual(f4.stop_on_save, 0);
            Assert.AreEqual(f5.stop_on_save, 0);
            #endregion

            #region Unit_Name
            Assert.AreEqual(f1.unit_name, "");
            Assert.AreEqual(f2.unit_name, "");
            Assert.AreEqual(f3.unit_name, "");
            Assert.AreEqual(f4.unit_name, "");
            Assert.AreEqual(f5.unit_name, "");
            #endregion

            #region Values

            Assert.AreEqual(1, _f1.FieldValues.Count());
            Assert.AreEqual(1, _f2.FieldValues.Count());
            Assert.AreEqual(1, _f3.FieldValues.Count());
            Assert.AreEqual(1, _f4.FieldValues.Count());
            Assert.AreEqual(1, _f5.FieldValues.Count());

            Assert.AreEqual(field_Values1.value, _f1.FieldValues[0].Value);
            Assert.AreEqual(field_Values2.value, _f2.FieldValues[0].Value);
            Assert.AreEqual(field_Values3.value, _f3.FieldValues[0].Value);
            Assert.AreEqual(field_Values4.value, _f4.FieldValues[0].Value);
            Assert.AreEqual(field_Values5.value, _f5.FieldValues[0].Value);
            #endregion

            #region Version
            Assert.AreEqual(f1.version, 49);
            Assert.AreEqual(f2.version, 9);
            Assert.AreEqual(f3.version, 1);
            Assert.AreEqual(f4.version, 1);
            Assert.AreEqual(f5.version, 1);
            #endregion
            
            #endregion

        }
        //[Test]
        //public void SQL_Check_SubChecks_ReturnsCheckListValue()
        //{
        //    // Arrance
        //    #region Template1
        //    check_lists cl1 = new check_lists();
        //    cl1.created_at = DateTime.Now;
        //    cl1.updated_at = DateTime.Now;
        //    cl1.label = "A";
        //    cl1.description = "D";
        //    cl1.workflow_state = Constants.WorkflowStates.Created;
        //    cl1.case_type = "CheckList";
        //    cl1.folder_name = "Template1FolderName";
        //    cl1.display_index = 1;
        //    cl1.repeated = 1;

        //    DbContext.check_lists.Add(cl1);
        //    DbContext.SaveChanges();
        //    #endregion

        //    #region SubTemplate1
        //    check_lists cl2 = new check_lists();
        //    cl2.created_at = DateTime.Now;
        //    cl2.updated_at = DateTime.Now;
        //    cl2.label = "A.1";
        //    cl2.description = "D.1";
        //    cl2.workflow_state = Constants.WorkflowStates.Created;
        //    cl2.case_type = "CheckList";
        //    cl2.display_index = 1;
        //    cl2.repeated = 1;
        //    cl2.parent_id = cl1.id;

        //    DbContext.check_lists.Add(cl2);
        //    DbContext.SaveChanges();

        //    #endregion

        //    #region Fields
        //    #region field1


        //    fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
        //        5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
        //        0, 0, "", 49);
        //    //    new fields();
        //    //field_types ft1 = DbContext.field_types.Where(x => x.field_type == "comment").First();
        //    //f1.field_type = ft1;

        //    //f1.barcode_enabled = 1;
        //    //f1.barcode_type = "barcode";
        //    //f1.check_list_id = cl2.id;
        //    //f1.color = "e2f4fb";
        //    //f1.created_at = DateTime.Now;
        //    //f1.custom = "custom";
        //    //f1.decimal_count = null;
        //    //f1.default_value = "";
        //    //f1.description = "Comment field Description";
        //    //f1.display_index = 5;
        //    //f1.dummy = 1;
        //    //f1.geolocation_enabled = 0;
        //    //f1.geolocation_forced = 0;
        //    //f1.geolocation_hidden = 1;
        //    //f1.is_num = 0;
        //    //f1.label = "Comment field";
        //    //f1.mandatory = 1;
        //    //f1.max_length = 55;
        //    //f1.max_value = "55";
        //    //f1.min_value = "0";
        //    //f1.multi = 0;
        //    //f1.optional = 0;
        //    //f1.query_type = null;
        //    //f1.read_only = 1;
        //    //f1.selected = 0;
        //    //f1.split_screen = 0;
        //    //f1.stop_on_save = 0;
        //    //f1.unit_name = "";
        //    //f1.updated_at = DateTime.Now;
        //    //f1.version = 49;
        //    //f1.workflow_state = Constants.WorkflowStates.Created;

        //    //DbContext.fields.Add(f1);
        //    //DbContext.SaveChanges();
        //    //Thread.Sleep(2000);
        //    #endregion

        //    #region field2


        //    fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
        //        45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
        //        "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
        //    //    new fields();
        //    //field_types ft2 = DbContext.field_types.Where(x => x.field_type == "comment").First();
        //    //f2.field_type = ft2;

        //    //f2.barcode_enabled = 1;
        //    //f2.barcode_type = "barcode";
        //    //f2.check_list_id = cl2.id;
        //    //f2.color = "f5eafa";
        //    //f2.default_value = "";
        //    //f2.description = "showPDf Description";
        //    //f2.display_index = 45;
        //    //f2.dummy = 1;
        //    //f2.geolocation_enabled = 0;
        //    //f2.geolocation_forced = 1;
        //    //f2.geolocation_hidden = 0;
        //    //f2.is_num = 0;
        //    //f2.label = "ShowPdf";
        //    //f2.mandatory = 0;
        //    //f2.max_length = 5;
        //    //f2.max_value = "5";
        //    //f2.min_value = "0";
        //    //f2.multi = 0;
        //    //f2.optional = 0;
        //    //f2.query_type = null;
        //    //f2.read_only = 0;
        //    //f2.selected = 0;
        //    //f2.split_screen = 0;
        //    //f2.stop_on_save = 0;
        //    //f2.unit_name = "";
        //    //f2.updated_at = DateTime.Now;
        //    //f2.version = 9;
        //    //f2.workflow_state = Constants.WorkflowStates.Created;

        //    //DbContext.fields.Add(f2);
        //    //DbContext.SaveChanges();
        //    //Thread.Sleep(2000);

        //    #endregion

        //    #region field3

        //    fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
        //        83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
        //        "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
        //    //    new fields();
        //    //field_types ft3 = DbContext.field_types.Where(x => x.field_type == "number").First();

        //    //f3.field_type = ft3;

        //    //f3.barcode_enabled = 0;
        //    //f3.barcode_type = "barcode";
        //    //f3.check_list_id = cl2.id;
        //    //f3.color = "f0f8db";
        //    //f3.created_at = DateTime.Now;
        //    //f3.custom = "custom";
        //    //f3.decimal_count = 3;
        //    //f3.default_value = "";
        //    //f3.description = "Number Field Description";
        //    //f3.display_index = 83;
        //    //f3.dummy = 0;
        //    //f3.geolocation_enabled = 0;
        //    //f3.geolocation_forced = 0;
        //    //f3.geolocation_hidden = 1;
        //    //f3.is_num = 0;
        //    //f3.label = "Numberfield";
        //    //f3.mandatory = 1;
        //    //f3.max_length = 8;
        //    //f3.max_value = "4865";
        //    //f3.min_value = "0";
        //    //f3.multi = 0;
        //    //f3.optional = 1;
        //    //f3.query_type = null;
        //    //f3.read_only = 1;
        //    //f3.selected = 0;
        //    //f3.split_screen = 0;
        //    //f3.stop_on_save = 0;
        //    //f3.unit_name = "";
        //    //f3.updated_at = DateTime.Now;
        //    //f3.version = 1;
        //    //f3.workflow_state = Constants.WorkflowStates.Created;



        //    //DbContext.fields.Add(f3);
        //    //DbContext.SaveChanges();
        //    //Thread.Sleep(2000);

        //    #endregion

        //    #region field4


        //    fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
        //        84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
        //        "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
        //    //    new fields();
        //    //field_types ft4 = DbContext.field_types.Where(x => x.field_type == "comment").First();
        //    //f4.field_type = ft4;

        //    //f4.barcode_enabled = 1;
        //    //f4.barcode_type = "barcode";
        //    //f4.check_list_id = cl2.id;
        //    //f4.color = "fff6df";
        //    //f4.created_at = DateTime.Now;
        //    //f4.custom = "custom";
        //    //f4.decimal_count = null;
        //    //f4.default_value = "";
        //    //f4.description = "date Description";
        //    //f4.display_index = 84;
        //    //f4.dummy = 0;
        //    //f4.geolocation_enabled = 0;
        //    //f4.geolocation_forced = 0;
        //    //f4.geolocation_hidden = 1;
        //    //f4.is_num = 0;
        //    //f4.label = "Date";
        //    //f4.mandatory = 1;
        //    //f4.max_length = 666;
        //    //f4.max_value = "41153";
        //    //f4.min_value = "0";
        //    //f4.multi = 0;
        //    //f4.optional = 1;
        //    //f4.query_type = null;
        //    //f4.read_only = 0;
        //    //f4.selected = 1;
        //    //f4.split_screen = 0;
        //    //f4.stop_on_save = 0;
        //    //f4.unit_name = "";
        //    //f4.updated_at = DateTime.Now;
        //    //f4.version = 1;
        //    //f4.workflow_state = Constants.WorkflowStates.Created;


        //    //DbContext.fields.Add(f4);
        //    //DbContext.SaveChanges();
        //    //Thread.Sleep(2000);

        //    #endregion

        //    #region field5

        //    fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
        //        85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
        //        "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
        //    //    new fields();
        //    //field_types ft5 = DbContext.field_types.Where(x => x.field_type == "comment").First();

        //    //f5.field_type = ft5;
        //    //f5.barcode_enabled = 0;
        //    //f5.barcode_type = "barcode";
        //    //f5.check_list_id = cl2.id;
        //    //f5.color = "ffe4e4";
        //    //f5.created_at = DateTime.Now;
        //    //f5.custom = "custom";
        //    //f5.decimal_count = null;
        //    //f5.default_value = "";
        //    //f5.description = "picture Description";
        //    //f5.display_index = 85;
        //    //f5.dummy = 0;
        //    //f5.geolocation_enabled = 1;
        //    //f5.geolocation_forced = 0;
        //    //f5.geolocation_hidden = 1;
        //    //f5.is_num = 0;
        //    //f5.label = "Picture";
        //    //f5.mandatory = 1;
        //    //f5.max_length = 69;
        //    //f5.max_value = "69";
        //    //f5.min_value = "1";
        //    //f5.multi = 0;
        //    //f5.optional = 1;
        //    //f5.query_type = null;
        //    //f5.read_only = 0;
        //    //f5.selected = 1;
        //    //f5.split_screen = 0;
        //    //f5.stop_on_save = 0;
        //    //f5.unit_name = "";
        //    //f5.updated_at = DateTime.Now;
        //    //f5.version = 1;
        //    //f5.workflow_state = Constants.WorkflowStates.Created;

        //    //DbContext.fields.Add(f5);
        //    //DbContext.SaveChanges();
        //    //Thread.Sleep(2000);

        //    #endregion


        //    #endregion


        //    // Act



        //    // Assert
        //} //private method
        [Test]
        public void SQL_Check_ChecksRead_ReturnsListOfFieldValues()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);
            //    new check_lists();
            //cl1.created_at = DateTime.Now;
            //cl1.updated_at = DateTime.Now;
            //cl1.label = "A";
            //cl1.description = "D";
            //cl1.workflow_state = Constants.WorkflowStates.Created;
            //cl1.case_type = "CheckList";
            //cl1.folder_name = "Template1FolderName";
            //cl1.display_index = 1;
            //cl1.repeated = 1;

            //DbContext.check_lists.Add(cl1);
            //DbContext.SaveChanges();
            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
            //    new check_lists();
            //cl2.created_at = DateTime.Now;
            //cl2.updated_at = DateTime.Now;
            //cl2.label = "A.1";
            //cl2.description = "D.1";
            //cl2.workflow_state = Constants.WorkflowStates.Created;
            //cl2.case_type = "CheckList";
            //cl2.display_index = 1;
            //cl2.repeated = 1;
            //cl2.parent_id = cl1.id;

            //DbContext.check_lists.Add(cl2);
            //DbContext.SaveChanges();

            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
            //    new fields();
            //field_types ft1 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f1.field_type = ft1;

            //f1.barcode_enabled = 1;
            //f1.barcode_type = "barcode";
            //f1.check_list_id = cl2.id;
            //f1.color = "e2f4fb";
            //f1.created_at = DateTime.Now;
            //f1.custom = "custom";
            //f1.decimal_count = null;
            //f1.default_value = "";
            //f1.description = "Comment field Description";
            //f1.display_index = 5;
            //f1.dummy = 1;
            //f1.geolocation_enabled = 0;
            //f1.geolocation_forced = 0;
            //f1.geolocation_hidden = 1;
            //f1.is_num = 0;
            //f1.label = "Comment field";
            //f1.mandatory = 1;
            //f1.max_length = 55;
            //f1.max_value = "55";
            //f1.min_value = "0";
            //f1.multi = 0;
            //f1.optional = 0;
            //f1.query_type = null;
            //f1.read_only = 1;
            //f1.selected = 0;
            //f1.split_screen = 0;
            //f1.stop_on_save = 0;
            //f1.unit_name = "";
            //f1.updated_at = DateTime.Now;
            //f1.version = 49;
            //f1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f1);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);
            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
            //    new fields();
            //field_types ft2 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f2.field_type = ft2;

            //f2.barcode_enabled = 1;
            //f2.barcode_type = "barcode";
            //f2.check_list_id = cl2.id;
            //f2.color = "f5eafa";
            //f2.default_value = "";
            //f2.description = "showPDf Description";
            //f2.display_index = 45;
            //f2.dummy = 1;
            //f2.geolocation_enabled = 0;
            //f2.geolocation_forced = 1;
            //f2.geolocation_hidden = 0;
            //f2.is_num = 0;
            //f2.label = "ShowPdf";
            //f2.mandatory = 0;
            //f2.max_length = 5;
            //f2.max_value = "5";
            //f2.min_value = "0";
            //f2.multi = 0;
            //f2.optional = 0;
            //f2.query_type = null;
            //f2.read_only = 0;
            //f2.selected = 0;
            //f2.split_screen = 0;
            //f2.stop_on_save = 0;
            //f2.unit_name = "";
            //f2.updated_at = DateTime.Now;
            //f2.version = 9;
            //f2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f2);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
            //    new fields();
            //field_types ft3 = DbContext.field_types.Where(x => x.field_type == "number").First();

            //f3.field_type = ft3;

            //f3.barcode_enabled = 0;
            //f3.barcode_type = "barcode";
            //f3.check_list_id = cl2.id;
            //f3.color = "f0f8db";
            //f3.created_at = DateTime.Now;
            //f3.custom = "custom";
            //f3.decimal_count = 3;
            //f3.default_value = "";
            //f3.description = "Number Field Description";
            //f3.display_index = 83;
            //f3.dummy = 0;
            //f3.geolocation_enabled = 0;
            //f3.geolocation_forced = 0;
            //f3.geolocation_hidden = 1;
            //f3.is_num = 0;
            //f3.label = "Numberfield";
            //f3.mandatory = 1;
            //f3.max_length = 8;
            //f3.max_value = "4865";
            //f3.min_value = "0";
            //f3.multi = 0;
            //f3.optional = 1;
            //f3.query_type = null;
            //f3.read_only = 1;
            //f3.selected = 0;
            //f3.split_screen = 0;
            //f3.stop_on_save = 0;
            //f3.unit_name = "";
            //f3.updated_at = DateTime.Now;
            //f3.version = 1;
            //f3.workflow_state = Constants.WorkflowStates.Created;



            //DbContext.fields.Add(f3);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft4 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f4.field_type = ft4;

            //f4.barcode_enabled = 1;
            //f4.barcode_type = "barcode";
            //f4.check_list_id = cl2.id;
            //f4.color = "fff6df";
            //f4.created_at = DateTime.Now;
            //f4.custom = "custom";
            //f4.decimal_count = null;
            //f4.default_value = "";
            //f4.description = "date Description";
            //f4.display_index = 84;
            //f4.dummy = 0;
            //f4.geolocation_enabled = 0;
            //f4.geolocation_forced = 0;
            //f4.geolocation_hidden = 1;
            //f4.is_num = 0;
            //f4.label = "Date";
            //f4.mandatory = 1;
            //f4.max_length = 666;
            //f4.max_value = "41153";
            //f4.min_value = "0";
            //f4.multi = 0;
            //f4.optional = 1;
            //f4.query_type = null;
            //f4.read_only = 0;
            //f4.selected = 1;
            //f4.split_screen = 0;
            //f4.stop_on_save = 0;
            //f4.unit_name = "";
            //f4.updated_at = DateTime.Now;
            //f4.version = 1;
            //f4.workflow_state = Constants.WorkflowStates.Created;


            //DbContext.fields.Add(f4);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft5 = DbContext.field_types.Where(x => x.field_type == "comment").First();

            //f5.field_type = ft5;
            //f5.barcode_enabled = 0;
            //f5.barcode_type = "barcode";
            //f5.check_list_id = cl2.id;
            //f5.color = "ffe4e4";
            //f5.created_at = DateTime.Now;
            //f5.custom = "custom";
            //f5.decimal_count = null;
            //f5.default_value = "";
            //f5.description = "picture Description";
            //f5.display_index = 85;
            //f5.dummy = 0;
            //f5.geolocation_enabled = 1;
            //f5.geolocation_forced = 0;
            //f5.geolocation_hidden = 1;
            //f5.is_num = 0;
            //f5.label = "Picture";
            //f5.mandatory = 1;
            //f5.max_length = 69;
            //f5.max_value = "69";
            //f5.min_value = "1";
            //f5.multi = 0;
            //f5.optional = 1;
            //f5.query_type = null;
            //f5.read_only = 0;
            //f5.selected = 1;
            //f5.split_screen = 0;
            //f5.stop_on_save = 0;
            //f5.unit_name = "";
            //f5.updated_at = DateTime.Now;
            //f5.version = 1;
            //f5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f5);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            //= new workers();
            //worker.first_name = "Arne";
            //worker.last_name = "Jensen";
            //worker.email = "aa@tak.dk";
            //worker.created_at = DateTime.Now;
            //worker.updated_at = DateTime.Now;
            //worker.microting_uid = 21;
            //worker.workflow_state = Constants.WorkflowStates.Created;
            //worker.version = 69;
            //DbContext.workers.Add(worker);
            //DbContext.SaveChanges();
            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);
            //    new sites();
            //site.name = "SiteName";
            //site.microting_uid = 88;
            //site.updated_at = DateTime.Now;
            //site.created_at = DateTime.Now;
            //site.version = 64;
            //site.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.sites.Add(site);
            //DbContext.SaveChanges();
            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);
            //    new units();
            //unit.microting_uid = 48;
            //unit.otp_code = 49;
            //unit.site = site;
            //unit.site_id = site.id;
            //unit.created_at = DateTime.Now;
            //unit.customer_no = 348;
            //unit.updated_at = DateTime.Now;
            //unit.version = 9;
            //unit.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.units.Add(unit);
            //DbContext.SaveChanges();
            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);
            //    new site_workers();
            //site_workers.created_at = DateTime.Now;
            //site_workers.microting_uid = 55;
            //site_workers.updated_at = DateTime.Now;
            //site_workers.version = 63;
            //site_workers.site = site;
            //site_workers.site_id = site.id;
            //site_workers.worker = worker;
            //site_workers.worker_id = worker.id;
            //site_workers.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.site_workers.Add(site_workers);
            //DbContext.SaveChanges();
            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);
            //    new cases();
            //aCase.case_uid = caseUId;
            //aCase.check_list = cl1;
            //aCase.check_list_id = cl1.id;
            //aCase.created_at = DateTime.Now;
            //aCase.custom = custom;
            //aCase.done_at = DateTime.Now;
            //aCase.done_by_user_id = worker.id;
            //aCase.microting_check_uid = microtingCheckId;
            //aCase.microting_uid = microtingUId;
            //aCase.site = site;
            //aCase.site_id = site.id;
            //aCase.status = 66;
            //aCase.type = caseType;
            //aCase.unit = unit;
            //aCase.unit_id = unit.id;
            //aCase.updated_at = DateTime.Now;
            //aCase.version = 1;
            //aCase.worker = worker;
            //aCase.workflow_state = Constants.WorkflowStates.Created;       
            //DbContext.cases.Add(aCase);
            //DbContext.SaveChanges();
            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);
            //    new check_list_values();

            //check_List_Values.case_id = aCase.id;
            //check_List_Values.check_list_id = cl2.id;
            //check_List_Values.created_at = DateTime.Now;
            //check_List_Values.status = "completed";
            //check_List_Values.updated_at = DateTime.Now;
            //check_List_Values.user_id = null;
            //check_List_Values.version = 865;
            //check_List_Values.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.check_list_values.Add(check_List_Values);
            //DbContext.SaveChanges();

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
            //    new field_values();
            //field_Values1.case_id = aCase.id;
            //field_Values1.check_list = cl2;
            //field_Values1.check_list_id = cl2.id;
            //field_Values1.created_at = DateTime.Now;
            //field_Values1.date = DateTime.Now;
            //field_Values1.done_at = DateTime.Now;
            //field_Values1.field = f1;
            //field_Values1.field_id = f1.id;
            //field_Values1.updated_at = DateTime.Now;
            //field_Values1.user_id = null;
            //field_Values1.value = "tomt1";
            //field_Values1.version = 61234;
            //field_Values1.worker = worker;
            //field_Values1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values1);
            //DbContext.SaveChanges();
            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
            //    new field_values();
            //field_Values2.case_id = aCase.id;
            //field_Values2.check_list = cl2;
            //field_Values2.check_list_id = cl2.id;
            //field_Values2.created_at = DateTime.Now;
            //field_Values2.date = DateTime.Now;
            //field_Values2.done_at = DateTime.Now;
            //field_Values2.field = f2;
            //field_Values2.field_id = f2.id;
            //field_Values2.updated_at = DateTime.Now;
            //field_Values2.user_id = null;
            //field_Values2.value = "tomt2";
            //field_Values2.version = 61234;
            //field_Values2.worker = worker;
            //field_Values2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values2);
            //DbContext.SaveChanges();
            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
            //    new field_values();
            //field_Values3.case_id = aCase.id;
            //field_Values3.check_list = cl2;
            //field_Values3.check_list_id = cl2.id;
            //field_Values3.created_at = DateTime.Now;
            //field_Values3.date = DateTime.Now;
            //field_Values3.done_at = DateTime.Now;
            //field_Values3.field = f3;
            //field_Values3.field_id = f3.id;
            //field_Values3.updated_at = DateTime.Now;
            //field_Values3.user_id = null;
            //field_Values3.value = "tomt3";
            //field_Values3.version = 61234;
            //field_Values3.worker = worker;
            //field_Values3.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values3);
            //DbContext.SaveChanges();
            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
            //    new field_values();
            //field_Values4.case_id = aCase.id;
            //field_Values4.check_list = cl2;
            //field_Values4.check_list_id = cl2.id;
            //field_Values4.created_at = DateTime.Now;
            //field_Values4.date = DateTime.Now;
            //field_Values4.done_at = DateTime.Now;
            //field_Values4.field = f4;
            //field_Values4.field_id = f4.id;
            //field_Values4.updated_at = DateTime.Now;
            //field_Values4.user_id = null;
            //field_Values4.value = "tomt4";
            //field_Values4.version = 61234;
            //field_Values4.worker = worker;
            //field_Values4.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values4);
            //DbContext.SaveChanges();
            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
            //    new field_values();
            //field_Values5.case_id = aCase.id;
            //field_Values5.check_list = cl2;
            //field_Values5.check_list_id = cl2.id;
            //field_Values5.created_at = DateTime.Now;
            //field_Values5.date = DateTime.Now;
            //field_Values5.done_at = DateTime.Now;
            //field_Values5.field = f5;
            //field_Values5.field_id = f5.id;
            //field_Values5.updated_at = DateTime.Now;
            //field_Values5.user_id = null;
            //field_Values5.value = "tomt5";
            //field_Values5.version = 61234;
            //field_Values5.worker = worker;
            //field_Values5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values5);
            //DbContext.SaveChanges();
            #endregion


            #endregion
            #endregion
            // Act

            List<field_values> match = sut.ChecksRead(aCase.microting_uid, aCase.microting_check_uid);

            // Assert
           

            Assert.AreEqual(field_Value1.value, match[0].value);
            Assert.AreEqual(field_Value2.value, match[1].value);
            Assert.AreEqual(field_Value3.value, match[2].value);
            Assert.AreEqual(field_Value4.value, match[3].value);
            Assert.AreEqual(field_Value5.value, match[4].value);

            



        }
        [Test]
        public void SQL_Check_FieldRead_ReturnsField()
        {
            // Arrance

            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);
         
            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
        

            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
           
            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
            

            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
 

            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
            

            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            // Act

            Field match = sut.FieldRead(f1.id);

            // Assert

            Assert.AreEqual(f1.id, match.Id);
            

        }
        [Test]
        public void SQL_Check_FieldValueRead_ReturnsAnswer()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);
            //    new check_lists();
            //cl1.created_at = DateTime.Now;
            //cl1.updated_at = DateTime.Now;
            //cl1.label = "A";
            //cl1.description = "D";
            //cl1.workflow_state = Constants.WorkflowStates.Created;
            //cl1.case_type = "CheckList";
            //cl1.folder_name = "Template1FolderName";
            //cl1.display_index = 1;
            //cl1.repeated = 1;

            //DbContext.check_lists.Add(cl1);
            //DbContext.SaveChanges();
            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
            //    new check_lists();
            //cl2.created_at = DateTime.Now;
            //cl2.updated_at = DateTime.Now;
            //cl2.label = "A.1";
            //cl2.description = "D.1";
            //cl2.workflow_state = Constants.WorkflowStates.Created;
            //cl2.case_type = "CheckList";
            //cl2.display_index = 1;
            //cl2.repeated = 1;
            //cl2.parent_id = cl1.id;

            //DbContext.check_lists.Add(cl2);
            //DbContext.SaveChanges();

            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
            //    new fields();
            //field_types ft1 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f1.field_type = ft1;

            //f1.barcode_enabled = 1;
            //f1.barcode_type = "barcode";
            //f1.check_list_id = cl2.id;
            //f1.color = "e2f4fb";
            //f1.created_at = DateTime.Now;
            //f1.custom = "custom";
            //f1.decimal_count = null;
            //f1.default_value = "";
            //f1.description = "Comment field Description";
            //f1.display_index = 5;
            //f1.dummy = 1;
            //f1.geolocation_enabled = 0;
            //f1.geolocation_forced = 0;
            //f1.geolocation_hidden = 1;
            //f1.is_num = 0;
            //f1.label = "Comment field";
            //f1.mandatory = 1;
            //f1.max_length = 55;
            //f1.max_value = "55";
            //f1.min_value = "0";
            //f1.multi = 0;
            //f1.optional = 0;
            //f1.query_type = null;
            //f1.read_only = 1;
            //f1.selected = 0;
            //f1.split_screen = 0;
            //f1.stop_on_save = 0;
            //f1.unit_name = "";
            //f1.updated_at = DateTime.Now;
            //f1.version = 49;
            //f1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f1);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);
            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
            //    new fields();
            //field_types ft2 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f2.field_type = ft2;

            //f2.barcode_enabled = 1;
            //f2.barcode_type = "barcode";
            //f2.check_list_id = cl2.id;
            //f2.color = "f5eafa";
            //f2.default_value = "";
            //f2.description = "showPDf Description";
            //f2.display_index = 45;
            //f2.dummy = 1;
            //f2.geolocation_enabled = 0;
            //f2.geolocation_forced = 1;
            //f2.geolocation_hidden = 0;
            //f2.is_num = 0;
            //f2.label = "ShowPdf";
            //f2.mandatory = 0;
            //f2.max_length = 5;
            //f2.max_value = "5";
            //f2.min_value = "0";
            //f2.multi = 0;
            //f2.optional = 0;
            //f2.query_type = null;
            //f2.read_only = 0;
            //f2.selected = 0;
            //f2.split_screen = 0;
            //f2.stop_on_save = 0;
            //f2.unit_name = "";
            //f2.updated_at = DateTime.Now;
            //f2.version = 9;
            //f2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f2);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
            //    new fields();
            //field_types ft3 = DbContext.field_types.Where(x => x.field_type == "number").First();

            //f3.field_type = ft3;

            //f3.barcode_enabled = 0;
            //f3.barcode_type = "barcode";
            //f3.check_list_id = cl2.id;
            //f3.color = "f0f8db";
            //f3.created_at = DateTime.Now;
            //f3.custom = "custom";
            //f3.decimal_count = 3;
            //f3.default_value = "";
            //f3.description = "Number Field Description";
            //f3.display_index = 83;
            //f3.dummy = 0;
            //f3.geolocation_enabled = 0;
            //f3.geolocation_forced = 0;
            //f3.geolocation_hidden = 1;
            //f3.is_num = 0;
            //f3.label = "Numberfield";
            //f3.mandatory = 1;
            //f3.max_length = 8;
            //f3.max_value = "4865";
            //f3.min_value = "0";
            //f3.multi = 0;
            //f3.optional = 1;
            //f3.query_type = null;
            //f3.read_only = 1;
            //f3.selected = 0;
            //f3.split_screen = 0;
            //f3.stop_on_save = 0;
            //f3.unit_name = "";
            //f3.updated_at = DateTime.Now;
            //f3.version = 1;
            //f3.workflow_state = Constants.WorkflowStates.Created;



            //DbContext.fields.Add(f3);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft4 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f4.field_type = ft4;

            //f4.barcode_enabled = 1;
            //f4.barcode_type = "barcode";
            //f4.check_list_id = cl2.id;
            //f4.color = "fff6df";
            //f4.created_at = DateTime.Now;
            //f4.custom = "custom";
            //f4.decimal_count = null;
            //f4.default_value = "";
            //f4.description = "date Description";
            //f4.display_index = 84;
            //f4.dummy = 0;
            //f4.geolocation_enabled = 0;
            //f4.geolocation_forced = 0;
            //f4.geolocation_hidden = 1;
            //f4.is_num = 0;
            //f4.label = "Date";
            //f4.mandatory = 1;
            //f4.max_length = 666;
            //f4.max_value = "41153";
            //f4.min_value = "0";
            //f4.multi = 0;
            //f4.optional = 1;
            //f4.query_type = null;
            //f4.read_only = 0;
            //f4.selected = 1;
            //f4.split_screen = 0;
            //f4.stop_on_save = 0;
            //f4.unit_name = "";
            //f4.updated_at = DateTime.Now;
            //f4.version = 1;
            //f4.workflow_state = Constants.WorkflowStates.Created;


            //DbContext.fields.Add(f4);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft5 = DbContext.field_types.Where(x => x.field_type == "comment").First();

            //f5.field_type = ft5;
            //f5.barcode_enabled = 0;
            //f5.barcode_type = "barcode";
            //f5.check_list_id = cl2.id;
            //f5.color = "ffe4e4";
            //f5.created_at = DateTime.Now;
            //f5.custom = "custom";
            //f5.decimal_count = null;
            //f5.default_value = "";
            //f5.description = "picture Description";
            //f5.display_index = 85;
            //f5.dummy = 0;
            //f5.geolocation_enabled = 1;
            //f5.geolocation_forced = 0;
            //f5.geolocation_hidden = 1;
            //f5.is_num = 0;
            //f5.label = "Picture";
            //f5.mandatory = 1;
            //f5.max_length = 69;
            //f5.max_value = "69";
            //f5.min_value = "1";
            //f5.multi = 0;
            //f5.optional = 1;
            //f5.query_type = null;
            //f5.read_only = 0;
            //f5.selected = 1;
            //f5.split_screen = 0;
            //f5.stop_on_save = 0;
            //f5.unit_name = "";
            //f5.updated_at = DateTime.Now;
            //f5.version = 1;
            //f5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f5);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            //= new workers();
            //worker.first_name = "Arne";
            //worker.last_name = "Jensen";
            //worker.email = "aa@tak.dk";
            //worker.created_at = DateTime.Now;
            //worker.updated_at = DateTime.Now;
            //worker.microting_uid = 21;
            //worker.workflow_state = Constants.WorkflowStates.Created;
            //worker.version = 69;
            //DbContext.workers.Add(worker);
            //DbContext.SaveChanges();
            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);
            //    new sites();
            //site.name = "SiteName";
            //site.microting_uid = 88;
            //site.updated_at = DateTime.Now;
            //site.created_at = DateTime.Now;
            //site.version = 64;
            //site.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.sites.Add(site);
            //DbContext.SaveChanges();
            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);
            //    new units();
            //unit.microting_uid = 48;
            //unit.otp_code = 49;
            //unit.site = site;
            //unit.site_id = site.id;
            //unit.created_at = DateTime.Now;
            //unit.customer_no = 348;
            //unit.updated_at = DateTime.Now;
            //unit.version = 9;
            //unit.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.units.Add(unit);
            //DbContext.SaveChanges();
            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);
            //    new site_workers();
            //site_workers.created_at = DateTime.Now;
            //site_workers.microting_uid = 55;
            //site_workers.updated_at = DateTime.Now;
            //site_workers.version = 63;
            //site_workers.site = site;
            //site_workers.site_id = site.id;
            //site_workers.worker = worker;
            //site_workers.worker_id = worker.id;
            //site_workers.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.site_workers.Add(site_workers);
            //DbContext.SaveChanges();
            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);
            //    new cases();
            //aCase.case_uid = caseUId;
            //aCase.check_list = cl1;
            //aCase.check_list_id = cl1.id;
            //aCase.created_at = DateTime.Now;
            //aCase.custom = custom;
            //aCase.done_at = DateTime.Now;
            //aCase.done_by_user_id = worker.id;
            //aCase.microting_check_uid = microtingCheckId;
            //aCase.microting_uid = microtingUId;
            //aCase.site = site;
            //aCase.site_id = site.id;
            //aCase.status = 66;
            //aCase.type = caseType;
            //aCase.unit = unit;
            //aCase.unit_id = unit.id;
            //aCase.updated_at = DateTime.Now;
            //aCase.version = 1;
            //aCase.worker = worker;
            //aCase.workflow_state = Constants.WorkflowStates.Created;       
            //DbContext.cases.Add(aCase);
            //DbContext.SaveChanges();
            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);
            //    new check_list_values();

            //check_List_Values.case_id = aCase.id;
            //check_List_Values.check_list_id = cl2.id;
            //check_List_Values.created_at = DateTime.Now;
            //check_List_Values.status = "completed";
            //check_List_Values.updated_at = DateTime.Now;
            //check_List_Values.user_id = null;
            //check_List_Values.version = 865;
            //check_List_Values.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.check_list_values.Add(check_List_Values);
            //DbContext.SaveChanges();

            #endregion

            #region UploadedData
            uploaded_data ud = CreateUploadedData("checksum", "File1", "no", "mappe", "File1", 1, worker,
                "local", 55);

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, ud.id, null, "tomt1", 61234, worker);
  
            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
            //    new field_values();
            //field_Values2.case_id = aCase.id;
            //field_Values2.check_list = cl2;
            //field_Values2.check_list_id = cl2.id;
            //field_Values2.created_at = DateTime.Now;
            //field_Values2.date = DateTime.Now;
            //field_Values2.done_at = DateTime.Now;
            //field_Values2.field = f2;
            //field_Values2.field_id = f2.id;
            //field_Values2.updated_at = DateTime.Now;
            //field_Values2.user_id = null;
            //field_Values2.value = "tomt2";
            //field_Values2.version = 61234;
            //field_Values2.worker = worker;
            //field_Values2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values2);
            //DbContext.SaveChanges();
            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
            //    new field_values();
            //field_Values3.case_id = aCase.id;
            //field_Values3.check_list = cl2;
            //field_Values3.check_list_id = cl2.id;
            //field_Values3.created_at = DateTime.Now;
            //field_Values3.date = DateTime.Now;
            //field_Values3.done_at = DateTime.Now;
            //field_Values3.field = f3;
            //field_Values3.field_id = f3.id;
            //field_Values3.updated_at = DateTime.Now;
            //field_Values3.user_id = null;
            //field_Values3.value = "tomt3";
            //field_Values3.version = 61234;
            //field_Values3.worker = worker;
            //field_Values3.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values3);
            //DbContext.SaveChanges();
            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
            //    new field_values();
            //field_Values4.case_id = aCase.id;
            //field_Values4.check_list = cl2;
            //field_Values4.check_list_id = cl2.id;
            //field_Values4.created_at = DateTime.Now;
            //field_Values4.date = DateTime.Now;
            //field_Values4.done_at = DateTime.Now;
            //field_Values4.field = f4;
            //field_Values4.field_id = f4.id;
            //field_Values4.updated_at = DateTime.Now;
            //field_Values4.user_id = null;
            //field_Values4.value = "tomt4";
            //field_Values4.version = 61234;
            //field_Values4.worker = worker;
            //field_Values4.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values4);
            //DbContext.SaveChanges();
            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
            //    new field_values();
            //field_Values5.case_id = aCase.id;
            //field_Values5.check_list = cl2;
            //field_Values5.check_list_id = cl2.id;
            //field_Values5.created_at = DateTime.Now;
            //field_Values5.date = DateTime.Now;
            //field_Values5.done_at = DateTime.Now;
            //field_Values5.field = f5;
            //field_Values5.field_id = f5.id;
            //field_Values5.updated_at = DateTime.Now;
            //field_Values5.user_id = null;
            //field_Values5.value = "tomt5";
            //field_Values5.version = 61234;
            //field_Values5.worker = worker;
            //field_Values5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values5);
            //DbContext.SaveChanges();
            #endregion


            #endregion
            #endregion
            // Act

            var match = sut.FieldValueRead(f1, field_Value1, false);

            // Assert
            #region Assert
            Assert.True(match is FieldValue);
            Assert.AreEqual(field_Value1.accuracy, match.Accuracy);
            Assert.AreEqual(field_Value1.altitude, match.Altitude);
            //Assert.AreEqual(field_Value1.case_id, match.case_id);
            //Assert.AreEqual(field_Value1.check_list, match.check_list);
            //Assert.AreEqual(field_Value1.check_list_duplicate_id, match.check_list_duplicate_id);
            //Assert.AreEqual(field_Value1.check_list_id, match.check_list_id);
            //Assert.AreEqual(field_Value1.created_at, match.created_at);
            Assert.AreEqual(field_Value1.date, match.Date);
            //Assert.AreEqual(field_Value1.done_at, match.done_at);
            Assert.AreEqual(field_Value1.field, f1);
            Assert.AreEqual(field_Value1.field_id, match.FieldId);
            Assert.AreEqual(field_Value1.heading, match.Heading);
            Assert.AreEqual(field_Value1.id, match.Id);
            Assert.AreEqual(field_Value1.latitude, match.Latitude);
            Assert.AreEqual(field_Value1.longitude, match.Longitude);
            //Assert.AreEqual(field_Value1.updated_at, match.updated_at);
            //Assert.AreEqual("mappeFile1", match.UploadedData);
            Assert.AreEqual(field_Value1.uploaded_data.checksum, match.UploadedDataObj.Checksum);
            Assert.AreEqual(field_Value1.uploaded_data.current_file, match.UploadedDataObj.CurrentFile);
            Assert.AreEqual(field_Value1.uploaded_data.extension, match.UploadedDataObj.Extension);
            Assert.AreEqual(field_Value1.uploaded_data.file_location, match.UploadedDataObj.FileLocation);
            Assert.AreEqual(field_Value1.uploaded_data.file_name, match.UploadedDataObj.FileName);
            Assert.AreEqual(field_Value1.uploaded_data.id, match.UploadedDataObj.Id);
            Assert.AreEqual(field_Value1.uploaded_data.uploader_id, match.UploadedDataObj.UploaderId);
            Assert.AreEqual(field_Value1.uploaded_data.uploader_type, match.UploadedDataObj.UploaderType);
            //Assert.AreEqual(field_Value1.user_id, match.user_id);
            Assert.AreEqual(field_Value1.value, match.Value);
            //Assert.AreEqual(field_Value1.version, match.version);
            //Assert.AreEqual(field_Value1.worker, match.worker);
            //Assert.AreEqual(field_Value1.workflow_state, match.workflow_state);
            #endregion

        }
        [Test]
        public void SQL_Check_FieldValueReadWithUploadedData_ReturnsAnswer()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);
            //    new check_lists();
            //cl1.created_at = DateTime.Now;
            //cl1.updated_at = DateTime.Now;
            //cl1.label = "A";
            //cl1.description = "D";
            //cl1.workflow_state = Constants.WorkflowStates.Created;
            //cl1.case_type = "CheckList";
            //cl1.folder_name = "Template1FolderName";
            //cl1.display_index = 1;
            //cl1.repeated = 1;

            //DbContext.check_lists.Add(cl1);
            //DbContext.SaveChanges();
            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
            //    new check_lists();
            //cl2.created_at = DateTime.Now;
            //cl2.updated_at = DateTime.Now;
            //cl2.label = "A.1";
            //cl2.description = "D.1";
            //cl2.workflow_state = Constants.WorkflowStates.Created;
            //cl2.case_type = "CheckList";
            //cl2.display_index = 1;
            //cl2.repeated = 1;
            //cl2.parent_id = cl1.id;

            //DbContext.check_lists.Add(cl2);
            //DbContext.SaveChanges();

            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
            //    new fields();
            //field_types ft1 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f1.field_type = ft1;

            //f1.barcode_enabled = 1;
            //f1.barcode_type = "barcode";
            //f1.check_list_id = cl2.id;
            //f1.color = "e2f4fb";
            //f1.created_at = DateTime.Now;
            //f1.custom = "custom";
            //f1.decimal_count = null;
            //f1.default_value = "";
            //f1.description = "Comment field Description";
            //f1.display_index = 5;
            //f1.dummy = 1;
            //f1.geolocation_enabled = 0;
            //f1.geolocation_forced = 0;
            //f1.geolocation_hidden = 1;
            //f1.is_num = 0;
            //f1.label = "Comment field";
            //f1.mandatory = 1;
            //f1.max_length = 55;
            //f1.max_value = "55";
            //f1.min_value = "0";
            //f1.multi = 0;
            //f1.optional = 0;
            //f1.query_type = null;
            //f1.read_only = 1;
            //f1.selected = 0;
            //f1.split_screen = 0;
            //f1.stop_on_save = 0;
            //f1.unit_name = "";
            //f1.updated_at = DateTime.Now;
            //f1.version = 49;
            //f1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f1);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);
            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
            //    new fields();
            //field_types ft2 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f2.field_type = ft2;

            //f2.barcode_enabled = 1;
            //f2.barcode_type = "barcode";
            //f2.check_list_id = cl2.id;
            //f2.color = "f5eafa";
            //f2.default_value = "";
            //f2.description = "showPDf Description";
            //f2.display_index = 45;
            //f2.dummy = 1;
            //f2.geolocation_enabled = 0;
            //f2.geolocation_forced = 1;
            //f2.geolocation_hidden = 0;
            //f2.is_num = 0;
            //f2.label = "ShowPdf";
            //f2.mandatory = 0;
            //f2.max_length = 5;
            //f2.max_value = "5";
            //f2.min_value = "0";
            //f2.multi = 0;
            //f2.optional = 0;
            //f2.query_type = null;
            //f2.read_only = 0;
            //f2.selected = 0;
            //f2.split_screen = 0;
            //f2.stop_on_save = 0;
            //f2.unit_name = "";
            //f2.updated_at = DateTime.Now;
            //f2.version = 9;
            //f2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f2);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
            //    new fields();
            //field_types ft3 = DbContext.field_types.Where(x => x.field_type == "number").First();

            //f3.field_type = ft3;

            //f3.barcode_enabled = 0;
            //f3.barcode_type = "barcode";
            //f3.check_list_id = cl2.id;
            //f3.color = "f0f8db";
            //f3.created_at = DateTime.Now;
            //f3.custom = "custom";
            //f3.decimal_count = 3;
            //f3.default_value = "";
            //f3.description = "Number Field Description";
            //f3.display_index = 83;
            //f3.dummy = 0;
            //f3.geolocation_enabled = 0;
            //f3.geolocation_forced = 0;
            //f3.geolocation_hidden = 1;
            //f3.is_num = 0;
            //f3.label = "Numberfield";
            //f3.mandatory = 1;
            //f3.max_length = 8;
            //f3.max_value = "4865";
            //f3.min_value = "0";
            //f3.multi = 0;
            //f3.optional = 1;
            //f3.query_type = null;
            //f3.read_only = 1;
            //f3.selected = 0;
            //f3.split_screen = 0;
            //f3.stop_on_save = 0;
            //f3.unit_name = "";
            //f3.updated_at = DateTime.Now;
            //f3.version = 1;
            //f3.workflow_state = Constants.WorkflowStates.Created;



            //DbContext.fields.Add(f3);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft4 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f4.field_type = ft4;

            //f4.barcode_enabled = 1;
            //f4.barcode_type = "barcode";
            //f4.check_list_id = cl2.id;
            //f4.color = "fff6df";
            //f4.created_at = DateTime.Now;
            //f4.custom = "custom";
            //f4.decimal_count = null;
            //f4.default_value = "";
            //f4.description = "date Description";
            //f4.display_index = 84;
            //f4.dummy = 0;
            //f4.geolocation_enabled = 0;
            //f4.geolocation_forced = 0;
            //f4.geolocation_hidden = 1;
            //f4.is_num = 0;
            //f4.label = "Date";
            //f4.mandatory = 1;
            //f4.max_length = 666;
            //f4.max_value = "41153";
            //f4.min_value = "0";
            //f4.multi = 0;
            //f4.optional = 1;
            //f4.query_type = null;
            //f4.read_only = 0;
            //f4.selected = 1;
            //f4.split_screen = 0;
            //f4.stop_on_save = 0;
            //f4.unit_name = "";
            //f4.updated_at = DateTime.Now;
            //f4.version = 1;
            //f4.workflow_state = Constants.WorkflowStates.Created;


            //DbContext.fields.Add(f4);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft5 = DbContext.field_types.Where(x => x.field_type == "comment").First();

            //f5.field_type = ft5;
            //f5.barcode_enabled = 0;
            //f5.barcode_type = "barcode";
            //f5.check_list_id = cl2.id;
            //f5.color = "ffe4e4";
            //f5.created_at = DateTime.Now;
            //f5.custom = "custom";
            //f5.decimal_count = null;
            //f5.default_value = "";
            //f5.description = "picture Description";
            //f5.display_index = 85;
            //f5.dummy = 0;
            //f5.geolocation_enabled = 1;
            //f5.geolocation_forced = 0;
            //f5.geolocation_hidden = 1;
            //f5.is_num = 0;
            //f5.label = "Picture";
            //f5.mandatory = 1;
            //f5.max_length = 69;
            //f5.max_value = "69";
            //f5.min_value = "1";
            //f5.multi = 0;
            //f5.optional = 1;
            //f5.query_type = null;
            //f5.read_only = 0;
            //f5.selected = 1;
            //f5.split_screen = 0;
            //f5.stop_on_save = 0;
            //f5.unit_name = "";
            //f5.updated_at = DateTime.Now;
            //f5.version = 1;
            //f5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f5);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            //= new workers();
            //worker.first_name = "Arne";
            //worker.last_name = "Jensen";
            //worker.email = "aa@tak.dk";
            //worker.created_at = DateTime.Now;
            //worker.updated_at = DateTime.Now;
            //worker.microting_uid = 21;
            //worker.workflow_state = Constants.WorkflowStates.Created;
            //worker.version = 69;
            //DbContext.workers.Add(worker);
            //DbContext.SaveChanges();
            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);
            //    new sites();
            //site.name = "SiteName";
            //site.microting_uid = 88;
            //site.updated_at = DateTime.Now;
            //site.created_at = DateTime.Now;
            //site.version = 64;
            //site.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.sites.Add(site);
            //DbContext.SaveChanges();
            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);
            //    new units();
            //unit.microting_uid = 48;
            //unit.otp_code = 49;
            //unit.site = site;
            //unit.site_id = site.id;
            //unit.created_at = DateTime.Now;
            //unit.customer_no = 348;
            //unit.updated_at = DateTime.Now;
            //unit.version = 9;
            //unit.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.units.Add(unit);
            //DbContext.SaveChanges();
            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);
            //    new site_workers();
            //site_workers.created_at = DateTime.Now;
            //site_workers.microting_uid = 55;
            //site_workers.updated_at = DateTime.Now;
            //site_workers.version = 63;
            //site_workers.site = site;
            //site_workers.site_id = site.id;
            //site_workers.worker = worker;
            //site_workers.worker_id = worker.id;
            //site_workers.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.site_workers.Add(site_workers);
            //DbContext.SaveChanges();
            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);
            //    new cases();
            //aCase.case_uid = caseUId;
            //aCase.check_list = cl1;
            //aCase.check_list_id = cl1.id;
            //aCase.created_at = DateTime.Now;
            //aCase.custom = custom;
            //aCase.done_at = DateTime.Now;
            //aCase.done_by_user_id = worker.id;
            //aCase.microting_check_uid = microtingCheckId;
            //aCase.microting_uid = microtingUId;
            //aCase.site = site;
            //aCase.site_id = site.id;
            //aCase.status = 66;
            //aCase.type = caseType;
            //aCase.unit = unit;
            //aCase.unit_id = unit.id;
            //aCase.updated_at = DateTime.Now;
            //aCase.version = 1;
            //aCase.worker = worker;
            //aCase.workflow_state = Constants.WorkflowStates.Created;       
            //DbContext.cases.Add(aCase);
            //DbContext.SaveChanges();
            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);
            //    new check_list_values();

            //check_List_Values.case_id = aCase.id;
            //check_List_Values.check_list_id = cl2.id;
            //check_List_Values.created_at = DateTime.Now;
            //check_List_Values.status = "completed";
            //check_List_Values.updated_at = DateTime.Now;
            //check_List_Values.user_id = null;
            //check_List_Values.version = 865;
            //check_List_Values.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.check_list_values.Add(check_List_Values);
            //DbContext.SaveChanges();

            #endregion

            #region UploadedData
            uploaded_data ud = CreateUploadedData("checksum", "File1", "no", "mappe", "File1", 1, worker,
                "local", 55);

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, ud.id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, ud.id, null, "tomt2", 61234, worker);
            //    new field_values();
            //field_Values2.case_id = aCase.id;
            //field_Values2.check_list = cl2;
            //field_Values2.check_list_id = cl2.id;
            //field_Values2.created_at = DateTime.Now;
            //field_Values2.date = DateTime.Now;
            //field_Values2.done_at = DateTime.Now;
            //field_Values2.field = f2;
            //field_Values2.field_id = f2.id;
            //field_Values2.updated_at = DateTime.Now;
            //field_Values2.user_id = null;
            //field_Values2.value = "tomt2";
            //field_Values2.version = 61234;
            //field_Values2.worker = worker;
            //field_Values2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values2);
            //DbContext.SaveChanges();
            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, ud.id, null, "tomt3", 61234, worker);
            //    new field_values();
            //field_Values3.case_id = aCase.id;
            //field_Values3.check_list = cl2;
            //field_Values3.check_list_id = cl2.id;
            //field_Values3.created_at = DateTime.Now;
            //field_Values3.date = DateTime.Now;
            //field_Values3.done_at = DateTime.Now;
            //field_Values3.field = f3;
            //field_Values3.field_id = f3.id;
            //field_Values3.updated_at = DateTime.Now;
            //field_Values3.user_id = null;
            //field_Values3.value = "tomt3";
            //field_Values3.version = 61234;
            //field_Values3.worker = worker;
            //field_Values3.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values3);
            //DbContext.SaveChanges();
            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, ud.id, null, "tomt4", 61234, worker);
            //    new field_values();
            //field_Values4.case_id = aCase.id;
            //field_Values4.check_list = cl2;
            //field_Values4.check_list_id = cl2.id;
            //field_Values4.created_at = DateTime.Now;
            //field_Values4.date = DateTime.Now;
            //field_Values4.done_at = DateTime.Now;
            //field_Values4.field = f4;
            //field_Values4.field_id = f4.id;
            //field_Values4.updated_at = DateTime.Now;
            //field_Values4.user_id = null;
            //field_Values4.value = "tomt4";
            //field_Values4.version = 61234;
            //field_Values4.worker = worker;
            //field_Values4.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values4);
            //DbContext.SaveChanges();
            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, ud.id, null, "tomt5", 61234, worker);
            //    new field_values();
            //field_Values5.case_id = aCase.id;
            //field_Values5.check_list = cl2;
            //field_Values5.check_list_id = cl2.id;
            //field_Values5.created_at = DateTime.Now;
            //field_Values5.date = DateTime.Now;
            //field_Values5.done_at = DateTime.Now;
            //field_Values5.field = f5;
            //field_Values5.field_id = f5.id;
            //field_Values5.updated_at = DateTime.Now;
            //field_Values5.user_id = null;
            //field_Values5.value = "tomt5";
            //field_Values5.version = 61234;
            //field_Values5.worker = worker;
            //field_Values5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values5);
            //DbContext.SaveChanges();
            #endregion


            #endregion

            
            #endregion
            // Act

            var match = sut.FieldValueRead(f1, field_Value1, true);

            //// Assert
            #region Assert
            Assert.True(match is FieldValue);
            Assert.AreEqual(field_Value1.accuracy, match.Accuracy);
            Assert.AreEqual(field_Value1.altitude, match.Altitude);
            //Assert.AreEqual(field_Value1.case_id, match.case_id);
            //Assert.AreEqual(field_Value1.check_list, match.check_list);
            //Assert.AreEqual(field_Value1.check_list_duplicate_id, match.check_list_duplicate_id);
            //Assert.AreEqual(field_Value1.check_list_id, match.check_list_id);
            //Assert.AreEqual(field_Value1.created_at, match.created_at);
            Assert.AreEqual(field_Value1.date, match.Date);
            //Assert.AreEqual(field_Value1.done_at, match.done_at);
            Assert.AreEqual(field_Value1.field, f1);
            Assert.AreEqual(field_Value1.field_id, match.FieldId);
            Assert.AreEqual(field_Value1.heading, match.Heading);
            Assert.AreEqual(field_Value1.id, match.Id);
            Assert.AreEqual(field_Value1.latitude, match.Latitude);
            Assert.AreEqual(field_Value1.longitude, match.Longitude);
            //Assert.AreEqual(field_Value1.updated_at, match.updated_at);
            Assert.AreEqual("mappeFile1", match.UploadedData);
            //Assert.AreEqual(field_Value1.uploaded_data_id, match.UploadedDataObj);
            //Assert.AreEqual(field_Value1.user_id, match.user_id);         
            Assert.AreEqual(field_Value1.value, match.Value);
            //Assert.AreEqual(field_Value1.version, match.version);
            //Assert.AreEqual(field_Value1.worker, match.worker);
            //Assert.AreEqual(field_Value1.workflow_state, match.workflow_state);

            #endregion
        }
        [Test]
        public void SQL_Check_FieldValueRead_ReturnsTrue()
        {
            // Arrance

            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);
      
            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
   

            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
           
            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
          

            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
          

            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
         

            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
         

            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
          
            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);
  
            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);
  
            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);
  
            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);
      

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
        
            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
        
            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
           
            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
          
            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion

            // Act

            var match = sut.FieldValueRead(field_Value1.id);

            // Assert

            Assert.AreEqual(field_Value1.id, match.Id);

        }
        [Test]
        public void SQL_Check_FieldValueReadList_ReturnsList()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion
            // Act

            List<FieldValue> match = sut.FieldValueReadList(f1.id, 5);

            // Assert
            
            Assert.AreEqual(field_Value1.value, match[0].Value);

        }
        [Test]
        public void SQL_Check_FieldValueUpdate_UpdatesFieldValue()
        {
            // Arrance

            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion

            // Act

            sut.FieldValueUpdate(aCase.id, f1.id, "udfyldt");



            // Assert
            var newValue = DbContext.field_values.AsNoTracking().SingleOrDefault(x => x.id == field_Value1.id);

            Assert.AreEqual(newValue.value, "udfyldt");


        }
        [Test]
        public void SQL_Check_FieldValueReadAllValues_ReturnsReturnList()
        {
            // Arrance

            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File1", 1, worker,
                "local", 55);
            #endregion

            #region ud2
            uploaded_data ud2 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File2", 1, worker,
                "local", 55);
            #endregion

            #region ud3
            uploaded_data ud3 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File3", 1, worker,
                "local", 55);
            #endregion

            #region ud4
            uploaded_data ud4 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File4", 1, worker,
                "local", 55);
            #endregion

            #region ud5
            uploaded_data ud5 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File5", 1, worker,
                "local", 55);
            #endregion

            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "completed", null, 865);


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, ud1.id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, ud2.id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, ud3.id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, ud4.id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion

            // Act
            List<int> listOfCaseIds = new List<int>();
            listOfCaseIds.Add(aCase.id);
            var matchF1 = sut.FieldValueReadAllValues(f1.id, listOfCaseIds, "mappe/");
            var matchF2 = sut.FieldValueReadAllValues(f2.id, listOfCaseIds, "mappe/");
            var matchF3 = sut.FieldValueReadAllValues(f3.id, listOfCaseIds, "mappe/");
            var matchF4 = sut.FieldValueReadAllValues(f4.id, listOfCaseIds, "mappe/");
            var matchF5 = sut.FieldValueReadAllValues(f5.id, listOfCaseIds, "mappe/");

            // Assert
            #region Assert
            Assert.AreEqual("mappe/File1", matchF1[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File2", matchF2[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File3", matchF3[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File4", matchF4[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File5", matchF5[0].ElementAt(0).Value);
            #endregion

        }
        [Test]
        public void SQL_Check_CheckListValueStatusRead_ReturnsCheckListValuesStatus()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File1", 1, worker,
                "local", 55);
            #endregion

            #region ud2
            uploaded_data ud2 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File2", 1, worker,
                "local", 55);
            #endregion

            #region ud3
            uploaded_data ud3 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File3", 1, worker,
                "local", 55);
            #endregion

            #region ud4
            uploaded_data ud4 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File4", 1, worker,
                "local", 55);
            #endregion

            #region ud5
            uploaded_data ud5 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File5", 1, worker,
                "local", 55);
            #endregion

            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "checked", null, 865);


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, ud1.id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, ud2.id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, ud3.id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, ud4.id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion
            // Act
            var match = sut.CheckListValueStatusRead(aCase.id, cl2.id);
            // Assert

            Assert.AreEqual(check_List_Values.status, "checked");

        }
        [Test]
        public void SQL_Check_CheckListValueStatusUpdate_UpdatesCheckListValues()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File1", 1, worker,
                "local", 55);
            #endregion

            #region ud2
            uploaded_data ud2 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File2", 1, worker,
                "local", 55);
            #endregion

            #region ud3
            uploaded_data ud3 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File3", 1, worker,
                "local", 55);
            #endregion

            #region ud4
            uploaded_data ud4 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File4", 1, worker,
                "local", 55);
            #endregion

            #region ud5
            uploaded_data ud5 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File5", 1, worker,
                "local", 55);
            #endregion

            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "checked", null, 865);


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, ud1.id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, ud2.id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, ud3.id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, ud4.id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion
            // Act

            sut.CheckListValueStatusUpdate(aCase.id, cl2.id, "not_approved");

            // Assert
            var newValue = DbContext.check_list_values.AsNoTracking().SingleOrDefault(x => x.id == check_List_Values.id);

            Assert.AreEqual(newValue.status, "not_approved");


        }


        #endregion

        //todo
        #region notification


        #endregion
        //todo
        #region file

        #endregion

        #endregion

        #region (post) case
        [Test]
        public void SQL_PostCase_CaseReadByMUId_Returns_ReturnCase()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File1", 1, worker,
                "local", 55);
            #endregion

            #region ud2
            uploaded_data ud2 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File2", 1, worker,
                "local", 55);
            #endregion

            #region ud3
            uploaded_data ud3 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File3", 1, worker,
                "local", 55);
            #endregion

            #region ud4
            uploaded_data ud4 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File4", 1, worker,
                "local", 55);
            #endregion

            #region ud5
            uploaded_data ud5 = CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File5", 1, worker,
                "local", 55);
            #endregion

            #endregion

            #region Check List Values
            check_list_values check_List_Values = CreateCheckListValues(aCase, cl2, "checked", null, 865);


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = CreateFieldValues(aCase, cl2, f1, ud1.id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = CreateFieldValues(aCase, cl2, f2, ud2.id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = CreateFieldValues(aCase, cl2, f3, ud3.id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = CreateFieldValues(aCase, cl2, f4, ud4.id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = CreateFieldValues(aCase, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion


            // Act

            var match = sut.CaseReadByMUId(aCase.microting_uid);

            // Assert

            Assert.AreEqual(aCase.microting_uid, match.MicrotingUId);


        }

        [Test]
        public void SQL_PostCase_CaseReadByCaseId_Returns_cDto()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion

           
            #endregion


            // Act

            var match = sut.CaseReadByCaseId(aCase.id);

            // Assert

            Assert.AreEqual(aCase.id, match.CaseId);
        }

        [Test]
        public void SQL_Postcase_CaseReadByCaseUId_Returns_lstDto()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion


            #endregion


            // Act

            var match = sut.CaseReadByCaseUId(aCase.case_uid);


            // Assert

            Assert.AreEqual(aCase.case_uid, match[0].CaseUId);
        }

        [Test]
        public void SQL_PostCase_CaseReadFull()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, 1, worker);

            #endregion


            #endregion
            // Act
            var match = sut.CaseReadFull(aCase.microting_uid, aCase.microting_check_uid);
            // Assert
            Assert.AreEqual(aCase.microting_uid, match.microting_uid);
            Assert.AreEqual(aCase.microting_check_uid, match.microting_check_uid);
        }

        [Test]
        public void SQL_PostCase_CaseReadFirstId()
        {
            // Arrance
            #region Arrance
            #region Template1
            check_lists cl1 = CreateTemplate("A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = CreateSites("SiteName", 88);

            #endregion

            #region units
            units unit = CreateUnits(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = CreateSiteWorkers(55, site, worker);

            #endregion

            #region Case1

            cases aCase = CreateCase("caseUId", cl1, "custom", worker, "microtingCheckUId", "microtingUId",
               site, 100, "caseType", unit, 1, worker);

            #endregion


            #endregion
            // Act
            var match = sut.CaseReadFirstId(aCase.check_list_id);
            // Assert
            Assert.AreEqual(aCase.id, match);
        }

        [Test]
        public void SQL_PostCase_CaseReadAll()
        {


            // Arrance

            // Act

            // Assert

        }

        [Test]
        public void SQL_PostCase_CaseFindCustomMatchs()
        {


            // Arrance

            // Act

            // Assert

        }

        [Test]
        public void SQL_PostCase_CaseUpdateFieldValues()
        {


            // Arrance

            // Act

            // Assert

        }




        #endregion

        #region tag
        [Test]
        public void SQL_Tags_CreateTag_DoesCreateNewTag()
        {
            // Arrance
            string tagName = "Tag1";

            // Act
            sut.TagCreate(tagName);

            // Assert
            var tag = DbContext.tags.ToList();

            Assert.AreEqual(tag[0].name, tagName);
            Assert.AreEqual(1, tag.Count());
        }

        [Test]
        public void SQL_Tags_DeleteTag_DoesMarkTagAsRemoved()
        {
            // Arrance
            string tagName = "Tag1";
            tags tag = new tags();
            tag.name = tagName;
            tag.workflow_state = Constants.WorkflowStates.Created;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            // Act
            sut.TagDelete(tag.id);

            // Assert
            var result = DbContext.tags.AsNoTracking().ToList();

            Assert.AreEqual(result[0].name, tagName);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, result[0].workflow_state);
        }

        [Test]
        public void SQL_Tags_CreateTag_DoesRecreateRemovedTag()
        {
            // Arrance
            string tagName = "Tag1";
            tags tag = new tags();
            tag.name = tagName;
            tag.workflow_state = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            // Act
            sut.TagCreate(tagName);

            // Assert
            var result = DbContext.tags.AsNoTracking().ToList();

            Assert.AreEqual(result[0].name, tagName);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, result[0].workflow_state);
        }

        [Test]
        public void SQL_Tags_GetAllTags_DoesReturnAllTags()
        {
            // Arrance
            string tagName1 = "Tag1";
            tags tag = new tags();
            tag.name = tagName1;
            tag.workflow_state = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            string tagName2 = "Tag2";
            tag = new tags();

            tag.name = tagName2;
            tag.workflow_state = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();
            string tagName3 = "Tag3";
            tag = new tags();

            tag.name = tagName3;
            tag.workflow_state = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();
            //int tagId3 = sut.TagCreate(tagName3);

            // Act
            var tags = sut.GetAllTags(true);            

            // Assert
            Assert.True(true);
            Assert.AreEqual(3, tags.Count());
            Assert.AreEqual(tagName1, tags[0].Name);
            Assert.AreEqual(0, tags[0].TaggingCount);
            Assert.AreEqual(tagName2, tags[1].Name);
            Assert.AreEqual(0, tags[1].TaggingCount);       
            Assert.AreEqual(tagName3, tags[2].Name);
            Assert.AreEqual(0, tags[2].TaggingCount);
        }

        [Test]
        public void SQL_Tags_TemplateSetTags_DoesAssignTagToTemplate()
        {
            // Arrance
            check_lists cl1 = new check_lists();
            cl1.created_at = DateTime.Now;
            cl1.updated_at = DateTime.Now;
            cl1.label = "A";
            cl1.description = "D";
            cl1.workflow_state = Constants.WorkflowStates.Created;
            cl1.case_type = "CheckList";
            cl1.folder_name = "Template1FolderName";
            cl1.display_index = 1;
            cl1.repeated = 1;

            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();

            string tagName1 = "Tag1";
            tags tag = new tags();
            tag.name = tagName1;
            tag.workflow_state = Constants.WorkflowStates.Created;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            // Act
            List<int> tags = new List<int>();
            tags.Add(tag.id);
            sut.TemplateSetTags(cl1.id, tags);


            // Assert
            List<taggings> result = DbContext.taggings.AsNoTracking().ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(tag.id, result[0].tag_id);
            Assert.AreEqual(cl1.id, result[0].check_list_id);
            Assert.True(true);
        }
        #endregion

        #region case
        [Test]
        public void SQL_Case_CaseDeleteResult_DoesMarkCaseRemoved()
        {

            // Arrance
            sites site = new sites();
            site.name = "SiteName";
            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            check_lists cl = new check_lists();
            cl.label = "label";

            DbContext.check_lists.Add(cl);
            DbContext.SaveChanges();

            cases aCase = new cases();
            aCase.microting_uid = "microting_uid";
            aCase.microting_check_uid = "microting_check_uid";
            aCase.workflow_state = Constants.WorkflowStates.Created;
            aCase.check_list_id = cl.id;
            aCase.site_id = site.id;

            DbContext.cases.Add(aCase);
            DbContext.SaveChanges();

            // Act
            sut.CaseDeleteResult(aCase.id);
            //cases theCase = sut.CaseReadFull(aCase.microting_uid, aCase.microting_check_uid);
            var match = DbContext.cases.AsNoTracking().ToList();
            var versionedMatches = DbContext.case_versions.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Count);
            Assert.AreEqual(1, versionedMatches.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, match[0].workflow_state);
            Assert.AreEqual(Constants.WorkflowStates.Removed, versionedMatches[0].workflow_state);
        }

        [Test]
        public void SQL_Case_CaseDelete_DoesCaseRemoved()
        {
            // Arrance
            // Arrance
            sites site = new sites();
            site.name = "SiteName";
            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            check_lists cl = new check_lists();
            cl.label = "label";

            DbContext.check_lists.Add(cl);
            DbContext.SaveChanges();

            cases aCase = new cases();
            aCase.microting_uid = "microting_uid";
            aCase.microting_check_uid = "microting_check_uid";
            aCase.workflow_state = Constants.WorkflowStates.Created;
            aCase.check_list_id = cl.id;
            aCase.site_id = site.id;

            DbContext.cases.Add(aCase);
            DbContext.SaveChanges();

            // Act
            sut.CaseDelete(aCase.microting_uid);
            //cases theCase = sut.CaseReadFull(aCase.microting_uid, aCase.microting_check_uid);
            var match = DbContext.cases.AsNoTracking().ToList();
            var versionedMatches = DbContext.case_versions.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Count);
            Assert.AreEqual(1, versionedMatches.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, match[0].workflow_state);
            Assert.AreEqual(Constants.WorkflowStates.Removed, versionedMatches[0].workflow_state);
        }

        [Test]
        public void SQL_Case_CaseUpdateRetrived_DoesCaseGetUpdated()
        {

            // Arrance
            sites site = new sites();
            site.name = "SiteName";
            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            check_lists cl = new check_lists();
            cl.label = "label";

            DbContext.check_lists.Add(cl);
            DbContext.SaveChanges();

            cases aCase = new cases();
            aCase.microting_uid = "microting_uid";
            aCase.microting_check_uid = "microting_check_uid";
            aCase.workflow_state = Constants.WorkflowStates.Created;
            aCase.check_list_id = cl.id;
            aCase.site_id = site.id;
            aCase.status = 66;

            DbContext.cases.Add(aCase);
            DbContext.SaveChanges();

            // Act
            sut.CaseUpdateRetrived(aCase.microting_uid);
            //Case_Dto caseResult = sut.CaseFindCustomMatchs(aCase.microting_uid);
            List<cases> caseResults = DbContext.cases.AsNoTracking().ToList();


            // Assert

            
            Assert.NotNull(caseResults);
            Assert.AreEqual(1, caseResults.Count);
            Assert.AreNotEqual(1, caseResults[0]);
            

        

        }

        [Test]
        public void SQL_Case_CaseDeleteReversed_DoesDeletionReversed()
        {
            // Arrance
            sites site = CreateSites("mySite", 987);

            check_lists cl1 = CreateTemplate("bla", "bla_desc", "", "", 0, 0);

            check_list_sites cls1 = CreateCheckListSite(cl1.id, site.id);

            // Act
            sut.CaseDeleteReversed(cls1.microting_uid);
            //Case_Dto caseResult = sut.CaseFindCustomMatchs(aCase.microting_uid);
            List<cases> caseResults = DbContext.cases.AsNoTracking().ToList();
            List<sites> siteResults = DbContext.sites.AsNoTracking().ToList();


            // Assert



            Assert.NotNull(caseResults);
            //Assert.AreEqual(1, caseResults.Count);
            //Assert.AreNotEqual(1, caseResults[1]);
        }

       

        #endregion

        #region public site
        #region site
        //         public List<SiteName_Dto> SiteGetAll(bool includeRemoved)

        [Test]

        public void SQL_Site_SiteGetAll_DoesReturnAllSites()
        {
            // Arrance

            // Act

            // Assert
            Assert.True(true);
        }
        #endregion
        #endregion

        // Arrance

        // Act

        // Assert

        #region helperMethods
        public workers CreateWorker(string email, string firstName, string lastName, int microtingUId)
        {
            workers worker = new workers();
            worker.first_name = firstName;
            worker.last_name = lastName;
            worker.email = email;
            worker.created_at = DateTime.Now;
            worker.updated_at = DateTime.Now;
            worker.microting_uid = microtingUId;
            worker.workflow_state = Constants.WorkflowStates.Created;
            worker.version = 69;
            DbContext.workers.Add(worker);
            DbContext.SaveChanges();

            return worker;
        }
        public sites CreateSites(string name, int microtingUId)
        {

            sites site = new sites();
            site.name = name;
            site.microting_uid = microtingUId;
            site.updated_at = DateTime.Now;
            site.created_at = DateTime.Now;
            site.version = 64;
            site.workflow_state = Constants.WorkflowStates.Created;
            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            return site;
        }
        public units CreateUnits(int microtingUId, int otpCode, sites site, int customerNo)
        {

            units unit = new units();
            unit.microting_uid = microtingUId;
            unit.otp_code = otpCode;
            unit.site = site;
            unit.site_id = site.id;
            unit.created_at = DateTime.Now;
            unit.customer_no = customerNo;
            unit.updated_at = DateTime.Now;
            unit.version = 9;
            unit.workflow_state = Constants.WorkflowStates.Created;

            DbContext.units.Add(unit);
            DbContext.SaveChanges();

            return unit;
        }
        public site_workers CreateSiteWorkers(int microtingUId, sites site, workers worker)
        {
            site_workers site_workers = new site_workers();
            site_workers.created_at = DateTime.Now;
            site_workers.microting_uid = microtingUId;
            site_workers.updated_at = DateTime.Now;
            site_workers.version = 63;
            site_workers.site = site;
            site_workers.site_id = site.id;
            site_workers.worker = worker;
            site_workers.worker_id = worker.id;
            site_workers.workflow_state = Constants.WorkflowStates.Created;
            DbContext.site_workers.Add(site_workers);
            DbContext.SaveChanges();
            return site_workers;
        }
        public check_lists CreateTemplate(string label, string description, string caseType, string folderName, int displayIndex, int repeated)
        {
            check_lists cl1 = new check_lists();
            cl1.created_at = DateTime.Now;
            cl1.updated_at = DateTime.Now;
            cl1.label = label;
            cl1.description = description;
            cl1.workflow_state = Constants.WorkflowStates.Created;
            cl1.case_type = caseType;
            cl1.folder_name = folderName;
            cl1.display_index = displayIndex;
            cl1.repeated = repeated;

            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();
            return cl1;
        }
        public check_lists CreateSubTemplate(string label, string description, string caseType, int displayIndex, int repeated, check_lists parentId)
        {
            check_lists cl2 = new check_lists();
            cl2.created_at = DateTime.Now;
            cl2.updated_at = DateTime.Now;
            cl2.label = label;
            cl2.description = description;
            cl2.workflow_state = Constants.WorkflowStates.Created;
            cl2.case_type = caseType;
            cl2.display_index = displayIndex;
            cl2.repeated = repeated;
            cl2.parent_id = parentId.id;

            DbContext.check_lists.Add(cl2);
            DbContext.SaveChanges();
            return cl2;
        }
        public fields CreateField(short? barcodeEnabled, string barcodeType, check_lists checkList, string color, string custom, int? decimalCount, string defaultValue, string description, int? displayIndex, short? dummy, field_types ft, short? geolocationEnabled, short? geolocationForced, short? geolocationHidden, short? isNum, string label, short? mandatory, int maxLength, string maxValue, string minValue, short? multi, short? optional, string queryType, short? readOnly, short? selected, short? splitScreen, short? stopOnSave, string unitName, int version)
        {

            fields f = new fields();
            f.field_type = ft;

            f.barcode_enabled = barcodeEnabled;
            f.barcode_type = barcodeType;
            f.check_list_id = checkList.id;
            f.color = color;
            f.created_at = DateTime.Now;
            f.custom = custom;
            f.decimal_count = decimalCount;
            f.default_value = defaultValue;
            f.description = description;
            f.display_index = displayIndex;
            f.dummy = dummy;
            f.geolocation_enabled = geolocationEnabled;
            f.geolocation_forced = geolocationForced;
            f.geolocation_hidden = geolocationHidden;
            f.is_num = isNum;
            f.label = label;
            f.mandatory = mandatory;
            f.max_length = maxLength;
            f.max_value = maxValue;
            f.min_value = minValue;
            f.multi = multi;
            f.optional = optional;
            f.query_type = queryType;
            f.read_only = readOnly;
            f.selected = selected;
            f.split_screen = splitScreen;
            f.stop_on_save = stopOnSave;
            f.unit_name = unitName;
            f.updated_at = DateTime.Now;
            f.version = version;
            f.workflow_state = Constants.WorkflowStates.Created;

            DbContext.fields.Add(f);
            DbContext.SaveChanges();
            Thread.Sleep(2000);

            return f;
        }
        public cases CreateCase(string caseUId, check_lists checkList, string custom, workers doneByUderId, string microtingCheckId, string microtingUId, sites site, int? status, string caseType, units unit, int version, workers worker)
        {

            cases aCase = new cases();
            aCase.case_uid = caseUId;
            aCase.check_list = checkList;
            aCase.check_list_id = checkList.id;
            aCase.created_at = DateTime.Now;
            aCase.custom = custom;
            aCase.done_at = DateTime.Now;
            aCase.done_by_user_id = worker.id;
            aCase.microting_check_uid = microtingCheckId;
            aCase.microting_uid = microtingUId;
            aCase.site = site;
            aCase.site_id = site.id;
            aCase.status = status;
            aCase.type = caseType;
            aCase.unit = unit;
            aCase.unit_id = unit.id;
            aCase.updated_at = DateTime.Now;
            aCase.version = version;
            aCase.worker = worker;
            aCase.workflow_state = Constants.WorkflowStates.Created;
            DbContext.cases.Add(aCase);
            DbContext.SaveChanges();

            return aCase;
        }
        public field_values CreateFieldValues(cases aCase, check_lists checkList, fields f, int? ud_id,  int? userId, string value, int? version, workers worker)
        {
            field_values fv = new field_values();
            fv.case_id = aCase.id;
            fv.check_list = checkList;
            fv.check_list_id = checkList.id;
            fv.created_at = DateTime.Now;
            fv.date = DateTime.Now;
            fv.done_at = DateTime.Now;
            fv.field = f;
            fv.field_id = f.id;
            fv.updated_at = DateTime.Now;
            if (ud_id != null)
            {
                fv.uploaded_data_id = ud_id;
            }
            fv.user_id = userId;
            fv.value = value;
            fv.version = version;
            fv.worker = worker;
            fv.workflow_state = Constants.WorkflowStates.Created;

            DbContext.field_values.Add(fv);
            DbContext.SaveChanges();
            return fv;
        }
        public check_list_values CreateCheckListValues(cases aCase, check_lists checkList, string status, int? userId, int? version)
        {
            check_list_values CLV = new check_list_values();

            CLV.case_id = aCase.id;
            CLV.check_list_id = checkList.id;
            CLV.created_at = DateTime.Now;
            CLV.status = status;
            CLV.updated_at = DateTime.Now;
            CLV.user_id = userId;
            CLV.version = version;
            CLV.workflow_state = Constants.WorkflowStates.Created;

            DbContext.check_list_values.Add(CLV);
            DbContext.SaveChanges();
            return CLV;

        }
        public uploaded_data CreateUploadedData(string checkSum, string currentFile, string extension, string fileLocation, string fileName, short? local, workers worker, string uploaderType, int version)
        {
            uploaded_data UD = new uploaded_data();

            UD.checksum = checkSum;
            UD.created_at = DateTime.Now;
            UD.current_file = currentFile;
            UD.expiration_date = DateTime.Now.AddYears(1);
            UD.extension = extension;
            UD.file_location = fileLocation;
            UD.file_name = fileName;
            UD.local = local;
            UD.updated_at = DateTime.Now;
            UD.uploader_id = worker.id;
            UD.uploader_type = uploaderType;
            UD.version = version;
            UD.workflow_state = Constants.WorkflowStates.Created;

            DbContext.uploaded_data.Add(UD);
            DbContext.SaveChanges();
            return UD;
        }


        public check_list_sites CreateCheckListSite(int checkListId, int siteId)
        {
            check_list_sites cls = new check_list_sites();
            cls.site_id = siteId;
            cls.check_list_id = checkListId;
            cls.created_at = DateTime.Now;
            cls.updated_at = DateTime.Now;
            cls.workflow_state = Constants.WorkflowStates.Created;

            DbContext.check_list_sites.Add(cls);
            DbContext.SaveChanges();
            return cls;
        }
        
        #endregion


    }


}
