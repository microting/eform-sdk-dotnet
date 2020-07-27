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

namespace eFormSDK.Integration.CoreTests
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
            ConnectionString = @"Server = localhost; port = 3306; Database = eformsdk-tests; user = root; Convert Zero Datetime = true;";

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
            modelNames.Add("case_versions");
            modelNames.Add("cases");
            modelNames.Add("field_value_versions");
            modelNames.Add("field_values");
            modelNames.Add("field_versions");
            modelNames.Add("fields");
            modelNames.Add("folder_versions");
            modelNames.Add("folders");
            modelNames.Add("check_list_site_versions");
            modelNames.Add("check_list_sites");
            modelNames.Add("check_list_value_versions");
            modelNames.Add("check_list_values");
            modelNames.Add("taggings");
            modelNames.Add("tagging_versions");
            modelNames.Add("tags");
            modelNames.Add("tag_versions");
            modelNames.Add("check_list_versions");
            modelNames.Add("check_lists");
            modelNames.Add("entity_group_versions");
            modelNames.Add("entity_groups");
            modelNames.Add("entity_item_versions");
            modelNames.Add("entity_items");
            modelNames.Add("log_exceptions");
            modelNames.Add("logs");
            modelNames.Add("notification_versions");
            modelNames.Add("notifications");
            modelNames.Add("setting_versions");
            modelNames.Add("settings");
            modelNames.Add("unit_versions");
            modelNames.Add("units");
            modelNames.Add("site_worker_versions");
            modelNames.Add("site_workers");
            modelNames.Add("worker_versions");
            modelNames.Add("workers");
            modelNames.Add("site_versions");
            modelNames.Add("sites");
            modelNames.Add("uploaded_data");
            modelNames.Add("uploaded_data_versions");
            modelNames.Add("field_types");
            modelNames.Add("survey_configurations");
            modelNames.Add("survey_configuration_versions");
            modelNames.Add("site_survey_configurations");
            modelNames.Add("site_survey_configuration_versions");
            modelNames.Add("SiteTagVersions");
            modelNames.Add("SiteTags");
            modelNames.Add("languages");
            modelNames.Add("language_versions");
            modelNames.Add("question_sets");
            modelNames.Add("question_set_versions");
            modelNames.Add("questions");
            modelNames.Add("question_versions");
            modelNames.Add("options");
            modelNames.Add("option_versions");
            modelNames.Add("answers");
            modelNames.Add("answer_versions");
            modelNames.Add("answer_values");
            modelNames.Add("answer_value_versions");
            modelNames.Add("QuestionTranslationVersions");
            modelNames.Add("QuestionTranslations");
            modelNames.Add("OptionTranslationVersions");
            modelNames.Add("OptionTranslations");
            modelNames.Add("LanguageQuestionSetVersions");
            modelNames.Add("LanguageQuestionSets");

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