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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microting.eForm.Infrastructure.Data.Entities;

namespace Microting.eForm.Infrastructure
{
    public class MicrotingDbContext : DbContext
    {
        public MicrotingDbContext()
        {
        }

        public MicrotingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<AnswerValue> AnswerValues { get; set; }
        public virtual DbSet<AnswerValueVersion> AnswerValueVersions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AnswerVersion> AnswerVersions { get; set; }
        public virtual DbSet<CaseVersion> CaseVersions { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CheckListSiteVersion> CheckListSiteVersions { get; set; }
        public virtual DbSet<CheckListSite> CheckListSites { get; set; }
        public virtual DbSet<CheckListValueVersion> CheckListValueVersions { get; set; }
        public virtual DbSet<CheckListValue> CheckListValues { get; set; }
        public virtual DbSet<CheckListVersion> CheckListVersions { get; set; }
        public virtual DbSet<CheckList> CheckLists { get; set; }
        public virtual DbSet<EntityGroupVersion> EntityGroupVersions { get; set; }
        public virtual DbSet<EntityGroup> EntityGroups { get; set; }
        public virtual DbSet<EntityItemVersion> EntityItemVersions { get; set; }
        public virtual DbSet<EntityItem> EntityItems { get; set; }
        public virtual DbSet<FieldType> FieldTypes { get; set; }
        public virtual DbSet<FieldValueVersion> FieldValueVersions { get; set; }
        public virtual DbSet<FieldValue> FieldValues { get; set; }
        public virtual DbSet<ExtraFieldValue> ExtraFieldValues { get; set; }
        public virtual DbSet<ExtraFieldValueVersion> ExtraFieldValueVersions { get; set; }
        public virtual DbSet<FieldVersion> FieldVersions { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageVersion> LanguageVersions { get; set; }
        public virtual DbSet<LanguageQuestionSet> LanguageQuestionSets { get; set; }
        public virtual DbSet<LanguageQuestionSetVersion> LanguageQuestionSetVersions { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationVersion> NotificationVersions { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<OptionVersion> OptionVersions { get; set; }
        public virtual DbSet<OptionTranslation> OptionTranslations { get; set; }
        public virtual DbSet<OptionTranslationVersion> OptionTranslationVersions { get; set; }
        public virtual DbSet<QuestionSet> QuestionSets { get; set; }
        public virtual DbSet<QuestionSetVersion> QuestionSetVersions { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionVersion> QuestionVersions { get; set; }
        public virtual DbSet<QuestionTranslation> QuestionTranslations { get; set; }
        public virtual DbSet<QuestionTranslationVersion> QuestionTranslationVersions { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SettingVersion> SettingVersions { get; set; }
        public virtual DbSet<SiteSurveyConfiguration> SiteSurveyConfigurations { get; set; }
        public virtual DbSet<SiteSurveyConfigurationVersion> SiteSurveyConfigurationVersions { get; set; }
        public virtual DbSet<SiteVersion> SiteVersions { get; set; }
        public virtual DbSet<SiteWorkerVersion> SiteWorkerVersions { get; set; }
        public virtual DbSet<SiteWorker> SiteWorkers { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SurveyConfiguration> SurveyConfigurations { get; set; }
        public virtual DbSet<SurveyConfigurationVersion> SurveyConfigurationVersions { get; set; }
        public virtual DbSet<UnitVersion> UnitVersions { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<UploadedData> UploadedDatas { get; set; }
        public virtual DbSet<UploadedDataVersion> UploadedDataVersions { get; set; }
        public virtual DbSet<WorkerVersion> WorkerVersions { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagVersion> TagVersions { get; set; }
        public virtual DbSet<Tagging> Taggings { get; set; }
        public virtual DbSet<TaggingVersion> TaggingVersions { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<FolderVersion> FolderVersions { get; set; }
        public virtual DbSet<FolderTranslation> FolderTranslations { get; set; }
        public virtual DbSet<FolderTranslationVersion> FolderTranslationVersions { get; set; }
        public virtual DbSet<SiteTag> SiteTags { get; set; }
        public virtual DbSet<SiteTagVersion> SiteTagVersions { get; set; }
        public virtual DbSet<CheckListTranslation> CheckListTranslations { get; set; }
        public virtual DbSet<CheckListTranslationVersion> CheckListTranslationVersions { get; set; }
        public virtual DbSet<FieldTranslation> FieldTranslations { get; set; }
        public virtual DbSet<FieldTranslationVersion> FieldTranslationVersions { get; set; }
        public virtual DbSet<FieldOption> FieldOptions { get; set; }
        public virtual DbSet<FieldOptionVersion> FieldOptionVersions { get; set; }
        public virtual DbSet<FieldOptionTranslation> FieldOptionTranslations { get; set; }
        public virtual DbSet<FieldOptionTranslationVersion> FieldOptionTranslationVersions { get; set; }

        public virtual DatabaseFacade ContextDatabase
        {
            get => base.Database;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618

            modelBuilder.Entity<CheckList>(entity =>
            {
                entity.HasOne(d => d.Parent).WithMany(
                    p => p.Children).HasForeignKey(d => d.ParentId);
            });

            modelBuilder.Entity<Field>(entity =>
            {
                entity.HasOne(d => d.Parent).WithMany(
                    p => p.Children).HasForeignKey(d => d.ParentFieldId);
            });

            modelBuilder.Entity<Case>().HasIndex(p => new { p.MicrotingUid, p.MicrotingCheckUid });

#pragma warning restore 612, 618
        }

//        #region DefineLoggerFactory
//        public static readonly LoggerFactory MyLoggerFactory
//            = new LoggerFactory(new[] {new ConsoleLoggerProvider((_, __) => true, true)});
//        #endregion


        // dotnet ef migrations add AddingNewModels --project eFormCore --startup-project DBMigrator
//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
//                 optionsBuilder.UseSqlServer(@"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests;Integrated Security=True");
//             }
//
// //            optionsBuilder.EnableSensitiveDataLogging();
// //            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
//         }
    }
}