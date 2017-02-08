using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace eFormSqlController
{
    public class MigrationsContextFactory : IDbContextFactory<MicrotingDb>
    {

        public MicrotingDb Create()
        {
            string serverConnectionString =  File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Compiled\input\sql_connection.txt").Trim();

            return new MicrotingDb(serverConnectionString);
        }
    }
}