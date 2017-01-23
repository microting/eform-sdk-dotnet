using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace eFormOffice
{
    public class ExcelController
    {
        public string CreateExcel(List<List<string>> dataSet, string path, string name)
        {
            Excel.Application xlApp = new Excel.Application();
            xlApp.DisplayAlerts = false;

            if (xlApp == null)
                throw new Exception("CreateExcel failed. Not EXCEL found");

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = "Data dump";

            int lstNum = 1;
            foreach (List<string> lst in dataSet)
            {
                int itemNum = 1;
                foreach (string content in lst)
                {
                    xlWorkSheet.Cells[itemNum, lstNum] = content;

                    itemNum++;
                }
                lstNum++;
            }

            try
            {
                xlWorkBook.SaveAs(path + name, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            catch (Exception ex)
            {
                throw new Exception("CreateExcel failed. Unable to save the generated fil", ex);
            }
            string rStr = xlWorkBook.FullName;

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            return rStr;
        }
    }
}
