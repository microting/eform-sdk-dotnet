using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormDll
{
    public class Xml
    {
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Main
        {

            private string idField;

            private string labelField;

            private string descriptionField;

            private byte repeatedField;

            private System.DateTime startDateField;

            private System.DateTime endDateField;

            private string languageField;

            private bool multiApprovalField;

            private bool fastNavigationField;

            private bool manualSyncField;

            private string checkListFolderNameField;

            private bool downloadEnitiesField;

            private MainElement[] elementListField;

            /// <remarks/>
            public string Id
            {
                get
                {
                    return this.IdField;
                }
                set
                {
                    this.IdField = value;
                }
            }

            /// <remarks/>
            public string Label
            {
                get
                {
                    return this.LabelField;
                }
                set
                {
                    this.LabelField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.DescriptionField;
                }
                set
                {
                    this.DescriptionField = value;
                }
            }

            /// <remarks/>
            public byte Repeated
            {
                get
                {
                    return this.RepeatedField;
                }
                set
                {
                    this.RepeatedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime StartDate
            {
                get
                {
                    return this.StartDateField;
                }
                set
                {
                    this.StartDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    this.EndDateField = value;
                }
            }

            /// <remarks/>
            public string Language
            {
                get
                {
                    return this.LanguageField;
                }
                set
                {
                    this.LanguageField = value;
                }
            }

            /// <remarks/>
            public bool MultiApproval
            {
                get
                {
                    return this.MultiApprovalField;
                }
                set
                {
                    this.MultiApprovalField = value;
                }
            }

            /// <remarks/>
            public bool FastNavigation
            {
                get
                {
                    return this.FastNavigationField;
                }
                set
                {
                    this.FastNavigationField = value;
                }
            }

            /// <remarks/>
            public bool ManualSync
            {
                get
                {
                    return this.ManualSyncField;
                }
                set
                {
                    this.ManualSyncField = value;
                }
            }

            /// <remarks/>
            public string CheckListFolderName
            {
                get
                {
                    return this.CheckListFolderNameField;
                }
                set
                {
                    this.CheckListFolderNameField = value;
                }
            }

            /// <remarks/>
            public bool DownloadEnities
            {
                get
                {
                    return this.DownloadEnitiesField;
                }
                set
                {
                    this.DownloadEnitiesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Element", IsNullable = false)]
            public MainElement[] ElementList
            {
                get
                {
                    return this.ElementListField;
                }
                set
                {
                    this.ElementListField = value;
                }
            }

            public string IdField
            {
                get
                {
                    return IdField1;
                }

                set
                {
                    IdField1 = value;
                }
            }

            public string LabelField
            {
                get
                {
                    return LabelField1;
                }

                set
                {
                    LabelField1 = value;
                }
            }

            public string DescriptionField
            {
                get
                {
                    return DescriptionField1;
                }

                set
                {
                    DescriptionField1 = value;
                }
            }

            public byte RepeatedField
            {
                get
                {
                    return RepeatedField1;
                }

                set
                {
                    RepeatedField1 = value;
                }
            }

            public DateTime StartDateField
            {
                get
                {
                    return StartDateField1;
                }

                set
                {
                    StartDateField1 = value;
                }
            }

            public DateTime EndDateField
            {
                get
                {
                    return EndDateField1;
                }

                set
                {
                    EndDateField1 = value;
                }
            }

            public string LanguageField
            {
                get
                {
                    return LanguageField1;
                }

                set
                {
                    LanguageField1 = value;
                }
            }

            public bool MultiApprovalField
            {
                get
                {
                    return MultiApprovalField1;
                }

                set
                {
                    MultiApprovalField1 = value;
                }
            }

            public bool FastNavigationField
            {
                get
                {
                    return FastNavigationField1;
                }

                set
                {
                    FastNavigationField1 = value;
                }
            }

            public bool ManualSyncField
            {
                get
                {
                    return ManualSyncField1;
                }

                set
                {
                    ManualSyncField1 = value;
                }
            }

            public string CheckListFolderNameField
            {
                get
                {
                    return CheckListFolderNameField1;
                }

                set
                {
                    CheckListFolderNameField1 = value;
                }
            }

            public bool DownloadEnitiesField
            {
                get
                {
                    return DownloadEnitiesField1;
                }

                set
                {
                    DownloadEnitiesField1 = value;
                }
            }

            public MainElement[] ElementListField
            {
                get
                {
                    return ElementListField1;
                }

                set
                {
                    ElementListField1 = value;
                }
            }

            public string IdField1
            {
                get
                {
                    return idField;
                }

                set
                {
                    idField = value;
                }
            }

            public string LabelField1
            {
                get
                {
                    return labelField;
                }

                set
                {
                    labelField = value;
                }
            }

            public string DescriptionField1
            {
                get
                {
                    return descriptionField;
                }

                set
                {
                    descriptionField = value;
                }
            }

            public byte RepeatedField1
            {
                get
                {
                    return repeatedField;
                }

                set
                {
                    repeatedField = value;
                }
            }

            public DateTime StartDateField1
            {
                get
                {
                    return startDateField;
                }

                set
                {
                    startDateField = value;
                }
            }

            public DateTime EndDateField1
            {
                get
                {
                    return endDateField;
                }

                set
                {
                    endDateField = value;
                }
            }

            public string LanguageField1
            {
                get
                {
                    return languageField;
                }

                set
                {
                    languageField = value;
                }
            }

            public bool MultiApprovalField1
            {
                get
                {
                    return multiApprovalField;
                }

                set
                {
                    multiApprovalField = value;
                }
            }

            public bool FastNavigationField1
            {
                get
                {
                    return fastNavigationField;
                }

                set
                {
                    fastNavigationField = value;
                }
            }

            public bool ManualSyncField1
            {
                get
                {
                    return manualSyncField;
                }

                set
                {
                    manualSyncField = value;
                }
            }

            public string CheckListFolderNameField1
            {
                get
                {
                    return checkListFolderNameField;
                }

                set
                {
                    checkListFolderNameField = value;
                }
            }

            public bool DownloadEnitiesField1
            {
                get
                {
                    return downloadEnitiesField;
                }

                set
                {
                    downloadEnitiesField = value;
                }
            }

            public MainElement[] ElementListField1
            {
                get
                {
                    return elementListField;
                }

                set
                {
                    elementListField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class MainElement
        {

            private string idField;

            private string labelField;

            private string descriptionField;

            private bool reviewEnabledField;

            private bool approvedEnabledField;

            private bool doneButtonEnabledField;

            private bool extraFieldsEnabledField;

            private MainElementDataItem[] dataItemListField;

            private string typeField;

            /// <remarks/>
            public string Id
            {
                get
                {
                    return this.IdField;
                }
                set
                {
                    this.IdField = value;
                }
            }

            /// <remarks/>
            public string Label
            {
                get
                {
                    return this.LabelField;
                }
                set
                {
                    this.LabelField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.DescriptionField;
                }
                set
                {
                    this.DescriptionField = value;
                }
            }

            /// <remarks/>
            public bool ReviewEnabled
            {
                get
                {
                    return this.ReviewEnabledField;
                }
                set
                {
                    this.ReviewEnabledField = value;
                }
            }

            /// <remarks/>
            public bool ApprovedEnabled
            {
                get
                {
                    return this.ApprovedEnabledField;
                }
                set
                {
                    this.ApprovedEnabledField = value;
                }
            }

            /// <remarks/>
            public bool DoneButtonEnabled
            {
                get
                {
                    return this.DoneButtonEnabledField;
                }
                set
                {
                    this.DoneButtonEnabledField = value;
                }
            }

            /// <remarks/>
            public bool ExtraFieldsEnabled
            {
                get
                {
                    return this.ExtraFieldsEnabledField;
                }
                set
                {
                    this.ExtraFieldsEnabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("DataItem", IsNullable = false)]
            public MainElementDataItem[] DataItemList
            {
                get
                {
                    return this.DataItemListField;
                }
                set
                {
                    this.DataItemListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string type
            {
                get
                {
                    return this.TypeField;
                }
                set
                {
                    this.TypeField = value;
                }
            }

            public string IdField
            {
                get
                {
                    return IdField1;
                }

                set
                {
                    IdField1 = value;
                }
            }

            public string LabelField
            {
                get
                {
                    return LabelField1;
                }

                set
                {
                    LabelField1 = value;
                }
            }

            public string DescriptionField
            {
                get
                {
                    return DescriptionField1;
                }

                set
                {
                    DescriptionField1 = value;
                }
            }

            public bool ReviewEnabledField
            {
                get
                {
                    return ReviewEnabledField1;
                }

                set
                {
                    ReviewEnabledField1 = value;
                }
            }

            public bool ApprovedEnabledField
            {
                get
                {
                    return ApprovedEnabledField1;
                }

                set
                {
                    ApprovedEnabledField1 = value;
                }
            }

            public bool DoneButtonEnabledField
            {
                get
                {
                    return DoneButtonEnabledField1;
                }

                set
                {
                    DoneButtonEnabledField1 = value;
                }
            }

            public bool ExtraFieldsEnabledField
            {
                get
                {
                    return ExtraFieldsEnabledField1;
                }

                set
                {
                    ExtraFieldsEnabledField1 = value;
                }
            }

            public MainElementDataItem[] DataItemListField
            {
                get
                {
                    return DataItemListField1;
                }

                set
                {
                    DataItemListField1 = value;
                }
            }

            public string TypeField
            {
                get
                {
                    return TypeField1;
                }

                set
                {
                    TypeField1 = value;
                }
            }

            public string IdField1
            {
                get
                {
                    return idField;
                }

                set
                {
                    idField = value;
                }
            }

            public string LabelField1
            {
                get
                {
                    return labelField;
                }

                set
                {
                    labelField = value;
                }
            }

            public string DescriptionField1
            {
                get
                {
                    return descriptionField;
                }

                set
                {
                    descriptionField = value;
                }
            }

            public bool ReviewEnabledField1
            {
                get
                {
                    return reviewEnabledField;
                }

                set
                {
                    reviewEnabledField = value;
                }
            }

            public bool ApprovedEnabledField1
            {
                get
                {
                    return approvedEnabledField;
                }

                set
                {
                    approvedEnabledField = value;
                }
            }

            public bool DoneButtonEnabledField1
            {
                get
                {
                    return doneButtonEnabledField;
                }

                set
                {
                    doneButtonEnabledField = value;
                }
            }

            public bool ExtraFieldsEnabledField1
            {
                get
                {
                    return extraFieldsEnabledField;
                }

                set
                {
                    extraFieldsEnabledField = value;
                }
            }

            public MainElementDataItem[] DataItemListField1
            {
                get
                {
                    return dataItemListField;
                }

                set
                {
                    dataItemListField = value;
                }
            }

            public string TypeField1
            {
                get
                {
                    return typeField;
                }

                set
                {
                    typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class MainElementDataItem
        {
            private bool readOnlyField;
            private bool readOnlyFieldSpecified;
            private byte multiField;
            private bool multiFieldSpecified;
            private string valueField;
            private bool selectedField;
            private bool selectedFieldSpecified;
            private ushort maxLengthField;
            private bool maxLengthFieldSpecified;
            private bool splitScreenField;
            private bool splitScreenFieldSpecified;
            private bool geolocationEnabledField;
            private bool geolocationEnabledFieldSpecified;
            private bool geolocationForcedField;
            private bool geolocationForcedFieldSpecified;
            private bool geolocationHiddenField;
            private bool geolocationHiddenFieldSpecified;
            private string minValueField;
            private string maxValueField;
            private byte decimalCountField;
            private bool decimalCountFieldSpecified;
            private object unitNameField;
            private MainElementDataItemKeyValuePair[] keyValuePairListField;
            private byte idField;
            private string labelField;
            private string descriptionField;
            private bool mandatoryField;
            private string colorField;
            private string typeField;

            /// <remarks/>
            public bool ReadOnly
            {
                get
                {
                    return this.ReadOnlyField;
                }
                set
                {
                    this.ReadOnlyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool ReadOnlySpecified
            {
                get
                {
                    return this.ReadOnlyFieldSpecified;
                }
                set
                {
                    this.ReadOnlyFieldSpecified = value;
                }
            }

            /// <remarks/>
            public byte Multi
            {
                get
                {
                    return this.MultiField;
                }
                set
                {
                    this.MultiField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MultiSpecified
            {
                get
                {
                    return this.MultiFieldSpecified;
                }
                set
                {
                    this.MultiFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.ValueField;
                }
                set
                {
                    this.ValueField = value;
                }
            }

            /// <remarks/>
            public bool Selected
            {
                get
                {
                    return this.SelectedField;
                }
                set
                {
                    this.SelectedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SelectedSpecified
            {
                get
                {
                    return this.SelectedFieldSpecified;
                }
                set
                {
                    this.SelectedFieldSpecified = value;
                }
            }

            /// <remarks/>
            public ushort MaxLength
            {
                get
                {
                    return this.MaxLengthField;
                }
                set
                {
                    this.MaxLengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool MaxLengthSpecified
            {
                get
                {
                    return this.MaxLengthFieldSpecified;
                }
                set
                {
                    this.MaxLengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            public bool SplitScreen
            {
                get
                {
                    return this.SplitScreenField;
                }
                set
                {
                    this.SplitScreenField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool SplitScreenSpecified
            {
                get
                {
                    return this.SplitScreenFieldSpecified;
                }
                set
                {
                    this.SplitScreenFieldSpecified = value;
                }
            }

            /// <remarks/>
            public bool GeolocationEnabled
            {
                get
                {
                    return this.GeolocationEnabledField;
                }
                set
                {
                    this.GeolocationEnabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GeolocationEnabledSpecified
            {
                get
                {
                    return this.GeolocationEnabledFieldSpecified;
                }
                set
                {
                    this.GeolocationEnabledFieldSpecified = value;
                }
            }

            /// <remarks/>
            public bool GeolocationForced
            {
                get
                {
                    return this.GeolocationForcedField;
                }
                set
                {
                    this.GeolocationForcedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GeolocationForcedSpecified
            {
                get
                {
                    return this.GeolocationForcedFieldSpecified;
                }
                set
                {
                    this.GeolocationForcedFieldSpecified = value;
                }
            }

            /// <remarks/>
            public bool GeolocationHidden
            {
                get
                {
                    return this.GeolocationHiddenField;
                }
                set
                {
                    this.GeolocationHiddenField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool GeolocationHiddenSpecified
            {
                get
                {
                    return this.GeolocationHiddenFieldSpecified;
                }
                set
                {
                    this.GeolocationHiddenFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string MinValue
            {
                get
                {
                    return this.MinValueField;
                }
                set
                {
                    this.MinValueField = value;
                }
            }

            /// <remarks/>
            public string MaxValue
            {
                get
                {
                    return this.MaxValueField;
                }
                set
                {
                    this.MaxValueField = value;
                }
            }

            /// <remarks/>
            public byte DecimalCount
            {
                get
                {
                    return this.DecimalCountField;
                }
                set
                {
                    this.DecimalCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DecimalCountSpecified
            {
                get
                {
                    return this.DecimalCountFieldSpecified;
                }
                set
                {
                    this.DecimalCountFieldSpecified = value;
                }
            }

            /// <remarks/>
            public object UnitName
            {
                get
                {
                    return this.UnitNameField;
                }
                set
                {
                    this.UnitNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("KeyValuePair", IsNullable = false)]
            public MainElementDataItemKeyValuePair[] KeyValuePairList
            {
                get
                {
                    return this.KeyValuePairListField;
                }
                set
                {
                    this.KeyValuePairListField = value;
                }
            }

            /// <remarks/>
            public byte Id
            {
                get
                {
                    return this.IdField;
                }
                set
                {
                    this.IdField = value;
                }
            }

            /// <remarks/>
            public string Label
            {
                get
                {
                    return this.LabelField;
                }
                set
                {
                    this.LabelField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.DescriptionField;
                }
                set
                {
                    this.DescriptionField = value;
                }
            }

            /// <remarks/>
            public bool Mandatory
            {
                get
                {
                    return this.MandatoryField;
                }
                set
                {
                    this.MandatoryField = value;
                }
            }

            /// <remarks/>
            public string Color
            {
                get
                {
                    return this.ColorField;
                }
                set
                {
                    this.ColorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string type
            {
                get
                {
                    return this.TypeField;
                }
                set
                {
                    this.TypeField = value;
                }
            }

            public bool ReadOnlyField
            {
                get
                {
                    return ReadOnlyField1;
                }

                set
                {
                    ReadOnlyField1 = value;
                }
            }

            public bool ReadOnlyFieldSpecified
            {
                get
                {
                    return ReadOnlyFieldSpecified1;
                }

                set
                {
                    ReadOnlyFieldSpecified1 = value;
                }
            }

            public byte MultiField
            {
                get
                {
                    return MultiField1;
                }

                set
                {
                    MultiField1 = value;
                }
            }

            public bool MultiFieldSpecified
            {
                get
                {
                    return MultiFieldSpecified1;
                }

                set
                {
                    MultiFieldSpecified1 = value;
                }
            }

            public string ValueField
            {
                get
                {
                    return ValueField1;
                }

                set
                {
                    ValueField1 = value;
                }
            }

            public bool SelectedField
            {
                get
                {
                    return SelectedField1;
                }

                set
                {
                    SelectedField1 = value;
                }
            }

            public bool SelectedFieldSpecified
            {
                get
                {
                    return SelectedFieldSpecified1;
                }

                set
                {
                    SelectedFieldSpecified1 = value;
                }
            }

            public ushort MaxLengthField
            {
                get
                {
                    return MaxLengthField1;
                }

                set
                {
                    MaxLengthField1 = value;
                }
            }

            public bool MaxLengthFieldSpecified
            {
                get
                {
                    return MaxLengthFieldSpecified1;
                }

                set
                {
                    MaxLengthFieldSpecified1 = value;
                }
            }

            public bool SplitScreenField
            {
                get
                {
                    return SplitScreenField1;
                }

                set
                {
                    SplitScreenField1 = value;
                }
            }

            public bool SplitScreenFieldSpecified
            {
                get
                {
                    return SplitScreenFieldSpecified1;
                }

                set
                {
                    SplitScreenFieldSpecified1 = value;
                }
            }

            public bool GeolocationEnabledField
            {
                get
                {
                    return GeolocationEnabledField1;
                }

                set
                {
                    GeolocationEnabledField1 = value;
                }
            }

            public bool GeolocationEnabledFieldSpecified
            {
                get
                {
                    return GeolocationEnabledFieldSpecified1;
                }

                set
                {
                    GeolocationEnabledFieldSpecified1 = value;
                }
            }

            public bool GeolocationForcedField
            {
                get
                {
                    return GeolocationForcedField1;
                }

                set
                {
                    GeolocationForcedField1 = value;
                }
            }

            public bool GeolocationForcedFieldSpecified
            {
                get
                {
                    return GeolocationForcedFieldSpecified1;
                }

                set
                {
                    GeolocationForcedFieldSpecified1 = value;
                }
            }

            public bool GeolocationHiddenField
            {
                get
                {
                    return GeolocationHiddenField1;
                }

                set
                {
                    GeolocationHiddenField1 = value;
                }
            }

            public bool GeolocationHiddenFieldSpecified
            {
                get
                {
                    return GeolocationHiddenFieldSpecified1;
                }

                set
                {
                    GeolocationHiddenFieldSpecified1 = value;
                }
            }

            public string MinValueField
            {
                get
                {
                    return MinValueField1;
                }

                set
                {
                    MinValueField1 = value;
                }
            }

            public string MaxValueField
            {
                get
                {
                    return MaxValueField1;
                }

                set
                {
                    MaxValueField1 = value;
                }
            }

            public byte DecimalCountField
            {
                get
                {
                    return DecimalCountField1;
                }

                set
                {
                    DecimalCountField1 = value;
                }
            }

            public bool DecimalCountFieldSpecified
            {
                get
                {
                    return DecimalCountFieldSpecified1;
                }

                set
                {
                    DecimalCountFieldSpecified1 = value;
                }
            }

            public object UnitNameField
            {
                get
                {
                    return UnitNameField1;
                }

                set
                {
                    UnitNameField1 = value;
                }
            }

            public MainElementDataItemKeyValuePair[] KeyValuePairListField
            {
                get
                {
                    return KeyValuePairListField1;
                }

                set
                {
                    KeyValuePairListField1 = value;
                }
            }

            public byte IdField
            {
                get
                {
                    return IdField1;
                }

                set
                {
                    IdField1 = value;
                }
            }

            public string LabelField
            {
                get
                {
                    return LabelField1;
                }

                set
                {
                    LabelField1 = value;
                }
            }

            public string DescriptionField
            {
                get
                {
                    return DescriptionField1;
                }

                set
                {
                    DescriptionField1 = value;
                }
            }

            public bool MandatoryField
            {
                get
                {
                    return MandatoryField1;
                }

                set
                {
                    MandatoryField1 = value;
                }
            }

            public string ColorField
            {
                get
                {
                    return ColorField1;
                }

                set
                {
                    ColorField1 = value;
                }
            }

            public string TypeField
            {
                get
                {
                    return TypeField1;
                }

                set
                {
                    TypeField1 = value;
                }
            }

            public bool ReadOnlyField1
            {
                get
                {
                    return readOnlyField;
                }

                set
                {
                    readOnlyField = value;
                }
            }

            public bool ReadOnlyFieldSpecified1
            {
                get
                {
                    return readOnlyFieldSpecified;
                }

                set
                {
                    readOnlyFieldSpecified = value;
                }
            }

            public byte MultiField1
            {
                get
                {
                    return multiField;
                }

                set
                {
                    multiField = value;
                }
            }

            public bool MultiFieldSpecified1
            {
                get
                {
                    return multiFieldSpecified;
                }

                set
                {
                    multiFieldSpecified = value;
                }
            }

            public string ValueField1
            {
                get
                {
                    return valueField;
                }

                set
                {
                    valueField = value;
                }
            }

            public bool SelectedField1
            {
                get
                {
                    return selectedField;
                }

                set
                {
                    selectedField = value;
                }
            }

            public bool SelectedFieldSpecified1
            {
                get
                {
                    return selectedFieldSpecified;
                }

                set
                {
                    selectedFieldSpecified = value;
                }
            }

            public ushort MaxLengthField1
            {
                get
                {
                    return maxLengthField;
                }

                set
                {
                    maxLengthField = value;
                }
            }

            public bool MaxLengthFieldSpecified1
            {
                get
                {
                    return maxLengthFieldSpecified;
                }

                set
                {
                    maxLengthFieldSpecified = value;
                }
            }

            public bool SplitScreenField1
            {
                get
                {
                    return splitScreenField;
                }

                set
                {
                    splitScreenField = value;
                }
            }

            public bool SplitScreenFieldSpecified1
            {
                get
                {
                    return splitScreenFieldSpecified;
                }

                set
                {
                    splitScreenFieldSpecified = value;
                }
            }

            public bool GeolocationEnabledField1
            {
                get
                {
                    return geolocationEnabledField;
                }

                set
                {
                    geolocationEnabledField = value;
                }
            }

            public bool GeolocationEnabledFieldSpecified1
            {
                get
                {
                    return geolocationEnabledFieldSpecified;
                }

                set
                {
                    geolocationEnabledFieldSpecified = value;
                }
            }

            public bool GeolocationForcedField1
            {
                get
                {
                    return geolocationForcedField;
                }

                set
                {
                    geolocationForcedField = value;
                }
            }

            public bool GeolocationForcedFieldSpecified1
            {
                get
                {
                    return geolocationForcedFieldSpecified;
                }

                set
                {
                    geolocationForcedFieldSpecified = value;
                }
            }

            public bool GeolocationHiddenField1
            {
                get
                {
                    return geolocationHiddenField;
                }

                set
                {
                    geolocationHiddenField = value;
                }
            }

            public bool GeolocationHiddenFieldSpecified1
            {
                get
                {
                    return geolocationHiddenFieldSpecified;
                }

                set
                {
                    geolocationHiddenFieldSpecified = value;
                }
            }

            public string MinValueField1
            {
                get
                {
                    return minValueField;
                }

                set
                {
                    minValueField = value;
                }
            }

            public string MaxValueField1
            {
                get
                {
                    return maxValueField;
                }

                set
                {
                    maxValueField = value;
                }
            }

            public byte DecimalCountField1
            {
                get
                {
                    return decimalCountField;
                }

                set
                {
                    decimalCountField = value;
                }
            }

            public bool DecimalCountFieldSpecified1
            {
                get
                {
                    return decimalCountFieldSpecified;
                }

                set
                {
                    decimalCountFieldSpecified = value;
                }
            }

            public object UnitNameField1
            {
                get
                {
                    return unitNameField;
                }

                set
                {
                    unitNameField = value;
                }
            }

            public MainElementDataItemKeyValuePair[] KeyValuePairListField1
            {
                get
                {
                    return keyValuePairListField;
                }

                set
                {
                    keyValuePairListField = value;
                }
            }

            public byte IdField1
            {
                get
                {
                    return idField;
                }

                set
                {
                    idField = value;
                }
            }

            public string LabelField1
            {
                get
                {
                    return labelField;
                }

                set
                {
                    labelField = value;
                }
            }

            public string DescriptionField1
            {
                get
                {
                    return descriptionField;
                }

                set
                {
                    descriptionField = value;
                }
            }

            public bool MandatoryField1
            {
                get
                {
                    return mandatoryField;
                }

                set
                {
                    mandatoryField = value;
                }
            }

            public string ColorField1
            {
                get
                {
                    return colorField;
                }

                set
                {
                    colorField = value;
                }
            }

            public string TypeField1
            {
                get
                {
                    return typeField;
                }

                set
                {
                    typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class MainElementDataItemKeyValuePair
        {

            private byte keyField;

            private string valueField;

            private bool selectedField;

            private byte displayOrderField;

            /// <remarks/>
            public byte Key
            {
                get
                {
                    return this.KeyField;
                }
                set
                {
                    this.KeyField = value;
                }
            }

            /// <remarks/>
            public string Value
            {
                get
                {
                    return this.ValueField;
                }
                set
                {
                    this.ValueField = value;
                }
            }

            /// <remarks/>
            public bool Selected
            {
                get
                {
                    return this.SelectedField;
                }
                set
                {
                    this.SelectedField = value;
                }
            }

            /// <remarks/>
            public byte DisplayOrder
            {
                get
                {
                    return this.DisplayOrderField;
                }
                set
                {
                    this.DisplayOrderField = value;
                }
            }

            public byte KeyField
            {
                get
                {
                    return KeyField1;
                }

                set
                {
                    KeyField1 = value;
                }
            }

            public string ValueField
            {
                get
                {
                    return ValueField1;
                }

                set
                {
                    ValueField1 = value;
                }
            }

            public bool SelectedField
            {
                get
                {
                    return SelectedField1;
                }

                set
                {
                    SelectedField1 = value;
                }
            }

            public byte DisplayOrderField
            {
                get
                {
                    return DisplayOrderField1;
                }

                set
                {
                    DisplayOrderField1 = value;
                }
            }

            public byte KeyField1
            {
                get
                {
                    return keyField;
                }

                set
                {
                    keyField = value;
                }
            }

            public string ValueField1
            {
                get
                {
                    return valueField;
                }

                set
                {
                    valueField = value;
                }
            }

            public bool SelectedField1
            {
                get
                {
                    return selectedField;
                }

                set
                {
                    selectedField = value;
                }
            }

            public byte DisplayOrderField1
            {
                get
                {
                    return displayOrderField;
                }

                set
                {
                    displayOrderField = value;
                }
            }
        }
    }
}
