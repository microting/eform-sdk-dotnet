using eFormCore;
using eFormData;
using eFormShared;
using Newtonsoft.Json;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace eFormSDK.Wrapper
{
    public static class CoreW
    {
        private static Core core;
        private static MainElement mainElement;
        private static Int32 startCallbackPointer;

        public delegate void NativeCallback(Int32 param);

        [DllExport("Core_Create")]
        public static int Core_Create()
        {
            int result = 0;
            try
            {
                core = new Core();
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }

            return result;
        }


        [DllExport("Core_Start")]
        public static int Core_Start([MarshalAs(UnmanagedType.BStr)]String serverConnectionString)
        {
            int result = 0;
            try
            {
                core.Start(serverConnectionString);
                //IntPtr ptr = (IntPtr)startCallbackPointer;
                //NativeCallback callbackMethod =  (NativeCallback)Marshal.GetDelegateForFunctionPointer(ptr, typeof(NativeCallback));
                //callbackMethod(100);
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }

            return result;
        }

        [DllExport("Core_SubscribeStartEvent")]
        unsafe public static int Core_SubscribeStartEvent(Int32 callbackPointer)
        {
            int result = 0;
            try
            {
                startCallbackPointer = callbackPointer;
                //core.HandleCaseCompleted += Core_HandleCaseCompleted; 
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        private static void Core_HandleCaseCompleted(object sender, EventArgs e)
        {
            Case_Dto trigger = (Case_Dto)sender;
            int siteId = trigger.SiteUId;
            string caseType = trigger.CaseType;
            string caseUid = trigger.CaseUId;
            string mUId = trigger.MicrotingUId;
            string checkUId = trigger.CheckUId;
            string CaseId = trigger.CaseId.ToString();

            // Then use callback to pass these values back to Delphi and construct correspond 
            // Case_Dto object where, using statement like this

            //IntPtr ptr = (IntPtr)caseCompletedCallbackPointer;
            //CaseCompletedCallback caseCompletedCallbackMethod = (CaseCompletedCallback)Marshal.GetDelegateForFunctionPointer(ptr, typeof(NativeCallback));
            //caseCompletedCallbackMethod(siteId, caseType, caseUid, mUId, checkUId, CaseId);

        }

        #region TemplatFromXml
        [DllExport("Core_TemplatFromXml")]
        public static int Core_TemplatFromXml([MarshalAs(UnmanagedType.BStr)]String xml, ref int id,
                [MarshalAs(UnmanagedType.BStr)] ref string label, ref int displayOrder,
                [MarshalAs(UnmanagedType.BStr)] ref string checkListFolderName, ref int repeated,
                [MarshalAs(UnmanagedType.BStr)] ref string startDate, [MarshalAs(UnmanagedType.BStr)] ref string endDate,
                [MarshalAs(UnmanagedType.BStr)] ref string language, ref bool multiApproval, ref bool fastNavigation,
                ref bool downloadEntities, ref bool manualSync, [MarshalAs(UnmanagedType.BStr)] ref string caseType)
        {
            int result = 0;
            try
            {
                mainElement = core.TemplateFromXml(xml);

              //  String s = JsonConvert.SerializeObject(mainElement);

                id = mainElement.Id;
                label = mainElement.Label;
                displayOrder = mainElement.DisplayOrder;
                checkListFolderName = mainElement.CheckListFolderName;
                repeated = mainElement.Repeated;
                startDate = mainElement.StartDate.ToString("yyyy-MM-dd");
                endDate = mainElement.EndDate.ToString("yyyy-MM-dd");
                language = mainElement.Language;
                multiApproval = mainElement.MultiApproval;
                fastNavigation = mainElement.FastNavigation;
                downloadEntities = mainElement.DownloadEntities;
                manualSync = mainElement.ManualSync;
                caseType = mainElement.CaseType;
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_ElementListCount")]
        public static int Core_TemplatFromXml_ElementListCount(ref int count)
        {
            int result = 0;
            try
            {
                count = mainElement.ElementList.Count;
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetElementType")]
        public static int Core_TemplatFromXml_GetElementType(int n, [MarshalAs(UnmanagedType.BStr)] ref string elementType)
        {
            int result = 0;
            try
            {
                if (mainElement.ElementList[n] is DataElement)
                    elementType = "DataElement";
                else if (mainElement.ElementList[n] is GroupElement)
                    elementType = "GroupElement";
                else if (mainElement.ElementList[n] is CheckListValue)
                    elementType = "CheckListValue";
                else
                    elementType = "Element";

            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetDataElement")]
        public static int Core_TemplatFromXml_GetDataElement(int n, ref int id, [MarshalAs(UnmanagedType.BStr)] ref string label,
            [MarshalAs(UnmanagedType.BStr)] ref string description, ref int displayOrder, ref bool reviewEnabled, 
            ref bool extraFieldsEnabled, ref bool approvalEnabled)
        {
            int result = 0;
            try
            {
                DataElement e = mainElement.ElementList[n] as DataElement;
                id = e.Id;
                label = e.Label;
                description = e.Description.CDataWrapper[0].OuterXml;
                displayOrder = e.DisplayOrder;
                reviewEnabled = e.ReviewEnabled;
                extraFieldsEnabled = e.ExtraFieldsEnabled;
                approvalEnabled = e.ApprovalEnabled;   
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }


        [DllExport("Core_TemplatFromXml_DataItemCount")]
        public static int Core_TemplatFromXml_DataItemCount([MarshalAs(UnmanagedType.BStr)] string path, ref int count)
        {
            int result = 0;
            try
            {
                count = 0;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    DataElement dataElement = mainElement.ElementList[n] as DataElement;
                    count = dataElement.DataItemList.Count;
                    

                    if (parts.Length > 2)
                    {
                       // if (parts[2] == "FieldContainer")
                       // {
                            int m = int.Parse(parts[2]);
                            FieldContainer fieldContainer = dataElement.DataItemList[m] as FieldContainer;
                            count = fieldContainer.DataItemList.Count;
                      //  }
                    }
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        private static string GetDataItemType(DataItem dataItem)
        {
            if (dataItem is Picture)
                return "Picture";
            else if (dataItem is ShowPdf)
                return "ShowPdf";
            else if (dataItem is Date)
                return "Date";
            else if (dataItem is Signature)
                return "Signature";
            else if (dataItem is CheckBox)
                return "CheckBox";
            else if (dataItem is SaveButton)
                return "SaveButton";
            else if (dataItem is Timer)
                return "Timer";
            else if (dataItem is Text)
                return "Text";
            else if (dataItem is None)
                return "None";
            else if (dataItem is Comment)
                return "Comment";
            else if (dataItem is SingleSelect)
                return "SingleSelect";
            else if (dataItem is MultiSelect)
                return "MultiSelect";
            else if (dataItem is Number)
                return "Number";
            else if (dataItem is FieldContainer)
                return "FieldContainer";
            else
                return "DataItem";
        }

        [DllExport("Core_TemplatFromXml_GetDataItemType")]
        public static int Core_TemplatFromXml_GetDataItemType([MarshalAs(UnmanagedType.BStr)] string path, [MarshalAs(UnmanagedType.BStr)] ref string elementType)
        {
            int result = 0;
            try
            {
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement dataElement = mainElement.ElementList[n] as DataElement;
                    if (parts.Length == 3)
                    {
                        elementType = GetDataItemType(dataElement.DataItemList[m]);
                    }
                    else if (parts.Length == 4)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = dataElement.DataItemList[m] as FieldContainer;
                        elementType = GetDataItemType(fieldContainer.DataItemList[k]);
                    }
                }              

            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetPicture")]
        public static int Core_TemplatFromXml_GetPicture([MarshalAs(UnmanagedType.BStr)] string path, ref int id, 
           [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
           ref int displayOrder, ref bool mandatory, [MarshalAs(UnmanagedType.BStr)] ref string color)    
        {
            int result = 0;
            try
            {
                Picture picture = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;                  
                    picture = e.DataItemList[m] as Picture;
                    
                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        picture = fieldContainer.DataItemList[k] as Picture;
                    }
                }

                if (picture != null)
                {
                    id = picture.Id;
                    label = picture.Label;
                    description = picture.Description.CDataWrapper[0].OuterXml;
                    displayOrder = picture.DisplayOrder;
                    mandatory = picture.Mandatory;
                    color = picture.Color;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

       [DllExport("Core_TemplatFromXml_GetShowPdf")]
       public static int Core_TemplatFromXml_GetShowPdf([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
            [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
            ref int displayOrder, [MarshalAs(UnmanagedType.BStr)] ref string color, [MarshalAs(UnmanagedType.BStr)] ref string value)
       {
            int result = 0;
            try
            {
                ShowPdf showPdf = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    showPdf = e.DataItemList[m] as ShowPdf;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        showPdf = fieldContainer.DataItemList[k] as ShowPdf;
                    }
                }

                if (showPdf != null)
                {
                    id = showPdf.Id;
                    label = showPdf.Label;
                    description = showPdf.Description.CDataWrapper[0].OuterXml;
                    displayOrder = showPdf.DisplayOrder;
                    color = showPdf.Color;
                    value = showPdf.Value;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

       [DllExport("Core_TemplatFromXml_GetDate")]
       public static int Core_TemplatFromXml_GetDate([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
             [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
             ref int displayOrder, [MarshalAs(UnmanagedType.BStr)] ref string minValue, 
             [MarshalAs(UnmanagedType.BStr)] ref string maxValue, ref bool mandatory, ref bool _readonly,
             [MarshalAs(UnmanagedType.BStr)] ref string color, [MarshalAs(UnmanagedType.BStr)] ref string value)
        {
            int result = 0;
            try
            {
                Date date = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    date = e.DataItemList[m] as Date;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        date = fieldContainer.DataItemList[k] as Date;
                    }
                }

                if (date != null)
                {
                    id = date.Id;
                    label = date.Label;
                    description = date.Description.CDataWrapper[0].OuterXml;
                    displayOrder = date.DisplayOrder;
                    minValue = date.MinValue.ToString("yyyy-MM-dd");
                    maxValue = date.MaxValue.ToString("yyyy-MM-dd");
                    color = date.Color;
                    mandatory = date.Mandatory;
                    _readonly = date.ReadOnly;
                    value = date.DefaultValue;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetSignature")]
        public static int Core_TemplatFromXml_GetSignature([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
          [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
          ref int displayOrder, ref bool mandatory, [MarshalAs(UnmanagedType.BStr)] ref string color)
        {
            int result = 0;
            try
            {
                Signature signature = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    signature = e.DataItemList[m] as Signature;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        signature = fieldContainer.DataItemList[k] as Signature;
                    }
                }

                if (signature != null)
                {
                    id = signature.Id;
                    label = signature.Label;
                    description = signature.Description.CDataWrapper[0].OuterXml;
                    displayOrder = signature.DisplayOrder;
                    mandatory = signature.Mandatory;
                    color = signature.Color;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetSaveButton")]
        public static int Core_TemplatFromXml_GetSaveButton([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
           [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
           ref int displayOrder, [MarshalAs(UnmanagedType.BStr)] ref string value)
        {
            int result = 0;
            try
            {
                SaveButton saveButton = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    saveButton = e.DataItemList[m] as SaveButton;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        saveButton = fieldContainer.DataItemList[k] as SaveButton;
                    }
                }

                if (saveButton != null)
                {
                    id = saveButton.Id;
                    label = saveButton.Label;
                    description = saveButton.Description.CDataWrapper[0].OuterXml;
                    displayOrder = saveButton.DisplayOrder;
                    value = saveButton.Value;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetTimer")]
        public static int Core_TemplatFromXml_GetTimer([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
           [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
           ref int displayOrder, ref bool stopOnSave, ref bool mandatory)
        {
            int result = 0;
            try
            {
                Timer timer = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    timer = e.DataItemList[m] as Timer;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        timer = fieldContainer.DataItemList[k] as Timer;
                    }
                }

                if (timer != null)
                {
                    id = timer.Id;
                    label = timer.Label;
                    description = timer.Description.CDataWrapper[0].OuterXml;
                    displayOrder = timer.DisplayOrder;
                    stopOnSave = timer.StopOnSave;
                    mandatory = timer.Mandatory;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetNone")]
        public static int Core_TemplatFromXml_GetNone([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
          [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
          ref int displayOrder)
        {
            int result = 0;
            try
            {
                None none = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    none = e.DataItemList[m] as None;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        none = fieldContainer.DataItemList[k] as None;
                    }
                }

                if (none != null)
                {
                    id = none.Id;
                    label = none.Label;
                    description = none.Description.CDataWrapper[0].OuterXml;
                    displayOrder = none.DisplayOrder;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetCheckBox")]
        public static int Core_TemplatFromXml_GetCheckBox([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
             [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
             ref int displayOrder, ref bool mandatory, ref bool selected)
        {
            int result = 0;
            try
            {
                CheckBox checkBox = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    checkBox = e.DataItemList[m] as CheckBox;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        checkBox = fieldContainer.DataItemList[k] as CheckBox;
                    }
                }

                if (checkBox != null)
                {
                    id = checkBox.Id;
                    label = checkBox.Label;
                    description = checkBox.Description.CDataWrapper[0].OuterXml;
                    displayOrder = checkBox.DisplayOrder;
                    mandatory = checkBox.Mandatory;
                    selected = checkBox.Selected;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetMultiSelect")]
        public static int Core_TemplatFromXml_GetMultiSelect([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
            [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
            ref int displayOrder, ref bool mandatory)
        {
            int result = 0;
            try
            {
                MultiSelect multiSelect = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    multiSelect = e.DataItemList[m] as MultiSelect;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        multiSelect = fieldContainer.DataItemList[k] as MultiSelect;
                    }
                }


                if (multiSelect != null)
                {
                    id = multiSelect.Id;
                    label = multiSelect.Label;
                    description = multiSelect.Description.CDataWrapper[0].OuterXml;
                    displayOrder = multiSelect.DisplayOrder;
                    mandatory = multiSelect.Mandatory;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetSingleSelect")]
        public static int Core_TemplatFromXml_GetSingleSelect([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
           [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
           ref int displayOrder, ref bool mandatory)
        {
            int result = 0;
            try
            {
                SingleSelect singleSelect = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    singleSelect = e.DataItemList[m] as SingleSelect;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        singleSelect = fieldContainer.DataItemList[k] as SingleSelect;
                    }
                }

                if (singleSelect != null)
                {
                    id = singleSelect.Id;
                    label = singleSelect.Label;
                    description = singleSelect.Description.CDataWrapper[0].OuterXml;
                    displayOrder = singleSelect.DisplayOrder;
                    mandatory = singleSelect.Mandatory;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }


        [DllExport("Core_TemplatFromXml_GetNumber")]
        public static int Core_TemplatFromXml_GetNumber([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
          [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
          ref int displayOrder, [MarshalAs(UnmanagedType.BStr)] ref string minValue,
          [MarshalAs(UnmanagedType.BStr)] ref string maxValue, ref bool mandatory, ref int decimalCount,
          [MarshalAs(UnmanagedType.BStr)] ref string unitName)
        {
            int result = 0;
            try
            {
                Number number = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    number= e.DataItemList[m] as Number;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        number = fieldContainer.DataItemList[k] as Number;
                    }
                }


                if (number != null)
                {
                    id = number.Id;
                    label = number.Label;
                    description = number.Description.CDataWrapper[0].OuterXml;
                    displayOrder = number.DisplayOrder;
                    minValue = number.MinValue;
                    maxValue = number.MaxValue;
                    mandatory = number.Mandatory;
                    decimalCount = number.DecimalCount;
                    unitName = number.UnitName;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }


        [DllExport("Core_TemplatFromXml_GetText")]
        public static int Core_TemplatFromXml_GetText([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
          [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
          ref bool geolocationEnabled, [MarshalAs(UnmanagedType.BStr)] ref string value,  ref bool readOnly,
          ref bool mandatory)
        {
            int result = 0;
            try
            {
                Text text = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    text = e.DataItemList[m] as Text;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        text = fieldContainer.DataItemList[k] as Text;
                    }
                }

                if (text != null)
                {
                    id = text.Id;
                    label = text.Label;
                    description = text.Description.CDataWrapper[0].OuterXml;
                    geolocationEnabled = text.GeolocationEnabled;
                    value = text.Value;
                    readOnly = text.ReadOnly;
                    mandatory = text.Mandatory;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }


        [DllExport("Core_TemplatFromXml_GetComment")]
        public static int Core_TemplatFromXml_GetComment([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
          [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
          ref bool splitScreen, [MarshalAs(UnmanagedType.BStr)] ref string value, ref bool readOnly,
          ref bool mandatory)
        {
            int result = 0;
            try
            {
                Comment comment = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    comment = e.DataItemList[m] as Comment;

                    if (parts.Length > 3)
                    {
                        int k = int.Parse(parts[3]);
                        FieldContainer fieldContainer = e.DataItemList[m] as FieldContainer;
                        comment = fieldContainer.DataItemList[k] as Comment;
                    }
                }

                if (comment != null)
                {
                    id = comment.Id;
                    label = comment.Label;
                    description = comment.Description.CDataWrapper[0].OuterXml;
                    splitScreen = comment.SplitScreen;
                    value = comment.Value;
                    readOnly = comment.ReadOnly;
                    mandatory = comment.Mandatory;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_GetFieldContainer")]
        public static int Core_TemplatFromXml_GetFieldContainer([MarshalAs(UnmanagedType.BStr)] string path, ref int id,
            [MarshalAs(UnmanagedType.BStr)]  ref string _label, [MarshalAs(UnmanagedType.BStr)]  ref string description,
            [MarshalAs(UnmanagedType.BStr)] ref string value, [MarshalAs(UnmanagedType.BStr)] ref string fieldType)
        {
            int result = 0;
            try
            {
                FieldContainer fieldContainer = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement e = mainElement.ElementList[n] as DataElement;
                    fieldContainer = e.DataItemList[m] as FieldContainer;
                }

                if (fieldContainer != null)
                {
                    id = fieldContainer.Id;
                    _label = fieldContainer.Label;
                    description = fieldContainer.Description.InderValue;
                    value = fieldContainer.Value;
                    fieldType = fieldContainer.FieldType == null ? "" : fieldContainer.FieldType;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_KeyValueListCount")]
        public static int Core_TemplatFromXml_KeyValueListCount([MarshalAs(UnmanagedType.BStr)]string path, ref int count)
        {
            int result = 0;
            try
            {
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    DataElement dataElement = mainElement.ElementList[n] as DataElement;
                    if (parts[3] == "MultiSelect")
                    {
                        MultiSelect multiSelect = dataElement.DataItemList[m] as MultiSelect;
                        count = multiSelect.KeyValuePairList.Count;
                    }
                    else if (parts[3] == "SingleSelect")
                    {
                        SingleSelect singleSelect = dataElement.DataItemList[m] as SingleSelect;
                        count = singleSelect.KeyValuePairList.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }


        [DllExport("Core_TemplatFromXml_GetKeyValuePair")]
        public static int Core_TemplatFromXml_GetKeyValuePair([MarshalAs(UnmanagedType.BStr)]string path,
            [MarshalAs(UnmanagedType.BStr)] ref string key, [MarshalAs(UnmanagedType.BStr)] ref string value, ref bool selected,
            [MarshalAs(UnmanagedType.BStr)] ref string displayOrder)
        {
            int result = 0;
            try
            {
                KeyValuePair keyValuePair = null;
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    int m = int.Parse(parts[2]);
                    int k = int.Parse(parts[4]);
                    DataElement dataElement = mainElement.ElementList[n] as DataElement;
                    if (parts[3] == "MultiSelect")
                    {
                        MultiSelect multiSelect = dataElement.DataItemList[m] as MultiSelect;
                        keyValuePair = multiSelect.KeyValuePairList[k];
                    }
                    else if (parts[3] == "SingleSelect")
                    {
                        SingleSelect singleSelect = dataElement.DataItemList[m] as SingleSelect;
                        keyValuePair = singleSelect.KeyValuePairList[k];
                    }
                }

                if (keyValuePair != null)
                {
                    key = keyValuePair.Key;
                    value = keyValuePair.Value;
                    selected = keyValuePair.Selected;
                    displayOrder = keyValuePair.DisplayOrder;
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_DataItemGroupCount")]
        public static int Core_TemplatFromXml_DataItemGroupCount([MarshalAs(UnmanagedType.BStr)]string path,
              ref int count)
        {
            int result = 0;
            try
            {
                string[] parts = path.Split('_');
                if (parts[0] == "DataElement")
                {
                    int n = int.Parse(parts[1]);
                    DataElement dataElement = mainElement.ElementList[n] as DataElement;
                    count = dataElement.DataItemGroupList.Count;

                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }
        #endregion

        #region TemplateCreate
        [DllExport("Core_TemplateCreate")]
        public static int Core_TemplateCreate([MarshalAs(UnmanagedType.BStr)]String json, ref int templateId)
        {
            int result = 0;
            try
            {
                Packer packer = new Packer();
                MainElement mainElement = packer.UnpackMainElement(json);
                List<String> errors = core.TemplateValidation(mainElement);
                if (errors.Count > 0)
                {
                    string totalError = "";
                    foreach(string error in errors)
                        totalError += error + "\n";
                    throw new Exception(totalError);
                }
                templateId = core.TemplateCreate(mainElement);
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }
        #endregion

    }

}
