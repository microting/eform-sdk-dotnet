using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
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
