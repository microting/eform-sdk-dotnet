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
    [XmlInclude(typeof(Show_Pdf))]
    [XmlInclude(typeof(Picture))]
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
        public string Description { get; set; }
        #region public string/enun Color { get; set; }
        public string Color { get; set; }
        public DataItemColors GetColor()
        {
            switch (Color.ToLower())
            {
                case "e2f4fb":
                    return DataItemColors.Blue;
                case "f5eafa":
                    return DataItemColors.Purple;
                case "f0f8db":
                    return DataItemColors.Green;
                case "fff6df":
                    return DataItemColors.Yellow;
                case "ffe4e4":
                    return DataItemColors.Red;
                case "":
                    return DataItemColors.None;
                default:
                    return DataItemColors.None;
            }
        }
        public void SetColor(DataItemColors color)
        {
            switch (color)
            {
                case DataItemColors.Blue:
                    Color = "e2f4fb";
                    break;
                case DataItemColors.Purple:
                    Color = "f5eafa";
                    break;
                case DataItemColors.Green:
                    Color = "f0f8db";
                    break;
                case DataItemColors.Yellow:
                    Color = "fff6df";
                    break;
                case DataItemColors.Red:
                    Color = "ffe4e4";
                    break;
                case DataItemColors.None:
                    Color = "";
                    throw new NotImplementedException("TODO PANIC !!!");
                    break;
                default:
                    throw new IndexOutOfRangeException("Color not found in accepted list");
            }
        }
        #endregion
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

        public Audio(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            int multi)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
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

        public Check_Box(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            bool value, bool selected)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;
            
            Value = value;
            Selected = selected;
        }

        public bool Value { get; set; }
        public bool Selected { get; set; }
    }
    #endregion

    #region Comment
    public class Comment : DataItem
    {
        internal Comment()
        {

        }

        public Comment(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            string value, int maxLength, bool splitScreen)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;
          
            Value = value;
            Maxlength = maxLength;
            SplitScreen = splitScreen;
        }

        public string Value { get; set; }
        public bool SplitScreen { get; set; }
        public int Maxlength { get; set; }
    }
    #endregion

    #region Date
    public class Date : DataItem
    {
        internal Date()
        {

        }

        public Date(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            DateTime minValue, DateTime maxValue, string value)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;
            
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
        }

        public string Value { get; set; }
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

        public None(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
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

        public Number(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            int minValue, int maxValue, int value, int decimalCount, string unitName)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;
            
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
            DecimalCount = decimalCount;
            UnitName = unitName;
        }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Value { get; set; }
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

        public Multi_Select(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            List<KeyValuePair> keyValuePairList)
        {
            KeyValuePairList = new List<KeyValuePair>();

            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;
        
            KeyValuePairList = keyValuePairList;
        }

        [XmlArray("KeyValuePairList"), XmlArrayItem(typeof(KeyValuePair), ElementName = "KeyValuePair")]
        public List<KeyValuePair> KeyValuePairList { get; set; }
    }
    #endregion

    #region Show_Pdf
    public class Show_Pdf : DataItem
    {
        internal Show_Pdf()
        {

        }

        public Show_Pdf(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            string value)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;
            
            Value = value;
        }

        public string Value { get; set; }
    }
    #endregion

    #region Picture
    public class Picture : DataItem
    {
        internal Picture()
        {

        }

        public Picture(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder,
            int multi, bool geolocationEnabled)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;

            Multi = multi;
            GeolocationEnabled = geolocationEnabled;
    }

        public int Multi { get; set; }
        public bool GeolocationEnabled { get; set; }
    }
    #endregion

    #region Signature
    public class Signature : DataItem
    {
        internal Signature()
        {

        }

        public Signature(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
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

        public Single_Select(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            List<KeyValuePair> keyValuePairList)
        {
            KeyValuePairList = new List<KeyValuePair>();

            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
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

        public Text(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            string value, int maxLength, bool geolocationEnabled, bool geolocationForced, bool geolocationhidden)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;
            
            Value = value;
            MaxLength = maxLength;
            GeolocationEnabled = geolocationEnabled;
            GeolocationForced = geolocationForced;
            GeolocationHidden = geolocationhidden;
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

        public Timer(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder, 
            bool stopOnSave)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;

            StopOnSave = stopOnSave;
        }

        public bool StopOnSave { get; set; }
    }
    #endregion
    #endregion

    public enum DataItemColors
    {
        Blue,
        Purple,
        Green,
        Yellow,
        Red,
        None
    }
}