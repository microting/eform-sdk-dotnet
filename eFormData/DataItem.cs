using eFormShared;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace eFormData
{
    #region xml tags
    [Serializable()]
    [XmlInclude(typeof(Audio))]
    [XmlInclude(typeof(CheckBox))]
    [XmlInclude(typeof(Comment))]
    [XmlInclude(typeof(Date))]
    [XmlInclude(typeof(EntitySearch))]
    [XmlInclude(typeof(EntitySelect))]
    [XmlInclude(typeof(None))]
    [XmlInclude(typeof(Number))]
    [XmlInclude(typeof(MultiSelect))]
    [XmlInclude(typeof(Picture))]
    [XmlInclude(typeof(ShowPdf))]
    [XmlInclude(typeof(SaveButton))]
    [XmlInclude(typeof(Signature))]
    [XmlInclude(typeof(SingleSelect))]
    [XmlInclude(typeof(Text))]
    [XmlInclude(typeof(Timer))]
    #endregion
    public class DataItem
    {
        #region con
        internal DataItem()
        {

        }
        #endregion

        #region var
        public int Id { get; set; }
        public bool Mandatory { get; set; }
        public bool ReadOnly { get; set; }
        public string Label { get; set; }

        [XmlElement("Description")]
        public CDataValue Description { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }

        [XmlIgnore]
        public bool Dummy { get; set; }
        #endregion
    }

    #region children
    #region Audio
    public class Audio : DataItem
    {
        internal Audio()
        {

        }

        public Audio(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            int multi)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            Multi = multi;
        }

        public int Multi { get; set; }
    }
    #endregion

    #region CheckBox
    public class CheckBox : DataItem
    {
        internal CheckBox()
        {

        }

        public CheckBox(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            bool defaultValue, bool selected)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            DefaultValue = defaultValue;
            Selected = selected;
        }

        #region var
        public bool DefaultValue { get; set; }
        public bool Selected { get; set; }
        #endregion
    }
    #endregion

    #region Comment
    public class Comment : DataItem
    {
        internal Comment()
        {

        }

        public Comment(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            string value, int maxLength, bool splitScreen)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            Value = value;
            Maxlength = maxLength;
            SplitScreen = splitScreen;
        }

        #region var
        public string Value { get; set; }
        public int Maxlength { get; set; }
        public bool SplitScreen { get; set; }
        #endregion
    }
    #endregion

    #region Date
    public class Date : DataItem
    {
        internal Date()
        {

        }

        public Date(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            DateTime minValue, DateTime maxValue, string defaultValue)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            DefaultValue = defaultValue;
            MaxValue = maxValue;
            MinValue = minValue;
        }

        #region var
        public string DefaultValue { get; set; }

        #region public string/DateTime MaxValue { get; set; }
        [XmlIgnore]
        public DateTime MaxValue { get; set; }

        [XmlElement("MaxValue")]
        public string MaxValueString
        {
            get { return MaxValue.ToString("yyyy-MM-dd"); }
            set { MaxValue = DateTime.Parse(value); }
        }
        #endregion

        #region public string/DateTime MinValue { get; set; }
        [XmlIgnore]
        public DateTime MinValue { get; set; }

        [XmlElement("MinValue")]
        public string MinValueString
        {
            get { return MinValue.ToString("yyyy-MM-dd"); }
            set { MinValue = DateTime.Parse(value); }
        }
        #endregion
        #endregion
    }
    #endregion

    #region None
    public class None : DataItem
    {
        internal None()
        {

        }

        public None(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;
        }
    }
    #endregion

    #region Number
    public class Number : DataItem
    {
        internal Number()
        {

        }

        public Number(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            long minValue, long maxValue, int defaultValue, int decimalCount, string unitName)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            MinValue = minValue;
            MaxValue = maxValue;
            DefaultValue = defaultValue;
            DecimalCount = decimalCount;
            UnitName = unitName;
        }

        #region var
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public int DefaultValue { get; set; }
        public int DecimalCount { get; set; }
        public string UnitName { get; set; }
        #endregion
    }
    #endregion

    #region MultiSelect
    public class MultiSelect : DataItem
    {
        internal MultiSelect()
        {
            KeyValuePairList = new List<KeyValuePair>();
        }

        public MultiSelect(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            List<KeyValuePair> keyValuePairList)
        {
            KeyValuePairList = new List<KeyValuePair>();

            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            KeyValuePairList = keyValuePairList;
        }

        [XmlArray("KeyValuePairList"), XmlArrayItem(typeof(KeyValuePair), ElementName = "KeyValuePair")]
        public List<KeyValuePair> KeyValuePairList { get; set; }
    }
    #endregion

    #region Picture
    public class Picture : DataItem
    {
        internal Picture()
        {

        }

        public Picture(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            int multi, bool geolocationEnabled)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            Multi = multi;
            GeolocationEnabled = geolocationEnabled;
        }

        #region var
        public int Multi { get; set; }
        public bool GeolocationEnabled { get; set; }
        #endregion
    }
    #endregion

    #region ShowPdf
    public class ShowPdf : DataItem
    {
        internal ShowPdf()
        {

        }

        public ShowPdf(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            string value)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            Value = value;
        }

        public string Value { get; set; }
    }
    #endregion

    #region SaveButton
    public class SaveButton : DataItem
    {
        internal SaveButton()
        {

        }

        public SaveButton(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            string value)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            Value = value;
        }

        public string Value { get; set; }
    }
    #endregion

    #region Signature
    public class Signature : DataItem
    {
        internal Signature()
        {

        }

        public Signature(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;
        }
    }
    #endregion

    #region SingleSelect
    public class SingleSelect : DataItem
    {
        internal SingleSelect()
        {
            KeyValuePairList = new List<KeyValuePair>();
        }

        public SingleSelect(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            List<KeyValuePair> keyValuePairList)
        {
            KeyValuePairList = new List<KeyValuePair>();

            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            KeyValuePairList = keyValuePairList;
        }

        [XmlArray("KeyValuePairList"), XmlArrayItem(typeof(KeyValuePair), ElementName = "KeyValuePair")]
        public List<KeyValuePair> KeyValuePairList { get; set; }
    }
    #endregion

    #region Text
    public class Text : DataItem
    {
        internal Text()
        {

        }

        public Text(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            string value, int maxLength, bool geolocationEnabled, bool geolocationForced, bool geolocationhidden, bool barcodeEnabled, string barcodeType)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            Value = value;
            MaxLength = maxLength;
            GeolocationEnabled = geolocationEnabled;
            GeolocationForced = geolocationForced;
            GeolocationHidden = geolocationhidden;
            BarcodeEnabled = barcodeEnabled;
            BarcodeType = barcodeType;
        }

        #region var
        public string Value { get; set; }
        public int MaxLength { get; set; }
        public bool GeolocationEnabled { get; set; }
        public bool GeolocationForced { get; set; }
        public bool GeolocationHidden { get; set; }
        public bool BarcodeEnabled { get; set; }
        public string BarcodeType { get; set; }
        #endregion
    }
    #endregion

    #region Timer
    public class Timer : DataItem
    {
        internal Timer()
        {

        }

        public Timer(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            bool stopOnSave)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            StopOnSave = stopOnSave;
        }

        public bool StopOnSave { get; set; }
    }
    #endregion

    #region EntitySearch
    public class EntitySearch : DataItem
    {
        #region con
        internal EntitySearch()
        {

        }

        public EntitySearch(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            int defaultValue, int entityTypeId, bool isNum, string queryType, int minSearchLenght, bool barcodeEnabled, string barcodeType)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            DefaultValue = defaultValue;
            EntityTypeId = entityTypeId;
            IsNum = isNum;
            QueryType = queryType;
            MinSearchLenght = minSearchLenght;
            BarcodeEnabled = barcodeEnabled;
            BarcodeType = barcodeType;
        }
        #endregion

        #region var
        public int DefaultValue { get; set; }
        public int EntityTypeId { get; set; }

        public bool IsNum { get; set; }
        public string QueryType { get; set; }
        public int MinSearchLenght { get; set; }
        public bool BarcodeEnabled { get; set; }
        public string BarcodeType { get; set; }
        #endregion
    }
    #endregion

    #region EntitySelect
    public class EntitySelect : DataItem
    {
        #region con
        internal EntitySelect()
        {

        }

        public EntitySelect(int id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, bool dummy,
            int defaultValue, int source)

        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            Dummy = dummy;

            DefaultValue = defaultValue;
            Source = source;
        }
        #endregion

        #region var
        public int DefaultValue { get; set; }
        public int Source { get; set; }
        #endregion
    }
    #endregion
    #endregion

    public class Field : DataItem
    {
        public List<FieldValue> FieldValues { get; set; }
        public string FieldType { get; set; }
        public string FieldValue { get; set; }
        public List<KeyValuePair> KeyValuePairList { get; set; }
    }

    public class FieldValue : DataItem
    {

        public int FieldId { get; set; }
        public string FieldType { get; set; }
        public DateTime DateOfDoing { get; set; }
        public string Value { get; set; }
        public string ValueReadable { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Altitude { get; set; }
        public string Heading { get; set; }
        public string Accuracy { get; set; }
        public DateTime? Date { get; set; }
        public string UploadedData { get; set; }
        public UploadedData UploadedDataObj { get; set; }
        public List<KeyValuePair> KeyValuePairList { get; set; }
    }

    public class FieldGroup : DataItem
    {
        internal FieldGroup()
        {

        }

        public FieldGroup(int id, string label, CDataValue description, string color, int displayOrder, string value, List<DataItem> dataItemList)
        {
            Id = id;
            Label = label;
            Description = description;
            Color = color;
            DisplayOrder = displayOrder;

            Value = value;
            DataItemList = dataItemList;
        }

        public string Value { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }

        public static explicit operator FieldGroup(DataItemGroup dataItemGroup)
        {
            CDataValue description = new CDataValue();
            description.InderValue = dataItemGroup.Description;
            FieldGroup fg = new FieldGroup(int.Parse(dataItemGroup.Id), dataItemGroup.Label, description, dataItemGroup.Color, dataItemGroup.DisplayOrder, "", dataItemGroup.DataItemList);
            return fg;
        }
    }

    public class UploadedData
    {
        public int Id { get; set; }
        public string Checksum { get; set; }
        public string Extension { get; set; }
        public string CurrentFile { get; set; }
        public int? UploaderId { get; set; }
        public string UploaderType { get; set; }
        public string FileLocation { get; set; }
        public string FileName { get; set; }

    }

    public enum DataItemColors
    {
        e2f4fb_Blue,
        f5eafa_Purple,
        f0f8db_Green,
        fff6df_Yellow,
        ffe4e4_Red,
        None_Default 
    }
}