using eFormCore;
using eFormData;
using eFormShared;
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


        [DllExport("Core_TemplatFromXml_DataElement_DataItemCount")]
        public static int Core_TemplatFromXml_DataElement_DataItemCount(int n, ref int count)
        {
            int result = 0;
            try
            {
                count = (mainElement.ElementList[n] as DataElement).DataItemList.Count;
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_DataElement_GetDataItemType")]
        public static int Core_TemplatFromXml_DataElement_GetDataItemType(int n, int m, [MarshalAs(UnmanagedType.BStr)] ref string elementType)
        {
            int result = 0;
            try
            {
                DataElement dataElement = (mainElement.ElementList[n] as DataElement);
                if (dataElement.DataItemList[m] is Picture)
                    elementType = "Picture";
                if (dataElement.DataItemList[m] is ShowPdf)
                    elementType = "ShowPdf";
                if (dataElement.DataItemList[m] is Date)
                    elementType = "Date";
                else
                    elementType = "DataItem";

            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

        [DllExport("Core_TemplatFromXml_DataElement_GetPicture")]
        public static int Core_TemplatFromXml_DataElement_GetPicture(int n, int m, ref int id, 
           [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
           ref int displayOrder, ref bool mandatory, [MarshalAs(UnmanagedType.BStr)] ref string color)    
        {
            int result = 0;
            try
            {
                DataElement e = mainElement.ElementList[n] as DataElement;
                Picture picture = e.DataItemList[m] as Picture;
                id = picture.Id;
                label = picture.Label;
                description = picture.Description.CDataWrapper[0].OuterXml;
                displayOrder = picture.DisplayOrder;
                mandatory = picture.Mandatory;
                color = picture.Color;               
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

       [DllExport("Core_TemplatFromXml_DataElement_GetShowPdf")]
       public static int Core_TemplatFromXml_DataElement_GetShowPdf(int n, int m, ref int id,
            [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
            ref int displayOrder, [MarshalAs(UnmanagedType.BStr)] ref string color, [MarshalAs(UnmanagedType.BStr)] ref string value)
       {
            int result = 0;
            try
            {
                DataElement e = mainElement.ElementList[n] as DataElement;
                ShowPdf pdf = e.DataItemList[m] as ShowPdf;
                id = pdf.Id;
                label = pdf.Label;
                description = pdf.Description.CDataWrapper[0].OuterXml;
                displayOrder = pdf.DisplayOrder;              
                color = pdf.Color;
                value = pdf.Value;
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }
            return result;
        }

       [DllExport("Core_TemplatFromXml_DataElement_GetDate")]
       public static int Core_TemplatFromXml_DataElement_GetDate(int n, int m, ref int id,
             [MarshalAs(UnmanagedType.BStr)] ref string label, [MarshalAs(UnmanagedType.BStr)] ref string description,
             ref int displayOrder, [MarshalAs(UnmanagedType.BStr)] ref string minValue, 
             [MarshalAs(UnmanagedType.BStr)] ref string maxValue, ref bool mandatory, ref bool _readonly,
             [MarshalAs(UnmanagedType.BStr)] ref string color, [MarshalAs(UnmanagedType.BStr)] ref string value)
        {
            int result = 0;
            try
            {
                DataElement e = mainElement.ElementList[n] as DataElement;
                Date date = e.DataItemList[m] as Date;
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
