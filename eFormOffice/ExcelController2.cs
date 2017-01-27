using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.ComponentModel;

namespace eFormOffice
{
    public class ExcelController2
    {
        public string CreateExcel(List<List<string>> dataSet, string path, string name)
        {
            string fullPath = path + name;

            return "";
        }
    }
}