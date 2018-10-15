using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Microting.eForm
{
    public static class DbConfig
    {           
        public static bool IsMSSQL
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["eFormDbConnection"] != null)
                {
                    // MySQL Connection string should contain Convert Zero DateTime to handle Null Dates. So ensure MySQL connection
                    // string has this setting
                    if(ConfigurationManager.ConnectionStrings["eFormDbConnection"].ConnectionString.Contains("Convert Zero Datetime"))
                    {
                        return false;
                    }                    
                }
                return true;// for SQL Server
            }
        }
        public static string ConnectionString { get
            {
                if(ConfigurationManager.ConnectionStrings["eFormDbConnection"] !=null)
                {
                    return ConfigurationManager.ConnectionStrings["eFormDbConnection"].ConnectionString;
                }
                else
                {
                    // returns hard coded if app configuration has no db connection strings
                    return "data source=localhost;Initial catalog={0};Integrated Security=True";// for SQL Server
                    //return "Server = localhost; port = 3306; Database = {0}; user = eform; password = eform; Convert Zero Datetime = true;";// for MySQL Server
                }
            }
        }          
        
    }
}
