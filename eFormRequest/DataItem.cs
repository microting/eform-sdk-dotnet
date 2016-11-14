using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace eFormRequest
{
    #region xml tags
    [Serializable()]
    [XmlInclude(typeof(Audio))]
    [XmlInclude(typeof(Check_Box))]
    [XmlInclude(typeof(Entity_Search))]
    [XmlInclude(typeof(Comment))]
    [XmlInclude(typeof(Date))]
    [XmlInclude(typeof(None))]
    [XmlInclude(typeof(Number))]
    [XmlInclude(typeof(Multi_Select))]
    [XmlInclude(typeof(Show_PDF))]
    [XmlInclude(typeof(Picture))]
    [XmlInclude(typeof(SaveButton))]
    [XmlInclude(typeof(Signature))]
    [XmlInclude(typeof(Single_Select))]
    [XmlInclude(typeof(Entity_Select))]
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
        public string Id { get; set; }
        public bool Mandatory { get; set; }
        public bool ReadOnly { get; set; }
        public string Label { get; set; }

        [XmlElement("Description")]
        public CDataValue Description { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
        #endregion
    }

    #region children
    #region Audio
    public class Audio : DataItem
    {
        internal Audio()
        {

        }

        public Audio(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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
            
            Multi = multi;
        }

        public int Multi { get; set; }
    }
    #endregion

    #region Check_Box
    public class Check_Box : DataItem
    {
        internal Check_Box()
        {

        }

        public Check_Box(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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
            
            DefaultValue = defaultValue;
            Selected = selected;
        }

        public bool DefaultValue { get; set; }
        public bool Selected { get; set; }
    }
    #endregion

    #region Comment
    public class Comment : DataItem
    {
        internal Comment()
        {

        }

        public Comment(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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
          
            Value = value;
            Maxlength = maxLength;
            SplitScreen = splitScreen;
        }

        public string Value { get; set; }
        public int Maxlength { get; set; }
        public bool SplitScreen { get; set; }
    }
    #endregion

    #region Date
    public class Date : DataItem
    {
        internal Date()
        {

        }

        public Date(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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
            
            MinValue = minValue;
            MaxValue = maxValue;
            DefaultValue = defaultValue;
        }

        public string DefaultValue { get; set; }
        public DateTime MaxValue { get; set; }
        public DateTime MinValue { get; set; }
    }
    #endregion
    
    #region None
    public class None : DataItem
    {
        internal None()
        {

        }

        public None(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
        }
    }
    #endregion

    #region Number
    public class Number : DataItem
    {
        internal Number()
        {

        }

        public Number(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder,
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
            
            MinValue = minValue;
            MaxValue = maxValue;
            DefaultValue = defaultValue;
            DecimalCount = decimalCount;
            UnitName = unitName;
        }

        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public int DefaultValue { get; set; }
        public int DecimalCount { get; set; }
        public string UnitName { get; set; }
    }
    #endregion

    #region Multi_Select
    public class Multi_Select : DataItem
    {
        internal Multi_Select()
        {
            KeyValuePairList = new List<KeyValuePair>();
        }

        public Multi_Select(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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
        
            KeyValuePairList = keyValuePairList;
        }

        [XmlArray("KeyValuePairList"), XmlArrayItem(typeof(KeyValuePair), ElementName = "KeyValuePair")]
        public List<KeyValuePair> KeyValuePairList { get; set; }
    }
    #endregion

    #region Show_PDF
    public class Show_PDF : DataItem
    {
        internal Show_PDF()
        {

        }

        public Show_PDF(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
            string defaultValue)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
            
            DefaultValue = defaultValue;
        }

        public string DefaultValue { get; set; }
    }
    #endregion

    #region Picture
    public class Picture : DataItem
    {
        internal Picture()
        {

        }

        public Picture(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder,
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

            Multi = multi;
            GeolocationEnabled = geolocationEnabled;
    }

        public int Multi { get; set; }
        public bool GeolocationEnabled { get; set; }
    }
    #endregion

    #region SaveButton
    public class SaveButton : DataItem
    {
        internal SaveButton()
        {

        }

        public SaveButton(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder,
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

        public Signature(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;
        }
    }
    #endregion

    #region Single_Select
    public class Single_Select : DataItem
    {
        internal Single_Select()
        {
            KeyValuePairList = new List<KeyValuePair>();
        }

        public Single_Select(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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

        public Text(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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
            
            Value = value;
            MaxLength = maxLength;
            GeolocationEnabled = geolocationEnabled;
            GeolocationForced = geolocationForced;
            GeolocationHidden = geolocationhidden;
            BarcodeEnabled = barcodeEnabled;
            BarcodeType = barcodeType;
        }

        public string Value { get; set; }
        public int MaxLength { get; set; }
        public bool GeolocationEnabled { get; set; }
        public bool GeolocationForced { get; set; }
        public bool GeolocationHidden { get; set; }
        public bool BarcodeEnabled { get; set; }
        public string BarcodeType { get; set; }
    }
    #endregion

    #region Timer
    public class Timer : DataItem
    {
        internal Timer()
        {

        }

        public Timer(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
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

            StopOnSave = stopOnSave;
        }

        public bool StopOnSave { get; set; }
    }
    #endregion
    #endregion

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