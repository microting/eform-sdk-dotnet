using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormDll
{
    public class eFormMain
    {
        #region con
        public eFormMain()
        {

        }
        #endregion

        #region public
        public string Post(string apiId, string token, string serverAddress, string xmlStr)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Post(xmlStr, apiId);
        }

        public string CheckStatus(string apiId, string token, string serverAddress, string eFormId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Status(eFormId, apiId);
        }

        public string RetrieveFirst(string apiId, string token, string serverAddress, string eFormId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Retrieve(eFormId, 0, apiId); //Always gets the first
        }

        public string RetrieveId(string apiId, string token, string serverAddress, string eFormId, int eFormResponseId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Retrieve(eFormId, eFormResponseId, apiId);
        }

        public string Delete(string apiId, string token, string serverAddress, string eFormId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Delete(eFormId, apiId);
        }

        #region demo
        public string _DemoPostSample(string apiId, string token, string serverAddress)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Post(CreateSampleXml(apiId), apiId);
        }

        public string _DemoCreateXml(string apiId, string token, string serverAddress)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            return CreateSampleXml(apiId);
        }
        #endregion
        #endregion

        #region private
        private string CheckInput(string token, string serverAddress)
        {
            string returnMsg = "";

            if (token.Length != 32)
            {
                return "Tokens are always 32 charactors long" + Environment.NewLine ;
            }

            if (!serverAddress.Contains("http://") && !serverAddress.Contains("https://"))
            {
                return "Server Address is missing 'http://' or 'https://'" + Environment.NewLine;
            }

            return returnMsg.TrimEnd();
        }

        private string CreateSampleXml(string apiId)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddYears(1);
            DbFacade dbFacade = DbFacade.Instance;
            Sites site = new Sites(1, "", apiId);
            Users user = new Users(1, "", "");

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

            Cases kase = new Cases("", site.Id, 0, user.Id, "", 0, mainElement.Id);
            return kase.toXml();
        }
        #endregion

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

#pragma warning disable 0108
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Type GetType()
        {
            return base.GetType();
        }
#pragma warning restore 0108
    }
}
