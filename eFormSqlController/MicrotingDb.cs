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

        public virtual DbSet<case_versions> case_versions { get; set; }
        public virtual DbSet<cases> cases { get; set; }
        public virtual DbSet<check_list_value_versions> check_list_value_versions { get; set; }
        public virtual DbSet<check_list_values> check_list_values { get; set; }
        public virtual DbSet<check_list_versions> check_list_versions { get; set; }
        public virtual DbSet<check_lists> check_lists { get; set; }
        public virtual DbSet<field_types> field_types { get; set; }
        public virtual DbSet<field_value_versions> field_value_versions { get; set; }
        public virtual DbSet<field_values> field_values { get; set; }
        public virtual DbSet<field_versions> field_versions { get; set; }
        public virtual DbSet<fields> fields { get; set; }
        public virtual DbSet<notifications> notifications { get; set; }
        public virtual DbSet<site_versions> site_versions { get; set; }
        public virtual DbSet<sites> sites { get; set; }
        public virtual DbSet<uploaded_data> uploaded_data { get; set; }
        public virtual DbSet<uploaded_data_versions> uploaded_data_versions { get; set; }
        public virtual DbSet<user_versions> user_versions { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<case_versions>()
                .Property(e => e.date_of_doing)
                .HasPrecision(0);

            modelBuilder.Entity<case_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<case_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<case_versions>()
                .Property(e => e.navision_time)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.date_of_doing)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<cases>()
                .Property(e => e.navision_time)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_value_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_value_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_values>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_values>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_list_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<check_lists>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_types>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_types>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_value_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_value_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_value_versions>()
                .Property(e => e.date)
                .HasPrecision(0);

            modelBuilder.Entity<field_value_versions>()
                .Property(e => e.date_of_doing)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.date)
                .HasPrecision(0);

            modelBuilder.Entity<field_values>()
                .Property(e => e.date_of_doing)
                .HasPrecision(0);

            modelBuilder.Entity<field_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<field_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<fields>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<fields>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<site_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<site_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<sites>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<sites>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<uploaded_data>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<uploaded_data>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<uploaded_data>()
                .Property(e => e.expiration_date)
                .HasPrecision(0);

            modelBuilder.Entity<uploaded_data_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<uploaded_data_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<uploaded_data_versions>()
                .Property(e => e.expiration_date)
                .HasPrecision(0);

            modelBuilder.Entity<user_versions>()
                .Property(e => e.reset_password_sent_at)
                .HasPrecision(0);

            modelBuilder.Entity<user_versions>()
                .Property(e => e.remember_created_at)
                .HasPrecision(0);

            modelBuilder.Entity<user_versions>()
                .Property(e => e.current_sign_in_at)
                .HasPrecision(0);

            modelBuilder.Entity<user_versions>()
                .Property(e => e.last_sign_in_at)
                .HasPrecision(0);

            modelBuilder.Entity<user_versions>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<user_versions>()
                .Property(e => e.updated_at)
                .HasPrecision(0);

            modelBuilder.Entity<users>()
                .Property(e => e.reset_password_sent_at)
                .HasPrecision(0);

            modelBuilder.Entity<users>()
                .Property(e => e.remember_created_at)
                .HasPrecision(0);

            modelBuilder.Entity<users>()
                .Property(e => e.current_sign_in_at)
                .HasPrecision(0);

            modelBuilder.Entity<users>()
                .Property(e => e.last_sign_in_at)
                .HasPrecision(0);

            modelBuilder.Entity<users>()
                .Property(e => e.created_at)
                .HasPrecision(0);

            modelBuilder.Entity<users>()
                .Property(e => e.updated_at)
                .HasPrecision(0);
        }
    }
}
