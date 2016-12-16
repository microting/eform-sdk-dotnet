namespace eFormCustom
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CustomDb : DbContext
    {
        public CustomDb(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<containers> containers { get; set; }
        public virtual DbSet<factions> factions { get; set; }
        public virtual DbSet<workers> workers { get; set; }
        public virtual DbSet<locations> locations { get; set; }
        public virtual DbSet<sites> sites { get; set; }
        public virtual DbSet<templat_ids> templat_ids { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<containers>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<factions>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<workers>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<workers>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<workers>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<locations>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<sites>()
                .Property(e => e.area)
                .IsUnicode(false);

            modelBuilder.Entity<sites>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<templat_ids>()
                .Property(e => e.name)
                .IsUnicode(false);
        }
    }
}
