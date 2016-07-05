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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using eFormDll;
using eFormRequest;
using eFormResponse;

namespace eFormTester
{
    public partial class Tester : Form
    {
        #region var
        private eForm main = new eForm();
        private string apiId, organizationId, token, serverAddress, request, sampleXml;
        private DateTime startDate, endDate;
        #endregion

        #region con
        public Tester()
        {
            InitializeComponent();

            #region tries loads latest input
            try
            {
                string[] lines = File.ReadAllLines("LatestInput.txt");

                txtApiId.Text = lines[0];
                txtOrganizationId.Text = lines[1];
                txtToken.Text = lines[2];
                txtServerAddress.Text = lines[3];
            }
            catch (Exception)
            {
            }
            #endregion

            UpdateXmlStr(0);
            cbxDropDown.Text =    "Sample 1. Basic";

            cbxDropDown.Items.Add("Sample 1. Basic");
            cbxDropDown.Items.Add("Sample 2. Extended");
            cbxDropDown.Items.Add("Sample 3. Advanced");
            cbxDropDown.Items.Add("Sample 4. Complex");
        }
        #endregion

        #region buttons
        private void btnSendXml_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Sends the XML using the actual method, and get it's response XML
            string response = main.PostXml(apiId, token, serverAddress, request);
            //Sends XML, and gets a response


            tbxResponse.Text = response;
            AddToLog(startDate, response);
            Cursor.Current = Cursors.Default;
        }

        private void btnCheckId_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Sends the Id using the actual method, and get it's response XML
            string response = main.CheckStatus(apiId, token, serverAddress, request);
            //Get the Id's status


            tbxResponse.Text = response;
            AddToLog(startDate, response);
            Cursor.Current = Cursors.Default;
        }

        private void btnFetchId_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Sends the Id using the actual method, and get it's response XML
            string response = main.Retrieve(apiId, token, serverAddress, request);
            //Get the Id's data


            tbxResponse.Text = response;
            AddToLog(startDate, response);
            Cursor.Current = Cursors.Default;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Sends the Id using the actual method, and get it's response XML
            string response = main.Delete(apiId, token, serverAddress, request);
            //Marks the Id to be deleted


            tbxResponse.Text = response;
            AddToLog(startDate, response);
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region help buttons/drop down
        private void btnSendSample_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Ex. of a post with auto. gen. XML and it's response XML returned
            string response = DemoPostSample(apiId, token, serverAddress);


            tbxResponse.Text = response;
            AddToLog(startDate, response);
            Cursor.Current = Cursors.Default;
        }

        private void btnCreateXml_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Auto. gen. XML
            string xml = sampleXml;


            tbxRequest.Text = xml;
            Cursor.Current = Cursors.Default;
        }

        private void btnCreateId_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Auto. gen. Id
            string response = DemoPostSample(apiId, token, serverAddress);


            string id = RetriveId(response);
            if (id != "")
                tbxRequest.Text = id;
            else
                tbxRequest.Text = response + " // Failed to auto. gen. a Id";


            Cursor.Current = Cursors.Default;
        }

        private void btnRetriveId_Click(object sender, EventArgs e)
        {
            string id = RetriveId(tbxResponse.Text);
            if (id != "")
                tbxRequest.Text = id;
        }

        private void btnVerifyXmlRequest_Click(object sender, EventArgs e)
        {
            try
            {
                MainElement main = new MainElement();
                main = main.XmlToClass(tbxRequest.Text);
                string xmlStrResult = main.ClassToXml();

                MessageBox.Show("The XML request is convertable", "Info", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed. It was not convertable. Reason given:" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, "ERROR", MessageBoxButtons.OK);
            }
        }

        private void btnVerifyXmlResponse_Click(object sender, EventArgs e)
        {
            try
            {
                Response resp = new Response();
                resp = resp.XmlToClass(tbxResponse.Text);
                string xmlStrResult = resp.ClassToXml();

                MessageBox.Show("The XML response is convertable", "Info", MessageBoxButtons.OK);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed. It was not convertable. Reason given:" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, "ERROR", MessageBoxButtons.OK);
            }
        }

        private void cbxDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = (int)cbxDropDown.SelectedIndex;
            UpdateXmlStr(selectedIndex);
        }
        #endregion

        #region private
        private string DemoPostSample(string apiId, string token, string serverAddress)
        {
            eForm ef = new eForm();
            return ef.PostXml(apiId, token, serverAddress, sampleXml);
        }

        private string RetriveId(string str)
        {
            try
            {
                if (str.Contains("Value type=\"success\""))
                {

                    int startPoint = str.IndexOf("Value type=\"success\"") + 21; //21 magic number of the lenght. Digs out the Id
                    string result = str.Substring(startPoint, str.IndexOf("<", startPoint) - startPoint);

                    if (int.Parse(result) > 0)
                        return result;
                }
            }
            catch
            {
            }
            return "";
        }

        private void MapInput()
        {
            apiId = txtApiId.Text;
            organizationId = txtOrganizationId.Text;
            token = txtToken.Text;
            serverAddress = txtServerAddress.Text;
            request = tbxRequest.Text;
            startDate = DateTime.Now;
            endDate = DateTime.Now.AddYears(1);
        }

        private void StoreLatestInput()
        {
            try
            {
                using (StreamWriter file = new StreamWriter("LatestInput.txt"))
                {
                    file.WriteLine(apiId);
                    file.WriteLine(organizationId);
                    file.WriteLine(token);
                    file.WriteLine(serverAddress);
                }
            }
            catch (Exception) { }
        }

        private void AddToLog(DateTime startDate, string postResponse)
        {
            try
            {
                string pathLog = "Log.txt";
                if (!File.Exists(pathLog))
                {
                    using (StreamWriter sw = File.CreateText(pathLog)) { }
                }

                using (StreamWriter sw = File.AppendText(pathLog))
                {
                    sw.WriteLine(startDate + " # " + postResponse);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to write to log." + Environment.NewLine +
                    Environment.NewLine +
                    "- Exception message :" + Environment.NewLine +
                    ex.Message + Environment.NewLine +
                    "- Exception source :" + Environment.NewLine +
                    ex.Source + Environment.NewLine +
                    "- Exception StackTrace :" + Environment.NewLine +
                    ex.StackTrace,
                    "Exception found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        private void UpdateXmlStr(int sampleIndex)
        {
            if (sampleIndex == 0)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement("-thehrehrehfsdj check list", "Sample 1", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, new List<Element>());

                DataElement e1 = new DataElement("My basic check list", "Basic list", 1, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e1);

                e1.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", DataItemColors.Blue, 1, 0, 1000, 2, 0, ""));
                e1.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", DataItemColors.Green, 8, "true", 100, false, false, true));
                e1.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", DataItemColors.Blue, 3, "value", 10000, false));
                e1.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", DataItemColors.Yellow, 4, 1, true));
                e1.DataItemList.Add(new Check_Box("5", false, true, "Check box", "this is a description", DataItemColors.Purple, 15, true, true));
                e1.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", DataItemColors.Red, 16, startDate, startDate, startDate.ToString()));
                e1.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", DataItemColors.Yellow, 7));
                #endregion
                sampleXml = sample.ClassToXml();
            }

            if (sampleIndex == 1)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement("Extended check list", "Sample 2", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, new List<Element>());

                DataElement e1 = new DataElement("My extended check list 1", "Extended list", 1, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e1);

                DataElement e2 = new DataElement("My extended check list 2", "Extended list", 2, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e2);


                e1.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", DataItemColors.Blue, 1, 0, 1000, 2, 0, ""));
                e1.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", DataItemColors.Green, 2, "true", 100, false, false, true));
                e1.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", DataItemColors.Blue, 3, "value", 10000, false));
                e1.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", DataItemColors.Yellow, 4, 1, true));
                e1.DataItemList.Add(new Check_Box("5", true, false, "Check box", "this is a description", DataItemColors.Purple, 5, true, true));
                e1.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", DataItemColors.Red, 6, startDate, startDate, startDate.ToString()));
                e1.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", DataItemColors.Yellow, 7));
                e1.DataItemList.Add(new eFormRequest.Timer("8", false, false, "Timer", "this is a description", DataItemColors.Purple, 13, false));
                e1.DataItemList.Add(new Signature("9", false, false, "Signature", "this is a description", DataItemColors.Purple, 14));

                e2.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", DataItemColors.Blue, 1, 0, 1000, 2, 0, ""));
                e2.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", DataItemColors.Green, 2, "true", 100, false, false, true));
                e2.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", DataItemColors.Blue, 3, "value", 10000, false));
                e2.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", DataItemColors.Yellow, 4, 1, true));
                e2.DataItemList.Add(new Check_Box("5", false, true, "Check box", "this is a description", DataItemColors.Purple, 5, true, true));
                e2.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", DataItemColors.Red, 6, startDate, startDate, startDate.ToString()));
                e2.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", DataItemColors.Yellow, 7));
                e2.DataItemList.Add(new eFormRequest.Timer("8", false, false, "Timer", "this is a description", DataItemColors.Purple, 8, false));
                e2.DataItemList.Add(new Signature("9", false, false, "Signature", "this is a description", DataItemColors.Purple, 9));
                #endregion
                sampleXml = sample.ClassToXml();
            }

            if (sampleIndex == 2)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement("Advanced check list", "Sample 3", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, new List<Element>());

                GroupElement g1 = new GroupElement("Group lists", "Group of advanced check lists", 1, "Group element", false, false, false, false, "", new List<Element>());
                sample.ElementList.Add(g1);


                DataElement e1 = new DataElement("My advanced check list 1", "Advanced list", 1, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                g1.ElementList.Add(e1);

                DataElement e2 = new DataElement("My advanced check list 2", "Advanced list", 2, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                g1.ElementList.Add(e2);

                DataElement e3 = new DataElement("My advanced check list 3", "Advanced list", 3, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                g1.ElementList.Add(e3);


                List<KeyValuePair> singleKeyValuePairList = new List<KeyValuePair>();
                singleKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
                singleKeyValuePairList.Add(new KeyValuePair("2", "option 2", false, "2"));
                singleKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

                List<KeyValuePair> multiKeyValuePairList = new List<KeyValuePair>();
                multiKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
                multiKeyValuePairList.Add(new KeyValuePair("2", "option 2", true, "2"));
                multiKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

                e1.DataItemList.Add(new Single_Select("1", false, false, "Single select field", "this is a description", DataItemColors.Blue, 1, singleKeyValuePairList));
                e1.DataItemList.Add(new Multi_Select("2", false, false, "Multi select field", "this is a description", DataItemColors.Blue, 2, multiKeyValuePairList));
                e1.DataItemList.Add(new Audio("3", false, false, "Audio field", "this is a description", DataItemColors.Yellow, 3, 1));
                e1.DataItemList.Add(new Show_Pdf("4", false, false, "PDF field", "this is a description", DataItemColors.Yellow, 4, @"http://www.analysis.im/uploads/seminar/pdf-sample.pdf"));
                e1.DataItemList.Add(new Comment("5", false, false, "Comment field", "this is a description", DataItemColors.Blue, 5, "value", 10000, false));

                e2.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", DataItemColors.Blue, 1, 0, 1000, 2, 0, ""));
                e2.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", DataItemColors.Green, 2, "true", 100, false, false, true));
                e2.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", DataItemColors.Blue, 3, "value", 10000, false));
                e2.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", DataItemColors.Yellow, 4, 1, true));
                e2.DataItemList.Add(new Check_Box("5", false, false, "Check box", "this is a description", DataItemColors.Purple, 5, true, true));
                e2.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", DataItemColors.Red, 6, startDate, startDate, startDate.ToString()));
                e2.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", DataItemColors.Yellow, 7));
                e2.DataItemList.Add(new eFormRequest.Timer("8", false, false, "Timer", "this is a description", DataItemColors.Purple, 8, false));
                e2.DataItemList.Add(new Signature("9", false, false, "Signature", "this is a description", DataItemColors.Purple, 9));

                e3.DataItemList.Add(new Check_Box("1", true, false, "You are sure?", "Verify please", DataItemColors.Purple, 1, false, false));
                #endregion
                sampleXml = sample.ClassToXml();
            }

            if (sampleIndex == 3)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement("Complex check list", "Sample 4", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, new List<Element>());

                GroupElement g1 = new GroupElement("Group lists", "Group of groups", 1, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g2 = new GroupElement("Group lists", "Group of complex check lists 2", 2, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g3 = new GroupElement("Group lists", "Group of complex check lists 3", 3, "Group element", false, false, false, false, "", new List<Element>());
                sample.ElementList.Add(g1);
                sample.ElementList.Add(g2);
                sample.ElementList.Add(g3);

                GroupElement g11 = new GroupElement("Group lists", "Sub group of group 1, with check lists", 1, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g12 = new GroupElement("Group lists", "Sub group of group 1, with sub groups", 2, "Group element", false, false, false, false, "", new List<Element>());
                g1.ElementList.Add(g11);
                g1.ElementList.Add(g12);

                GroupElement g121 = new GroupElement("Group lists", "Sub sub group of group with check lists 1", 1, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g122 = new GroupElement("Group lists", "Sub sub group of group with check lists 2", 2, "Group element", false, false, false, false, "", new List<Element>());
                g12.ElementList.Add(g121);
                g12.ElementList.Add(g122);



                DataElement e1 = new DataElement("My complex check list 1", "Complex list", 101, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e1);

                DataElement e2 = new DataElement("My complex check list 2", "Complex list", 2, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                g2.ElementList.Add(e2);
                g3.ElementList.Add(e2);
                g11.ElementList.Add(e2);
                g121.ElementList.Add(e2);

                DataElement e3 = new DataElement("My complex check list 3", "Complex list", 1001, "Data element", true, true, true, false, "", new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e3);
                g122.ElementList.Add(e3);

                //Done - missing field to be renamed server side
                ////Entity e_1 = new Entity("iden", "decription 1", "km", "colour", "radioCode", "1");
                ////Entity e_2 = new Entity("iden", "decription 2", "km", "colour", "radioCode", "2");
                ////Entity e_3 = new Entity("iden", "decription 3", "km", "colour", "radioCode", "3");
                ////List<Entity> lstE = new List<Entity>();
                ////lstE.Add(e_1);
                ////lstE.Add(e_2);
                ////lstE.Add(e_3);

                ////EntityType eS = new EntityType("name", "1001", lstE);

                ////int alreadyKnownId = 12878;
                ////e1.DataItemList.Add(new Entity_Select("1", false, false, "Entity_Select field", "random description", DataItemColors.Blue, 1, eS));
                ////e1.DataItemList.Add(new Entity_Select("1", false, false, "Entity_Select field", "random description", DataItemColors.Blue, 1, alreadyKnownId));


                //TODO      e1.DataItemList.Add(new Choose_Entity("2", true, false, "Choose", "descrip", DataItemColors.Red, 2, "this is", true, "bla bla", 0, false, "unk", 1));


                e2.DataItemList.Add(new Comment("2", false, false, "Comment field", "this is a description", DataItemColors.Blue, 2, "Text", 10000, false));
                e2.DataItemList.Add(new Check_Box("1", true, false, "Check field", "this is a description", DataItemColors.Purple, 1, false, false));

                e3.DataItemList.Add(new Check_Box("1", true, false, "You are sure?", "Verify please", DataItemColors.Purple, 1, false, false));
                #endregion
                sampleXml = sample.ClassToXml();
            }
        }


        //TODO be removed, before release

        private void button1_Click(object sender, EventArgs e)
        {
            eForm ef = new eForm();
            UpdateXmlStr(0);
            string temp = ef.PostExtendedXml(txtApiId.Text, txtToken.Text, txtServerAddress.Text, sampleXml, txtOrganizationId.Text);
            tbxResponse.Text = temp;

            //-- .. --//

            //string xmlStr = tbxResponse.Text; 
            //Response resp = new Response();
            //resp = resp.XmlToClass(xmlStr);

            //string after = resp.ClassToXml();
            //after = after.Replace("<Latitude />", "<Latitude></Latitude>");
            //after = after.Replace("<Longitude />", "<Longitude></Longitude>");
            //after = after.Replace("<Altitude />", "<Altitude></Altitude>");
            //after = after.Replace("<Heading />", "<Heading></Heading>");
            //after = after.Replace("<Accuracy />", "<Accuracy></Accuracy>");
            //after = after.Replace("<Date />", "<Date></Date>");
            //after = after.Replace("<Value />", "<Value></Value>");
            //after = after.Replace("<ExtraDataItemList />", "<ExtraDataItemList></ExtraDataItemList>");

            //if (xmlStr.ToLower() == after.ToLower())
            //    MessageBox.Show("PARTY !");
            //else
            //    MessageBox.Show("Nope :(");
        }
    }
}