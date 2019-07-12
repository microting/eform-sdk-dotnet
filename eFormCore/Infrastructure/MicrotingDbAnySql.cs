/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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
using Microting.eForm.Infrastructure.Data.Entities;

namespace Microting.eForm.Infrastructure
{
    public partial class MicrotingDbAnySql : DbContext
    {
        public MicrotingDbAnySql() { }

        public MicrotingDbAnySql(DbContextOptions options)
          : base(options)
        {
           
        }
       
        public virtual DbSet<answer_values> answer_values { get; set; }
        public virtual DbSet<answer_value_versions> answer_value_versions { get; set; }
        public virtual DbSet<answers> answers { get; set; }
        public virtual DbSet<answer_versions> answer_versions { get; set; }
        public virtual DbSet<case_versions> case_versions { get; set; }
        public virtual DbSet<cases> cases { get; set; }
        public virtual DbSet<check_list_site_versions> check_list_site_versions { get; set; }
        public virtual DbSet<check_list_sites> check_list_sites { get; set; }
        public virtual DbSet<check_list_value_versions> check_list_value_versions { get; set; }
        public virtual DbSet<check_list_values> check_list_values { get; set; }
        public virtual DbSet<check_list_versions> check_list_versions { get; set; }
        public virtual DbSet<check_lists> check_lists { get; set; }
        public virtual DbSet<entity_group_versions> entity_group_versions { get; set; }
        public virtual DbSet<entity_groups> entity_groups { get; set; }
        public virtual DbSet<entity_item_versions> entity_item_versions { get; set; }
        public virtual DbSet<entity_items> entity_items { get; set; }
        public virtual DbSet<field_types> field_types { get; set; }
        public virtual DbSet<field_value_versions> field_value_versions { get; set; }
        public virtual DbSet<field_values> field_values { get; set; }
        public virtual DbSet<field_versions> field_versions { get; set; }
        public virtual DbSet<fields> fields { get; set; }
        public virtual DbSet<languages> languages { get; set; }
        public virtual DbSet<language_versions> language_versions { get; set; }
        public virtual DbSet<log_exceptions> log_exceptions { get; set; }
        public virtual DbSet<logs> logs { get; set; }
        public virtual DbSet<notifications> notifications { get; set; }
        public virtual DbSet<options> options { get; set; }
        public virtual DbSet<option_versions> option_versions { get; set; }
        public virtual DbSet<question_sets> question_sets { get; set; }
        public virtual DbSet<question_set_versions> question_set_versions { get; set; }
        public virtual DbSet<questions> questions { get; set; }
        public virtual DbSet<question_versions> question_versions { get; set; }
        public virtual DbSet<settings> settings { get; set; }
        public virtual DbSet<site_survey_configurations> site_survey_configurations { get; set; }
        public virtual DbSet<site_survey_configuration_versions> site_survey_configuration_versions { get; set; }
        public virtual DbSet<site_versions> site_versions { get; set; }
        public virtual DbSet<site_worker_versions> site_worker_versions { get; set; }
        public virtual DbSet<site_workers> site_workers { get; set; }
        public virtual DbSet<sites> sites { get; set; }
        public virtual DbSet<survey_configurations> survey_configurations { get; set; }
        public virtual DbSet<survey_configuration_versions> survey_configuration_versions { get; set; }
        public virtual DbSet<unit_versions> unit_versions { get; set; }
        public virtual DbSet<units> units { get; set; }
        public virtual DbSet<uploaded_data> uploaded_data { get; set; }
        public virtual DbSet<uploaded_data_versions> uploaded_data_versions { get; set; }
        public virtual DbSet<worker_versions> worker_versions { get; set; }
        public virtual DbSet<workers> workers { get; set; }
        public virtual DbSet<tags> tags { get; set; }
        public virtual DbSet<tag_versions> tag_versions { get; set; }
        public virtual DbSet<taggings> taggings { get; set; }
        public virtual DbSet<tagging_versions> tagging_versions { get; set; }
        public virtual DbSet<folders> folders { get; set; }
        public virtual DbSet<folder_versions> folder_versions { get; set; }

        public virtual Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade ContextDatabase
        {
            get => base.Database;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618

            modelBuilder.Entity<check_lists>(entity =>
            {
                entity.HasOne(d => d.Parent).WithMany(p => p.Children).HasForeignKey(d => d.ParentId);
            });

            modelBuilder.Entity<fields>(entity =>
            {
                entity.HasOne(d => d.Parent).WithMany(p => p.Children).HasForeignKey(d => d.ParentFieldId);
            });

#pragma warning restore 612, 618
        }

        // dotnet ef migrations add AddingNewModels --project eFormCore --startup-project DBMigrator
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests;Integrated Security=True");
            }
        }
    }
}