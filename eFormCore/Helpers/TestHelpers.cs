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
                connectionString = @"Server = localhost; port = 3306; Database = eformsdk-tests; user = root; password = 'secretpassword'; Convert Zero Datetime = true;";
//            }

            dbContext = GetContext(connectionString);
        }
        private MicrotingDbContext GetContext(string connectionStr)
        {

            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();

            // if (connectionStr.ToLower().Contains("convert zero datetime"))
            // {
                dbContextOptionsBuilder.UseMySql(connectionStr);
            // }
            // else
            // {
                // dbContextOptionsBuilder.UseSqlServer(connectionStr);
            // }
            dbContextOptionsBuilder.UseLazyLoadingProxies(true);
            return new MicrotingDbContext(dbContextOptionsBuilder.Options);

        }
        #region helperMethods
        public async Task<Worker> CreateWorker(string email, string firstName, string lastName, int microtingUId)
        {
            Worker worker = new Worker();
            worker.FirstName = firstName;
            worker.LastName = lastName;
            worker.Email = email;
            worker.CreatedAt = DateTime.UtcNow;
            worker.UpdatedAt = DateTime.UtcNow;
            worker.MicrotingUid = microtingUId;
            worker.WorkflowState = Constants.WorkflowStates.Created;
            worker.Version = 69;
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return worker;
        }
        public async Task<Site> CreateSite(string name, int microtingUId)
        {

            Site site = new Site();
            site.Name = name;
            site.MicrotingUid = microtingUId;
            site.UpdatedAt = DateTime.UtcNow;
            site.CreatedAt = DateTime.UtcNow;
            site.Version = 64;
            site.WorkflowState = Constants.WorkflowStates.Created;
            dbContext.Sites.Add(site);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return site;
        }
        public async Task<Unit> CreateUnit(int microtingUId, int otpCode, Site site, int customerNo)
        {

            Unit unit = new Unit();
            unit.MicrotingUid = microtingUId;
            unit.OtpCode = otpCode;
            unit.Site = site;
            unit.SiteId = site.Id;
            unit.CreatedAt = DateTime.UtcNow;
            unit.CustomerNo = customerNo;
            unit.UpdatedAt = DateTime.UtcNow;
            unit.Version = 9;
            unit.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.Units.Add(unit);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return unit;
        }
        public async Task<SiteWorker> CreateSiteWorker(int microtingUId, Site site, Worker worker)
        {
            SiteWorker siteWorker = new SiteWorker();
            siteWorker.CreatedAt = DateTime.UtcNow;
            siteWorker.MicrotingUid = microtingUId;
            siteWorker.UpdatedAt = DateTime.UtcNow;
            siteWorker.Version = 63;
            siteWorker.Site = site;
            siteWorker.SiteId = site.Id;
            siteWorker.Worker = worker;
            siteWorker.WorkerId = worker.Id;
            siteWorker.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.SiteWorkers.Add(siteWorker);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return siteWorker;
        }
        public async Task<CheckList> CreateTemplate(DateTime cl_ca, DateTime cl_ua, string label, string description, string caseType, string folderName, int displayIndex, int repeated)
        {
            
            CheckList cl1 = new CheckList();
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
            
            dbContext.CheckLists.Add(cl1);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return cl1;
        }
        public async Task<CheckList> CreateSubTemplate(string label, string description, string caseType, int displayIndex, int repeated, CheckList parentId)
        {
            CheckList cl2 = new CheckList();
            cl2.CreatedAt = DateTime.UtcNow;
            cl2.UpdatedAt = DateTime.UtcNow;
            cl2.Label = label;
            cl2.Description = description;
            cl2.WorkflowState = Constants.WorkflowStates.Created;
            cl2.CaseType = caseType;
            cl2.DisplayIndex = displayIndex;
            cl2.Repeated = repeated;
            cl2.ParentId = parentId.Id;

            dbContext.CheckLists.Add(cl2);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return cl2;
        }
        public async Task<Field> CreateField(short? barcodeEnabled, string barcodeType, CheckList checkList, string color, string custom, int? decimalCount, string defaultValue, string description, int? displayIndex, short? dummy, FieldType ft, short? geolocationEnabled, short? geolocationForced, short? geolocationHidden, short? isNum, string label, short? mandatory, int maxLength, string maxValue, string minValue, short? multi, short? optional, string queryType, short? readOnly, short? selected, short? split, short? stopOnSave, string unitName, int version)
        {

            Field f = new Field();
            f.FieldTypeId = ft.Id;

            f.BarcodeEnabled = barcodeEnabled;
            f.BarcodeType = barcodeType;
            f.CheckListId = checkList.Id;
            f.Color = color;
            f.CreatedAt = DateTime.UtcNow;
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
            f.Split = split;
            f.StopOnSave = stopOnSave;
            f.UnitName = unitName;
            f.UpdatedAt = DateTime.UtcNow;
            f.Version = version;
            f.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.Fields.Add(f);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            Thread.Sleep(2000);

            return f;
        }
        public async Task<Case> CreateCase(string caseUId, CheckList checkList, DateTime created_at, string custom, DateTime done_at, Worker doneByUserId, int microtingCheckId, int microtingUId, Site site, int? status, string caseType, Unit unit, DateTime updated_at, int version, Worker worker, string WorkFlowState)
        {

            Case aCase = new Case
            {
                CaseUid = caseUId,
                CheckList = checkList,
                CheckListId = checkList.Id,
                CreatedAt = created_at,
                Custom = custom,
                DoneAt = done_at,
                WorkerId = worker.Id,
                MicrotingCheckUid = microtingCheckId,
                MicrotingUid = microtingUId,
                Site = site,
                SiteId = site.Id,
                Status = status,
                Type = caseType,
                Unit = unit,
                UnitId = unit.Id,
                UpdatedAt = updated_at,
                Version = version,
                Worker = worker,
                WorkflowState = WorkFlowState
            };

            dbContext.Cases.Add(aCase);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return aCase;
        }
        public async Task<FieldValue> CreateFieldValue(Case aCase, CheckList checkList, Field f, int? ud_id, int? userId, string value, int? version, Worker worker)
        {
            FieldValue fv = new FieldValue();
            fv.CaseId = aCase.Id;
            fv.CheckList = checkList;
            fv.CheckListId = checkList.Id;
            fv.CreatedAt = DateTime.UtcNow;
            fv.Date = DateTime.UtcNow;
            fv.DoneAt = DateTime.UtcNow;
            fv.Field = f;
            fv.FieldId = f.Id;
            fv.UpdatedAt = DateTime.UtcNow;
            if (ud_id != null)
            {
                fv.UploadedDataId = ud_id;
            }
            fv.WorkerId = userId;
            fv.Value = value;
            fv.Version = version;
            fv.Worker = worker;
            fv.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.FieldValues.Add(fv);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return fv;
        }
        public async Task<CheckListValue> CreateCheckListValue(Case aCase, CheckList checkList, string status, int? userId, int? version)
        {
            CheckListValue CLV = new CheckListValue();

            CLV.CaseId = aCase.Id;
            CLV.CheckListId = checkList.Id;
            CLV.CreatedAt = DateTime.UtcNow;
            CLV.Status = status;
            CLV.UpdatedAt = DateTime.UtcNow;
            CLV.UserId = userId;
            CLV.Version = version;
            CLV.WorkflowState = Constants.WorkflowStates.Created;
            
            dbContext.CheckListValues.Add(CLV);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return CLV;

        }
        public async Task<UploadedData> CreateUploadedData(string checkSum, string currentFile, string extension, string fileLocation, string fileName, short? local, Worker worker, string uploaderType, int version, bool createPhysicalFile)
        {
            UploadedData UD = new UploadedData();
               
            UD.Checksum = checkSum;
            UD.CreatedAt = DateTime.UtcNow;
            UD.CurrentFile = currentFile;
            UD.ExpirationDate = DateTime.UtcNow.AddYears(1);
            UD.Extension = extension;
            UD.FileLocation = fileLocation;
            UD.FileName = fileName;
            UD.Local = local;
            UD.UpdatedAt = DateTime.UtcNow;
            UD.UploaderId = worker.Id;
            UD.UploaderType = uploaderType;
            UD.Version = version;
            UD.WorkflowState = Constants.WorkflowStates.Created;

            dbContext.UploadedDatas.Add(UD);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);


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
        public async Task<EntityGroup> CreateEntityGroup(string microtingUId, string name, string entityType, string workflowState)
        {

            var lists =  dbContext.EntityGroups.Where(x => x.MicrotingUid == microtingUId).ToList();

            if (lists.Count == 0)
            {
                EntityGroup eG = new EntityGroup();

                eG.CreatedAt = DateTime.UtcNow;
                //eG.Id = xxx;
                eG.MicrotingUid = microtingUId;
                eG.Name = name;
                eG.Type = entityType;
                eG.UpdatedAt = DateTime.UtcNow;
                eG.Version = 1;
                eG.WorkflowState = workflowState;

                dbContext.EntityGroups.Add(eG);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                return eG;
            } else {
                throw new ArgumentException("microtingUId already exists: " + microtingUId);
            }
            
        }
        public async Task<EntityItem> CreateEntityItem(string description, int displayIndex, int entityGroupId, string entityItemUId, string microtingUId, string name, short? synced, int version, string workflowState)
        {
            EntityItem eI = new EntityItem();
            eI.CreatedAt = DateTime.UtcNow;
            eI.Description = description;
            eI.DisplayIndex = displayIndex;
            eI.EntityGroupId = entityGroupId;
            eI.EntityItemUid = entityItemUId;
            eI.MicrotingUid = microtingUId;
            eI.Name = name;
            eI.Synced = synced;
            eI.UpdatedAt = DateTime.UtcNow;
            eI.Version = version;
            eI.WorkflowState = workflowState;

            dbContext.EntityItems.Add(eI);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return eI;
        }
        public async Task <Tag> CreateTag(string name, string workflowState, int version)
        {
            Tag tag = new Tag();
            tag.Name = name;
            tag.WorkflowState = workflowState;
            tag.Version = version;
            await tag.Create(dbContext).ConfigureAwait(false);

//            DbContext.tags.Add(tag);
//            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return tag;
        }
        public async Task<CheckListSite> CreateCheckListSite(CheckList checklist, DateTime createdAt,
            Site site, DateTime updatedAt, int version, string workflowState, int microting_uid)
        {
            CheckListSite cls = new CheckListSite();
            cls.CheckList = checklist;
            cls.CreatedAt = createdAt;
            cls.Site = site;
            cls.UpdatedAt = updatedAt;
            cls.Version = version;
            cls.WorkflowState = workflowState;
            cls.MicrotingUid = microting_uid;
            await cls.Create(dbContext).ConfigureAwait(false);
//            DbContext.check_list_sites.Add(cls);
//            await dbContext.SaveChangesAsync().ConfigureAwait(false);
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
