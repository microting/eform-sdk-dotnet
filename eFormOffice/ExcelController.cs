using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace eFormOffice
{
    public class ExcelController
    {
        public string CreateExcel(List<List<string>> dataSet, string path)
        {
            //Excel.Application excel = new Excel.Application();
            //excel.Visible = true;
            //Excel.Workbook wb = excel.Workbooks.Open("test.xls");
            //Excel.Worksheet sh = wb.Sheets.Add();
            //sh.Name = "TestSheet";
            //sh.Cells[1, "A"].Value2 = "SNO";
            //sh.Cells[2, "B"].Value2 = "A";
            //sh.Cells[2, "C"].Value2 = "1122";
            //wb.Close(true);
            //excel.Quit();

            return path + "test.xls";
        }
    }
}
