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

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microting.eForm.Dto;
using KeyValuePair = Microting.eForm.Dto.KeyValuePair;

namespace Microting.eForm.Infrastructure.Models
{
    // xml tags
    [Serializable]
    [XmlInclude(typeof(Audio))]
    [XmlInclude(typeof(CheckBox))]
    [XmlInclude(typeof(Comment))]
    [XmlInclude(typeof(Date))]
    [XmlInclude(typeof(EntitySearch))]
    [XmlInclude(typeof(EntitySelect))]
    [XmlInclude(typeof(None))]
    [XmlInclude(typeof(Number))]
    [XmlInclude(typeof(NumberStepper))]
    [XmlInclude(typeof(MultiSelect))]
    [XmlInclude(typeof(Picture))]
    [XmlInclude(typeof(ShowPdf))]
    [XmlInclude(typeof(SaveButton))]
    [XmlInclude(typeof(Signature))]
    [XmlInclude(typeof(SingleSelect))]
    [XmlInclude(typeof(Text))]
    [XmlInclude(typeof(Timer))]
    [XmlInclude(typeof(FieldContainer))]
    //
    public class DataItem
    {
        // con
        internal DataItem()
        {
        }
        //

        // var
        public int Id { get; set; }
        public bool Mandatory { get; set; }
        public bool ReadOnly { get; set; }
        public string Label { get; set; }

        [XmlElement("Description")] public CDataValue Description { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }

        [XmlIgnore] public bool Dummy { get; set; }

        public string OriginalId { get; set; }
        //
    }

    // children
    // Audio
    public class Audio : DataItem
    {
        internal Audio()
        {
        }

        public Audio(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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
    //

    // CheckBox
    public class CheckBox : DataItem
    {
        internal CheckBox()
        {
        }

        public CheckBox(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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

        // var
        public bool DefaultValue { get; set; }

        public bool Selected { get; set; }
        //
    }
    //

    // Comment
    public class Comment : DataItem
    {
        internal Comment()
        {
        }

        public Comment(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
            string value, int maxLength, bool split)
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
            Split = split;
        }

        // var
        public string Value { get; set; }
        public int Maxlength { get; set; }

        public bool Split { get; set; }
        //
    }
    //

    // Date
    public class Date : DataItem
    {
        internal Date()
        {
        }

        public Date(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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

        // var
        public string DefaultValue { get; set; }

        // public string/DateTime MaxValue { get; set; }
        [XmlIgnore] public DateTime MaxValue { get; set; }

        [XmlElement("MaxValue")]
        public string MaxValueString
        {
            get { return MaxValue.ToString("yyyy-MM-dd"); }
            set { MaxValue = DateTime.Parse(value); }
        }
        //

        // public string/DateTime MinValue { get; set; }
        [XmlIgnore] public DateTime MinValue { get; set; }

        [XmlElement("MinValue")]
        public string MinValueString
        {
            get { return MinValue.ToString("yyyy-MM-dd"); }
            set { MinValue = DateTime.Parse(value); }
        }
        //
        //
    }
    //

    // None
    public class None : DataItem
    {
        internal None()
        {
        }

        public None(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy)
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
    //

    // Number
    public class Number : DataItem
    {
        internal Number()
        {
        }

        public Number(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
            string minValue, string maxValue, int defaultValue, int decimalCount, string unitName)
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

        // var
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public int DefaultValue { get; set; }
        public int DecimalCount { get; set; }

        public string UnitName { get; set; }
        //
    }
    //

    // Number Stepper
    public class NumberStepper : DataItem
    {
        internal NumberStepper()
        {
        }

        public NumberStepper(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
            string minValue, string maxValue, int defaultValue, int decimalCount, string unitName)
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

        // var
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public int DefaultValue { get; set; }
        public int DecimalCount { get; set; }

        public string UnitName { get; set; }
        //
    }
    //

    // MultiSelect
    public class MultiSelect : DataItem
    {
        internal MultiSelect()
        {
            KeyValuePairList = new List<KeyValuePair>();
        }

        public MultiSelect(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy, System.Collections.Generic.List<KeyValuePair> keyValuePairList)
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
        public System.Collections.Generic.List<KeyValuePair> KeyValuePairList { get; set; }
    }
    //

    // Picture
    public class Picture : DataItem
    {
        internal Picture()
        {
        }

        public Picture(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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

        // var
        public int Multi { get; set; }

        public bool GeolocationEnabled { get; set; }
        //
    }
    //

    // ShowPdf
    public class ShowPdf : DataItem
    {
        internal ShowPdf()
        {
        }

        public ShowPdf(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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
    //

    // SaveButton
    public class SaveButton : DataItem
    {
        internal SaveButton()
        {
        }

        public SaveButton(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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
    //

    // Signature
    public class Signature : DataItem
    {
        internal Signature()
        {
        }

        public Signature(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy)
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
    //

    // SingleSelect
    public class SingleSelect : DataItem
    {
        internal SingleSelect()
        {
            KeyValuePairList = new List<KeyValuePair>();
        }

        public SingleSelect(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy, System.Collections.Generic.List<KeyValuePair> keyValuePairList)
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
        public System.Collections.Generic.List<KeyValuePair> KeyValuePairList { get; set; }
    }
    //

    // Text
    public class Text : DataItem
    {
        internal Text()
        {
        }

        public Text(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
            string value, int maxLength, bool geolocationEnabled, bool geolocationForced, bool geolocationhidden,
            bool barcodeEnabled, string barcodeType)
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

        // var
        public string Value { get; set; }
        public int MaxLength { get; set; }
        public bool GeolocationEnabled { get; set; }
        public bool GeolocationForced { get; set; }
        public bool GeolocationHidden { get; set; }
        public bool BarcodeEnabled { get; set; }

        public string BarcodeType { get; set; }
        //
    }
    //

    // Timer
    public class Timer : DataItem
    {
        internal Timer()
        {
        }

        public Timer(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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
    //

    // EntitySearch
    public class EntitySearch : DataItem
    {
        // con
        internal EntitySearch()
        {
        }

        public EntitySearch(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
            int defaultValue, int entityTypeId, bool isNum, string queryType, int minSearchLenght, bool barcodeEnabled,
            string barcodeType)
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
        //

        // var
        public int DefaultValue { get; set; }
        public int EntityTypeId { get; set; }

        public bool IsNum { get; set; }
        public string QueryType { get; set; }
        public int MinSearchLenght { get; set; }
        public bool BarcodeEnabled { get; set; }

        public string BarcodeType { get; set; }
        //
    }
    //

    // EntitySelect
    public class EntitySelect : DataItem
    {
        // con
        internal EntitySelect()
        {
        }

        public EntitySelect(int id, bool mandatory, bool readOnly, string label, string description, string color,
            int displayOrder, bool dummy,
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
        //

        // var
        public int DefaultValue { get; set; }

        public int Source { get; set; }
        //
    }
    //
    //

    public class Field : DataItem
    {
        public List<FieldValue> FieldValues { get; set; }
        public string FieldType { get; set; }
        public string FieldValue { get; set; }
        public int? EntityGroupId { get; set; }
        public List<KeyValuePair> KeyValuePairList { get; set; }
    }

    public class FieldValue : DataItem
    {
        public int FieldId { get; set; }
        public string FieldType { get; set; }
        public DateTime DateOfDoing { get; set; }
        public string Value { get; set; }
        public string MicrotingUuid { get; set; }
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

    public class FieldContainer : DataItem
    {
        internal FieldContainer()
        {
        }

        public FieldContainer(int id, string label, CDataValue description, string color, int displayOrder,
            string value, List<DataItem> dataItemList)
        {
            Id = id;
            Label = label;
            Description = description;
            Color = color;
            DisplayOrder = displayOrder;

            Value = value;
            DataItemList = dataItemList;
            FieldType = "FieldContainer";
        }

        public string FieldType { get; set; }
        public string Value { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }

        public static explicit operator FieldContainer(DataItemGroup dataItemGroup)
        {
            CDataValue description = new CDataValue();
            description.InderValue = dataItemGroup.Description;
            FieldContainer fg = new FieldContainer(int.Parse(dataItemGroup.Id), dataItemGroup.Label, description,
                dataItemGroup.Color, dataItemGroup.DisplayOrder, "", dataItemGroup.DataItemList);
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