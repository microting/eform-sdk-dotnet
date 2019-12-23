/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace Microting.eForm.Helpers
{
    public class TestHelpers
    {
        public MicrotingDbContext dbContext;
//        private string returnXML;
//        private string returnJSON;
        public TestHelpers()
        {
        
            string connectionString;

//            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
//            {
//                ConnectionString = @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests;Integrated Security=True";
//            }
//            else
//            {
                connectionString = @"Server = localhost; port = 3306; Database = eformsdk-tests; user = root; Convert Zero Datetime = true;";
//            }

            dbContext = GetContext(connectionString);
        }
        private MicrotingDbContext GetContext(string connectionStr)
        {

            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();

            if (connectionStr.ToLower().Contains("convert zero datetime"))
            {
                dbContextOptionsBuilder.UseMySql(connectionStr);
            }
            else
            {
                dbContextOptionsBuilder.UseSqlServer(connectionStr);
            }
            dbContextOptionsBuilder.UseLazyLoadingProxies(true);
            return new MicrotingDbContext(dbContextOptionsBuilder.Options);

        }
        #region helperMethods
        public async Task<workers> CreateWorker(string email, string firstName, string lastName, int microtingUId)
        {
            workers worker = new workers();
            worker.FirstName = firstName;
            worker.LastName = lastName;
            worker.Email = email;
            worker.CreatedAt = DateTime.Now;
            worker.UpdatedAt = DateTime.Now;
            worker.MicrotingUid = microtingUId;
            worker.WorkflowState = Constants.WorkflowStates.Created;
            worker.Version = 69;
            dbContext.workers.Add(worker);
            await dbContext.SaveChangesAsync();

            return worker;
        }
        public async Task<sites> CreateSite(string name, int microtingUId)
        {

            sites site = new sites();
            site.Name = name;
            site.MicrotingUid = microtingUId;
            site.UpdatedAt = DateTime.Now;
            site.CreatedAt = DateTime.Now;
            site.Version = 64;
            site.WorkflowState = Constants.WorkflowStates.Created;
            dbContext.sites.Add(site);
            await dbContext.SaveChangesAsync();

            return site;
        }
        public async Task<units> CreateUnit(int microtingUId, int otpCode, sites site, int customerNo)
        {

            units unit = new units();
            unit.MicrotingUid = microtingUId;
            unit.OtpCode = otpCode;
            unit.Site = site;
            unit.SiteId = site.Id;
            unit.CreatedAt = DateTime.Now;
            unit.CustomerNo = customerNo;
            unit.UpdatedAt = DateTime.Now;
            unit.Version = 9;
            unit.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.units.Add(unit);
            await dbContext.SaveChangesAsync();

            return unit;
        }
        public async Task<site_workers> CreateSiteWorker(int microtingUId, sites site, workers worker)
        {
            site_workers site_workers = new site_workers();
            site_workers.CreatedAt = DateTime.Now;
            site_workers.MicrotingUid = microtingUId;
            site_workers.UpdatedAt = DateTime.Now;
            site_workers.Version = 63;
            site_workers.Site = site;
            site_workers.SiteId = site.Id;
            site_workers.Worker = worker;
            site_workers.WorkerId = worker.Id;
            site_workers.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.site_workers.Add(site_workers);
            await dbContext.SaveChangesAsync();

            return site_workers;
        }
        public async Task<check_lists> CreateTemplate(DateTime cl_ca, DateTime cl_ua, string label, string description, string caseType, string folderName, int displayIndex, int repeated)
        {
            
            check_lists cl1 = new check_lists();
            cl1.CreatedAt = cl_ca;
            cl1.UpdatedAt = cl_ua;
            cl1.Label = label;
            cl1.Description = description;
            cl1.WorkflowState = Constants.WorkflowStates.Created;
            cl1.CaseType = caseType;
            cl1.FolderName = folderName;
            cl1.DisplayIndex = displayIndex;
            cl1.Repeated = repeated;
            cl1.ParentId = 0;
            
            dbContext.check_lists.Add(cl1);
            await dbContext.SaveChangesAsync();
            return cl1;
        }
        public async Task<check_lists> CreateSubTemplate(string label, string description, string caseType, int displayIndex, int repeated, check_lists parentId)
        {
            check_lists cl2 = new check_lists();
            cl2.CreatedAt = DateTime.Now;
            cl2.UpdatedAt = DateTime.Now;
            cl2.Label = label;
            cl2.Description = description;
            cl2.WorkflowState = Constants.WorkflowStates.Created;
            cl2.CaseType = caseType;
            cl2.DisplayIndex = displayIndex;
            cl2.Repeated = repeated;
            cl2.ParentId = parentId.Id;

            dbContext.check_lists.Add(cl2);
            await dbContext.SaveChangesAsync();
            return cl2;
        }
        public async Task<fields> CreateField(short? barcodeEnabled, string barcodeType, check_lists checkList, string color, string custom, int? decimalCount, string defaultValue, string description, int? displayIndex, short? dummy, field_types ft, short? geolocationEnabled, short? geolocationForced, short? geolocationHidden, short? isNum, string label, short? mandatory, int maxLength, string maxValue, string minValue, short? multi, short? optional, string queryType, short? readOnly, short? selected, short? splitScreen, short? stopOnSave, string unitName, int version)
        {

            fields f = new fields();
            f.FieldTypeId = ft.Id;

            f.BarcodeEnabled = barcodeEnabled;
            f.BarcodeType = barcodeType;
            f.CheckListId = checkList.Id;
            f.Color = color;
            f.CreatedAt = DateTime.Now;
            f.Custom = custom;
            f.DecimalCount = decimalCount;
            f.DefaultValue = defaultValue;
            f.Description = description;
            f.DisplayIndex = displayIndex;
            f.Dummy = dummy;
            f.GeolocationEnabled = geolocationEnabled;
            f.GeolocationForced = geolocationForced;
            f.GeolocationHidden = geolocationHidden;
            f.IsNum = isNum;
            f.Label = label;
            f.Mandatory = mandatory;
            f.MaxLength = maxLength;
            f.MaxValue = maxValue;
            f.MinValue = minValue;
            f.Multi = multi;
            f.Optional = optional;
            f.QueryType = queryType;
            f.ReadOnly = readOnly;
            f.Selected = selected;
            f.SplitScreen = splitScreen;
            f.StopOnSave = stopOnSave;
            f.UnitName = unitName;
            f.UpdatedAt = DateTime.Now;
            f.Version = version;
            f.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.fields.Add(f);
            await dbContext.SaveChangesAsync();
            Thread.Sleep(2000);

            return f;
        }
        public async Task<cases> CreateCase(string caseUId, check_lists checkList, DateTime created_at, string custom, DateTime done_at, workers doneByUserId, int microtingCheckId, int microtingUId, sites site, int? status, string caseType, units unit, DateTime updated_at, int version, workers worker, string WorkFlowState)
        {

            cases aCase = new cases();

            aCase.CaseUid = caseUId;
            aCase.CheckList = checkList;
            aCase.CheckListId = checkList.Id;
            aCase.CreatedAt = created_at;
            aCase.Custom = custom;
            aCase.DoneAt = done_at;
            aCase.WorkerId = worker.Id;
            aCase.MicrotingCheckUid = microtingCheckId;
            aCase.MicrotingUid = microtingUId;
            aCase.Site = site;
            aCase.SiteId = site.Id;
            aCase.Status = status;
            aCase.Type = caseType;
            aCase.Unit = unit;
            aCase.UnitId = unit.Id;
            aCase.UpdatedAt = updated_at;
            aCase.Version = version;
            aCase.Worker = worker;
            aCase.WorkflowState = WorkFlowState;
            dbContext.cases.Add(aCase);
            await dbContext.SaveChangesAsync();

            return aCase;
        }
        public async Task<field_values> CreateFieldValue(cases aCase, check_lists checkList, fields f, int? ud_id, int? userId, string value, int? version, workers worker)
        {
            field_values fv = new field_values();
            fv.CaseId = aCase.Id;
            fv.CheckList = checkList;
            fv.CheckListId = checkList.Id;
            fv.CreatedAt = DateTime.Now;
            fv.Date = DateTime.Now;
            fv.DoneAt = DateTime.Now;
            fv.Field = f;
            fv.FieldId = f.Id;
            fv.UpdatedAt = DateTime.Now;
            if (ud_id != null)
            {
                fv.UploadedDataId = ud_id;
            }
            fv.WorkerId = userId;
            fv.Value = value;
            fv.Version = version;
            fv.Worker = worker;
            fv.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.field_values.Add(fv);
            await dbContext.SaveChangesAsync();
            return fv;
        }
        public async Task<check_list_values> CreateCheckListValue(cases aCase, check_lists checkList, string status, int? userId, int? version)
        {
            check_list_values CLV = new check_list_values();

            CLV.CaseId = aCase.Id;
            CLV.CheckListId = checkList.Id;
            CLV.CreatedAt = DateTime.Now;
            CLV.Status = status;
            CLV.UpdatedAt = DateTime.Now;
            CLV.UserId = userId;
            CLV.Version = version;
            CLV.WorkflowState = Constants.WorkflowStates.Created;
            
            dbContext.check_list_values.Add(CLV);
            await dbContext.SaveChangesAsync();
            return CLV;

        }
        public async Task<uploaded_data> CreateUploadedData(string checkSum, string currentFile, string extension, string fileLocation, string fileName, short? local, workers worker, string uploaderType, int version, bool createPhysicalFile)
        {
            uploaded_data UD = new uploaded_data();
               
            UD.Checksum = checkSum;
            UD.CreatedAt = DateTime.Now;
            UD.CurrentFile = currentFile;
            UD.ExpirationDate = DateTime.Now.AddYears(1);
            UD.Extension = extension;
            UD.FileLocation = fileLocation;
            UD.FileName = fileName;
            UD.Local = local;
            UD.UpdatedAt = DateTime.Now;
            UD.UploaderId = worker.Id;
            UD.UploaderType = uploaderType;
            UD.Version = version;
            UD.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.uploaded_data.Add(UD);
            await dbContext.SaveChangesAsync();


            string path = System.IO.Path.Combine(fileLocation, fileName);

            if (createPhysicalFile)
            {
                System.IO.Directory.CreateDirectory(fileLocation);
                if (!System.IO.File.Exists(path))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(path))
                    {
                        for (byte i = 0; i < 100; i++)
                        {
                            fs.WriteByte(i);
                        }
                    }
                }
            }
            return UD;
        }
        public async Task<entity_groups> CreateEntityGroup(string microtingUId, string name, string entityType, string workflowState)
        {

            var lists =  dbContext.entity_groups.Where(x => x.MicrotingUid == microtingUId).ToList();

            if (lists.Count == 0)
            {
                entity_groups eG = new entity_groups();

                eG.CreatedAt = DateTime.Now;
                //eG.Id = xxx;
                eG.MicrotingUid = microtingUId;
                eG.Name = name;
                eG.Type = entityType;
                eG.UpdatedAt = DateTime.Now;
                eG.Version = 1;
                eG.WorkflowState = workflowState;

                dbContext.entity_groups.Add(eG);
                await dbContext.SaveChangesAsync();
                return eG;
            } else {
                throw new ArgumentException("microtingUId already exists: " + microtingUId);
            }
            
        }
        public async Task<entity_items> CreateEntityItem(string description, int displayIndex, int entityGroupId, string entityItemUId, string microtingUId, string name, short? synced, int version, string workflowState)
        {
            entity_items eI = new entity_items();
            eI.CreatedAt = DateTime.Now;
            eI.Description = description;
            eI.DisplayIndex = displayIndex;
            eI.EntityGroupId = entityGroupId;
            eI.EntityItemUid = entityItemUId;
            eI.MicrotingUid = microtingUId;
            eI.Name = name;
            eI.Synced = synced;
            eI.UpdatedAt = DateTime.Now;
            eI.Version = version;
            eI.WorkflowState = workflowState;

            dbContext.entity_items.Add(eI);
            await dbContext.SaveChangesAsync();

            return eI;
        }
        public async Task <tags> CreateTag(string name, string workflowState, int version)
        {
            tags tag = new tags();
            tag.Name = name;
            tag.WorkflowState = workflowState;
            tag.Version = version;
            await tag.Create(dbContext);

//            DbContext.tags.Add(tag);
//            await dbContext.SaveChangesAsync();

            return tag;
        }
        public async Task<check_list_sites> CreateCheckListSite(check_lists checklist, DateTime createdAt,
            sites site, DateTime updatedAt, int version, string workflowState, int microting_uid)
        {
            check_list_sites cls = new check_list_sites();
            cls.CheckList = checklist;
            cls.CreatedAt = createdAt;
            cls.Site = site;
            cls.UpdatedAt = updatedAt;
            cls.Version = version;
            cls.WorkflowState = workflowState;
            cls.MicrotingUid = microting_uid;
            await cls.Create(dbContext);
//            DbContext.check_list_sites.Add(cls);
//            await dbContext.SaveChangesAsync();
            return cls;
        }

        public async Task<int> GetRandomInt()
        {
            await Task.Run(() => { }); // TODO FIX ME
            Random random = new Random();
            int i = random.Next(0, int.MaxValue);            
            return i;
        }
        #endregion


    }
}
