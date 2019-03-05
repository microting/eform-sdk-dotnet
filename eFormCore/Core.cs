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
using eFormData;
using eFormShared;
using eFormSubscriber;
using eFormSqlController;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Xml;
//using OfficeOpenXml;
using Castle.Windsor;
using eFormCore.Installers;
using Castle.MicroKernel.Registration;
using Rebus.Bus;
using eForm.Messages;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microting.eForm;
using SwiftClient;

namespace eFormCore
{
    public class Core : CoreBase, ICore
    {
        #region events
        public event EventHandler HandleCaseCreated;
        public event EventHandler HandleCaseRetrived;
        public event EventHandler HandleCaseCompleted;
        public event EventHandler HandleCaseDeleted;
        public event EventHandler HandleFileDownloaded;
        public event EventHandler HandleSiteActivated;
        public event EventHandler HandleNotificationNotFound;
        public event EventHandler HandleEventException;
        #endregion

        #region var
        Subscriber subscriber;
        Communicator communicator;
        SqlController sqlController;
        Tools t = new Tools();

        IWindsorContainer container;

        public Log log;

        object _lockMain = new object();
        object _lockEventMessage = new object();

        bool updateIsRunningFiles = false;
        bool updateIsRunningNotifications = false;
        bool updateIsRunningEntities = false;

        bool coreThreadRunning = false;
        bool coreRestarting = false;
        bool coreStatChanging = false;
        bool coreAvailable = false;

        bool skipRestartDelay = false;

        string connectionString;
        string fileLocationPicture;
        string fileLocationPdf;

        int sameExceptionCountTried = 0;
        int maxParallelism = 1;
        int numberOfWorkers = 1;
        private bool _swiftEnabled = false;
        private string _swiftUserName = "";
        private string _swiftPassword = "";
        List<string> _swiftEndpoints = new List<string>();
        private Client _swiftClient;
        private string _comOrganizationId;
        IBus bus;
		#endregion

		//con

		#region public state
		public bool Start(string connectionString)
		{
			try
			{
				if (!coreAvailable && !coreStatChanging)
				{


					if (!StartSqlOnly(connectionString))
					{
						return false;
					}

                    try
                    {
                        maxParallelism = int.Parse(sqlController.SettingRead(Settings.maxParallelism));
                        numberOfWorkers = int.Parse(sqlController.SettingRead(Settings.numberOfWorkers));
                        _comOrganizationId = sqlController.SettingRead(Settings.comOrganizationId);
                    }
                    catch { }

				    

                    container.Install(
						new RebusHandlerInstaller()
						, new RebusInstaller(connectionString, maxParallelism, numberOfWorkers)
					);
					bus = container.Resolve<IBus>();
					log.LogCritical(t.GetMethodName("Core"), "called");

					//---

					coreStatChanging = true;

					//subscriber
					subscriber = new Subscriber(sqlController, log, bus);
					subscriber.Start();
					log.LogStandard(t.GetMethodName("Core"), "Subscriber started");

					log.LogCritical(t.GetMethodName("Core"), "started");
					coreAvailable = true;
					coreStatChanging = false;

                    //coreThread
                    //Thread coreThread = new Thread(() => CoreThread());
                    //coreThread.Start();
                    coreThreadRunning = true;

                    log.LogStandard(t.GetMethodName("Core"), "CoreThread started");
				}
			}
			#region catch
			catch (Exception ex)
			{
				FatalExpection(t.GetMethodName("Core") + " failed", ex);
				throw ex;
				//return false;
			}
			#endregion

			return true;
		}

        public bool StartSqlOnly(string connectionString)
        {

            try
            {
                if (!coreAvailable && !coreStatChanging)
                {
                    coreStatChanging = true;

                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

                    //sqlController
                    sqlController = new SqlController(connectionString);

                    //check settings
                    if (sqlController.SettingCheckAll().Count > 0)
                        throw new ArgumentException("Use AdminTool to setup database correctly. 'SettingCheckAll()' returned with errors");

                    if (sqlController.SettingRead(Settings.token) == "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
                        throw new ArgumentException("Use AdminTool to setup database correctly. Token not set, only default value found");

                    if (sqlController.SettingRead(Settings.firstRunDone) != "true")
                        throw new ArgumentException("Use AdminTool to setup database correctly. FirstRunDone has not completed");

                    if (sqlController.SettingRead(Settings.knownSitesDone) != "true")
                        throw new ArgumentException("Use AdminTool to setup database correctly. KnownSitesDone has not completed");

                    //log
                    if (log == null)
                        log = sqlController.StartLog(this);

                    log.LogCritical(t.GetMethodName("Core"), "###########################################################################");
                    log.LogCritical(t.GetMethodName("Core"), "called");
                    log.LogStandard(t.GetMethodName("Core"), "SqlController and Logger started");

                    //settings read
                    this.connectionString = connectionString;
                    fileLocationPicture = sqlController.SettingRead(Settings.fileLocationPicture);
                    fileLocationPdf = sqlController.SettingRead(Settings.fileLocationPdf);
                    log.LogStandard(t.GetMethodName("Core"), "Settings read");

                    //communicators
                    string token = sqlController.SettingRead(Settings.token);
                    string comAddressApi = sqlController.SettingRead(Settings.comAddressApi);
                    string comAddressBasic = sqlController.SettingRead(Settings.comAddressBasic);
                    string comOrganizationId = sqlController.SettingRead(Settings.comOrganizationId);
                    string ComAddressPdfUpload = sqlController.SettingRead(Settings.comAddressPdfUpload);
                    string ComSpeechToText = sqlController.SettingRead(Settings.comSpeechToText);
                    communicator = new Communicator(token, comAddressApi, comAddressBasic, comOrganizationId, ComAddressPdfUpload, log, ComSpeechToText);

                    container = new WindsorContainer();
                    container.Register(Component.For<SqlController>().Instance(sqlController));
                    container.Register(Component.For<Communicator>().Instance(communicator));
                    container.Register(Component.For<Log>().Instance(log));
                    container.Register(Component.For<Core>().Instance(this));

                    try
				    {
				        _swiftEnabled = (sqlController.SettingRead(Settings.swiftEnabled).ToLower() == "true");

				    } catch {}

				    if (_swiftEnabled)
				    {
				        _swiftUserName = sqlController.SettingRead(Settings.swiftUserName);
				        _swiftPassword = sqlController.SettingRead(Settings.swiftPassword);
				        foreach (var endpoint in sqlController.SettingRead(Settings.swiftEndPoints).Split(','))
				        {
				            _swiftEndpoints.Add(endpoint);
				        }
				        _swiftClient = new Client()
				            .WithCredentials(new SwiftCredentials
				            {
				                Username = _swiftUserName,
				                Password = _swiftPassword,
				                Endpoints = _swiftEndpoints
				            })
				            .SetRetryCount(6)
				            .SetRetryPerEndpointCount(2)
				            .SetLogger(new SwiftConsoleLog());
				        
				        container.Register(Component.For<Client>().Instance(_swiftClient));
				    }



                    log.LogStandard(t.GetMethodName("Core"), "Communicator started");

                    log.LogCritical(t.GetMethodName("Core"), "started");
                    coreAvailable = true;
                    coreStatChanging = false;
                }
            }
            #region catch
            catch (Exception ex)
            {
                coreThreadRunning = false;
                coreStatChanging = false;

                FatalExpection(t.GetMethodName("Core") + " failed", ex);
                return false;
            }
            #endregion

            return true;
        }

        public override void Restart(int sameExceptionCount, int sameExceptionCountMax)
        {
            try
            {
                if (coreRestarting == false)
                {
                    coreRestarting = true;
                    log.LogCritical(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(sameExceptionCount), sameExceptionCount);
                    log.LogVariable(t.GetMethodName("Core"), nameof(sameExceptionCountMax), sameExceptionCountMax);

                    sameExceptionCountTried++;

                    if (sameExceptionCountTried > sameExceptionCountMax)
                        sameExceptionCountTried = sameExceptionCountMax;

                    if (sameExceptionCountTried > 4)
                        throw new Exception("The same Exception repeated to many times (5+) within one hour");

                    int secondsDelay = 0;
                    switch (sameExceptionCountTried)
                    {
                        case 1: secondsDelay = 030; break;
                        case 2: secondsDelay = 060; break;
                        case 3: secondsDelay = 120; break;
                        case 4: secondsDelay = 512; break;
                        default: throw new ArgumentOutOfRangeException("sameExceptionCount should be above 0");
                    }
                    log.LogVariable(t.GetMethodName("Core"), nameof(sameExceptionCountTried), sameExceptionCountTried);
                    log.LogVariable(t.GetMethodName("Core"), nameof(secondsDelay), secondsDelay);

                    Close();

                    log.LogStandard(t.GetMethodName("Core"), "Trying to restart the Core in " + secondsDelay + " seconds");

                    if (!skipRestartDelay)
                        Thread.Sleep(secondsDelay * 1000);
                    else
                        log.LogStandard(t.GetMethodName("Core"), "Delay skipped");

                    Start(connectionString);
                    coreRestarting = false;
                }
            }
            catch (Exception ex)
            {
                FatalExpection(t.GetMethodName("Core") + "failed. Core failed to restart", ex);
            }
        }

        public bool Close()
        {
            log.LogStandard(t.GetMethodName("Core"), "Close called");
            try
            {
                if (coreAvailable && !coreStatChanging)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        coreStatChanging = true;

                        coreAvailable = false;
                        log.LogCritical(t.GetMethodName("Core"), "called");

                        try
                        {
                            if (subscriber != null)
                            {
                                log.LogEverything(t.GetMethodName("Core"), "Subscriber requested to close connection");
                                subscriber.Close();
                                log.LogEverything(t.GetMethodName("Core"), "Subscriber closed");
                                bus.Advanced.Workers.SetNumberOfWorkers(0);
                                bus.Dispose();
                                coreThreadRunning = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.LogException(t.GetMethodName("Core"), "Subscriber failed to close", ex, false);
                        }

                        int tries = 0;
                        while (coreThreadRunning)
                        {
                            Thread.Sleep(100);
                            tries++;

                            if (tries > 600)
                                FatalExpection("Failed to close Core correct after 60 secs", new Exception());
                        }

                        updateIsRunningFiles = false;
                        updateIsRunningNotifications = false;
                        updateIsRunningEntities = false;

                        log.LogStandard(t.GetMethodName("Core"), "Core closed");
                        subscriber = null;
                        communicator = null;
                        sqlController = null;
                        bus = null;

                        coreStatChanging = false;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                //"Even if you handle it, it will be automatically re-thrown by the CLR at the end of the try/catch/finally."
                Thread.ResetAbort(); //This ends the re-throwning
            }
            catch (Exception ex)
            {
                FatalExpection(t.GetMethodName("Core") + " failed. Core failed to close", ex);
            }
            return true;
        }

        public bool Running()
        {
            return coreAvailable;
        }

        public void FatalExpection(string reason, Exception exception)
        {
            coreAvailable = false;
            coreThreadRunning = false;
            coreStatChanging = false;
            coreRestarting = false;

            try
            {
                log.LogFatalException(t.GetMethodName("Core") + " called for reason:'" + reason + "'", exception);
            }
            catch { }

            try { HandleEventException?.Invoke(exception, EventArgs.Empty); } catch { }
            throw new Exception("FATAL exception, Core shutting down, due to:'" + reason + "'", exception);
        }
        #endregion

        #region public actions
        #region template
        public MainElement TemplateFromXml(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
                throw new ArgumentNullException("xmlString cannot be null or empty");
            string methodName = t.GetMethodName("Core");
            try
            {
                log.LogStandard(t.GetMethodName("Core"), "called");
                log.LogEverything(t.GetMethodName("Core"), "XML to transform:");
                log.LogEverything(t.GetMethodName("Core"), xmlString);

                //XML HACK TODO
                #region xmlString = corrected xml if needed
                xmlString = xmlString.Trim();
                //xmlString = xmlString.Replace("=\"choose_entity\">", "=\"EntitySearch\">");
                xmlString = xmlString.Replace("=\"single_select\">", "=\"SingleSelect\">");
                xmlString = xmlString.Replace("=\"multi_select\">", "=\"MultiSelect\">");
                xmlString = xmlString.Replace("xsi:type", "type");


                xmlString = t.ReplaceInsensitive(xmlString, "<main", "<Main");
                xmlString = t.ReplaceInsensitive(xmlString, "</main", "</Main");

                xmlString = t.ReplaceInsensitive(xmlString, "<element", "<Element");
                xmlString = t.ReplaceInsensitive(xmlString, "</element", "</Element");

                xmlString = t.ReplaceInsensitive(xmlString, "<dataItem", "<DataItem");
                xmlString = t.ReplaceInsensitive(xmlString, "</dataItem", "</DataItem");

                List<string> keyWords = new List<string>();
                keyWords.Add("GroupElement");
                keyWords.Add("DataElement");

                keyWords.Add("Audio");
                keyWords.Add("CheckBox");
                keyWords.Add("Comment");
                keyWords.Add("Date");
                keyWords.Add("EntitySearch");
                keyWords.Add("EntitySelect");
                keyWords.Add("None");
                keyWords.Add("Number");
                keyWords.Add("NumberStepper");
                keyWords.Add("MultiSelect");
                keyWords.Add("Picture");
                keyWords.Add("ShowPdf");
                keyWords.Add("SaveButton");
                keyWords.Add("Signature");
                keyWords.Add("SingleSelect");
                keyWords.Add("Text");
                keyWords.Add("Timer");

                foreach (var item in keyWords)
                    xmlString = t.ReplaceInsensitive(xmlString, "=\"" + item + "\">", "=\"" + item + "\">");

                xmlString = xmlString.Replace("<Main>", "<Main xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                xmlString = xmlString.Replace("<Element type=", "<Element xsi:type=");
                xmlString = xmlString.Replace("<DataItem type=", "<DataItem xsi:type=");
                //xmlString = xmlString.Replace("<DataItemGroup type=", "<DataItemGroup xsi:type=");
                xmlString = xmlString.Replace("FieldGroup", "FieldContainer");
                xmlString = xmlString.Replace("<DataItemGroup type=", "<DataItem xsi:type=");
                xmlString = xmlString.Replace("</DataItemGroup>", "</DataItem>");
                xmlString = xmlString.Replace("<DataItemGroupList>", "<DataItemList>");
                xmlString = xmlString.Replace("</DataItemGroupList>", "</DataItemList>");

                xmlString = xmlString.Replace("<FolderName>", "<CheckListFolderName>");
                xmlString = xmlString.Replace("</FolderName>", "</CheckListFolderName>");

                xmlString = xmlString.Replace("=\"ShowPDF\">", "=\"ShowPdf\">");
                xmlString = xmlString.Replace("='ShowPDF'>", "='ShowPdf'>");
                xmlString = xmlString.Replace("=\"choose_entity\">", "=\"EntitySearch\">");
                xmlString = xmlString.Replace("=\"Single_Select\">", "=\"SingleSelect\">");
                xmlString = xmlString.Replace("=\"Check_Box\">", "=\"CheckBox\">");
                xmlString = xmlString.Replace("=\"SingleSelectSearch\">", "=\"EntitySelect\">");

                string temp = t.Locate(xmlString, "<DoneButtonDisabled>", "</DoneButtonDisabled>");
                if (temp == "false")
                {
                    xmlString = xmlString.Replace("DoneButtonDisabled", "DoneButtonEnabled");
                    xmlString = xmlString.Replace("<DoneButtonEnabled>false", "<DoneButtonEnabled>true");
                }
                if (temp == "true")
                {
                    xmlString = xmlString.Replace("DoneButtonDisabled", "DoneButtonEnabled");
                    xmlString = xmlString.Replace("<DoneButtonEnabled>true", "<DoneButtonEnabled>false");
                }

                xmlString = xmlString.Replace("<MinValue />", "<MinValue>" + long.MinValue + "</MinValue>");
                xmlString = xmlString.Replace("<MinValue/>", "<MinValue>" + long.MinValue + "</MinValue>");
                xmlString = xmlString.Replace("<MaxValue />", "<MaxValue>" + long.MaxValue + "</MaxValue>");
                xmlString = xmlString.Replace("<MaxValue/>", "<MaxValue>" + long.MaxValue + "</MaxValue>");
                xmlString = xmlString.Replace("<MaxValue></MaxValue>", "<MaxValue>" + long.MaxValue + "</MaxValue>");
                xmlString = xmlString.Replace("<MinValue></MinValue>", "<MinValue>" + long.MinValue + "</MinValue>");
                xmlString = xmlString.Replace("<DecimalCount></DecimalCount>", "<DecimalCount>0</DecimalCount>");
                xmlString = xmlString.Replace("<DecimalCount />", "<DecimalCount>" + "0" + "</DecimalCount>");
                xmlString = xmlString.Replace("<DecimalCount/>", "<DecimalCount>" + "0" + "</DecimalCount>");
                xmlString = xmlString.Replace("<DisplayOrder></DisplayOrder>", "<DisplayOrder>" + "0" + "</DisplayOrder>");

                List<string> dILst = t.LocateList(xmlString, "type=\"Date\">", "</DataItem>");
                if (dILst != null)
                {
                    foreach (var item in dILst)
                    {
                        string before = item;
                        string after = item;
                        after = after.Replace("<Value />", "<DefaultValue/>");
                        after = after.Replace("<Value/>", "<DefaultValue/>");
                        after = after.Replace("<Value>", "<DefaultValue>");
                        after = after.Replace("</Value>", "</DefaultValue>");

                        xmlString = xmlString.Replace(before, after);
                    }
                }

//                xmlString = t.ReplaceAtLocationAll(xmlString, "<Id>", "</Id>", "1", false);
                xmlString = t.ReplaceInsensitive(xmlString, ">True<", ">true<");
                xmlString = t.ReplaceInsensitive(xmlString, ">False<", ">false<");
                #endregion

                log.LogEverything(t.GetMethodName("Core"), "XML after possible corrections:");
                log.LogEverything(t.GetMethodName("Core"), xmlString);

                MainElement mainElement = new MainElement();
                mainElement = mainElement.XmlToClass(xmlString);

                //XML HACK
                mainElement.CaseType = "";
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                if (mainElement.Repeated < 1)
                {
                    log.LogCritical(t.GetMethodName("Core"), "mainElement.Repeated = 1 // enforced");
                    mainElement.Repeated = 1;
                }

                return mainElement;
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        if (ex.InnerException.InnerException.InnerException != null)
                        {

                            throw new Exception("Could not parse XML, got error: " + ex.InnerException.InnerException.InnerException.Message, ex);
                        }
                        else
                        {
                            throw new Exception("Could not parse XML, got error: " + ex.InnerException.InnerException.Message, ex);
                        }
                    }
                    else
                    {
                        throw new Exception("Could not parse XML, got error: " + ex.InnerException.Message, ex);
                    }
                }
                else
                {
                    throw new Exception("Could not parse XML, got error: " + ex.Message, ex);
                }
            }
        }

        public List<string> TemplateValidation(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");

                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach (var dataItem in dataItems)
                    {
                        #region entities
                        if (dataItem.GetType() == typeof(EntitySearch))
                        {
                            EntitySearch entitySearch = (EntitySearch)dataItem;
                            var temp = sqlController.EntityGroupRead(entitySearch.EntityTypeId.ToString());
                            if (temp == null)
                                errorLst.Add("Element entitySearch.EntityTypeId:'" + entitySearch.EntityTypeId + "' is an reference to a local unknown EntitySearch group. Please update reference");
                        }

                        if (dataItem.GetType() == typeof(EntitySelect))
                        {
                            EntitySelect entitySelect = (EntitySelect)dataItem;
                            var temp = sqlController.EntityGroupRead(entitySelect.Source.ToString());
                            if (temp == null)
                                errorLst.Add("Element entitySelect.Source:'" + entitySelect.Source + "' is an reference to a local unknown EntitySearch group. Please update reference");
                        }
                        #endregion

                        #region PDF
                        if (dataItem.GetType() == typeof(ShowPdf))
                        {
                            ShowPdf showPdf = (ShowPdf)dataItem;
                            errorLst.AddRange(PdfValidate(showPdf.Value, showPdf.Id));
                        }
                        #endregion
                    }

                    return errorLst;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public MainElement TemplateUploadData(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");

                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach (var dataItem in dataItems)
                    {
                        #region PDF
                        if (dataItem.GetType() == typeof(ShowPdf))
                        {
                            ShowPdf showPdf = (ShowPdf)dataItem;

                            if (PdfValidate(showPdf.Value, showPdf.Id).Count != 0)
                            {
                                try
                                {
                                    //download file
                                    string downloadPath = sqlController.SettingRead(Settings.fileLocationPdf);
                                    try
                                    {
                                        (new FileInfo(downloadPath)).Directory.Create();

                                        using (WebClient client = new WebClient())
                                        {
                                            client.DownloadFile(showPdf.Value, downloadPath + "temp.pdf");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Download failed. Path:'" + showPdf.Value + "'", ex);
                                    }

                                    //upload file
                                    string hash = PdfUpload(downloadPath + "temp.pdf");
                                    if (hash != null)
                                    {
                                        //rename local file
                                        FileInfo FileInfo = new FileInfo(downloadPath + "temp.pdf");
                                        FileInfo.CopyTo(downloadPath + hash + ".pdf", true);
                                        FileInfo.Delete();

                                        showPdf.Value = hash;
                                    }
                                    else
                                    {
                                        throw new Exception("hash is null for field id:'" + showPdf.Id + "'");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("failed, for one PDF field id:'" + showPdf.Id + "'", ex);
                                }
                            }
                        }
                        #endregion
                    }

                    return mainElement;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public int TemplateCreate(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");

                    List<string> errors = TemplateValidation(mainElement);

                    if (errors == null) errors = new List<string>();
                    if (errors.Count > 0)
                        throw new Exception("mainElement failed TemplateValidation. Run TemplateValidation to see errors");

                    int templateId = sqlController.TemplateCreate(mainElement);
                    log.LogEverything(t.GetMethodName("Core"), "Template id:" + templateId.ToString() + " created in DB");
                    return templateId;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public MainElement TemplateRead(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return sqlController.TemplateRead(templateId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool TemplateDelete(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return sqlController.TemplateDelete(templateId);                    
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public Template_Dto TemplateItemRead(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return sqlController.TemplateItemRead(templateId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(t.GetMethodName("Core"), "(int " + templateId.ToString() + ") failed", ex, false);
                }
                catch
                {
                    log.LogException(t.GetMethodName("Core"), "(int templateId) failed", ex, false);
                }
                throw new Exception("failed", ex);
            }
        }

        public List<Template_Dto> TemplateItemReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(includeRemoved), includeRemoved);

                    return TemplateItemReadAll(includeRemoved, Constants.WorkflowStates.Created, "", true, "", new List<int>());
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(t.GetMethodName("Core"), "(bool " + includeRemoved.ToString() + ") failed", ex, false);
                }
                catch
                {
                    log.LogException(t.GetMethodName("Core"), "(bool includeRemoved) failed", ex, false);
                }
                throw new Exception("failed", ex);
            }
        }

        public List<Template_Dto> TemplateItemReadAll(bool includeRemoved, string siteWorkflowState, string searchKey, bool descendingSort, string sortParameter, List<int> tagIds)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(includeRemoved), includeRemoved);
                    log.LogVariable(t.GetMethodName("Core"), nameof(searchKey), searchKey);
                    log.LogVariable(t.GetMethodName("Core"), nameof(descendingSort), descendingSort);
                    log.LogVariable(t.GetMethodName("Core"), nameof(sortParameter), sortParameter);
                    log.LogVariable(t.GetMethodName("Core"), nameof(tagIds), tagIds.ToString());

                    return sqlController.TemplateItemReadAll(includeRemoved, siteWorkflowState, searchKey, descendingSort, sortParameter, tagIds);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool TemplateSetTags(int templateId, List<int> tagIds)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(tagIds), tagIds.ToString());

                    return sqlController.TemplateSetTags(templateId, tagIds);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region case

        /// <summary>
        /// This method will send the mainElement to the Microting API endpoint.
        /// </summary>
        /// <param name="mainElement">eForm to be deployed</param>
        /// <param name="caseUId">Optional own id</param>
        /// <param name="siteUid">API id of the site to deploy the eForm at</param>
        /// <returns>Microting API ID</returns>
        public string CaseCreate(MainElement mainElement, string caseUId, int siteUid)
        {
            List<int> siteUids = new List<int>();
            siteUids.Add(siteUid);
            List<string> lst = CaseCreate(mainElement, caseUId, siteUids, "");

            try
            {
                return lst[0];
            }
            catch
            {
                return null;
            }
        }

        public List<string> CaseCreate(MainElement mainElement, string caseUId, List<int> siteUids, string custom)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        log.LogStandard(t.GetMethodName("Core"), "called");
                        string siteIdsStr = string.Join(",", siteUids);
                        log.LogVariable(t.GetMethodName("Core"), nameof(caseUId), caseUId);
                        log.LogVariable(t.GetMethodName("Core"), nameof(siteIdsStr), siteIdsStr);
                        log.LogVariable(t.GetMethodName("Core"), nameof(custom), custom);

                        #region check input
                        DateTime start = DateTime.Parse(mainElement.StartDate.ToShortDateString());
                        DateTime end = DateTime.Parse(mainElement.EndDate.ToShortDateString());

                        if (end < DateTime.Now)
                        {
                            log.LogStandard(t.GetMethodName("Core"), "mainElement.EndDate is set to " + end);
                            throw new ArgumentException("mainElement.EndDate needs to be a future date");
                        }

                        if (end <= start)
                        {
                            log.LogStandard(t.GetMethodName("Core"), "mainElement.StartDat is set to " + start);
                            throw new ArgumentException("mainElement.StartDate needs to be at least the day, before the remove date (mainElement.EndDate)");
                        }

                        if (caseUId != "" && mainElement.Repeated != 1)
                            throw new ArgumentException("if caseUId can only be used for mainElement.Repeated == 1");
                        #endregion

                        //sending and getting a reply
                        List<string> lstMUId = new List<string>();

                        foreach (int siteUid in siteUids)
                        {
                            string mUId = SendXml(mainElement, siteUid);

                            if (mainElement.Repeated == 1)
                                sqlController.CaseCreate(mainElement.Id, siteUid, mUId, null, caseUId, custom, DateTime.Now);
                            else
                                sqlController.CheckListSitesCreate(mainElement.Id, siteUid, mUId);

                            Case_Dto cDto = sqlController.CaseReadByMUId(mUId);
                            //InteractionCaseUpdate(cDto);
                            try { HandleCaseCreated?.Invoke(cDto, EventArgs.Empty); }
                            catch { log.LogWarning(t.GetMethodName("Core"), "HandleCaseCreated event's external logic suffered an Expection"); }
                            log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been created");

                            lstMUId.Add(mUId);
                        }
                        return lstMUId;
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public string CaseCheck(string microtingUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);

                    Case_Dto cDto = CaseLookupMUId(microtingUId);
                    return communicator.CheckStatus(cDto.MicrotingUId, cDto.SiteUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public ReplyElement CaseRead(string microtingUId, string checkUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(checkUId), checkUId);

                    if (checkUId == null)
                        checkUId = "";

                    if (checkUId == "" || checkUId == "0")
                        checkUId = null;

                    cases aCase = sqlController.CaseReadFull(microtingUId, checkUId);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        log.LogWarning(t.GetMethodName("Core"), "No case found with MuuId:'" + microtingUId + "'");
                        return null;
                    }
                    #endregion

                    int id = aCase.id;
                    log.LogEverything(t.GetMethodName("Core"), "aCase.id:" + aCase.id.ToString() + ", found");

                    ReplyElement replyElement = sqlController.CheckRead(microtingUId, checkUId);
                    return replyElement;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public Case_Dto CaseReadByCaseId(int id)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(id), id);

                    return sqlController.CaseReadByCaseId(id);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public int? CaseReadFirstId(int? templateId, string workflowState)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    return sqlController.CaseReadFirstId(templateId, workflowState);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }
        
        public List<Case> CaseReadAll(int? templateId, DateTime? start, DateTime? end)
        {
            return CaseReadAll(templateId, start, end, Constants.WorkflowStates.NotRemoved, null);
        }

        public List<Case> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, string searchKey)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(start), start);
                    log.LogVariable(t.GetMethodName("Core"), nameof(end), end);
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    return CaseReadAll(templateId, start, end, workflowState, searchKey, false, null);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public List<Case> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, string searchKey, bool descendingSort, string sortParameter)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(start), start);
                    log.LogVariable(t.GetMethodName("Core"), nameof(end), end);
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    log.LogVariable(t.GetMethodName("Core"), nameof(descendingSort), descendingSort);
                    log.LogVariable(t.GetMethodName("Core"), nameof(sortParameter), sortParameter);

                    return sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey, descendingSort, sortParameter);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public CaseList CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, 
            string searchKey, bool descendingSort, string sortParameter, int pageIndex, int pageSize)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(start), start);
                    log.LogVariable(t.GetMethodName("Core"), nameof(end), end);
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    log.LogVariable(t.GetMethodName("Core"), nameof(descendingSort), descendingSort);
                    log.LogVariable(t.GetMethodName("Core"), nameof(sortParameter), sortParameter);
                    log.LogVariable(t.GetMethodName("Core"), nameof(pageIndex), pageIndex);
                    log.LogVariable(t.GetMethodName("Core"), nameof(pageSize), pageSize);
                    log.LogVariable(t.GetMethodName("Core"), nameof(searchKey), searchKey);

                    return sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey, descendingSort, sortParameter, pageIndex, pageSize);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public bool CaseUpdate(int caseId, List<string> newFieldValuePairLst, List<string> newCheckListValuePairLst)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);

                    if (newFieldValuePairLst == null)
                        newFieldValuePairLst = new List<string>();

                    if (newCheckListValuePairLst == null)
                        newCheckListValuePairLst = new List<string>();

                    int id = 0;
                    string value = "";

                    foreach (string str in newFieldValuePairLst)
                    {
                        id = int.Parse(t.SplitToList(str, 0, false));
                        value = t.SplitToList(str, 1, false);
                        sqlController.FieldValueUpdate(caseId, id, value);
                    }

                    foreach (string str in newCheckListValuePairLst)
                    {
                        id = int.Parse(t.SplitToList(str, 0, false));
                        value = t.SplitToList(str, 1, false);
                        sqlController.CheckListValueStatusUpdate(caseId, id, value);
                    }

                    return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return false;
            }
        }

        public bool CaseDelete(int templateId, int siteUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteUId), siteUId);

                    return CaseDelete(templateId, siteUId, "");
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(t.GetMethodName("Core"), "(int " + templateId.ToString() + ", int " + siteUId.ToString() + ") failed", ex, false);
                }
                catch
                {
                    log.LogException(t.GetMethodName("Core"), "(int templateId, int siteUId) failed", ex, false);
                }
                return false;
            }
        }

        public bool CaseDelete(int templateId, int siteUId, string workflowState)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteUId), siteUId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    List<string> errors = new List<string>();
                    foreach (string microtingUId in sqlController.CheckListSitesRead(templateId, siteUId, workflowState))
                    {
                        if (!CaseDelete(microtingUId))
                        {
                            string error = "Failed to delete case with microtingUId: " + microtingUId;
                            errors.Add(error);
                        }
                    }
                    if (errors.Count() > 0)
                    {
                        throw new Exception(String.Join("\n", errors));
                    }
                    else
                        return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(t.GetMethodName("Core"), "(int " + templateId.ToString() + ", int " + siteUId.ToString() + ", string " + workflowState + ") failed", ex, false);
                }
                catch
                {
                    log.LogException(t.GetMethodName("Core"), "(int templateId, int siteUId, string workflowState) failed", ex, false);
                }
                return false;
            }
        }

        public bool CaseDelete(string microtingUId)
        {
            //bus.SendLocal(new EformDeleteFromServer(microtingUId)).Wait();
            //bus.SendLocal(new EformCompleted(notificationUId, microtingUId)).Wait();

            //string microtingUId = message.MicrotringUUID;
            string methodName = t.GetMethodName("Core");

            //log.LogStandard(t.GetMethodName("Core"), "called");
            log.LogVariable(methodName, nameof(microtingUId), microtingUId);

            var cDto = sqlController.CaseReadByMUId(microtingUId);
            string xmlResponse = communicator.Delete(microtingUId, cDto.SiteUId);
            log.LogEverything(methodName, "XML response is 1218 : " + xmlResponse);
            Response resp = new Response();

            if (xmlResponse.Contains("Error occured: Contact Microting"))
            {
                log.LogEverything(methodName, "XML response is : " + xmlResponse);
                log.LogEverything("DELETE ERROR", "failed for microtingUId: " + microtingUId);
                return false;
            }

            if (xmlResponse.Contains("Error"))
            {
                try
                {
                    resp = resp.XmlToClass(xmlResponse);
                    log.LogException(methodName, "failed", new Exception("Error from Microting server: " + resp.Value), false);
                    return false;
                }
                catch (Exception ex)
                {
                    try
                    {
                        log.LogException(t.GetMethodName("Core"), "(string " + microtingUId + ") failed", ex, false);
                        throw ex;
                    }
                    catch
                    {
                        log.LogException(t.GetMethodName("Core"), "(string microtingUId) failed", ex, false);
                        throw ex;
                    }
                }
            }

            if (xmlResponse.Contains("Parsing in progress: Can not delete check list!"))
                for (int i = 1; i < 102; i++)
                {
                    Thread.Sleep(i * 5000);
                    xmlResponse = communicator.Delete(microtingUId, cDto.SiteUId);
                    try
                    {
                        resp = resp.XmlToClass(xmlResponse);
                        if (resp.Type.ToString() == "Success")
                        {
                            log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + $" has been removed from server in retry loop with i being : {i.ToString()}");
                            break;
                        }                            
                        else
                        {
                            log.LogEverything(t.GetMethodName("Core"), $"retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                        }
                    } catch (Exception ex)
                    {
                        log.LogEverything(t.GetMethodName("Core"), $" Exception is: {ex.Message}, retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                    }
                    //if (!xmlResponse.Contains("Parsing in progress: Can not delete check list!"))
                    //{
                    //    break;
                    //}
                    //else
                    //{
                    //    log.LogEverything(t.GetMethodName("Core"), $"retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                    //}
                }

            log.LogEverything(t.GetMethodName("Core"), "XML response:");
            log.LogEverything(t.GetMethodName("Core"), xmlResponse);

            resp = resp.XmlToClass(xmlResponse);
            if (resp.Type.ToString() == "Success")
            {
                log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been removed from server");
                try
                {
                    if (sqlController.CaseDelete(microtingUId))
                    {
                        cDto = sqlController.CaseReadByMUId(microtingUId);
                        FireHandleCaseDeleted(cDto);

                        log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been removed");

                        return true;
                    } else
                    {
                        try
                        {
                            sqlController.CaseDeleteReversed(microtingUId);

                            cDto = sqlController.CaseReadByMUId(microtingUId);
                            FireHandleCaseDeleted(cDto);
                            log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been removed");

                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.LogException(t.GetMethodName("Core"), "(string microtingUId) failed", ex, false);
                            throw ex;
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    log.LogException(t.GetMethodName("Core"), "(string microtingUId) failed", ex, false);
                }

                
            }
            return false;
            //return true;
        }

        public bool CaseDeleteResult(int caseId)
        {
            string methodName = t.GetMethodName("Core");
            log.LogStandard(t.GetMethodName("Core"), "called");
            log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);
            try
            {
                return sqlController.CaseDeleteResult(caseId);
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(t.GetMethodName("Core"), "(int " + caseId.ToString() + ") failed", ex, false);
                }
                catch
                {
                    log.LogException(t.GetMethodName("Core"), "(int caseId) failed", ex, false);
                }

                return false;
            }
        }

        public bool CaseUpdateFieldValues(int id)
        {
            string methodName = t.GetMethodName("Core");
            log.LogStandard(t.GetMethodName("Core"), "called");
            log.LogVariable(t.GetMethodName("Core"), nameof(id), id);
            try
            {
                return sqlController.CaseUpdateFieldValues(id);
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return false;
            }
        }

        public Case_Dto CaseLookupMUId(string microtingUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);

                    return sqlController.CaseReadByMUId(microtingUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public Case_Dto CaseLookupCaseId(int caseId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);

                    return sqlController.CaseReadByCaseId(caseId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public List<Case_Dto> CaseLookupCaseUId(string caseUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(caseUId), caseUId);

                    return sqlController.CaseReadByCaseUId(caseUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public int? CaseIdLookup(string microtingUId, string checkUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(checkUId), checkUId);

                    if (checkUId == null)
                        checkUId = "";

                    if (checkUId == "" || checkUId == "0")
                        checkUId = null;

                    cases aCase = sqlController.CaseReadFull(microtingUId, checkUId);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        log.LogWarning(t.GetMethodName("Core"), "No case found with MuuId:'" + microtingUId + "'");
                        return -1;
                    }
                    #endregion
                    int id = aCase.id;
                    log.LogEverything(t.GetMethodName("Core"), "aCase.id:" + aCase.id.ToString() + ", found");

                    return id;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public string CasesToCsv(int templateId, DateTime? start, DateTime? end, string pathAndName,
            string customPathForUploadedData, string decimalSeparator, string thousandSeparator)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(start), start.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(end), end.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(pathAndName), pathAndName);
                    log.LogVariable(t.GetMethodName("Core"), nameof(customPathForUploadedData), customPathForUploadedData);

                    List<List<string>> dataSet = GenerateDataSetFromCases(templateId, start, end, customPathForUploadedData, decimalSeparator, thousandSeparator);

                    if (dataSet == null)
                        return "";

                    List<string> temp;
                    string text = "";

                    for (int rowN = 0; rowN < dataSet[0].Count; rowN++)
                    {
                        temp = new List<string>();

                        foreach (List<string> lst in dataSet)
                        {
                            try
                            {
                                int.Parse(lst[rowN]);
                                temp.Add(lst[rowN]);
                            }
                            catch
                            {
                                try
                                {
                                    DateTime.Parse(lst[rowN]);
                                    temp.Add(lst[rowN]);
                                }
                                catch
                                {
                                    try
                                    {
                                        temp.Add("\"" + lst[rowN] + "\"");
                                    }
                                    catch (Exception ex2)
                                    {
                                        temp.Add(ex2.Message);
                                    }
                                }
                            }
                        }

                        text += string.Join(";", temp.ToArray()) + Environment.NewLine;
                    }

                    if (!pathAndName.Contains(".csv"))
                        pathAndName = pathAndName + ".csv";

                    File.WriteAllText(pathAndName, text.Trim(), Encoding.UTF8);
                    return Path.GetFullPath(pathAndName);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public string CasesToCsv(int templateId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData)
        {
            return CasesToCsv(templateId, start, end, pathAndName, customPathForUploadedData, ".", "");
        }

        public string CaseToJasperXml(int caseId, string timeStamp, string customPathForUploadedData)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                //if (coreRunning)
                if (true)
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(timeStamp), timeStamp);

                    if (timeStamp == null)
                        timeStamp = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss");

                    //get needed data
                    Case_Dto cDto = CaseLookupCaseId(caseId);
                    ReplyElement reply = CaseRead(cDto.MicrotingUId, cDto.CheckUId);
                    if (reply == null)
                        throw new NullReferenceException("reply is null. Delete or fix the case with ID " + caseId.ToString());
                    string clsLst = "";
                    string fldLst = "";
                    GetChecksAndFields(ref clsLst, ref fldLst, reply.ElementList, customPathForUploadedData);
                    log.LogVariable(t.GetMethodName("Core"), nameof(clsLst), clsLst);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fldLst), fldLst);

                    #region convert to jasperXml
                    string jasperXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + Environment.NewLine + "<root>"
                        + Environment.NewLine + "<C" + reply.Id + " case_id=\"" + caseId + "\" case_name=\"" + reply.Label + "\" serial_number=\"" + caseId + "/" + cDto.MicrotingUId + "\" check_list_status=\"approved\">"
                        + Environment.NewLine + "<worker>" + Advanced_WorkerNameRead(reply.DoneById) + "</worker>"
                        + Environment.NewLine + "<check_id>" + reply.MicrotingUId + "</check_id>"
                        + Environment.NewLine + "<date>" + reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss") + "</date>"
                        + Environment.NewLine + "<check_date>" + reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss") + "</check_date>"
                        + Environment.NewLine + "<site_name>" + Advanced_SiteItemRead(reply.SiteMicrotingUUID).SiteName + "</site_name>"
                        + Environment.NewLine + "<check_lists>"

                        + clsLst

                        + Environment.NewLine + "</check_lists>"
                        + Environment.NewLine + "<fields>"

                        + fldLst

                        + Environment.NewLine + "</fields>"
                        + Environment.NewLine + "</C" + reply.Id + ">"
                        + Environment.NewLine + "</root>";
                    log.LogVariable(t.GetMethodName("Core"), nameof(jasperXml), jasperXml);
                    #endregion

                    //place in settings allocated placement
                    string fullPath = sqlController.SettingRead(Settings.fileLocationJasper) + @"results\" + timeStamp + "_" + caseId + ".xml";
                    //string path = sqlController.SettingRead(Settings.fileLocationJasper) + "results/" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss") + "_" + caseId + ".xml";
                    string path = sqlController.SettingRead(Settings.fileLocationJasper);
                    Directory.CreateDirectory(path + @"results\");
                    File.WriteAllText(fullPath, jasperXml.Trim(), Encoding.UTF8);

                    //string path = Path.GetFullPath(locaR);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fullPath), fullPath);
                    return fullPath;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        #region settings
//        #region httpServerAddress
//        public string GetHttpServerAddress()
//        {
//            string methodName = t.GetMethodName("Core");
//            log.LogStandard(t.GetMethodName("Core"), "called");
//            try
//            {
//                return sqlController.SettingRead(Settings.httpServerAddress);
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                return "N/A";
//            }
//        }
//
//        public bool SetHttpServerAddress(string serverAddress)
//        {
//            string methodName = t.GetMethodName("Core");
//            try
//            {
//                if (Running())
//                {
//                    log.LogStandard(t.GetMethodName("Core"), "called");
//                    log.LogVariable(t.GetMethodName("Core"), nameof(serverAddress), serverAddress);
//
//                    sqlController.SettingUpdate(Settings.httpServerAddress, serverAddress);
//                    return true;
//                }
//                else
//                    throw new Exception("Core is not running");
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                throw new Exception("failed", ex);
//            }
//        }
//        #endregion
//
//        #region fileLocationPicture
//        public string GetPicturePath()
//        {
//            string methodName = t.GetMethodName("Core");
//            log.LogStandard(t.GetMethodName("Core"), "called");
//            try
//            {
//                return sqlController.SettingRead(Settings.fileLocationPicture);
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                return "N/A";
//            }
//        }
//
//        public bool SetPicturePath(string fileLocationPicture)
//        {
//            string methodName = t.GetMethodName("Core");
//            try
//            {
//                if (Running())
//                {
//                    log.LogStandard(t.GetMethodName("Core"), "called");
//                    log.LogVariable(t.GetMethodName("Core"), nameof(fileLocationPicture), fileLocationPicture);
//
//                    sqlController.SettingUpdate(Settings.fileLocationPicture, fileLocationPicture);
//                    return true;
//                }
//                else
//                    throw new Exception("Core is not running");
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                throw new Exception("failed", ex);
//            }
//        }
//        #endregion
//
//        #region fileLocationPdf
//        public string GetPdfPath()
//        {
//            string methodName = t.GetMethodName("Core");
//            log.LogStandard(t.GetMethodName("Core"), "called");
//            try
//            {
//                return sqlController.SettingRead(Settings.fileLocationPdf);
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                return "N/A";
//            }
//        }
//
//        public bool SetPdfPath(string fileLocationPdf)
//        {
//            string methodName = t.GetMethodName("Core");
//            try
//            {
//                if (Running())
//                {
//                    log.LogStandard(t.GetMethodName("Core"), "called");
//                    log.LogVariable(t.GetMethodName("Core"), nameof(fileLocationPdf), fileLocationPdf);
//
//                    sqlController.SettingUpdate(Settings.fileLocationPdf, fileLocationPdf);
//                    return true;
//                }
//                else
//                    throw new Exception("Core is not running");
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                throw new Exception("failed", ex);
//            }
//        }
//        #endregion
//
//        #region fileLocationJasper
//        public string GetJasperPath()
//        {
//            string methodName = t.GetMethodName("Core");
//            log.LogStandard(t.GetMethodName("Core"), "called");
//            try
//            {
//                return sqlController.SettingRead(Settings.fileLocationJasper);
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                return "N/A";
//            }
//        }
//
//        public bool SetJasperPath(string fileLocationJasper)
//        {
//            string methodName = t.GetMethodName("Core");
//            try
//            {
//                if (Running())
//                {
//                    log.LogStandard(t.GetMethodName("Core"), "called");
//                    log.LogVariable(t.GetMethodName("Core"), nameof(fileLocationJasper), fileLocationJasper);
//
//                    sqlController.SettingUpdate(Settings.fileLocationJasper, fileLocationJasper);
//                    return true;
//                }
//                else
//                    throw new Exception("Core is not running");
//            }
//            catch (Exception ex)
//            {
//                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//                throw new Exception("failed", ex);
//            }
//        }
//        #endregion
//        
        #endregion
        #region sdkSettings
        
        public string GetSdkSetting(Settings settingName)
        {
            string methodName = t.GetMethodName("Core");
            log.LogStandard(t.GetMethodName("Core"), "called");
            try
            {
                return sqlController.SettingRead(settingName);
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return "N/A";
            }
        }

        public bool SetSdkSetting(Settings settingName, string settingValue)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(settingValue), settingValue);

                    sqlController.SettingUpdate(settingName, settingValue);
                    return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        public string CaseToPdf(int caseId, string jasperTemplate, string timeStamp, string customPathForUploadedData)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                //if (coreRunning)
                if (true)
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(jasperTemplate), jasperTemplate);

                    if (timeStamp == null)
                        timeStamp = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss");

                    CaseToJasperXml(caseId, timeStamp, customPathForUploadedData);

                    #region run jar
                    // Start the child process.
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    // Redirect the output stream of the child process.
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    string locaJ;
                    string exepath = AppDomain.CurrentDomain.BaseDirectory;
                    log.LogStandard(t.GetMethodName("Core"), "exepath is " + exepath);
                    if (File.Exists(exepath + "\\bin\\JasperExporter.jar"))
                    {
                        locaJ = exepath + "\\bin\\JasperExporter.jar";
                    }
                    else
                    {
                        if(File.Exists(exepath + "\\JasperExpoter.jar"))
                        {
                            locaJ = exepath + "\\JasperExporter.jar";
                        }
                        else
                        {
                            locaJ = sqlController.SettingRead(Settings.fileLocationJasper) + "utils\\JasperExporter.jar";
                        }                        
                    }

                    string locaT = sqlController.SettingRead(Settings.fileLocationJasper) + "templates\\" + jasperTemplate + "\\compact\\" + jasperTemplate + ".jrxml";
                    if (!File.Exists(locaT))
                    {
                        throw new FileNotFoundException("jrxml template was not found at " + locaT);
                    }
                    string locaC = sqlController.SettingRead(Settings.fileLocationJasper) + "results\\" + timeStamp + "_" + caseId + ".xml";
                    locaC = locaC.Replace("\\", "/");

                    if (!File.Exists(locaC))
                    {
                        throw new FileNotFoundException("Case result xml was not found at " + locaC);
                    }
                    string locaR = sqlController.SettingRead(Settings.fileLocationJasper) + "results\\" + timeStamp + "_" + caseId + ".pdf";

                    string command =
                        "-d64 -Xms512m -Xmx2g -Dfile.encoding=UTF-8 -jar " + locaJ +
                        " -template=\"" + locaT + "\"" +
                        " -type=\"pdf\"" +
                        " -uri=\"" + locaC + "\"" +
                        " -outputFile=\"" + locaR + "\"";

                    log.LogVariable(t.GetMethodName("Core"), nameof(command), command);
                    p.StartInfo.FileName = "java.exe";
                    p.StartInfo.Arguments = command;
                    p.StartInfo.Verb = "runas";
                    p.Start();
                    // IF needed:
                    // Do not wait for the child process to exit before
                    // reading to the end of its redirected stream.
                    // p.WaitForExit();
                    // Read the output stream first and then wait.
                    string output = p.StandardOutput.ReadToEnd();
                    log.LogVariable(t.GetMethodName("Core"), nameof(output), output);
                    p.WaitForExit();

                    if (output != "")
                        throw new Exception("output='" + output + "', expected to be no output. This indicates an error has happened");
                    #endregion

                    //return path
                    string path = Path.GetFullPath(locaR);
                    log.LogVariable(t.GetMethodName("Core"), nameof(path), path);
                    return path;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }
        #endregion

        #region site
        public Site_Dto SiteCreate(string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(name), name);
                    log.LogVariable(t.GetMethodName("Core"), nameof(userFirstName), userFirstName);
                    log.LogVariable(t.GetMethodName("Core"), nameof(userLastName), userLastName);
                    log.LogVariable(t.GetMethodName("Core"), nameof(userEmail), userEmail);

                    Tuple<Site_Dto, Unit_Dto> siteResult = communicator.SiteCreate(name);

                    string token = sqlController.SettingRead(Settings.token);
                    int customerNo = communicator.OrganizationLoadAllFromRemote(token).CustomerNo;

                    string siteName = siteResult.Item1.SiteName;
                    int siteId = siteResult.Item1.SiteId;
                    int unitUId = siteResult.Item2.UnitUId;
                    int otpCode = siteResult.Item2.OtpCode;
                    SiteName_Dto siteDto = sqlController.SiteRead(siteResult.Item1.SiteId);
                    if (siteDto == null)
                    {
                        sqlController.SiteCreate((int)siteId, siteName);
                    }
                    siteDto = sqlController.SiteRead(siteId);
                    Unit_Dto unitDto = sqlController.UnitRead(unitUId);
                    if (unitDto == null)
                    {
                        sqlController.UnitCreate(unitUId, customerNo, otpCode, siteDto.SiteUId);
                    }

                    if (string.IsNullOrEmpty(userEmail))
                    {
                        Random rdn = new Random();
                        userEmail = siteId + "." + customerNo + "@invalid.invalid";
                    }

                    Worker_Dto workerDto = Advanced_WorkerCreate(userFirstName, userLastName, userEmail);
                    Advanced_SiteWorkerCreate(siteDto, workerDto);

                    return SiteRead(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <returns></returns>
        // TODO: Refactor to DeviceUserRead(int siteMicrotingUUID)
        public Site_Dto SiteRead(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    return sqlController.SiteReadSimple(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public List<Site_Dto> SiteReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    if (includeRemoved)
                        return Advanced_SiteReadAll(null, null, null);
                    else
                        return Advanced_SiteReadAll(Constants.WorkflowStates.NotRemoved, null, null);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public Site_Dto SiteReset(int siteId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId);

                    Site_Dto site = SiteRead(siteId);
                    Advanced_UnitRequestOtp((int)site.UnitId);

                    return SiteRead(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteMicrotingUid"></param>
        /// <param name="siteName"></param>
        /// <param name="userFirstName"></param>
        /// <param name="userLastName"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        // TODO Refactor to be named DeviceUserUpdate(int siteMicrotingUid, string siteName, string userFirstName, string userLastName, string userEmail)
        public bool SiteUpdate(int siteMicrotingUid, string siteName, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    Site_Dto siteDto = SiteRead(siteMicrotingUid);
                    Advanced_SiteItemUpdate(siteMicrotingUid, siteName);
                    if (String.IsNullOrEmpty(userEmail))
                    {
                        //if (String.IsNullOrEmpty)
                    }
                    Advanced_WorkerUpdate((int)siteDto.WorkerUid, userFirstName, userLastName, userEmail);
                    return true;
                }
                else
                    throw new Exception("Core is not running");

            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// This method deletes a device user.
        /// This includes deleting: site, unit, worker and site_worker
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <returns>true/false if the process went well</returns>
        // TODO: Refactor to DeviceUserDelete(int siteMicrotingUUID)
        public bool SiteDelete(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    Site_Dto siteDto = SiteRead(microtingUid);

                    if (siteDto != null)
                    {
                        Advanced_SiteItemDelete(microtingUid);
                        Site_Worker_Dto siteWorkerDto = Advanced_SiteWorkerRead(null, microtingUid, siteDto.WorkerUid);
                        Advanced_SiteWorkerDelete(siteWorkerDto.MicrotingUId);
                        Advanced_WorkerDelete((int)siteDto.WorkerUid);
                        return true;
                    } else
                    {
                        return false;
                    }

                    
                }
                else
                    throw new Exception("Core is not running");

            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region entity
        public EntityGroup EntityGroupCreate(string entityType, string name)
        {
            try
            {
                if (Running())
                {
                    EntityGroup entityGroup = sqlController.EntityGroupCreate(name, entityType);

                    string entityGroupMUId = communicator.EntityGroupCreate(entityType, name, entityGroup.Id.ToString());

                    bool isCreated = sqlController.EntityGroupUpdate(entityGroup.Id, entityGroupMUId);

                    if (isCreated)
                        return new EntityGroup(entityGroup.Id, entityGroup.Name, entityGroup.Type, entityGroupMUId, new List<EntityItem>());
                    else
                    {
                        sqlController.EntityGroupDelete(entityGroupMUId);
                        throw new Exception("EntityListCreate failed, due to list not created correct");
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "EntityListCreate failed", ex, false);
                throw new Exception("EntityListCreate failed", ex);
            }
        }

        public EntityGroup EntityGroupRead(string entityGroupMUId)
        {
            if (string.IsNullOrEmpty(entityGroupMUId))
                throw new ArgumentNullException("entityGroupMUId cannot be null or empty");
            return EntityGroupRead(entityGroupMUId, Constants.EntityItemSortParameters.Name, "");
        }

        public EntityGroup EntityGroupRead(string entityGroupMUId, string sort, string nameFilter)
        {
            string methodName = t.GetMethodName("Core");
            if (string.IsNullOrEmpty(entityGroupMUId))
                throw new ArgumentNullException("entityGroupMUId cannot be null or empty");
            try
            {
                if (Running())
                {
                    while (updateIsRunningEntities)
                        Thread.Sleep(200);

                    return sqlController.EntityGroupReadSorted(entityGroupMUId, sort, nameFilter);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(t.GetMethodName("Core"), "(string entityGroupMUId " + entityGroupMUId + ", string sort " + sort + ", string nameFilter " + nameFilter + ") failed", ex, false);
                }
                catch
                {
                    log.LogException(t.GetMethodName("Core"), "(string entityGroupMUId, string sort, string nameFilter) failed", ex, false);
                }
                throw new Exception("failed", ex);

            }
        }

        public bool EntityGroupUpdate(EntityGroup entityGroup)
        {
            try
            {
                if (Running())
                {
                    List<string> ids = new List<string>();
                    foreach (var item in entityGroup.EntityGroupItemLst)
                        ids.Add(item.EntityItemUId);

                    if (ids.Count != ids.Distinct().Count())
                        throw new Exception("List entityGroup.entityItemUIds are not all unique"); // Duplicates exist

                    while (updateIsRunningEntities)
                        Thread.Sleep(200);

                    bool isUpdated = communicator.EntityGroupUpdate(entityGroup.Type, entityGroup.Name, entityGroup.Id, entityGroup.MicrotingUUID);

                    if (isUpdated)
                        sqlController.EntityGroupUpdateName(entityGroup.Name, entityGroup.MicrotingUUID);

                    //sqlController.EntityGroupUpdateItems(entityGroup);

                    //CoreHandleUpdateEntityItems();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "EntityGroupRead failed", ex, false);
                throw new Exception("EntityGroupRead failed", ex);
            }
            return true;
        }

        public bool EntityGroupDelete(string entityGroupMUId)
        {
            try
            {
                if (Running())
                {
                    while (updateIsRunningEntities)
                        Thread.Sleep(200);

                    string type = sqlController.EntityGroupDelete(entityGroupMUId);

                    if (type != null)
                        communicator.EntityGroupDelete(type, entityGroupMUId);

                    //CoreHandleUpdateEntityItems();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "EntityGroupDelete failed", ex, false);
                throw new Exception("EntityGroupDelete failed", ex);
            }
            return true;
        }

        #region EntityItem

        public EntityItem EntitySearchItemCreate(int entitItemGroupId, string name, string description, string ownUUID) {
            return EntityItemCreate(entitItemGroupId, name, description, ownUUID, 0);
        }

        public EntityItem EntitySelectItemCreate(int entitItemGroupId, string name, int displayIndex, string ownUUID) {
            return EntityItemCreate(entitItemGroupId, name, "", ownUUID, displayIndex);
        }

        private EntityItem EntityItemCreate(int entitItemGroupId, string name, string description, string ownUUID, int displayIndex)
        {
            EntityGroup eg = sqlController.EntityGroupRead(entitItemGroupId);
            EntityItem et = sqlController.EntityItemRead(entitItemGroupId, name, description);
            if (et == null) {
                string microtingUId;
                if (eg.Type == Constants.FieldTypes.EntitySearch) {
                    microtingUId = communicator.EntitySearchItemCreate(eg.MicrotingUUID, name, description, ownUUID);
                } else {
                    microtingUId = communicator.EntitySelectItemCreate(eg.MicrotingUUID, name, displayIndex, ownUUID);
                }

                if (microtingUId != null) {
                    et = new EntityItem(name, description, ownUUID,Constants.WorkflowStates.Created, microtingUId, displayIndex);
                    return sqlController.EntityItemCreate(eg.Id, et);
                } else
                {
                    return null;
                }
            } else {
                if (et.WorkflowState == Constants.WorkflowStates.Removed)
                {
                    et.WorkflowState = Constants.WorkflowStates.Created;
                    sqlController.EntityItemUpdate(et);
                }
                return et;
            }
        }

        public void EntityItemUpdate(int id, string name, string description, string ownUUID, int displayIndex)
        {
            EntityItem et = sqlController.EntityItemRead(id);
            if (et == null) {
                throw new NullReferenceException("EntityItem not found with id " + id.ToString());
            } else {
                if (et.Name != name || et.Description != description || et.DisplayIndex != displayIndex)
                {
                    EntityGroup eg = sqlController.EntityGroupRead(et.EntityItemGroupId);
                    bool result = false;
                    if (eg.Type == Constants.FieldTypes.EntitySearch)
                    {
                        result = communicator.EntitySearchItemUpdate(eg.MicrotingUUID, et.MicrotingUUID, name, description, ownUUID);
                    } else {
                        result = communicator.EntitySelectItemUpdate(eg.MicrotingUUID, et.MicrotingUUID, name, displayIndex, ownUUID);
                    }
                    if (result) {
                        et.DisplayIndex = displayIndex;
                        et.Name = name;
                        et.Description = description;

                        sqlController.EntityItemUpdate(et);
                    } else {
                        throw new Exception("Unable to update entityItem with id " + id.ToString());
                    }
                }
            }
        }

        public void EntityItemDelete(int id)
        {
            EntityItem et = sqlController.EntityItemRead(id);
            if (et == null)
            {
                throw new NullReferenceException("EntityItem not found with id " + id.ToString());
            }
            else
            {
                EntityGroup eg = sqlController.EntityGroupRead(et.EntityItemGroupId);
                bool result = false;
                if (eg.Type == Constants.FieldTypes.EntitySearch) {
                    result = communicator.EntitySearchItemDelete(et.MicrotingUUID);
                } else {
                    result = communicator.EntitySelectItemDelete(et.MicrotingUUID);
                }
                if (result) {
                    sqlController.EntityItemDelete(id);
                }
                else
                {
                    throw new Exception("Unable to update entityItem with id " + id.ToString());
                }
            }
        }

        #endregion

        public string PdfUpload(string localPath)
        {
            try
            {
                if (Running())
                {
                    string chechSum = "";
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(localPath))
                        {
                            byte[] grr = md5.ComputeHash(stream);
                            chechSum = BitConverter.ToString(grr).Replace("-", "").ToLower();
                        }
                    }

                    if (communicator.PdfUpload(localPath, chechSum))
                        return chechSum;
                    else
                    {
                        log.LogWarning(t.GetMethodName("Core"), "Uploading of PDF failed");
                        return null;
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception(t.GetMethodName("Core") + " failed", ex);
            }
        }
        #endregion
        #endregion

        #region tags
        public List<Tag> GetAllTags(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return sqlController.GetAllTags(includeRemoved);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// This method will create a tag, which is globally accessible.
        /// </summary>
        /// <param name="name">Name of the tag, which is not allowed to be null or empty.</param>
        /// <returns></returns>
        public int TagCreate(string name)
        {
            string methodName = t.GetMethodName("Core");
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name is not allowed to be null or empty");
            }
            try
            {
                if (Running())
                {
                    return sqlController.TagCreate(name);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }



        public bool TagDelete(int tagId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return sqlController.TagDelete(tagId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion


        #region speach to text

        public bool TranscribeUploadedData(int uploadedDataId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    uploaded_data uploadedData = sqlController.GetUploadedData(uploadedDataId);
                    if (uploadedData != null)
                    {
                        string[] audioFileExtenstions = { ".3gp", ".aa", ".aac", ".aax", ".act", ".aiff", ".amr", ".ape", ".au", ".awb", ".dct", ".dss", ".dvf", ".flac", ".gsm", ".iklax", ".ivs", ".m4a", ".m4b", ".m4p", ".mmf", ".mp3", ".mpc", ".msv", ".nsf", ".ogg", ".oga", ".mogg", ".opus", ".ra", ".rm", ".raw", ".sln", ".tta", ".vox", ".wav", ".wma", ".wv", ".webm", ".8svx" };
                        if (audioFileExtenstions.Any(uploadedData.extension.Contains)) {

                            int requestId = SpeechToText(uploadedData.file_location + @"\" + uploadedData.file_name);
                            uploadedData.transcription_id = requestId;

                            sqlController.UpdateUploadedData(uploadedData);
                        }
                        return true;
                    } else
                    {
                        return false;
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public int SpeechToText(string pathToAudioFile)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return communicator.SpeechToText(pathToAudioFile);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool SpeechToText(int requestId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    //Tuple<Site_Dto, Unit_Dto> siteResult = communicator.SiteCreate(name);
                    return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        #endregion

        #region public advanced actions
        #region templat
        public bool Advanced_TemplateDisplayIndexChangeDb(int templateId, int newDisplayIndex)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(newDisplayIndex), newDisplayIndex);

                    return sqlController.TemplateDisplayIndexChange(templateId, newDisplayIndex);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_TemplateDisplayIndexChangeServer(int templateId, int siteUId, int newDisplayIndex)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteUId), siteUId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(newDisplayIndex), newDisplayIndex);

                    string respXml = null;
                    List<string> errors = new List<string>();
                    foreach (string microtingUId in sqlController.CheckListSitesRead(templateId, siteUId, Constants.WorkflowStates.NotRemoved))
                    {
                        respXml = communicator.TemplateDisplayIndexChange(microtingUId.ToString(), siteUId, newDisplayIndex);
                        Response resp = new Response();
                        resp = resp.XmlToClassUsingXmlDocument(respXml);
                        if (resp.Type != Response.ResponseTypes.Success)
                        {
                            string error = "Failed to set display index for eForm " + microtingUId + " to " + newDisplayIndex;
                            errors.Add(error);
                        }
                    }
                    if (errors.Count() > 0)
                    {
                        throw new Exception(String.Join("\n", errors));
                    }
                    return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_TemplateUpdateFieldIdsForColumns(int templateId, int? fieldId1, int? fieldId2, int? fieldId3, int? fieldId4, int? fieldId5, int? fieldId6, int? fieldId7, int? fieldId8, int? fieldId9, int? fieldId10)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId1), fieldId1);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId2), fieldId2);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId3), fieldId3);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId4), fieldId4);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId5), fieldId5);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId6), fieldId6);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId7), fieldId7);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId8), fieldId8);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId9), fieldId9);
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId10), fieldId10);

                    return sqlController.TemplateUpdateFieldIdsForColumns(templateId, fieldId1, fieldId2, fieldId3, fieldId4, fieldId5, fieldId6, fieldId7, fieldId8, fieldId9, fieldId10);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public List<Field_Dto> Advanced_TemplateFieldReadAll(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return sqlController.TemplateFieldReadAll(templateId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public void Advanced_ConsistencyCheckTemplates()
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    List<int> dummyList = new List<int>();
                    List<Template_Dto> allTemplates = sqlController.TemplateItemReadAll(true, Constants.WorkflowStates.Removed, "", false, "", dummyList);
                    foreach (Template_Dto item in allTemplates)
                    {
                        foreach (SiteName_Dto site in item.DeployedSites)
                        {
                            CaseDelete(item.Id, site.SiteUId, "");
                        }
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
            }
        }

        #region sites
        public List<Site_Dto> Advanced_SiteReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    log.LogVariable(t.GetMethodName("Core"), nameof(offSet), offSet.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(limit), limit.ToString());

                    return sqlController.SimpleSiteGetAll(workflowState, offSet, limit);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public SiteName_Dto Advanced_SiteItemRead(int microting_uuid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microting_uuid), microting_uuid);

                    return sqlController.SiteRead(microting_uuid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public List<SiteName_Dto> Advanced_SiteItemReadAll()
        {
            return Advanced_SiteItemReadAll(true);
        }

        public List<SiteName_Dto> Advanced_SiteItemReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return sqlController.SiteGetAll(includeRemoved);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_SiteItemUpdate(int siteId, string name)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(name), name);

                    if (sqlController.SiteRead(siteId) == null)
                        return false;

                    bool success = communicator.SiteUpdate(siteId, name);
                    if (!success)
                        return false;

                    return sqlController.SiteUpdate(siteId, name);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_SiteItemDelete(int siteId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId);

                    bool success = communicator.SiteDelete(siteId);
                    if (!success)
                        return false;

                    return sqlController.SiteDelete(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        #endregion

        #region workers
        public Worker_Dto Advanced_WorkerCreate(string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(firstName), firstName);
                    log.LogVariable(t.GetMethodName("Core"), nameof(lastName), lastName);
                    log.LogVariable(t.GetMethodName("Core"), nameof(email), email);

                    Worker_Dto workerDto = communicator.WorkerCreate(firstName, lastName, email);
                    int workerUId = workerDto.WorkerUId;

                    workerDto = sqlController.WorkerRead(workerDto.WorkerUId);
                    if (workerDto == null)
                    {
                        sqlController.WorkerCreate(workerUId, firstName, lastName, email);
                    }

                    return Advanced_WorkerRead(workerUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public string Advanced_WorkerNameRead(int workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);

                    return sqlController.WorkerNameRead(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public Worker_Dto Advanced_WorkerRead(int workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);

                    return sqlController.WorkerRead(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public List<Worker_Dto> Advanced_WorkerReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    log.LogVariable(t.GetMethodName("Core"), nameof(offSet), offSet.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(limit), limit.ToString());

                    return sqlController.WorkerGetAll(workflowState, offSet, limit);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_WorkerUpdate(int workerId, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(firstName), firstName);
                    log.LogVariable(t.GetMethodName("Core"), nameof(lastName), lastName);
                    log.LogVariable(t.GetMethodName("Core"), nameof(email), email);

                    if (sqlController.WorkerRead(workerId) == null)
                        return false;

                    bool success = communicator.WorkerUpdate(workerId, firstName, lastName, email);
                    if (!success)
                        return false;

                    return sqlController.WorkerUpdate(workerId, firstName, lastName, email);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_WorkerDelete(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    bool success = communicator.WorkerDelete(microtingUid);
                    if (!success)
                        return false;

                    return sqlController.WorkerDelete(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion
        #endregion

        #region site_workers
        public Site_Worker_Dto Advanced_SiteWorkerCreate(SiteName_Dto siteDto, Worker_Dto workerDto)
        {   
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), "siteId", siteDto.SiteUId);
                    log.LogVariable(t.GetMethodName("Core"), "workerId", workerDto.WorkerUId);

                    Site_Worker_Dto result = communicator.SiteWorkerCreate(siteDto.SiteUId, workerDto.WorkerUId);
                    //var parsedData = JRaw.Parse(result);
                    //int workerUid = int.Parse(parsedData["id"].ToString());

                    Site_Worker_Dto siteWorkerDto = sqlController.SiteWorkerRead(result.MicrotingUId, null, null);

                    if (siteWorkerDto == null)
                    {
                        sqlController.SiteWorkerCreate(result.MicrotingUId, siteDto.SiteUId, workerDto.WorkerUId);
                    }

                    return Advanced_SiteWorkerRead(result.MicrotingUId, null, null);
                    //return null;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public Site_Worker_Dto Advanced_SiteWorkerRead(int? siteWorkerMicrotingUid, int? siteId, int? workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteWorkerMicrotingUid), siteWorkerMicrotingUid.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId.ToString());
                    log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId.ToString());

                    return sqlController.SiteWorkerRead(siteWorkerMicrotingUid, siteId, workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_SiteWorkerDelete(int workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);

                    bool success = communicator.SiteWorkerDelete(workerId);
                    if (!success)
                        return false;

                    return sqlController.SiteWorkerDelete(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region units
        public Unit_Dto Advanced_UnitRead(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    return sqlController.UnitRead(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public List<Unit_Dto> Advanced_UnitReadAll()
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");

                    return sqlController.UnitGetAll();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public Unit_Dto Advanced_UnitRequestOtp(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    int otp_code = communicator.UnitRequestOtp(microtingUid);

                    Unit_Dto my_dto = Advanced_UnitRead(microtingUid);

                    sqlController.UnitUpdate(microtingUid, my_dto.CustomerNo, otp_code, my_dto.SiteUId);

                    return Advanced_UnitRead(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw ex;
            }
        }
        #endregion

        #region fields
        public Field Advanced_FieldRead(int id)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(id), id);

                    return sqlController.FieldRead(id);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public UploadedData Advanced_UploadedDataRead(int id)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                log.LogStandard(t.GetMethodName("Core"), "called");
                log.LogVariable(t.GetMethodName("Core"), nameof(id), id);

                var ud = sqlController.GetUploadedData(id);
                UploadedData uD = new UploadedData();
                uD.Checksum = ud.checksum;
                uD.CurrentFile = ud.current_file;
                uD.Extension = ud.extension;
                uD.FileLocation = ud.file_location;
                uD.FileName = ud.file_name;
                uD.Id = ud.id;
                uD.UploaderId = ud.uploader_id;
                uD.UploaderType = ud.uploader_type;
                return uD;
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public List<FieldValue> Advanced_FieldValueReadList(int id, int instances)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(id), id);
                    log.LogVariable(t.GetMethodName("Core"), nameof(instances), instances);

                    return sqlController.FieldValueReadList(id, instances);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        //EntityGroupList
        public EntityGroupList Advanced_EntityGroupAll(string sort, string nameFilter, int pageIndex, int pageSize, string entityType, bool desc, string workflowState)
        {
            if (entityType != Constants.FieldTypes.EntitySearch && entityType != Constants.FieldTypes.EntitySelect)
                throw new Exception("EntityGroupAll failed. EntityType:" + entityType + " is not an known type");
            if (workflowState != Constants.WorkflowStates.NotRemoved && workflowState != Constants.WorkflowStates.Created && workflowState != Constants.WorkflowStates.Removed)
                throw new Exception("EntityGroupAll failed. workflowState:" + workflowState + " is not an known workflow state");

            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(sort), sort);
                    log.LogVariable(t.GetMethodName("Core"), nameof(nameFilter), nameFilter);
                    log.LogVariable(t.GetMethodName("Core"), nameof(pageIndex), pageIndex);
                    log.LogVariable(t.GetMethodName("Core"), nameof(pageSize), pageSize);
                    log.LogVariable(t.GetMethodName("Core"), nameof(entityType), entityType);
                    log.LogVariable(t.GetMethodName("Core"), nameof(desc), desc);
                    log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    return sqlController.EntityGroupAll(sort, nameFilter, pageIndex, pageSize, entityType, desc, workflowState);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_DeleteUploadedData(int fieldId, int uploadedDataId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(fieldId), fieldId);
                    log.LogVariable(t.GetMethodName("Core"), nameof(uploadedDataId), uploadedDataId);

                    uploaded_data uD = sqlController.GetUploadedData(uploadedDataId);

                    try
                    {
                        Directory.CreateDirectory(uD.file_location + "Deleted");
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                            File.Move(uD.file_location + uD.file_name, uD.file_location + @"Deleted\" + uD.file_name);
                        }
                        else
                        {
                            File.Move(uD.file_location + uD.file_name, uD.file_location + @"Deleted/" + uD.file_name);
                        }
                    }
                    catch (Exception exd)
                    {
                        log.LogException(t.GetMethodName("Core"), "failed", exd, true);
                        throw new Exception("failed", exd);
                    }

                    return sqlController.DeleteFile(uploadedDataId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public bool Advanced_UpdateCaseFieldValue(int caseId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    log.LogStandard(t.GetMethodName("Core"), "called");
                    log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);
                    return sqlController.CaseUpdateFieldValues(caseId);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region private
        private List<Element> ReplaceDataElementsAndDataItems(int caseId, List<Element> elementList, List<FieldValue> lstAnswers)
        {
            List<Element> elementListReplaced = new List<Element>();

            foreach (Element element in elementList)
            {
                #region if DataElement
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    #region replace DataItemGroups
                    foreach (var dataItemGroup in dataE.DataItemGroupList)
                    {
                        FieldContainer fG = (FieldContainer)dataItemGroup;

                        List<DataItem> dataItemListTemp = new List<DataItem>();
                        foreach (var dataItem in fG.DataItemList)
                        {
                            foreach (var answer in lstAnswers)
                            {
                                if (dataItem.Id == answer.Id)
                                {
                                    dataItemListTemp.Add(answer);
                                    break;
                                }
                            }
                        }
                        fG.DataItemList = dataItemListTemp;
                    }
                    #endregion

                    #region replace DataItems
                    List<DataItem> dataItemListTemp2 = new List<DataItem>();
                    foreach (var dataItem in dataE.DataItemList)
                    {
                        foreach (var answer in lstAnswers)
                        {
                            if (dataItem.Id == answer.FieldId)
                            {
                                dataItemListTemp2.Add(answer);
                                break;
                            }
                        }
                    }
                    dataE.DataItemList = dataItemListTemp2;
                    #endregion

                    elementListReplaced.Add(new CheckListValue(dataE, sqlController.CheckListValueStatusRead(caseId, element.Id)));
                }
                #endregion

                #region if GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    ReplaceDataElementsAndDataItems(caseId, groupE.ElementList, lstAnswers);

                    elementListReplaced.Add(groupE);
                }
                #endregion
            }

            return elementListReplaced;
        }

        private string SendXml(MainElement mainElement, int siteId)
        {
            log.LogEverything(t.GetMethodName("Core"), "siteId:" + siteId + ", requested sent eForm");

            string xmlStrRequest = mainElement.ClassToXml();
            string xmlStrResponse = communicator.PostXml(xmlStrRequest, siteId);

            Response response = new Response();
            response = response.XmlToClass(xmlStrResponse);

            //if reply is "success", it's created
            if (response.Type.ToString().ToLower() == "success")
            {
                return response.Value;
            }

            throw new Exception("siteId:'" + siteId + "' // failed to create eForm at Microting // Response :" + xmlStrResponse);
        }

        private List<List<string>> GenerateDataSetFromCases(int? templateId, DateTime? start, DateTime? end, string customPathForUploadedData, string decimalSeparator, string thousandSaperator)
        {
            List<List<string>> dataSet = new List<List<string>>();
            List<string> colume1CaseIds = new List<string> { "Id" };
            List<int> caseIds = new List<int>();

            List<Case> caseList = sqlController.CaseReadAll(templateId, start, end, Constants.WorkflowStates.NotRemoved, null, false, "");
            var template = sqlController.TemplateItemRead((int)templateId);

            if (caseList.Count == 0)
                return null;

            #region firstColumes generate
            {
                List<string> colume2 = new List<string> { "Date" };
                List<string> colume3 = new List<string> { "Time" };
                List<string> colume4 = new List<string> { "Day" };
                List<string> colume5 = new List<string> { "Week" };
                List<string> colume6 = new List<string> { "Month" };
                List<string> colume7 = new List<string> { "Year" };
                List<string> colume8 = new List<string> { "Created At" };
                List<string> colume9 = new List<string> { "Site" };
                List<string> colume10 = new List<string> { "Device User" };
                List<string> colume11 = new List<string> { "Device Id" };
                List<string> colume12 = new List<string> { "eForm Name" };

                var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
                foreach (var aCase in caseList)
                {
                    DateTime time = (DateTime)aCase.DoneAt;
                    colume1CaseIds.Add(aCase.Id.ToString());
                    caseIds.Add(aCase.Id);

                    colume2.Add(time.ToString("yyyy.MM.dd"));
                    colume3.Add(time.ToString("HH:mm:ss"));
                    colume4.Add(time.DayOfWeek.ToString());
                    colume5.Add(time.Year.ToString() + "." + cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
                    colume6.Add(time.Year.ToString() + "." + time.ToString("MMMM").Substring(0, 3));
                    colume7.Add(time.Year.ToString());
                    colume8.Add(aCase.CreatedAt.Value.ToString("yyyy.MM.dd HH:mm:ss"));
                    colume9.Add(aCase.SiteName);
                    colume10.Add(aCase.WorkerName);
                    colume11.Add(aCase.UnitId.ToString());
                    colume12.Add(template.Label);
                }

                dataSet.Add(colume1CaseIds);
                dataSet.Add(colume2);
                dataSet.Add(colume3);
                dataSet.Add(colume4);
                dataSet.Add(colume5);
                dataSet.Add(colume6);
                dataSet.Add(colume7);
                dataSet.Add(colume8);
                dataSet.Add(colume9);
                dataSet.Add(colume10);
                dataSet.Add(colume11);
                dataSet.Add(colume12);
            }
            #endregion

            #region fieldValue generate
            {
                if (templateId != null)
                {
                    MainElement templateData = sqlController.TemplateRead((int)templateId);

                    List<string> lstReturn = new List<string>();
                    lstReturn = GenerateDataSetFromCasesSubSet(lstReturn, templateData.ElementList, "");

                    List<string> newRow;
                    foreach (string set in lstReturn)
                    {
                        int fieldId = int.Parse(t.SplitToList(set, 0, false));
                        string label = t.SplitToList(set, 1, false);

                        List<List<eFormShared.KeyValuePair>> result = sqlController.FieldValueReadAllValues(fieldId, caseIds, customPathForUploadedData, decimalSeparator, thousandSaperator);

                        if (result.Count == 1)
                        {
                            newRow = new List<string>();
                            newRow.Insert(0, label);
                            List<eFormShared.KeyValuePair> tempList = result[0];
                            foreach (int i in caseIds)
                            {
                                string value = "";
                                foreach (eFormShared.KeyValuePair KvP in tempList)
                                {
                                    if (KvP.Key == i.ToString())
                                    {
                                        value = KvP.Value;
                                    }
                                }
                                newRow.Add(value);
                            }
                            dataSet.Add(newRow);
                        }
                        else
                        {
                            int option = 0;
                            Field field = sqlController.FieldRead(fieldId);
                            foreach (var lst in result)
                            {
                                newRow = new List<string>();
                                List<eFormShared.KeyValuePair> fieldKvP = field.KeyValuePairList;
                                newRow.Insert(0, label + " | " + fieldKvP.ElementAt(option).Value);
                                foreach (int i in caseIds)
                                {
                                    string value = "";
                                    foreach (eFormShared.KeyValuePair KvP in lst)
                                    {
                                        if (KvP.Key == i.ToString())
                                        {
                                            value = KvP.Value;
                                        }
                                    }
                                    newRow.Add(value);
                                }
                                dataSet.Add(newRow);
                                option++;
                            }
                        }
                    }
                }
            }
            #endregion

            return dataSet;
        }

        private List<string> GenerateDataSetFromCasesSubSet(List<string> lstReturn, List<Element> elementList, string preLabel)
        {
            foreach (Element element in elementList)
            {
                string sep = " / ";

                #region if DataElement
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    if (preLabel != "")
                        preLabel = preLabel + sep + dataE.Label;

                    foreach (FieldContainer dataItemGroup in dataE.DataItemGroupList)
                    {
                        foreach (DataItem dataItem in dataItemGroup.DataItemList)
                        {
                            if (dataItem.GetType() == typeof(SaveButton))
                                continue;
                            if (dataItem.GetType() == typeof(None))
                                continue;

                            if (preLabel != "")
                                lstReturn.Add(dataItem.Id + "|" + preLabel.Remove(0, sep.Length) + sep + dataItemGroup.Label + sep + dataItem.Label);
                            else
                                lstReturn.Add(dataItem.Id + "|" + dataItemGroup.Label + sep + dataItem.Label);
                        }
                    }

                    foreach (DataItem dataItem in dataE.DataItemList)
                    {
                        if (dataItem.GetType() == typeof(SaveButton))
                            continue;
                        if (dataItem.GetType() == typeof(None))
                            continue;

                        if (preLabel != "")
                            lstReturn.Add(dataItem.Id + "|" + preLabel.Remove(0, sep.Length) + sep + dataItem.Label);
                        else
                            lstReturn.Add(dataItem.Id + "|" + dataItem.Label);
                    }
                }
                #endregion

                #region if GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    if (preLabel != "")
                        GenerateDataSetFromCasesSubSet(lstReturn, groupE.ElementList, preLabel + sep + groupE.Label);
                    else
                        GenerateDataSetFromCasesSubSet(lstReturn, groupE.ElementList, groupE.Label);
                }
                #endregion
            }

            return lstReturn;
        }

        private List<string> PdfValidate(string pdfString, int pdfId)
        {
            List<string> errorLst = new List<string>();

            if (pdfString.ToLower().Contains("microting.com"))
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' contains an URL that points to Microting's builder temporary hosting. Indicating that it's not a proper hash");
            if (pdfString.ToLower().Contains("http") || pdfString.ToLower().Contains("https"))
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' contains an HTTP or HTTPS. Indicating that it's not a proper hash");
            if (pdfString.Length != 32)
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' lenght is not the correct lenght (32). Indicating that it's not a proper hash");

            if (errorLst.Count > 0)
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' please check 'value' input, and consider running PdfUpload");

            return errorLst;
        }
        
        private void GetChecksAndFields(ref string clsLst, ref string fldLst, List<Element> elementLst, string customPathForUploadedData)

        {
            string jasperFieldXml = "";
            string jasperCheckXml = "";

            foreach (Element element in elementLst)
            {
                if (element.GetType() == typeof(CheckListValue))
                {
                    CheckListValue dataE = (CheckListValue)element;

                    jasperCheckXml += Environment.NewLine + "<C" + dataE.Id + ">" + dataE.Status + "</C" + dataE.Id + ">";

                    foreach (var item in dataE.DataItemList)
                    {

                        jasperFieldXml += Environment.NewLine + "<F" + item.Id + " name=\"" + item.Label + "\" parent=\"" + item.Label + "\">";

                        if (item is Field)
                        {
                            Field field = (Field)item;
                            foreach (FieldValue answer in field.FieldValues)
                            {
                                switch (field.FieldType)
                                {
                                    case Constants.FieldTypes.Picture:
                                    case Constants.FieldTypes.Signature:
                                        if (answer.UploadedDataObj != null)
                                        {

                                            if (customPathForUploadedData != null)
                                            {
                                                jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\"><![CDATA[" + customPathForUploadedData + answer.UploadedDataObj.FileName + "]]></F" + field.Id + "_value>";
                                            }
                                            else
                                            {
                                                jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\"><![CDATA[" + answer.UploadedDataObj.FileName + "]]></F" + field.Id + "_value>";
                                            }
                                        }
                                        else
                                        {
                                            //jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\">NO FILE</F" + field.Id + "_value>";
                                        }
                                        break;
                                    default:
                                        jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\"><![CDATA[" + (answer.ValueReadable ?? string.Empty) + "]]></F" + field.Id + "_value>";
                                        break;
                                }
                            }
                        } else if (item is FieldContainer)
                        {
                            FieldContainer fieldC = (FieldContainer)item;

                            foreach (Field field in fieldC.DataItemList)
                            {
                                jasperFieldXml += Environment.NewLine + "<F" + field.Id + " name=\"" + field.Label + "\" parent=\"" + dataE.Label + "\">";
                                foreach (FieldValue answer in field.FieldValues)
                                {
                                    switch (field.FieldType)
                                    {
                                        case Constants.FieldTypes.Picture:
                                        case Constants.FieldTypes.Signature:
                                            if (answer.UploadedDataObj != null)
                                            {

                                                if (customPathForUploadedData != null)
                                                {
                                                    jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\"><![CDATA[" + customPathForUploadedData + answer.UploadedDataObj.FileName + "]]></F" + field.Id + "_value>";
                                                }
                                                else
                                                {
                                                    jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\"><![CDATA[" + answer.UploadedDataObj.FileName + "]]></F" + field.Id + "_value>";
                                                }
                                            }
                                            else
                                            {
                                                //jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\">NO FILE</F" + field.Id + "_value>";
                                            }
                                            break;
                                        default:
                                            jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\"><![CDATA[" + (answer.ValueReadable ?? string.Empty) + "]]></F" + field.Id + "_value>";
                                            break;
                                    }
                                }


                                jasperFieldXml += Environment.NewLine + "</F" + field.Id + ">";
                            }
                        }

                        jasperFieldXml += Environment.NewLine + "</F" + item.Id + ">";
                    }
                }

                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;
                    GetChecksAndFields(ref clsLst, ref fldLst, groupE.ElementList, customPathForUploadedData);
                }
            }

            clsLst = clsLst + jasperCheckXml;
            fldLst = fldLst + jasperFieldXml;
        }
        #endregion

        #region intrepidation threads
        private void CoreThread()
        {
            coreThreadRunning = true;

            log.LogEverything(t.GetMethodName("Core"), "initiated");
            while (coreAvailable)
            {
                try
                {
                    if (coreThreadRunning)
                    {
                        //Thread updateFilesThread
                        //    = new Thread(() => CoreHandleUpdateFiles());
                        //updateFilesThread.Start();

                        //Thread updateNotificationsThread
                        //    = new Thread(() => CoreHandleUpdateNotifications());
                        //updateNotificationsThread.Start();

                        //Thread updateTablesThread
                        //    = new Thread(() => CoreHandleUpdateTables());
                        //updateTablesThread.Start();

                        Thread.Sleep(2000);
                    }

                    Thread.Sleep(500);
                }
                catch (ThreadAbortException)
                {
                    log.LogWarning(t.GetMethodName("Core"), "catch of ThreadAbortException");
                }
                catch (Exception ex)
                {
                    FatalExpection(t.GetMethodName("Core") + "failed", ex);
                }
            }
            log.LogEverything(t.GetMethodName("Core"), "completed");

            coreThreadRunning = false;
        }

        public bool DownloadUploadedData(int uploadedDataId)
        {
            uploaded_data uploadedData = sqlController.GetUploadedData(uploadedDataId);

            if (uploadedData != null)
            {
                string urlStr = uploadedData.file_location;
                log.LogEverything(t.GetMethodName("Core"), "Received file:" + uploadedData.ToString());

                #region finding file name and creating folder if needed
                FileInfo file = new FileInfo(fileLocationPicture);
                file.Directory.Create(); // If the directory already exists, this method does nothing.

                int index = urlStr.LastIndexOf("/") + 1;
                string fileName = uploadedData.id.ToString() + "_" + urlStr.Remove(0, index);
                #endregion

                #region download file
                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(urlStr, fileLocationPicture + fileName);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Downloading and creating fil locally failed.", ex);
                    }

                }
                #endregion

                #region finding checkSum
                string chechSum = "";
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(fileLocationPicture + fileName))
                    {
                        byte[] grr = md5.ComputeHash(stream);
                        chechSum = BitConverter.ToString(grr).Replace("-", "").ToLower();
                    }
                }
                #endregion

                #region checks checkSum
                if (chechSum != fileName.Substring(fileName.LastIndexOf(".") - 32, 32))
                    log.LogWarning(t.GetMethodName("Core"), "Download of '" + urlStr + "' failed. Check sum did not match");
                #endregion

                Case_Dto dto = sqlController.FileCaseFindMUId(urlStr);
                File_Dto fDto = new File_Dto(dto.SiteUId, dto.CaseType, dto.CaseUId, dto.MicrotingUId, dto.CheckUId, fileLocationPicture + fileName);
                try { HandleFileDownloaded?.Invoke(fDto, EventArgs.Empty); }
                catch { log.LogWarning(t.GetMethodName("Core"), "HandleFileDownloaded event's external logic suffered an Expection"); }
                log.LogStandard(t.GetMethodName("Core"), "Downloaded file '" + urlStr + "'.");

                sqlController.FileProcessed(urlStr, chechSum, fileLocationPicture, fileName, uploadedData.id);

                if (_swiftEnabled)
                {
                    if (File.Exists(Path.Combine(fileLocationPicture, fileName)))
                    {
                        using (var fileStream = File.OpenRead(fileName))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                fileStream.CopyTo(memoryStream);

                                var resp = _swiftClient.PutObject(_comOrganizationId + "_uploaded_data", fileName, memoryStream);                                
                            }
                        }
                    }
                }
                
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<Stream> GetFileFromStorageSystem(string fileName)
        {
            if (_swiftEnabled)
            {
                SwiftResponse rsp = await _swiftClient.GetObject(_comOrganizationId + "_uploaded_data", fileName);
                if (rsp.IsSuccess)
                {
                    return rsp.Stream;
                }
                else
                {
                    throw new Exception("Could not get file " + fileName);
                }
            }
            else
            {_swiftEnabled
                throw new FileNotFoundException();
            }
        }

        public async Task<bool> PutFilToStorageSystem(String filePath, string fileName)
        {
            if (_swiftEnabled)
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                SwiftResponse rsp = await _swiftClient.PutObject(_comOrganizationId + "_uploaded_data", fileName, fileStream);
                if (rsp.IsSuccess)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Could not get file " + fileName);
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        
        public bool CheckStatusByMicrotingUid(string microtingUid)
        {
            List<Case_Dto> lstCase = new List<Case_Dto>();
            MainElement mainElement = new MainElement();

            Case_Dto concreteCase = sqlController.CaseReadByMUId(microtingUid);
            log.LogEverything(t.GetMethodName("Core"), concreteCase.ToString() + " has been matched");

            if (concreteCase.CaseUId == "" || concreteCase.CaseUId == "ReversedCase")
                lstCase.Add(concreteCase);
            else
                lstCase = sqlController.CaseReadByCaseUId(concreteCase.CaseUId);

            foreach (Case_Dto aCase in lstCase)
            {
                if (aCase.SiteUId == concreteCase.SiteUId)
                {
                    #region get response's data and update DB with data
                    string checkIdLastKnown = sqlController.CaseReadLastCheckIdByMicrotingUId(microtingUid); //null if NOT a checkListSite
                    log.LogVariable(t.GetMethodName("Core"), nameof(checkIdLastKnown), checkIdLastKnown);

                    string respXml;
                    if (checkIdLastKnown == null)
                        respXml = communicator.Retrieve(microtingUid, concreteCase.SiteUId);
                    else
                        respXml = communicator.RetrieveFromId(microtingUid, concreteCase.SiteUId, checkIdLastKnown);
                    log.LogVariable(t.GetMethodName("Core"), nameof(respXml), respXml);

                    Response resp = new Response();
                    resp = resp.XmlToClassUsingXmlDocument(respXml);
                    //resp = resp.XmlToClass(respXml);

                    if (resp.Type == Response.ResponseTypes.Success)
                    {
                        log.LogEverything(t.GetMethodName("Core"), "resp.Type == Response.ResponseTypes.Success (true)");
                        if (resp.Checks.Count > 0)
                        {
                            XmlDocument xDoc = new XmlDocument();

                            xDoc.LoadXml(respXml);
                            XmlNode checks = xDoc.DocumentElement.LastChild;
                            int i = 0;
                            foreach (Check check in resp.Checks)
                            {

                                int unitUId = sqlController.UnitRead(int.Parse(check.UnitId)).UnitUId;
                                log.LogVariable(t.GetMethodName("Core"), nameof(unitUId), unitUId);
                                int workerUId = sqlController.WorkerRead(int.Parse(check.WorkerId)).WorkerUId;
                                log.LogVariable(t.GetMethodName("Core"), nameof(workerUId), workerUId);

                                sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml.ToString(), i);

                                sqlController.CaseUpdateCompleted(microtingUid, check.Id, DateTime.Parse(check.Date), workerUId, unitUId);
                                log.LogEverything(t.GetMethodName("Core"), "sqlController.CaseUpdateCompleted(...)");

                                #region IF needed retract case, thereby completing the process
                                if (checkIdLastKnown == null)
                                {
                                    string responseRetractionXml = communicator.Delete(aCase.MicrotingUId, aCase.SiteUId);
                                    Response respRet = new Response();
                                    respRet = respRet.XmlToClass(respXml);

                                    if (respRet.Type == Response.ResponseTypes.Success)
                                    {
                                        log.LogEverything(t.GetMethodName("Core"), aCase.ToString() + " has been retracted");
                                    }
                                    else
                                        log.LogWarning(t.GetMethodName("Core"), "Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteUId + ". Not a critical issue, but needs to be fixed if repeated");
                                }
                                #endregion

                                sqlController.CaseRetract(microtingUid, check.Id);
                                log.LogEverything(t.GetMethodName("Core"), "sqlController.CaseRetract(...)");
                                // TODO add case.id
                                Case_Dto cDto = sqlController.CaseReadByMUId(microtingUid);
								//InteractionCaseUpdate(cDto);
								FireHandleCaseCompleted(cDto);
                                //try { HandleCaseCompleted?.Invoke(cDto, EventArgs.Empty); }
                                //catch { log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection"); }
                                log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been completed");
                                i++;
                            }
                        }
                    }
                    else
                    {
                        log.LogEverything(t.GetMethodName("Core"), "resp.Type == Response.ResponseTypes.Success (false)");
                        throw new Exception("Failed to retrive eForm " + microtingUid + " from site " + aCase.SiteUId);
                    }
                    #endregion
                }
                else
                {
                    //delete eForm on other tablets and update DB to "deleted"
                    CaseDelete(aCase.MicrotingUId);
                }
            }
            return true;
        }        
        #endregion


        public List<eFormShared.KeyValuePair> PairRead(string str)
        {
            return sqlController.PairRead(str);
        }

        #region fireEvents

        public void FireHandleCaseCompleted(Case_Dto caseDto)
		{
		    log.LogStandard(t.GetMethodName("Core"), $"FireHandleCaseCompleted for MicrotingUId {caseDto.MicrotingUId}");
			try { HandleCaseCompleted.Invoke(caseDto, EventArgs.Empty); }
			catch (Exception ex)
			{
				log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection");
				throw ex;
			}
		}

        public void FireHandleCaseDeleted(Case_Dto caseDto)
        {
            try { HandleCaseDeleted?.Invoke(caseDto, EventArgs.Empty); }
            catch { log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection"); }
        }

        public void FireHandleNotificationNotFound(Note_Dto notification)
        {
            try { HandleNotificationNotFound?.Invoke(notification, EventArgs.Empty); }
            catch { log.LogWarning(t.GetMethodName("Core"), "HandleNotificationNotFound event's external logic suffered an Expection"); }
        }

        public void FireHandleSiteActivated(Note_Dto notification)
        {
            try { HandleSiteActivated?.Invoke(notification, EventArgs.Empty); }
            catch { log.LogWarning(t.GetMethodName("Core"), "HandleSiteActivated event's external logic suffered an Expection"); }
        }

		public void FireHandleCaseRetrived(Case_Dto caseDto)
		{
		    log.LogStandard(t.GetMethodName("Core"), $"FireHandleCaseRetrived for MicrotingUId {caseDto.MicrotingUId}");

			try { HandleCaseRetrived.Invoke(caseDto, EventArgs.Empty); }
			catch (Exception ex)
			{
				log.LogWarning(t.GetMethodName("Core"), "HandleCaseRetrived event's external logic suffered an Expection");
				throw ex;
			}
		}
        #endregion
    }

}