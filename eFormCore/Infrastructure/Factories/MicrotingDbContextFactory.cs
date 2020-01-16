using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Factories
{
    public class MicrotingDbContextFactory
    {
        public MicrotingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MicrotingDbContext>();
            if (args.Any())
            {
                if (args.FirstOrDefault().ToLower().Contains("convert zero datetime"))
                {
                    optionsBuilder.UseMySql(args.FirstOrDefault());
                }
                else
                {
                    optionsBuilder.UseSqlServer(args.FirstOrDefault());
                }
            }
            else
            {
                throw new ArgumentNullException("Connection string not present");
            }
            //optionsBuilder.UseSqlServer(@"data source=(LocalDb)\SharedInstance;Initial catalog=installationchecking-base-tests;Integrated Security=True");
            //dotnet ef migrations add InitialCreate --project eFormCore/Microting.eForm --startup-project DBMigrator
            optionsBuilder.UseLazyLoadingProxies(true);
            return new MicrotingDbContext(optionsBuilder.Options);
        }
    }
}