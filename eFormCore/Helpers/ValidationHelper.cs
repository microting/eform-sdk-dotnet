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