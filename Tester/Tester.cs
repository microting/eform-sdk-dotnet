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

using eFormCommunicator;
using eFormRequest;
using eFormResponse;
using eFormSubscriber;
using Trools;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace eFormTester
{
    public partial class Tester : Form
    {
        #region var
        Communicator communicator;
        Subscriber subscriber;
        bool subscriberAlive;
        string organizationId, serverToken, serverAddress, request, sampleXml, notificationToken, notificationAddress;
        int siteId;
        DateTime startDate, endDate;
        Tools t = new Tools();
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
                txtServerToken.Text = lines[2];
                txtServerAddress.Text = lines[3];
                txtNotificationToken.Text = lines[4];
                txtNotificationAddress.Text = lines[5];
            }
            catch (Exception)
            {
            }
            #endregion

            UpdateXmlStr(0);
            cbxDropDown.Items.Add("Sample 1 - Basic");
            cbxDropDown.Items.Add("Sample 2 - Extended");
            cbxDropDown.Items.Add("Sample 3 - Advanced");
            cbxDropDown.Items.Add("Sample 4 - Complex");

            subscriberAlive = false;

            AddToLog("");
            AddToLog("Tester started");
        }
        #endregion

        #region buttons
        // eForm Standard
        private void btnSendXml_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                //Sends the XML using the actual method, and get it's response XML
                string response = communicator.PostXml(request, siteId);
                //Sends XML, and gets a response

                tbxResponse.Text = response;
                AddToLog(response);
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        private void btnFetchId_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                //Sends the Id using the actual method, and get it's response XML
                string response = communicator.Retrieve(request, siteId);
                //Get the Id's data

                tbxResponse.Text = response;
                AddToLog(response);
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                //Sends the Id using the actual method, and get it's response XML
                string response = communicator.Delete(request, siteId);
                //Marks the Id to be deleted

                tbxResponse.Text = response;
                AddToLog(response);
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        // eForm Extended
        private void btnSendExtXml_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                //Sends the XML using the actual method, and get it's response XML. ONLY needed if complex XML elements are included (Entity_Select or Entity_Search). However is a bit slower than PostXml
                string response = communicator.PostXmlExtended(request, siteId, organizationId);
                //Sends XML, and gets a response

                tbxResponse.Text = response;
                AddToLog(response);
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        private void btnCheckId_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                //Sends the Id using the actual method, and get it's response XML
                string response = communicator.CheckStatus(request, siteId);
                //Get the Id's status

                tbxResponse.Text = response;
                AddToLog(response);
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        // Subscriber
        private void btnSub_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                if (!subscriberAlive)
                {
                    subscriber = new Subscriber(notificationToken, notificationAddress, "netApp");
                    subscriber.EventMsgServer += SubscriberEventMsgServer;
                    subscriber.EventMsgClient += SubscriberEventMsgClient;
                    SubscriberEventMsgClient("Subscriber now triggers events", null);
                    MessageBox.Show         ("Subscribed. Events can be found in the log", "Subscribed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    subscriber.Start();
                    subscriberAlive = true;
                }
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        private void btnUnsub_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                if (subscriberAlive)
                {
                    //reversed order of subscribing
                    subscriber.Close(true);
                    subscriberAlive = false;

                    subscriber.EventMsgServer -= SubscriberEventMsgServer;
                    subscriber.EventMsgClient -= SubscriberEventMsgClient;
                    SubscriberEventMsgClient("Subscriber no longer triggers events", null);
                    MessageBox.Show         ("Unsubscribed. Events can be found in the log", "Unsubscribed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region help buttons/drop down
        private void btnSendSample_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                //Ex. of a post with auto. gen. XML and it's response XML returned
                string response = communicator.PostXml(sampleXml, siteId);

                tbxResponse.Text = response;
                AddToLog(response);
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        private void btnCreateXml_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                //Auto. gen. XML
                tbxRequest.Text = sampleXml;
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
        }

        private void btnRetriveId_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            try
            {
                string response = tbxResponse.Text;

                if (response.Contains("Value type=\"success\">"))
                {
                    string idStr = t.Locate(response, "Value type=\"success\">", "<");

                    if (int.Parse(idStr) > 0)
                        tbxRequest.Text = idStr;
                }
            }
            catch (Exception ex) { HandleExpection(ex); }
            Cursor.Current = Cursors.Default;
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
            try
            {
                int selectedIndex = (int)cbxDropDown.SelectedIndex;
                UpdateXmlStr(selectedIndex);
            }
            catch (Exception ex) { HandleExpection(ex); }
        }
        #endregion

        #region private
        private void MapInput()
        {
            siteId = int.Parse(txtApiId.Text);
            organizationId = txtOrganizationId.Text;
            serverToken = txtServerToken.Text;
            serverAddress = txtServerAddress.Text;
            notificationToken = txtNotificationToken.Text;
            notificationAddress = txtNotificationAddress.Text;
            request = tbxRequest.Text;
            startDate = DateTime.Now;
            endDate = DateTime.Now.AddYears(1);

            communicator = new Communicator(serverToken, serverAddress);

            try
            {
                using (StreamWriter file = new StreamWriter("LatestInput.txt"))
                {
                    file.WriteLine(siteId);
                    file.WriteLine(organizationId);
                    file.WriteLine(serverToken);
                    file.WriteLine(serverAddress);
                    file.WriteLine(notificationToken);
                    file.WriteLine(notificationAddress);
                }
            }
            catch (Exception) { }
        }

        private void AddToLog(string logString)
        {
            try
            {
                File.AppendAllText(DateTime.Now.ToShortDateString() + "_Log.txt", DateTime.Now.ToLongTimeString() + " # " + logString + Environment.NewLine);
            }
            catch (Exception ex) { HandleExpection(ex); }
        }

        private void UpdateXmlStr(int sampleIndex)
        {
            if (sampleIndex == 0)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement(1, "Sample 1", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, "", "", "", new List<Element>());

                DataElement e1 = new DataElement(2, "Basic list", 1, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e1);

                e1.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", "e2f4fb", 1, 0, 1000, 2, 0, ""));
                e1.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", "e2f4fb", 8, "true", 100, false, false, true, false, ""));
                e1.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", "e2f4fb", 3, "value", 10000, false));
                e1.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", "e2f4fb", 4, 1, true));
                e1.DataItemList.Add(new Check_Box("5", false, true, "Check box", "this is a description", "e2f4fb", 15, true, true));
                e1.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", "e2f4fb", 16, startDate, startDate, startDate.ToString()));
                e1.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", "e2f4fb", 7));
                #endregion
                sampleXml = sample.ClassToXml();
            }

            if (sampleIndex == 1)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement(1, "Sample 2", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, "", "", "", new List<Element>());

                DataElement e1 = new DataElement(1, "Extended list", 1, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e1);

                DataElement e2 = new DataElement(2, "Extended list", 2, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e2);


                e1.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", "e2f4fb", 1, 0, 1000, 2, 0, ""));
                e1.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", "e2f4fb", 2, "true", 100, false, false, true, false, ""));
                e1.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", "e2f4fb", 3, "value", 10000, false));
                e1.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", "e2f4fb", 4, 1, true));
                e1.DataItemList.Add(new Check_Box("5", true, false, "Check box", "this is a description", "e2f4fb", 5, true, true));
                e1.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", "e2f4fb", 6, startDate, startDate, startDate.ToString()));
                e1.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", "e2f4fb", 7));
                e1.DataItemList.Add(new eFormRequest.Timer("8", false, false, "Timer", "this is a description", "e2f4fb", 13, false));
                e1.DataItemList.Add(new Signature("9", false, false, "Signature", "this is a description", "e2f4fb", 14));

                e2.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", "e2f4fb", 1, 0, 1000, 2, 0, ""));
                e2.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", "e2f4fb", 2, "true", 100, false, false, true, false, ""));
                e2.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", "e2f4fb", 3, "value", 10000, false));
                e2.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", "e2f4fb", 4, 1, true));
                e2.DataItemList.Add(new Check_Box("5", false, true, "Check box", "this is a description", "e2f4fb", 5, true, true));
                e2.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", "e2f4fb", 6, startDate, startDate, startDate.ToString()));
                e2.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", "e2f4fb", 7));
                e2.DataItemList.Add(new eFormRequest.Timer("8", false, false, "Timer", "this is a description", "e2f4fb", 8, false));
                e2.DataItemList.Add(new Signature("9", false, false, "Signature", "this is a description", "e2f4fb", 9));
                #endregion
                sampleXml = sample.ClassToXml();
            }

            if (sampleIndex == 2)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement(1, "Sample 3", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, "", "", "", new List<Element>());

                GroupElement g1 = new GroupElement(11, "Group of advanced check lists", 1, "Group element", false, false, false, false, "", new List<Element>());
                sample.ElementList.Add(g1);


                DataElement e1 = new DataElement(21, "Advanced list", 1, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                g1.ElementList.Add(e1);

                DataElement e2 = new DataElement(22, "Advanced list", 2, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                g1.ElementList.Add(e2);

                DataElement e3 = new DataElement(23, "Advanced list", 3, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                g1.ElementList.Add(e3);


                List<KeyValuePair> singleKeyValuePairList = new List<KeyValuePair>();
                singleKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
                singleKeyValuePairList.Add(new KeyValuePair("2", "option 2", false, "2"));
                singleKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

                List<KeyValuePair> multiKeyValuePairList = new List<KeyValuePair>();
                multiKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
                multiKeyValuePairList.Add(new KeyValuePair("2", "option 2", true, "2"));
                multiKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

                e1.DataItemList.Add(new Single_Select("1", false, false, "Single select field", "this is a description", "e2f4fb", 1, singleKeyValuePairList));
                e1.DataItemList.Add(new Multi_Select("2", false, false, "Multi select field", "this is a description", "e2f4fb", 2, multiKeyValuePairList));
                e1.DataItemList.Add(new Audio("3", false, false, "Audio field", "this is a description", "e2f4fb", 3, 1));
                e1.DataItemList.Add(new Show_Pdf("4", false, false, "PDF field", "this is a description", "e2f4fb", 4, @"http://www.analysis.im/uploads/seminar/pdf-sample.pdf"));
                e1.DataItemList.Add(new Comment("5", false, false, "Comment field", "this is a description", "e2f4fb", 5, "value", 10000, false));

                e2.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", "e2f4fb", 1, 0, 1000, 2, 0, ""));
                e2.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", "e2f4fb", 2, "true", 100, false, false, true, false, ""));
                e2.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", "e2f4fb", 3, "value", 10000, false));
                e2.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", "e2f4fb", 4, 1, true));
                e2.DataItemList.Add(new Check_Box("5", false, false, "Check box", "this is a description", "e2f4fb", 5, true, true));
                e2.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", "e2f4fb", 6, startDate, startDate, startDate.ToString()));
                e2.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", "e2f4fb", 7));
                e2.DataItemList.Add(new eFormRequest.Timer("8", false, false, "Timer", "this is a description", "e2f4fb", 8, false));
                e2.DataItemList.Add(new Signature("9", false, false, "Signature", "this is a description", "e2f4fb", 9));

                e3.DataItemList.Add(new Check_Box("1", true, false, "You are sure?", "Verify please", "e2f4fb", 1, false, false));
                #endregion
                sampleXml = sample.ClassToXml();
            }

            if (sampleIndex == 3)
            {
                #region population of sample
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddYears(1);
                MainElement sample = new MainElement(1, "Sample 4", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, "", "", "", new List<Element>());

                GroupElement g1 = new GroupElement(2, "Group of groups", 1, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g2 = new GroupElement(3, "Group of complex check lists 2", 2, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g3 = new GroupElement(4, "Group of complex check lists 3", 3, "Group element", false, false, false, false, "", new List<Element>());
                sample.ElementList.Add(g1);
                sample.ElementList.Add(g2);
                sample.ElementList.Add(g3);

                GroupElement g11 = new GroupElement(5, "Sub group of group 1, with check lists", 1, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g12 = new GroupElement(6, "Sub group of group 1, with sub groups", 2, "Group element", false, false, false, false, "", new List<Element>());
                g1.ElementList.Add(g11);
                g1.ElementList.Add(g12);

                GroupElement g121 = new GroupElement(7, "Sub sub group of group with check lists 1", 1, "Group element", false, false, false, false, "", new List<Element>());
                GroupElement g122 = new GroupElement(8, "Sub sub group of group with check lists 2", 2, "Group element", false, false, false, false, "", new List<Element>());
                g12.ElementList.Add(g121);
                g12.ElementList.Add(g122);



                DataElement e1 = new DataElement(9, "Complex list", 101, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e1);

                DataElement e2 = new DataElement(10, "Complex list", 2, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
                g2.ElementList.Add(e2);
                g3.ElementList.Add(e2);
                g11.ElementList.Add(e2);
                g121.ElementList.Add(e2);

                List<DataItemGroup> digLst = new List<DataItemGroup>();
                DataElement e3 = new DataElement(11, "Complex list", 1001, "Data element", true, true, true, false, "", digLst, new List<eFormRequest.DataItem>());
                sample.ElementList.Add(e3);
                g122.ElementList.Add(e3);

                List<eFormRequest.DataItem> dILst = new List<eFormRequest.DataItem>();
                digLst.Add(new FieldGroup("111", "digLabl", "this is a bad desciption", "e2f4fb", 0, "title of the field", dILst));
                dILst.Add(new Number("1", false, false, "Number field", "this is a description", "e2f4fb", 1, 0, 1000, 2, 0, ""));
                dILst.Add(new Text("2", false, false, "Text field", "this is a description bla", "e2f4fb", 2, "true", 100, false, false, true, false, ""));


                ////TODO - missing fields to be renamed server side.
                ////check update of list
                ////delete list
                //Entity e_1 = new Entity("iden", "decription 1", "km", "colour", "radioCode", "1");
                //Entity e_2 = new Entity("iden", "decription 2", "km", "colour", "radioCode", "2");
                //Entity e_3 = new Entity("iden", "decription 3", "km", "colour", "radioCode", "3");
                //List<Entity> lstE = new List<Entity>();
                //lstE.Add(e_1);
                //lstE.Add(e_2);
                //lstE.Add(e_3);

                //EntityType eS = new EntityType("name", "1001", lstE);

                //int alreadyKnownListId = 12878;
                //e1.DataItemList.Add(new Entity_Select("1", false, false, "Entity_Select field", "random description", "e2f4fb", 1, eS));
                //e1.DataItemList.Add(new Entity_Select("1", false, false, "Entity_Select field", "random description", "e2f4fb", 1, alreadyKnownListId));
                
                ////Entity_Search missing
                ////e1.DataItemList.Add(new Choose_Entity("2", true, false, "Choose", "description", DataItemColors.Red, 2, "this is", true, "bla bla", 0, false, "unk", 1));

                e2.DataItemList.Add(new Comment("2", false, false, "Comment field", "this is a description", "e2f4fb", 2, "Text", 10000, false));
                e2.DataItemList.Add(new Check_Box("1", true, false, "Check field", "this is a description", "e2f4fb", 1, false, false));

                e3.DataItemList.Add(new Check_Box("1", true, false, "You are sure?", "Verify please", "e2f4fb", 1, false, false));
                #endregion
                sampleXml = sample.ClassToXml();
            }
        }

        private void HandleExpection(Exception ex)
        {
            MessageBox.Show("Exception found." + Environment.NewLine +
                Environment.NewLine +
                "- Exception message :" + Environment.NewLine +
                ex.Message + Environment.NewLine +
                "- Exception source :" + Environment.NewLine +
                ex.Source + Environment.NewLine +
                "- Exception StackTrace :" + Environment.NewLine +
                ex.StackTrace,
                "Exception found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region Subscriber thread and events handler
        private void SubscriberEventMsgServer(object sender, EventArgs args)
        {
            AddToLog("Server # " + sender.ToString()); //Trace messages. For testing and tracking mainly. Can be removed.

            string reply = sender.ToString();
            if (reply.Contains("-update\",\"data") && reply.Contains("\"id\\\":"))
            {
                //Do something with the 'reply'
                //
                //
                //
                //done something with the 'reply'

                string nfId = t.Locate(reply, "\"id\\\":", ",");
                subscriber.ConfirmId(nfId);
            }
        }

        private void SubscriberEventMsgClient(object sender, EventArgs args)
        {
            AddToLog("Client # " + sender.ToString()); //Trace messages. For testing and tracking mainly. Can be removed.
        }
        #endregion
    }
}