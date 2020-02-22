/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microting.eForm.Infrastructure.Models;
//
//namespace Microting.eForm.Helpers
//{
//    public static class ValidationHelper
//    {
//    
//        private async Task<List<string>> FieldValidation(MainElement mainElement)
//        {
//            string methodName = "Core.FieldValidation";
//
//            await log.LogStandard(methodName, "called");
//
//            List<string> errorLst = new List<string>();
//            var dataItems = mainElement.DataItemGetAll();
//
//            foreach (var dataItem in dataItems)
//            {
//                #region entities
//
//                if (dataItem.GetType() == typeof(EntitySearch))
//                {
//                    EntitySearch entitySearch = (EntitySearch) dataItem;
//                    var temp = _sqlController.EntityGroupRead(entitySearch.EntityTypeId.ToString());
//                    if (temp == null)
//                        errorLst.Add("Element entitySearch.EntityTypeId:'" + entitySearch.EntityTypeId +
//                                     "' is an reference to a local unknown EntitySearch group. Please update reference");
//                }
//
//                if (dataItem.GetType() == typeof(EntitySelect))
//                {
//                    EntitySelect entitySelect = (EntitySelect) dataItem;
//                    var temp = _sqlController.EntityGroupRead(entitySelect.Source.ToString());
//                    if (temp == null)
//                        errorLst.Add("Element entitySelect.Source:'" + entitySelect.Source +
//                                     "' is an reference to a local unknown EntitySearch group. Please update reference");
//                }
//
//                #endregion
//
//                #region PDF
//
//                if (dataItem.GetType() == typeof(ShowPdf))
//                {
//                    ShowPdf showPdf = (ShowPdf) dataItem;
//                    errorLst.AddRange(await PdfValidate(showPdf.Value, showPdf.Id));
//                }
//                
//                List<string> acceptedColors = new List<string>();
//                acceptedColors.Add(Constants.FieldColors.Grey);
//                acceptedColors.Add(Constants.FieldColors.Red);
//                acceptedColors.Add(Constants.FieldColors.Green);
//                acceptedColors.Add(Constants.FieldColors.Blue);
//                acceptedColors.Add(Constants.FieldColors.Purple);
//                acceptedColors.Add(Constants.FieldColors.Yellow);
//                acceptedColors.Add(Constants.FieldColors.Default);
//                acceptedColors.Add(Constants.FieldColors.None);
//                if (!acceptedColors.Contains(dataItem.Color) && !string.IsNullOrEmpty(dataItem.Color))
//                {
//                    errorLst.Add($"DataItem with label {dataItem.Label} did supply color {dataItem.Color}, but the only allowed values are: e8eaf6 for grey, ffe4e4 for red, f0f8db for green, e2f4fb for blue, e2f4fb for purple, fff6df for yellow, None for default or leave it blank.");
//                }
//
//                #endregion
//            }
//
//            return errorLst;
//
//        }
//    
//        private async Task<List<string>> CheckListValidation(MainElement mainElement)
//        {
//            string methodName = "Core.CheckListValidation";
//            await log.LogStandard(methodName, "called");
//            List<string> errorLst = new List<string>();
//            
//            List<string> acceptedColors = new List<string>();
//            acceptedColors.Add(Constants.CheckListColors.Grey);
//            acceptedColors.Add(Constants.CheckListColors.Red);
//            acceptedColors.Add(Constants.CheckListColors.Green);
//
//            if (!acceptedColors.Contains(mainElement.Color) && !string.IsNullOrEmpty(mainElement.Color))
//            {
//                errorLst.Add($"mainElement with label {mainElement.Label} did supply color {mainElement.Color}, but the only allowed colors are: grey, red, green or leave it blank.");
//            }
//            
//            return errorLst;
//        }
//    }
//}