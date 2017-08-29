namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MicrotingDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "eFormSqlController.MicrotingDb";
        }

        protected override void Seed(MicrotingDb context)
        {

        }
    }
}
