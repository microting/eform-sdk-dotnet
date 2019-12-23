using Microting.eForm.Infrastructure.Factories;

namespace Microting.eForm.Infrastructure.Helpers
{
    public class DbContextHelper
    {
        private string ConnectionString { get;}
        
        public DbContextHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public MicrotingDbContext GetDbContext()
        {
            MicrotingDbContextFactory contextFactory = new MicrotingDbContextFactory();

            return contextFactory.CreateDbContext(new[] {ConnectionString});
        }
    }
}