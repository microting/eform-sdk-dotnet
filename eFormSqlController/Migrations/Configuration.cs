namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MicrotingDbMs>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "eFormSqlController.MicrotingDb";
        }

        protected override void Seed(MicrotingDbMs context)
        {

        }
    }
}