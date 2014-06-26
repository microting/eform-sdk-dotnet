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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Inspection
{
    public partial class Form1 : Form
    {

        private DbFacade dbFacade;

        string site_name, site_uuid, user_name, user_uuid, token, server_address, elementId;
        Http http;
        public Form1()
        {
            InitializeComponent();
            dbFacade = DbFacade.Instance;
            site_name = txtSiteName.Text;
            site_uuid = txtSiteUuid.Text;
            user_name = txtUserName.Text;
            user_uuid = txtUserUuid.Text;
            token = txtToken.Text;
            server_address = txtServerAddress.Text;
            http = new Http(token, server_address);

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            endDate = endDate.AddYears(1);
            Sites site = new Sites(1, site_name, site_uuid);
            Users user = new Users(1, user_name, user_uuid);
            
            //Adding main element
            Element mainElement = new Element("Check list 20", "Check list", 1, startDate, endDate, "en", true, false, "0", false, false, true, false, false, "Odense", false, "", "bla bla bla");

            //Adding sub elements to the main element
            Element subElement1 = new Element("check list colors", "Check list colors", 0, startDate, endDate, "en", false, false, "Check list 20", false, false, false, true, false, "", false, "", "This is the description for the color fields");
            Element subElement2 = new Element("check list 1", "Check list 1", 0, startDate, endDate, "en", false, false, "Check list 20", false, false, false, true, false, "", false, "", "this is the description 1");
            
            //Creating Lists and population them
            List<KeyValuePair> singleKeyValuePairList = new List<KeyValuePair>();
            singleKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
            singleKeyValuePairList.Add(new KeyValuePair("2", "option 2", false, "2"));
            singleKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

            List<KeyValuePair> multiKeyValuePairList = new List<KeyValuePair>();
            multiKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
            multiKeyValuePairList.Add(new KeyValuePair("2", "option 2", true, "2"));
            multiKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

            //Adding data elements to sub element 1 (Check list colors)
            DataElement singleSelectElement = new SingleSelectElement("2", "Single select search field", "this is a description", false, singleKeyValuePairList, 0, "check list colors");
            DataElement multiSelectElement = new MultiSelectElement("3", "Multi select field", "this is a description", false, multiKeyValuePairList, 0, "check list colors");
            DataElement numberElement = new NumberElement("4", "Number field", "this is a description", false, 0, 10000, 0, 2, "", 0, "check list colors");
            DataElement textElement = new TextElement("5", "Text field", "this is a description", "check list colors", false, "", 50, true, false, false, 0);
            DataElement commentElement = new CommentElement("6", "Comment field", "this is a description", false, "", 10000, false, 5, "check list colors");
            DataElement pictureElement = new PictureElement("7", "Picture field", "this is a description", false, 1, 5, "check list colors");
            DataElement checkBoxElement = new CheckBoxElement("8", "Check box field", "this is a description", false, false, false, 4, "check list colors");
            DataElement audioElement = new AudioElement("9", "Audio field", "this is a description", false, 1, 4, "check list colors");
            DataElement pdfElement = new PdfElement("10", "PDF field", "this is a description", "path/to/pdf.pdf", false, 3, "check list colors");
            DataElement dateElement = new DateElement("11", "Date field", "this is a description", false, startDate, startDate, startDate.ToString(), 3, "check list colors");
            DataElement noneElement = new NoneElement("12", "None field, only shows text", "this is a description", false, false, 2, "check list colors");
            DataElement timerElement = new TimerElement("13", "Timer", "this is a description", false, 2, "check list colors");
            DataElement signatureElement = new SignatureElement("14", "Signature", "this is a description", false, 1, "check list colors");



            //Adding data elements to sub element 2 (Check list 1)
            DataElement singleSelectElement2 = new SingleSelectElement("2", "Single select search field", "this is a description", false, singleKeyValuePairList, 0, "check list 1");
            DataElement multiSelectElement2 = new MultiSelectElement("3", "Multi select field", "this is a description", false, multiKeyValuePairList, 0, "check list 1");
            DataElement numberElement2 = new NumberElement("4", "Number field", "this is a description", false, 0, 10000, 0, 2, "", 0, "check list 1");
            DataElement textElement2 = new TextElement("5", "Text field", "this is a description", "check list 1", false, "", 50, true, false, false, 0);
            DataElement commentElement2 = new CommentElement("6", "Comment field", "this is a description", false, "", 10000, false, 5, "check list 1");
            DataElement pictureElement2 = new PictureElement("7", "Picture field", "this is a description", false, 1, 5, "check list 1");
            DataElement checkBoxElement2 = new CheckBoxElement("8", "Check box field", "this is a description", false, false, false, 4, "check list 1");
            DataElement audioElement2 = new AudioElement("9", "Audio field", "this is a description", false, 1, 4, "check list 1");
            DataElement pdfElement2 = new PdfElement("10", "PDF field", "this is a description", "path/to/pdf.pdf", false, 3, "check list 1");
            DataElement dateElement2 = new DateElement("11", "Date field", "this is a description", false, startDate, startDate, startDate.ToString(), 3, "check list 1");
            DataElement noneElement2 = new NoneElement("12", "None field, only shows text", "this is a description", false, false, 2, "check list 1");
            DataElement timerElement2 = new TimerElement("13", "Timer", "this is a description", false, 2, "check list 1");
            DataElement signatureElement2 = new SignatureElement("14", "Signature", "this is a description", false, 1, "check list 1");

            //Adding Main Element To "DB"
            dbFacade.elements.Add(mainElement);

            //Adding Sub Elements To "DB"
            dbFacade.elements.Add(subElement1);
            dbFacade.elements.Add(subElement2);

            //Adding Data Elements To "DB"
            dbFacade.dataElements.Add(singleSelectElement);
            dbFacade.dataElements.Add(multiSelectElement);
            dbFacade.dataElements.Add(numberElement);
            dbFacade.dataElements.Add(textElement);
            dbFacade.dataElements.Add(commentElement);
            dbFacade.dataElements.Add(pictureElement);
            dbFacade.dataElements.Add(checkBoxElement);
            dbFacade.dataElements.Add(audioElement);
            dbFacade.dataElements.Add(dateElement);
            dbFacade.dataElements.Add(noneElement);
            dbFacade.dataElements.Add(timerElement);
            dbFacade.dataElements.Add(signatureElement);

            dbFacade.dataElements.Add(singleSelectElement2);
            dbFacade.dataElements.Add(multiSelectElement2);
            dbFacade.dataElements.Add(numberElement2);
            dbFacade.dataElements.Add(textElement2);
            dbFacade.dataElements.Add(commentElement2);
            dbFacade.dataElements.Add(pictureElement2);
            dbFacade.dataElements.Add(checkBoxElement2);
            dbFacade.dataElements.Add(audioElement2);
            dbFacade.dataElements.Add(dateElement2);
            dbFacade.dataElements.Add(noneElement2);
            dbFacade.dataElements.Add(timerElement2);
            dbFacade.dataElements.Add(signatureElement2);
            
            
            Cases kase = new Cases("" , site.Id, 0, user.Id, "", 0, mainElement.Id);
            string xmlToMicroting = kase.toXml();
            txtSend.Text = xmlToMicroting;

            //Step 4 - Add text to txtXMLToMicroting
            string postResponse = http.Post(xmlToMicroting, site_uuid);
            XDocument xmlDoc = XDocument.Parse(postResponse);
            var xResponses = from xResponse in xmlDoc.Descendants("Response")
                            select new 
                            {
                                ExecutionStatus = xResponse.Element("Value").Attribute("type").Value,
                                Message = xResponse.Element("Value"),
                            };

            elementId = xResponses.First().Message.Value.ToString();
            txtXMLToMicroting.Text = postResponse;


        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string serverStatus = http.Status(elementId, site_uuid);
            txtCheck.Text = serverStatus;
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            string xmlResponse = http.Retrieve(elementId, 0, site_uuid);
            txtFetch.Text = xmlResponse;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string xmlResponse =  http.Delete(elementId, site_uuid);
            deleteResponseText.Text = xmlResponse;
        }

    }
}
