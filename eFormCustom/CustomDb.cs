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

        public virtual DbSet<Container_Collection_Entry> Container_Collection_Entry { get; set; }
        public virtual DbSet<input_containers> input_containers { get; set; }
        public virtual DbSet<input_factions> input_factions { get; set; }
        public virtual DbSet<input_locations> input_locations { get; set; }
        public virtual DbSet<sites> sites { get; set; }
        public virtual DbSet<variable> variable { get; set; }
        public virtual DbSet<workers> workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Container_Collection_Entry>()
                .Property(e => e.Placement_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Container_Collection_Entry>()
                .Property(e => e.Weight)
                .IsUnicode(false);

            modelBuilder.Entity<input_containers>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<input_containers>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<input_factions>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<input_factions>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<input_locations>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<input_locations>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<sites>()
                .Property(e => e.area)
                .IsUnicode(false);

            modelBuilder.Entity<sites>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<variable>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<variable>()
                .Property(e => e.value)
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
        }
    }
}
