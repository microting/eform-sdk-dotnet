namespace eFormSqlController
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MicrotingDb : DbContext
    {
        public MicrotingDb(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<cases> cases { get; set; }
        public virtual DbSet<check_list_sites> check_list_sites { get; set; }
        public virtual DbSet<check_list_values> check_list_values { get; set; }
        public virtual DbSet<check_lists> check_lists { get; set; }
        public virtual DbSet<data_uploaded> data_uploaded { get; set; }
        public virtual DbSet<entity_groups> entity_groups { get; set; }
        public virtual DbSet<entity_items> entity_items { get; set; }
        public virtual DbSet<field_types> field_types { get; set; }
        public virtual DbSet<field_values> field_values { get; set; }
        public virtual DbSet<fields> fields { get; set; }
        public virtual DbSet<notifications> notifications { get; set; }
        public virtual DbSet<outlook> outlook { get; set; }
        public virtual DbSet<sites> sites { get; set; }
        public virtual DbSet<version_cases> version_cases { get; set; }
        public virtual DbSet<version_check_list_sites> version_check_list_sites { get; set; }
        public virtual DbSet<version_check_list_values> version_check_list_values { get; set; }
        public virtual DbSet<version_check_lists> version_check_lists { get; set; }
        public virtual DbSet<version_data_uploaded> version_data_uploaded { get; set; }
        public virtual DbSet<version_entity_groups> version_entity_groups { get; set; }
        public virtual DbSet<version_entity_items> version_entity_items { get; set; }
        public virtual DbSet<version_field_values> version_field_values { get; set; }
        public virtual DbSet<version_fields> version_fields { get; set; }
        public virtual DbSet<version_sites> version_sites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cases>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.done_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.microting_check_uid)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.case_uid)
                .IsUnicode(false);

            modelBuilder.Entity<cases>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_sites>()
                .Property(e => e.last_check_id)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_values>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_values>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<check_list_values>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_values>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.case_type)
                .IsUnicode(false);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.folder_name)
                .IsUnicode(false);

            modelBuilder.Entity<data_uploaded>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<data_uploaded>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<data_uploaded>()
                .Property(e => e.expiration_date)
                .HasPrecision(0);

            modelBuilder.Entity<entity_groups>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<entity_groups>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<entity_groups>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<entity_groups>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<entity_groups>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<entity_groups>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.entity_group_id)
                .IsUnicode(false);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.entity_item_uid)
                .IsUnicode(false);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<entity_items>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<field_types>()
                .Property(e => e.field_type)
                .IsUnicode(false);

            modelBuilder.Entity<field_types>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<field_values>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<field_values>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.done_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.date)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<field_values>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<field_values>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<field_values>()
                .Property(e => e.altitude)
                .IsUnicode(false);

            modelBuilder.Entity<field_values>()
                .Property(e => e.heading)
                .IsUnicode(false);

            modelBuilder.Entity<field_values>()
                .Property(e => e.accuracy)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<fields>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<fields>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.color)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.default_value)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.unit_name)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.min_value)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.max_value)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.barcode_type)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.query_type)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.key_value_pair_list)
                .IsUnicode(false);

            modelBuilder.Entity<fields>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<notifications>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<notifications>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<notifications>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<notifications>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<notifications>()
                .Property(e => e.transmission)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<outlook>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<outlook>()
                .Property(e => e.global_id)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.start_at)
                .HasPrecision(0);

            modelBuilder.Entity<outlook>()
                .Property(e => e.expire_at)
                .HasPrecision(0);

            modelBuilder.Entity<outlook>()
                .Property(e => e.subject)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.body)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.site_ids)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.info)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.custom_fields)
                .IsUnicode(false);

            modelBuilder.Entity<outlook>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<sites>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<sites>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.done_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.microting_check_uid)
                .IsUnicode(false);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.case_uid)
                .IsUnicode(false);

            modelBuilder.Entity<version_cases>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_list_sites>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_list_sites>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_check_list_sites>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_check_list_sites>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_list_sites>()
                .Property(e => e.last_check_id)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_list_values>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_list_values>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_list_values>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_check_list_values>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.case_type)
                .IsUnicode(false);

            modelBuilder.Entity<version_check_lists>()
                .Property(e => e.folder_name)
                .IsUnicode(false);

            modelBuilder.Entity<version_data_uploaded>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_data_uploaded>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_data_uploaded>()
                .Property(e => e.expiration_date)
                .HasPrecision(0);

            modelBuilder.Entity<version_entity_groups>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_groups>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_entity_groups>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_entity_groups>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_groups>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_groups>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.entity_group_id)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.entity_item_uid)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.microting_uid)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<version_entity_items>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.done_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.date)
                .HasPrecision(0);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.altitude)
                .IsUnicode(false);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.heading)
                .IsUnicode(false);

            modelBuilder.Entity<version_field_values>()
                .Property(e => e.accuracy)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.workflow_state)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.label)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.color)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.default_value)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.unit_name)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.min_value)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.max_value)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.barcode_type)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.query_type)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.key_value_pair_list)
                .IsUnicode(false);

            modelBuilder.Entity<version_fields>()
                .Property(e => e.custom)
                .IsUnicode(false);

            modelBuilder.Entity<version_sites>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<version_sites>()
                .Property(e => e.updated_at)
                .HasPrecision(0);
        }
    }
}
