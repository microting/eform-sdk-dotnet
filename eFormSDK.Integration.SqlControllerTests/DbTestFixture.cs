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
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure;
using NUnit.Framework;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace eFormSDK.Integration.SqlControllerTests
{
    [TestFixture]
    public abstract class DbTestFixture
    {

        protected MicrotingDbContext DbContext;
        protected string ConnectionString;

        private MicrotingDbContext GetContext(string connectionStr)
        {

            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();

            dbContextOptionsBuilder.UseMySql(connectionStr, mysqlOptions =>
            {
                mysqlOptions.ServerVersion(new Version(10, 4, 0), ServerType.MariaDb);
            });
            dbContextOptionsBuilder.UseLazyLoadingProxies(true);
            return new MicrotingDbContext(dbContextOptionsBuilder.Options);

        }

        [SetUp]
        public async Task Setup()
        {
            ConnectionString = @"Server = localhost; port = 3306; Database = eformsdk-tests; user = root; password = 'secretpassword'; Convert Zero Datetime = true;";

            DbContext = GetContext(ConnectionString);

            DbContext.Database.SetCommandTimeout(300);

            try
            {
                await ClearDb();
            }
            catch
            {
            }
            try
            {
                Core core = new Core();
                await core.StartSqlOnly(ConnectionString);
                await core.Close();
            } catch
            {
                AdminTools adminTools = new AdminTools(ConnectionString);
                await adminTools.DbSetup("abc1234567890abc1234567890abcdef");
            }

            await DoSetup();
        }

        [TearDown]
        public async Task TearDown()
        {

            await ClearDb();

            ClearFile();

            DbContext.Dispose();
        }

        private async Task ClearDb()
        {

            List<string> modelNames = new List<string>();
            modelNames.Add("CaseVersions");
            modelNames.Add("Cases");
            modelNames.Add("FieldValueVersions");
            modelNames.Add("FieldValues");
            modelNames.Add("FieldVersions");
            modelNames.Add("Fields");
            modelNames.Add("FolderVersions");
            modelNames.Add("Folders");
            modelNames.Add("CheckListSiteVersions");
            modelNames.Add("CheckListSites");
            modelNames.Add("CheckListValueVersions");
            modelNames.Add("CheckListValues");
            modelNames.Add("Taggings");
            modelNames.Add("TaggingVersions");
            modelNames.Add("Tags");
            modelNames.Add("TagVersions");
            modelNames.Add("CheckListVersions");
            modelNames.Add("CheckLists");
            modelNames.Add("EntityGroupVersions");
            modelNames.Add("EntityGroups");
            modelNames.Add("EntityItemVersions");
            modelNames.Add("EntityItems");
            modelNames.Add("NotificationVersions");
            modelNames.Add("Notifications");
            modelNames.Add("SettingVersions");
            modelNames.Add("Settings");
            modelNames.Add("UnitVersions");
            modelNames.Add("Units");
            modelNames.Add("SiteWorkerVersions");
            modelNames.Add("SiteWorkers");
            modelNames.Add("WorkerVersions");
            modelNames.Add("Workers");
            modelNames.Add("SiteVersions");
            modelNames.Add("Sites");
            modelNames.Add("UploadedDatas");
            modelNames.Add("UploadedDataVersions");
            modelNames.Add("FieldTypes");
            modelNames.Add("SurveyConfigurations");
            modelNames.Add("SurveyConfigurationVersions");
            modelNames.Add("SiteSurveyConfigurations");
            modelNames.Add("SiteSurveyConfigurationVersions");
            modelNames.Add("SiteTagVersions");
            modelNames.Add("SiteTags");
            modelNames.Add("Languages");
            modelNames.Add("LanguageVersions");
            modelNames.Add("QuestionSets");
            modelNames.Add("QuestionSetVersions");
            modelNames.Add("Questions");
            modelNames.Add("QuestionVersions");
            modelNames.Add("Options");
            modelNames.Add("OptionVersions");
            modelNames.Add("Answers");
            modelNames.Add("AnswerVersions");
            modelNames.Add("AnswerValues");
            modelNames.Add("AnswerValueVersions");
            modelNames.Add("QuestionTranslationVersions");
            modelNames.Add("QuestionTranslations");
            modelNames.Add("OptionTranslationVersions");
            modelNames.Add("OptionTranslations");
            modelNames.Add("LanguageQuestionSetVersions");
            modelNames.Add("LanguageQuestionSets");
            modelNames.Add("CheckLisTranslations");
            modelNames.Add("CheckListTranslationVersions");
            modelNames.Add("FieldTranslations");
            modelNames.Add("FieldTranslationVersions");
            modelNames.Add("FieldOptions");
            modelNames.Add("FieldOptionVersions");
            modelNames.Add("FieldOptionTranslations");
            modelNames.Add("FieldOptionTranslationVersions");

            foreach (var modelName in modelNames)
            {
                try
                {
                    await DbContext.Database.ExecuteSqlRawAsync($"SET FOREIGN_KEY_CHECKS = 0;TRUNCATE `eformsdk-tests`.`{modelName}`");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private string _path;

        private void ClearFile()
        {
            _path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            _path = System.IO.Path.GetDirectoryName(_path).Replace(@"file:", "");

            string picturePath;


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                picturePath = _path + @"\output\dataFolder\picture\Deleted";
            }
            else
            {
                picturePath = _path + @"/output/dataFolder/picture/Deleted";
            }

            DirectoryInfo diPic = new DirectoryInfo(picturePath);

            try
            {
                foreach (FileInfo file in diPic.GetFiles())
                {
                    file.Delete();
                }
            }
            catch { }


        }
#pragma warning disable 1998
        public virtual async Task DoSetup() { }
#pragma warning restore 1998

    }
}