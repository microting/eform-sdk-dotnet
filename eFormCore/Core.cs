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
using Amazon.S3.Util;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microting.eForm;
using Microting.eForm.Communication;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using Microting.eForm.Infrastructure.Models.reply;
using Microting.eForm.Installers;
using Microting.eForm.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenStack.NetCoreSwiftClient;
using OpenStack.NetCoreSwiftClient.Infrastructure.Models;
using Field = Microting.eForm.Infrastructure.Models.Field;
using Path = System.IO.Path;
using Tag = Microting.eForm.Dto.Tag;


namespace eFormCore
{
    public class Core : CoreBase
    {
        #region events
        public event EventHandler HandleCaseCreated;
        public event EventHandler HandleCaseRetrived;
        public event EventHandler HandleCaseCompleted;
        public event EventHandler HandleeFormProcessedByServer;
        public event EventHandler HandleeFormProsessingError;
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
        public DbContextHelper dbContextHelper;
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
            string methodName = "Core.Start";
            
            try
			{
				if (!_coreAvailable && !_coreStatChanging)
				{
                    if (!await StartSqlOnly(connectionString).ConfigureAwait(false))
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
					log.LogCritical(methodName, "called");

					//---

					_coreStatChanging = true;

					//subscriber
					_subscriber = new Subscriber(_sqlController, log, _bus);
					_subscriber.Start();
					log.LogStandard(methodName, "Subscriber started");

					log.LogCritical(methodName, "started");
					_coreAvailable = true;
					_coreStatChanging = false;

                    //coreThread
                    //Thread coreThread = new Thread(() => CoreThread());
                    //coreThread.Start();
                    _coreThreadRunning = true;

                    log.LogStandard(methodName, "CoreThread started");
				}
			}
			#region catch
			catch (Exception ex)
			{
                await FatalExpection(methodName + " failed", ex).ConfigureAwait(false);
				throw ex;
				//return false;
			}
			#endregion

			return true;
		}

        public async Task<bool> StartSqlOnly(string connectionString)
        {
            string methodName = "Core.StartSqlOnly";
            try
            {
                if (!_coreAvailable && !_coreStatChanging)
                {
                    _coreStatChanging = true;

                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

                    //sqlController
                    dbContextHelper = new DbContextHelper(connectionString);
                    _sqlController = new SqlController(dbContextHelper);

                    //check settings
                    if (_sqlController.SettingCheckAll().GetAwaiter().GetResult().Count > 0)
                        throw new ArgumentException("Use AdminTool to setup database correctly. 'SettingCheckAll()' returned with errors");

                    if (await _sqlController.SettingRead(Settings.token).ConfigureAwait(false) == "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
                        throw new ArgumentException("Use AdminTool to setup database correctly. Token not set, only default value found");

                    if (await _sqlController.SettingRead(Settings.firstRunDone).ConfigureAwait(false) != "true")
                        throw new ArgumentException("Use AdminTool to setup database correctly. FirstRunDone has not completed");

                    if (await _sqlController.SettingRead(Settings.knownSitesDone).ConfigureAwait(false) != "true")
                        throw new ArgumentException("Use AdminTool to setup database correctly. KnownSitesDone has not completed");

                    //log
                    if (log == null)
                        log = _sqlController.StartLog(this);

                    log.LogCritical(methodName, "###########################################################################");
                    log.LogCritical(methodName, "called");
                    log.LogStandard(methodName, "SqlController and Logger started");

                    //settings read
                    this._connectionString = connectionString;
                    _fileLocationPicture = await _sqlController.SettingRead(Settings.fileLocationPicture);
                    _fileLocationPdf = await _sqlController.SettingRead(Settings.fileLocationPdf);
                    log.LogStandard(methodName, "Settings read");

                    //communicators
                    string token = await _sqlController.SettingRead(Settings.token).ConfigureAwait(false);
                    string comAddressApi = await _sqlController.SettingRead(Settings.comAddressApi).ConfigureAwait(false);
                    string comAddressBasic = await _sqlController.SettingRead(Settings.comAddressBasic).ConfigureAwait(false);
                    string comOrganizationId = await _sqlController.SettingRead(Settings.comOrganizationId).ConfigureAwait(false);
                    string ComAddressPdfUpload = await _sqlController.SettingRead(Settings.comAddressPdfUpload).ConfigureAwait(false);
                    string ComSpeechToText = await _sqlController.SettingRead(Settings.comSpeechToText).ConfigureAwait(false);
                    _communicator = new Communicator(token, comAddressApi, comAddressBasic, comOrganizationId, ComAddressPdfUpload, log, ComSpeechToText);

                    _container = new WindsorContainer();
                    _container.Register(Component.For<SqlController>().Instance(_sqlController));
                    _container.Register(Component.For<Communicator>().Instance(_communicator));
                    _container.Register(Component.For<Log>().Instance(log));
                    _container.Register(Component.For<Core>().Instance(this));
                    
                    
                    try
                    {
                        _customerNo = await _sqlController.SettingRead(Settings.customerNo).ConfigureAwait(false);
                    }
                    catch { }

                    try
				    {
				        _swiftEnabled = (_sqlController.SettingRead(Settings.swiftEnabled).GetAwaiter().GetResult().ToLower() == "true");

				    } catch {}

                    try
                    {
                        _s3Enabled = (_sqlController.SettingRead(Settings.s3Enabled).GetAwaiter().GetResult().ToLower() == "true");

                    } catch {}

				    if (_swiftEnabled)
				    {
				        _swiftUserName = await _sqlController.SettingRead(Settings.swiftUserName).ConfigureAwait(false);
				        _swiftPassword = await _sqlController.SettingRead(Settings.swiftPassword).ConfigureAwait(false);
				        _swiftEndpoint = await _sqlController.SettingRead(Settings.swiftEndPoint).ConfigureAwait(false);
				        _keystoneEndpoint = await _sqlController.SettingRead(Settings.keystoneEndPoint).ConfigureAwait(false);

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
				            log.LogWarning(methodName, ex.Message);
				        }
				        
				        
				        _container.Register(Component.For<SwiftClientService>().Instance(_swiftClient));
				    }

                    if (_s3Enabled)
                    {
                        try
                        {
                            _s3AccessKeyId = await _sqlController.SettingRead(Settings.s3AccessKeyId).ConfigureAwait(false);
                            _s3SecretAccessKey = await _sqlController.SettingRead(Settings.s3SecrectAccessKey).ConfigureAwait(false);
                            _s3Endpoint = await _sqlController.SettingRead(Settings.s3Endpoint).ConfigureAwait(false);

                            if (_s3Endpoint.Contains("https"))
                            {
                                _s3Client = new AmazonS3Client(_s3AccessKeyId, _s3SecretAccessKey, new AmazonS3Config()
                                {
                                    ServiceURL = _s3Endpoint,
                                });
                            }
                            else
                            {
                                _s3Client = new AmazonS3Client(_s3AccessKeyId, _s3SecretAccessKey, RegionEndpoint.EUCentral1);
                                
                            }

                            _container.Register(Component.For<AmazonS3Client>().Instance(_s3Client));
                        }
                        catch (Exception ex)
                        {
                            log.LogWarning(methodName, ex.Message);
                        }
                        
                    }



                    log.LogStandard(methodName, "Communicator started");

                    log.LogCritical(methodName, "started");
                    _coreAvailable = true;
                    _coreStatChanging = false;
                }
            }
            #region catch
            catch (Exception ex)
            {
                _coreThreadRunning = false;
                _coreStatChanging = false;

                await FatalExpection(methodName + " failed", ex);
                return false;
            }
            #endregion

            return true;
        }

        public override async Task Restart(int sameExceptionCount, int sameExceptionCountMax)
        {
            string methodName = "Core.Restart";
            try
            {
                if (_coreRestarting == false)
                {
                    _coreRestarting = true;
                    log.LogCritical(methodName, "called");
                    log.LogVariable(methodName, nameof(sameExceptionCount), sameExceptionCount);
                    log.LogVariable(methodName, nameof(sameExceptionCountMax), sameExceptionCountMax);

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
                    log.LogVariable(methodName, nameof(_sameExceptionCountTried), _sameExceptionCountTried);
                    log.LogVariable(methodName, nameof(secondsDelay), secondsDelay);

                    await Close().ConfigureAwait(false);

                    log.LogStandard(methodName, "Trying to restart the Core in " + secondsDelay + " seconds");

                    if (!skipRestartDelay)
                        Thread.Sleep(secondsDelay * 1000);
                    else
                        log.LogStandard(methodName, "Delay skipped");

                    await Start(_connectionString).ConfigureAwait(false);
                    _coreRestarting = false;
                }
            }
            catch (Exception ex)
            {
                await FatalExpection(methodName + "failed. Core failed to restart", ex).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Closes the Core and disables Events
        /// </summary>
#pragma warning disable 1998
        public async Task<bool> Close()
#pragma warning restore 1998
        {
            string methodName = "Core.Close";
            log.LogStandard(methodName, "Close called");
            try
            {
                if (_coreAvailable && !_coreStatChanging)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        _coreStatChanging = true;

                        _coreAvailable = false;
                        log.LogCritical(methodName, "called");

                        try
                        {
                            if (_subscriber != null)
                            {
                                log.LogEverything(methodName, "Subscriber requested to close connection");
                                _subscriber.Close().RunSynchronously();
                                log.LogEverything(methodName, "Subscriber closed");
                                _bus.Advanced.Workers.SetNumberOfWorkers(0);
                                _bus.Dispose();
                                _coreThreadRunning = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.LogException(methodName, "Subscriber failed to close", ex);
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

                        log.LogStandard(methodName, "Core closed");
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
                FatalExpection(methodName + " failed. Core failed to close", ex).RunSynchronously();
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

#pragma warning disable 1998
        public async Task FatalExpection(string reason, Exception exception)
#pragma warning restore 1998
        {
            string methodName = "Core.FatalExpection";
            _coreAvailable = false;
            _coreThreadRunning = false;
            _coreStatChanging = false;
            _coreRestarting = false;

            try
            {
                log.LogFatalException(methodName + " called for reason:'" + reason + "'", exception);
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
#pragma warning disable 1998
        public async Task<MainElement> TemplateFromXml(string xmlString)
#pragma warning restore 1998
        {
            if (string.IsNullOrEmpty(xmlString))
                throw new ArgumentNullException("xmlString cannot be null or empty");
            string methodName = "Core.TemplateFromXml";
            try
            {
                log.LogStandard(methodName, "called");
                log.LogEverything(methodName, "XML to transform:");
                log.LogEverything(methodName, xmlString);

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

                log.LogEverything(methodName, "XML after possible corrections:");
                log.LogEverything(methodName, xmlString);

                MainElement mainElement = new MainElement();
                mainElement = mainElement.XmlToClass(xmlString);

                //XML HACK
                mainElement.CaseType = "";
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                if (mainElement.Repeated < 1)
                {
                    log.LogCritical(methodName, "mainElement.Repeated = 1 // enforced");
                    mainElement.Repeated = 1;
                }

                return mainElement;
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.TemplateValidation";
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
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        private async Task<List<string>> FieldValidation(MainElement mainElement)
        {
            string methodName = "Core.FieldValidation";

            log.LogStandard(methodName, "called");

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

#pragma warning disable 1998
        private async Task<List<string>> CheckListValidation(MainElement mainElement)
#pragma warning restore 1998
        {
            string methodName = "Core.CheckListValidation";
            log.LogStandard(methodName, "called");
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

            string methodName = "Core.TemplateUploadData";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");

                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach (var dataItem in dataItems)
                    {
                        #region PDF
                        if (dataItem.GetType() == typeof(ShowPdf))
                        {
                            ShowPdf showPdf = (ShowPdf)dataItem;

                            if (PdfValidate(showPdf.Value, showPdf.Id).GetAwaiter().GetResult().Count != 0)
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
                log.LogException(methodName, "failed", ex);
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

            string methodName = "Core.TemplateCreate";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");

                    List<string> errors = await TemplateValidation(mainElement);

                    if (errors == null) errors = new List<string>();
                    if (errors.Count > 0)
                        throw new Exception("mainElement failed TemplateValidation. Run TemplateValidation to see errors");

                    int templateId = await _sqlController.TemplateCreate(mainElement);
                    log.LogEverything(methodName, "Template id:" + templateId.ToString() + " created in DB");
                    return templateId;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve the template MainElement from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        public async Task<MainElement> TemplateRead(int templateId)
        {
            string methodName = "Core.TemplateRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);

                    return await _sqlController.TemplateRead(templateId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to delete an eForm template in the Microting local DB. Returns if it was successfully
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        public async Task<bool> TemplateDelete(int templateId)
        {
            string methodName = "Core.TemplateDelete";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);

                    return await _sqlController.TemplateDelete(templateId).ConfigureAwait(false);                    
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve the template meta data from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        public async Task<Template_Dto> TemplateItemRead(int templateId)
        {
            string methodName = "Core.TemplateItemRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);

                    return await _sqlController.TemplateItemRead(templateId).ConfigureAwait(false);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(methodName, "(int " + templateId.ToString() + ") failed", ex);
                }
                catch
                {
                    log.LogException(methodName, "(int templateId) failed", ex);
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
            string methodName = "Core.TemplateItemReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(includeRemoved), includeRemoved);

                    return await TemplateItemReadAll(includeRemoved, Constants.WorkflowStates.Created, "", true, "", new List<int>()).ConfigureAwait(false);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(methodName, "(bool " + includeRemoved.ToString() + ") failed", ex);
                }
                catch
                {
                    log.LogException(methodName, "(bool includeRemoved) failed", ex);
                }
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<Template_Dto>> TemplateItemReadAll(bool includeRemoved, string siteWorkflowState, string searchKey, bool descendingSort, string sortParameter, List<int> tagIds)
        {
            string methodName = "Core.TemplateItemReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(includeRemoved), includeRemoved);
                    log.LogVariable(methodName, nameof(searchKey), searchKey);
                    log.LogVariable(methodName, nameof(descendingSort), descendingSort);
                    log.LogVariable(methodName, nameof(sortParameter), sortParameter);
                    log.LogVariable(methodName, nameof(tagIds), tagIds.ToString());

                    return await _sqlController.TemplateItemReadAll(includeRemoved, siteWorkflowState, searchKey, descendingSort, sortParameter, tagIds).ConfigureAwait(false);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> TemplateSetTags(int templateId, List<int> tagIds)
        {
            string methodName = "Core.TemplateSetTags";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(tagIds), tagIds.ToString());

                    return await _sqlController.TemplateSetTags(templateId, tagIds).ConfigureAwait(false);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
        public async Task<int?> CaseCreate(MainElement mainElement, string caseUId, int siteUid, int? folderId)
        {
            List<int> siteUids = new List<int>();
            siteUids.Add(siteUid);
            List<int> lst = await CaseCreate(mainElement, caseUId, siteUids, "", folderId).ConfigureAwait(false);

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
        public async Task<List<int>> CaseCreate(MainElement mainElement, string caseUId, List<int> siteUids, string custom, int? folderId)
        {
            string methodName = "Core.CaseCreate";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    string siteIdsStr = string.Join(",", siteUids);
                    log.LogVariable(methodName, nameof(caseUId), caseUId);
                    log.LogVariable(methodName, nameof(siteIdsStr), siteIdsStr);
                    log.LogVariable(methodName, nameof(custom), custom);

                    #region check input
                    DateTime start = DateTime.Parse(mainElement.StartDate.ToLongDateString());
                    DateTime end = DateTime.Parse(mainElement.EndDate.ToLongDateString());

                    if (end < DateTime.Now)
                    {
                        log.LogStandard(methodName, $"mainElement.EndDate is set to {end}");
                        throw new ArgumentException("mainElement.EndDate needs to be a future date");
                    }

                    if (end <= start)
                    {
                        log.LogStandard(methodName, $"mainElement.StartDat is set to {start}");
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
                            await _sqlController.CaseCreate(mainElement.Id, siteUid, mUId, null, caseUId, custom, DateTime.Now, folderId).ConfigureAwait(false);
                        else
                            await _sqlController.CheckListSitesCreate(mainElement.Id, siteUid, mUId, folderId).ConfigureAwait(false);

                        CaseDto cDto = await _sqlController.CaseReadByMUId(mUId);
                        //InteractionCaseUpdate(cDto);
                        try { HandleCaseCreated?.Invoke(cDto, EventArgs.Empty); }
                        catch { log.LogWarning(methodName, "HandleCaseCreated event's external logic suffered an Expection"); }
                        log.LogStandard(methodName, $"{cDto} has been created");

                        lstMUId.Add(mUId);
                    }
                    return lstMUId;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        /// <summary>
        /// Tries to retrieve the status of a case
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        public async Task<string> CaseCheck(int microtingUId)
        {
            string methodName = "Core.CaseCheck";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUId), microtingUId);

                    CaseDto cDto = await CaseLookupMUId(microtingUId).ConfigureAwait(false);
                    return await _communicator.CheckStatus(cDto.MicrotingUId.ToString(), cDto.SiteUId).ConfigureAwait(false);
                }
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.CaseRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUId), microtingUId);
                    log.LogVariable(methodName, nameof(checkUId), checkUId);

                    cases aCase = await _sqlController.CaseReadFull(microtingUId, checkUId).ConfigureAwait(false);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        log.LogWarning(methodName, $"No case found with MuuId:'{microtingUId}'");
                        return null;
                    }
                    #endregion

                    int id = aCase.Id;
                    log.LogEverything(methodName, $"aCase.Id:{aCase.Id}, found");

                    ReplyElement replyElement = await _sqlController.CheckRead(microtingUId, checkUId);
                    return replyElement;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<CaseDto> CaseReadByCaseId(int id)
        {
            string methodName = "Core.CaseReadByCaseId";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(id), id);

                    return await _sqlController.CaseReadByCaseId(id).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<int?> CaseReadFirstId(int? templateId, string workflowState)
        {
            string methodName = "Core.CaseReadFirstId";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(workflowState), workflowState);

                    return await _sqlController.CaseReadFirstId(templateId, workflowState);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }
        
        public Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end)
        {
            return CaseReadAll(templateId, start, end, Constants.WorkflowStates.NotRemoved, null);
        }

        public async Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, string searchKey)
        {
            string methodName = "Core.CaseReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(start), start);
                    log.LogVariable(methodName, nameof(end), end);
                    log.LogVariable(methodName, nameof(workflowState), workflowState);

                    return await CaseReadAll(templateId, start, end, workflowState, searchKey, false, null).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, string searchKey, bool descendingSort, string sortParameter)
        {
            string methodName = "Core.CaseReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(start), start);
                    log.LogVariable(methodName, nameof(end), end);
                    log.LogVariable(methodName, nameof(workflowState), workflowState);
                    log.LogVariable(methodName, nameof(descendingSort), descendingSort);
                    log.LogVariable(methodName, nameof(sortParameter), sortParameter);

                    return await _sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey, descendingSort, sortParameter).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<CaseList> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState, 
            string searchKey, bool descendingSort, string sortParameter, int pageIndex, int pageSize)
        {
            string methodName = "Core.CaseReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(start), start);
                    log.LogVariable(methodName, nameof(end), end);
                    log.LogVariable(methodName, nameof(workflowState), workflowState);
                    log.LogVariable(methodName, nameof(descendingSort), descendingSort);
                    log.LogVariable(methodName, nameof(sortParameter), sortParameter);
                    log.LogVariable(methodName, nameof(pageIndex), pageIndex);
                    log.LogVariable(methodName, nameof(pageSize), pageSize);
                    log.LogVariable(methodName, nameof(searchKey), searchKey);

                    return await _sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey, descendingSort, sortParameter, pageIndex, pageSize).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.CaseUpdate";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(caseId), caseId);

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
                        await _sqlController.FieldValueUpdate(caseId, id, value).ConfigureAwait(false);
                    }

                    foreach (string str in newCheckListValuePairLst)
                    {
                        id = int.Parse(t.SplitToList(str, 0, false));
                        value = t.SplitToList(str, 1, false);
                        await _sqlController.CheckListValueStatusUpdate(caseId, id, value).ConfigureAwait(false);
                    }

                    return true;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return false;
            }
        }

        public async Task<bool> CaseDelete(int templateId, int siteUId)
        {
            string methodName = "Core.CaseDelete";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(siteUId), siteUId);

                    return await CaseDelete(templateId, siteUId, Constants.WorkflowStates.NotRemoved).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(methodName, $"(int {templateId}, int {siteUId}) failed", ex);
                }
                catch
                {
                    log.LogException(methodName, "(int templateId, int siteUId) failed", ex);
                }
                return false;
            }
        }

        public async Task<bool> CaseDelete(int templateId, int siteUId, string workflowState)
        {
            string methodName = "Core.CaseDelete";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(siteUId), siteUId);
                    log.LogVariable(methodName, nameof(workflowState), workflowState);

                    List<string> errors = new List<string>();
                    foreach (int microtingUId in await _sqlController.CheckListSitesRead(templateId, siteUId, workflowState).ConfigureAwait(false))
                    {
                        if (! await CaseDelete(microtingUId).ConfigureAwait(false))
                        {
                            string error = $"Failed to delete case with microtingUId: {microtingUId}";
                            errors.Add(error);
                        }
                    }
                    if (errors.Count() > 0)
                    {
                        throw new Exception(String.Join("\n", errors));
                    }

                    return true;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(methodName,
                        $"(int {templateId}, int {siteUId}, string {workflowState}) failed", ex);
                }
                catch
                {
                    log.LogException(methodName, "(int templateId, int siteUId, string workflowState) failed", ex);
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
            string methodName = "Core.CaseDelete";

            log.LogVariable(methodName, nameof(microtingUId), microtingUId);

            var cDto = await _sqlController.CaseReadByMUId(microtingUId).ConfigureAwait(false);
            string xmlResponse = await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId).ConfigureAwait(false);
            log.LogEverything(methodName, "XML response is 1218 : " + xmlResponse);
            Response resp = new Response();

            if (xmlResponse.Contains("Error occured: Contact Microting"))
            {
                log.LogEverything(methodName, $"XML response is : {xmlResponse}");
                log.LogEverything("DELETE ERROR", $"failed for microtingUId: {microtingUId}");
                return false;
            }

            if (xmlResponse.Contains("Error"))
            {
                try
                {
                    resp = resp.XmlToClass(xmlResponse);
                    log.LogException(methodName, "failed", new Exception(
                        $"Error from Microting server: {resp.Value}"));
                    return false;
                }
                catch (Exception ex)
                {
                    try
                    {
                        log.LogException(methodName, $"(string {microtingUId}) failed", ex);
                        throw ex;
                    }
                    catch
                    {
                        log.LogException(methodName, "(string microtingUId) failed", ex);
                        throw ex;
                    }
                }
            }

            if (xmlResponse.Contains("Parsing in progress: Can not delete check list!"))
                for (int i = 1; i < 102; i++)
                {
                    Thread.Sleep(i * 5000);
                    xmlResponse = await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId).ConfigureAwait(false);
                    try
                    {
                        resp = resp.XmlToClass(xmlResponse);
                        if (resp.Type.ToString() == "Success")
                        {
                            log.LogStandard(methodName,
                                    cDto.ToString() +
                                    $" has been removed from server in retry loop with i being : {i.ToString()}");
                            break;
                        }                            
                        else
                        {
                            log.LogEverything(methodName,
                                    $"retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                        }
                    } catch (Exception ex)
                    {
                        log.LogEverything(methodName,
                            $" Exception is: {ex.Message}, retrying delete and i is {i.ToString()} and xmlResponse" +
                            xmlResponse);
                    }
                }

            log.LogEverything(methodName, "XML response:");
            log.LogEverything(methodName, xmlResponse);

            resp = resp.XmlToClass(xmlResponse);
            if (resp.Type.ToString() == "Success")
            {
                log.LogStandard(methodName, $"{cDto} has been removed from server");
                try
                {
                    bool result = await _sqlController.CaseDelete(microtingUId).ConfigureAwait(false);
                    try
                    {
                        await _sqlController.CaseDeleteReversed(microtingUId).ConfigureAwait(false);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "There is more than one instance.")
                        {
                            cDto = await _sqlController.CaseReadByMUId(microtingUId);
                            await FireHandleCaseDeleted(cDto).ConfigureAwait(false);
                            log.LogStandard(methodName, $"{cDto} has been removed");
                            return result;        
                        }

                        log.LogException(methodName, "(string microtingUId) failed", ex);
                        throw ex;
                    }                  
                }
                catch (Exception ex)
                {
                    log.LogException(methodName, "(string microtingUId) failed", ex);
                }
            }
            return false;
        }

        public async Task<bool> CaseDeleteResult(int caseId)
        {
            string methodName = "Core.CaseDeleteResult";
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(caseId), caseId);
            try
            {
                return await _sqlController.CaseDeleteResult(caseId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(methodName, $"(int {caseId}) failed", ex);
                }
                catch
                {
                    log.LogException(methodName, "(int caseId) failed", ex);
                }

                return false;
            }
        }

        public async Task<bool> CaseUpdateFieldValues(int id)
        {
            string methodName = "Core.CaseUpdateFieldValues";
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(id), id);
            try
            {
                return await _sqlController.CaseUpdateFieldValues(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return false;
            }
        }

        public async Task<CaseDto> CaseLookup(int microtingUId, int checkUId)
        {
            string methodName = "Core.CaseLookup";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUId), microtingUId);
                    log.LogVariable(methodName, nameof(checkUId), checkUId);

                    return await _sqlController.CaseLookup(microtingUId, checkUId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        /// <summary>
        /// Looks up the case's markers, from the match
        /// </summary>
        /// <param name="microtingUId">Microting unique ID of the eForm case</param>
        public async Task<CaseDto> CaseLookupMUId(int microtingUId)
        {
            string methodName = "Core.CaseLookupMUId";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUId), microtingUId);

                    return await _sqlController.CaseReadByMUId(microtingUId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        /// <summary>
        /// Looks up the case's markers, from the match
        /// </summary>
        /// <param name="CaseId">Microting DB's ID of the eForm case</param>
        public async Task<CaseDto> CaseLookupCaseId(int caseId)
        {
            string methodName = "Core.CaseLookupCaseId";

            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(caseId), caseId);

                    return await _sqlController.CaseReadByCaseId(caseId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        /// <summary>
        /// Looks up the case's markers, from the matches
        /// </summary>
        /// <param name="caseUId">Case's unique ID of the set of case(s)</param>
        public async Task<List<CaseDto>> CaseLookupCaseUId(string caseUId)
        {
            string methodName = "Core.CaseLookupCaseUId";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(caseUId), caseUId);

                    return await _sqlController.CaseReadByCaseUId(caseUId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.CaseIdLookup";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUId), microtingUId);
                    log.LogVariable(methodName, nameof(checkUId), checkUId);

                    cases aCase = await _sqlController.CaseReadFull(microtingUId, checkUId).ConfigureAwait(false);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        log.LogWarning(methodName, $"No case found with MuuId:'{microtingUId}'");
                        return -1;
                    }
                    #endregion
                    int id = aCase.Id;
                    log.LogEverything(methodName, $"aCase.Id:{aCase.Id}, found");

                    return id;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
        /// <param name="customPathForUploadedData"></param>
        /// <param name="decimalSeparator"></param>
        /// <param name="thousandSeparator"></param>
        /// <param name="utcTime"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="timeZoneInfo"></param>
        public async Task<string> CasesToCsv(int templateId, DateTime? start, DateTime? end, string pathAndName,
            string customPathForUploadedData, string decimalSeparator, string thousandSeparator, bool utcTime, CultureInfo cultureInfo, TimeZoneInfo timeZoneInfo)
        {
            string methodName = "Core.CasesToCsv";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId.ToString());
                    log.LogVariable(methodName, nameof(start), start.ToString());
                    log.LogVariable(methodName, nameof(end), end.ToString());
                    log.LogVariable(methodName, nameof(pathAndName), pathAndName);
                    log.LogVariable(methodName, nameof(customPathForUploadedData), customPathForUploadedData);

                    List<List<string>> dataSet = await GenerateDataSetFromCases(templateId, start, end, customPathForUploadedData, decimalSeparator, thousandSeparator, utcTime, cultureInfo, timeZoneInfo).ConfigureAwait(false);

                    if (dataSet == null)
                        return "";

                    // string text = "";
                    StringBuilder stringBuilder = new StringBuilder();

                    for (int rowN = 0; rowN < dataSet[0].Count; rowN++)
                    {
                        var temp = new List<string>();

                        foreach (List<string> lst in dataSet)
                        {
                            try
                            {
                                int.Parse(lst[rowN]);
                                temp.Add(lst[rowN]);
                            }
                            catch
                            {
                                DateTime date;
                                if (DateTime.TryParse(lst[rowN], out date))
                                {
                                    temp.Add(lst[rowN]);
                                }
                                else
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

                        stringBuilder.AppendLine(string.Join(";", temp.ToArray()));
                    }

                    if (!pathAndName.Contains(".csv"))
                        pathAndName = pathAndName + ".csv";

                    TextWriter textWriter = new StreamWriter(pathAndName, true, Encoding.UTF8);
                    textWriter.Write(stringBuilder.ToString());
                    textWriter.Flush();
                    textWriter.Close();
                    textWriter.Dispose();
                    return Path.GetFullPath(pathAndName);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
        public Task<string> CasesToCsv(int templateId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData)
        {
            CultureInfo cultureInfo = new CultureInfo("de-DE");
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
            return CasesToCsv(templateId, start, end, pathAndName, customPathForUploadedData, ".", "", false, cultureInfo, timeZoneInfo);
        }

        public async Task<string> CaseToJasperXml(CaseDto cDto, ReplyElement reply, int caseId, string timeStamp, string customPathForUploadedData, string customXMLContent)
        {
            string methodName = "Core.CaseToJasperXml";
            try
            {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(caseId), caseId.ToString());
                    log.LogVariable(methodName, nameof(timeStamp), timeStamp);

                    if (timeStamp == null)
                        timeStamp = $"{DateTime.Now:yyyyMMdd}_{DateTime.Now:hhmmss}";

                    //get needed data
//                    CaseDto cDto = CaseLookupCaseId(caseId);
//                    ReplyElement reply = CaseRead(cDto.MicrotingUId, cDto.CheckUId);
                    if (reply == null)
                        throw new NullReferenceException($"reply is null. Delete or fix the case with ID {caseId}");
                    string clsLst = "";
                    string fldLst = "";
                    GetChecksAndFields(ref clsLst, ref fldLst, reply.ElementList, customPathForUploadedData);
                    log.LogVariable(methodName, nameof(clsLst), clsLst);
                    log.LogVariable(methodName, nameof(fldLst), fldLst);

                    #region convert to jasperXml
                    string jasperXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + Environment.NewLine + "<root>"
                        + Environment.NewLine + "<C" + reply.Id + " case_id=\"" + caseId + "\" case_name=\"" + reply.Label + "\" serial_number=\"" + caseId + "/" + cDto.MicrotingUId + "\" check_list_status=\"approved\">"
                        + Environment.NewLine + "<worker>" + await Advanced_WorkerNameRead(reply.DoneById) + "</worker>"
                        + Environment.NewLine + "<check_id>" + reply.MicrotingUId + "</check_id>"
                        + Environment.NewLine + "<date>" + reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss") + "</date>"
                        + Environment.NewLine + "<check_date>" + reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss") + "</check_date>"
                        + Environment.NewLine + "<site_name>" + Advanced_SiteItemRead(reply.SiteMicrotingUuid).GetAwaiter().GetResult().SiteName + "</site_name>"
                        + Environment.NewLine + "<check_lists>"

                        + clsLst

                        + Environment.NewLine + "</check_lists>"
                        + Environment.NewLine + "<fields>"

                        + fldLst

                        + Environment.NewLine + "</fields>"
                        + Environment.NewLine + "</C" + reply.Id + ">"
                        + customXMLContent
                        + Environment.NewLine + "</root>";
                    log.LogVariable(methodName, nameof(jasperXml), jasperXml);
                    #endregion

                    //place in settings allocated placement
                    string fullPath = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results",
                        $"{timeStamp}_{caseId}.xml");
                    string path = await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false);
                    Directory.CreateDirectory(Path.Combine(path, "results"));
                    File.WriteAllText(fullPath, jasperXml.Trim(), Encoding.UTF8);

                    log.LogVariable(methodName, nameof(fullPath), fullPath);
                    return fullPath;
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        #region sdkSettings
        
        public async Task<string> GetSdkSetting(Settings settingName)
        {
            string methodName = "Core.GetSdkSetting";
            log.LogStandard(methodName, "called");
            try
            {
                return await _sqlController.SettingRead(settingName).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return "N/A";
            }
        }

        public async Task<bool> SetSdkSetting(Settings settingName, string settingValue)
        {
            string methodName = "Core.SetSdkSetting";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(settingValue), settingValue);

                    await _sqlController.SettingUpdate(settingName, settingValue).ConfigureAwait(false);
                    return true;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        public Task<string> CaseToPdf(int caseId, string jasperTemplate, string timeStamp, string customPathForUploadedData, string customXmlContent)
        {
            return CaseToPdf(caseId, jasperTemplate, timeStamp, customPathForUploadedData, "pdf", customXmlContent);
        }

        private async Task<string> JasperToPdf(int caseId, string jasperTemplate, string timeStamp)
        {
            string methodName = "Core.JasperToPdf";
            #region run jar
            // Start the child process.
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            string localJasperExporter = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "utils",
                "JasperExporter.jar");
            if (!File.Exists(localJasperExporter))
            {
                using (WebClient webClient = new WebClient())
                {
                    Directory.CreateDirectory(Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "utils"));
                    webClient.DownloadFile("https://github.com/microting/JasperExporter/releases/download/stable/JasperExporter.jar", localJasperExporter);
                }
            }

            string _templateFile = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "templates", jasperTemplate, "compact",
                $"{jasperTemplate}.jrxml");                    
            if (!File.Exists(_templateFile))
            {
                throw new FileNotFoundException($"jrxml template was not found at {_templateFile}");
            }
            string _dataSourceXML = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results",
                $"{timeStamp}_{caseId}.xml");

            if (!File.Exists(_dataSourceXML))
            {
                throw new FileNotFoundException("Case result xml was not found at " + _dataSourceXML);
            }
            string _resultDocument = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results",
                $"{timeStamp}_{caseId}.pdf");

            string command =
                $"-d64 -Xms512m -Xmx2g -Dfile.encoding=UTF-8 -jar {localJasperExporter} -template=\"{_templateFile}\" -type=\"pdf\" -uri=\"{_dataSourceXML}\" -outputFile=\"{_resultDocument}\"";

            log.LogVariable(methodName, nameof(command), command);
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
            log.LogVariable(methodName, nameof(output), output);
            p.WaitForExit();

            if (output != "")
                throw new Exception("output='" + output + "', expected to be no output. This indicates an error has happened");
            #endregion

            return _resultDocument;
        }

        private async Task<string> DocxToPdf(int caseId, string jasperTemplate, string timeStamp, ReplyElement reply, CaseDto cDto, string customPathForUploadedData, string customXmlContent, string fileType)
        {
            
            SortedDictionary<string, string> valuePairs = new SortedDictionary<string, string>();
            // get base values
            valuePairs.Add("F_CaseName", reply.Label.Replace("&", "&amp;"));
            valuePairs.Add("F_SerialNumber", $"{caseId}/{cDto.MicrotingUId}");
            valuePairs.Add("F_Worker", _sqlController.WorkerNameRead(reply.DoneById).GetAwaiter().GetResult().Replace("&", "&amp;"));
            valuePairs.Add("F_CheckId", reply.MicrotingUId.ToString());
            valuePairs.Add("F_CheckDate", reply.DoneAt.ToString("yyyy-MM-dd HH:mm:ss"));
            valuePairs.Add("F_SiteName", _sqlController.SiteRead(reply.SiteMicrotingUuid).GetAwaiter().GetResult().SiteName.Replace("&", "&amp;"));
            
            // get field_values
            List<KeyValuePair<string, List<string>>> pictures = new List<KeyValuePair<string, List<string>>>();
            List<KeyValuePair<string, string>> pictureGeotags = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> signatures = new List<KeyValuePair<string, string>>();
            List<int> caseIds = new List<int>();
            caseIds.Add(caseId);
            List<FieldValue> fieldValues = await _sqlController.FieldValueReadList(caseIds).ConfigureAwait(false);

            List<FieldDto> allFields = await _sqlController.TemplateFieldReadAll(int.Parse(jasperTemplate)).ConfigureAwait(false);
            foreach (FieldDto field in allFields)
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
                            fields field = await _sqlController.FieldReadRaw(fieldValue.FieldId).ConfigureAwait(false);
                            check_lists checkList = await _sqlController.CheckListRead((int)field.CheckListId).ConfigureAwait(false);

                            string geoTag = "";
                            if (fieldValue.Latitude != null)
                            {
                                geoTag =
                                    $"https://www.google.com/maps/place/{fieldValue.Latitude},{fieldValue.Longitude}";
                            }
                            var list = new List<string>();
                            list.Add(fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName);
                            list.Add(geoTag);
                            pictures.Add(new KeyValuePair<string, List<string>>($"{checkList.Label.Replace("&", "&amp;")} - {field.Label.Replace("&", "&amp;")}", list));
//                            if (fieldValue.Latitude != null) {
//                                pictureGeotags.Add(new KeyValuePair<string, string>($"{checkList.Label.Replace("&", "&amp;")} - {field.Label.Replace("&", "&amp;")}", ));
//                            }
                            if (_swiftEnabled)
                            {
                                SwiftObjectGetResponse swiftObjectGetResponse = await GetFileFromSwiftStorage(fieldValue.UploadedDataObj.FileName).ConfigureAwait(false);
                                Directory.CreateDirectory(fieldValue.UploadedDataObj.FileLocation);
                                var fileStream =
                                    File.Create(fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName);
                                swiftObjectGetResponse.ObjectStreamContent.Seek(0, SeekOrigin.Begin);
                                swiftObjectGetResponse.ObjectStreamContent.CopyTo(fileStream);
                                fileStream.Close();    
                            }

                            if (_s3Enabled)
                            {
                                GetObjectResponse fileFromS3Storage =
                                    await GetFileFromS3Storage(fieldValue.UploadedDataObj.FileName);
                                Directory.CreateDirectory(fieldValue.UploadedDataObj.FileLocation);
                                var fileStream =
                                    File.Create(fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName);
                                fileFromS3Storage.ResponseStream.CopyTo(fileStream);
                                fileStream.Close();
                                fileStream.Dispose();
                                fileFromS3Storage.ResponseStream.Close();
                                fileFromS3Storage.ResponseStream.Dispose();
                            }
                            
                            if (imageFieldCountList.ContainsKey($"FCount_{fieldValue.FieldId}"))
                            {
                                imageFieldCountList[$"FCount_{fieldValue.FieldId}"] += 1;
                            }
                        }
                        break;
                    case Constants.FieldTypes.Signature:
                        if (fieldValue.UploadedDataObj != null)
                        {
                            fields field = await _sqlController.FieldReadRaw(fieldValue.FieldId).ConfigureAwait(false);
                            check_lists checkList = await _sqlController.CheckListRead((int)field.CheckListId).ConfigureAwait(false);
                        
                            signatures.Add(new KeyValuePair<string, string>($"F_{fieldValue.FieldId}", fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName));
                            if (_swiftEnabled)
                            {
                                SwiftObjectGetResponse swiftObjectGetResponse = await GetFileFromSwiftStorage(fieldValue.UploadedDataObj.FileName).ConfigureAwait(false);
                                Directory.CreateDirectory(fieldValue.UploadedDataObj.FileLocation);
                                var fileStream =
                                    File.Create(fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName);
                                swiftObjectGetResponse.ObjectStreamContent.Seek(0, SeekOrigin.Begin);
                                swiftObjectGetResponse.ObjectStreamContent.CopyTo(fileStream);
                                fileStream.Close();    
                            }

                            if (_s3Enabled)
                            {
                                GetObjectResponse fileFromS3Storage =
                                    await GetFileFromS3Storage(fieldValue.UploadedDataObj.FileName).ConfigureAwait(false);
                                Directory.CreateDirectory(fieldValue.UploadedDataObj.FileLocation);
                                var fileStream =
                                    File.Create(fieldValue.UploadedDataObj.FileLocation + fieldValue.UploadedDataObj.FileName);
                                fileFromS3Storage.ResponseStream.CopyTo(fileStream);
                                fileStream.Close();
                                fileStream.Dispose();
                                fileFromS3Storage.ResponseStream.Close();
                                fileFromS3Storage.ResponseStream.Dispose();
                            }
                            
                            valuePairs.Remove($"F_{field.Id}");
                        }
                        break;
                    case Constants.FieldTypes.CheckBox:
                        valuePairs[$"F_{fieldValue.FieldId}"] = !string.IsNullOrEmpty(fieldValue.ValueReadable) ? (
                            fieldValue.ValueReadable.ToLower() == "checked" ? "&#10004;" : "") : "";
                        break;
                    case Constants.FieldTypes.FieldGroup:
                        break;
                    case Constants.FieldTypes.Timer:
                        valuePairs[$"F_{fieldValue.FieldId}"] = TimeSpan.FromMilliseconds(Double.Parse(fieldValue.Value.Split('|')[3])).ToString();
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
                                fieldValue.ValueReadable = fieldValue.ValueReadable.Replace("\t", @"</w:t><w:tab/><w:t>"); 
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

            string templateFile = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "templates", jasperTemplate, "compact",
                $"{jasperTemplate}.docx");  
            
            // Try to create the results directory first
            Directory.CreateDirectory(Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results"));
            
            string resultDocument = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results",
                $"{timeStamp}_{caseId}.docx");

            ReportHelper.SearchAndReplace(templateFile, valuePairs, resultDocument);
            
            ReportHelper.InsertImages(resultDocument, pictures);

            ReportHelper.InsertSignature(resultDocument, signatures);
            ReportHelper.ValidateWordDocument(resultDocument);
            
            if (fileType == "pdf")
            {
                string outputFolder = Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results");
            
                ReportHelper.ConvertToPdf(resultDocument, outputFolder);
                return Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results",
                    $"{timeStamp}_{caseId}.pdf");    
            }
            else
            {
                return Path.Combine(await _sqlController.SettingRead(Settings.fileLocationJasper).ConfigureAwait(false), "results",
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
            
            string methodName = "Core.CaseToPdf";
            try
            {
                //if (coreRunning)
                if (true)
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(caseId), caseId.ToString());
                    log.LogVariable(methodName, nameof(jasperTemplate), jasperTemplate);

                    if (timeStamp == null)
                        timeStamp = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss");
                    
                    CaseDto cDto = await CaseLookupCaseId(caseId).ConfigureAwait(false);
                    ReplyElement reply = await CaseRead((int)cDto.MicrotingUId, (int)cDto.CheckUId).ConfigureAwait(false);
                    
                    string resultDocument = "";

                    if (reply.JasperExportEnabled)
                    {
                        await CaseToJasperXml(cDto, reply, caseId, timeStamp, customPathForUploadedData, customXmlContent).ConfigureAwait(false);
                        resultDocument = await JasperToPdf(caseId, jasperTemplate, timeStamp).ConfigureAwait(false);
                    }
                    else
                    {
                        resultDocument = await DocxToPdf(caseId, jasperTemplate, timeStamp, reply, cDto, customPathForUploadedData, customXmlContent, fileType).ConfigureAwait(false);
                    }

                    //return path
                    string path = Path.GetFullPath(resultDocument);
                    log.LogVariable(methodName, nameof(path), path);
                    return path;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }
        #endregion

        #region site

        public async Task<SiteDto> SiteCreate(string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = "Core.SiteCreate";
            using (var db = dbContextHelper.GetDbContext())
            {
                try
                {
                    if (Running())
                    {
                        log.LogStandard(methodName, "called");
                        log.LogVariable(methodName, nameof(name), name);
                        log.LogVariable(methodName, nameof(userFirstName), userFirstName);
                        log.LogVariable(methodName, nameof(userLastName), userLastName);
                        log.LogVariable(methodName, nameof(userEmail), userEmail);

                        Tuple<SiteDto, UnitDto> siteResult = await _communicator.SiteCreate(name);

                        string token = await _sqlController.SettingRead(Settings.token).ConfigureAwait(false);
                        int customerNo = _communicator.OrganizationLoadAllFromRemote(token).GetAwaiter().GetResult()
                            .CustomerNo;

                        string siteName = siteResult.Item1.SiteName;
                        int siteId = siteResult.Item1.SiteId;
                        int unitUId = siteResult.Item2.UnitUId;
                        int otpCode = siteResult.Item2.OtpCode;
                        sites site =
                            await db.sites.SingleOrDefaultAsync(x => x.MicrotingUid == siteResult.Item1.SiteId).ConfigureAwait(false);
                        if (site == null)
                        {
                            site = new sites
                            {
                                MicrotingUid = siteId,
                                Name = siteName
                            };
                            await site.Create(db).ConfigureAwait(false);
                        }

                        SiteNameDto siteDto = await _sqlController.SiteRead(siteId).ConfigureAwait(false);
                        units unit = await db.units.SingleOrDefaultAsync(x => x.MicrotingUid == unitUId).ConfigureAwait(false);
                        if (unit == null)
                        {
                            unit = new units
                            {
                                MicrotingUid = unitUId,
                                CustomerNo = customerNo,
                                OtpCode = otpCode,
                                SiteId = site.Id
                            };

                            await unit.Create(db).ConfigureAwait(false);
                        }

                        if (string.IsNullOrEmpty(userEmail))
                        {
                            Random rdn = new Random();
                            userEmail = siteId + "." + customerNo + "@invalid.invalid";
                        }

                        WorkerDto workerDto = await Advanced_WorkerCreate(userFirstName, userLastName, userEmail)
                            .ConfigureAwait(false);
                        await Advanced_SiteWorkerCreate(siteDto, workerDto).ConfigureAwait(false);

                        return await SiteRead(siteId).ConfigureAwait(false);
                    }

                    throw new Exception("Core is not running");
                }
                catch (Exception ex)
                {
                    log.LogException(methodName, "failed", ex);
                    throw new Exception("failed", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <returns></returns>
        // TODO: Refactor to DeviceUserRead(int siteMicrotingUUID)
        public async Task<SiteDto> SiteRead(int microtingUid)
        {
            string methodName = "Core.SiteRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                    return await _sqlController.SiteReadSimple(microtingUid).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<SiteDto>> SiteReadAll(bool includeRemoved)
        {
            if (Running())
            {
                if (includeRemoved)
                    return await Advanced_SiteReadAll(null, null, null).ConfigureAwait(false);
                
                return await Advanced_SiteReadAll(Constants.WorkflowStates.NotRemoved, null, null).ConfigureAwait(false);
            }

            throw new Exception("Core is not running");
        }

        public async Task<SiteDto> SiteReset(int siteId)
        {
            string methodName = "Core.SiteReset";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(siteId), siteId);

                    SiteDto site = await SiteRead(siteId).ConfigureAwait(false);
                    await Advanced_UnitRequestOtp((int)site.UnitId).ConfigureAwait(false);

                    return await SiteRead(siteId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.SiteUpdate";
            try
            {
                if (Running())
                {
                    SiteDto siteDto = await SiteRead(siteMicrotingUid).ConfigureAwait(false);
                    await Advanced_SiteItemUpdate(siteMicrotingUid, siteName).ConfigureAwait(false);
                    if (String.IsNullOrEmpty(userEmail))
                    {
                        //if (String.IsNullOrEmpty)
                    }
                    await Advanced_WorkerUpdate((int)siteDto.WorkerUid, userFirstName, userLastName, userEmail).ConfigureAwait(false);
                    return true;
                }

                throw new Exception("Core is not running");

            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.SiteDelete";
            try
            {
                if (Running())
                {
                    SiteDto siteDto = await SiteRead(microtingUid).ConfigureAwait(false);

                    if (siteDto != null)
                    {
                        await Advanced_SiteItemDelete(microtingUid).ConfigureAwait(false);
                        SiteWorkerDto siteWorkerDto = await Advanced_SiteWorkerRead(null, microtingUid, siteDto.WorkerUid).ConfigureAwait(false);
                        await Advanced_SiteWorkerDelete(siteWorkerDto.MicrotingUId).ConfigureAwait(false);
                        await Advanced_WorkerDelete((int)siteDto.WorkerUid).ConfigureAwait(false);
                        return true;
                    }

                    return false;


                }

                throw new Exception("Core is not running");

            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.EntityGroupCreate";
            try
            {
                if (Running())
                {
                    EntityGroup entityGroup = await _sqlController.EntityGroupCreate(name, entityType).ConfigureAwait(false);

                    string entityGroupMUId = await _communicator.EntityGroupCreate(entityType, name, entityGroup.Id.ToString()).ConfigureAwait(false);

                    bool isCreated = await _sqlController.EntityGroupUpdate(entityGroup.Id, entityGroupMUId).ConfigureAwait(false);

                    if (isCreated)
                        return new EntityGroup()
                        {
                            Id = entityGroup.Id,
                            Name = entityGroup.Name,
                            Type = entityGroup.Type,
                            EntityGroupItemLst = new List<EntityItem>(),
                            MicrotingUUID = entityGroupMUId
                        };
                    await _sqlController.EntityGroupDelete(entityGroupMUId).ConfigureAwait(false);
                    throw new Exception("EntityListCreate failed, due to list not created correct");
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "EntityListCreate failed", ex);
                throw new Exception("EntityListCreate failed", ex);
            }
        }

        /// <summary>
        /// Returns the EntityGroup and its EntityItems
        /// </summary>
        /// <param name="entityGroupMuId">The unique microting id of the EntityGroup</param>
        public async Task<EntityGroup> EntityGroupRead(string entityGroupMuId)
        {
            if (string.IsNullOrEmpty(entityGroupMuId))
                throw new ArgumentNullException(nameof(entityGroupMuId));
            return await EntityGroupRead(entityGroupMuId, Constants.EntityItemSortParameters.DisplayIndex, "").ConfigureAwait(false);
        }

        public async Task<EntityGroup> EntityGroupRead(string entityGroupMuId, string sort, string nameFilter)
        {
            string methodName = "Core.EntityGroupRead";
            if (string.IsNullOrEmpty(entityGroupMuId))
                throw new ArgumentNullException(nameof(entityGroupMuId));
            try
            {
                if (Running())
                {
                    while (_updateIsRunningEntities)
                        Thread.Sleep(200);

                    return await _sqlController.EntityGroupReadSorted(entityGroupMuId, sort, nameFilter).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                try
                {
                    log.LogException(methodName, "(string entityGroupMUId " + entityGroupMuId + ", string sort " + sort + ", string nameFilter " + nameFilter + ") failed", ex);
                }
                catch
                {
                    log.LogException(methodName, "(string entityGroupMUId, string sort, string nameFilter) failed", ex);
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
            string methodName = "Core.EntityGroupUpdate";
            try
            {
                if (Running())
                {
                    bool isUpdated = await _communicator.EntityGroupUpdate(entityGroup.Type, entityGroup.Name, entityGroup.Id, entityGroup.MicrotingUUID).ConfigureAwait(false);

                    if (isUpdated)
                        return await _sqlController.EntityGroupUpdateName(entityGroup.Name, entityGroup.MicrotingUUID).ConfigureAwait(false);
                }
                
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "EntityGroupRead failed", ex);
                throw new Exception("EntityGroupRead failed", ex);
            }
        }

        /// <summary>
        /// Deletes an EntityGroup, both its items should be deleted before using
        /// </summary>
        /// <param name="entityGroupMUId">The unique microting id of the EntityGroup</param>
        public async Task<bool> EntityGroupDelete(string entityGroupMUId)
        {
            string methodName = "Core.EntityGroupDelete";
            try
            {
                if (Running())
                {
                    while (_updateIsRunningEntities)
                        Thread.Sleep(200);

                    EntityGroup entityGroup = await _sqlController.EntityGroupRead(entityGroupMUId).ConfigureAwait(false);
                    await _communicator.EntityGroupDelete(entityGroup.Type, entityGroupMUId).ConfigureAwait(false);
                    string type = await _sqlController.EntityGroupDelete(entityGroupMUId).ConfigureAwait(false);
                    return true;
                }
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "EntityGroupDelete failed", ex);
                throw new Exception("EntityGroupDelete failed", ex);
            }
        }

        #region EntityItem

        public Task<EntityItem> EntitySearchItemCreate(int entitItemGroupId, string name, string description, string ownUUID) {
            return EntityItemCreate(entitItemGroupId, name, description, ownUUID, 0);
        }

        public Task<EntityItem> EntitySelectItemCreate(int entitItemGroupId, string name, int displayIndex, string ownUUID) {
            return EntityItemCreate(entitItemGroupId, name, "", ownUUID, displayIndex);
        }

        private async Task<EntityItem> EntityItemCreate(int entitItemGroupId, string name, string description, string ownUUID, int displayIndex)
        {
            EntityGroup eg = await _sqlController.EntityGroupRead(entitItemGroupId).ConfigureAwait(false);
            EntityItem et = await _sqlController.EntityItemRead(entitItemGroupId, name, description).ConfigureAwait(false);
            if (et == null) {
                string microtingUId;
                if (eg.Type == Constants.FieldTypes.EntitySearch) {
                    microtingUId = await _communicator.EntitySearchItemCreate(eg.MicrotingUUID, name, description, ownUUID).ConfigureAwait(false);
                } else {
                    microtingUId = await _communicator.EntitySelectItemCreate(eg.MicrotingUUID, name, displayIndex, ownUUID).ConfigureAwait(false);
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
                    return await _sqlController.EntityItemCreate(eg.Id, et).ConfigureAwait(false);
                }

                return null;
            }

            if (et.WorkflowState == Constants.WorkflowStates.Removed)
            {
                et.WorkflowState = Constants.WorkflowStates.Created;
                await _sqlController.EntityItemUpdate(et).ConfigureAwait(false);
            }
            return et;
        }

        public async Task EntityItemUpdate(int id, string name, string description, string ownUUID, int displayIndex)
        {
            using (var dbContext = dbContextHelper.GetDbContext())
            {
                entity_items et = await dbContext.entity_items.SingleOrDefaultAsync(x => x.Id == id);
                if (et == null) {
                    throw new NullReferenceException("EntityItem not found with id " + id);
                }

                if (et.Name != name || et.Description != description || et.DisplayIndex != displayIndex ||
                    et.EntityItemUid != ownUUID)
                {
                    entity_groups eg =
                        await dbContext.entity_groups.SingleOrDefaultAsync(x =>
                            x.Id == et.EntityGroupId);
                    bool result = false;
                    if (eg.Type == Constants.FieldTypes.EntitySearch)
                    {
                        result = await _communicator
                            .EntitySearchItemUpdate(eg.MicrotingUid, et.MicrotingUid,
                                name, description, ownUUID)
                            .ConfigureAwait(false);
                    } else {
                        result = await _communicator
                            .EntitySelectItemUpdate(eg.MicrotingUid, et.MicrotingUid,
                                name, displayIndex, ownUUID)
                            .ConfigureAwait(false);
                    }
                    if (result) {
                        et.DisplayIndex = displayIndex;
                        et.Name = name;
                        et.Description = description;
                        et.EntityItemUid = ownUUID;
                        await et.Update(dbContext);
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

            EntityGroup eg = await _sqlController.EntityGroupRead(et.EntityItemGroupId).ConfigureAwait(false);
            bool result = false;
            if (eg.Type == Constants.FieldTypes.EntitySearch) {
                result = await _communicator.EntitySearchItemDelete(et.MicrotingUUID).ConfigureAwait(false);
            } else {
                result = await _communicator.EntitySelectItemDelete(et.MicrotingUUID).ConfigureAwait(false);
            }
            if (result) {
                await _sqlController.EntityItemDelete(id).ConfigureAwait(false);
            }
            else
            {
                throw new Exception("Unable to update entityItem with id " + id.ToString());
            }
        }

        #endregion

        public async Task<string> PdfUpload(string localPath)
        {
            string methodName = "Core.PdfUpload";
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

                    if (await _communicator.PdfUpload(localPath, chechSum).ConfigureAwait(false))
                        return chechSum;
                    else
                    {
                        log.LogWarning(methodName, "Uploading of PDF failed");
                        return null;
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion
        
        #region folder

        public async Task<List<FolderDto>> FolderGetAll(bool includeRemoved)
        {
            string methodName = "Core.FolderGetAll";
            try
            {
                if (Running())
                {
                    List<FolderDto> folderDtos = await _sqlController.FolderGetAll(includeRemoved).ConfigureAwait(false);

                    return folderDtos;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "FolderGetAll failed", ex);
                throw new Exception("FolderGetAll failed", ex);
            }
        }

        public async Task<FolderDto> FolderRead(int id)
        {
            string methodName = "Core.FolderRead";
            try
            {
                if (Running())
                {
                    FolderDto folderDto = await _sqlController.FolderRead(id).ConfigureAwait(false);

                    return folderDto;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "FolderRead failed", ex);
                throw new Exception("FolderRead failed", ex);
            }
        }


        public async Task FolderCreate(string name, string description, int? parent_id)
        {
            string methodName = "Core.FolderCreate";
            try
            {
                if (Running())
                {
                    int apiParentId = 0;
                    if (parent_id != null)
                    {
                        apiParentId = (int)FolderRead((int) parent_id).GetAwaiter().GetResult().MicrotingUId;
                    }
                    int id = await _communicator.FolderCreate(name, description, apiParentId).ConfigureAwait(false);
                    int result = await _sqlController.FolderCreate(name, description, parent_id, id).ConfigureAwait(false);
                    return;

                }
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "FolderCreate failed", ex);
                throw new Exception("FolderCreate failed", ex);
            }
        }

        public async Task FolderUpdate(int id, string name, string description, int? parent_id)
        {
            string methodName = "Core.FolderUpdate";
            try
            {
                if (Running())
                {
                    FolderDto folder = await FolderRead(id).ConfigureAwait(false);
                    int apiParentId = 0;
                    if (parent_id != null)
                    {
                        apiParentId = (int)FolderRead((int) parent_id).GetAwaiter().GetResult().MicrotingUId;
                    }
                    await _communicator.FolderUpdate((int)folder.MicrotingUId, name, description, apiParentId).ConfigureAwait(false);
                    await _sqlController.FolderUpdate(id, name, description, parent_id).ConfigureAwait(false);
                    return;
                }
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "FolderUpdate failed", ex);
                throw new Exception("FolderUpdate failed", ex);
            }
        }

        public async Task FolderDelete(int id)
        {
            string methodName = "Core.FolderDelete";
            try
            {
                if (Running())
                {
                    FolderDto folder = await FolderRead(id).ConfigureAwait(false);
                    bool success = await _communicator.FolderDelete((int)folder.MicrotingUId).ConfigureAwait(false);
                    if (success)
                    {
                        await _sqlController.FolderDelete(id).ConfigureAwait(false);
                    }

                    return;
                }
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "FolderDelete failed", ex);
                throw new Exception("FolderDelete failed", ex);
            }
        }
        #endregion
        
        #endregion

        #region tags
        public async Task<List<Tag>> GetAllTags(bool includeRemoved)
        {
            string methodName = "Core.GetAllTags";
            try
            {
                if (Running())
                {
                    return await _sqlController.GetAllTags(includeRemoved).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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
            string methodName = "Core.TagCreate";
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name is not allowed to be null or empty");
            }
            try
            {
                if (Running())
                {
                    return await _sqlController.TagCreate(name).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }



        public async Task<bool> TagDelete(int tagId)
        {
            string methodName = "Core.TagDelete";
            try
            {
                if (Running())
                {
                    return await _sqlController.TagDelete(tagId);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        #endregion


        #region speach to text

        public async Task<bool> TranscribeUploadedData(int uploadedDataId)
        {
            string methodName = "Core.TranscribeUploadedData";
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
                            log.LogStandard(methodName, $"filePath is {filePath}");
                            int requestId = await SpeechToText(filePath).ConfigureAwait(false);
                            uploadedData.TranscriptionId = requestId;

                            await _sqlController.UpdateUploadedData(uploadedData).ConfigureAwait(false);
                        }
                        return true;
                    }

                    return false;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<int> SpeechToText(string pathToAudioFile)
        {
            string methodName = "Core.SpeechToText";
            try
            {
                if (Running())
                {
                    return await _communicator.SpeechToText(pathToAudioFile).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

//        public async Task<bool> SpeechToText(int requestId)
//        {
//            string methodName = "Core.SpeechToText";
//            try
//            {
//                if (Running())
//                {
//                    //Tuple<Site_Dto, Unit_Dto> siteResult = communicator.SiteCreate(name);
//                    return true;
//                }
//                else
//                    throw new Exception("Core is not running");
//            }
//            catch (Exception ex)
//            {
//                log.LogException(methodName, "failed", ex);
//                throw new Exception("failed", ex);
//            }
//        }

        #endregion

        #region InSight

        #region SurveyConfiguration
        
        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            using (var dbContext = dbContextHelper.GetDbContext())
            {
                site_survey_configurations siteSurveyConfiguration =
                    await dbContext.site_survey_configurations.SingleOrDefaultAsync(x => x.SiteId == siteId && x.SurveyConfigurationId == id).ConfigureAwait(false);

                if (siteSurveyConfiguration == null)
                {
                    
                }
            }
            
            return true;
        }

        public async Task<bool> GetAllSurveyConfigurations()
        {
            var parsedData = JObject.Parse(await _communicator.GetAllSurveyConfigurations().ConfigureAwait(false));

            foreach (var item in parsedData)
            {
                foreach (JToken subItem in item.Value)
                {
                    using (var db = dbContextHelper.GetDbContext())
                    {
                        string name = subItem["Name"].ToString();
                        int microtingUid = int.Parse(subItem["MicrotingUid"].ToString());
                        var innerParsedData = JObject.Parse(await _communicator.GetSurveyConfiguration(microtingUid).ConfigureAwait(false));

                        JToken parsedQuestionSet = innerParsedData.GetValue("QuestionSet");

                        if (parsedQuestionSet != null)
                        {
                            int questionSetMicrotingUid = int.Parse(parsedQuestionSet["MicrotingUid"].ToString());
                            var questionSet = await db.question_sets.SingleOrDefaultAsync(x => x.MicrotingUid == questionSetMicrotingUid).ConfigureAwait(false);
                            if (questionSet != null)
                            {
                                questionSet.Name = parsedQuestionSet["Name"].ToString();
                                await questionSet.Update(db);
                            }
                            else
                            {
                                questionSet = new question_sets()
                                {
                                    Name = parsedQuestionSet["Name"].ToString(),
                                    MicrotingUid = questionSetMicrotingUid
                                };
                                await questionSet.Create(db);
                            }

                            var surveyConfiguration = 
                                await db.survey_configurations.SingleOrDefaultAsync(x =>
                                    x.MicrotingUid == microtingUid).ConfigureAwait(false);
                            if (surveyConfiguration != null)
                            {
                                surveyConfiguration.Name = name;
                                if (subItem["WorkflowState"].ToString().Equals("active"))
                                {
                                    surveyConfiguration.WorkflowState = Constants.WorkflowStates.Active;
                                }
                                else
                                {
                                    surveyConfiguration.WorkflowState = Constants.WorkflowStates.Created;
                                }
                                await surveyConfiguration.Update(db).ConfigureAwait(false);
                            }
                            else
                            {
                                surveyConfiguration = new survey_configurations()
                                {
                                    MicrotingUid = microtingUid,
                                    Name = name,
                                    QuestionSetId = questionSet.Id,
                                    WorkflowState = subItem["WorkflowState"].ToString().Equals("active")
                                        ? Constants.WorkflowStates.Active : Constants.WorkflowStates.Created
                                };
                                await surveyConfiguration.Create(db).ConfigureAwait(false);
                            }

                            foreach (JToken child in innerParsedData.GetValue("Sites").Children())
                            {
                                var site = await db.sites.SingleOrDefaultAsync(x => x.MicrotingUid == int.Parse(child["MicrotingUid"].ToString())).ConfigureAwait(false);
                                if (site != null)
                                {
                                    var siteSurveyConfiguration =
                                        await db.site_survey_configurations.SingleOrDefaultAsync(x =>
                                            x.SiteId == site.Id && x.SurveyConfigurationId == surveyConfiguration.Id).ConfigureAwait(false);
                                    if (siteSurveyConfiguration == null)
                                    {
                                        siteSurveyConfiguration = new site_survey_configurations()
                                        {
                                            SiteId = site.Id,
                                            SurveyConfigurationId = surveyConfiguration.Id
                                        };
                                        await siteSurveyConfiguration.Create(db).ConfigureAwait(false);
                                    }
                                    else
                                    {
                                        siteSurveyConfiguration.WorkflowState = Constants.WorkflowStates.Created;
                                        await siteSurveyConfiguration.Update(db).ConfigureAwait(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion
        
        #region QuestionSet

        public async Task<bool> GetAllQuestionSets()
        {
            var parsedData = JObject.Parse(await _communicator.GetAllQuestionSets().ConfigureAwait(false));

            if (!parsedData.Any())
                return false;
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            using (var db = dbContextHelper.GetDbContext())
            {
                var language = await db.languages.SingleOrDefaultAsync(x => x.Name == "Danish").ConfigureAwait(false);
                if (language == null)
                {
                    language = new languages()
                    {
                        Name = "Danish"
                    };
                    await language.Create(db);
                }
                
                foreach (var item in parsedData)
                {
                    foreach (JToken subItem in item.Value)
                    {
                        int questionSetMicrotingUid = int.Parse(subItem["MicrotingUid"].ToString());
                        var questionSet = await db.question_sets.SingleOrDefaultAsync(x => x.MicrotingUid == questionSetMicrotingUid).ConfigureAwait(false);
                        if (questionSet != null)
                        {
                            questionSet.Name = subItem["Name"].ToString();
                            await questionSet.Update(db).ConfigureAwait(false);
                        }
                        else
                        {
                            questionSet = new question_sets()
                            {
                                Name = subItem["Name"].ToString(),
                                MicrotingUid = questionSetMicrotingUid
                            };
                            await questionSet.Create(db).ConfigureAwait(false);
                        }
                        
                        var innerParsedData = JObject.Parse(await _communicator.GetQuestionSet(questionSetMicrotingUid).ConfigureAwait(false));
                        
                        JToken parsedQuestions = innerParsedData.GetValue("Questions");
                        foreach (JToken child in parsedQuestions.Children())
                        {
                            var question = await db.questions.SingleOrDefaultAsync(x =>
                                x.MicrotingUid == int.Parse(child["MicrotingUid"].ToString())).ConfigureAwait(false);
                            if (question == null)
                            {
                                var result = JsonConvert.DeserializeObject<questions>(child.ToString(), jsonSerializerSettings);
                                result.QuestionSetId = questionSet.Id;
                                await result.Create(db, false).ConfigureAwait(false);
                            }
                            else
                            {
                                question.WorkflowState = child["WorkflowState"].ToString();
                                await question.Update(db).ConfigureAwait(false);
                            }
                        }
                        JToken parsedQuestionTranslations = innerParsedData.GetValue("QuestionTranslations");
                        foreach (JToken child in parsedQuestionTranslations.Children())
                        {
                            var questionTranslation =
                                await db.QuestionTranslations.SingleOrDefaultAsync(x =>
                                    x.MicrotingUid == int.Parse(child["MicrotingUid"].ToString())).ConfigureAwait(false);
                            if (questionTranslation == null)
                            {
                                var result = JsonConvert.DeserializeObject<question_translations>(child.ToString());
                                result.QuestionId = db.questions.Single(x => x.MicrotingUid == result.QuestionId).Id;
                                result.LanguageId = language.Id;
                                await result.Create(db).ConfigureAwait(false);

                            }
                            else
                            {
                                questionTranslation.Name = child["Name"].ToString();
                                questionTranslation.WorkflowState = child["WorkflowState"].ToString();
                                await questionTranslation.Update(db);
                            }
                        }
                        
                        JToken parsedOptions = innerParsedData.GetValue("Options");
                        int i = 0;
                        foreach (JToken child in parsedOptions.Children())
                        {
                            var option = await db.options.SingleOrDefaultAsync(x =>
                                x.MicrotingUid == int.Parse(child["MicrotingUid"].ToString())).ConfigureAwait(false);
                            if (option == null)
                            {
                                var result = JsonConvert.DeserializeObject<options>(child.ToString());
                                var nextQuestionId =
                                    db.questions.SingleOrDefault(x => x.MicrotingUid == result.NextQuestionId);
                                var question = db.questions.Single(x => x.MicrotingUid == result.QuestionId);
                                result.QuestionId = question.Id;
                                result.NextQuestionId = nextQuestionId?.Id;
                                await result.Create(db).ConfigureAwait(false);
                            }
                            else
                            {
                                try
                                {
                                    option.WorkflowState = child["WorkflowState"].ToString();
                                    option.WeightValue = int.Parse(child["WeightValue"].ToString());
                                    option.OptionIndex = int.Parse(child["OptionIndex"].ToString());

                                    int? nextQuestionId = null;
                                    if (!string.IsNullOrEmpty(child["NextQuestionId"].ToString()))
                                    {
                                        nextQuestionId = db.questions.SingleOrDefault(x =>
                                            x.MicrotingUid == int.Parse(child["NextQuestionId"].ToString()))?.Id;
                                    }
                                    option.NextQuestionId = nextQuestionId;
                                    await option.Update(db).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            i += 1;
                        }
                        
                        JToken parsedOptionTranslations = innerParsedData.GetValue("OptionTranslations");
                        foreach (JToken child in parsedOptionTranslations.Children())
                        {
                            var optionTranslation =
                                await db.OptionTranslations.SingleOrDefaultAsync(x =>
                                    x.MicrotingUid == int.Parse(child["MicrotingUid"].ToString())).ConfigureAwait(false);
                            if (optionTranslation == null)
                            {
                                var result = JsonConvert.DeserializeObject<option_translations>(child.ToString());
                                result.OptionId = db.options.Single(x => x.MicrotingUid == result.OptionId).Id;
                                result.LanguageId = language.Id;
                                await result.Create(db).ConfigureAwait(false);
                            }
                            else
                            {
                                optionTranslation.Name = child["Name"].ToString();
                                optionTranslation.WorkflowState = child["WorkflowState"].ToString();
                                await optionTranslation.Update(db).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
            return true;
        }
        
        #endregion
        
        #region Answer

        public async Task<bool> GetAllAnswers()
        {
            using (var db = dbContextHelper.GetDbContext())
            {
                foreach (question_sets questionSet in await db.question_sets.ToListAsync())
                {
                    await GetAnswersForQuestionSet(questionSet.MicrotingUid).ConfigureAwait(false);
                }
            }

            return true;
        }

        private async Task<int> SaveAnswers(question_sets questionSet, JObject parsedData)
        {
            var settings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
            int numAnswers = 0;

            using (var db = dbContextHelper.GetDbContext())
            {
                foreach (var item in parsedData)
                {
                    foreach (JToken subItem in item.Value)
                    {
                        answers answer = JsonConvert.DeserializeObject<answers>(subItem.ToString(), settings);

                        var result = db.answers.SingleOrDefault(x => x.MicrotingUid == answer.MicrotingUid);
                        if (result != null)
                        {
                            answer.Id = result.Id;
                        }

                        if (result == null)
                        {
                            units unit = await db.units.SingleOrDefaultAsync(x => x.MicrotingUid == answer.UnitId).ConfigureAwait(false);
                            if (unit != null)
                            {
                                answer.UnitId = unit.Id;
                            }
                            else
                            {
                                answer.UnitId = null;
                            }
                            answer.SiteId = db.sites.Single(x => x.MicrotingUid == answer.SiteId).Id;
                            try
                            {
                                if (questionSet == null)
                                {
                                    await GetAllSurveyConfigurations().ConfigureAwait(false);
                                    questionSet =
                                        await db.question_sets.SingleOrDefaultAsync(x =>
                                            x.MicrotingUid == answer.QuestionSet.MicrotingUid).ConfigureAwait(false);
                                }
                                answer.QuestionSetId = questionSet.Id;
                                survey_configurations surveyConfiguration = await db.survey_configurations
                                    .SingleOrDefaultAsync(x => x.MicrotingUid == answer.SurveyConfigurationId)
                                    .ConfigureAwait(false);
                                if (surveyConfiguration == null)
                                {
                                    await GetAllSurveyConfigurations().ConfigureAwait(false);
                                    surveyConfiguration = await db.survey_configurations
                                                         .SingleOrDefaultAsync(x => x.MicrotingUid == answer.SurveyConfigurationId)
                                                         .ConfigureAwait(false);
                                }

                                if (surveyConfiguration != null)
                                {
                                    answer.SurveyConfigurationId = surveyConfiguration.Id;
                                    answer.QuestionSet = null;
                                    answer.LanguageId = db.languages.Single(x => x.Name == "Danish").Id;
                                    await answer.Create(db).ConfigureAwait(false);
                                    foreach (JToken avItem in subItem["AnswerValues"])
                                    {
                                        answer_values answerValue =
                                            JsonConvert.DeserializeObject<answer_values>(avItem.ToString(), settings);
                                        if (db.answer_values.SingleOrDefault(x => x.MicrotingUid == answerValue.MicrotingUid) ==
                                            null)
                                        {
                                            var question = db.questions.Single(x => x.MicrotingUid == answerValue.QuestionId);
                                            var option = db.options.Single(x => x.MicrotingUid == answerValue.OptionId);
                                            if (question.QuestionType == Constants.QuestionTypes.Buttons || question.QuestionType == Constants.QuestionTypes.List || question.QuestionType == Constants.QuestionTypes.Multi)
                                            {
                                                answerValue.Value = option.OptionTranslationses.First().Name;
                                            }
                                            answerValue.AnswerId = answer.Id;
                                            answerValue.QuestionId =
                                                question.Id;
                                            answerValue.OptionId =
                                                option.Id;
                                            await answerValue.Create(db).ConfigureAwait(false);
                                        }
                                    }
                                }
                                
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            numAnswers ++;
                        }

                        
                    }
                }
            }

            return numAnswers;
        }

        public async Task GetAnswersForQuestionSet(int? questionSetId)
        {
            if (questionSetId == null)
                return;
            
            using (var db = dbContextHelper.GetDbContext())
            {
                int numAnswers = 10;
                question_sets questionSet =
                    await db.question_sets.SingleOrDefaultAsync(x => x.MicrotingUid == questionSetId).ConfigureAwait(false);
                if (questionSet != null)
                {
                    var lastAnswer = await db.answers.LastOrDefaultAsync(x => x.QuestionSetId == questionSet.Id).ConfigureAwait(false);
                    JObject parsedData = null;
                    if (lastAnswer != null)
                    {
                        while (numAnswers > 9)
                        {
                            try
                            {
                                lastAnswer = await db.answers.LastOrDefaultAsync(x => x.QuestionSetId == questionSet.Id)
                                    .ConfigureAwait(false);
                                if (lastAnswer != null)
                                {
                                    parsedData = JObject.Parse(await _communicator
                                        .GetLastAnswer((int) questionSetId, (int) lastAnswer.MicrotingUid)
                                        .ConfigureAwait(false));
                                    numAnswers = await SaveAnswers(questionSet, parsedData).ConfigureAwait(false);
                                }
                                else
                                {
                                    numAnswers = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        parsedData = JObject.Parse(await _communicator.GetLastAnswer((int)questionSetId, 0).ConfigureAwait(false));
                        numAnswers = await SaveAnswers(questionSet, parsedData).ConfigureAwait(false);

                        while (numAnswers > 9)
                        {
                            try
                            {
                                lastAnswer = await db.answers.LastOrDefaultAsync(x => x.QuestionSetId == questionSet.Id)
                                    .ConfigureAwait(false);
                                if (lastAnswer != null)
                                {
                                    parsedData = JObject.Parse(await _communicator
                                        .GetLastAnswer((int) questionSetId, (int) lastAnswer.MicrotingUid)
                                        .ConfigureAwait(false));
                                    numAnswers = await SaveAnswers(questionSet, parsedData).ConfigureAwait(false);
                                }
                                else
                                {
                                    numAnswers = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }
        
        #endregion
        
        #endregion
        
        #region public advanced actions
        #region templat
        public async Task<bool> Advanced_TemplateDisplayIndexChangeDb(int templateId, int newDisplayIndex)
        {
            string methodName = "Core.Advanced_TemplateDisplayIndexChangeDb";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(newDisplayIndex), newDisplayIndex);

                    return await _sqlController.TemplateDisplayIndexChange(templateId, newDisplayIndex).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_TemplateDisplayIndexChangeServer(int templateId, int siteUId, int newDisplayIndex)
        {
            string methodName = "Core.Advanced_TemplateDisplayIndexChangeServer";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(siteUId), siteUId);
                    log.LogVariable(methodName, nameof(newDisplayIndex), newDisplayIndex);

                    string respXml = null;
                    List<string> errors = new List<string>();
                    foreach (int microtingUId in await _sqlController.CheckListSitesRead(templateId, siteUId, Constants.WorkflowStates.NotRemoved).ConfigureAwait(false))
                    {
                        respXml = await _communicator.TemplateDisplayIndexChange(microtingUId.ToString(), siteUId, newDisplayIndex).ConfigureAwait(false);
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

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_TemplateUpdateFieldIdsForColumns(int templateId, int? fieldId1, int? fieldId2, int? fieldId3, int? fieldId4, int? fieldId5, int? fieldId6, int? fieldId7, int? fieldId8, int? fieldId9, int? fieldId10)
        {
            string methodName = "Core.Advanced_TemplateUpdateFieldIdsForColumns";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);
                    log.LogVariable(methodName, nameof(fieldId1), fieldId1);
                    log.LogVariable(methodName, nameof(fieldId2), fieldId2);
                    log.LogVariable(methodName, nameof(fieldId3), fieldId3);
                    log.LogVariable(methodName, nameof(fieldId4), fieldId4);
                    log.LogVariable(methodName, nameof(fieldId5), fieldId5);
                    log.LogVariable(methodName, nameof(fieldId6), fieldId6);
                    log.LogVariable(methodName, nameof(fieldId7), fieldId7);
                    log.LogVariable(methodName, nameof(fieldId8), fieldId8);
                    log.LogVariable(methodName, nameof(fieldId9), fieldId9);
                    log.LogVariable(methodName, nameof(fieldId10), fieldId10);

                    return await _sqlController.TemplateUpdateFieldIdsForColumns(templateId, fieldId1, fieldId2,
                            fieldId3, fieldId4, fieldId5, fieldId6, fieldId7, fieldId8, fieldId9, fieldId10)
                        .ConfigureAwait(false);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldDto>> Advanced_TemplateFieldReadAll(int templateId)
        {
            string methodName = "Core.Advanced_TemplateFieldReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(templateId), templateId);

                    return await _sqlController.TemplateFieldReadAll(templateId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        #region sites
        public async Task<List<SiteDto>> Advanced_SiteReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = "Core.Advanced_SiteReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(workflowState), workflowState);
                    log.LogVariable(methodName, nameof(offSet), offSet.ToString());
                    log.LogVariable(methodName, nameof(limit), limit.ToString());

                    return await _sqlController.SimpleSiteGetAll(workflowState, offSet, limit).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<SiteNameDto> Advanced_SiteItemRead(int microting_uuid)
        {
            string methodName = "Core.Advanced_SiteItemRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microting_uuid), microting_uuid);

                    return await _sqlController.SiteRead(microting_uuid).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public Task<List<SiteNameDto>> Advanced_SiteItemReadAll()
        {
            return Advanced_SiteItemReadAll(true);
        }

        public async Task<List<SiteNameDto>> Advanced_SiteItemReadAll(bool includeRemoved)
        {
            string methodName = "Core.Advanced_SiteItemReadAll";
            try
            {
                if (Running())
                {
                    return await _sqlController.SiteGetAll(includeRemoved).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteItemUpdate(int siteId, string name)
        {
            string methodName = "Core.Advanced_SiteItemUpdate";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(siteId), siteId);
                    log.LogVariable(methodName, nameof(name), name);

                    if (await _sqlController.SiteRead(siteId).ConfigureAwait(false) == null)
                        return false;

                    bool success = await _communicator.SiteUpdate(siteId, name).ConfigureAwait(false);
                    if (!success)
                        return false;

                    return await _sqlController.SiteUpdate(siteId, name).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteItemDelete(int siteId)
        {
            string methodName = "Core.Advanced_SiteItemDelete";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(siteId), siteId);

                    bool success = await _communicator.SiteDelete(siteId).ConfigureAwait(false);
                    if (!success)
                        return false;

                    return await _sqlController.SiteDelete(siteId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        #endregion

        #region workers
        public async Task<WorkerDto> Advanced_WorkerCreate(string firstName, string lastName, string email)
        {
            string methodName = "Core.Advanced_WorkerCreate";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(firstName), firstName);
                    log.LogVariable(methodName, nameof(lastName), lastName);
                    log.LogVariable(methodName, nameof(email), email);

                    WorkerDto workerDto = await _communicator.WorkerCreate(firstName, lastName, email).ConfigureAwait(false);
                    int workerUId = workerDto.WorkerUId;

                    workerDto = await _sqlController.WorkerRead(workerDto.WorkerUId).ConfigureAwait(false);
                    if (workerDto == null)
                    {
                        await _sqlController.WorkerCreate(workerUId, firstName, lastName, email).ConfigureAwait(false);
                    }

                    return await Advanced_WorkerRead(workerUId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<string> Advanced_WorkerNameRead(int workerId)
        {
            string methodName = "Core.Advanced_WorkerNameRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(workerId), workerId);

                    return await _sqlController.WorkerNameRead(workerId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<WorkerDto> Advanced_WorkerRead(int workerId)
        {
            string methodName = "Core.Advanced_WorkerRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(workerId), workerId);

                    return await _sqlController.WorkerRead(workerId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<WorkerDto>> Advanced_WorkerReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = "Core.Advanced_WorkerReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(workflowState), workflowState);
                    log.LogVariable(methodName, nameof(offSet), offSet.ToString());
                    log.LogVariable(methodName, nameof(limit), limit.ToString());

                    return await _sqlController.WorkerGetAll(workflowState, offSet, limit).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_WorkerUpdate(int workerId, string firstName, string lastName, string email)
        {
            string methodName = "Core.Advanced_WorkerUpdate";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(workerId), workerId);
                    log.LogVariable(methodName, nameof(firstName), firstName);
                    log.LogVariable(methodName, nameof(lastName), lastName);
                    log.LogVariable(methodName, nameof(email), email);

                    if (await _sqlController.WorkerRead(workerId).ConfigureAwait(false) == null)
                        return false;

                    bool success = await _communicator.WorkerUpdate(workerId, firstName, lastName, email).ConfigureAwait(false);
                    if (!success)
                        return false;

                    return await _sqlController.WorkerUpdate(workerId, firstName, lastName, email).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_WorkerDelete(int microtingUid)
        {
            string methodName = "Core.Advanced_WorkerDelete";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                    bool success = await _communicator.WorkerDelete(microtingUid).ConfigureAwait(false);
                    if (!success)
                        return false;

                    return await _sqlController.WorkerDelete(microtingUid).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        #endregion
        #endregion

        #region site_workers
        public async Task<SiteWorkerDto> Advanced_SiteWorkerCreate(SiteNameDto siteDto, WorkerDto workerDto)
        {   
            string methodName = "Core.Advanced_SiteWorkerCreate";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, "siteId", siteDto.SiteUId);
                    log.LogVariable(methodName, "workerId", workerDto.WorkerUId);

                    SiteWorkerDto result = await _communicator.SiteWorkerCreate(siteDto.SiteUId, workerDto.WorkerUId).ConfigureAwait(false);

                    SiteWorkerDto siteWorkerDto = await _sqlController.SiteWorkerRead(result.MicrotingUId, null, null).ConfigureAwait(false);

                    if (siteWorkerDto == null)
                    {
                        await _sqlController.SiteWorkerCreate(result.MicrotingUId, siteDto.SiteUId, workerDto.WorkerUId).ConfigureAwait(false);
                    }

                    return await Advanced_SiteWorkerRead(result.MicrotingUId, null, null).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<SiteWorkerDto> Advanced_SiteWorkerRead(int? siteWorkerMicrotingUid, int? siteId, int? workerId)
        {
            string methodName = "Core.Advanced_SiteWorkerRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(siteWorkerMicrotingUid), siteWorkerMicrotingUid.ToString());
                    log.LogVariable(methodName, nameof(siteId), siteId.ToString());
                    log.LogVariable(methodName, nameof(workerId), workerId.ToString());

                    return await _sqlController.SiteWorkerRead(siteWorkerMicrotingUid, siteId, workerId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteWorkerDelete(int workerId)
        {
            string methodName = "Core.Advanced_SiteWorkerDelete";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(workerId), workerId);

                    bool success = await _communicator.SiteWorkerDelete(workerId).ConfigureAwait(false);
                    if (!success)
                        return false;

                    return await _sqlController.SiteWorkerDelete(workerId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region units
        public async Task<UnitDto> Advanced_UnitRead(int microtingUid)
        {
            string methodName = "Core.Advanced_UnitRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                    return await _sqlController.UnitRead(microtingUid).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<List<UnitDto>> Advanced_UnitReadAll()
        {
            string methodName = "Core.Advanced_UnitReadAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");

                    return await _sqlController.UnitGetAll().ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<UnitDto> Advanced_UnitRequestOtp(int microtingUid)
        {
            string methodName = "Core.Advanced_UnitRequestOtp";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                    int otp_code = await _communicator.UnitRequestOtp(microtingUid).ConfigureAwait(false);

                    UnitDto my_dto = await Advanced_UnitRead(microtingUid).ConfigureAwait(false);

                    await _sqlController.UnitUpdate(microtingUid, my_dto.CustomerNo, otp_code, my_dto.SiteUId).ConfigureAwait(false);

                    return await Advanced_UnitRead(microtingUid).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw ex;
            }
        }
        
        public async Task<bool> Advanced_UnitDelete(int unitId)
        {
            string methodName = "Core.Advanced_UnitDelete";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(unitId), unitId);

                    bool success = await _communicator.UnitDelete(unitId).ConfigureAwait(false);
                    if (!success)
                        return false;

                    return await _sqlController.UnitDelete(unitId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_UnitCreate(int siteMicrotingUid)
        {
            string methodName = "Core.Advanced_UnitCreate";
            try
            {
                if (Running())
                {
                    using (var dbContext = dbContextHelper.GetDbContext())
                    {
                        log.LogStandard(methodName, "called");
                        log.LogVariable(methodName, nameof(siteMicrotingUid), siteMicrotingUid);

                        sites site = await dbContext.sites.SingleOrDefaultAsync(x => x.MicrotingUid == siteMicrotingUid);

                        string result = await _communicator.UnitCreate((int)site.MicrotingUid).ConfigureAwait(false);
                        if (result != null)
                        {
                            units  unit = JsonConvert.DeserializeObject<units>(result);
                            unit.SiteId = dbContext.sites.Single(x => x.MicrotingUid == unit.SiteId).Id;
                            await unit.Create(dbContext).ConfigureAwait(false);
                            return true;
                        }

                        return false;
                    }
                }
                
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_UnitMove(int unitId, int siteId)
        {
            string methodName = "Core.Advanced_UnitMove";
            try
            {
                if (Running())
                {
                    using (var dbContext = dbContextHelper.GetDbContext())
                    {
                        log.LogStandard(methodName, "called");
                        log.LogVariable(methodName, nameof(unitId), unitId);
                        log.LogVariable(methodName, nameof(siteId), siteId);

                        units unit = await dbContext.units.SingleOrDefaultAsync(x => x.Id == unitId);
                        sites site = await dbContext.sites.SingleOrDefaultAsync(x => x.MicrotingUid == siteId);

                        string result = await _communicator.UnitMove((int)unit.MicrotingUid, (int)site.MicrotingUid).ConfigureAwait(false);
                        if (result != null)
                        {
                            unit.SiteId = site.Id;
                            await unit.Update(dbContext).ConfigureAwait(false);
                            return true;
                        }

                        return false;
                    }
                }
                
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        #endregion

        #region fields
        public async Task<Field> Advanced_FieldRead(int id)
        {
            string methodName = "Core.Advanced_FieldRead";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(id), id);

                    return await _sqlController.FieldRead(id).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<UploadedData> Advanced_UploadedDataRead(int id)
        {
            string methodName = "Core.Advanced_UploadedDataRead";
            try
            {
                log.LogStandard(methodName, "called");
                log.LogVariable(methodName, nameof(id), id);

                var ud = await _sqlController.GetUploadedData(id).ConfigureAwait(false);
                UploadedData uD = new UploadedData
                {
                    Checksum = ud.Checksum,
                    CurrentFile = ud.CurrentFile,
                    Extension = ud.Extension,
                    FileLocation = ud.FileLocation,
                    FileName = ud.FileName,
                    Id = ud.Id,
                    UploaderId = ud.UploaderId,
                    UploaderType = ud.UploaderType
                };
                return uD;
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldValue>> Advanced_FieldValueReadList(int id, int instances)
        {
            string methodName = "Core.Advanced_FieldValueReadList";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(id), id);
                    log.LogVariable(methodName, nameof(instances), instances);

                    return await _sqlController.FieldValueReadList(id, instances).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        
        public async Task<List<FieldValue>> Advanced_FieldValueReadList(int fieldId, List<int> caseIds)
        {
            string methodName = "Core.Advanced_FieldValueReadList";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(fieldId), fieldId);

                    return await _sqlController.FieldValueReadList(fieldId, caseIds).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldValue>> Advanced_FieldValueReadList(List<int> caseIds)
        {
            string methodName = "Core.Advanced_FieldValueReadList";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");

                    return await _sqlController.FieldValueReadList(caseIds).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<CheckListValue>> Advanced_CheckListValueReadList(List<int> caseIds)
        {
            string methodName = "Core.Advanced_CheckListValueReadList";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");

                    return await _sqlController.CheckListValueReadList(caseIds).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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

            string methodName = "Core.Advanced_EntityGroupAll";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(sort), sort);
                    log.LogVariable(methodName, nameof(nameFilter), nameFilter);
                    log.LogVariable(methodName, nameof(pageIndex), pageIndex);
                    log.LogVariable(methodName, nameof(pageSize), pageSize);
                    log.LogVariable(methodName, nameof(entityType), entityType);
                    log.LogVariable(methodName, nameof(desc), desc);
                    log.LogVariable(methodName, nameof(workflowState), workflowState);

                    return await _sqlController.EntityGroupAll(sort, nameFilter, pageIndex, pageSize, entityType, desc, workflowState).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_DeleteUploadedData(int fieldId, int uploadedDataId)
        {
            string methodName = "Core.Advanced_DeleteUploadedData";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(fieldId), fieldId);
                    log.LogVariable(methodName, nameof(uploadedDataId), uploadedDataId);

                    uploaded_data uD = await _sqlController.GetUploadedData(uploadedDataId).ConfigureAwait(false);

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
                        log.LogException(methodName, "failed", exd);
                        // TODO write code to handel the restart needed scenario!!!
                        throw new Exception("failed", exd);
                    }

                    return await _sqlController.DeleteFile(uploadedDataId).ConfigureAwait(false);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_UpdateCaseFieldValue(int caseId)
        {
            string methodName = "Core.Advanced_UpdateCaseFieldValue";
            try
            {
                if (Running())
                {
                    log.LogStandard(methodName, "called");
                    log.LogVariable(methodName, nameof(caseId), caseId);
                    return await _sqlController.CaseUpdateFieldValues(caseId).ConfigureAwait(false);
                }

                return false;
            }
            catch (Exception ex)
            {
                log.LogException(methodName, "failed", ex);
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

                    await ReplaceDataElementsAndDataItems(caseId, groupE.ElementList, lstAnswers).ConfigureAwait(false);

                    elementListReplaced.Add(groupE);
                }
                #endregion
            }

            return elementListReplaced;
        }

        private async Task<int> SendXml(MainElement mainElement, int siteId)
        {
            string methodName = "Core.SendXml";
            log.LogEverything(methodName, "siteId:" + siteId + ", requested sent eForm");

            string xmlStrRequest = mainElement.ClassToXml();
            
            log.LogEverything(methodName, "siteId:" + siteId + ", ClassToXml done");
            string xmlStrResponse = await _communicator.PostXml(xmlStrRequest, siteId);
            log.LogEverything(methodName, "siteId:" + siteId + ", PostXml done");

            Response response = new Response();
            response = response.XmlToClass(xmlStrResponse);
            log.LogEverything(methodName, "siteId:" + siteId + ", XmlToClass done");

            //if reply is "success", it's created
            if (response.Type.ToString().ToLower() == "success")
            {
                return int.Parse(response.Value);
            }

            throw new Exception("siteId:'" + siteId + "' // failed to create eForm at Microting // Response :" + xmlStrResponse);
        }

        public async Task<List<List<string>>> GenerateDataSetFromCases(int? checkListId, DateTime? start, DateTime? end, string customPathForUploadedData, string decimalSeparator, string thousandSaperator, bool utcTime, CultureInfo cultureInfo, TimeZoneInfo timeZoneInfo)
        {
            using (MicrotingDbContext dbContext = dbContextHelper.GetDbContext())
            {
                List<List<string>> dataSet = new List<List<string>>();
                List<string> colume1CaseIds = new List<string> { "Id" };
                List<int> caseIds = new List<int>();

                if (start == null)
                    start = DateTime.MinValue;
                if (end == null)
                    end = DateTime.MaxValue;
                List<cases> cases = await dbContext.cases.Where(x =>
                    x.DoneAt > start && x.DoneAt < end
                                     && x.WorkflowState != Constants.WorkflowStates.Removed
                                     && x.CheckListId == checkListId).ToListAsync();

                check_lists checkList = await dbContext.check_lists.SingleAsync(x => x.Id == (int) checkListId);

                if (cases.Count == 0)
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
                    foreach (var aCase in cases)
                    {
                        DateTime time = (DateTime)aCase.DoneAt;
                        DateTime createdAt = (DateTime) aCase.CreatedAt;
                        if (!utcTime)
                        {
                            time = TimeZoneInfo.ConvertTimeFromUtc(time, timeZoneInfo);
                            createdAt = TimeZoneInfo.ConvertTimeFromUtc(createdAt, timeZoneInfo);
                        }
                        colume1CaseIds.Add(aCase.Id.ToString());
                        caseIds.Add(aCase.Id);

                        colume2.Add(time.ToString("yyyy.MM.dd"));
                        colume3.Add(time.ToString("HH:mm:ss"));
                        colume4.Add(time.DayOfWeek.ToString());
                        colume5.Add(time.Year + "." + cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
                        //colume6.Add(time.Year.ToString() + "." + time.ToString("MMMM").Substring(0, 3));
                        colume6.Add(time.Year + "." + time.ToString("MMMM").AsSpan().Slice(0,3).ToString());
                        colume7.Add(time.Year.ToString());
                        colume8.Add(createdAt.ToString("yyyy.MM.dd HH:mm:ss"));
                        colume9.Add(aCase.Site.Name);
                        colume10.Add(aCase.Worker.full_name());
                        colume11.Add(aCase.UnitId.ToString());
                        colume12.Add(checkList.Label);
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
                    if (checkListId != null)
                    {

                        List<string> lstReturn = new List<string>();
                        lstReturn = await GenerateDataSetFromCasesSubSet(lstReturn, checkListId, "");

                        List<string> newRow;
                        foreach (string set in lstReturn)
                        {
                            int fieldId = int.Parse(t.SplitToList(set, 0, false));
                            string label = t.SplitToList(set, 1, false);

                            List<List<KeyValuePair>> result = await _sqlController.FieldValueReadAllValues(fieldId, caseIds, customPathForUploadedData, decimalSeparator, thousandSaperator).ConfigureAwait(false);

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
                                Field field = await _sqlController.FieldRead(fieldId).ConfigureAwait(false);
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
        }

        private async Task<List<string>> GenerateDataSetFromCasesSubSet(List<string> lstReturn, int? checkListId, string preLabel)
        {
            string sep = " / ";
            using (MicrotingDbContext dbContext = dbContextHelper.GetDbContext())
            {
                if (checkListId != null)
                {
                    if (dbContext.check_lists.Any(x => x.ParentId == checkListId))
                    {
                        foreach (check_lists checkList in dbContext.check_lists.Where(x => x.ParentId == checkListId).OrderBy(x => x.DisplayIndex))
                        {
                            check_lists parentCheckList = dbContext.check_lists.Single(x => x.Id == checkListId);
                            if (parentCheckList.ParentId != null)
                            {
                                if (preLabel != "")
                                    preLabel = preLabel + sep + parentCheckList.Label;
                            }
                            await GenerateDataSetFromCasesSubSet(lstReturn, checkList.Id, preLabel);
                        }
                    }
                    else
                    {
                        foreach (fields field in dbContext.fields.Where(x => x.CheckListId == checkListId && x.ParentFieldId == null).OrderBy(x => x.DisplayIndex))
                        {
                            if (dbContext.fields.Any(x => x.ParentFieldId == field.Id))
                            {
                                foreach (var subField in dbContext.fields.Where(x => x.ParentFieldId == field.Id).OrderBy(x => x.DisplayIndex))
                                {
                                    if (field.FieldTypeId != 3 && field.FieldTypeId != 18)
                                    {
                                        if (preLabel != "")
                                            lstReturn.Add(subField.Id + "|" + preLabel + sep + field.Label + sep +
                                                          subField.Label);
                                        else
                                            lstReturn.Add(subField.Id + "|" + field.Label + sep + subField.Label);
                                    }
                                }
                            }
                            else
                            {
                                if (field.FieldTypeId != 3 && field.FieldTypeId != 18)
                                {
                                    if (preLabel != "")
                                        lstReturn.Add(field.Id + "|" + preLabel + sep + field.Label);
                                    else
                                        lstReturn.Add(field.Id + "|" + field.Label);
                                }
                            }
                        }
                    }
                }
                return lstReturn;
            }
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
                                jasperFieldXml += GetJasperFieldValue(field, answer, customPathForUploadedData).GetAwaiter().GetResult();
                            }
                        } else if (item is FieldContainer)
                        {
                            FieldContainer fieldC = (FieldContainer)item;

                            foreach (Field field in fieldC.DataItemList)
                            {
                                jasperFieldXml += Environment.NewLine + "<F" + field.Id + " name=\"" + field.Label + "\" parent=\"" + dataE.Label + "\">";
                                foreach (FieldValue answer in field.FieldValues)
                                {                                    
                                    jasperFieldXml += GetJasperFieldValue(field, answer, customPathForUploadedData).GetAwaiter().GetResult();
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

            string methodName = "Core.CoreThread";
            log.LogEverything(methodName, "initiated");
            while (_coreAvailable)
            {
                try
                {
                    if (_coreThreadRunning)
                    {

                        Thread.Sleep(2000);
                    }

                    Thread.Sleep(500);
                }
                catch (ThreadAbortException)
                {
                    log.LogWarning(methodName, "catch of ThreadAbortException");
                }
                catch (Exception ex)
                {
                    await FatalExpection(methodName + "failed", ex).ConfigureAwait(false);
                }
            }
            log.LogEverything(methodName, "completed");

            _coreThreadRunning = false;
        }

        public async Task<bool> DownloadUploadedData(int uploadedDataId)
        {
            string methodName = "Core.DownloadUploadedData";
            uploaded_data uploadedData = await _sqlController.GetUploadedData(uploadedDataId).ConfigureAwait(false);

            if (uploadedData != null)
            {
                string urlStr = uploadedData.FileLocation;
                log.LogEverything(methodName, "Received file:" + uploadedData.ToString());

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
                        log.LogStandard(methodName, $"Downloading file to #{_fileLocationPicture}/#{fileName}");
                        client.DownloadFile(urlStr, _fileLocationPicture + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.LogWarning(methodName, "We got an error " + ex.Message);
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
                if (chechSum != fileName.AsSpan().Slice(fileName.LastIndexOf(".") - 32, 32).ToString()) 
                //.Substring(fileName.LastIndexOf(".") - 32, 32))
                    log.LogWarning(methodName, $"Download of '{urlStr}' failed. Check sum did not match");
                #endregion

                CaseDto dto = await _sqlController.FileCaseFindMUId(urlStr).ConfigureAwait(false);
                FileDto fDto = new FileDto(dto.SiteUId, dto.CaseType, dto.CaseUId, dto.MicrotingUId.ToString(), dto.CheckUId.ToString(), _fileLocationPicture + fileName);
                try { HandleFileDownloaded?.Invoke(fDto, EventArgs.Empty); }
                catch { log.LogWarning(methodName, "HandleFileDownloaded event's external logic suffered an Expection"); }
                log.LogStandard(methodName, "Downloaded file '" + urlStr + "'.");

                await _sqlController.FileProcessed(urlStr, chechSum, _fileLocationPicture, fileName, uploadedData.Id).ConfigureAwait(false);

                if (_swiftEnabled || _s3Enabled)
                {
                    log.LogStandard(methodName, $"Trying to upload file {fileName}");
                    string filePath = Path.Combine(_fileLocationPicture, fileName);
                    if (File.Exists(filePath))
                    {
                        log.LogStandard(methodName, $"File exists at path {filePath}");
                        await PutFileToStorageSystem(filePath, fileName).ConfigureAwait(false);
                    }
                    else
                    {
                        log.LogWarning(methodName, $"File could not be found at filepath {filePath}");
                    }
                }
                
                return true;
            }

            return false;
        }

        public async Task<GetObjectResponse> GetFileFromS3Storage(string fileName)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = $"{await _sqlController.SettingRead(Settings.s3BucketName).ConfigureAwait(false)}/{_customerNo}",
                Key = fileName
            };

            return await _s3Client.GetObjectAsync(request);
        }

        public async Task<SwiftObjectGetResponse> GetFileFromSwiftStorage(string fileName)
        {
            try
            {
                return await GetFileFromSwiftStorage(fileName, 0).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
                _swiftClient.AuthenticateAsyncV2(_keystoneEndpoint, _swiftUserName, _swiftPassword);
                
                return await GetFileFromSwiftStorage(fileName, 0).ConfigureAwait(false);
            }
        }

        private async Task<SwiftObjectGetResponse> GetFileFromSwiftStorage(string fileName, int retries)
        {
            string methodName = "Core.GetFileFromSwiftStorage";
            if (_swiftEnabled)
            {                
                log.LogStandard(methodName, $"Trying to get file {fileName} from {_customerNo}_uploaded_data");
                SwiftObjectGetResponse response = await _swiftClient.ObjectGetAsync(_customerNo + "_uploaded_data", fileName).ConfigureAwait(false);
                if (response.IsSuccess)
                {
                    return response;
                }
                else
                {
                    if (response.Reason == "Unauthorized")
                    {
                        log.LogWarning(methodName, "Check swift credentials : Unauthorized");
                        throw new UnauthorizedAccessException();
                    }
                    
                    log.LogCritical(methodName, $"Could not get file {fileName}, reason is {response.Reason}");
                    throw new Exception($"Could not get file {fileName}");
                }
            }

            throw new FileNotFoundException();
        }

        public async Task PutFileToStorageSystem(string filePath, string fileName)
        {
            try
            {
                await PutFileToStorageSystem(filePath, fileName, 0).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
                _swiftClient.AuthenticateAsyncV2(_keystoneEndpoint, _swiftUserName, _swiftPassword);
                await PutFileToStorageSystem(filePath, fileName, 0).ConfigureAwait(false);
            }
            
        }

        private async Task PutFileToStorageSystem(String filePath, string fileName, int tryCount)
        {
            if (_swiftEnabled)
            {
                await PutFileToSwiftStorage(filePath, fileName, tryCount).ConfigureAwait(false);
            }

            if (_s3Enabled)
            {
                await PutFileToS3Storage(filePath, fileName, tryCount).ConfigureAwait(false);
            }
        }

#pragma warning disable 1998
        private async Task PutFileToSwiftStorage(string filePath, string fileName, int tryCount)
#pragma warning restore 1998
        {
            string methodName = "Core.PutFileToSwiftStorage";
            log.LogStandard(methodName, $"Trying to upload file {fileName} to {_customerNo}_uploaded_data");
            try
            {
                var fileStream = new FileStream(filePath, FileMode.Open);

                SwiftBaseResponse response = _swiftClient
                    .ObjectPutAsync(_customerNo + "_uploaded_data", fileName, fileStream).GetAwaiter().GetResult();

                if (!response.IsSuccess)
                {
                    if (response.Reason == "Unauthorized")
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                        log.LogWarning(methodName, "Check swift credentials : Unauthorized");
                        throw new UnauthorizedAccessException();
                    }

                    log.LogWarning(methodName, $"Something went wrong, message was {response.Reason}");

                    response = _swiftClient.ContainerPutAsync(_customerNo + "_uploaded_data").GetAwaiter().GetResult();
                    if (response.IsSuccess)
                    {
                        response = _swiftClient
                            .ObjectPutAsync(_customerNo + "_uploaded_data", fileName, fileStream).GetAwaiter().GetResult();
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
                log.LogCritical(methodName, $"File not found at {filePath}");
                log.LogCritical(methodName, ex.Message);
            }   
        }

        private async Task PutFileToS3Storage(string filePath, string fileName, int tryCount)
        {
            string methodName = "Core.PutFileToS3Storage";
            string bucketName = await _sqlController.SettingRead(Settings.s3BucketName);
            log.LogStandard(methodName, $"Trying to upload file {fileName} to {bucketName}");

            var fileStream = new FileStream(filePath, FileMode.Open);
            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = $"{await _sqlController.SettingRead(Settings.s3BucketName).ConfigureAwait(false)}/{_customerNo}",
                Key = fileName,
                FilePath = filePath
            };
            try
            {
                var response = await _s3Client.PutObjectAsync(putObjectRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogWarning(methodName, $"Something went wrong, message was {ex.Message}");
            }
            
        }
        
        public async Task<bool> CheckStatusByMicrotingUid(int microtingUid)
        {
            string methodName = "Core.CheckStatusByMicrotingUid";
            List<CaseDto> lstCase = new List<CaseDto>();
            MainElement mainElement = new MainElement();

            CaseDto concreteCase = await _sqlController.CaseReadByMUId(microtingUid).ConfigureAwait(false);
            log.LogEverything(methodName, concreteCase.ToString() + " has been matched");

            if (concreteCase.CaseUId == "" || concreteCase.CaseUId == "ReversedCase")
                lstCase.Add(concreteCase);
            else
                lstCase = await _sqlController.CaseReadByCaseUId(concreteCase.CaseUId).ConfigureAwait(false);

            foreach (CaseDto aCase in lstCase)
            {
                if (aCase.SiteUId == concreteCase.SiteUId)
                {
                    #region get response's data and update DB with data
                    int? checkIdLastKnown = await _sqlController.CaseReadLastCheckIdByMicrotingUId(microtingUid).ConfigureAwait(false); //null if NOT a checkListSite
                    log.LogVariable(methodName, nameof(checkIdLastKnown), checkIdLastKnown);

                    string respXml;
                    if (checkIdLastKnown == null)
                        respXml = await _communicator.Retrieve(microtingUid.ToString(), concreteCase.SiteUId).ConfigureAwait(false);
                    else
                        respXml = await _communicator.RetrieveFromId(microtingUid.ToString(), concreteCase.SiteUId, checkIdLastKnown.ToString()).ConfigureAwait(false);
                    log.LogVariable(methodName, nameof(respXml), respXml);

                    Response resp = new Response();
                    resp = resp.XmlToClassUsingXmlDocument(respXml);
                    //resp = resp.XmlToClass(respXml);

                    if (resp.Type == Response.ResponseTypes.Success)
                    {
                        log.LogEverything(methodName, "resp.Type == Response.ResponseTypes.Success (true)");
                        if (resp.Checks.Count > 0)
                        {
                            XmlDocument xDoc = new XmlDocument();

                            xDoc.LoadXml(respXml);
                            XmlNode checks = xDoc.DocumentElement.LastChild;
                            int i = 0;
                            foreach (Check check in resp.Checks)
                            {

                                int unitUId = _sqlController.UnitRead(int.Parse(check.UnitId)).GetAwaiter().GetResult().UnitUId;
                                log.LogVariable(methodName, nameof(unitUId), unitUId);
                                int workerUId = _sqlController.WorkerRead(int.Parse(check.WorkerId)).GetAwaiter().GetResult().WorkerUId;
                                log.LogVariable(methodName, nameof(workerUId), workerUId);

                                await _sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml.ToString(), i).ConfigureAwait(false);

                                await _sqlController.CaseUpdateCompleted(microtingUid, (int)check.Id, DateTime.Parse(check.Date), workerUId, unitUId).ConfigureAwait(false);
                                log.LogEverything(methodName, "sqlController.CaseUpdateCompleted(...)");

                                #region IF needed retract case, thereby completing the process
                                if (checkIdLastKnown == null)
                                {
                                    string responseRetractionXml = await _communicator.Delete(aCase.MicrotingUId.ToString(), aCase.SiteUId).ConfigureAwait(false);
                                    Response respRet = new Response();
                                    respRet = respRet.XmlToClass(respXml);

                                    if (respRet.Type == Response.ResponseTypes.Success)
                                    {
                                        log.LogEverything(methodName, aCase + " has been retracted");
                                    }
                                    else
                                        log.LogWarning(methodName, "Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteUId + ". Not a critical issue, but needs to be fixed if repeated");
                                }
                                #endregion

                                await _sqlController.CaseRetract(microtingUid, (int)check.Id).ConfigureAwait(false);
                                log.LogEverything(methodName, "sqlController.CaseRetract(...)");
                                // TODO add case.Id
                                CaseDto cDto = await _sqlController.CaseReadByMUId(microtingUid);
								//InteractionCaseUpdate(cDto);
                                await FireHandleCaseCompleted(cDto).ConfigureAwait(false);
                                //try { HandleCaseCompleted?.Invoke(cDto, EventArgs.Empty); }
                                //catch { log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection"); }
                                log.LogStandard(methodName, cDto.ToString() + " has been completed");
                                i++;
                            }
                        }
                    }
                    else
                    {
                        log.LogEverything(methodName, "resp.Type == Response.ResponseTypes.Success (false)");
                        throw new Exception("Failed to retrive eForm " + microtingUid + " from site " + aCase.SiteUId);
                    }
                    #endregion
                }
                else
                {
                    await CaseDelete((int)aCase.MicrotingUId).ConfigureAwait(false);
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

#pragma warning disable 1998
        public async Task FireHandleCaseCompleted(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseCompleted";
		    log.LogStandard(methodName, $"FireHandleCaseCompleted for MicrotingUId {caseDto.MicrotingUId}");
			try { HandleCaseCompleted.Invoke(caseDto, EventArgs.Empty); }
			catch (Exception ex)
			{
				log.LogWarning(methodName, "HandleCaseCompleted event's external logic suffered an Expection");
				throw ex;
			}
		}

#pragma warning disable 1998
        public async Task FireHandleCaseDeleted(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseDeleted";
            try { HandleCaseDeleted?.Invoke(caseDto, EventArgs.Empty); }
            catch { log.LogWarning(methodName, "HandleCaseCompleted event's external logic suffered an Expection"); }
        }

#pragma warning disable 1998
        public async Task FireHandleNotificationNotFound(NoteDto notification)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleNotificationNotFound";
            try { HandleNotificationNotFound?.Invoke(notification, EventArgs.Empty); }
            catch { log.LogWarning(methodName, "HandleNotificationNotFound event's external logic suffered an Expection"); }
        }

#pragma warning disable 1998
        public async Task FireHandleSiteActivated(NoteDto notification)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleSiteActivated";
            try { HandleSiteActivated?.Invoke(notification, EventArgs.Empty); }
            catch { log.LogWarning(methodName, "HandleSiteActivated event's external logic suffered an Expection"); }
        }

#pragma warning disable 1998
        public async Task FireHandleCaseProcessedByServer(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseProcessedByServer";
            log.LogStandard(methodName, $"HandleCaseProcessedByServer for MicrotingUId {caseDto.MicrotingUId}");

            try { HandleeFormProcessedByServer.Invoke(caseDto, EventArgs.Empty); }
            catch (Exception ex)
            {
                log.LogWarning(methodName, "HandleCaseProcessedByServer event's external logic suffered an Expection");
                throw ex;
            }
        }

#pragma warning disable 1998
        public async Task FireHandleCaseProcessingError(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseProcessingError";
            log.LogStandard(methodName, $"HandleCaseProcessingError for MicrotingUId {caseDto.MicrotingUId}");

            try { HandleeFormProsessingError.Invoke(caseDto, EventArgs.Empty); }
            catch (Exception ex)
            {
                log.LogWarning(methodName, "HandleCaseProcessingError event's external logic suffered an Expection");
                throw ex;
            }
        }

#pragma warning disable 1998
        public async Task FireHandleCaseRetrived(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseRetrived";
		    log.LogStandard(methodName, $"FireHandleCaseRetrived for MicrotingUId {caseDto.MicrotingUId}");

			try { HandleCaseRetrived.Invoke(caseDto, EventArgs.Empty); }
			catch (Exception ex)
			{
				log.LogWarning(methodName, "HandleCaseRetrived event's external logic suffered an Expection");
				throw ex;
			}
		}
        #endregion
    }

}