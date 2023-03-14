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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure;
using NUnit.Framework;
using Testcontainers.MariaDb;

namespace eFormSDK.Base.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {
        private readonly MariaDbContainer _mariadbTestcontainer = new MariaDbBuilder()
            .WithDatabase(
                "eformsdk-tests").WithUsername("bla").WithPassword("secretpassword")
            .Build();

        protected MicrotingDbContext DbContext;
        protected string ConnectionString;

        private MicrotingDbContext GetContext(string connectionStr)
        {
            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();

            dbContextOptionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
                new Version(10, 8)));
            var microtingDbContext = new MicrotingDbContext(dbContextOptionsBuilder.Options);
            string file = Path.Combine("SQL", "eformsdk-tests.sql");
            string rawSql = File.ReadAllText(file);

            microtingDbContext.Database.EnsureCreated();
            microtingDbContext.Database.ExecuteSqlRaw(rawSql);
            microtingDbContext.Database.Migrate();

            return microtingDbContext;
        }

        [SetUp]
        public async Task Setup()
        {
            Console.WriteLine($"{DateTime.Now} : Starting MariaDb Container...");
            await _mariadbTestcontainer.StartAsync();
            Console.WriteLine($"{DateTime.Now} : Started MariaDb Container");
            ConnectionString = _mariadbTestcontainer.GetConnectionString();

            DbContext = GetContext(_mariadbTestcontainer.GetConnectionString());

            DbContext.Database.SetCommandTimeout(300);

            try
            {
                //await ClearDb();
            }
            catch
            {
                // ignored
            }

            try
            {
                Core core = new Core();
                Console.WriteLine($"{DateTime.Now} : StartSqlOnly...");
                await core.StartSqlOnly(_mariadbTestcontainer.GetConnectionString());
                Console.WriteLine($"{DateTime.Now} : StartSqlOnly close...");
                await core.Close();
            }
            catch
            {
                AdminTools adminTools = new AdminTools(_mariadbTestcontainer.GetConnectionString());
                Console.WriteLine($"{DateTime.Now} : DbSetup...");
                await adminTools.DbSetup("abc1234567890abc1234567890abcdef");
                Console.WriteLine($"{DateTime.Now} : DbSetup done");
            }

            await DoSetup();
        }

        [TearDown]
        public async Task TearDown()
        {
            Console.WriteLine($"{DateTime.Now} : TearDown...");
            await ClearDb();

            ClearFile();

            await DbContext.DisposeAsync();

            await _mariadbTestcontainer.StopAsync();
        }

        private async Task ClearDb()
        {
            Console.WriteLine($"{DateTime.Now} : ClearDb...");
            List<string> modelNames = new List<string>
            {
                "CaseVersions",
                "Cases",
                "FieldValueVersions",
                "FieldValues",
                "FieldVersions",
                "Fields",
                "FolderVersions",
                "Folders",
                "FolderTranslationVersions",
                "FolderTranslations",
                "CheckListSiteVersions",
                "CheckListSites",
                "CheckListValueVersions",
                "CheckListValues",
                "Taggings",
                "TaggingVersions",
                "Tags",
                "TagVersions",
                "CheckListVersions",
                "CheckLists",
                "EntityGroupVersions",
                "EntityGroups",
                "EntityItemVersions",
                "EntityItems",
                "NotificationVersions",
                "Notifications",
                "SettingVersions",
                "Settings",
                "UnitVersions",
                "Units",
                "SiteWorkerVersions",
                "SiteWorkers",
                "WorkerVersions",
                "Workers",
                "SiteVersions",
                "Sites",
                "UploadedDatas",
                "UploadedDataVersions",
                "FieldTypes",
                "SurveyConfigurations",
                "SurveyConfigurationVersions",
                "SiteSurveyConfigurations",
                "SiteSurveyConfigurationVersions",
                "SiteTagVersions",
                "SiteTags",
                "Languages",
                "LanguageVersions",
                "QuestionSets",
                "QuestionSetVersions",
                "Questions",
                "QuestionVersions",
                "Options",
                "OptionVersions",
                "Answers",
                "AnswerVersions",
                "AnswerValues",
                "AnswerValueVersions",
                "QuestionTranslationVersions",
                "QuestionTranslations",
                "OptionTranslationVersions",
                "OptionTranslations",
                "LanguageQuestionSetVersions",
                "LanguageQuestionSets",
                "CheckListTranslations",
                "CheckListTranslationVersions",
                "FieldTranslations",
                "FieldTranslationVersions",
                "FieldOptions",
                "FieldOptionVersions",
                "FieldOptionTranslations",
                "FieldOptionTranslationVersions"
            };
            bool firstRunNotDone = true;

            foreach (var modelName in modelNames)
            {
                try
                {
                    if (firstRunNotDone)
                    {
                        Console.WriteLine($"{DateTime.Now} : Truncating {modelName}...");
                        await DbContext.Database.ExecuteSqlRawAsync(
                            $"SET FOREIGN_KEY_CHECKS = 0;TRUNCATE `eformsdk-tests`.`{modelName}`");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Unknown database 'eformsdk-tests'")
                    {
                        firstRunNotDone = false;
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private string _path;

        private void ClearFile()
        {
            _path = Assembly.GetExecutingAssembly().Location;
            _path = Path.GetDirectoryName(_path)?.Replace(@"file:", "");

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
            catch
            {
                // ignored
            }
        }
#pragma warning disable 1998
        public virtual async Task DoSetup()
        {
        }
#pragma warning restore 1998
    }
}