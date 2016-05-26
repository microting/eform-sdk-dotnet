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

namespace eFormTester
{
    public partial class Form1 : Form
    {
        #region var
        private eFormMain main = new eFormMain();
        private string apiId, token, serverAddress, request;
        private DateTime startDate, endDate;
        #endregion

        #region con
        public Form1()
        {
            InitializeComponent();

            #region tries loads latest input
            try
            {
                string[] lines = File.ReadAllLines("LatestInput.txt");

                txtApi_Id.Text = lines[0];
                txtToken.Text = lines[1];
                txtServerAddress.Text = lines[2];
            }
            catch (Exception)
            {
            }
            #endregion
        }
        #endregion

        #region buttons
        private void btnSendXml_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Sends the XML using the actual method, and get it's response XML
            string response = main.Post(apiId, token, serverAddress, request);
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
            string response = main.RetrieveFirst(apiId, token, serverAddress, request);
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

        #region demo buttons
        private void btnSendSample_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Ex. of a post with auto. gen. XML and it's response XML returned
            string response = main._DemoPostSample(apiId, token, serverAddress);


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
            string xml = main._DemoCreateXml(apiId, token, serverAddress);


            tbxRequest.Text = xml;
            Cursor.Current = Cursors.Default;
        }

        private void btnCreateId_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MapInput();
            StoreLatestInput();


            //Auto. gen. Id
            string response = main._DemoPostSample(apiId, token, serverAddress);


            if (response.Contains("Value type=\"success\""))
            {
                int startPoint = response.IndexOf("Value type=\"success\"") + 21; //21 magic number of the lenght

                tbxRequest.Text = response.Substring(startPoint, response.IndexOf("<", startPoint) - startPoint);
            }
            else
            {
                tbxRequest.Text = "Failed to auto. gen. a Id";
            }
            Cursor.Current = Cursors.Default;
        }
        #endregion
        #endregion

        #region private
        private void MapInput()
        {
            apiId = txtApi_Id.Text;
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
    }
}
