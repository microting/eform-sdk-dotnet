/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using Castle.MicroKernel.Registration;
using Rebus.Bus;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microting.eForm;
using Microting.eForm.Communication;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;
using Microting.eForm.Infrastructure.Models.reply;
using Microting.eForm.Installers;
using Microting.eForm.Services;
using OpenStack.NetCoreSwiftClient;
using OpenStack.NetCoreSwiftClient.Infrastructure.Models;
using Tag = Microting.eForm.Dto.Tag;


namespace eFormCore
{
    public class Core : CoreBase
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
        private Subscriber _subscriber;
        private Communicator _communicator;
        private SqlController _sqlController;
        private readonly Tools t = new Tools();

        private IWindsorContainer _container;

        public Log log;

        private readonly object _lockMain = new object();
        object _lockEventMessage = new object();

//        private bool _updateIsRunningFiles = false;
//        private bool _updateIsRunningNotifications = false;
        private bool _updateIsRunningEntities = false;

        private bool _coreThreadRunning = false;
        private bool _coreRestarting = false;
        private bool _coreStatChanging = false;
        private bool _coreAvailable = false;

        private bool skipRestartDelay = false;

        private string _connectionString;
        private string _fileLocationPicture;
        private string _fileLocationPdf;

        private int _sameExceptionCountTried = 0;
        private int _maxParallelism = 1;
        private int _numberOfWorkers = 1;
        private string _customerNo;
        private IBus _bus;
        
        #region swift
        private bool _swiftEnabled = false;
        private string _swiftUserName = "";
        private string _swiftPassword = "";
        private string _swiftEndpoint = "";
        private string _keystoneEndpoint = "";
        private SwiftClientService _swiftClient;
        #endregion
        
        #region s3
        private bool _s3Enabled = false;
        private string _s3AccessKeyId = "";
        private string _s3SecretAccessKey = "";
        private string _s3Endpoint = "";
        private static AmazonS3Client _s3Client;
        #endregion
		#endregion

		//con

		#region public state

        /// <summary>
        /// Starts the Core and enables Events. Restarts if needed
        /// </summary>
        public async Task<bool> Start(string connectionString)
		{
			try
			{
				if (!_coreAvailable && !_coreStatChanging)
				{


					if (!await StartSqlOnly(connectionString))
					{
						return false;
					}

                    try
                    {
                        _maxParallelism = int.Parse(await _sqlController.SettingRead(Settings.maxParallelism));
                        _numberOfWorkers = int.Parse(await _sqlController.SettingRead(Settings.numberOfWorkers));
                    }
                    catch { }

				    

                    _container.Install(
						new RebusHandlerInstaller()
						, new RebusInstaller(connectionString, _maxParallelism, _numberOfWorkers)
					);
					_bus = _container.Resolve<IBus>();
					await log.LogCritical(t.GetMethodName("Core"), "called");

					//---

					_coreStatChanging = true;

					//subscriber
					_subscriber = new Subscriber(_sqlController, log, _bus);
					_subscriber.Start();
					await log.LogStandard(t.GetMethodName("Core"), "Subscriber started");

					await log.LogCritical(t.GetMethodName("Core"), "started");
					_coreAvailable = true;
					_coreStatChanging = false;

                    //coreThread
                    //Thread coreThread = new Thread(() => CoreThread());
                    //coreThread.Start();
                    _coreThreadRunning = true;

                    await log.LogStandard(t.GetMethodName("Core"), "CoreThread started");
				}
			}
			#region catch
			catch (Exception ex)
			{
                await FatalExpection(t.GetMethodName("Core") + " failed", ex);
				throw ex;
				//return false;
			}
			#endregion

			return true;
		}

        public async Task<bool> StartSqlOnly(string connectionString)
        {

            try
            {
                if (!_coreAvailable && !_coreStatChanging)
                {
                    _coreStatChanging = true;

                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

                    //sqlController
                    _sqlController = new SqlController(connectionString);

                    //check settings
                    if (_sqlController.SettingCheckAll().Result.Count > 0)
                        throw new ArgumentException("Use AdminTool to setup database correctly. 'SettingCheckAll()' returned with errors");

                    if (await _sqlController.SettingRead(Settings.token) == "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
                        throw new ArgumentException("Use AdminTool to setup database correctly. Token not set, only default value found");

                    if (await _sqlController.SettingRead(Settings.firstRunDone) != "true")
                        throw new ArgumentException("Use AdminTool to setup database correctly. FirstRunDone has not completed");

                    if (await _sqlController.SettingRead(Settings.knownSitesDone) != "true")
                        throw new ArgumentException("Use AdminTool to setup database correctly. KnownSitesDone has not completed");

                    //log
                    if (log == null)
                        log = await _sqlController.StartLog(this);

                    await log.LogCritical(t.GetMethodName("Core"), "###########################################################################");
                    await log.LogCritical(t.GetMethodName("Core"), "called");
                    await log.LogStandard(t.GetMethodName("Core"), "SqlController and Logger started");

                    //settings read
                    this._connectionString = connectionString;
                    _fileLocationPicture = await _sqlController.SettingRead(Settings.fileLocationPicture);
                    _fileLocationPdf = await _sqlController.SettingRead(Settings.fileLocationPdf);
                    await log.LogStandard(t.GetMethodName("Core"), "Settings read");

                    //communicators
                    string token = await _sqlController.SettingRead(Settings.token);
                    string comAddressApi = await _sqlController.SettingRead(Settings.comAddressApi);
                    string comAddressBasic = await _sqlController.SettingRead(Settings.comAddressBasic);
                    string comOrganizationId = await _sqlController.SettingRead(Settings.comOrganizationId);
                    string ComAddressPdfUpload = await _sqlController.SettingRead(Settings.comAddressPdfUpload);
                    string ComSpeechToText = await _sqlController.SettingRead(Settings.comSpeechToText);
                    _communicator = new Communicator(token, comAddressApi, comAddressBasic, comOrganizationId, ComAddressPdfUpload, log, ComSpeechToText);

                    _container = new WindsorContainer();
                    _container.Register(Component.For<SqlController>().Instance(_sqlController));
                    _container.Register(Component.For<Communicator>().Instance(_communicator));
                    _container.Register(Component.For<Log>().Instance(log));
                    _container.Register(Component.For<Core>().Instance(this));
                    
                    
                    try
                    {
                        _customerNo = await _sqlController.SettingRead(Settings.customerNo);
                    }
                    catch { }

                    try
				    {
				        _swiftEnabled = (_sqlController.SettingRead(Settings.swiftEnabled).Result.ToLower() == "true");

				    } catch {}

                    try
                    {
                        _s3Enabled = (_sqlController.SettingRead(Settings.s3Enabled).Result.ToLower() == "true");

                    } catch {}

				    if (_swiftEnabled)
				    {
				        _swiftUserName = await _sqlController.SettingRead(Settings.swiftUserName);
				        _swiftPassword = await _sqlController.SettingRead(Settings.swiftPassword);
				        _swiftEndpoint = await _sqlController.SettingRead(Settings.swiftEndPoint);
				        _keystoneEndpoint = await _sqlController.SettingRead(Settings.keystoneEndPoint);

				        _swiftClient = new SwiftClientService(
				            new SwiftClientConfig
				            {
				                AuthUrl = _keystoneEndpoint,
				                Domain = "",
				                Name = _swiftUserName,
				                ObjectStoreUrl = _swiftEndpoint,
				                Password = _swiftPassword
				            });
				        try
				        {
				            _swiftClient.AuthenticateAsync();
				        }
				        catch (Exception ex)
				        {
				            await log.LogWarning(t.GetMethodName("Core"), ex.Message);
				        }
				        
				        
				        _container.Register(Component.For<SwiftClientService>().Instance(_swiftClient));
				    }

                    if (_s3Enabled)
                    {
                        _s3AccessKeyId = await _sqlController.SettingRead(Settings.s3AccessKeyId);
                        _s3SecretAccessKey = await _sqlController.SettingRead(Settings.s3SecrectAccessKey);
                        _s3Endpoint = await _sqlController.SettingRead(Settings.s3Endpoint);

                        _s3Client = new AmazonS3Client(_s3AccessKeyId,_s3SecretAccessKey, RegionEndpoint.EUCentral1);
				        
                        _container.Register(Component.For<AmazonS3Client>().Instance(_s3Client));
                    }



                    await log.LogStandard(t.GetMethodName("Core"), "Communicator started");

                    await log.LogCritical(t.GetMethodName("Core"), "started");
                    _coreAvailable = true;
                    _coreStatChanging = false;
                }
            }
            #region catch
            catch (Exception ex)
            {
                _coreThreadRunning = false;
                _coreStatChanging = false;

                await FatalExpection(t.GetMethodName("Core") + " failed", ex);
                return false;
            }
            #endregion

            return true;
        }

        public override async Task Restart(int sameExceptionCount, int sameExceptionCountMax)
        {
            try
            {
                if (_coreRestarting == false)
                {
                    _coreRestarting = true;
                    await log.LogCritical(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(sameExceptionCount), sameExceptionCount);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(sameExceptionCountMax), sameExceptionCountMax);

                    _sameExceptionCountTried++;

                    if (_sameExceptionCountTried > sameExceptionCountMax)
                        _sameExceptionCountTried = sameExceptionCountMax;

                    if (_sameExceptionCountTried > 4)
                        throw new Exception("The same Exception repeated to many times (5+) within one hour");

                    int secondsDelay = 0;
                    switch (_sameExceptionCountTried)
                    {
                        case 1: secondsDelay = 030; break;
                        case 2: secondsDelay = 060; break;
                        case 3: secondsDelay = 120; break;
                        case 4: secondsDelay = 512; break;
                        default: throw new ArgumentOutOfRangeException("sameExceptionCount should be above 0");
                    }
                    await log.LogVariable(t.GetMethodName("Core"), nameof(_sameExceptionCountTried), _sameExceptionCountTried);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(secondsDelay), secondsDelay);

                    await Close();

                    await log.LogStandard(t.GetMethodName("Core"), "Trying to restart the Core in " + secondsDelay + " seconds");

                    if (!skipRestartDelay)
                        Thread.Sleep(secondsDelay * 1000);
                    else
                        await log.LogStandard(t.GetMethodName("Core"), "Delay skipped");

                    await Start(_connectionString);
                    _coreRestarting = false;
                }
            }
            catch (Exception ex)
            {
                await FatalExpection(t.GetMethodName("Core") + "failed. Core failed to restart", ex);
            }
        }

        /// <summary>
        /// Closes the Core and disables Events
        /// </summary>
        public async Task<bool> Close()
        {
            await log.LogStandard(t.GetMethodName("Core"), "Close called");
            try
            {
                if (_coreAvailable && !_coreStatChanging)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        _coreStatChanging = true;

                        _coreAvailable = false;
                        log.LogCritical(t.GetMethodName("Core"), "called").RunSynchronously();

                        try
                        {
                            if (_subscriber != null)
                            {
                                log.LogEverything(t.GetMethodName("Core"), "Subscriber requested to close connection").RunSynchronously();
                                _subscriber.Close().RunSynchronously();
                                log.LogEverything(t.GetMethodName("Core"), "Subscriber closed").RunSynchronously();
                                _bus.Advanced.Workers.SetNumberOfWorkers(0);
                                _bus.Dispose();
                                _coreThreadRunning = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.LogException(t.GetMethodName("Core"), "Subscriber failed to close", ex, false).RunSynchronously();
                        }

                        int tries = 0;
                        while (_coreThreadRunning)
                        {
                            Thread.Sleep(100);
                            tries++;

                            if (tries > 600)
                                FatalExpection("Failed to close Core correct after 60 secs", new Exception()).RunSynchronously();
                        }

                        _updateIsRunningEntities = false;

                        log.LogStandard(t.GetMethodName("Core"), "Core closed").RunSynchronously();
                        _subscriber = null;
                        _communicator = null;
                        _sqlController = null;
                        _bus = null;

                        _coreStatChanging = false;
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
                FatalExpection(t.GetMethodName("Core") + " failed. Core failed to close", ex).RunSynchronously();
            }
            return true;
        }

        /// <summary>
        /// Tells if the Core is useable
        /// </summary>
        public bool Running()
        {
            return _coreAvailable;
        }

        public async Task FatalExpection(string reason, Exception exception)
        {
            _coreAvailable = false;
            _coreThreadRunning = false;
            _coreStatChanging = false;
            _coreRestarting = false;

            try
            {
                await log.LogFatalException(t.GetMethodName("Core") + " called for reason:'" + reason + "'", exception);
            }
            catch { }

            try { HandleEventException?.Invoke(exception, EventArgs.Empty); } catch { }
            throw new Exception("FATAL exception, Core shutting down, due to:'" + reason + "'", exception);
        }
        #endregion

        #region public actions
        #region template

        /// <summary>
        /// Converts XML from ex. eForm Builder or other sources, into a MainElement
        /// </summary>
        /// <param name="xmlString">XML string to be converted</param>
        public async Task<MainElement> TemplateFromXml(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
                throw new ArgumentNullException("xmlString cannot be null or empty");
            string methodName = t.GetMethodName("Core");
            try
            {
                await log.LogStandard(t.GetMethodName("Core"), "called");
                await log.LogEverything(t.GetMethodName("Core"), "XML to transform:");
                await log.LogEverything(t.GetMethodName("Core"), xmlString);

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

                await log.LogEverything(t.GetMethodName("Core"), "XML after possible corrections:");
                await log.LogEverything(t.GetMethodName("Core"), xmlString);

                MainElement mainElement = new MainElement();
                mainElement = mainElement.XmlToClass(xmlString);

                //XML HACK
                mainElement.CaseType = "";
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                if (mainElement.Repeated < 1)
                {
                    await log.LogCritical(t.GetMethodName("Core"), "mainElement.Repeated = 1 // enforced");
                    mainElement.Repeated = 1;
                }

                return mainElement;
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
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

        public async Task<List<string>> TemplateValidation(MainElement mainElement)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    if (mainElement == null)
                        throw new ArgumentNullException(nameof(mainElement), "mainElement not allowed to be null");

                    List<string> errorList = await FieldValidation(mainElement);
                    errorList = errorList.Concat(await CheckListValidation(mainElement)).ToList();

                    return errorList;

                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        private async Task<List<string>> FieldValidation(MainElement mainElement)
        {

            await log.LogStandard(t.GetMethodName("Core"), "called");

            List<string> errorLst = new List<string>();
            var dataItems = mainElement.DataItemGetAll();

            foreach (var dataItem in dataItems)
            {
                #region entities

                if (dataItem.GetType() == typeof(EntitySearch))
                {
                    EntitySearch entitySearch = (EntitySearch) dataItem;
                    var temp = _sqlController.EntityGroupRead(entitySearch.EntityTypeId.ToString());
                    if (temp == null)
                        errorLst.Add("Element entitySearch.EntityTypeId:'" + entitySearch.EntityTypeId +
                                     "' is an reference to a local unknown EntitySearch group. Please update reference");
                }

                if (dataItem.GetType() == typeof(EntitySelect))
                {
                    EntitySelect entitySelect = (EntitySelect) dataItem;
                    var temp = _sqlController.EntityGroupRead(entitySelect.Source.ToString());
                    if (temp == null)
                        errorLst.Add("Element entitySelect.Source:'" + entitySelect.Source +
                                     "' is an reference to a local unknown EntitySearch group. Please update reference");
                }

                #endregion

                #region PDF

                if (dataItem.GetType() == typeof(ShowPdf))
                {
                    ShowPdf showPdf = (ShowPdf) dataItem;
                    errorLst.AddRange(await PdfValidate(showPdf.Value, showPdf.Id));
                }
                
                List<string> acceptedColors = new List<string>();
                acceptedColors.Add(Constants.FieldColors.Grey);
                acceptedColors.Add(Constants.FieldColors.Red);
                acceptedColors.Add(Constants.FieldColors.Green);
                acceptedColors.Add(Constants.FieldColors.Blue);
                acceptedColors.Add(Constants.FieldColors.Purple);
                acceptedColors.Add(Constants.FieldColors.Yellow);
                acceptedColors.Add(Constants.FieldColors.Default);
                acceptedColors.Add(Constants.FieldColors.None);
                if (!acceptedColors.Contains(dataItem.Color) && !string.IsNullOrEmpty(dataItem.Color))
                {
                    errorLst.Add($"DataItem with label {dataItem.Label} did supply color {dataItem.Color}, but the only allowed values are: e8eaf6 for grey, ffe4e4 for red, f0f8db for green, e2f4fb for blue, e2f4fb for purple, fff6df for yellow, None for default or leave it blank.");
                }

                #endregion
            }

            return errorLst;

        }

        private async Task<List<string>> CheckListValidation(MainElement mainElement)
        {
            await log.LogStandard(t.GetMethodName("Core"), "called");
            List<string> errorLst = new List<string>();
            
            List<string> acceptedColors = new List<string>();
            acceptedColors.Add(Constants.CheckListColors.Grey);
            acceptedColors.Add(Constants.CheckListColors.Red);
            acceptedColors.Add(Constants.CheckListColors.Green);

            if (!acceptedColors.Contains(mainElement.Color) && !string.IsNullOrEmpty(mainElement.Color))
            {
                errorLst.Add($"mainElement with label {mainElement.Label} did supply color {mainElement.Color}, but the only allowed colors are: grey, red, green or leave it blank.");
            }
            
            return errorLst;
        }

        public async Task<MainElement> TemplateUploadData(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException(nameof(mainElement), "mainElement not allowed to be null");

            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");

                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach (var dataItem in dataItems)
                    {
                        #region PDF
                        if (dataItem.GetType() == typeof(ShowPdf))
                        {
                            ShowPdf showPdf = (ShowPdf)dataItem;

                            if (PdfValidate(showPdf.Value, showPdf.Id).Result.Count != 0)
                            {
                                try
                                {
                                    //download file
                                    string downloadPath = await _sqlController.SettingRead(Settings.fileLocationPdf);
                                    long ticks = DateTime.Now.Ticks;
                                    string tempFileName = $"{ticks}_temp.pdf";
                                    string filePathAndFileName = Path.Combine(downloadPath, tempFileName);
                                    try
                                    {
//                                        (new FileInfo(downloadPath)).Directory.Create();
                                        Directory.CreateDirectory(downloadPath);

                                        using (WebClient client = new WebClient())
                                        {
                                            client.DownloadFile(showPdf.Value, filePathAndFileName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Download failed. Path:'" + showPdf.Value + "'", ex);
                                    }

                                    //upload file
                                    string hash = await PdfUpload(filePathAndFileName);
                                    if (hash != null)
                                    {
                                        //rename local file
                                        FileInfo FileInfo = new FileInfo(filePathAndFileName);

                                        FileInfo.CopyTo(downloadPath + hash + ".pdf", true);
                                        FileInfo.Delete();

                                        await PutFileToStorageSystem(Path.Combine(downloadPath, $"{hash}.pdf"), $"{hash}.pdf");

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
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to create an eForm template in the Microting local DB. Returns that templat's templatId
        /// </summary>
        /// <param name="mainElement">Templat MainElement to be inserted into the Microting local DB</param>
        public async Task<int> TemplateCreate(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException(nameof(mainElement), "mainElement not allowed to be null");

            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");

                    List<string> errors = await TemplateValidation(mainElement);

                    if (errors == null) errors = new List<string>();
                    if (errors.Count > 0)
                        throw new Exception("mainElement failed TemplateValidation. Run TemplateValidation to see errors");

                    int templateId = await _sqlController.TemplateCreate(mainElement);
                    await log.LogEverything(t.GetMethodName("Core"), "Template id:" + templateId.ToString() + " created in DB");
                    return templateId;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve the template MainElement from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        public async Task<MainElement> TemplateRead(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return await _sqlController.TemplateRead(templateId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to delete an eForm template in the Microting local DB. Returns if it was successfully
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        public async Task<bool> TemplateDelete(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return await _sqlController.TemplateDelete(templateId);                    
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve the template meta data from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        public async Task<Template_Dto> TemplateItemRead(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return await _sqlController.TemplateItemRead(templateId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    await log.LogException(t.GetMethodName("Core"), "(int " + templateId.ToString() + ") failed", ex, false);
                }
                catch
                {
                    await log.LogException(t.GetMethodName("Core"), "(int templateId) failed", ex, false);
                }
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve all templates meta data from the Microting DB
        /// </summary>
        /// <param name="includeRemoved">Filters list to only show all active or all including removed</param>
        public async Task<List<Template_Dto>> TemplateItemReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(includeRemoved), includeRemoved);

                    return await TemplateItemReadAll(includeRemoved, Constants.WorkflowStates.Created, "", true, "", new List<int>());
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    await log.LogException(t.GetMethodName("Core"), "(bool " + includeRemoved.ToString() + ") failed", ex, false);
                }
                catch
                {
                    await log.LogException(t.GetMethodName("Core"), "(bool includeRemoved) failed", ex, false);
                }
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<Template_Dto>> TemplateItemReadAll(bool includeRemoved, string siteWorkflowState, string searchKey, bool descendingSort, string sortParameter, List<int> tagIds)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(includeRemoved), includeRemoved);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(searchKey), searchKey);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(descendingSort), descendingSort);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(sortParameter), sortParameter);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(tagIds), tagIds.ToString());

                    return await _sqlController.TemplateItemReadAll(includeRemoved, siteWorkflowState, searchKey, descendingSort, sortParameter, tagIds);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> TemplateSetTags(int templateId, List<int> tagIds)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(tagIds), tagIds.ToString());

                    return await _sqlController.TemplateSetTags(templateId, tagIds);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
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
        public async Task<int?> CaseCreate(MainElement mainElement, string caseUId, int siteUid)
        {
            List<int> siteUids = new List<int>();
            siteUids.Add(siteUid);
            List<int> lst = await CaseCreate(mainElement, caseUId, siteUids, "");

            try
            {
                return lst[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Tries to create an eForm case(s) in the Microting local DB, and creates it in the Microting system, with extended parameters
        /// </summary>
        /// <param name="mainElement">The templat MainElement the case(s) will be based on</param>
        /// <param name="caseUId">NEEDS TO BE UNIQUE IF ASSIGNED. The unique identifier that you can assign yourself to the set of case(s). If used (not blank or null), the cases are connected. Meaning that if one is completed, all in the set is retracted. If you wish to use caseUId and not have the cases connected, use this method multiple times, each with a unique caseUId</param>
        /// <param name="siteIds">List of siteIds that case(s) will be sent to</param>
        /// <param name="custom">Custom extended parameter</param>
        public async Task<List<int>> CaseCreate(MainElement mainElement, string caseUId, List<int> siteUids, string custom)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
//                    lock (_lockMain) //Will let sending Cases sending finish, before closing
//                    {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    string siteIdsStr = string.Join(",", siteUids);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(caseUId), caseUId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteIdsStr), siteIdsStr);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(custom), custom);

                    #region check input
                    DateTime start = DateTime.Parse(mainElement.StartDate.ToShortDateString());
                    DateTime end = DateTime.Parse(mainElement.EndDate.ToShortDateString());

                    if (end < DateTime.Now)
                    {
                        await log.LogStandard(t.GetMethodName("Core"), "mainElement.EndDate is set to " + end);
                        throw new ArgumentException("mainElement.EndDate needs to be a future date");
                    }

                    if (end <= start)
                    {
                        await log.LogStandard(t.GetMethodName("Core"), "mainElement.StartDat is set to " + start);
                        throw new ArgumentException("mainElement.StartDate needs to be at least the day, before the remove date (mainElement.EndDate)");
                    }

                    if (caseUId != "" && mainElement.Repeated != 1)
                        throw new ArgumentException("if caseUId can only be used for mainElement.Repeated == 1");
                    #endregion

                    //sending and getting a reply
                    List<int> lstMUId = new List<int>();

                    foreach (int siteUid in siteUids)
                    {
                        int mUId = await SendXml(mainElement, siteUid);

                        if (mainElement.Repeated == 1)
                            await _sqlController.CaseCreate(mainElement.Id, siteUid, mUId, null, caseUId, custom, DateTime.Now);
                        else
                            await _sqlController.CheckListSitesCreate(mainElement.Id, siteUid, mUId);

                        Case_Dto cDto = await _sqlController.CaseReadByMUId(mUId);
                        //InteractionCaseUpdate(cDto);
                        try { HandleCaseCreated?.Invoke(cDto, EventArgs.Empty); }
                        catch { await log.LogWarning(t.GetMethodName("Core"), "HandleCaseCreated event's external logic suffered an Expection"); }
                        await log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been created");

                        lstMUId.Add(mUId);
                    }
                    return lstMUId;
//                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Tries to retrieve the status of a case
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        public async Task<string> CaseCheck(int microtingUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);

                    Case_Dto cDto = await CaseLookupMUId(microtingUId);
                    return await _communicator.CheckStatus(cDto.MicrotingUId.ToString(), cDto.SiteUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Tries to retrieve the answered full case from the DB
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        /// <param name="checkUId">If left empty, "0" or NULL it will try to retrieve the first check. Alternative is stating the Id of the specific check wanted to retrieve</param>
        public async Task<ReplyElement> CaseRead(int microtingUId, int checkUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(checkUId), checkUId);
//
//                    if (checkUId == null)
//                        checkUId = "";
//
//                    if (checkUId == "" || checkUId == "0")
//                        checkUId = null;

                    cases aCase = await _sqlController.CaseReadFull(microtingUId, checkUId);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        await log.LogWarning(t.GetMethodName("Core"), "No case found with MuuId:'" + microtingUId + "'");
                        return null;
                    }
                    #endregion

                    int id = aCase.Id;
                    await log.LogEverything(t.GetMethodName("Core"), "aCase.Id:" + aCase.Id.ToString() + ", found");

                    ReplyElement replyElement = await _sqlController.CheckRead(microtingUId, checkUId);
                    return replyElement;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public async Task<Case_Dto> CaseReadByCaseId(int id)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(id), id);

                    return await _sqlController.CaseReadByCaseId(id);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public async Task<int?> CaseReadFirstId(int? templateId, string workflowState)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    return await _sqlController.CaseReadFirstId(templateId, workflowState);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }
        
        public async Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end)
        {
            return await CaseReadAll(templateId, start, end, Constants.WorkflowStates.NotRemoved, null);
        }

        public async Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, string searchKey)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(start), start);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(end), end);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    return await CaseReadAll(templateId, start, end, workflowState, searchKey, false, null);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public async Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, string searchKey, bool descendingSort, string sortParameter)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(start), start);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(end), end);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(descendingSort), descendingSort);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(sortParameter), sortParameter);

                    return await _sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey, descendingSort, sortParameter);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public async Task<CaseList> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, 
            string searchKey, bool descendingSort, string sortParameter, int pageIndex, int pageSize)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(start), start);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(end), end);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(descendingSort), descendingSort);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(sortParameter), sortParameter);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(pageIndex), pageIndex);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(pageSize), pageSize);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(searchKey), searchKey);

                    return await _sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey, descendingSort, sortParameter, pageIndex, pageSize);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Tries to set the resultats of a case to new values
        /// </summary>
        /// <param name="newFieldValuePairLst">List of '[fieldValueId]|[new value]'</param>
        /// <param name="newCheckListValuePairLst">List of '[checkListValueId]|[new status]'</param>
        public async Task<bool> CaseUpdate(int caseId, List<string> newFieldValuePairLst, List<string> newCheckListValuePairLst)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);

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
                        await _sqlController.FieldValueUpdate(caseId, id, value);
                    }

                    foreach (string str in newCheckListValuePairLst)
                    {
                        id = int.Parse(t.SplitToList(str, 0, false));
                        value = t.SplitToList(str, 1, false);
                        await _sqlController.CheckListValueStatusUpdate(caseId, id, value);
                    }

                    return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return false;
            }
        }

        public async Task<bool> CaseDelete(int templateId, int siteUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteUId), siteUId);

                    return await CaseDelete(templateId, siteUId, "");
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    await log.LogException(t.GetMethodName("Core"), "(int " + templateId.ToString() + ", int " + siteUId.ToString() + ") failed", ex, false);
                }
                catch
                {
                    await log.LogException(t.GetMethodName("Core"), "(int templateId, int siteUId) failed", ex, false);
                }
                return false;
            }
        }

        public async Task<bool> CaseDelete(int templateId, int siteUId, string workflowState)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteUId), siteUId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    List<string> errors = new List<string>();
                    foreach (int microtingUId in await _sqlController.CheckListSitesRead(templateId, siteUId, workflowState))
                    {
                        if (! await CaseDelete(microtingUId))
                        {
                            string error = $"Failed to delete case with microtingUId: {microtingUId}";
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
                    await log.LogException(t.GetMethodName("Core"), "(int " + templateId.ToString() + ", int " + siteUId.ToString() + ", string " + workflowState + ") failed", ex, false);
                }
                catch
                {
                    await log.LogException(t.GetMethodName("Core"), "(int templateId, int siteUId, string workflowState) failed", ex, false);
                }
                return false;
            }
        }

        /// <summary>
        /// Marks a case as deleted, and will remove it from the device, if needed
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        public async Task<bool> CaseDelete(int microtingUId)
        {
            //bus.SendLocal(new EformDeleteFromServer(microtingUId)).Wait();
            //bus.SendLocal(new EformCompleted(notificationUId, microtingUId)).Wait();

            //string microtingUId = message.MicrotringUUID;
            string methodName = t.GetMethodName("Core");

            //await log.LogStandard(t.GetMethodName("Core"), "called");
            await log.LogVariable(methodName, nameof(microtingUId), microtingUId);

            var cDto = await _sqlController.CaseReadByMUId(microtingUId);
            string xmlResponse = await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId);
            await log.LogEverything(methodName, "XML response is 1218 : " + xmlResponse);
            Response resp = new Response();

            if (xmlResponse.Contains("Error occured: Contact Microting"))
            {
                await log.LogEverything(methodName, "XML response is : " + xmlResponse);
                await log.LogEverything("DELETE ERROR", "failed for microtingUId: " + microtingUId);
                return false;
            }

            if (xmlResponse.Contains("Error"))
            {
                try
                {
                    resp = resp.XmlToClass(xmlResponse);
                    await log.LogException(methodName, "failed", new Exception("Error from Microting server: " + resp.Value), false);
                    return false;
                }
                catch (Exception ex)
                {
                    try
                    {
                        await log.LogException(t.GetMethodName("Core"), "(string " + microtingUId + ") failed", ex, false);
                        throw ex;
                    }
                    catch
                    {
                        await log.LogException(t.GetMethodName("Core"), "(string microtingUId) failed", ex, false);
                        throw ex;
                    }
                }
            }

            if (xmlResponse.Contains("Parsing in progress: Can not delete check list!"))
                for (int i = 1; i < 102; i++)
                {
                    Thread.Sleep(i * 5000);
                    xmlResponse = await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId);
                    try
                    {
                        resp = resp.XmlToClass(xmlResponse);
                        if (resp.Type.ToString() == "Success")
                        {
                            await log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + $" has been removed from server in retry loop with i being : {i.ToString()}");
                            break;
                        }                            
                        else
                        {
                            await log.LogEverything(t.GetMethodName("Core"), $"retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                        }
                    } catch (Exception ex)
                    {
                        await log.LogEverything(t.GetMethodName("Core"), $" Exception is: {ex.Message}, retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                    }
                    //if (!xmlResponse.Contains("Parsing in progress: Can not delete check list!"))
                    //{
                    //    break;
                    //}
                    //else
                    //{
                    //    await log.LogEverything(t.GetMethodName("Core"), $"retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                    //}
                }

            await log.LogEverything(t.GetMethodName("Core"), "XML response:");
            await log.LogEverything(t.GetMethodName("Core"), xmlResponse);

            resp = resp.XmlToClass(xmlResponse);
            if (resp.Type.ToString() == "Success")
            {
                await log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been removed from server");
                try
                {
                    if (await _sqlController.CaseDelete(microtingUId))
                    {
                        cDto = await _sqlController.CaseReadByMUId(microtingUId);
                        await FireHandleCaseDeleted(cDto);

                        await log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been removed");

                        return true;
                    } else
                    {
                        try
                        {
                            await _sqlController.CaseDeleteReversed(microtingUId);

                            cDto = await _sqlController.CaseReadByMUId(microtingUId);
                            await FireHandleCaseDeleted(cDto);
                            await log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been removed");

                            return true;
                        }
                        catch (Exception ex)
                        {
                            await log.LogException(t.GetMethodName("Core"), "(string microtingUId) failed", ex, false);
                            throw ex;
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    await log.LogException(t.GetMethodName("Core"), "(string microtingUId) failed", ex, false);
                }

                
            }
            return false;
            //return true;
        }

        public async Task<bool> CaseDeleteResult(int caseId)
        {
            string methodName = t.GetMethodName("Core");
            await log.LogStandard(t.GetMethodName("Core"), "called");
            await log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);
            try
            {
                return await _sqlController.CaseDeleteResult(caseId);
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                try
                {
                    await log.LogException(t.GetMethodName("Core"), "(int " + caseId.ToString() + ") failed", ex, false);
                }
                catch
                {
                    await log.LogException(t.GetMethodName("Core"), "(int caseId) failed", ex, false);
                }

                return false;
            }
        }

        public async Task<bool> CaseUpdateFieldValues(int id)
        {
            string methodName = t.GetMethodName("Core");
            await log.LogStandard(t.GetMethodName("Core"), "called");
            await log.LogVariable(t.GetMethodName("Core"), nameof(id), id);
            try
            {
                return await _sqlController.CaseUpdateFieldValues(id);
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return false;
            }
        }

        public async Task<Case_Dto> CaseLookup(int microtingUId, int checkUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(checkUId), checkUId);

                    return await _sqlController.CaseLookup(microtingUId, checkUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Looks up the case's markers, from the match
        /// </summary>
        /// <param name="microtingUId">Microting unique ID of the eForm case</param>
        public async Task<Case_Dto> CaseLookupMUId(int microtingUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);

                    return await _sqlController.CaseReadByMUId(microtingUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Looks up the case's markers, from the match
        /// </summary>
        /// <param name="CaseId">Microting DB's ID of the eForm case</param>
        public async Task<Case_Dto> CaseLookupCaseId(int caseId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);

                    return await _sqlController.CaseReadByCaseId(caseId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Looks up the case's markers, from the matches
        /// </summary>
        /// <param name="caseUId">Case's unique ID of the set of case(s)</param>
        public async Task<List<Case_Dto>> CaseLookupCaseUId(string caseUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(caseUId), caseUId);

                    return await _sqlController.CaseReadByCaseUId(caseUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Looks up the case's ID, from the match
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        /// <param name="checkUId">If left empty, "0" or NULL it will try to retrieve the first check. Alternative is stating the Id of the specific check wanted to retrieve</param>
        public async Task<int?> CaseIdLookup(int microtingUId, int checkUId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUId), microtingUId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(checkUId), checkUId);

//                    if (checkUId == null)
//                        checkUId = "";
//
//                    if (checkUId == "" || checkUId == "0")
//                        checkUId = null;

                    cases aCase = await _sqlController.CaseReadFull(microtingUId, checkUId);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        await log.LogWarning(t.GetMethodName("Core"), "No case found with MuuId:'" + microtingUId + "'");
                        return -1;
                    }
                    #endregion
                    int id = aCase.Id;
                    await log.LogEverything(t.GetMethodName("Core"), "aCase.Id:" + aCase.Id.ToString() + ", found");

                    return id;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Tries to retrieve all connected cases to a templat, and delivers them as a CSV fil, at the returned path's location
        /// </summary>
        /// <param name="templateId">The templat's ID to be used. Null will remove this limit</param>
        /// <param name="start">Only cases from after this time limit. Null will remove this limit</param>
        /// <param name="end">Only cases from before this time limit. Null will remove this limit</param>
        /// <param name="pathAndName">Location where fil is to be placed, along with fil name. No extension needed. Relative or absolut</param>
        public async Task<string> CasesToCsv(int templateId, DateTime? start, DateTime? end, string pathAndName,
            string customPathForUploadedData, string decimalSeparator, string thousandSeparator)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(start), start.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(end), end.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(pathAndName), pathAndName);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(customPathForUploadedData), customPathForUploadedData);

                    List<List<string>> dataSet = await GenerateDataSetFromCases(templateId, start, end, customPathForUploadedData, decimalSeparator, thousandSeparator);

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
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        /// <summary>
        /// Tries to retrieve all connected cases to a templat, and delivers them as a CSV fil, at the returned path's location
        /// </summary>
        /// <param name="templateId">The templat's ID to be used. Null will remove this limit</param>
        /// <param name="start">Only cases from after this time limit. Null will remove this limit</param>
        /// <param name="end">Only cases from before this time limit. Null will remove this limit</param>
        /// <param name="pathAndName">Location where fil is to be placed, along with fil name. No extension needed. Relative or absolut</param>
        public async Task<string> CasesToCsv(int templateId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData)
        {
            return await CasesToCsv(templateId, start, end, pathAndName, customPathForUploadedData, ".", "");
        }

        public async Task<string> CaseToJasperXml(Case_Dto cDto, ReplyElement reply, int caseId, string timeStamp, string customPathForUploadedData, string customXMLContent)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                //if (coreRunning)
                if (true)
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(timeStamp), timeStamp);

                    if (timeStamp == null)
                        timeStamp = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss");

                    //get needed data
//                    Case_Dto cDto = CaseLookupCaseId(caseId);
//                    ReplyElement reply = CaseRead(cDto.MicrotingUId, cDto.CheckUId);
                    if (reply == null)
                        throw new NullReferenceException("reply is null. Delete or fix the case with ID " + caseId.ToString());
                    string clsLst = "";
                    string fldLst = "";
                    GetChecksAndFields(ref clsLst, ref fldLst, reply.ElementList, customPathForUploadedData);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(clsLst), clsLst);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fldLst), fldLst);

                    #region convert to jasperXml
                    string jasperXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + Environment.NewLine + "<root>"
                        + Environment.NewLine + "<C" + reply.Id + " case_id=\"" + caseId + "\" case_name=\"" + reply.Label + "\" serial_number=\"" + caseId + "/" + cDto.MicrotingUId + "\" check_list_status=\"approved\">"
                        + Environment.NewLine + "<worker>" + Advanced_WorkerNameRead(reply.DoneById) + "</worker>"
                        + Environment.NewLine + "<check_id>" + reply.MicrotingUId + "</check_id>"
                        + Environment.NewLine + "<date>" + reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss") + "</date>"
                        + Environment.NewLine + "<check_date>" + reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss") + "</check_date>"
                        + Environment.NewLine + "<site_name>" + Advanced_SiteItemRead(reply.SiteMicrotingUuid).Result.SiteName + "</site_name>"
                        + Environment.NewLine + "<check_lists>"

                        + clsLst

                        + Environment.NewLine + "</check_lists>"
                        + Environment.NewLine + "<fields>"

                        + fldLst

                        + Environment.NewLine + "</fields>"
                        + Environment.NewLine + "</C" + reply.Id + ">"
                        + customXMLContent
                        + Environment.NewLine + "</root>";
                    await log.LogVariable(t.GetMethodName("Core"), nameof(jasperXml), jasperXml);
                    #endregion

                    //place in settings allocated placement
                    string fullPath = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results", timeStamp + "_" + caseId + ".xml");
                    //string path = sqlController.SettingRead(Settings.fileLocationJasper) + "results/" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss") + "_" + caseId + ".xml";
                    string path = await _sqlController.SettingRead(Settings.fileLocationJasper);
                    Directory.CreateDirectory(Path.Combine(path, "results"));
                    File.WriteAllText(fullPath, jasperXml.Trim(), Encoding.UTF8);

                    //string path = Path.GetFullPath(locaR);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fullPath), fullPath);
                    return fullPath;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        #region sdkSettings
        
        public async Task<string> GetSdkSetting(Settings settingName)
        {
            string methodName = t.GetMethodName("Core");
            await log.LogStandard(t.GetMethodName("Core"), "called");
            try
            {
                return await _sqlController.SettingRead(settingName);
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return "N/A";
            }
        }

        public async Task<bool> SetSdkSetting(Settings settingName, string settingValue)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(settingValue), settingValue);

                    await _sqlController.SettingUpdate(settingName, settingValue);
                    return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        public async Task SetJasperExportEnabled(int eFormId, bool isEnabled)
        {
            if (Running())
            {
                await _sqlController.SetJasperExportEnabled(eFormId, isEnabled);
            }
        }

        public async Task SetDocxExportEnabled(int eFormId, bool isEnabled)
        {
            if (Running())
            {
                await _sqlController.SetDocxExportEnabled(eFormId, isEnabled);
            }
        }
        
        public async Task<string> CaseToPdf(int caseId, string jasperTemplate, string timeStamp, string customPathForUploadedData, string customXmlContent)
        {
            return await CaseToPdf(caseId, jasperTemplate, timeStamp, customPathForUploadedData, "pdf", customXmlContent);
        }

        private async Task<string> JasperToPdf(int caseId, string jasperTemplate, string timeStamp)
        {
            #region run jar
            // Start the child process.
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
//            string exePath = AppDomain.CurrentDomain.BaseDirectory;
//            await log.LogStandard(t.GetMethodName("Core"), "exePath is " + exePath);
            string localJasperExporter = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "utils",
                "JasperExporter.jar");
            if (!File.Exists(localJasperExporter))
            {
                using (WebClient webClient = new WebClient())
                {
                    Directory.CreateDirectory(Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "utils"));
                    webClient.DownloadFile("https://github.com/microting/JasperExporter/releases/download/stable/JasperExporter.jar", localJasperExporter);
                }
            }

            string _templateFile = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "templates", jasperTemplate, "compact",
                $"{jasperTemplate}.jrxml");                    
            if (!File.Exists(_templateFile))
            {
                throw new FileNotFoundException($"jrxml template was not found at {_templateFile}");
            }
            string _dataSourceXML = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results",
                $"{timeStamp}_{caseId}.xml");

            if (!File.Exists(_dataSourceXML))
            {
                throw new FileNotFoundException("Case result xml was not found at " + _dataSourceXML);
            }
            string _resultDocument = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results",
                $"{timeStamp}_{caseId}.pdf");

            string command =
                $"-d64 -Xms512m -Xmx2g -Dfile.encoding=UTF-8 -jar {localJasperExporter} -template=\"{_templateFile}\" -type=\"pdf\" -uri=\"{_dataSourceXML}\" -outputFile=\"{_resultDocument}\"";

            await log.LogVariable(t.GetMethodName("Core"), nameof(command), command);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                p.StartInfo.FileName = "java.exe";
            }
            else
            {
                p.StartInfo.FileName = "java";
            }

            p.StartInfo.Arguments = command;
            p.StartInfo.Verb = "runas";
            p.Start();
            // IF needed:
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            await log.LogVariable(t.GetMethodName("Core"), nameof(output), output);
            p.WaitForExit();

            if (output != "")
                throw new Exception("output='" + output + "', expected to be no output. This indicates an error has happened");
            #endregion

            return _resultDocument;
        }

        private async Task<string> DocxToPdf(int caseId, string jasperTemplate, string timeStamp, ReplyElement reply, Case_Dto cDto, string customPathForUploadedData, string customXmlContent, string fileType)
        {
            
            SortedDictionary<string, string> valuePairs = new SortedDictionary<string, string>();
            // get base values
            valuePairs.Add("F_CaseName", reply.Label);
            valuePairs.Add("F_SerialNumber", $"{caseId}/{cDto.MicrotingUId}");
            valuePairs.Add("F_Worker", await _sqlController.WorkerNameRead(reply.DoneById));
            valuePairs.Add("F_CheckId", reply.MicrotingUId.ToString());
            valuePairs.Add("F_CheckDate", reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss"));
            valuePairs.Add("F_SiteName", _sqlController.SiteRead(reply.SiteMicrotingUuid).Result.SiteName);
            
            // get field_values
            List<KeyValuePair<string, string>> pictures = new List<KeyValuePair<string, string>>();
            List<int> caseIds = new List<int>();
            caseIds.Add(caseId);
            List<FieldValue> fieldValues = await _sqlController.FieldValueReadList(caseIds);

            List<Field_Dto> allFields = await _sqlController.TemplateFieldReadAll(int.Parse(jasperTemplate));
            foreach (Field_Dto field in allFields)
            {
                valuePairs.Add($"F_{field.Id}", "");
            }
            
            SortedDictionary<string, int> imageFieldCountList = new SortedDictionary<string, int>();
            foreach (FieldValue fieldValue in fieldValues)
            {
                switch (fieldValue.FieldType)
                {
                    case Constants.FieldTypes.MultiSelect:
                        valuePairs[$"F_{fieldValue.FieldId}"] =
                            fieldValue.ValueReadable.Replace("|", @"</w:t><w:br/><w:t>");
                        break;
                    
                    case Constants.FieldTypes.Picture:
                        imageFieldCountList[$"FCount_{fieldValue.FieldId}"] = 0;
                        if (fieldValue.UploadedDataObj != null)
                        {
                            fields field = await _sqlController.FieldReadRaw(fieldValue.FieldId);
                            check_lists checkList = await _sqlController.CheckListRead((int)field.CheckListId);
                        
                            pictures.Add(new KeyValuePair<string, string>($"{checkList.Label} - {field.Label}", fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName));
                            SwiftObjectGetResponse swiftObjectGetResponse = GetFileFromSwiftStorage(fieldValue.UploadedDataObj.FileName).Result;
                            var fileStream =
                                File.Create(fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName);
                            swiftObjectGetResponse.ObjectStreamContent.Seek(0, SeekOrigin.Begin);
                            swiftObjectGetResponse.ObjectStreamContent.CopyTo(fileStream);
                            fileStream.Close();    
                            if (imageFieldCountList.ContainsKey($"FCount_{fieldValue.FieldId}"))
                            {
                                imageFieldCountList[$"FCount_{fieldValue.FieldId}"] += 1;
                            }
                        }
                        break;
                    case Constants.FieldTypes.CheckBox:
                        // TODO change this to use Winding 0252 = checkmark
                        valuePairs[$"F_{fieldValue.FieldId}"] = fieldValue.ValueReadable.ToLower() == "checked" ? "&#10004;" : "";
                        break;
                    default:
                        if (fieldValue.ValueReadable == "null")
                        {
                            valuePairs[$"F_{fieldValue.FieldId}"] = "";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(fieldValue.ValueReadable))
                            {
                                valuePairs[$"F_{fieldValue.FieldId}"] = "";
                            }
                            else
                            {
                                fieldValue.ValueReadable = fieldValue.ValueReadable.Replace("<br>", "|||");
                                fieldValue.ValueReadable = Regex.Replace(fieldValue.ValueReadable, "<.*?>", 
                                    string.Empty);
                                fieldValue.ValueReadable =
                                    fieldValue.ValueReadable.Replace("|||", @"</w:t><w:br/><w:t>");
                                valuePairs[$"F_{fieldValue.FieldId}"] = fieldValue.ValueReadable;
                            }
                        }
                        break;
                }
            }

            foreach (KeyValuePair<string,int> keyValuePair in imageFieldCountList)
            {
                valuePairs.Add(keyValuePair.Key, keyValuePair.Value.ToString());
            }
            
            // get check_list_values
            List<CheckListValue> checkListValues = await _sqlController.CheckListValueReadList(caseIds);
            foreach (CheckListValue checkListValue in checkListValues)
            {
                valuePairs.Add($"C_{checkListValue.Id}", checkListValue.Status);
            }
            // TODO get custom xml values

            if (!string.IsNullOrEmpty(customXmlContent))
            {
                foreach (KeyValuePair<string,string> keyValuePair in ParseCustomXmlContent(customXmlContent))
                {
                    valuePairs.Add(keyValuePair.Key, keyValuePair.Value);
                }    
            }

            string templateFile = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "templates", jasperTemplate, "compact",
                $"{jasperTemplate}.docx");  
            
            // Try to create the results directory first
            Directory.CreateDirectory(Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results"));
            
            string resultDocument = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results",
                $"{timeStamp}_{caseId}.docx");

            ReportHelper.SearchAndReplace(templateFile, valuePairs, resultDocument);
            
            // TODO insert images
            ReportHelper.InsertImages(resultDocument, pictures);
            
            if (fileType == "pdf")
            {
                string outputFolder = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results");
            
                ReportHelper.ConvertToPdf(resultDocument, outputFolder);
                return Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results",
                    $"{timeStamp}_{caseId}.pdf");    
            }
            else
            {
                return Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper), "results",
                    $"{timeStamp}_{caseId}.docx");
            }
            
        }

        private Dictionary<string, string> ParseCustomXmlContent(string customXmlContent)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(customXmlContent);
            XmlElement root = xmlDocument.DocumentElement;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            foreach (XmlNode node in root.ChildNodes)
            {
                dictionary.Add($"F_{node.Name}", node.InnerText);
            }

            return dictionary;
        }

        public async Task<string> CaseToPdf(int caseId, string jasperTemplate, string timeStamp, string customPathForUploadedData, string fileType, string customXmlContent)
        {
            if (fileType != "pdf" && fileType != "docx" && fileType != "pptx")
            {
                throw new ArgumentException($"Filetypes allowed are only: pdf, docx, pptx, currently specified was {fileType}");    
            }            
            
            string methodName = t.GetMethodName("Core");
            try
            {
                //if (coreRunning)
                if (true)
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(jasperTemplate), jasperTemplate);

                    if (timeStamp == null)
                        timeStamp = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss");
                    
                    Case_Dto cDto = await CaseLookupCaseId(caseId);
                    ReplyElement reply = await CaseRead((int)cDto.MicrotingUId, (int)cDto.CheckUId);
                    
                    string resultDocument = "";

                    if (reply.JasperExportEnabled)
                    {
                        await CaseToJasperXml(cDto, reply, caseId, timeStamp, customPathForUploadedData, customXmlContent);
                        resultDocument = await JasperToPdf(caseId, jasperTemplate, timeStamp);
                    }
                    else
                    {
                        resultDocument = await DocxToPdf(caseId, jasperTemplate, timeStamp, reply, cDto, customPathForUploadedData, customXmlContent, fileType);
                    }

                    //return path
                    string path = Path.GetFullPath(resultDocument);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(path), path);
                    return path;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }
        #endregion

        #region site
        public async Task<Site_Dto> SiteCreate(string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(name), name);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(userFirstName), userFirstName);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(userLastName), userLastName);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(userEmail), userEmail);

                    Tuple<Site_Dto, Unit_Dto> siteResult = await _communicator.SiteCreate(name);

                    string token = await _sqlController.SettingRead(Settings.token);
                    int customerNo = _communicator.OrganizationLoadAllFromRemote(token).Result.CustomerNo;

                    string siteName = siteResult.Item1.SiteName;
                    int siteId = siteResult.Item1.SiteId;
                    int unitUId = siteResult.Item2.UnitUId;
                    int otpCode = siteResult.Item2.OtpCode;
                    SiteName_Dto siteDto = await _sqlController.SiteRead(siteResult.Item1.SiteId);
                    if (siteDto == null)
                    {
                        await _sqlController.SiteCreate((int)siteId, siteName);
                    }
                    siteDto = await _sqlController.SiteRead(siteId);
                    Unit_Dto unitDto = await _sqlController.UnitRead(unitUId);
                    if (unitDto == null)
                    {
                        await _sqlController.UnitCreate(unitUId, customerNo, otpCode, siteDto.SiteUId);
                    }

                    if (string.IsNullOrEmpty(userEmail))
                    {
                        Random rdn = new Random();
                        userEmail = siteId + "." + customerNo + "@invalid.invalid";
                    }

                    Worker_Dto workerDto = await Advanced_WorkerCreate(userFirstName, userLastName, userEmail);
                    await Advanced_SiteWorkerCreate(siteDto, workerDto);

                    return await SiteRead(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <returns></returns>
        // TODO: Refactor to DeviceUserRead(int siteMicrotingUUID)
        public async Task<Site_Dto> SiteRead(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    return await _sqlController.SiteReadSimple(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<Site_Dto>> SiteReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    if (includeRemoved)
                        return await Advanced_SiteReadAll(null, null, null);
                    else
                        return await Advanced_SiteReadAll(Constants.WorkflowStates.NotRemoved, null, null);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<Site_Dto> SiteReset(int siteId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId);

                    Site_Dto site = await SiteRead(siteId);
                    await Advanced_UnitRequestOtp((int)site.UnitId);

                    return await SiteRead(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
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
        public async Task<bool> SiteUpdate(int siteMicrotingUid, string siteName, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    Site_Dto siteDto = await SiteRead(siteMicrotingUid);
                    await Advanced_SiteItemUpdate(siteMicrotingUid, siteName);
                    if (String.IsNullOrEmpty(userEmail))
                    {
                        //if (String.IsNullOrEmpty)
                    }
                    await Advanced_WorkerUpdate((int)siteDto.WorkerUid, userFirstName, userLastName, userEmail);
                    return true;
                }
                else
                    throw new Exception("Core is not running");

            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
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
        public async Task<bool> SiteDelete(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    Site_Dto siteDto = await SiteRead(microtingUid);

                    if (siteDto != null)
                    {
                        await Advanced_SiteItemDelete(microtingUid);
                        Site_Worker_Dto siteWorkerDto = await Advanced_SiteWorkerRead(null, microtingUid, siteDto.WorkerUid);
                        await Advanced_SiteWorkerDelete(siteWorkerDto.MicrotingUId);
                        await Advanced_WorkerDelete((int)siteDto.WorkerUid);
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
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region entity

        /// <summary>
        /// Creates an EntityGroup, and returns its unique microting id for further use
        /// </summary>
        /// <param name="entityType">Entity type, either "EntitySearch" or "EntitySelect"</param>
        /// <param name="name">Templat MainElement's ID to be retrieved from the Microting local DB</param>
        public async Task<EntityGroup> EntityGroupCreate(string entityType, string name)
        {
            try
            {
                if (Running())
                {
                    EntityGroup entityGroup = await _sqlController.EntityGroupCreate(name, entityType);

                    string entityGroupMUId = await _communicator.EntityGroupCreate(entityType, name, entityGroup.Id.ToString());

                    bool isCreated = await _sqlController.EntityGroupUpdate(entityGroup.Id, entityGroupMUId);

                    if (isCreated)
                        return new EntityGroup()
                        {
                            Id = entityGroup.Id,
                            Name = entityGroup.Name,
                            Type = entityGroup.Type,
                            EntityGroupItemLst = new List<EntityItem>(),
                            MicrotingUUID = entityGroupMUId
                        };
                    else
                    {
                        await _sqlController.EntityGroupDelete(entityGroupMUId);
                        throw new Exception("EntityListCreate failed, due to list not created correct");
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "EntityListCreate failed", ex, false);
                throw new Exception("EntityListCreate failed", ex);
            }
        }

        /// <summary>
        /// Returns the EntityGroup and its EntityItems
        /// </summary>
        /// <param name="entityGroupMUId">The unique microting id of the EntityGroup</param>
        public async Task<EntityGroup> EntityGroupRead(string entityGroupMUId)
        {
            if (string.IsNullOrEmpty(entityGroupMUId))
                throw new ArgumentNullException("entityGroupMUId cannot be null or empty");
            return await EntityGroupRead(entityGroupMUId, Constants.EntityItemSortParameters.DisplayIndex, "");
        }

        public async Task<EntityGroup> EntityGroupRead(string entityGroupMUId, string sort, string nameFilter)
        {
            string methodName = t.GetMethodName("Core");
            if (string.IsNullOrEmpty(entityGroupMUId))
                throw new ArgumentNullException("entityGroupMUId cannot be null or empty");
            try
            {
                if (Running())
                {
                    while (_updateIsRunningEntities)
                        Thread.Sleep(200);

                    return await _sqlController.EntityGroupReadSorted(entityGroupMUId, sort, nameFilter);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    await log.LogException(t.GetMethodName("Core"), "(string entityGroupMUId " + entityGroupMUId + ", string sort " + sort + ", string nameFilter " + nameFilter + ") failed", ex, false);
                }
                catch
                {
                    await log.LogException(t.GetMethodName("Core"), "(string entityGroupMUId, string sort, string nameFilter) failed", ex, false);
                }
                throw new Exception("failed", ex);

            }
        }

        /// <summary>
        /// Updates the EntityGroup and its EntityItems for those needed
        /// </summary>
        /// <param name="entityGroup">The EntityGroup and its EntityItems</param>
        public async Task<bool> EntityGroupUpdate(EntityGroup entityGroup)
        {
            try
            {
                if (Running())
                {
//                    List<string> ids = new List<string>();
//                    foreach (var item in entityGroup.EntityGroupItemLst)
//                        ids.Add(item.EntityItemUId);
//
//                    if (ids.Count != ids.Distinct().Count())
//                        throw new Exception("List entityGroup.entityItemUIds are not all unique"); // Duplicates exist
//
//                    while (_updateIsRunningEntities)
//                        Thread.Sleep(200);

                    bool isUpdated = await _communicator.EntityGroupUpdate(entityGroup.Type, entityGroup.Name, entityGroup.Id, entityGroup.MicrotingUUID);

                    if (isUpdated)
                        await _sqlController.EntityGroupUpdateName(entityGroup.Name, entityGroup.MicrotingUUID);

                    //sqlController.EntityGroupUpdateItems(entityGroup);

                    //CoreHandleUpdateEntityItems();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "EntityGroupRead failed", ex, false);
                throw new Exception("EntityGroupRead failed", ex);
            }
            return true;
        }

        /// <summary>
        /// Deletes an EntityGroup, both its items should be deleted before using
        /// </summary>
        /// <param name="entityGroupMUId">The unique microting id of the EntityGroup</param>
        public async Task<bool> EntityGroupDelete(string entityGroupMUId)
        {
            try
            {
                if (Running())
                {
                    while (_updateIsRunningEntities)
                        Thread.Sleep(200);

                    EntityGroup entityGroup = await _sqlController.EntityGroupRead(entityGroupMUId);
                    await _communicator.EntityGroupDelete(entityGroup.Type, entityGroupMUId);
                    string type = await _sqlController.EntityGroupDelete(entityGroupMUId);

                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "EntityGroupDelete failed", ex, false);
                throw new Exception("EntityGroupDelete failed", ex);
            }
            return true;
        }

        #region EntityItem

        public async Task<EntityItem> EntitySearchItemCreate(int entitItemGroupId, string name, string description, string ownUUID) {
            return await EntityItemCreate(entitItemGroupId, name, description, ownUUID, 0);
        }

        public async Task<EntityItem> EntitySelectItemCreate(int entitItemGroupId, string name, int displayIndex, string ownUUID) {
            return await EntityItemCreate(entitItemGroupId, name, "", ownUUID, displayIndex);
        }

        private async Task<EntityItem> EntityItemCreate(int entitItemGroupId, string name, string description, string ownUUID, int displayIndex)
        {
            EntityGroup eg = await _sqlController.EntityGroupRead(entitItemGroupId);
            EntityItem et = await _sqlController.EntityItemRead(entitItemGroupId, name, description);
            if (et == null) {
                string microtingUId;
                if (eg.Type == Constants.FieldTypes.EntitySearch) {
                    microtingUId = await _communicator.EntitySearchItemCreate(eg.MicrotingUUID, name, description, ownUUID);
                } else {
                    microtingUId = await _communicator.EntitySelectItemCreate(eg.MicrotingUUID, name, displayIndex, ownUUID);
                }

                if (microtingUId != null)
                {
                    et = new EntityItem
                    {
                        Name = name,
                        Description = description,
                        EntityItemUId = ownUUID,
                        WorkflowState = Constants.WorkflowStates.Created,
                        MicrotingUUID = microtingUId,
                        DisplayIndex = displayIndex
                    };
//                        (name, description, ownUUID,Constants.WorkflowStates.Created, microtingUId, displayIndex);
                    return await _sqlController.EntityItemCreate(eg.Id, et);
                } else
                {
                    return null;
                }
            } else {
                if (et.WorkflowState == Constants.WorkflowStates.Removed)
                {
                    et.WorkflowState = Constants.WorkflowStates.Created;
                    await _sqlController.EntityItemUpdate(et);
                }
                return et;
            }
        }

        public async Task EntityItemUpdate(int id, string name, string description, string ownUUID, int displayIndex)
        {
            EntityItem et = await _sqlController.EntityItemRead(id);
            if (et == null) {
                throw new NullReferenceException("EntityItem not found with id " + id.ToString());
            } else {
                if (et.Name != name || et.Description != description || et.DisplayIndex != displayIndex)
                {
                    EntityGroup eg = await _sqlController.EntityGroupRead(et.EntityItemGroupId);
                    bool result = false;
                    if (eg.Type == Constants.FieldTypes.EntitySearch)
                    {
                        result = await _communicator.EntitySearchItemUpdate(eg.MicrotingUUID, et.MicrotingUUID, name, description, ownUUID);
                    } else {
                        result = await _communicator.EntitySelectItemUpdate(eg.MicrotingUUID, et.MicrotingUUID, name, displayIndex, ownUUID);
                    }
                    if (result) {
                        et.DisplayIndex = displayIndex;
                        et.Name = name;
                        et.Description = description;

                        await _sqlController.EntityItemUpdate(et);
                    } else {
                        throw new Exception("Unable to update entityItem with id " + id.ToString());
                    }
                }
            }
        }

        public async Task EntityItemDelete(int id)
        {
            EntityItem et = await _sqlController.EntityItemRead(id);
            if (et == null)
            {
                throw new NullReferenceException("EntityItem not found with id " + id.ToString());
            }
            else
            {
                EntityGroup eg = await _sqlController.EntityGroupRead(et.EntityItemGroupId);
                bool result = false;
                if (eg.Type == Constants.FieldTypes.EntitySearch) {
                    result = await _communicator.EntitySearchItemDelete(et.MicrotingUUID);
                } else {
                    result = await _communicator.EntitySelectItemDelete(et.MicrotingUUID);
                }
                if (result) {
                    await _sqlController.EntityItemDelete(id);
                }
                else
                {
                    throw new Exception("Unable to update entityItem with id " + id.ToString());
                }
            }
        }

        #endregion

        public async Task<string> PdfUpload(string localPath)
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

                    if (await _communicator.PdfUpload(localPath, chechSum))
                        return chechSum;
                    else
                    {
                        await log.LogWarning(t.GetMethodName("Core"), "Uploading of PDF failed");
                        return null;
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception(t.GetMethodName("Core") + " failed", ex);
            }
        }
        #endregion
        
        #region folder

        public async Task<List<Folder_Dto>> FolderGetAll(bool includeRemoved)
        {
            try
            {
                if (Running())
                {
                    List<Folder_Dto> folderDtos = await _sqlController.FolderGetAll(includeRemoved);

                    return folderDtos;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "FolderGetAll failed", ex, false);
                throw new Exception("FolderGetAll failed", ex);
            }
        }

        public async Task<Folder_Dto> FolderRead(int id)
        {
            try
            {
                if (Running())
                {
                    Folder_Dto folderDto = await _sqlController.FolderRead(id);

                    return folderDto;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "FolderRead failed", ex, false);
                throw new Exception("FolderRead failed", ex);
            }
        }


        public async Task FolderCreate(string name, string description, int? parent_id)
        {
            try
            {
                if (Running())
                {
                    int apiParentId = 0;
                    if (parent_id != null)
                    {
                        apiParentId = (int)FolderRead((int) parent_id).Result.MicrotingUId;
                    }
                    int id = await _communicator.FolderCreate(name, description, apiParentId);
                    int result = await _sqlController.FolderCreate(name, description, parent_id, id);

                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "FolderCreate failed", ex, false);
                throw new Exception("FolderCreate failed", ex);
            }
        }

        public async Task FolderUpdate(int id, string name, string description, int? parent_id)
        {
            try
            {
                if (Running())
                {
                    Folder_Dto folder = await FolderRead(id);
                    int apiParentId = 0;
                    if (parent_id != null)
                    {
                        apiParentId = (int)FolderRead((int) parent_id).Result.MicrotingUId;
                    }
                    await _communicator.FolderUpdate((int)folder.MicrotingUId, name, description, apiParentId);
                    await _sqlController.FolderUpdate(id, name, description, parent_id);
                }
                else
                {
                    throw new Exception("Core is not running");
                }
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "FolderUpdate failed", ex, false);
                throw new Exception("FolderUpdate failed", ex);
            }
        }

        public async Task FolderDelete(int id)
        {
            try
            {
                if (Running())
                {
                    Folder_Dto folder = await FolderRead(id);
                    bool success = await _communicator.FolderDelete((int)folder.MicrotingUId);
                    if (success)
                    {
                        await _sqlController.FolderDelete(id);
                    }
                }
                else
                {
                    throw new Exception("Core is not running");
                }
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "FolderDelete failed", ex, false);
                throw new Exception("FolderDelete failed", ex);
            }
        }
        #endregion
        
        #endregion

        #region tags
        public async Task<List<Tag>> GetAllTags(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return await _sqlController.GetAllTags(includeRemoved);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// This method will create a tag, which is globally accessible.
        /// </summary>
        /// <param name="name">Name of the tag, which is not allowed to be null or empty.</param>
        /// <returns></returns>
        public async Task<int> TagCreate(string name)
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
                    return await _sqlController.TagCreate(name);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }



        public async Task<bool> TagDelete(int tagId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return await _sqlController.TagDelete(tagId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion


        #region speach to text

        public async Task<bool> TranscribeUploadedData(int uploadedDataId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    uploaded_data uploadedData = await _sqlController.GetUploadedData(uploadedDataId);
                    if (uploadedData != null)
                    {
                        string[] audioFileExtenstions = { ".3gp", ".aa", ".aac", ".aax", ".act", ".aiff", ".amr", ".ape", ".au", ".awb", ".dct", ".dss", ".dvf", ".flac", ".gsm", ".iklax", ".ivs", ".m4a", ".m4b", ".m4p", ".mmf", ".mp3", ".mpc", ".msv", ".nsf", ".ogg", ".oga", ".mogg", ".opus", ".ra", ".rm", ".raw", ".sln", ".tta", ".vox", ".wav", ".wma", ".wv", ".webm", ".8svx" };
                        if (audioFileExtenstions.Any(uploadedData.Extension.Contains))
                        {

                            string filePath = Path.Combine(uploadedData.FileLocation, uploadedData.FileName);
                            await log.LogStandard(methodName, $"filePath is {filePath}");
                            int requestId = await SpeechToText(filePath);
                            uploadedData.TranscriptionId = requestId;

                            await _sqlController.UpdateUploadedData(uploadedData);
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
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<int> SpeechToText(string pathToAudioFile)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return await _communicator.SpeechToText(pathToAudioFile);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> SpeechToText(int requestId)
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
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        #endregion

        #region public advanced actions
        #region templat
        public async Task<bool> Advanced_TemplateDisplayIndexChangeDb(int templateId, int newDisplayIndex)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(newDisplayIndex), newDisplayIndex);

                    return await _sqlController.TemplateDisplayIndexChange(templateId, newDisplayIndex);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_TemplateDisplayIndexChangeServer(int templateId, int siteUId, int newDisplayIndex)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteUId), siteUId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(newDisplayIndex), newDisplayIndex);

                    string respXml = null;
                    List<string> errors = new List<string>();
                    foreach (int microtingUId in await _sqlController.CheckListSitesRead(templateId, siteUId, Constants.WorkflowStates.NotRemoved))
                    {
                        respXml = await _communicator.TemplateDisplayIndexChange(microtingUId.ToString(), siteUId, newDisplayIndex);
                        Response resp = new Response();
                        resp = resp.XmlToClassUsingXmlDocument(respXml);
                        if (resp.Type != Response.ResponseTypes.Success)
                        {
                            string error = $"Failed to set display index for eForm {microtingUId} to {newDisplayIndex}";
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
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_TemplateUpdateFieldIdsForColumns(int templateId, int? fieldId1, int? fieldId2, int? fieldId3, int? fieldId4, int? fieldId5, int? fieldId6, int? fieldId7, int? fieldId8, int? fieldId9, int? fieldId10)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId1), fieldId1);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId2), fieldId2);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId3), fieldId3);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId4), fieldId4);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId5), fieldId5);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId6), fieldId6);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId7), fieldId7);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId8), fieldId8);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId9), fieldId9);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId10), fieldId10);

                    return await _sqlController.TemplateUpdateFieldIdsForColumns(templateId, fieldId1, fieldId2, fieldId3, fieldId4, fieldId5, fieldId6, fieldId7, fieldId8, fieldId9, fieldId10);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<Field_Dto>> Advanced_TemplateFieldReadAll(int templateId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(templateId), templateId);

                    return await _sqlController.TemplateFieldReadAll(templateId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

//        public void Advanced_ConsistencyCheckTemplates()
//        {
//            string methodName = t.GetMethodName("Core");
//            try
//            {
//                if (Running())
//                {
//                    await log.LogStandard(t.GetMethodName("Core"), "called");
//                    List<int> dummyList = new List<int>();
//                    List<Template_Dto> allTemplates = _sqlController.TemplateItemReadAll(true, Constants.WorkflowStates.Removed, "", false, "", dummyList);
//                    foreach (Template_Dto item in allTemplates)
//                    {
//                        foreach (SiteName_Dto site in item.DeployedSites)
//                        {
//                            await CaseDelete(item.Id, site.SiteUId, "");
//                        }
//                    }
//                }
//                else
//                    throw new Exception("Core is not running");
//            }
//            catch (Exception ex)
//            {
//                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
//            }
//        }

        #region sites
        public async Task<List<Site_Dto>> Advanced_SiteReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(offSet), offSet.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(limit), limit.ToString());

                    return await _sqlController.SimpleSiteGetAll(workflowState, offSet, limit);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<SiteName_Dto> Advanced_SiteItemRead(int microting_uuid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microting_uuid), microting_uuid);

                    return await _sqlController.SiteRead(microting_uuid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<SiteName_Dto>> Advanced_SiteItemReadAll()
        {
            return await Advanced_SiteItemReadAll(true);
        }

        public async Task<List<SiteName_Dto>> Advanced_SiteItemReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    return await _sqlController.SiteGetAll(includeRemoved);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteItemUpdate(int siteId, string name)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(name), name);

                    if (await _sqlController.SiteRead(siteId) == null)
                        return false;

                    bool success = await _communicator.SiteUpdate(siteId, name);
                    if (!success)
                        return false;

                    return await _sqlController.SiteUpdate(siteId, name);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteItemDelete(int siteId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId);

                    bool success = await _communicator.SiteDelete(siteId);
                    if (!success)
                        return false;

                    return await _sqlController.SiteDelete(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        #endregion

        #region workers
        public async Task<Worker_Dto> Advanced_WorkerCreate(string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(firstName), firstName);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(lastName), lastName);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(email), email);

                    Worker_Dto workerDto = await _communicator.WorkerCreate(firstName, lastName, email);
                    int workerUId = workerDto.WorkerUId;

                    workerDto = await _sqlController.WorkerRead(workerDto.WorkerUId);
                    if (workerDto == null)
                    {
                        await _sqlController.WorkerCreate(workerUId, firstName, lastName, email);
                    }

                    return await Advanced_WorkerRead(workerUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<string> Advanced_WorkerNameRead(int workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);

                    return await _sqlController.WorkerNameRead(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<Worker_Dto> Advanced_WorkerRead(int workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);

                    return await _sqlController.WorkerRead(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<Worker_Dto>> Advanced_WorkerReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(offSet), offSet.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(limit), limit.ToString());

                    return await _sqlController.WorkerGetAll(workflowState, offSet, limit);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_WorkerUpdate(int workerId, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(firstName), firstName);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(lastName), lastName);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(email), email);

                    if (await _sqlController.WorkerRead(workerId) == null)
                        return false;

                    bool success = await _communicator.WorkerUpdate(workerId, firstName, lastName, email);
                    if (!success)
                        return false;

                    return await _sqlController.WorkerUpdate(workerId, firstName, lastName, email);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_WorkerDelete(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    bool success = await _communicator.WorkerDelete(microtingUid);
                    if (!success)
                        return false;

                    return await _sqlController.WorkerDelete(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion
        #endregion

        #region site_workers
        public async Task<Site_Worker_Dto> Advanced_SiteWorkerCreate(SiteName_Dto siteDto, Worker_Dto workerDto)
        {   
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), "siteId", siteDto.SiteUId);
                    await log.LogVariable(t.GetMethodName("Core"), "workerId", workerDto.WorkerUId);

                    Site_Worker_Dto result = await _communicator.SiteWorkerCreate(siteDto.SiteUId, workerDto.WorkerUId);
                    //var parsedData = JRaw.Parse(result);
                    //int workerUid = int.Parse(parsedData["id"].ToString());

                    Site_Worker_Dto siteWorkerDto = await _sqlController.SiteWorkerRead(result.MicrotingUId, null, null);

                    if (siteWorkerDto == null)
                    {
                        await _sqlController.SiteWorkerCreate(result.MicrotingUId, siteDto.SiteUId, workerDto.WorkerUId);
                    }

                    return await Advanced_SiteWorkerRead(result.MicrotingUId, null, null);
                    //return null;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<Site_Worker_Dto> Advanced_SiteWorkerRead(int? siteWorkerMicrotingUid, int? siteId, int? workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteWorkerMicrotingUid), siteWorkerMicrotingUid.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(siteId), siteId.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId.ToString());

                    return await _sqlController.SiteWorkerRead(siteWorkerMicrotingUid, siteId, workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteWorkerDelete(int workerId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workerId), workerId);

                    bool success = await _communicator.SiteWorkerDelete(workerId);
                    if (!success)
                        return false;

                    return await _sqlController.SiteWorkerDelete(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region units
        public async Task<Unit_Dto> Advanced_UnitRead(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    return await _sqlController.UnitRead(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public async Task<List<Unit_Dto>> Advanced_UnitReadAll()
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");

                    return await _sqlController.UnitGetAll();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                return null;
            }
        }

        public async Task<Unit_Dto> Advanced_UnitRequestOtp(int microtingUid)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(microtingUid), microtingUid);

                    int otp_code = await _communicator.UnitRequestOtp(microtingUid);

                    Unit_Dto my_dto = await Advanced_UnitRead(microtingUid);

                    await _sqlController.UnitUpdate(microtingUid, my_dto.CustomerNo, otp_code, my_dto.SiteUId);

                    return await Advanced_UnitRead(microtingUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw ex;
            }
        }
        
        public async Task<bool> Advanced_UnitDelete(int unitId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(unitId), unitId);

                    bool success = await _communicator.UnitDelete(unitId);
                    if (!success)
                        return false;

                    return await _sqlController.UnitDelete(unitId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region fields
        public async Task<Field> Advanced_FieldRead(int id)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(id), id);

                    return await _sqlController.FieldRead(id);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<UploadedData> Advanced_UploadedDataRead(int id)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                await log.LogStandard(t.GetMethodName("Core"), "called");
                await log.LogVariable(t.GetMethodName("Core"), nameof(id), id);

                var ud = await _sqlController.GetUploadedData(id);
                UploadedData uD = new UploadedData();
                uD.Checksum = ud.Checksum;
                uD.CurrentFile = ud.CurrentFile;
                uD.Extension = ud.Extension;
                uD.FileLocation = ud.FileLocation;
                uD.FileName = ud.FileName;
                uD.Id = ud.Id;
                uD.UploaderId = ud.UploaderId;
                uD.UploaderType = ud.UploaderType;
                return uD;
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldValue>> Advanced_FieldValueReadList(int id, int instances)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(id), id);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(instances), instances);

                    return await _sqlController.FieldValueReadList(id, instances);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        
        public async Task<List<FieldValue>> Advanced_FieldValueReadList(int fieldId, List<int> caseIds)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId), fieldId);

                    return await _sqlController.FieldValueReadList(fieldId, caseIds);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldValue>> Advanced_FieldValueReadList(List<int> caseIds)
        {
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");

                    return await _sqlController.FieldValueReadList(caseIds);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<CheckListValue>> Advanced_CheckListValueReadList(List<int> caseIds)
        {
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");

                    return await _sqlController.CheckListValueReadList(caseIds);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }


        #endregion

        //EntityGroupList
        public async Task<EntityGroupList> Advanced_EntityGroupAll(string sort, string nameFilter, int pageIndex, int pageSize, string entityType, bool desc, string workflowState)
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
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(sort), sort);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(nameFilter), nameFilter);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(pageIndex), pageIndex);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(pageSize), pageSize);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(entityType), entityType);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(desc), desc);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(workflowState), workflowState);

                    return await _sqlController.EntityGroupAll(sort, nameFilter, pageIndex, pageSize, entityType, desc, workflowState);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_DeleteUploadedData(int fieldId, int uploadedDataId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(fieldId), fieldId);
                    await log.LogVariable(t.GetMethodName("Core"), nameof(uploadedDataId), uploadedDataId);

                    uploaded_data uD = await _sqlController.GetUploadedData(uploadedDataId);

                    try
                    {
                        Directory.CreateDirectory(uD.FileLocation + "Deleted");
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                            File.Move(uD.FileLocation + uD.FileName, uD.FileLocation + @"Deleted\" + uD.FileName);
                        }
                        else
                        {
                            File.Move(uD.FileLocation + uD.FileName, uD.FileLocation + @"Deleted/" + uD.FileName);
                        }
                    }
                    catch (Exception exd)
                    {
                        await log.LogException(t.GetMethodName("Core"), "failed", exd, true);
                        throw new Exception("failed", exd);
                    }

                    return await _sqlController.DeleteFile(uploadedDataId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_UpdateCaseFieldValue(int caseId)
        {
            string methodName = t.GetMethodName("Core");
            try
            {
                if (Running())
                {
                    await log.LogStandard(t.GetMethodName("Core"), "called");
                    await log.LogVariable(t.GetMethodName("Core"), nameof(caseId), caseId);
                    return await _sqlController.CaseUpdateFieldValues(caseId);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Core"), "failed", ex, false);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region private
        private async Task<List<Element>> ReplaceDataElementsAndDataItems(int caseId, List<Element> elementList, List<FieldValue> lstAnswers)
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

                    elementListReplaced.Add(new CheckListValue(dataE, await _sqlController.CheckListValueStatusRead(caseId, element.Id)));
                }
                #endregion

                #region if GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    await ReplaceDataElementsAndDataItems(caseId, groupE.ElementList, lstAnswers);

                    elementListReplaced.Add(groupE);
                }
                #endregion
            }

            return elementListReplaced;
        }

        private async Task<int> SendXml(MainElement mainElement, int siteId)
        {
            await log.LogEverything(t.GetMethodName("Core"), "siteId:" + siteId + ", requested sent eForm");

            string xmlStrRequest = mainElement.ClassToXml();
            
            await log.LogEverything(t.GetMethodName("Core"), "siteId:" + siteId + ", ClassToXml done");
            string xmlStrResponse = await _communicator.PostXml(xmlStrRequest, siteId);
            await log.LogEverything(t.GetMethodName("Core"), "siteId:" + siteId + ", PostXml done");

            Response response = new Response();
            response = response.XmlToClass(xmlStrResponse);
            await log.LogEverything(t.GetMethodName("Core"), "siteId:" + siteId + ", XmlToClass done");

            //if reply is "success", it's created
            if (response.Type.ToString().ToLower() == "success")
            {
                return int.Parse(response.Value);
            }

            throw new Exception("siteId:'" + siteId + "' // failed to create eForm at Microting // Response :" + xmlStrResponse);
        }

        private async Task<List<List<string>>> GenerateDataSetFromCases(int? templateId, DateTime? start, DateTime? end, string customPathForUploadedData, string decimalSeparator, string thousandSaperator)
        {
            List<List<string>> dataSet = new List<List<string>>();
            List<string> colume1CaseIds = new List<string> { "Id" };
            List<int> caseIds = new List<int>();

            List<Case> caseList = await _sqlController.CaseReadAll(templateId, start, end, Constants.WorkflowStates.NotRemoved, null, false, "");
            var template = await _sqlController.TemplateItemRead((int)templateId);

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
                    MainElement templateData = await _sqlController.TemplateRead((int)templateId);

                    List<string> lstReturn = new List<string>();
                    lstReturn = await GenerateDataSetFromCasesSubSet(lstReturn, templateData.ElementList, "");

                    List<string> newRow;
                    foreach (string set in lstReturn)
                    {
                        int fieldId = int.Parse(t.SplitToList(set, 0, false));
                        string label = t.SplitToList(set, 1, false);

                        List<List<KeyValuePair>> result = await _sqlController.FieldValueReadAllValues(fieldId, caseIds, customPathForUploadedData, decimalSeparator, thousandSaperator);

                        if (result.Count == 1)
                        {
                            newRow = new List<string>();
                            newRow.Insert(0, label);
                            List<KeyValuePair> tempList = result[0];
                            foreach (int i in caseIds)
                            {
                                string value = "";
                                foreach (KeyValuePair KvP in tempList)
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
                            Field field = await _sqlController.FieldRead(fieldId);
                            foreach (var lst in result)
                            {
                                newRow = new List<string>();
                                List<KeyValuePair> fieldKvP = field.KeyValuePairList;
                                newRow.Insert(0, label + " | " + fieldKvP.ElementAt(option).Value);
                                foreach (int i in caseIds)
                                {
                                    string value = "";
                                    foreach (KeyValuePair KvP in lst)
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

        private async Task<List<string>> GenerateDataSetFromCasesSubSet(List<string> lstReturn, List<Element> elementList, string preLabel)
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
                        await GenerateDataSetFromCasesSubSet(lstReturn, groupE.ElementList, preLabel + sep + groupE.Label);
                    else
                        await GenerateDataSetFromCasesSubSet(lstReturn, groupE.ElementList, groupE.Label);
                }
                #endregion
            }

            return lstReturn;
        }

        private async Task<List<string>> PdfValidate(string pdfString, int pdfId)
        {
            await Task.Run(() => { }); // TODO FIX ME
            List<string> errorLst = new List<string>();

            if (pdfString.ToLower().Contains("microting.com"))
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' contains an URL that points to Microting's builder temporary hosting. Indicating that it's not a proper hash");
            if (pdfString.ToLower().Contains("http") || pdfString.ToLower().Contains("https"))
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' contains an HTTP or HTTPS. Indicating that it's not a proper hash");
            if (pdfString.Length != 32)
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' length is not the correct length (32). Indicating that it's not a proper hash");

            if (errorLst.Count > 0)
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' please check 'value' input, and consider running PdfUpload");

            return errorLst;
        }

        private async Task<string> GetJasperFieldValue(Field field, FieldValue answer, string customPathForUploadedData)
        {
            await Task.Run(() => { }); // TODO FIX ME
            string jasperFieldXml = "";
            string latitude = answer.Latitude;
            string longitude = answer.Longitude;
            string altitude = answer.Altitude;
            string heading = answer.Heading;
            string gps = "latitude=\"" + latitude + "\" longitude=\"" +
                         longitude + "\" altitude=\"" + altitude + "\" heading=\"" +
                         heading + "\"";
            switch (field.FieldType)
            {
                case Constants.FieldTypes.Picture:
                case Constants.FieldTypes.Signature:
                    if (answer.UploadedDataObj != null)
                    {
                        if (customPathForUploadedData != null)
                        {
                            jasperFieldXml +=
                                Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" +
                                answer.Id + "\" " + gps + "><![CDATA[" + customPathForUploadedData +
                                answer.UploadedDataObj.FileName + "]]></F" + field.Id + "_value>";
                        }
                        else
                        {
                            jasperFieldXml +=
                                Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" +
                                answer.Id + "\" " + gps + "><![CDATA[" + answer.UploadedDataObj.FileName +
                                "]]></F" + field.Id + "_value>";
                        }
                    }
                    else
                    {
                        //jasperFieldXml += Environment.NewLine + "<F" + field.Id + "_value field_value_id=\"" + answer.Id + "\">NO FILE</F" + field.Id + "_value>";
                    }
                    break;
                case Constants.FieldTypes.Number:
                case Constants.FieldTypes.NumberStepper:
                    
                    jasperFieldXml += Environment.NewLine + "<F" + field.Id +
                                      "_value field_value_id=\"" + answer.Id + "\" " + gps + "><![CDATA[" +
                                      (answer.ValueReadable.Replace(",",".") ?? string.Empty) + "]]></F" + field.Id +
                                      "_value>";
                    break;
                default:
                {
                    jasperFieldXml += Environment.NewLine + "<F" + field.Id +
                                      "_value field_value_id=\"" + answer.Id + "\" " + gps + "><![CDATA[" +
                                      (answer.ValueReadable ?? string.Empty) + "]]></F" + field.Id +
                                      "_value>";
                    break;
                }
            }

            return jasperFieldXml;
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
                                jasperFieldXml += GetJasperFieldValue(field, answer, customPathForUploadedData);
                            }
                        } else if (item is FieldContainer)
                        {
                            FieldContainer fieldC = (FieldContainer)item;

                            foreach (Field field in fieldC.DataItemList)
                            {
                                jasperFieldXml += Environment.NewLine + "<F" + field.Id + " name=\"" + field.Label + "\" parent=\"" + dataE.Label + "\">";
                                foreach (FieldValue answer in field.FieldValues)
                                {                                    
                                    jasperFieldXml += GetJasperFieldValue(field, answer, customPathForUploadedData);
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
        private async Task CoreThread()
        {
            _coreThreadRunning = true;

            await log.LogEverything(t.GetMethodName("Core"), "initiated");
            while (_coreAvailable)
            {
                try
                {
                    if (_coreThreadRunning)
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
                    await log.LogWarning(t.GetMethodName("Core"), "catch of ThreadAbortException");
                }
                catch (Exception ex)
                {
                    await FatalExpection(t.GetMethodName("Core") + "failed", ex);
                }
            }
            await log.LogEverything(t.GetMethodName("Core"), "completed");

            _coreThreadRunning = false;
        }

        public async Task<bool> DownloadUploadedData(int uploadedDataId)
        {
            uploaded_data uploadedData = await _sqlController.GetUploadedData(uploadedDataId);

            if (uploadedData != null)
            {
                string urlStr = uploadedData.FileLocation;
                await log.LogEverything(t.GetMethodName("Core"), "Received file:" + uploadedData.ToString());

                #region finding file name and creating folder if needed
                FileInfo file = new FileInfo(_fileLocationPicture);
                if (file.Directory != null)
                    file.Directory.Create(); // If the directory already exists, this method does nothing.

                int index = urlStr.LastIndexOf("/") + 1;
                string fileName = uploadedData.Id.ToString() + "_" + urlStr.Remove(0, index);
                #endregion

                #region download file
                using (var client = new WebClient())
                {
                    try
                    {
                        await log.LogStandard(t.GetMethodName("Core"), $"Downloading file to #{_fileLocationPicture}/#{fileName}");
                        client.DownloadFile(urlStr, _fileLocationPicture + fileName);
                    }
                    catch (Exception ex)
                    {
                        await log.LogWarning(t.GetMethodName("Core"), "We got an error " + ex.Message);
                        throw new Exception("Downloading and creating fil locally failed.", ex);
                    }

                }
                #endregion

                #region finding checkSum
                string chechSum = "";
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(_fileLocationPicture + fileName))
                    {
                        byte[] grr = md5.ComputeHash(stream);
                        chechSum = BitConverter.ToString(grr).Replace("-", "").ToLower();
                    }
                }
                #endregion

                #region checks checkSum
                if (chechSum != fileName.Substring(fileName.LastIndexOf(".") - 32, 32))
                    await log.LogWarning(t.GetMethodName("Core"), "Download of '" + urlStr + "' failed. Check sum did not match");
                #endregion

                Case_Dto dto = await _sqlController.FileCaseFindMUId(urlStr);
                File_Dto fDto = new File_Dto(dto.SiteUId, dto.CaseType, dto.CaseUId, dto.MicrotingUId.ToString(), dto.CheckUId.ToString(), _fileLocationPicture + fileName);
                try { HandleFileDownloaded?.Invoke(fDto, EventArgs.Empty); }
                catch { await log.LogWarning(t.GetMethodName("Core"), "HandleFileDownloaded event's external logic suffered an Expection"); }
                await log.LogStandard(t.GetMethodName("Core"), "Downloaded file '" + urlStr + "'.");

                await _sqlController.FileProcessed(urlStr, chechSum, _fileLocationPicture, fileName, uploadedData.Id);

                if (_swiftEnabled)
                {
                    await log.LogStandard(t.GetMethodName("Core"), $"Swift enabled, so trying to upload file {fileName}");
                    string filePath = Path.Combine(_fileLocationPicture, fileName);
                    if (File.Exists(filePath))
                    {
                        await log.LogStandard(t.GetMethodName("Core"), $"File exists at path {filePath}");
                        try
                        {
                            await PutFileToStorageSystem(filePath, fileName);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            await log.LogStandard(t.GetMethodName("Core"), "Trying to reauthenticate before putting file again");
                            _swiftClient.AuthenticateAsyncV2(_keystoneEndpoint, _swiftUserName, _swiftPassword);
                            await PutFileToStorageSystem(filePath, fileName);                                                        
                        }                        
                    }
                    else
                    {
                        await log.LogWarning(t.GetMethodName("Core"), $"File could not be found at filepath {filePath}");
                    }
                }
                
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<GetObjectResponse> GetFileFromS3Storage(string fileName)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = $"{_customerNo}_uploaded_data",
                Key = fileName
            };

            GetObjectResponse response = await _s3Client.GetObjectAsync(request);
            return response;
        }

        public async Task<SwiftObjectGetResponse> GetFileFromSwiftStorage(string fileName)
        {
            try
            {
                return await GetFileFromSwiftStorage(fileName, 0);
            }
            catch (UnauthorizedAccessException)
            {
                _swiftClient.AuthenticateAsyncV2(_keystoneEndpoint, _swiftUserName, _swiftPassword);
                
                return await GetFileFromSwiftStorage(fileName, 0);
            }
        }

        private async Task<SwiftObjectGetResponse> GetFileFromSwiftStorage(string fileName, int retries)
        {
            if (_swiftEnabled)
            {                
                await log.LogStandard(t.GetMethodName("Core"), $"Trying to get file {fileName} from {_customerNo}_uploaded_data");
                SwiftObjectGetResponse response = await _swiftClient.ObjectGetAsync(_customerNo + "_uploaded_data", fileName);
                if (response.IsSuccess)
                {
                    return response;
                }
                else
                {
                    if (response.Reason == "Unauthorized")
                    {
                        await log.LogWarning(t.GetMethodName("Core"), "Check swift credentials : Unauthorized");
                        throw new UnauthorizedAccessException();
                    }
                    
                    await log.LogCritical(t.GetMethodName("Core"), $"Could not get file {fileName}, reason is {response.Reason}");
                    throw new Exception($"Could not get file {fileName}");
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public async Task PutFileToStorageSystem(string filePath, string fileName)
        {
            try
            {
                await PutFileToStorageSystem(filePath, fileName, 0);
            }
            catch (UnauthorizedAccessException)
            {
                _swiftClient.AuthenticateAsyncV2(_keystoneEndpoint, _swiftUserName, _swiftPassword);
                await PutFileToStorageSystem(filePath, fileName, 0);
            }
            
        }

        private async Task PutFileToStorageSystem(String filePath, string fileName, int tryCount)
        {
            if (_swiftEnabled)
            {
                await PutFileToSwiftStorage(filePath, fileName, tryCount);
            }

            if (_s3Enabled)
            {
                await PutFileToS3Storage(filePath, fileName, tryCount);
            }
        }

        private async Task PutFileToSwiftStorage(string filePath, string fileName, int tryCount)
        {
            await log.LogStandard(t.GetMethodName("Core"), $"Trying to upload file {fileName} to {_customerNo}_uploaded_data");
            try
            {
                var fileStream = new FileStream(filePath, FileMode.Open);

                SwiftBaseResponse response = _swiftClient
                    .ObjectPutAsync(_customerNo + "_uploaded_data", fileName, fileStream).Result;

                if (!response.IsSuccess)
                {
                    if (response.Reason == "Unauthorized")
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                        await log.LogWarning(t.GetMethodName("Core"), "Check swift credentials : Unauthorized");
                        throw new UnauthorizedAccessException();
                    }

                    await log.LogWarning(t.GetMethodName("Core"), $"Something went wrong, message was {response.Reason}");

                    response = _swiftClient.ContainerPutAsync(_customerNo + "_uploaded_data").Result;
                    if (response.IsSuccess)
                    {
                        response = _swiftClient
                            .ObjectPutAsync(_customerNo + "_uploaded_data", fileName, fileStream).Result;
                        if (!response.IsSuccess)
                        {
                            fileStream.Close();
                            fileStream.Dispose();
                            throw new Exception($"Could not upload file {fileName}");
                        }
                    }
                }

                fileStream.Close();
                fileStream.Dispose();
            }
            catch (FileNotFoundException ex)
            {
                await log.LogCritical(t.GetMethodName("Core"), $"File not found at {filePath}");
                await log.LogCritical(t.GetMethodName("Core"), ex.Message);
            }   
        }

        private async Task PutFileToS3Storage(string filePath, string fileName, int tryCount)
        {
            await Task.Run(() => { }); // TODO FIX ME
            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = $"{_customerNo}_uploaded_data",
                Key = fileName,
                FilePath = filePath
            };
        }
        
        public async Task<bool> CheckStatusByMicrotingUid(int microtingUid)
        {
            List<Case_Dto> lstCase = new List<Case_Dto>();
            MainElement mainElement = new MainElement();

            Case_Dto concreteCase = await _sqlController.CaseReadByMUId(microtingUid);
            await log.LogEverything(t.GetMethodName("Core"), concreteCase.ToString() + " has been matched");

            if (concreteCase.CaseUId == "" || concreteCase.CaseUId == "ReversedCase")
                lstCase.Add(concreteCase);
            else
                lstCase = await _sqlController.CaseReadByCaseUId(concreteCase.CaseUId);

            foreach (Case_Dto aCase in lstCase)
            {
                if (aCase.SiteUId == concreteCase.SiteUId)
                {
                    #region get response's data and update DB with data
                    int? checkIdLastKnown = await _sqlController.CaseReadLastCheckIdByMicrotingUId(microtingUid); //null if NOT a checkListSite
                    await log.LogVariable(t.GetMethodName("Core"), nameof(checkIdLastKnown), checkIdLastKnown);

                    string respXml;
                    if (checkIdLastKnown == null)
                        respXml = await _communicator.Retrieve(microtingUid.ToString(), concreteCase.SiteUId);
                    else
                        respXml = await _communicator.RetrieveFromId(microtingUid.ToString(), concreteCase.SiteUId, checkIdLastKnown.ToString());
                    await log.LogVariable(t.GetMethodName("Core"), nameof(respXml), respXml);

                    Response resp = new Response();
                    resp = resp.XmlToClassUsingXmlDocument(respXml);
                    //resp = resp.XmlToClass(respXml);

                    if (resp.Type == Response.ResponseTypes.Success)
                    {
                        await log.LogEverything(t.GetMethodName("Core"), "resp.Type == Response.ResponseTypes.Success (true)");
                        if (resp.Checks.Count > 0)
                        {
                            XmlDocument xDoc = new XmlDocument();

                            xDoc.LoadXml(respXml);
                            XmlNode checks = xDoc.DocumentElement.LastChild;
                            int i = 0;
                            foreach (Check check in resp.Checks)
                            {

                                int unitUId = _sqlController.UnitRead(int.Parse(check.UnitId)).Result.UnitUId;
                                await log.LogVariable(t.GetMethodName("Core"), nameof(unitUId), unitUId);
                                int workerUId = _sqlController.WorkerRead(int.Parse(check.WorkerId)).Result.WorkerUId;
                                await log.LogVariable(t.GetMethodName("Core"), nameof(workerUId), workerUId);

                                await _sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml.ToString(), i);

                                await _sqlController.CaseUpdateCompleted(microtingUid, (int)check.Id, DateTime.Parse(check.Date), workerUId, unitUId);
                                await log.LogEverything(t.GetMethodName("Core"), "sqlController.CaseUpdateCompleted(...)");

                                #region IF needed retract case, thereby completing the process
                                if (checkIdLastKnown == null)
                                {
                                    string responseRetractionXml = await _communicator.Delete(aCase.MicrotingUId.ToString(), aCase.SiteUId);
                                    Response respRet = new Response();
                                    respRet = respRet.XmlToClass(respXml);

                                    if (respRet.Type == Response.ResponseTypes.Success)
                                    {
                                        await log.LogEverything(t.GetMethodName("Core"), aCase.ToString() + " has been retracted");
                                    }
                                    else
                                        await log.LogWarning(t.GetMethodName("Core"), "Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteUId + ". Not a critical issue, but needs to be fixed if repeated");
                                }
                                #endregion

                                await _sqlController.CaseRetract(microtingUid, (int)check.Id);
                                await log.LogEverything(t.GetMethodName("Core"), "sqlController.CaseRetract(...)");
                                // TODO add case.Id
                                Case_Dto cDto = await _sqlController.CaseReadByMUId(microtingUid);
								//InteractionCaseUpdate(cDto);
                                await FireHandleCaseCompleted(cDto);
                                //try { HandleCaseCompleted?.Invoke(cDto, EventArgs.Empty); }
                                //catch { await log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection"); }
                                await log.LogStandard(t.GetMethodName("Core"), cDto.ToString() + " has been completed");
                                i++;
                            }
                        }
                    }
                    else
                    {
                        await log.LogEverything(t.GetMethodName("Core"), "resp.Type == Response.ResponseTypes.Success (false)");
                        throw new Exception("Failed to retrive eForm " + microtingUid + " from site " + aCase.SiteUId);
                    }
                    #endregion
                }
                else
                {
                    //delete eForm on other tablets and update DB to "deleted"
                    await CaseDelete((int)aCase.MicrotingUId);
                }
            }
            return true;
        }        
        #endregion


        public List<KeyValuePair> PairRead(string str)
        {
            return _sqlController.PairRead(str);
        }

        #region fireEvents

        public async Task FireHandleCaseCompleted(Case_Dto caseDto)
		{
		    await log.LogStandard(t.GetMethodName("Core"), $"FireHandleCaseCompleted for MicrotingUId {caseDto.MicrotingUId}");
			try { HandleCaseCompleted.Invoke(caseDto, EventArgs.Empty); }
			catch (Exception ex)
			{
				await log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection");
				throw ex;
			}
		}

        public async Task FireHandleCaseDeleted(Case_Dto caseDto)
        {
            try { HandleCaseDeleted?.Invoke(caseDto, EventArgs.Empty); }
            catch { await log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection"); }
        }

        public async Task FireHandleNotificationNotFound(Note_Dto notification)
        {
            try { HandleNotificationNotFound?.Invoke(notification, EventArgs.Empty); }
            catch { await log.LogWarning(t.GetMethodName("Core"), "HandleNotificationNotFound event's external logic suffered an Expection"); }
        }

        public async Task FireHandleSiteActivated(Note_Dto notification)
        {
            try { HandleSiteActivated?.Invoke(notification, EventArgs.Empty); }
            catch { await log.LogWarning(t.GetMethodName("Core"), "HandleSiteActivated event's external logic suffered an Expection"); }
        }

		public async Task FireHandleCaseRetrived(Case_Dto caseDto)
		{
		    await log.LogStandard(t.GetMethodName("Core"), $"FireHandleCaseRetrived for MicrotingUId {caseDto.MicrotingUId}");

			try { HandleCaseRetrived.Invoke(caseDto, EventArgs.Empty); }
			catch (Exception ex)
			{
				await log.LogWarning(t.GetMethodName("Core"), "HandleCaseRetrived event's external logic suffered an Expection");
				throw ex;
			}
		}
        #endregion
    }

}