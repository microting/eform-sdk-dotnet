/*
The MIT License (MIT)

Copyright (c) 2014 microting

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormDll
{
    public sealed class DbFacade
    {
        public List<Element> elements;
        public List<DataElement> dataElements = new List<DataElement>();

        static readonly DbFacade _dbFacade = new DbFacade();

        private DbFacade() {
            elements = new List<Element>();
            dataElements = new List<DataElement>();
        }

        public static DbFacade Instance
        {
            get { return _dbFacade; }
        }
        public List<Element> Elements { get; set; }
        public List<DataElement> DataElements { get; set; }

        public Element findElementById(string element_id)
        {
            return elements.Find(x => x.Id == element_id);
        }

        public List<Element> findChildElementsById(string element_id)
        {
            List<Element> myTempList = new List<Element>();
            foreach (Element e in elements)
            {
                if (e.ParentId == element_id)
                {
                    myTempList.Add(e);
                }
            }
            return myTempList;
        }

        public DataElement findDataElementById(string data_element_id)
        {
            return dataElements.Find(x => x.Id == data_element_id);
        }

        public List<DataElement> findAllDataElementsByElementId(string element_id)
        {
            List<DataElement> myTempList = new List<DataElement>();
            foreach (DataElement e in dataElements)
            {
                if (e.ElementId == element_id)
                {
                    myTempList.Add(e);
                }
            }
            return myTempList;
        }

    }
}
