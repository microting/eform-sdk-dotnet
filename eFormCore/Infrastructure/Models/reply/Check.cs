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

namespace Microting.eForm.Infrastructure.Models.reply
{
    [Serializable]
    public class Check
    {
        #region con

        internal Check()
        {
            ElementList = new List<ElementList>();
        }

        #endregion

        #region var

        public string UnitId { get; set; }

        #region public string Date { get; set; }

        // private string date = "";
        //public string Date { get { return date.Substring(0, 19); } set { date = value; } }
        // public string Date { get { return date.AsSpan(0, 19).ToString(); } set { date = value; } }
        public string Date { get; set; }

        #endregion

        public string Worker { get; set; }
        public int? Id { get; set; }
        public string WorkerId { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string OsVersion { get; set; }
        public string SoftwareVersion { get; set; }

        [XmlArray("ElementList"), XmlArrayItem(typeof(ElementList), ElementName = "Element")]
        public List<ElementList> ElementList { get; set; }

        #endregion
    }
}