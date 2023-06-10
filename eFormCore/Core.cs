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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ICSharpCode.SharpZipLib.Zip;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
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
using Rebus.Bus;
using Case = Microting.eForm.Dto.Case;
using CheckListValue = Microting.eForm.Infrastructure.Models.CheckListValue;
using DataItem = Microting.eForm.Infrastructure.Models.DataItem;
using EntityGroup = Microting.eForm.Infrastructure.Models.EntityGroup;
using EntityItem = Microting.eForm.Infrastructure.Models.EntityItem;
using Field = Microting.eForm.Infrastructure.Models.Field;
using FieldValue = Microting.eForm.Infrastructure.Models.FieldValue;
using KeyValuePair = Microting.eForm.Dto.KeyValuePair;
using Settings = Microting.eForm.Dto.Settings;
using Tag = Microting.eForm.Dto.Tag;
using UploadedData = Microting.eForm.Infrastructure.Models.UploadedData;

namespace eFormCore
{
    using System.Net.Http;

    public class Core : CoreBase
    {
        // events
        public event EventHandler HandleCaseCreated;
        public event EventHandler HandleCaseRetrived;
        public event EventHandler HandleCaseCompleted;
#pragma warning disable CS0067
        public event EventHandler HandleeFormProcessedByServer;
        public event EventHandler HandleeFormProsessingError;
#pragma warning restore CS0067
        public event EventHandler HandleCaseDeleted;
#pragma warning disable CS0067
        public event EventHandler HandleFileDownloaded;
#pragma warning restore CS0067
        public event EventHandler HandleSiteActivated;
        public event EventHandler HandleNotificationNotFound;

        public event EventHandler HandleEventException;
        //

        // var
        private Subscriber _subscriber;
        private Communicator _communicator;
        private SqlController _sqlController;
        public DbContextHelper DbContextHelper;
        private readonly Tools _t = new Tools();

        private IWindsorContainer _container;

        public Log Log;

        private readonly object _lockMain = new object();
        object _lockEventMessage = new object();

//        private bool _updateIsRunningFiles = false;
//        private bool _updateIsRunningNotifications = false;
        private bool _updateIsRunningEntities;

        private bool _coreThreadRunning;
        private bool _coreRestarting;
        private bool _coreStatChanging;
        private bool _coreAvailable;

        private bool skipRestartDelay = false;

        private string _connectionString;
        private string _fileLocationPicture;
        private string _fileLocationPdf;

        private int _sameExceptionCountTried;
        private int _maxParallelism = 1;
        private int _numberOfWorkers = 1;
        private string _rabbitMqUser = "admin";
        private string _rabbitMqPassword = "password";
        private string _rabbitMqHost = "localhost";
        private string _customerNo;
        private IBus _bus;

        // swift
        private bool _swiftEnabled;
        private string _swiftUserName = "";
        private string _swiftPassword = "";
        private string _swiftEndpoint = "";

        private string _keystoneEndpoint = "";
        // private SwiftClientService _swiftClient;
        //

        // s3
        private bool _s3Enabled;
        private string _s3AccessKeyId = "";
        private string _s3SecretAccessKey = "";
        private string _s3Endpoint = "";

        private static AmazonS3Client _s3Client;
        //
        //

        //con

        // public state

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
                        _maxParallelism = int.Parse(await _sqlController.SettingRead(Settings.maxParallelism)
                            .ConfigureAwait(false));
                        _numberOfWorkers = int.Parse(await _sqlController.SettingRead(Settings.numberOfWorkers)
                            .ConfigureAwait(false));
                        _rabbitMqUser = await _sqlController.SettingRead(Settings.rabbitMqUser).ConfigureAwait(false);
                        _rabbitMqPassword = await _sqlController.SettingRead(Settings.rabbitMqPassword)
                            .ConfigureAwait(false);
                        _rabbitMqHost = await _sqlController.SettingRead(Settings.rabbitMqHost).ConfigureAwait(false);
                        _customerNo = await _sqlController.SettingRead(Settings.customerNo).ConfigureAwait(false);
                    }
                    catch
                    {
                    }

                    if (connectionString.Contains("frontend"))
                    {
                        if (_rabbitMqHost == "localhost")
                        {
                            _rabbitMqHost = $"frontend-{_customerNo}-rabbitmq";
                        }
                    }

                    _container.Install(
                        new RebusHandlerInstaller()
                        , new RebusInstaller(_customerNo, connectionString, _maxParallelism, _numberOfWorkers, _rabbitMqUser, _rabbitMqPassword, _rabbitMqHost)
                    );
                    _bus = _container.Resolve<IBus>();
                    Log.LogCritical(methodName, "called");

                    //---

                    _coreStatChanging = true;

                    //subscriber
                    _subscriber = new Subscriber(_sqlController, Log, _bus);
                    _subscriber.Start();
                    Log.LogStandard(methodName, "Subscriber started");

                    Log.LogCritical(methodName, "started");
                    _coreAvailable = true;
                    _coreStatChanging = false;

                    //coreThread
                    //Thread coreThread = new Thread(() => CoreThread());
                    //coreThread.Start();
                    _coreThreadRunning = true;

                    Log.LogStandard(methodName, "CoreThread started");
                }
            }
            // catch
            catch (Exception ex)
            {
                await FatalExpection(methodName + " failed", ex).ConfigureAwait(false);
                throw;
                //return false;
            }
            //

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
                    DbContextHelper = new DbContextHelper(connectionString);
                    _sqlController = new SqlController(DbContextHelper);

                    //check settings
                    var errors = _sqlController.SettingCheckAll().GetAwaiter().GetResult();
                    if (errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            Log.LogCritical(methodName, $"_sqlController.SettingCheckAll() returned error : {error}");
                        }

                        throw new ArgumentException(
                            "Use AdminTool to setup database correctly. 'SettingCheckAll()' returned with errors");
                    }

                    if (await _sqlController.SettingRead(Settings.token).ConfigureAwait(false) ==
                        "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
                        throw new ArgumentException(
                            "Use AdminTool to setup database correctly. Token not set, only default value found");

                    if (await _sqlController.SettingRead(Settings.firstRunDone).ConfigureAwait(false) != "true")
                        throw new ArgumentException(
                            "Use AdminTool to setup database correctly. FirstRunDone has not completed");

                    if (await _sqlController.SettingRead(Settings.knownSitesDone).ConfigureAwait(false) != "true")
                        throw new ArgumentException(
                            "Use AdminTool to setup database correctly. KnownSitesDone has not completed");

                    //log
                    Log ??= _sqlController.StartLog(this);

                    Log.LogCritical(methodName,
                        "###########################################################################");
                    Log.LogCritical(methodName, "called");
                    Log.LogStandard(methodName, "SqlController and Logger started");

                    //settings read
                    _connectionString = connectionString;
                    _fileLocationPicture =
                        Path.Combine(Path.GetTempPath(),
                            "pictures"); //_sqlController.SettingRead(Settings.fileLocationPicture);
                    _fileLocationPdf =
                        Path.Combine(Path.GetTempPath(),
                            "pdf"); // await _sqlController.SettingRead(Settings.fileLocationPdf);
                    Log.LogStandard(methodName, "Settings read");

                    //communicators
                    string token = await _sqlController.SettingRead(Settings.token).ConfigureAwait(false);
                    string comAddressApi =
                        await _sqlController.SettingRead(Settings.comAddressApi).ConfigureAwait(false);
                    string comAddressBasic =
                        await _sqlController.SettingRead(Settings.comAddressBasic).ConfigureAwait(false);
                    string comOrganizationId =
                        await _sqlController.SettingRead(Settings.comOrganizationId).ConfigureAwait(false);
                    string comAddressPdfUpload =
                        await _sqlController.SettingRead(Settings.comAddressPdfUpload).ConfigureAwait(false);
                    string comSpeechToText =
                        await _sqlController.SettingRead(Settings.comSpeechToText).ConfigureAwait(false);
                    _communicator = new Communicator(token, comAddressApi, comAddressBasic, comOrganizationId,
                        comAddressPdfUpload, Log, comSpeechToText, connectionString);

                    _container = new WindsorContainer();
                    _container.Register(
                        Component.For<SqlController>().Instance(_sqlController),
                        Component.For<Communicator>().Instance(_communicator),
                        Component.For<Log>().Instance(Log),
                        Component.For<Core>().Instance(this));

                    try
                    {
                        _customerNo = await _sqlController.SettingRead(Settings.customerNo).ConfigureAwait(false);
                    }
                    catch
                    {
                    }

                    try
                    {
                        _swiftEnabled = (_sqlController.SettingRead(Settings.swiftEnabled).GetAwaiter().GetResult()
                            .ToLower() == "true");
                    }
                    catch
                    {
                    }

                    try
                    {
                        _s3Enabled =
                            (_sqlController.SettingRead(Settings.s3Enabled).GetAwaiter().GetResult().ToLower() ==
                             "true");
                    }
                    catch
                    {
                    }

                    if (_swiftEnabled)
                    {
                        _swiftUserName = await _sqlController.SettingRead(Settings.swiftUserName).ConfigureAwait(false);
                        _swiftPassword = await _sqlController.SettingRead(Settings.swiftPassword).ConfigureAwait(false);
                        _swiftEndpoint = await _sqlController.SettingRead(Settings.swiftEndPoint).ConfigureAwait(false);
                        _keystoneEndpoint = await _sqlController.SettingRead(Settings.keystoneEndPoint)
                            .ConfigureAwait(false);

                        // _swiftClient = new SwiftClientService(
                        //     new SwiftClientConfig
                        //     {
                        //         AuthUrl = _keystoneEndpoint,
                        //         Domain = "",
                        //         Name = _swiftUserName,
                        //         ObjectStoreUrl = _swiftEndpoint,
                        //         Password = _swiftPassword
                        //     });
                        // try
                        // {
                        //     _swiftClient.AuthenticateAsync();
                        // }
                        // catch (Exception ex)
                        // {
                        //     Log.LogWarning(methodName, ex.Message);
                        // }
                        //
                        //
                        // _container.Register(Component.For<SwiftClientService>().Instance(_swiftClient));
                    }

                    if (_s3Enabled)
                    {
                        try
                        {
                            _s3AccessKeyId = await _sqlController.SettingRead(Settings.s3AccessKeyId)
                                .ConfigureAwait(false);
                            _s3SecretAccessKey = await _sqlController.SettingRead(Settings.s3SecrectAccessKey)
                                .ConfigureAwait(false);
                            _s3Endpoint = await _sqlController.SettingRead(Settings.s3Endpoint).ConfigureAwait(false);

                            if (_s3Endpoint.Contains("https"))
                            {
                                _s3Client = new AmazonS3Client(_s3AccessKeyId, _s3SecretAccessKey, new AmazonS3Config
                                {
                                    ServiceURL = _s3Endpoint
                                });
                            }
                            else
                            {
                                _s3Client = new AmazonS3Client(_s3AccessKeyId, _s3SecretAccessKey,
                                    RegionEndpoint.EUCentral1);
                            }

                            _container.Register(Component.For<AmazonS3Client>().Instance(_s3Client));
                        }
                        catch (Exception ex)
                        {
                            Log.LogWarning(methodName, ex.Message);
                        }
                    }


                    Log.LogStandard(methodName, "Communicator started");

                    Log.LogCritical(methodName, "started");
                    _coreAvailable = true;
                    _coreStatChanging = false;
                }
            }
            // catch
            catch (Exception ex)
            {
                _coreThreadRunning = false;
                _coreStatChanging = false;

                await FatalExpection(methodName + " failed", ex);
                return false;
            }
            //

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
                    Log.LogCritical(methodName, "called");
                    Log.LogVariable(methodName, nameof(sameExceptionCount), sameExceptionCount);
                    Log.LogVariable(methodName, nameof(sameExceptionCountMax), sameExceptionCountMax);

                    _sameExceptionCountTried++;

                    if (_sameExceptionCountTried > sameExceptionCountMax)
                        _sameExceptionCountTried = sameExceptionCountMax;

                    if (_sameExceptionCountTried > 4)
                        throw new Exception("The same Exception repeated to many times (5+) within one hour");

                    int secondsDelay = 0;
                    switch (_sameExceptionCountTried)
                    {
                        case 1:
                            secondsDelay = 030;
                            break;
                        case 2:
                            secondsDelay = 060;
                            break;
                        case 3:
                            secondsDelay = 120;
                            break;
                        case 4:
                            secondsDelay = 512;
                            break;
                        default: throw new ArgumentOutOfRangeException("sameExceptionCount should be above 0");
                    }

                    Log.LogVariable(methodName, nameof(_sameExceptionCountTried), _sameExceptionCountTried);
                    Log.LogVariable(methodName, nameof(secondsDelay), secondsDelay);

                    await Close().ConfigureAwait(false);

                    Log.LogStandard(methodName, "Trying to restart the Core in " + secondsDelay + " seconds");

                    if (!skipRestartDelay)
                        Thread.Sleep(secondsDelay * 1000);
                    else
                        Log.LogStandard(methodName, "Delay skipped");

                    await Start(_connectionString).ConfigureAwait(false);
                    _coreRestarting = false;
                }
            }
            catch (Exception ex)
            {
                await FatalExpection(methodName + "failed. Core failed to restart", ex).ConfigureAwait(false);
                throw;
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
            Log.LogStandard(methodName, "Close called");
            try
            {
                if (_coreAvailable && !_coreStatChanging)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        _coreStatChanging = true;

                        _coreAvailable = false;
                        Log.LogCritical(methodName, "called");

                        try
                        {
                            if (_subscriber != null)
                            {
                                Log.LogEverything(methodName, "Subscriber requested to close connection");
                                _subscriber.Close().GetAwaiter().GetResult();
                                Log.LogEverything(methodName, "Subscriber closed");
                                _bus.Advanced.Workers.SetNumberOfWorkers(0);
                                _bus.Dispose();
                                _coreThreadRunning = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.LogException(methodName, "Subscriber failed to close", ex);
                        }

                        int tries = 0;
                        while (_coreThreadRunning)
                        {
                            Thread.Sleep(100);
                            tries++;

                            if (tries > 600)
                                FatalExpection("Failed to close Core correct after 60 secs", new Exception())
                                    .GetAwaiter().GetResult();
                        }

                        _updateIsRunningEntities = false;

                        Log.LogStandard(methodName, "Core closed");
                        _subscriber = null;
                        _communicator = null;
                        _sqlController = null;
                        _bus = null;

                        _coreStatChanging = false;
                    }
                }
            }
            // obsolete
            /*catch (ThreadAbortException)
            {
                //"Even if you handle it, it will be automatically re-thrown by the CLR at the end of the try/catch/finally."
                Thread.ResetAbort(); //This ends the re-throwning
            }*/
            catch (Exception ex)
            {
                FatalExpection(methodName + " failed. Core failed to close", ex).GetAwaiter().GetResult();
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
                Log?.LogFatalException(methodName + " called for reason:'" + reason + "'", exception);
            }
            catch
            {
            }

            try
            {
                HandleEventException?.Invoke(exception, EventArgs.Empty);
            }
            catch
            {
            }

            throw new Exception("FATAL exception, Core shutting down, due to:'" + reason + "'", exception);
        }
        //

        // public actions
        // template

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
                Log.LogStandard(methodName, "called");
                Log.LogEverything(methodName, "XML to transform:");
                Log.LogEverything(methodName, xmlString);

                //XML HACK TODO
                // xmlString = corrected xml if needed
                xmlString = xmlString.Trim();
                //xmlString = xmlString.Replace("=\"choose_entity\">", "=\"EntitySearch\">");
                xmlString = xmlString.Replace("=\"single_select\">", "=\"SingleSelect\">");
                xmlString = xmlString.Replace("=\"multi_select\">", "=\"MultiSelect\">");
                xmlString = xmlString.Replace("xsi:type", "type");


                // xmlString = _t.ReplaceInsensitive(xmlString, "<main", "<Main");
                xmlString = xmlString.Replace("<main", "<Main");
                // xmlString = _t.ReplaceInsensitive(xmlString, "</main", "</Main");
                xmlString = xmlString.Replace("</main", "</Main");

                // xmlString = _t.ReplaceInsensitive(xmlString, "<element", "<Element");
                xmlString = xmlString.Replace("<element", "<Element");
                // xmlString = _t.ReplaceInsensitive(xmlString, "</element", "</Element");
                xmlString = xmlString.Replace("</element", "</Element");

                // xmlString = _t.ReplaceInsensitive(xmlString, "<dataItem", "<DataItem");
                xmlString = xmlString.Replace("<dataItem", "<DataItem");
                // xmlString = _t.ReplaceInsensitive(xmlString, "</dataItem", "</DataItem");
                xmlString = xmlString.Replace("</dataItem", "</DataItem");

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
                {
                    xmlString = xmlString.Replace("=\"" + item.ToLower() + "\">", "=\"" + item + "\">");
                }
                // xmlString = _t.ReplaceInsensitive(xmlString, "=\"" + item + "\">", "=\"" + item + "\">");

                xmlString = xmlString.Replace("<Main>",
                    "<Main xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                xmlString = xmlString.Replace("<Element type=", "<Element xsi:type=");
                xmlString = xmlString.Replace("<DataItem type=", "<DataItem xsi:type=");
                //xmlString = xmlString.Replace("<DataItemGroup type=", "<DataItemGroup xsi:type=");
                xmlString = xmlString.Replace("FieldGroup", "FieldContainer");
                xmlString = xmlString.Replace("<DataItemGroup type=", "<DataItem xsi:type=");
                xmlString = xmlString.Replace("</DataItemGroup>", "</DataItem>");
                xmlString = xmlString.Replace("<DataItemGroupList>", "<DataItemList>");
                xmlString = xmlString.Replace("</DataItemGroupList>", "</DataItemList>");
                xmlString = xmlString.Replace("<Id>", "<OriginalId>");
                xmlString = xmlString.Replace("</Id>", "</OriginalId>");

                xmlString = xmlString.Replace("<FolderName>", "<CheckListFolderName>");
                xmlString = xmlString.Replace("</FolderName>", "</CheckListFolderName>");

                xmlString = xmlString.Replace("=\"ShowPDF\">", "=\"ShowPdf\">");
                xmlString = xmlString.Replace("='ShowPDF'>", "='ShowPdf'>");
                xmlString = xmlString.Replace("=\"choose_entity\">", "=\"EntitySearch\">");
                xmlString = xmlString.Replace("=\"Single_Select\">", "=\"SingleSelect\">");
                xmlString = xmlString.Replace("=\"Check_Box\">", "=\"CheckBox\">");
                xmlString = xmlString.Replace("=\"SingleSelectSearch\">", "=\"EntitySelect\">");

                string temp = _t.Locate(xmlString, "<DoneButtonDisabled>", "</DoneButtonDisabled>");
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
                xmlString = xmlString.Replace("<DisplayOrder></DisplayOrder>",
                    "<DisplayOrder>" + "0" + "</DisplayOrder>");
                var matches = Regex.Matches(xmlString, "<Description>(.*)</Description>");
                foreach (Match match in matches)
                {
                    if (!match.Value.Contains("CDATA"))
                    {
                        string oldValue = match.Value;
                        string newValue = Regex.Replace(oldValue, "<Description>(.*)</Description>",
                            "<Description><![CDATA[$1]]></Description>");
                        xmlString = xmlString.Replace(oldValue, newValue);
                    }
                }

                List<string> dILst = _t.LocateList(xmlString, "type=\"Date\">", "</DataItem>");
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
                // xmlString = _t.ReplaceInsensitive(xmlString, ">True<", ">true<");
                xmlString = xmlString.Replace(">True<", ">true<");
                xmlString = xmlString.Replace(">False<", ">false<");
                // xmlString = _t.ReplaceInsensitive(xmlString, ">False<", ">false<");
                //

                Log.LogEverything(methodName, "XML after possible corrections:");
                Log.LogEverything(methodName, xmlString);

                MainElement mainElement = new MainElement();
                mainElement = mainElement.XmlToClass(xmlString);

                //XML HACK
                mainElement.CaseType = "";
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                if (mainElement.Repeated < 1)
                {
                    Log.LogCritical(methodName, "mainElement.Repeated = 1 // enforced");
                    mainElement.Repeated = 1;
                }

                return mainElement;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            throw new Exception(
                                "Could not parse XML, got error: " +
                                ex.InnerException.InnerException.InnerException.Message, ex);
                        }

                        throw new Exception(
                            "Could not parse XML, got error: " + ex.InnerException.InnerException.Message, ex);
                    }

                    throw new Exception("Could not parse XML, got error: " + ex.InnerException.Message, ex);
                }

                throw new Exception("Could not parse XML, got error: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Converts XML from ex. eForm Builder or other sources, into a MainElement
        /// </summary>
        /// <param name="json">json string to be converted</param>
        public MainElement TemplateFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("json cannot be null or empty");
            string methodName = "Core.TemplateFromXml";
            try
            {
                Log.LogStandard(methodName, "called");
                Log.LogEverything(methodName, "json to transform:");
                Log.LogEverything(methodName, json);

                // xmlString = corrected xml if needed
                // check with json Payload
                //string temp = t.Locate(xmlString, "<DoneButtonDisabled>", "</DoneButtonDisabled>");
                //if (temp == "false")
                //{
                //    xmlString = xmlString.Replace("DoneButtonDisabled", "DoneButtonEnabled");
                //    xmlString = xmlString.Replace("<DoneButtonEnabled>false", "<DoneButtonEnabled>true");
                //}
                //if (temp == "true")
                //{
                //    xmlString = xmlString.Replace("DoneButtonDisabled", "DoneButtonEnabled");
                //    xmlString = xmlString.Replace("<DoneButtonEnabled>true", "<DoneButtonEnabled>false");
                //}
                //

                MainElement mainElement = new MainElement();
                mainElement = mainElement.JsonToClass(json);

                //XML HACK
                mainElement.CaseType = "";
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                if (mainElement.Repeated < 1)
                {
                    Log.LogCritical(methodName, "mainElement.Repeated = 1 // enforced");
                    mainElement.Repeated = 1;
                }

                return mainElement;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("Could not parse XML, got error: " + ex.Message, ex);
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

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        private async Task<List<string>> FieldValidation(MainElement mainElement)
        {
            string methodName = "Core.FieldValidation";

            Log.LogStandard(methodName, "called");

            List<string> errorLst = new List<string>();
            var dataItems = mainElement.DataItemGetAll();

            foreach (var dataItem in dataItems)
            {
                // entities

                if (dataItem.GetType() == typeof(EntitySearch))
                {
                    EntitySearch entitySearch = (EntitySearch)dataItem;
                    var temp = _sqlController.EntityGroupRead(entitySearch.EntityTypeId.ToString());
                    if (temp == null)
                        errorLst.Add("Element entitySearch.EntityTypeId:'" + entitySearch.EntityTypeId +
                                     "' is an reference to a local unknown EntitySearch group. Please update reference");
                }

                if (dataItem.GetType() == typeof(EntitySelect))
                {
                    EntitySelect entitySelect = (EntitySelect)dataItem;
                    var temp = _sqlController.EntityGroupRead(entitySelect.Source.ToString());
                    if (temp == null)
                        errorLst.Add("Element entitySelect.Source:'" + entitySelect.Source +
                                     "' is an reference to a local unknown EntitySearch group. Please update reference");
                }

                //

                // PDF

                if (dataItem.GetType() == typeof(ShowPdf))
                {
                    ShowPdf showPdf = (ShowPdf)dataItem;
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
                    errorLst.Add(
                        $"DataItem with label {dataItem.Label} did supply color {dataItem.Color}, but the only allowed values are: e8eaf6 for grey, ffe4e4 for red, f0f8db for green, e2f4fb for blue, e2f4fb for purple, fff6df for yellow, None for default or leave it blank.");
                }

                //
            }

            return errorLst;
        }

#pragma warning disable 1998
        private async Task<List<string>> CheckListValidation(MainElement mainElement)
#pragma warning restore 1998
        {
            string methodName = "Core.CheckListValidation";
            Log.LogStandard(methodName, "called");
            List<string> errorLst = new List<string>();

            List<string> acceptedColors = new List<string>();
            acceptedColors.Add(Constants.CheckListColors.Grey);
            acceptedColors.Add(Constants.CheckListColors.Red);
            acceptedColors.Add(Constants.CheckListColors.Green);

            if (!acceptedColors.Contains(mainElement.Color) && !string.IsNullOrEmpty(mainElement.Color))
            {
                errorLst.Add(
                    $"mainElement with label {mainElement.Label} did supply color {mainElement.Color}, but the only allowed colors are: grey, red, green or leave it blank.");
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
                    Log.LogStandard(methodName, "called");

                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach (var dataItem in dataItems)
                    {
                        // PDF
                        if (dataItem.GetType() == typeof(ShowPdf))
                        {
                            ShowPdf showPdf = (ShowPdf)dataItem;

                            if ((await PdfValidate(showPdf.Value, showPdf.Id)).Count != 0)
                            {
                                try
                                {
                                    //download file
                                    string downloadPath =
                                        Path.GetTempPath(); //await _sqlController.SettingRead(Settings.fileLocationPdf);
                                    long ticks = DateTime.UtcNow.Ticks;
                                    string tempFileName = $"{ticks}_temp.pdf";
                                    string filePathAndFileName = Path.Combine(downloadPath, tempFileName);
                                    try
                                    {
//                                        (new FileInfo(downloadPath)).Directory.Create();
                                        Directory.CreateDirectory(downloadPath);

                                        using var client = new HttpClient();
                                        var streamFile = await client.GetStreamAsync(showPdf.Value);

                                        await using var stream = new FileStream(filePathAndFileName, FileMode.Create);
                                        await streamFile.CopyToAsync(stream);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.LogException("Download failed. Path:'" + showPdf.Value + "'", ex.Message,
                                            ex);
                                        try
                                        {
                                            downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "pdf");
                                            Directory.CreateDirectory(downloadPath);

                                            filePathAndFileName = Path.Combine(downloadPath, tempFileName);
                                            using var client = new HttpClient();
                                            var streamFile = await client.GetStreamAsync(showPdf.Value);
                                            await using var stream = new FileStream(filePathAndFileName,
                                                FileMode.Create);
                                            await streamFile.CopyToAsync(stream);
                                        }
                                        catch (Exception e)
                                        {
                                            throw new Exception("Download failed. Path:'" + showPdf.Value + "'", e);
                                        }
                                    }

                                    //upload file
                                    string hash = await PdfUpload(filePathAndFileName);
                                    if (hash != null)
                                    {
                                        //rename local file
                                        FileInfo fileInfo = new FileInfo(filePathAndFileName);

                                        fileInfo.CopyTo(downloadPath + hash + ".pdf", true);
                                        fileInfo.Delete();

                                        await PutFileToStorageSystem(Path.Combine(downloadPath, $"{hash}.pdf"),
                                            $"{hash}.pdf");

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
                        //
                    }

                    return mainElement;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                    Log.LogStandard(methodName, "called");

                    List<string> errors = await TemplateValidation(mainElement) ?? new List<string>();

                    if (errors.Count > 0)
                        throw new Exception(
                            "mainElement failed TemplateValidation. Run TemplateValidation to see errors");

                    int templateId = await _sqlController.TemplateCreate(mainElement).ConfigureAwait(false);
                    Log.LogEverything(methodName, "Template id:" + templateId + " created in DB");
                    return templateId;
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve the template MainElement from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        /// <param name="language"></param>
        public async Task<MainElement> ReadeForm(int eFormId, Language language)
        {
            return await ReadeForm(eFormId, language, true);
        }

        public async Task<MainElement> ReadeForm(int eFormId, Language language, bool includeDummyFields)
        {
            string methodName = "Core.TemplateRead";
            try
            {
                if (Running())
                {
                    Log.LogStandard(methodName, "called");
                    Log.LogVariable(methodName, nameof(eFormId), eFormId);

                    return await _sqlController.ReadeForm(eFormId, language, includeDummyFields);
                }

                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);

                return await _sqlController.TemplateDelete(templateId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve the template meta data from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        /// <param name="timeZoneInfo"></param>
        /// <param name="language"></param>
        public async Task<Template_Dto> TemplateItemRead(int templateId, Language language)
        {
            string methodName = "Core.TemplateItemRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);

                return await _sqlController.TemplateItemRead(templateId, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                try
                {
                    Log.LogException(methodName, "(int " + templateId + ") failed", ex);
                }
                catch
                {
                    Log.LogException(methodName, "(int templateId) failed", ex);
                }

                throw new Exception("failed", ex);
            }
        }

        /// <summary>
        /// Tries to retrieve all templates meta data from the Microting DB
        /// </summary>
        /// <param name="includeRemoved">Filters list to only show all active or all including removed</param>
        /// <param name="timeZoneInfo"></param>
        /// <param name="language"></param>
        public async Task<List<Template_Dto>> TemplateItemReadAll(bool includeRemoved, TimeZoneInfo timeZoneInfo,
            Language language)
        {
            string methodName = "Core.TemplateItemReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(includeRemoved), includeRemoved);

                return await TemplateItemReadAll(includeRemoved, Constants.WorkflowStates.Created, "", true, "",
                    new List<int>(), timeZoneInfo, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                try
                {
                    Log.LogException(methodName, "(bool " + includeRemoved + ") failed", ex);
                }
                catch
                {
                    Log.LogException(methodName, "(bool includeRemoved) failed", ex);
                }

                throw new Exception("failed", ex);
            }
        }

        public async Task<List<Template_Dto>> TemplateItemReadAll(bool includeRemoved, string siteWorkflowState,
            string searchKey, bool descendingSort, string sortParameter, List<int> tagIds, TimeZoneInfo timeZoneInfo,
            Language language)
        {
            string methodName = "Core.TemplateItemReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(includeRemoved), includeRemoved);
                Log.LogVariable(methodName, nameof(searchKey), searchKey);
                Log.LogVariable(methodName, nameof(descendingSort), descendingSort);
                Log.LogVariable(methodName, nameof(sortParameter), sortParameter);
                Log.LogVariable(methodName, nameof(tagIds), tagIds.ToString());

                return await _sqlController.TemplateItemReadAll(includeRemoved, siteWorkflowState, searchKey,
                    descendingSort, sortParameter, tagIds, timeZoneInfo, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> TemplateSetTags(int templateId, List<int> tagIds)
        {
            string methodName = "Core.TemplateSetTags";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(tagIds), tagIds.ToString());

                return await _sqlController.TemplateSetTags(templateId, tagIds).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        //

        // case

        /// <summary>
        /// This method will send the mainElement to the Microting API endpoint.
        /// </summary>
        /// <param name="mainElement">eForm to be deployed</param>
        /// <param name="caseUId">Optional own id</param>
        /// <param name="siteUid">API id of the site to deploy the eForm at</param>
        /// <param name="folderId"></param>
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
        /// <param name="siteUids"></param>
        /// <param name="custom">Custom extended parameter</param>
        /// <param name="folderId"></param>
        public async Task<List<int>> CaseCreate(MainElement mainElement, string caseUId, List<int> siteUids,
            string custom, int? folderId)
        {
            string methodName = "Core.CaseCreate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                string siteIdsStr = string.Join(",", siteUids);
                Log.LogVariable(methodName, nameof(caseUId), caseUId);
                Log.LogVariable(methodName, nameof(siteIdsStr), siteIdsStr);
                Log.LogVariable(methodName, nameof(custom), custom);

                // check input
                DateTime start = DateTime.Parse(mainElement.StartDate.ToLongDateString());
                DateTime end = DateTime.Parse(mainElement.EndDate.ToLongDateString());

                if (end < DateTime.UtcNow)
                {
                    Log.LogStandard(methodName, $"mainElement.EndDate is set to {end}");
                    throw new ArgumentException("mainElement.EndDate needs to be a future date");
                }

                if (end <= start)
                {
                    Log.LogStandard(methodName, $"mainElement.StartDat is set to {start}");
                    throw new ArgumentException(
                        "mainElement.StartDate needs to be at least the day, before the remove date (mainElement.EndDate)");
                }

                if (caseUId != "" && mainElement.Repeated != 1)
                    throw new ArgumentException("if caseUId can only be used for mainElement.Repeated == 1");
                //

                //sending and getting a reply
                List<int> lstMUId = new List<int>();

                foreach (int siteUid in siteUids)
                {
                    int mUId = await SendXml(mainElement, siteUid);

                    if (mainElement.Repeated == 1)
                        await _sqlController.CaseCreate(mainElement.Id, siteUid, mUId, null, caseUId, custom,
                            DateTime.UtcNow, folderId).ConfigureAwait(false);
                    else
                        await _sqlController.CheckListSitesCreate(mainElement.Id, siteUid, mUId, folderId)
                            .ConfigureAwait(false);

                    CaseDto cDto = await _sqlController.CaseReadByMUId(mUId);
                    //InteractionCaseUpdate(cDto);
                    try
                    {
                        HandleCaseCreated?.Invoke(cDto, EventArgs.Empty);
                    }
                    catch
                    {
                        Log.LogWarning(methodName, "HandleCaseCreated event's external logic suffered an Expection");
                    }

                    Log.LogStandard(methodName, $"{cDto} has been created");

                    lstMUId.Add(mUId);
                }

                return lstMUId;
                throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUId), microtingUId);

                CaseDto cDto = await CaseLookupMUId(microtingUId).ConfigureAwait(false);
                return await _communicator.CheckStatus(cDto.MicrotingUId.ToString(), cDto.SiteUId)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        /// <summary>
        /// Tries to retrieve the answered full case from the DB
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        /// <param name="checkUId">If left empty, "0" or NULL it will try to retrieve the first check. Alternative is stating the Id of the specific check wanted to retrieve</param>
        /// <param name="language"></param>
        public async Task<ReplyElement> CaseRead(int microtingUId, int checkUId, Language language)
        {
            string methodName = "Core.CaseRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUId), microtingUId);
                Log.LogVariable(methodName, nameof(checkUId), checkUId);

                Microting.eForm.Infrastructure.Data.Entities.Case aCase =
                    await _sqlController.CaseReadFull(microtingUId, checkUId).ConfigureAwait(false);
                // handling if no match case found
                if (aCase == null)
                {
                    Log.LogWarning(methodName, $"No case found with MuuId:'{microtingUId}'");
                    return null;
                }
                //

                int id = aCase.Id;
                Log.LogEverything(methodName, $"aCase.Id:{aCase.Id}, found");

                ReplyElement replyElement = await _sqlController.CheckRead(microtingUId, checkUId, language);
                return replyElement;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<ReplyElement> CaseRead(int id, Language language)
        {
            string methodName = "Core.CaseRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                // Log.LogVariable(methodName, nameof(microtingUId), microtingUId);
                // Log.LogVariable(methodName, nameof(checkUId), checkUId);

                // Microting.eForm.Infrastructure.Data.Entities.Case aCase = await _sqlController.CaseReadFull(id).ConfigureAwait(false);
                // handling if no match case found
                // if (aCase == null)
                // {
                //     Log.LogWarning(methodName, $"No case found with MuuId:'{microtingUId}'");
                //     return null;
                // }
                //

                // int id = aCase.Id;
                // Log.LogEverything(methodName, $"aCase.Id:{aCase.Id}, found");

                ReplyElement replyElement = await _sqlController.CheckRead(id, language);
                return replyElement;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<CaseDto> CaseReadByCaseId(int id)
        {
            string methodName = "Core.CaseReadByCaseId";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(id), id);

                return await _sqlController.CaseReadByCaseId(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<int?> CaseReadFirstId(int? templateId, string workflowState)
        {
            string methodName = "Core.CaseReadFirstId";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(workflowState), workflowState);

                return await _sqlController.CaseReadFirstId(templateId, workflowState);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end, TimeZoneInfo timeZoneInfo)
        {
            return CaseReadAll(templateId, start, end, Constants.WorkflowStates.NotRemoved, null, timeZoneInfo);
        }

        public async Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState,
            string searchKey, TimeZoneInfo timeZoneInfo)
        {
            string methodName = "Core.CaseReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(start), start);
                Log.LogVariable(methodName, nameof(end), end);
                Log.LogVariable(methodName, nameof(workflowState), workflowState);

                return await CaseReadAll(templateId, start, end, workflowState, searchKey, false, null, timeZoneInfo)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<List<Case>> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState,
            string searchKey, bool descendingSort, string sortParameter, TimeZoneInfo timeZoneInfo)
        {
            string methodName = "Core.CaseReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(start), start);
                Log.LogVariable(methodName, nameof(end), end);
                Log.LogVariable(methodName, nameof(workflowState), workflowState);
                Log.LogVariable(methodName, nameof(descendingSort), descendingSort);
                Log.LogVariable(methodName, nameof(sortParameter), sortParameter);

                return await _sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey,
                    descendingSort, sortParameter, timeZoneInfo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public Task<CaseList> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState,
            string searchKey, bool descendingSort, string sortParameter, int pageIndex, int pageSize)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
            return CaseReadAll(templateId, start, end, workflowState, searchKey, descendingSort, sortParameter,
                pageIndex, pageSize, timeZoneInfo);
        }

        public async Task<CaseList> CaseReadAll(int? templateId, DateTime? start, DateTime? end, string workflowState,
            string searchKey, bool descendingSort, string sortParameter, int pageIndex, int pageSize,
            TimeZoneInfo timeZoneInfo)
        {
            string methodName = "Core.CaseReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(start), start);
                Log.LogVariable(methodName, nameof(end), end);
                Log.LogVariable(methodName, nameof(workflowState), workflowState);
                Log.LogVariable(methodName, nameof(descendingSort), descendingSort);
                Log.LogVariable(methodName, nameof(sortParameter), sortParameter);
                Log.LogVariable(methodName, nameof(pageIndex), pageIndex);
                Log.LogVariable(methodName, nameof(pageSize), pageSize);
                Log.LogVariable(methodName, nameof(searchKey), searchKey);

                return await _sqlController.CaseReadAll(templateId, start, end, workflowState, searchKey,
                    descendingSort, sortParameter, pageIndex, pageSize, timeZoneInfo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        /// <summary>
        /// Tries to set the resultats of a case to new values
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="newFieldValuePairLst">List of '[fieldValueId]|[new value]'</param>
        /// <param name="newCheckListValuePairLst">List of '[checkListValueId]|[new status]'</param>
        public async Task<bool> CaseUpdate(int caseId, List<string> newFieldValuePairLst,
            List<string> newCheckListValuePairLst)
        {
            string methodName = "Core.CaseUpdate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(caseId), caseId);

                if (newFieldValuePairLst == null)
                    newFieldValuePairLst = new List<string>();

                if (newCheckListValuePairLst == null)
                    newCheckListValuePairLst = new List<string>();

                int id = 0;
                string value = "";

                foreach (string str in newFieldValuePairLst)
                {
                    id = int.Parse(_t.SplitToList(str, 0, false));
                    value = _t.SplitToList(str, 1, false);
                    await _sqlController.FieldValueUpdate(caseId, id, value).ConfigureAwait(false);
                }

                foreach (string str in newCheckListValuePairLst)
                {
                    id = int.Parse(_t.SplitToList(str, 0, false));
                    value = _t.SplitToList(str, 1, false);
                    await _sqlController.CheckListValueStatusUpdate(caseId, id, value).ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return false;
            }
        }

        public async Task<bool> CaseDelete(int templateId, int siteUId)
        {
            string methodName = "Core.CaseDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(siteUId), siteUId);

                return await CaseDelete(templateId, siteUId, Constants.WorkflowStates.NotRemoved).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                try
                {
                    Log.LogException(methodName, $"(int {templateId}, int {siteUId}) failed", ex);
                }
                catch
                {
                    Log.LogException(methodName, "(int templateId, int siteUId) failed", ex);
                }

                return false;
            }
        }

        public async Task<bool> CaseDelete(int templateId, int siteUId, string workflowState)
        {
            string methodName = "Core.CaseDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(siteUId), siteUId);
                Log.LogVariable(methodName, nameof(workflowState), workflowState);

                List<string> errors = new List<string>();
                foreach (int microtingUId in await _sqlController.CheckListSitesRead(templateId, siteUId, workflowState)
                             .ConfigureAwait(false))
                {
                    if (!CaseDelete(microtingUId).GetAwaiter().GetResult())
                    {
                        string error = $"Failed to delete case with microtingUId: {microtingUId}";
                        errors.Add(error);
                    }
                }

                if (errors.Any())
                {
                    throw new Exception(String.Join("\n", errors));
                }

                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    Log.LogException(methodName,
                        $"(int {templateId}, int {siteUId}, string {workflowState}) failed", ex);
                }
                catch
                {
                    Log.LogException(methodName, "(int templateId, int siteUId, string workflowState) failed", ex);
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

            Log.LogVariable(methodName, nameof(microtingUId), microtingUId);

            var cDto = await _sqlController.CaseReadByMUId(microtingUId).ConfigureAwait(false);
            string xmlResponse =
                await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId).ConfigureAwait(false);
            Log.LogEverything(methodName, "XML response is 1218 : " + xmlResponse);
            Response resp = new Response();

            if (xmlResponse.Contains("Error occured: Contact Microting"))
            {
                Log.LogEverything(methodName, $"XML response is : {xmlResponse}");
                Log.LogEverything("DELETE ERROR", $"failed for microtingUId: {microtingUId}");
                return false;
            }

            if (xmlResponse.Contains("Error"))
            {
                try
                {
                    resp = resp.XmlToClass(xmlResponse);
                    Log.LogException(methodName, "failed", new Exception(
                        $"Error from Microting server: {resp.Value}"));
                    return false;
                }
                catch (Exception ex)
                {
                    try
                    {
                        Log.LogException(methodName, $"(string {microtingUId}) failed", ex);
                        throw;
                    }
                    catch
                    {
                        Log.LogException(methodName, "(string microtingUId) failed", ex);
                        throw;
                    }
                }
            }

            if (xmlResponse.Contains("Parsing in progress: Can not delete check list!"))
                for (int i = 1; i < 102; i++)
                {
                    Thread.Sleep(i * 5000);
                    xmlResponse = await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId)
                        .ConfigureAwait(false);
                    try
                    {
                        resp = resp.XmlToClass(xmlResponse);
                        if (resp.Type.ToString() == "Success")
                        {
                            Log.LogStandard(methodName,
                                cDto +
                                $" has been removed from server in retry loop with i being : {i.ToString()}");
                            break;
                        }

                        Log.LogEverything(methodName,
                            $"retrying delete and i is {i.ToString()} and xmlResponse" + xmlResponse);
                    }
                    catch (Exception ex)
                    {
                        Log.LogEverything(methodName,
                            $" Exception is: {ex.Message}, retrying delete and i is {i.ToString()} and xmlResponse" +
                            xmlResponse);
                    }
                }

            Log.LogEverything(methodName, "XML response:");
            Log.LogEverything(methodName, xmlResponse);

            resp = resp.XmlToClass(xmlResponse);
            if (resp.Type.ToString() == "Success")
            {
                Log.LogStandard(methodName, $"{cDto} has been removed from server");
                try
                {
                    bool result = await _sqlController.CaseDelete(microtingUId).ConfigureAwait(false);
                    try
                    {
                        await _sqlController.CaseDeleteReversed(microtingUId).ConfigureAwait(false);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "There is more than one instance.")
                        {
                            cDto = await _sqlController.CaseReadByMUId(microtingUId);
                            await FireHandleCaseDeleted(cDto).ConfigureAwait(false);
                            Log.LogStandard(methodName, $"{cDto} has been removed");
                            return result;
                        }

                        Log.LogException(methodName, "(string microtingUId) failed", ex);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Log.LogException(methodName, "(string microtingUId) failed", ex);
                }
            }

            return false;
        }

        public async Task<bool> CaseDeleteResult(int caseId)
        {
            string methodName = "Core.CaseDeleteResult";
            Log.LogStandard(methodName, "called");
            Log.LogVariable(methodName, nameof(caseId), caseId);
            try
            {
                return await _sqlController.CaseDeleteResult(caseId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                try
                {
                    Log.LogException(methodName, $"(int {caseId}) failed", ex);
                }
                catch
                {
                    Log.LogException(methodName, "(int caseId) failed", ex);
                }

                return false;
            }
        }

        public async Task<bool> CaseUpdateFieldValues(int id, Language language)
        {
            string methodName = "Core.CaseUpdateFieldValues";
            Log.LogStandard(methodName, "called");
            Log.LogVariable(methodName, nameof(id), id);
            try
            {
                return await _sqlController.CaseUpdateFieldValues(id, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return false;
            }
        }

        public async Task<CaseDto> CaseLookup(int microtingUId, int checkUId)
        {
            string methodName = "Core.CaseLookup";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUId), microtingUId);
                Log.LogVariable(methodName, nameof(checkUId), checkUId);

                return await _sqlController.CaseLookup(microtingUId, checkUId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUId), microtingUId);

                return await _sqlController.CaseReadByMUId(microtingUId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(caseId), caseId);

                return await _sqlController.CaseReadByCaseId(caseId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(caseUId), caseUId);

                return await _sqlController.CaseReadByCaseUId(caseUId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUId), microtingUId);
                Log.LogVariable(methodName, nameof(checkUId), checkUId);

                Microting.eForm.Infrastructure.Data.Entities.Case aCase =
                    await _sqlController.CaseReadFull(microtingUId, checkUId).ConfigureAwait(false);
                // handling if no match case found
                if (aCase == null)
                {
                    Log.LogWarning(methodName, $"No case found with MuuId:'{microtingUId}'");
                    return -1;
                }

                //
                int id = aCase.Id;
                Log.LogEverything(methodName, $"aCase.Id:{aCase.Id}, found");

                return id;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
            string customPathForUploadedData, string decimalSeparator, string thousandSeparator, bool utcTime,
            CultureInfo cultureInfo, TimeZoneInfo timeZoneInfo, Language language, bool gpsCoordinates,
            bool includeCheckListText)
        {
            string methodName = "Core.CasesToCsv";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId.ToString());
                Log.LogVariable(methodName, nameof(start), start.ToString());
                Log.LogVariable(methodName, nameof(end), end.ToString());
                Log.LogVariable(methodName, nameof(pathAndName), pathAndName);
                Log.LogVariable(methodName, nameof(customPathForUploadedData), customPathForUploadedData);

                List<List<string>> dataSet = await GenerateDataSetFromCases(templateId, start, end,
                    customPathForUploadedData, decimalSeparator, thousandSeparator, utcTime, cultureInfo, timeZoneInfo,
                    language, includeCheckListText, gpsCoordinates).ConfigureAwait(false);

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
                await textWriter.WriteAsync(stringBuilder.ToString());
                await textWriter.FlushAsync();
                textWriter.Close();
                await textWriter.DisposeAsync();

                return Path.GetFullPath(pathAndName);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        // /// <summary>
        // /// Tries to retrieve all connected cases to a templat, and delivers them as a CSV fil, at the returned path's location
        // /// </summary>
        // /// <param name="templateId">The templat's ID to be used. Null will remove this limit</param>
        // /// <param name="start">Only cases from after this time limit. Null will remove this limit</param>
        // /// <param name="end">Only cases from before this time limit. Null will remove this limit</param>
        // /// <param name="pathAndName">Location where fil is to be placed, along with fil name. No extension needed. Relative or absolut</param>
        // /// <param name="customPathForUploadedData"></param>
        // public Task<string> CasesToCsv(int templateId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData)
        // {
        //     CultureInfo cultureInfo = new CultureInfo("de-DE");
        //     TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
        //     Language language = DbContextHelper.GetDbContext().Languages.Single(x => x.LanguageCode == "da");
        //     return CasesToCsv(templateId, start, end, pathAndName, customPathForUploadedData, ".", "", false, cultureInfo, timeZoneInfo, language);
        // }

        public async Task<string> CaseToJasperXml(CaseDto cDto, ReplyElement reply, int caseId, string timeStamp,
            string customPathForUploadedData, string customXMLContent, Language language)
        {
            string methodName = "Core.CaseToJasperXml";
            try
            {
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(caseId), caseId.ToString());
                Log.LogVariable(methodName, nameof(timeStamp), timeStamp);

                timeStamp ??= $"{DateTime.UtcNow:yyyyMMdd}_{DateTime.UtcNow:hhmmss}";

                //get needed data
//                    CaseDto cDto = CaseLookupCaseId(caseId);
//                    ReplyElement reply = CaseRead(cDto.MicrotingUId, cDto.CheckUId);
                if (reply == null)
                    throw new NullReferenceException($"reply is null. Delete or fix the case with ID {caseId}");
                string clsLst = "";
                string fldLst = "";
                GetChecksAndFields(ref clsLst, ref fldLst, reply.ElementList, customPathForUploadedData);
                string extrafldLst = await GetExtraFieldValues(caseId, customPathForUploadedData, language);
                Log.LogVariable(methodName, nameof(clsLst), clsLst);
                Log.LogVariable(methodName, nameof(fldLst), fldLst);

                // convert to jasperXml

                // TODO make this dynamic, so it can be defined by user, which timezone to show data in.
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
                reply.DoneAt = TimeZoneInfo.ConvertTimeFromUtc(reply.DoneAt, timeZoneInfo);
                string jasperXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                                   + Environment.NewLine + "<root>"
                                   + Environment.NewLine + "<C" + reply.Id + " case_id=\"" + caseId +
                                   "\" case_name=\"" + reply.Label + "\" serial_number=\"" + caseId + "/" +
                                   cDto.MicrotingUId + "\" check_list_status=\"approved\">"
                                   + Environment.NewLine + "<worker>" +
                                   dbContext.Workers.Single(x => x.Id == reply.DoneById).full_name() + "</worker>"
                                   + Environment.NewLine + "<check_id>" + reply.MicrotingUId + "</check_id>"
                                   + Environment.NewLine + "<date>" +
                                   reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture) +
                                   "</date>"
                                   + Environment.NewLine + "<check_date>" +
                                   reply.DoneAt.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture) +
                                   "</check_date>"
                                   + Environment.NewLine + "<site_name>" +
                                   dbContext.Sites.Single(x => x.MicrotingUid == reply.SiteMicrotingUuid).Name +
                                   "</site_name>"
                                   + Environment.NewLine + "<check_lists>"
                                   + clsLst
                                   + Environment.NewLine + "</check_lists>"
                                   + Environment.NewLine + "<fields>"
                                   + fldLst
                                   + Environment.NewLine + "</fields>"
                                   + Environment.NewLine + "<extra_fields>"
                                   + extrafldLst
                                   + Environment.NewLine + "</extra_fields>"
                                   + Environment.NewLine + "</C" + reply.Id + ">"
                                   + customXMLContent
                                   + Environment.NewLine + "</root>";
                Log.LogVariable(methodName, nameof(jasperXml), jasperXml);
                //

                //place in settings allocated placement
                string fullPath = Path.Combine(Path.GetTempPath(), "results",
                    $"{timeStamp}_{caseId}.xml");
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "results"));
                await File.WriteAllTextAsync(fullPath, jasperXml.Trim(), Encoding.UTF8);

                Log.LogVariable(methodName, nameof(fullPath), fullPath);
                return fullPath;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        // sdkSettings

        public async Task<string> GetSdkSetting(Settings settingName)
        {
            string methodName = "Core.GetSdkSetting";
            Log.LogStandard(methodName, "called");
            try
            {
                return await _sqlController.SettingRead(settingName).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return "N/A";
            }
        }

        public async Task<bool> SetSdkSetting(Settings settingName, string settingValue)
        {
            string methodName = "Core.SetSdkSetting";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(settingValue), settingValue);

                await _sqlController.SettingUpdate(settingName, settingValue).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        //

        public Task<string> CaseToPdf(int caseId, string jasperTemplate, string timeStamp,
            string customPathForUploadedData, string customXmlContent, Language language)
        {
            return CaseToPdf(caseId, jasperTemplate, timeStamp, customPathForUploadedData, "pdf", customXmlContent,
                language);
        }

        private async Task<string> JasperToPdf(int caseId, string jasperTemplate, string timeStamp)
        {
            string methodName = "Core.JasperToPdf";
            // run jar
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            string localJasperExporter = Path.Combine(Path.GetTempPath(), "utils",
                "JasperExporter.jar");
            if (!File.Exists(localJasperExporter))
            {
                using var webClient = new HttpClient();
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "utils"));
                var streamFile = await webClient.GetStreamAsync(
                    "https://github.com/microting/JasperExporter/releases/download/stable/JasperExporter.jar");

                await using var stream = new FileStream(localJasperExporter, FileMode.Create);
                await streamFile.CopyToAsync(stream);
            }

            string templateFile = Path.Combine(Path.GetTempPath(), "templates", jasperTemplate, "compact",
                $"{jasperTemplate}.jrxml");
            //if (!File.Exists(templateFile))
            //{
            var zipFileName = $"{jasperTemplate}_jasper_compact.zip";

            var saveFolder =
                Path.Combine(Path.GetTempPath(), "templates", jasperTemplate);
            var zipArchiveFolder =
                Path.Combine(Path.GetTempPath(), "templates", Path.Combine("zip-archives", jasperTemplate));
            var extractPath = Path.Combine(saveFolder);

            var filePath = Path.Combine(zipArchiveFolder, zipFileName);

            var objectResponse = await GetFileFromS3Storage(zipFileName);
            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "templates"));
            Directory.CreateDirectory(zipArchiveFolder);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await using var fileStream = File.Create(filePath);
            await objectResponse.ResponseStream.CopyToAsync(fileStream);
            fileStream.Close();
            objectResponse.Dispose();
            await fileStream.DisposeAsync();
            // extract
            var fastZip = new FastZip();
            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(filePath, extractPath, null);
            foreach (var file in Directory.GetFiles(Path.Combine(extractPath, "compact"), "*.jasper"))
            {
                File.Delete(file);
            }
            //throw new FileNotFoundException($"jrxml template was not found at {templateFile}");

            //}
            string _dataSourceXML = Path.Combine(Path.GetTempPath(), "results",
                $"{timeStamp}_{caseId}.xml");

            if (!File.Exists(_dataSourceXML))
            {
                throw new FileNotFoundException("Case result xml was not found at " + _dataSourceXML);
            }

            string _resultDocument = Path.Combine(Path.GetTempPath(), "results",
                $"{timeStamp}_{caseId}.pdf");

            string command =
                $" -Xms512m -Xmx2g -Dfile.encoding=UTF-8 -jar {localJasperExporter} -template=\"{templateFile}\" -type=\"pdf\" -uri=\"{_dataSourceXML}\" -outputFile=\"{_resultDocument}\"";

            Log.LogVariable(methodName, nameof(command), command);
            p.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "java.exe" : "java";

            p.StartInfo.Arguments = command;
            p.StartInfo.Verb = "runas";
            p.Start();
            // IF needed:
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = await p.StandardOutput.ReadToEndAsync();
            Log.LogVariable(methodName, nameof(output), output);
            await p.WaitForExitAsync();

            if (output != "")
                if (output.Contains("ERROR"))
                {
                    throw new Exception("output='" + output +
                                        "', expected to be no output. This indicates an error has happened");
                }

            //

            return _resultDocument;
        }

        private async Task<string> DocxToPdf(int caseId, string templateId, string timeStamp,
            Microting.eForm.Infrastructure.Data.Entities.Case dbCase, CaseDto cDto, string customPathForUploadedData,
            string customXmlContent, string fileType, Language language)
        {
            SortedDictionary<string, string> valuePairs = new SortedDictionary<string, string>();
            // TODO make this dynamic, so it can be defined by user, which timezone to show data in.
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
            await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();

            var checkListTranslations = await dbContext.CheckListTranslations
                .Where(x => x.CheckListId == dbCase.CheckListId)
                .Where(x => x.LanguageId == language.Id)
                .ToListAsync();

            var doneAt = TimeZoneInfo.ConvertTimeFromUtc((DateTime)dbCase.DoneAt, timeZoneInfo);
            // get base values
            valuePairs.Add("F_CaseName", checkListTranslations.First().Text.Replace("&", "&amp;"));
            valuePairs.Add("F_SerialNumber", $"{caseId}/{cDto.MicrotingUId}");
            valuePairs.Add("F_Worker",
                dbContext.Workers.Single(x => x.Id == dbCase.WorkerId).full_name().Replace("&", "&amp;"));
            valuePairs.Add("F_CheckId", dbCase.MicrotingCheckUid.ToString());
            valuePairs.Add("F_CheckDate", doneAt.ToString("dd-MM-yyyy"));
            valuePairs.Add("F_SiteName", dbContext.Sites.Single(x => x.Id == dbCase.SiteId).Name.Replace("&", "&amp;"));

            // get field_values
            List<KeyValuePair<string, List<string>>> pictures = new List<KeyValuePair<string, List<string>>>();
            List<KeyValuePair<string, string>> signatures = new List<KeyValuePair<string, string>>();
            List<int> caseIds = new List<int>();
            caseIds.Add(caseId);
            List<FieldValue> fieldValues =
                await _sqlController.FieldValueReadList(caseIds, language).ConfigureAwait(false);

            List<FieldDto> allFields = await _sqlController.TemplateFieldReadAll(int.Parse(templateId), language)
                .ConfigureAwait(false);
            foreach (FieldDto field in allFields)
            {
                valuePairs.Add($"F_{field.Id}", "");
            }

            bool noImageTitle = false;

            if (!string.IsNullOrEmpty(customXmlContent))
            {
                foreach (KeyValuePair<string, string> keyValuePair in ParseCustomXmlContent(customXmlContent))
                {
                    valuePairs.Add(keyValuePair.Key, keyValuePair.Value);
                    if (keyValuePair.Key == "F_noImageTitle")
                    {
                        noImageTitle = true;
                    }
                }
            }

            SortedDictionary<string, int> imageFieldCountList = new SortedDictionary<string, int>();
            foreach (FieldValue fieldValue in fieldValues)
            {
                string fileContent = "";
                switch (fieldValue.FieldType)
                {
                    case Constants.FieldTypes.MultiSelect:
                        valuePairs[$"F_{fieldValue.FieldId}"] =
                            fieldValue.ValueReadable.Replace("|", @"</w:t><w:br/><w:t>");
                        break;
                    case Constants.FieldTypes.Picture:
                        if (fieldValue.UploadedDataObj != null)
                        {
                            Microting.eForm.Infrastructure.Data.Entities.Field field =
                                await dbContext.Fields.FirstOrDefaultAsync(x => x.Id == fieldValue.FieldId);
                            CheckList checkList =
                                await dbContext.CheckLists.FirstOrDefaultAsync(x => x.Id == field.CheckListId);

                            string geoTag = "";
                            if (fieldValue.Latitude != null)
                            {
                                geoTag =
                                    $"https://www.google.com/maps/place/{fieldValue.Latitude},{fieldValue.Longitude}";
                            }

                            var list = new List<string>();
                            string fileName =
                                $"{fieldValue.UploadedDataObj.Id}_700_{fieldValue.UploadedDataObj.Checksum}{fieldValue.UploadedDataObj.Extension}";

                            // if (_swiftEnabled)
                            // {
                            //     using SwiftObjectGetResponse swiftObjectGetResponse = await GetFileFromSwiftStorage(fileName).ConfigureAwait(false);
                            //     using var image = new MagickImage(swiftObjectGetResponse.ObjectStreamContent);
                            //     fileContent = image.ToBase64();
                            // }

                            if (_s3Enabled)
                            {
                                try
                                {
                                    using GetObjectResponse response =
                                        await GetFileFromS3Storage(fileName);
                                    using var image = new MagickImage(response.ResponseStream);
                                    var profile = image.GetExifProfile();
                                    // Write all values to the console
                                    try
                                    {
                                        foreach (var value in profile.Values)
                                        {
                                            Console.WriteLine("{0}({1}): {2}", value.Tag, value.DataType, value);

                                            if (value.Tag == ExifTag.Orientation)
                                            {
                                                if (value.GetValue().ToString() == "6")
                                                {
                                                    image.Rotate(90);
                                                }
                                                else if (value.GetValue().ToString() == "8")
                                                {
                                                    image.Rotate(270);
                                                }
                                                else if (value.GetValue().ToString() == "3")
                                                {
                                                    image.Rotate(180);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // Console.WriteLine(e);
                                    }

                                    //image.Rotate(90); // This is done, since all images are apparently rotated 90 degrees in the wrong direction.
                                    fileContent = image.ToBase64();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);

                                    string bigFilename = fileName;
                                    fileName =
                                        $"{fieldValue.UploadedDataObj.Id}_{fieldValue.UploadedDataObj.Checksum}{fieldValue.UploadedDataObj.Extension}";


                                    Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "pictures"));

                                    using GetObjectResponse response1 =
                                        await GetFileFromS3Storage(fileName);
                                    string filePath = Path.Combine(Path.GetTempPath(), "pictures", fileName);
                                    await using (var fileStream = File.Create(filePath))
                                    {
                                        await response1.ResponseStream.CopyToAsync(fileStream);
                                    }

                                    File.Copy(filePath, Path.Combine(Path.GetTempPath(), "pictures", bigFilename),
                                        true);

                                    string filePathResized = Path.Combine(Path.GetTempPath(), "pictures", bigFilename);
                                    using (var image = new MagickImage(filePathResized))
                                    {
                                        decimal currentRation = image.Height / (decimal)image.Width;
                                        int newWidth = 700;
                                        int newHeight = (int)Math.Round((currentRation * newWidth));

                                        image.Resize(newWidth, newHeight);
                                        image.Crop(newWidth, newHeight);
                                        await image.WriteAsync(filePathResized);
                                        await PutFileToStorageSystem(Path.Combine(_fileLocationPicture, bigFilename),
                                            bigFilename).ConfigureAwait(false);
                                        fileContent = image.ToBase64();
                                        image.Dispose();
                                    }

                                    File.Delete(filePathResized);
                                    File.Delete(filePath);
                                }
                            }

                            list.Add(fileContent);
                            list.Add(geoTag);
                            list.Add(field.Id.ToString());
                            CheckListTranslation checkListTranslation =
                                await dbContext.CheckListTranslations.FirstAsync(x =>
                                    x.CheckListId == checkList.Id && x.LanguageId == language.Id);
                            FieldTranslation fieldTranslation =
                                await dbContext.FieldTranslations.FirstAsync(x =>
                                    x.FieldId == field.Id && x.LanguageId == language.Id);
                            pictures.Add(noImageTitle
                                ? new KeyValuePair<string, List<string>>(
                                    $"{fieldTranslation.Text.Replace("&", "&amp;")}", list)
                                : new KeyValuePair<string, List<string>>(
                                    $"{checkListTranslation.Text.Replace("&", "&amp;")} - {fieldTranslation.Text.Replace("&", "&amp;")}",
                                    list));

                            if (imageFieldCountList.ContainsKey($"FCount_{fieldValue.FieldId}"))
                            {
                                imageFieldCountList[$"FCount_{fieldValue.FieldId}"] += 1;
                            }
                            else
                            {
                                imageFieldCountList[$"FCount_{fieldValue.FieldId}"] = 1;
                            }
                        }
                        else
                        {
                            imageFieldCountList[$"FCount_{fieldValue.FieldId}"] = 0;
                        }

                        break;
                    case Constants.FieldTypes.Signature:
                        if (fieldValue.UploadedDataObj != null)
                        {
                            Microting.eForm.Infrastructure.Data.Entities.Field field =
                                await dbContext.Fields.FirstOrDefaultAsync(x => x.Id == fieldValue.FieldId);

                            // if (_swiftEnabled)
                            // {
                            //     SwiftObjectGetResponse swiftObjectGetResponse = await GetFileFromSwiftStorage(fieldValue.UploadedDataObj.FileName).ConfigureAwait(false);
                            //     using var image = new MagickImage(swiftObjectGetResponse.ObjectStreamContent);
                            //     fileContent = image.ToBase64();
                            // }

                            if (_s3Enabled)
                            {
                                using GetObjectResponse response =
                                    await GetFileFromS3Storage(fieldValue.UploadedDataObj.FileName)
                                        .ConfigureAwait(false);
                                using var image = new MagickImage(response.ResponseStream);
                                fileContent = image.ToBase64();
                            }

                            signatures.Add(new KeyValuePair<string, string>($"F_{fieldValue.FieldId}", fileContent));

                            valuePairs.Remove($"F_{field.Id}");
                        }

                        break;
                    case Constants.FieldTypes.CheckBox:
                        valuePairs[$"F_{fieldValue.FieldId}"] = !string.IsNullOrEmpty(fieldValue.ValueReadable)
                            ? (
                                fieldValue.ValueReadable.ToLower() == "checked" ? "&#10004;" : "")
                            : "";
                        break;
                    case Constants.FieldTypes.FieldGroup:
                        break;
                    case Constants.FieldTypes.Timer:
                        valuePairs[$"F_{fieldValue.FieldId}"] = TimeSpan
                            .FromMilliseconds(Double.Parse(fieldValue.Value.Split('|')[3])).ToString();
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
                                fieldValue.ValueReadable = fieldValue.ValueReadable.Replace("\n", "|||");
                                fieldValue.ValueReadable = Regex.Replace(fieldValue.ValueReadable, "<.*?>",
                                    string.Empty);
                                fieldValue.ValueReadable =
                                    fieldValue.ValueReadable.Replace("\t", @"</w:t><w:tab/><w:t>");
                                fieldValue.ValueReadable =
                                    fieldValue.ValueReadable.Replace("|||", @"</w:t><w:br/><w:t>");
                                valuePairs[$"F_{fieldValue.FieldId}"] = fieldValue.ValueReadable;
                            }
                        }

                        break;
                }
            }

            List<string> paragraphTextsForRemove = new List<string>();

            foreach (KeyValuePair<string, int> keyValuePair in imageFieldCountList)
            {
                if (keyValuePair.Value == 0)
                {
                    paragraphTextsForRemove.Add(keyValuePair.Key.Replace("FCount_", "FPictures_"));
                }

                valuePairs.Add(keyValuePair.Key, keyValuePair.Value.ToString());
            }

            // get check_list_values
            List<CheckListValue> checkListValues = await _sqlController.CheckListValueReadList(caseIds);
            foreach (CheckListValue checkListValue in checkListValues)
            {
                valuePairs.Add($"C_{checkListValue.Id}", checkListValue.Status);
            }

            // Try to create the results directory first
            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "results"));

            string resultDocument = Path.Combine(Path.GetTempPath(), "results",
                $"{timeStamp}_{caseId}.docx");

            if (GetSdkSetting(Settings.s3Enabled).Result.ToLower() == "true")
            {
                try
                {
                    var objectResponse = await GetFileFromS3Storage($"{templateId}.docx");
                    Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "results"));
                    await using var fileStream = File.Create(resultDocument);
                    await objectResponse.ResponseStream.CopyToAsync(fileStream);
                }
                catch
                {
                    try
                    {
                        var objectResponse = await GetFileFromS3Storage($"{templateId}_docx_compact.zip");
                        string zipFileName = Path.Combine(Path.GetTempPath(), $"{templateId}.zip");
                        await using var fileStream = File.Create(zipFileName);
                        await objectResponse.ResponseStream.CopyToAsync(fileStream);
                        fileStream.Close();
                        var fastZip = new FastZip();
                        // Will always overwrite if target filenames already exist
                        string extractPath = Path.Combine(Path.GetTempPath(), "results");
                        Directory.CreateDirectory(extractPath);
                        fastZip.ExtractZip(zipFileName, extractPath, "");
                        string extractedFile = Path.Combine(extractPath, "compact", $"{templateId}.docx");
                        await PutFileToStorageSystem(extractedFile, $"{templateId}.docx");
                        File.Move(extractedFile, resultDocument);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            Log.LogEverything("WordprocessingDocument", "Open");
            WordprocessingDocument wordDoc = WordprocessingDocument.Open(resultDocument, true);

            Log.LogEverything("ReportHelper.SearchAndReplace", "Start");
            ReportHelper.SearchAndReplace(valuePairs, wordDoc);

            Log.LogEverything("ReportHelper.InsertImages", "Start");
            ReportHelper.InsertImages(wordDoc, pictures);

            Log.LogEverything("ReportHelper.signatures", "Start");
            ReportHelper.InsertSignature(wordDoc, signatures);

            foreach (var text in paragraphTextsForRemove)
            {
                var paragraphs = wordDoc.MainDocumentPart.Document.Body.Descendants<Paragraph>();
                var paragraph = paragraphs.FirstOrDefault(p => p.InnerText.Contains(text));
                if (paragraph != null)
                {
                    var runs = paragraph.Descendants<Run>().ToList();
                    foreach (var run in runs)
                    {
                        var textFields = run.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().ToList();
                        foreach (var textField in textFields)
                        {
                            textField.Text = "";
                        }
                    }

                    // if (textField != null)
                    // {
                    //     textField.Text = "";
                    // }
                }
            }
            //ReportHelper.ValidateWordDocument(resultDocument);

            Log.LogEverything("ReportHelper.signatures", "wordDoc.Save");
            wordDoc.Save();
            Log.LogEverything("ReportHelper.signatures", "wordDoc.Dispose");
            wordDoc.Dispose();

            if (fileType == "pdf")
            {
                string outputFolder = Path.Combine(Path.GetTempPath(), "results");

                ReportHelper.ConvertToPdf(resultDocument, outputFolder);
                return Path.Combine(Path.GetTempPath(), "results",
                    $"{timeStamp}_{caseId}.pdf");
            }

            return Path.Combine(Path.GetTempPath(), "results",
                $"{timeStamp}_{caseId}.docx");
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

        public async Task<string> CaseToPdf(int caseId, string jasperTemplate, string timeStamp,
            string customPathForUploadedData, string fileType, string customXmlContent, Language language)
        {
            if (fileType != "pdf" && fileType != "docx" && fileType != "pptx")
            {
                throw new ArgumentException(
                    $"Filetypes allowed are only: pdf, docx, pptx, currently specified was {fileType}");
            }

            string methodName = "Core.CaseToPdf";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(caseId), caseId.ToString());
                Log.LogVariable(methodName, nameof(jasperTemplate), jasperTemplate);

                timeStamp ??= DateTime.UtcNow.ToString("yyyyMMdd") + "_" + DateTime.UtcNow.ToString("hhmmss");
                CaseDto cDto = await CaseLookupCaseId(caseId).ConfigureAwait(false);
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                var dbCase = await dbContext.Cases.FirstAsync(x => x.Id == caseId);
                var checkList = await dbContext.CheckLists.FirstAsync(x => x.Id == dbCase.CheckListId);

                string resultDocument = "";

                if (checkList.JasperExportEnabled)
                {
                    ReplyElement reply = await CaseRead((int)cDto.MicrotingUId, (int)cDto.CheckUId, language)
                        .ConfigureAwait(false);
                    await CaseToJasperXml(cDto, reply, caseId, timeStamp, customPathForUploadedData, customXmlContent,
                        language).ConfigureAwait(false);
                    resultDocument = await JasperToPdf(caseId, jasperTemplate, timeStamp).ConfigureAwait(false);
                }
                else
                {
                    resultDocument = await DocxToPdf(caseId, jasperTemplate, timeStamp, dbCase, cDto,
                        customPathForUploadedData, customXmlContent, fileType, language).ConfigureAwait(false);
                }

                //return path
                string path = Path.GetFullPath(resultDocument);
                Log.LogVariable(methodName, nameof(path), path);
                return path;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }
        //

        // site

        public async Task<SiteDto> SiteCreate(string name, string userFirstName, string userLastName, string userEmail,
            string languageCode)
        {
            string methodName = "Core.SiteCreate";
            await using var db = DbContextHelper.GetDbContext();
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(name), name);
                Log.LogVariable(methodName, nameof(userFirstName), userFirstName);
                Log.LogVariable(methodName, nameof(userLastName), userLastName);
                Log.LogVariable(methodName, nameof(userEmail), userEmail);

                Microting.eForm.Infrastructure.Data.Entities.EntityGroup selectableList = await db.EntityGroups
                        .FirstOrDefaultAsync(x =>
                            x.Name == "Device users" && x.Type == Constants.FieldTypes.EntitySelect) ??
                    await EntityGroupCreate(Constants.FieldTypes.EntitySelect, "Device users", "", true, false);
                selectableList.Locked = true;
                await selectableList.Update(db);

                Microting.eForm.Infrastructure.Data.Entities.EntityGroup searchableList = await db.EntityGroups
                        .FirstOrDefaultAsync(x =>
                            x.Name == "Device users" && x.Type == Constants.FieldTypes.EntitySearch) ??
                    await EntityGroupCreate(Constants.FieldTypes.EntitySearch, "Device users", "", true, false);
                searchableList.Locked = true;
                await searchableList.Update(db);

                Tuple<SiteDto, UnitDto> siteResult = await _communicator.SiteCreate(name, languageCode);

                //string token = await _sqlController.SettingRead(Settings.token).ConfigureAwait(false);
                int customerNo = int.Parse(_sqlController.SettingRead(Settings.customerNo).ConfigureAwait(false)
                    .GetAwaiter().GetResult());

                string siteName = siteResult.Item1.SiteName;
                int siteId = siteResult.Item1.SiteId;
                int unitUId = siteResult.Item2.UnitUId;
                int? otpCode = siteResult.Item2.OtpCode;
                Site site =
                    await db.Sites.FirstOrDefaultAsync(x => x.MicrotingUid == siteResult.Item1.SiteId)
                        .ConfigureAwait(false);
                if (site == null)
                {
                    Language language = await db.Languages.FirstOrDefaultAsync(x => x.LanguageCode == languageCode);
                    if (language == null)
                    {
                        if (languageCode == "da")
                        {
                            language = await db.Languages.FirstAsync(x => x.Name == "Danish");
                            language.LanguageCode = "da";
                            await language.Update(db);
                        }
                    }

                    site = new Site
                    {
                        MicrotingUid = siteId,
                        Name = siteName,
                        LanguageId = language.Id
                    };
                    await site.Create(db).ConfigureAwait(false);
                    var selectItem = await EntitySelectItemCreate(selectableList.Id, site.Name, 0, site.Id.ToString());
                    site.SelectableEntityItemId = selectItem.Id;
                    var searchItem = await EntitySearchItemCreate(searchableList.Id, site.Name, "", site.Id.ToString());
                    site.SearchableEntityItemId = searchItem.Id;
                    await site.Update(db);
                }

                SiteNameDto siteDto = await _sqlController.SiteRead(siteId).ConfigureAwait(false);
                Unit unit = await db.Units.FirstOrDefaultAsync(x => x.MicrotingUid == unitUId).ConfigureAwait(false);
                if (unit == null)
                {
                    unit = new Unit
                    {
                        MicrotingUid = unitUId,
                        CustomerNo = customerNo,
                        OtpCode = otpCode,
                        SiteId = site.Id
                    };

                    await unit.Create(db).ConfigureAwait(false);
                }

                string legacyEmail = siteId + "." + customerNo + "@invalid.invalid";

                WorkerDto workerDto = await Advanced_WorkerCreate(userFirstName, userLastName, userEmail, legacyEmail)
                    .ConfigureAwait(false);
                await Advanced_SiteWorkerCreate(siteDto, workerDto).ConfigureAwait(false);

                return await SiteRead(siteId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                return await _sqlController.SiteReadSimple(microtingUid).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<SiteDto>> SiteReadAll(bool includeRemoved)
        {
            if (!Running()) throw new Exception("Core is not running");
            if (includeRemoved)
                return await Advanced_SiteReadAll(null, null, null).ConfigureAwait(false);

            return await Advanced_SiteReadAll(Constants.WorkflowStates.NotRemoved, null, null).ConfigureAwait(false);
        }

        public async Task<SiteDto> SiteReset(int siteId)
        {
            string methodName = "Core.SiteReset";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(siteId), siteId);

                SiteDto site = await SiteRead(siteId).ConfigureAwait(false);
                await Advanced_UnitRequestOtp((int)site.UnitId).ConfigureAwait(false);

                return await SiteRead(siteId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
        public async Task<bool> SiteUpdate(int siteMicrotingUid, string siteName, string userFirstName,
            string userLastName, string userEmail, string languageCode)
        {
            string methodName = "Core.SiteUpdate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                SiteDto siteDto = await SiteRead(siteMicrotingUid).ConfigureAwait(false);
                await Advanced_SiteItemUpdate(siteMicrotingUid, siteName, languageCode).ConfigureAwait(false);
                if (String.IsNullOrEmpty(userEmail))
                {
                    //if (String.IsNullOrEmpty)
                }

                await Advanced_WorkerUpdate((int)siteDto.WorkerUid, userFirstName, userLastName, userEmail,
                    siteDto.Email).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                SiteDto siteDto = await SiteRead(microtingUid).ConfigureAwait(false);

                if (siteDto == null) return false;
                await Advanced_SiteItemDelete(microtingUid).ConfigureAwait(false);
                SiteWorkerDto siteWorkerDto = await Advanced_SiteWorkerRead(null, microtingUid, siteDto.WorkerUid)
                    .ConfigureAwait(false);
                await Advanced_SiteWorkerDelete(siteWorkerDto.MicrotingUId).ConfigureAwait(false);
                await Advanced_WorkerDelete((int)siteDto.WorkerUid).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        //

        // entity

        /// <summary>
        /// Creates an EntityGroup, and returns its unique microting id for further use
        /// </summary>
        /// <param name="entityType">Entity type, either "EntitySearch" or "EntitySelect"</param>
        /// <param name="name">Templat MainElement's ID to be retrieved from the Microting local DB</param>
        /// <param name="description"></param>
        public async Task<Microting.eForm.Infrastructure.Data.Entities.EntityGroup> EntityGroupCreate(string entityType,
            string name, string description, bool locked, bool editable)
        {
            string methodName = "Core.EntityGroupCreate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using var dbContext = DbContextHelper.GetDbContext();
                Microting.eForm.Infrastructure.Data.Entities.EntityGroup entityGroup =
                    new Microting.eForm.Infrastructure.Data.Entities.EntityGroup
                    {
                        Name = name,
                        Type = entityType,
                        Description = description,
                        Locked = locked,
                        Editable = editable
                    };

                await entityGroup.Create(dbContext).ConfigureAwait(false);

                string entityGroupMuId = await _communicator
                    .EntityGroupCreate(entityType, name, entityGroup.Id.ToString()).ConfigureAwait(false);

                if (string.IsNullOrEmpty(entityGroupMuId))
                {
                    await _sqlController.EntityGroupDelete(entityGroupMuId).ConfigureAwait(false);
                    throw new Exception("EntityListCreate failed, due to list not created correct");
                }

                entityGroup.MicrotingUid = entityGroupMuId;

                await entityGroup.Update(dbContext).ConfigureAwait(false);

                return entityGroup;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "EntityListCreate failed", ex);
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
            return await EntityGroupRead(entityGroupMuId, Constants.EntityItemSortParameters.DisplayIndex, "")
                .ConfigureAwait(false);
        }

        public async Task<EntityGroup> EntityGroupRead(string entityGroupMuId, string sort, string nameFilter)
        {
            string methodName = "Core.EntityGroupRead";
            if (string.IsNullOrEmpty(entityGroupMuId))
                throw new ArgumentNullException(nameof(entityGroupMuId));
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                while (_updateIsRunningEntities)
                    Thread.Sleep(200);

                return await _sqlController.EntityGroupReadSorted(entityGroupMuId, sort, nameFilter)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                try
                {
                    Log.LogException(methodName,
                        "(string entityGroupMUId " + entityGroupMuId + ", string sort " + sort +
                        ", string nameFilter " + nameFilter + ") failed", ex);
                }
                catch
                {
                    Log.LogException(methodName, "(string entityGroupMUId, string sort, string nameFilter) failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                bool isUpdated = await _communicator
                    .EntityGroupUpdate(entityGroup.Type, entityGroup.Name, entityGroup.Id, entityGroup.MicrotingUUID)
                    .ConfigureAwait(false);

                if (!isUpdated) throw new Exception("Update failed");
                await using var dbContext = DbContextHelper.GetDbContext();
                var eg = await dbContext.EntityGroups.FirstOrDefaultAsync(x => x.Id == entityGroup.Id);
                eg.Name = entityGroup.Name;
                eg.Description = entityGroup.Description;
                await eg.Update(dbContext);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "EntityGroupRead failed", ex);
                throw new Exception("EntityGroupRead failed", ex);
            }
        }

        /// <summary>
        /// Deletes an EntityGroup, both its items should be deleted before using
        /// </summary>
        /// <param name="entityGroupMuId">The unique microting id of the EntityGroup</param>
        public async Task<bool> EntityGroupDelete(string entityGroupMuId)
        {
            string methodName = "Core.EntityGroupDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                while (_updateIsRunningEntities)
                    Thread.Sleep(200);

                EntityGroup entityGroup = await _sqlController.EntityGroupRead(entityGroupMuId).ConfigureAwait(false);
                await _communicator.EntityGroupDelete(entityGroup.Type, entityGroupMuId).ConfigureAwait(false);
                string type = await _sqlController.EntityGroupDelete(entityGroupMuId).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "EntityGroupDelete failed", ex);
                throw new Exception("EntityGroupDelete failed", ex);
            }
        }

        // EntityItem

        public Task<EntityItem> EntitySearchItemCreate(int entitItemGroupId, string name, string description,
            string ownUuid)
        {
            return EntityItemCreate(entitItemGroupId, name, description, ownUuid, 0);
        }

        public Task<EntityItem> EntitySelectItemCreate(int entitItemGroupId, string name, int displayIndex,
            string ownUuid)
        {
            return EntityItemCreate(entitItemGroupId, name, "", ownUuid, displayIndex);
        }

        private async Task<EntityItem> EntityItemCreate(int entitItemGroupId, string name, string description,
            string ownUuid, int displayIndex)
        {
            EntityGroup eg = await _sqlController.EntityGroupRead(entitItemGroupId).ConfigureAwait(false);
            EntityItem et = await _sqlController.EntityItemRead(entitItemGroupId, name, description)
                .ConfigureAwait(false);
            if (et == null)
            {
                string microtingUId;
                if (eg.Type == Constants.FieldTypes.EntitySearch)
                {
                    microtingUId = await _communicator
                        .EntitySearchItemCreate(eg.MicrotingUUID, name, description, ownUuid).ConfigureAwait(false);
                }
                else
                {
                    microtingUId = await _communicator
                        .EntitySelectItemCreate(eg.MicrotingUUID, name, displayIndex, ownUuid).ConfigureAwait(false);
                }

                if (microtingUId != null)
                {
                    et = new EntityItem
                    {
                        Name = name,
                        Description = description,
                        EntityItemUId = ownUuid,
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
                if (eg.Type == Constants.FieldTypes.EntitySearch)
                {
                    await _communicator
                        .EntitySearchItemUpdate(eg.MicrotingUUID, et.MicrotingUUID, name, description, ownUuid)
                        .ConfigureAwait(false);
                }
                else
                {
                    await _communicator
                        .EntitySelectItemUpdate(eg.MicrotingUUID, et.MicrotingUUID, name, displayIndex, ownUuid)
                        .ConfigureAwait(false);
                }

                et.WorkflowState = Constants.WorkflowStates.Created;
                et.DisplayIndex = displayIndex;
                et.EntityItemUId = ownUuid;
                await _sqlController.EntityItemUpdate(et).ConfigureAwait(false);
            }

            return et;
        }

        public async Task EntityItemUpdate(int id, string name, string description, string ownUuid, int displayIndex)
        {
            await using var dbContext = DbContextHelper.GetDbContext();
            Microting.eForm.Infrastructure.Data.Entities.EntityItem et =
                await dbContext.EntityItems.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                throw new NullReferenceException("EntityItem not found with id " + id);
            }

            if (et.Name != name || et.Description != description || et.DisplayIndex != displayIndex ||
                et.EntityItemUid != ownUuid)
            {
                Microting.eForm.Infrastructure.Data.Entities.EntityGroup eg =
                    await dbContext.EntityGroups.FirstOrDefaultAsync(x =>
                        x.Id == et.EntityGroupId);
                bool result = false;
                if (eg.Type == Constants.FieldTypes.EntitySearch)
                {
                    result = await _communicator
                        .EntitySearchItemUpdate(eg.MicrotingUid, et.MicrotingUid,
                            name, description, ownUuid)
                        .ConfigureAwait(false);
                }
                else
                {
                    result = await _communicator
                        .EntitySelectItemUpdate(eg.MicrotingUid, et.MicrotingUid,
                            name, displayIndex, ownUuid)
                        .ConfigureAwait(false);
                }

                if (result)
                {
                    et.DisplayIndex = displayIndex;
                    et.Name = name;
                    et.Description = description;
                    et.EntityItemUid = ownUuid;
                    await et.Update(dbContext);
                }
                else
                {
                    throw new Exception("Unable to update entityItem with id " + id);
                }
            }
        }

        public async Task EntityItemDelete(int id)
        {
            EntityItem et = await _sqlController.EntityItemRead(id);
            if (et == null)
            {
                throw new NullReferenceException("EntityItem not found with id " + id);
            }

            EntityGroup eg = await _sqlController.EntityGroupRead(et.EntityItemGroupId).ConfigureAwait(false);
            bool result = false;
            if (eg.Type == Constants.FieldTypes.EntitySearch)
            {
                result = await _communicator.EntitySearchItemDelete(et.MicrotingUUID).ConfigureAwait(false);
            }
            else
            {
                result = await _communicator.EntitySelectItemDelete(et.MicrotingUUID).ConfigureAwait(false);
            }

            if (result)
            {
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                var entityItem = await dbContext.EntityItems.FirstOrDefaultAsync(x => x.Id == id);
                await entityItem.Delete(dbContext);
            }
            else
            {
                throw new Exception("Unable to update entityItem with id " + id);
            }
        }

        //

        public async Task<string> PdfUpload(string localPath)
        {
            string methodName = "Core.PdfUpload";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                string checkSum = "";
                using (var md5 = MD5.Create())
                {
                    await using (var stream = File.OpenRead(localPath))
                    {
                        byte[] grr = await md5.ComputeHashAsync(stream);
                        checkSum = BitConverter.ToString(grr).Replace("-", "").ToLower();
                    }
                }

                if (await _communicator.PdfUpload(localPath, checkSum).ConfigureAwait(false))
                    return checkSum;
                Log.LogWarning(methodName, "Uploading of PDF failed");
                return null;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public async Task PdfUpload(Stream stream, string hash, string fileName)
        {
            await _communicator.PdfUpload(stream, hash, fileName).ConfigureAwait(false);
        }
        //

        // folder

        public async Task<List<FolderDto>> FolderGetAll(bool includeRemoved)
        {
            string methodName = "Core.FolderGetAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                List<FolderDto> folderDtos = await _sqlController.FolderGetAll(includeRemoved).ConfigureAwait(false);

                return folderDtos;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "FolderGetAll failed", ex);
                throw new Exception("FolderGetAll failed", ex);
            }
        }

        public async Task<FolderDto> FolderRead(int id)
        {
            string methodName = "Core.FolderRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                FolderDto folderDto = await _sqlController.FolderRead(id).ConfigureAwait(false);

                return folderDto;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "FolderRead failed", ex);
                throw new Exception("FolderRead failed", ex);
            }
        }

        public async Task<int> FolderCreate(List<CommonTranslationsModel> translations, int? parentId)
        {
            string methodName = "Core.FolderCreate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                int apiParentId = 0;
                if (parentId != null)
                {
                    apiParentId = (int)dbContext.Folders.Single(x => x.Id == parentId).MicrotingUid;
                    //    apiParentId = (int)FolderRead((int) parentId).GetAwaiter().GetResult().MicrotingUId;
                }

                Folder folder = new Folder
                {
                    ParentId = parentId
                };
                await folder.Create(dbContext);
                int result = await _communicator.FolderCreate(folder.Id, apiParentId).ConfigureAwait(false);
                folder.MicrotingUid = result;
                await folder.Update(dbContext);
                foreach (CommonTranslationsModel translation in translations)
                {
                    Language language =
                        await dbContext.Languages.FirstOrDefaultAsync(x => x.Id == translation.LanguageId);
                    await _communicator.FolderUpdate((int)folder.MicrotingUid, translation.Name,
                        translation.Description,
                        language.LanguageCode, apiParentId);
                    FolderTranslation folderTranslation = new FolderTranslation
                    {
                        FolderId = folder.Id,
                        Name = translation.Name,
                        Description = translation.Description,
                        LanguageId = language.Id
                    };
                    await folderTranslation.Create(dbContext);
                }

                return folder.Id;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "FolderCreate failed", ex);
                throw new Exception("FolderCreate failed", ex);
            }
        }

        public async Task<int> FolderCreate(List<KeyValuePair<string, string>> name,
            List<KeyValuePair<string, string>> description, int? parentId)
        {
            string methodName = "Core.FolderCreate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                int apiParentId = 0;
                if (parentId != null)
                {
                    apiParentId = (int)dbContext.Folders.Single(x => x.Id == parentId).MicrotingUid;
                    //    apiParentId = (int)FolderRead((int) parentId).GetAwaiter().GetResult().MicrotingUId;
                }

                Folder folder = new Folder
                {
                    ParentId = parentId
                };
                await folder.Create(dbContext);
                int result = await _communicator.FolderCreate(folder.Id, apiParentId).ConfigureAwait(false);
                folder.MicrotingUid = result;
                await folder.Update(dbContext);
                for (int i = 0; i < name.Count; i++)
                {
                    await _communicator.FolderUpdate((int)folder.MicrotingUid, name[i].Value, description[i].Value,
                        name[i].Key, apiParentId);
                    Language language =
                        await dbContext.Languages.FirstOrDefaultAsync(x => x.LanguageCode == name[i].Key);
                    FolderTranslation folderTranslation = new FolderTranslation
                    {
                        FolderId = folder.Id,
                        Name = name[i].Value,
                        Description = description[i].Value,
                        LanguageId = language.Id
                    };
                    await folderTranslation.Create(dbContext);
                }

                return folder.Id;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "FolderCreate failed", ex);
                throw new Exception("FolderCreate failed", ex);
            }
        }

        public async Task FolderUpdate(int id, List<CommonTranslationsModel> translations, int? parentId)
        {
            string methodName = "Core.FolderUpdate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                Folder folder = await dbContext.Folders.FirstAsync(x => x.Id == id);
                //FolderDto folder = await FolderRead(id).ConfigureAwait(false);
                int apiParentId = 0;
                if (parentId != null)
                {
                    apiParentId = (int)dbContext.Folders.First(x => x.Id == parentId).MicrotingUid!;
                }

                foreach (CommonTranslationsModel translation in translations)
                {
                    Language language = await dbContext.Languages.FirstAsync(x => x.Id == translation.LanguageId);
                    translation.Description ??= "";
                    await _communicator.FolderUpdate((int)folder.MicrotingUid!, translation.Name,
                        translation.Description,
                        language.LanguageCode, apiParentId);
                    FolderTranslation folderTranslation =
                        await dbContext.FolderTranslations.FirstOrDefaultAsync(x =>
                            x.FolderId == folder.Id && x.LanguageId == language.Id);
                    if (folderTranslation == null)
                    {
                        folderTranslation = new FolderTranslation
                        {
                            FolderId = folder.Id,
                            Name = translation.Name,
                            Description = translation.Description,
                            LanguageId = language.Id
                        };
                        await folderTranslation.Create(dbContext);
                    }
                    else
                    {
                        folderTranslation.Name = translation.Name;
                        folderTranslation.Description = translation.Description;
                        await folderTranslation.Update(dbContext);
                    }
                }

                if (folder.ParentId != parentId && parentId != null)
                {
                    folder.ParentId = parentId;
                    await folder.Update(dbContext);
                }
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "FolderUpdate failed", ex);
                throw new Exception("FolderUpdate failed", ex);
            }
        }

        public async Task FolderUpdate(int id, List<KeyValuePair<string, string>> name,
            List<KeyValuePair<string, string>> description, int? parentId)
        {
            string methodName = "Core.FolderUpdate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                Folder folder = await dbContext.Folders.FirstOrDefaultAsync(x => x.Id == id);
                //FolderDto folder = await FolderRead(id).ConfigureAwait(false);
                int apiParentId = 0;
                if (parentId != null)
                {
                    apiParentId = (int)dbContext.Folders.Single(x => x.Id == parentId).MicrotingUid;
                }

                for (int i = 0; i < name.Count; i++)
                {
                    await _communicator.FolderUpdate((int)folder.MicrotingUid, name[i].Value, description[i].Value,
                        name[i].Key, apiParentId);
                    Language language =
                        await dbContext.Languages.FirstOrDefaultAsync(x => x.LanguageCode == name[i].Key);
                    FolderTranslation folderTranslation =
                        await dbContext.FolderTranslations.FirstOrDefaultAsync(x =>
                            x.FolderId == folder.Id && x.LanguageId == language.Id);
                    if (folderTranslation == null)
                    {
                        folderTranslation = new FolderTranslation
                        {
                            FolderId = folder.Id,
                            Name = name[i].Value,
                            Description = description[i].Value,
                            LanguageId = language.Id
                        };
                        await folderTranslation.Create(dbContext);
                    }
                    else
                    {
                        folderTranslation.Name = name[i].Value;
                        folderTranslation.Description = description[i].Value;
                        await folderTranslation.Update(dbContext);
                    }
                }

                if (folder.ParentId != parentId && parentId != null)
                {
                    folder.ParentId = parentId;
                    await folder.Update(dbContext);
                }
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "FolderUpdate failed", ex);
                throw new Exception("FolderUpdate failed", ex);
            }
        }

        public async Task FolderDelete(int id)
        {
            string methodName = "Core.FolderDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                Folder folder = await dbContext.Folders.FirstOrDefaultAsync(x => x.Id == id);
                //FolderDto folder = await FolderRead(id).ConfigureAwait(false);
                bool success = await _communicator.FolderDelete((int)folder.MicrotingUid).ConfigureAwait(false);
                if (success)
                {
                    await folder.Delete(dbContext);
                }
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "FolderDelete failed", ex);
                throw new Exception("FolderDelete failed", ex);
            }
        }
        //

        //

        // tags
        public async Task<List<Tag>> GetAllTags(bool includeRemoved)
        {
            string methodName = "Core.GetAllTags";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                return await _sqlController.GetAllTags(includeRemoved).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name is not allowed to be null or empty");
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                return await _sqlController.TagCreate(name).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> TagDelete(int tagId)
        {
            string methodName = "Core.TagDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                return await _sqlController.TagDelete(tagId);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        //


        // speach to text

        public async Task<bool> TranscribeUploadedData(int uploadedDataId)
        {
            string methodName = "Core.TranscribeUploadedData";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Microting.eForm.Infrastructure.Data.Entities.UploadedData uploadedData =
                    await _sqlController.GetUploadedData(uploadedDataId);
                if (uploadedData == null) return false;
                string[] audioFileExtenstions =
                {
                    ".3gp", ".aa", ".aac", ".aax", ".act", ".aiff", ".amr", ".ape", ".au", ".awb", ".dct", ".dss",
                    ".dvf", ".flac", ".gsm", ".iklax", ".ivs", ".m4a", ".m4b", ".m4p", ".mmf", ".mp3", ".mpc", ".msv",
                    ".nsf", ".ogg", ".oga", ".mogg", ".opus", ".ra", ".rm", ".raw", ".sln", ".tta", ".vox", ".wav",
                    ".wma", ".wv", ".webm", ".8svx"
                };
                if (!audioFileExtenstions.Any(uploadedData.Extension.Contains)) return false;
                string filePath = Path.Combine(uploadedData.FileLocation, uploadedData.FileName);
                Log.LogStandard(methodName, $"filePath is {filePath}");
                string fileName =
                    $"{uploadedData.Id}_{uploadedData.Checksum}{uploadedData.Extension}";

                var stream = await GetFileFromS3Storage(fileName);

                int requestId = await SpeechToText(stream.ResponseStream, uploadedData.Extension).ConfigureAwait(false);
                uploadedData.TranscriptionId = requestId;

                await _sqlController.UpdateUploadedData(uploadedData).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<int> SpeechToText(Stream pathToAudioFile, string extension)
        {
            string methodName = "Core.SpeechToText";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                return await _communicator.SpeechToText(pathToAudioFile, extension).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        //

        // InSight

        // SurveyConfiguration

        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            await using var dbContext = DbContextHelper.GetDbContext();
            SiteSurveyConfiguration siteSurveyConfiguration =
                await dbContext.SiteSurveyConfigurations
                    .FirstOrDefaultAsync(x => x.SiteId == siteId && x.SurveyConfigurationId == id)
                    .ConfigureAwait(false);

            if (siteSurveyConfiguration == null)
            {
            }

            return true;
        }

        public async Task<bool> GetAllSurveyConfigurations()
        {
            var parsedData = JObject.Parse(await _communicator.GetAllSurveyConfigurations().ConfigureAwait(false));

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            foreach (var item in parsedData)
            {
                foreach (JToken subItem in item.Value)
                {
                    await using var db = DbContextHelper.GetDbContext();
                    string name = subItem["Name"].ToString();
                    int microtingUid = int.Parse(subItem["MicrotingUid"].ToString());
                    var innerParsedData =
                        JObject.Parse(await _communicator.GetSurveyConfiguration(microtingUid).ConfigureAwait(false));
                    JToken parsedQuestionSet = innerParsedData.GetValue("QuestionSet");

                    if (parsedQuestionSet.ToString() == "{}") continue;
                    int questionSetMicrotingUid = int.Parse(parsedQuestionSet["MicrotingUid"].ToString());
                    if (questionSetMicrotingUid == 0) continue;
                    var questionSet = await db.QuestionSets
                        .FirstOrDefaultAsync(x => x.MicrotingUid == questionSetMicrotingUid).ConfigureAwait(false);
                    if (questionSet != null)
                    {
                        questionSet.Name = parsedQuestionSet["Name"].ToString();
                        await questionSet.Update(db);
                    }
                    else
                    {
                        questionSet = new QuestionSet
                        {
                            Name = parsedQuestionSet["Name"].ToString(),
                            MicrotingUid = questionSetMicrotingUid
                        };
                        await questionSet.Create(db);
                    }

                    var surveyConfiguration = JsonConvert.DeserializeObject<SurveyConfiguration>(subItem.ToString());
                    bool removed = surveyConfiguration.WorkflowState == Constants.WorkflowStates.Removed;
                    if (!await db.SurveyConfigurations.AnyAsync(x =>
                            x.MicrotingUid == surveyConfiguration.MicrotingUid))
                    {
                        surveyConfiguration.QuestionSetId = questionSet.Id;
                        await surveyConfiguration.Create(db);
                    }
                    else
                    {
                        surveyConfiguration = await db.SurveyConfigurations.FirstAsync(x =>
                            x.MicrotingUid == surveyConfiguration.MicrotingUid);
                        surveyConfiguration.Name = subItem["Name"].ToString();
                        await surveyConfiguration.Update(db);
                    }

                    if (removed)
                    {
                        await surveyConfiguration.Delete(db);
                    }

                    foreach (JToken child in innerParsedData.GetValue("Sites").Children())
                    {
                        var site = await db.Sites
                            .FirstOrDefaultAsync(x => x.MicrotingUid == int.Parse(child["MicrotingUid"].ToString()))
                            .ConfigureAwait(false);
                        if (site == null) continue;
                        {
                            var siteSurveyConfiguration =
                                await db.SiteSurveyConfigurations.FirstOrDefaultAsync(x =>
                                        x.SiteId == site.Id && x.SurveyConfigurationId == surveyConfiguration.Id)
                                    .ConfigureAwait(false);
                            if (siteSurveyConfiguration == null)
                            {
                                siteSurveyConfiguration = new SiteSurveyConfiguration
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

            return true;
        }
        //

        // QuestionSet

        public async Task<bool> GetQuestionSet(int microtingUid, int questionSetId, int threadNumber)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            await using var db = DbContextHelper.GetDbContext();
            var innerParsedData = JObject.Parse(await _communicator.GetQuestionSet(microtingUid).ConfigureAwait(false));

            JToken parsedQuestions = innerParsedData.GetValue("Questions");
            bool removed = false;
            if (parsedQuestions != null)
                foreach (JToken child in parsedQuestions.Children())
                {
                    var question = JsonConvert.DeserializeObject<Question>(child.ToString(), jsonSerializerSettings);
                    removed = question.WorkflowState == Constants.WorkflowStates.Removed;
                    Log.LogStandard("Core.GetAllQuestionSets",
                        $"Parsing question on thread {threadNumber} {question.MicrotingUid}");
                    if (!await db.Questions.AnyAsync(x => x.MicrotingUid == question.MicrotingUid)
                            .ConfigureAwait(false))
                    {
                        question.QuestionSetId = questionSetId;
                        await question.Create(db).ConfigureAwait(false);
                    }
                    else
                    {
                        question = await db.Questions.FirstAsync(x => x.MicrotingUid == question.MicrotingUid);
                        question.QuestionIndex = int.Parse(child["QuestionIndex"].ToString());
                        question.BackButtonEnabled = child["BackButtonEnabled"].ToString() == "true";
                        question.Image = child["Image"].ToString() == "true";
                        question.ImagePosition = child["ImagePosition"].ToString();
                        question.MaxDuration = int.Parse(child["MaxDuration"].ToString());
                        var bla = child["Maximum"].ToString();
                        question.Maximum = string.IsNullOrEmpty(child["Maximum"].ToString())
                            ? 0
                            : int.Parse(child["Maximum"].ToString());
                        question.ValidDisplay = child["ValidDisplay"].ToString() == "true";
                        await question.Update(db).ConfigureAwait(false);
                    }

                    if (removed)
                    {
                        await question.Delete(db);
                    }
                }

            JToken parsedQuestionTranslations = innerParsedData.GetValue("QuestionTranslations");
            foreach (JToken child in parsedQuestionTranslations.Children())
            {
                var questionTranslation =
                    JsonConvert.DeserializeObject<QuestionTranslation>(child.ToString(), jsonSerializerSettings);
                Log.LogStandard("Core.GetAllQuestionSets",
                    $"Parsing question translation on thread {threadNumber} {questionTranslation.Name}");
                removed = questionTranslation.WorkflowState == Constants.WorkflowStates.Removed;
                if (!await db.QuestionTranslations.AnyAsync(x =>
                        x.MicrotingUid == questionTranslation.MicrotingUid).ConfigureAwait(false))
                {
                    questionTranslation.QuestionId = db.Questions
                        .Single(x => x.MicrotingUid == questionTranslation.QuestionId).Id;
                    await questionTranslation.Create(db).ConfigureAwait(false);
                }
                else
                {
                    questionTranslation = await db.QuestionTranslations.FirstAsync(x =>
                        x.MicrotingUid == questionTranslation.MicrotingUid).ConfigureAwait(false);
                    questionTranslation.Name = child["Name"].ToString();
                    await questionTranslation.Update(db).ConfigureAwait(false);
                }

                if (removed)
                {
                    await questionTranslation.Delete(db).ConfigureAwait(false);
                }
            }

            JToken parsedOptions = innerParsedData.GetValue("Options");
            //int i = 0;
            foreach (JToken child in parsedOptions.Children())
            {
                var option = JsonConvert.DeserializeObject<Option>(child.ToString(), jsonSerializerSettings);
                Log.LogStandard("Core.GetAllQuestionSets",
                    $"Parsing option on thread {threadNumber} {option.MicrotingUid}");
                removed = option.WorkflowState == Constants.WorkflowStates.Removed;
                if (!await db.Options.AnyAsync(x => x.MicrotingUid == option.MicrotingUid).ConfigureAwait(false))
                {
                    int? nextQuestionId = null;
                    if (!string.IsNullOrEmpty(option.NextQuestionId.ToString()))
                    {
                        nextQuestionId = db.Questions.FirstOrDefault(x =>
                            x.MicrotingUid == int.Parse(option.NextQuestionId.ToString()))?.Id;
                    }

                    option.WeightValue = int.Parse(child["WeightedValue"].ToString());
                    option.Weight = int.Parse(child["Weight"].ToString());
                    option.OptionIndex = int.Parse(child["OptionIndex"].ToString());
                    option.NextQuestionId = nextQuestionId;
                    option.QuestionId = db.Questions.Single(x => x.MicrotingUid == option.QuestionId).Id;
                    await option.Create(db).ConfigureAwait(false);
                }
                else
                {
                    try
                    {
                        option = await db.Options.FirstAsync(x => x.MicrotingUid == option.MicrotingUid)
                            .ConfigureAwait(false);
                        option.WeightValue = int.Parse(child["WeightedValue"].ToString());
                        option.Weight = int.Parse(child["Weight"].ToString());
                        option.OptionIndex = int.Parse(child["OptionIndex"].ToString());
                        int? nextQuestionId = null;
                        if (!string.IsNullOrEmpty(child["NextQuestionId"].ToString()))
                        {
                            nextQuestionId = db.Questions.FirstOrDefault(x =>
                                x.MicrotingUid == int.Parse(child["NextQuestionId"].ToString()))?.Id;
                        }

                        option.NextQuestionId = nextQuestionId;
                        await option.Update(db).ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }

                if (removed)
                {
                    await option.Delete(db).ConfigureAwait(false);
                }
            }

            JToken parsedOptionTranslations = innerParsedData.GetValue("OptionTranslations");
            foreach (JToken child in parsedOptionTranslations.Children())
            {
                var optionTranslation =
                    JsonConvert.DeserializeObject<OptionTranslation>(child.ToString(), jsonSerializerSettings);
                Log.LogStandard("Core.GetAllQuestionSets",
                    $"Parsing option translation on thread {threadNumber} {optionTranslation.Name}");
                removed = optionTranslation.WorkflowState == Constants.WorkflowStates.Removed;
                if (!await db.OptionTranslations.AnyAsync(x =>
                        x.MicrotingUid == optionTranslation.MicrotingUid).ConfigureAwait(false))
                {
                    optionTranslation.OptionId =
                        db.Options.Single(x => x.MicrotingUid == optionTranslation.OptionId).Id;
                    await optionTranslation.Create(db).ConfigureAwait(false);
                }
                else
                {
                    optionTranslation = await db.OptionTranslations.FirstAsync(x =>
                        x.MicrotingUid == optionTranslation.MicrotingUid).ConfigureAwait(false);
                    optionTranslation.Name = child["Name"].ToString();
                    await optionTranslation.Update(db).ConfigureAwait(false);
                }

                if (removed)
                {
                    await optionTranslation.Delete(db).ConfigureAwait(false);
                }
            }

            return true;
        }

        public async Task<bool> GetAllQuestionSets()
        {
            var parsedData = JObject.Parse(await _communicator.GetAllQuestionSets().ConfigureAwait(false));

            if (!parsedData.HasValues)
                return false;
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            await using var db = DbContextHelper.GetDbContext();
            var language = await db.Languages.FirstOrDefaultAsync(x => x.Name == "Danish").ConfigureAwait(false);
            if (language == null)
            {
                language = new Language
                {
                    Name = "Danish"
                };
                await language.Create(db);
            }

            foreach (var item in parsedData)
            {
                Task[] tasks = new Task[item.Value.Count()];
                int i = 0;
                foreach (JToken subItem in item.Value)
                {
                    var questionSet =
                        JsonConvert.DeserializeObject<QuestionSet>(subItem.ToString(), jsonSerializerSettings);

                    bool removed = questionSet.WorkflowState == Constants.WorkflowStates.Removed;
                    if (!await db.QuestionSets.AnyAsync(x => x.MicrotingUid == questionSet.MicrotingUid)
                            .ConfigureAwait(false))
                    {
                        await questionSet.Create(db).ConfigureAwait(false);
                    }
                    else
                    {
                        questionSet =
                            await db.QuestionSets.FirstAsync(x => x.MicrotingUid == questionSet.MicrotingUid)
                                .ConfigureAwait(false);
                        questionSet.Name = subItem["Name"].ToString();
                        await questionSet.Update(db).ConfigureAwait(false);
                    }

                    if (removed)
                    {
                        await questionSet.Delete(db).ConfigureAwait(false);
                    }

                    tasks[i] = GetQuestionSet((int)questionSet.MicrotingUid, questionSet.Id, i);
                    i += 1;
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            return true;
        }

        //

        // Answer

        public async Task<bool> GetAllAnswers()
        {
            await using var db = DbContextHelper.GetDbContext();
            foreach (QuestionSet questionSet in await db.QuestionSets.ToListAsync())
            {
                await GetAnswersForQuestionSet(questionSet.MicrotingUid).ConfigureAwait(false);
            }

            return true;
        }

        private async Task<int> SaveAnswers(QuestionSet questionSet, JObject parsedData)
        {
            Task[] tasks = new Task[int.Parse(parsedData["NumAnswers"].ToString())];

            int i = 0;
            foreach (JToken item in parsedData["Answers"])
            {
                tasks[i] = SaveAnswer(item, questionSet.Id);
                i += 1;
            }

            await Task.WhenAll(tasks);

            return int.Parse(parsedData["NumAnswers"].ToString());
        }

        private async Task SaveAnswer(JToken subItem, int questionSetId)
        {
            Log.LogStandard("Core.SaveAnswer", $"called {DateTime.UtcNow}");
            var settings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
            await using (var db = DbContextHelper.GetDbContext())
            {
                Answer answer = JsonConvert.DeserializeObject<Answer>(subItem.ToString(), settings);
                if (answer == null)
                {
                    Console.WriteLine("fdssd");
                }

                Answer result = null;
                try
                {
                    result = await db.Answers.FirstOrDefaultAsync(x => x.MicrotingUid == answer.MicrotingUid
                                                                       && x.WorkflowState !=
                                                                       Constants.WorkflowStates.Removed)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                // if (result != null)
                // {
                //     answer.Id = result.Id;
                // }

                if (result == null)
                {
                    Unit unit = await db.Units.FirstOrDefaultAsync(x => x.MicrotingUid == answer.UnitId)
                        .ConfigureAwait(false);
                    if (unit != null)
                    {
                        answer.UnitId = unit.Id;
                    }
                    else
                    {
                        answer.UnitId = null;
                    }

                    try
                    {
                        answer.SiteId = db.Sites.Single(x => x.MicrotingUid == answer.SiteId).Id;
                        answer.QuestionSetId = questionSetId;
                        SurveyConfiguration surveyConfiguration = await db.SurveyConfigurations
                            .FirstOrDefaultAsync(x => x.MicrotingUid == answer.SurveyConfigurationId)
                            .ConfigureAwait(false);

                        if (surveyConfiguration != null)
                        {
                            answer.SurveyConfigurationId = surveyConfiguration.Id;
                        }
                        else
                        {
                            answer.SurveyConfigurationId = null;
                        }

                        answer.QuestionSet = null;
                        answer.LanguageId = db.Languages.Single(x => x.Name == "Danish").Id;
                        await answer.Create(db).ConfigureAwait(false);

                        foreach (JToken avItem in subItem["AnswerValues"])
                        {
                            // log.LogStandard("Core.SaveAnswer", $"AnswerValues parsing started {DateTime.UtcNow}");
                            AnswerValue answerValue =
                                JsonConvert.DeserializeObject<AnswerValue>(avItem.ToString(), settings);
                            if (db.AnswerValues.FirstOrDefault(x => x.MicrotingUid == answerValue.MicrotingUid) ==
                                null)
                            {
                                var question = await db.Questions
                                    .FirstAsync(x => x.MicrotingUid == answerValue.QuestionId).ConfigureAwait(false);
                                var option = await db.Options.FirstAsync(x => x.MicrotingUid == answerValue.OptionId)
                                    .ConfigureAwait(false);
                                if (question.QuestionType == Constants.QuestionTypes.Buttons ||
                                    question.QuestionType == Constants.QuestionTypes.List ||
                                    question.QuestionType == Constants.QuestionTypes.Multi)
                                {
                                    OptionTranslation optionTranslation =
                                        await db.OptionTranslations.FirstAsync(x => x.OptionId == option.Id)
                                            .ConfigureAwait(false);
                                    answerValue.Value = optionTranslation.Name;
                                }

                                answerValue.AnswerId = answer.Id;
                                answerValue.QuestionId =
                                    question.Id;
                                answerValue.OptionId =
                                    option.Id;
                                await answerValue.Create(db).ConfigureAwait(false);

                                // log.LogStandard("Core.SaveAnswer", $"AnswerValues parsing done {DateTime.UtcNow}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    answer = result;
                    answer.WorkflowState = Constants.WorkflowStates.Created;
                    await answer.Update(db).ConfigureAwait(false);
                    foreach (JToken avItem in subItem["AnswerValues"])
                    {
                        // log.LogStandard("Core.SaveAnswer", $"AnswerValues parsing started {DateTime.UtcNow}");
                        AnswerValue answerValue =
                            JsonConvert.DeserializeObject<AnswerValue>(avItem.ToString(), settings);
                        if (db.AnswerValues.FirstOrDefault(x => x.MicrotingUid == answerValue.MicrotingUid) ==
                            null)
                        {
                            var question = await db.Questions.FirstAsync(x => x.MicrotingUid == answerValue.QuestionId)
                                .ConfigureAwait(false);
                            var option = await db.Options
                                .FirstOrDefaultAsync(x => x.MicrotingUid == answerValue.OptionId).ConfigureAwait(false);
                            if (option != null)
                            {
                                if (question.QuestionType == Constants.QuestionTypes.Buttons ||
                                    question.QuestionType == Constants.QuestionTypes.List ||
                                    question.QuestionType == Constants.QuestionTypes.Multi)
                                {
                                    OptionTranslation optionTranslation =
                                        await db.OptionTranslations.FirstAsync(x => x.OptionId == option.Id)
                                            .ConfigureAwait(false);
                                    answerValue.Value = optionTranslation.Name;
                                }

                                answerValue.AnswerId = answer.Id;
                                answerValue.QuestionId =
                                    question.Id;
                                answerValue.OptionId =
                                    option.Id;
                                await answerValue.Create(db).ConfigureAwait(false);
                            }

                            // log.LogStandard("Core.SaveAnswer", $"AnswerValues parsing done {DateTime.UtcNow}");
                        }
                    }
                }
            }

            Log.LogStandard("Core.SaveAnswer", $"ended {DateTime.UtcNow}");
        }

        public async Task GetAnswersForQuestionSet(int? apiQuestionSetId)
        {
            if (apiQuestionSetId == null)
                return;

            await using var db = DbContextHelper.GetDbContext();
            int numAnswers = 10;
            QuestionSet questionSet =
                await db.QuestionSets.FirstOrDefaultAsync(x => x.MicrotingUid == apiQuestionSetId)
                    .ConfigureAwait(false);
            if (questionSet != null)
            {
                var questionSetId = questionSet.Id;
                var lastAnswer = await db.Answers.OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(x =>
                        x.QuestionSetId == questionSetId
                        && x.WorkflowState != Constants.WorkflowStates.PreCreated
                        && x.WorkflowState != Constants.WorkflowStates.Removed)
                    .ConfigureAwait(false);
                JObject parsedData = null;
                if (lastAnswer != null)
                {
                    while (numAnswers > 9)
                    {
                        try
                        {
                            lastAnswer = await db.Answers.OrderByDescending(x => x.Id)
                                .FirstOrDefaultAsync(x =>
                                    x.QuestionSetId == questionSetId
                                    && x.WorkflowState != Constants.WorkflowStates.PreCreated
                                    && x.WorkflowState != Constants.WorkflowStates.Removed)
                                .ConfigureAwait(false);
                            if (lastAnswer != null)
                            {
                                parsedData = JObject.Parse(await _communicator
                                    .GetLastAnswer((int)apiQuestionSetId, (int)lastAnswer.MicrotingUid)
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
                            throw;
                        }
                    }
                }
                else
                {
                    parsedData = JObject.Parse(await _communicator.GetLastAnswer((int)apiQuestionSetId, 0)
                        .ConfigureAwait(false));
                    numAnswers = await SaveAnswers(questionSet, parsedData).ConfigureAwait(false);

                    while (numAnswers > 9)
                    {
                        try
                        {
                            lastAnswer = await db.Answers.OrderByDescending(x => x.Id)
                                .FirstOrDefaultAsync(x =>
                                    x.QuestionSetId == questionSetId
                                    && x.WorkflowState != Constants.WorkflowStates.PreCreated
                                    && x.WorkflowState != Constants.WorkflowStates.Removed)
                                .ConfigureAwait(false);
                            if (lastAnswer != null)
                            {
                                parsedData = JObject.Parse(await _communicator
                                    .GetLastAnswer((int)apiQuestionSetId, (int)lastAnswer.MicrotingUid)
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

        //

        //

        // public advanced actions
        // templat
        public async Task<bool> Advanced_TemplateDisplayIndexChangeDb(int templateId, int newDisplayIndex)
        {
            string methodName = "Core.Advanced_TemplateDisplayIndexChangeDb";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(newDisplayIndex), newDisplayIndex);

                return await _sqlController.TemplateDisplayIndexChange(templateId, newDisplayIndex)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_TemplateDisplayIndexChangeServer(int templateId, int siteUId,
            int newDisplayIndex)
        {
            string methodName = "Core.Advanced_TemplateDisplayIndexChangeServer";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(siteUId), siteUId);
                Log.LogVariable(methodName, nameof(newDisplayIndex), newDisplayIndex);

                string respXml = null;
                List<string> errors = new List<string>();
                foreach (int microtingUId in await _sqlController
                             .CheckListSitesRead(templateId, siteUId, Constants.WorkflowStates.NotRemoved)
                             .ConfigureAwait(false))
                {
                    respXml = await _communicator
                        .TemplateDisplayIndexChange(microtingUId.ToString(), siteUId, newDisplayIndex)
                        .ConfigureAwait(false);
                    Response resp = new Response();
                    resp = resp.XmlToClassUsingXmlDocument(respXml);
                    if (resp.Type == Response.ResponseTypes.Success) continue;
                    string error = $"Failed to set display index for eForm {microtingUId} to {newDisplayIndex}";
                    errors.Add(error);
                }

                if (errors.Any())
                {
                    throw new Exception(String.Join("\n", errors));
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_TemplateUpdateFieldIdsForColumns(int templateId, int? fieldId1, int? fieldId2,
            int? fieldId3, int? fieldId4, int? fieldId5, int? fieldId6, int? fieldId7, int? fieldId8, int? fieldId9,
            int? fieldId10)
        {
            string methodName = "Core.Advanced_TemplateUpdateFieldIdsForColumns";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);
                Log.LogVariable(methodName, nameof(fieldId1), fieldId1);
                Log.LogVariable(methodName, nameof(fieldId2), fieldId2);
                Log.LogVariable(methodName, nameof(fieldId3), fieldId3);
                Log.LogVariable(methodName, nameof(fieldId4), fieldId4);
                Log.LogVariable(methodName, nameof(fieldId5), fieldId5);
                Log.LogVariable(methodName, nameof(fieldId6), fieldId6);
                Log.LogVariable(methodName, nameof(fieldId7), fieldId7);
                Log.LogVariable(methodName, nameof(fieldId8), fieldId8);
                Log.LogVariable(methodName, nameof(fieldId9), fieldId9);
                Log.LogVariable(methodName, nameof(fieldId10), fieldId10);

                return await _sqlController.TemplateUpdateFieldIdsForColumns(templateId, fieldId1, fieldId2,
                        fieldId3, fieldId4, fieldId5, fieldId6, fieldId7, fieldId8, fieldId9, fieldId10)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldDto>> Advanced_TemplateFieldReadAll(int templateId, Language language)
        {
            string methodName = "Core.Advanced_TemplateFieldReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(templateId), templateId);

                return await _sqlController.TemplateFieldReadAll(templateId, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        // sites
        public async Task<List<SiteDto>> Advanced_SiteReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = "Core.Advanced_SiteReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(workflowState), workflowState);
                Log.LogVariable(methodName, nameof(offSet), offSet.ToString());
                Log.LogVariable(methodName, nameof(limit), limit.ToString());

                return await _sqlController.SimpleSiteGetAll(workflowState, offSet, limit).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<SiteNameDto> Advanced_SiteItemRead(int microting_uuid)
        {
            string methodName = "Core.Advanced_SiteItemRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microting_uuid), microting_uuid);

                return await _sqlController.SiteRead(microting_uuid).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
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
                if (!Running()) throw new Exception("Core is not running");
                return await _sqlController.SiteGetAll(includeRemoved).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteItemUpdate(int siteId, string name, string languageCode)
        {
            string methodName = "Core.Advanced_SiteItemUpdate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(siteId), siteId);
                Log.LogVariable(methodName, nameof(name), name);

                var db = DbContextHelper.GetDbContext();
                if (!db.Sites.Any(x => x.MicrotingUid == siteId))
                {
                    return false;
                }
                //if (await _sqlController.SiteRead(siteId).ConfigureAwait(false) == null)

                bool success = await _communicator.SiteUpdate(siteId, name, languageCode).ConfigureAwait(false);
                if (!success)
                    return false;

                Site site = await db.Sites.FirstOrDefaultAsync(x => x.MicrotingUid == siteId);

                if (site != null)
                {
                    Language language = db.Languages.Single(x => x.LanguageCode == languageCode);
                    site.Name = name;
                    site.LanguageId = language.Id;
                    await site.Update(db).ConfigureAwait(false);

                    if (site.SearchableEntityItemId == 0)
                    {
                        Microting.eForm.Infrastructure.Data.Entities.EntityGroup searchableList = await db.EntityGroups
                                .FirstOrDefaultAsync(x =>
                                    x.Name == "Device users" && x.Type == Constants.FieldTypes.EntitySearch) ??
                            await EntityGroupCreate(Constants.FieldTypes.EntitySearch, "Device users", "", true, false);
                        searchableList.Locked = true;
                        await searchableList.Update(db);

                        var searchItem =
                            await EntitySearchItemCreate(searchableList.Id, site.Name, "", site.Id.ToString());
                        site.SearchableEntityItemId = searchItem.Id;
                        await site.Update(db);
                    }
                    else
                    {
                        await EntityItemUpdate(site.SearchableEntityItemId, site.Name, "", site.Id.ToString(), 0);
                    }

                    if (site.SelectableEntityItemId == 0)
                    {
                        Microting.eForm.Infrastructure.Data.Entities.EntityGroup selectableList = await db.EntityGroups
                                .FirstOrDefaultAsync(x =>
                                    x.Name == "Device users" && x.Type == Constants.FieldTypes.EntitySelect) ??
                            await EntityGroupCreate(Constants.FieldTypes.EntitySelect, "Device users", "", true, false);
                        selectableList.Locked = true;
                        await selectableList.Update(db);

                        var selectItem =
                            await EntitySelectItemCreate(selectableList.Id, site.Name, 0, site.Id.ToString());
                        site.SelectableEntityItemId = selectItem.Id;
                        await site.Update(db);
                    }
                    else
                    {
                        await EntityItemUpdate(site.SelectableEntityItemId, site.Name, "", site.Id.ToString(), 0);
                    }

                    return true;
                }

                return false;

                //return await _sqlController.SiteUpdate(siteId, name).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteItemDelete(int microtingUid)
        {
            string methodName = "Core.Advanced_SiteItemDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUid), microtingUid);
                await using MicrotingDbContext db = DbContextHelper.GetDbContext();

                bool success = await _communicator.SiteDelete(microtingUid).ConfigureAwait(false);
                if (!success)
                    return false;

                Site site = await db.Sites.FirstOrDefaultAsync(x => x.MicrotingUid == microtingUid);

                if (site != null)
                {
                    await site.Delete(db);
                    if (site.SearchableEntityItemId != 0)
                    {
                        await EntityItemDelete(site.SearchableEntityItemId);
                    }

                    if (site.SelectableEntityItemId != 0)
                    {
                        await EntityItemDelete(site.SelectableEntityItemId);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        //

        // workers
        public async Task<WorkerDto> Advanced_WorkerCreate(string firstName, string lastName, string email,
            string legacyEmail)
        {
            string methodName = "Core.Advanced_WorkerCreate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(firstName), firstName);
                Log.LogVariable(methodName, nameof(lastName), lastName);
                Log.LogVariable(methodName, nameof(email), email);

                WorkerDto workerDto = await _communicator.WorkerCreate(firstName, lastName, legacyEmail)
                    .ConfigureAwait(false);
                int workerUId = workerDto.WorkerUId;

                workerDto = await _sqlController.WorkerRead(workerDto.WorkerUId).ConfigureAwait(false);
                if (workerDto == null)
                {
                    await _sqlController.WorkerCreate(workerUId, firstName, lastName, email).ConfigureAwait(false);
                }

                return await Advanced_WorkerRead(workerUId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<string> Advanced_WorkerNameRead(int workerId)
        {
            string methodName = "Core.Advanced_WorkerNameRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(workerId), workerId);

                return await _sqlController.WorkerNameRead(workerId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<WorkerDto> Advanced_WorkerRead(int workerId)
        {
            string methodName = "Core.Advanced_WorkerRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(workerId), workerId);

                return await _sqlController.WorkerRead(workerId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<WorkerDto>> Advanced_WorkerReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = "Core.Advanced_WorkerReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(workflowState), workflowState);
                Log.LogVariable(methodName, nameof(offSet), offSet.ToString());
                Log.LogVariable(methodName, nameof(limit), limit.ToString());

                return await _sqlController.WorkerGetAll(workflowState, offSet, limit).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_WorkerUpdate(int workerId, string firstName, string lastName, string email,
            string legacyEmail)
        {
            string methodName = "Core.Advanced_WorkerUpdate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(workerId), workerId);
                Log.LogVariable(methodName, nameof(firstName), firstName);
                Log.LogVariable(methodName, nameof(lastName), lastName);
                Log.LogVariable(methodName, nameof(email), email);

                if (await _sqlController.WorkerRead(workerId).ConfigureAwait(false) == null)
                    return false;

                bool success = await _communicator.WorkerUpdate(workerId, firstName, lastName, legacyEmail)
                    .ConfigureAwait(false);
                if (!success)
                    return false;

                return await _sqlController.WorkerUpdate(workerId, firstName, lastName, email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_WorkerDelete(int microtingUid)
        {
            string methodName = "Core.Advanced_WorkerDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                bool success = await _communicator.WorkerDelete(microtingUid).ConfigureAwait(false);
                if (!success)
                    return false;

                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
                var worker = await dbContext.Workers.FirstOrDefaultAsync(x => x.MicrotingUid == microtingUid);
                if (worker == null) return false;
                await worker.Delete(dbContext);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        //
        //

        // site_workers
        public async Task<SiteWorkerDto> Advanced_SiteWorkerCreate(SiteNameDto siteDto, WorkerDto workerDto)
        {
            string methodName = "Core.Advanced_SiteWorkerCreate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, "siteId", siteDto.SiteUId);
                Log.LogVariable(methodName, "workerId", workerDto.WorkerUId);

                SiteWorkerDto result = await _communicator.SiteWorkerCreate(siteDto.SiteUId, workerDto.WorkerUId)
                    .ConfigureAwait(false);

                SiteWorkerDto siteWorkerDto = await _sqlController.SiteWorkerRead(result.MicrotingUId, null, null)
                    .ConfigureAwait(false);

                if (siteWorkerDto == null)
                {
                    await _sqlController.SiteWorkerCreate(result.MicrotingUId, siteDto.SiteUId, workerDto.WorkerUId)
                        .ConfigureAwait(false);
                }

                return await Advanced_SiteWorkerRead(result.MicrotingUId, null, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<SiteWorkerDto> Advanced_SiteWorkerRead(int? siteWorkerMicrotingUid, int? siteId,
            int? workerId)
        {
            string methodName = "Core.Advanced_SiteWorkerRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(siteWorkerMicrotingUid), siteWorkerMicrotingUid.ToString());
                Log.LogVariable(methodName, nameof(siteId), siteId.ToString());
                Log.LogVariable(methodName, nameof(workerId), workerId.ToString());

                return await _sqlController.SiteWorkerRead(siteWorkerMicrotingUid, siteId, workerId)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_SiteWorkerDelete(int workerId)
        {
            string methodName = "Core.Advanced_SiteWorkerDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(workerId), workerId);

                bool success = await _communicator.SiteWorkerDelete(workerId).ConfigureAwait(false);
                if (!success)
                    return false;

                return await _sqlController.SiteWorkerDelete(workerId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        //

        // units
        public async Task<UnitDto> Advanced_UnitRead(int microtingUid)
        {
            string methodName = "Core.Advanced_UnitRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                return await _sqlController.UnitRead(microtingUid).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<List<UnitDto>> Advanced_UnitReadAll()
        {
            string methodName = "Core.Advanced_UnitReadAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");

                return await _sqlController.UnitGetAll().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                return null;
            }
        }

        public async Task<UnitDto> Advanced_UnitRequestOtp(int microtingUid)
        {
            string methodName = "Core.Advanced_UnitRequestOtp";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(microtingUid), microtingUid);

                await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();

                Unit unit = await dbContext.Units.FirstOrDefaultAsync(x => x.MicrotingUid == microtingUid);
                Site site = await dbContext.Sites.FirstOrDefaultAsync(x => x.Id == unit.SiteId);

                var result = await _communicator
                    .UnitRequestOtp(microtingUid, (int)site.MicrotingUid, true, unit.PushEnabled, unit.SyncDelayEnabled,
                        unit.SyncDialog)
                    .ConfigureAwait(false);
                var parsedResult = JObject.Parse(result);

                int otpCode = int.Parse(parsedResult["OtpCode"].ToString());

                UnitDto myDto = await Advanced_UnitRead(microtingUid).ConfigureAwait(false);

                await _sqlController.UnitUpdate(microtingUid, myDto.CustomerNo, otpCode, myDto.SiteUId)
                    .ConfigureAwait(false);

                return await Advanced_UnitRead(microtingUid).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw;
            }
        }

        public async Task<bool> Advanced_UnitDelete(int unitId)
        {
            string methodName = "Core.Advanced_UnitDelete";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(unitId), unitId);

                bool success = await _communicator.UnitDelete(unitId).ConfigureAwait(false);
                if (!success)
                    return false;

                return await _sqlController.UnitDelete(unitId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_UnitCreate(int siteMicrotingUid)
        {
            string methodName = "Core.Advanced_UnitCreate";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using var dbContext = DbContextHelper.GetDbContext();
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(siteMicrotingUid), siteMicrotingUid);

                Site site = await dbContext.Sites.FirstOrDefaultAsync(x => x.MicrotingUid == siteMicrotingUid);

                string result = await _communicator.UnitCreate((int)site.MicrotingUid).ConfigureAwait(false);
                if (result == null) return false;
                {
                    Unit unit = JsonConvert.DeserializeObject<Unit>(result);
                    unit.SiteId = dbContext.Sites.Single(x => x.MicrotingUid == unit.SiteId).Id;
                    await unit.Create(dbContext).ConfigureAwait(false);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_UnitMove(int unitId, int siteId)
        {
            string methodName = "Core.Advanced_UnitMove";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                await using var dbContext = DbContextHelper.GetDbContext();
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(unitId), unitId);
                Log.LogVariable(methodName, nameof(siteId), siteId);

                Unit unit = await dbContext.Units.FirstOrDefaultAsync(x => x.Id == unitId);
                Site site = await dbContext.Sites.FirstOrDefaultAsync(x => x.Id == siteId);

                string result = await _communicator.UnitMove((int)unit.MicrotingUid, (int)site.MicrotingUid)
                    .ConfigureAwait(false);
                if (result == null) return false;
                unit.SiteId = site.Id;
                await unit.Update(dbContext).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }
        //

        // fields
        public async Task<Field> Advanced_FieldRead(int id, Language language)
        {
            string methodName = "Core.Advanced_FieldRead";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(id), id);

                return await _sqlController.FieldRead(id, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<UploadedData> Advanced_UploadedDataRead(int id)
        {
            string methodName = "Core.Advanced_UploadedDataRead";
            try
            {
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(id), id);

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
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldValue>> Advanced_FieldValueReadList(int id, int instances, Language language)
        {
            string methodName = "Core.Advanced_FieldValueReadList";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(id), id);
                Log.LogVariable(methodName, nameof(instances), instances);

                return await _sqlController.FieldValueReadList(id, instances, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldValue>> Advanced_FieldValueReadList(int fieldId, List<int> caseIds,
            Language language)
        {
            string methodName = "Core.Advanced_FieldValueReadList";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(fieldId), fieldId);

                return await _sqlController.FieldValueReadList(fieldId, caseIds, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<FieldValue>> Advanced_FieldValueReadList(List<int> caseIds, Language language)
        {
            string methodName = "Core.Advanced_FieldValueReadList";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");

                return await _sqlController.FieldValueReadList(caseIds, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<List<CheckListValue>> Advanced_CheckListValueReadList(List<int> caseIds)
        {
            string methodName = "Core.Advanced_CheckListValueReadList";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");

                return await _sqlController.CheckListValueReadList(caseIds).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }


        //

        //EntityGroupList
        public async Task<EntityGroupList> Advanced_EntityGroupAll(string sort, string nameFilter, int pageIndex,
            int pageSize, string entityType, bool desc, string workflowState)
        {
            if (entityType != Constants.FieldTypes.EntitySearch && entityType != Constants.FieldTypes.EntitySelect)
                throw new Exception("EntityGroupAll failed. EntityType:" + entityType + " is not an known type");
            if (workflowState != Constants.WorkflowStates.NotRemoved &&
                workflowState != Constants.WorkflowStates.Created && workflowState != Constants.WorkflowStates.Removed)
                throw new Exception("EntityGroupAll failed. workflowState:" + workflowState +
                                    " is not an known workflow state");

            string methodName = "Core.Advanced_EntityGroupAll";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(sort), sort);
                Log.LogVariable(methodName, nameof(nameFilter), nameFilter);
                Log.LogVariable(methodName, nameof(pageIndex), pageIndex);
                Log.LogVariable(methodName, nameof(pageSize), pageSize);
                Log.LogVariable(methodName, nameof(entityType), entityType);
                Log.LogVariable(methodName, nameof(desc), desc);
                Log.LogVariable(methodName, nameof(workflowState), workflowState);

                return await _sqlController
                    .EntityGroupAll(sort, nameFilter, pageIndex, pageSize, entityType, desc, workflowState)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_DeleteUploadedData(int fieldId, int uploadedDataId)
        {
            string methodName = "Core.Advanced_DeleteUploadedData";
            try
            {
                if (!Running()) throw new Exception("Core is not running");
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(uploadedDataId), uploadedDataId);

                await using var db = DbContextHelper.GetDbContext();
                Microting.eForm.Infrastructure.Data.Entities.UploadedData uD =
                    await db.UploadedDatas.FirstAsync(x => x.Id == uploadedDataId);

                await uD.Delete(db);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task<bool> Advanced_UpdateCaseFieldValue(int caseId, Language language)
        {
            string methodName = "Core.Advanced_UpdateCaseFieldValue";
            try
            {
                if (!Running()) return false;
                Log.LogStandard(methodName, "called");
                Log.LogVariable(methodName, nameof(caseId), caseId);
                return await _sqlController.CaseUpdateFieldValues(caseId, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogException(methodName, "failed", ex);
                throw new Exception("failed", ex);
            }
        }

        public async Task SendPushMessage(int siteId, string header, string body, int microtingUuid)
        {
            await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
            Site site = await dbContext.Sites.FirstOrDefaultAsync(x => x.Id == siteId);
            if (site != null)
            {
                await _communicator.SendPushMessage((int)site.MicrotingUid, header, body, microtingUuid);
            }
        }
        //

        // private
        private async Task<List<Element>> ReplaceDataElementsAndDataItems(int caseId, List<Element> elementList,
            List<FieldValue> lstAnswers)
        {
            List<Element> elementListReplaced = new List<Element>();

            foreach (Element element in elementList)
            {
                // if DataElement
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    // replace DataItemGroups
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
                    //

                    // replace DataItems
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
                    //

                    elementListReplaced.Add(new CheckListValue(dataE,
                        await _sqlController.CheckListValueStatusRead(caseId, element.Id)));
                }
                //

                // if GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    await ReplaceDataElementsAndDataItems(caseId, groupE.ElementList, lstAnswers).ConfigureAwait(false);

                    elementListReplaced.Add(groupE);
                }
                //
            }

            return elementListReplaced;
        }

        private async Task<int> SendXml(MainElement mainElement, int siteId)
        {
            string methodName = "Core.SendXml";
            Log.LogEverything(methodName, "siteId:" + siteId + ", requested sent eForm");

            string xmlStrRequest = mainElement.ClassToXml();

            Log.LogEverything(methodName, "siteId:" + siteId + ", ClassToXml done");
            string xmlStrResponse = await _communicator.PostXml(xmlStrRequest, siteId);
            Log.LogEverything(methodName, "siteId:" + siteId + ", PostXml done");

            Response response = new Response();
            response = response.XmlToClass(xmlStrResponse);
            Log.LogEverything(methodName, "siteId:" + siteId + ", XmlToClass done");

            //if reply is "success", it's created
            if (response.Type.ToString().ToLower() == "success")
            {
                return int.Parse(response.Value);
            }

            throw new Exception("siteId:'" + siteId + "' // failed to create eForm at Microting // Response :" +
                                xmlStrResponse);
        }

        private async Task<int> SendJson(MainElement mainElement, int siteId)
        {
            string methodName = "Core.SendJson";
            Log.LogEverything(methodName, "siteId:" + siteId + ", requested sent eForm");

            string request = mainElement.ClassToJson();

            Log.LogEverything(methodName, "siteId:" + siteId + ", ClassToJson done");
            string jsonStringResponse = await _communicator.PostJson(request, siteId);
            Log.LogEverything(methodName, "siteId:" + siteId + ", PostJson done");

            Response response = new Response();
            response = response.JsonToClass(jsonStringResponse);
            Log.LogEverything(methodName, "siteId:" + siteId + ", JsonToClass done");

            //if reply is "success", it's created
            if (response.Type.ToString().ToLower() == "success")
            {
                return int.Parse(response.Value);
            }

            throw new Exception("siteId:'" + siteId + "' // failed to create eForm at Microting // Response :" +
                                jsonStringResponse);
        }

        public async Task<List<List<string>>> GenerateDataSetFromCases(int? checkListId, DateTime? start, DateTime? end,
            string customPathForUploadedData, string decimalSeparator, string thousandSeparator, bool utcTime,
            CultureInfo cultureInfo, TimeZoneInfo timeZoneInfo, Language language)
        {
            return await GenerateDataSetFromCases(checkListId, start, end, customPathForUploadedData, decimalSeparator,
                thousandSeparator, utcTime, cultureInfo, timeZoneInfo, language, false, false);
        }

        public async Task<List<List<string>>> GenerateDataSetFromCases(int? checkListId, DateTime? start, DateTime? end,
            string customPathForUploadedData, string decimalSeparator, string thousandSeparator, bool utcTime,
            CultureInfo cultureInfo, TimeZoneInfo timeZoneInfo, Language language, bool includeCheckListText,
            bool gpsCoordinates)
        {
            await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
            List<List<string>> dataSet = new List<List<string>>();
            List<string> colume1CaseIds = new List<string> { "Id" };
            List<int> caseIds = new List<int>();

            start ??= DateTime.MinValue;
            end ??= DateTime.MaxValue;
            List<Microting.eForm.Infrastructure.Data.Entities.Case> cases = await dbContext.Cases.Where(x =>
                x.DoneAtUserModifiable > start && x.DoneAtUserModifiable < end
                                               && x.WorkflowState != Constants.WorkflowStates.Removed
                                               && x.CheckListId == checkListId).ToListAsync();

            CheckList checkList = await dbContext.CheckLists.FirstAsync(x => x.Id == (int)checkListId);

            if (cases.Count == 0)
                return null;

            // First columns section start
            {
                List<string> colume2 = new List<string> { "Created on device date" };
                List<string> colume3 = new List<string> { "Created on device time" };
                List<string> colume4 = new List<string> { "Created on device day" };
                List<string> colume5 = new List<string> { "Created on device week" };
                List<string> colume6 = new List<string> { "Created on device month" };
                List<string> colume7 = new List<string> { "Created on device year" };
                List<string> colume8 = new List<string> { "Received by Microting server" };
                List<string> colume9 = new List<string> { "Site" };
                List<string> colume10 = new List<string> { "Device User" };
                List<string> colume11 = new List<string> { "Device Id" };
                List<string> colume12 = new List<string> { "eForm Name" };

                var cal = DateTimeFormatInfo.CurrentInfo?.Calendar;
                foreach (var aCase in cases)
                {
                    DateTime time = (DateTime)aCase.DoneAtUserModifiable;
                    DateTime createdAt = (DateTime)aCase.CreatedAt;
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
                    colume5.Add(
                        $"{time.Year}.{cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)}");
                    //colume6.Add(time.Year.ToString() + "." + time.ToString("MMMM").Substring(0, 3));
                    colume6.Add(time.Year + "." + time.ToString("MMMM").AsSpan().Slice(0, 3).ToString());
                    colume7.Add(time.Year.ToString());
                    colume8.Add(createdAt.ToString("yyyy.MM.dd HH:mm:ss"));
                    Site site = await dbContext.Sites.FirstAsync(x => x.Id == aCase.SiteId);
                    colume9.Add(site.Name);
                    Worker worker = await dbContext.Workers.FirstAsync(x => x.Id == aCase.WorkerId);
                    colume10.Add(worker.full_name());
                    colume11.Add(aCase.UnitId.ToString());
                    CheckListTranslation checkListTranslation =
                        await dbContext.CheckListTranslations.FirstAsync(x =>
                            x.CheckListId == checkList.Id && x.LanguageId == language.Id);
                    colume12.Add(checkListTranslation.Text);
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
            // First columns section end

            // fieldValue generate start
            {
                try
                {
                    if (checkListId != null)
                    {
                        List<string> lstReturn = new List<string>();
                        lstReturn = await GenerateDataSetFromCasesSubSet(lstReturn, checkListId, "", language,
                            includeCheckListText);

                        foreach (string set in lstReturn)
                        {
                            int fieldId = int.Parse(_t.SplitToList(set, 0, false));
                            string label = _t.SplitToList(set, 1, false);

                            List<List<KeyValuePair>> result = await _sqlController.FieldValueReadAllValues(fieldId,
                                caseIds, customPathForUploadedData, decimalSeparator, thousandSeparator, language,
                                gpsCoordinates).ConfigureAwait(false);

                            List<string> newRow;
                            if (result.Count == 1)
                            {
                                newRow = new List<string>();
                                newRow.Insert(0, label);
                                List<KeyValuePair> tempList = result[0];
                                foreach (int i in caseIds)
                                {
                                    string value = "";
                                    foreach (KeyValuePair kvP in tempList)
                                    {
                                        if (kvP.Key == i.ToString())
                                        {
                                            value = kvP.Value;
                                        }
                                    }

                                    newRow.Add(value);
                                }

                                dataSet.Add(newRow);
                            }
                            else
                            {
                                int option = 0;
                                Field field = await _sqlController.FieldRead(fieldId, language).ConfigureAwait(false);
                                foreach (var lst in result)
                                {
                                    newRow = new List<string>();
                                    List<KeyValuePair> fieldKvP = field.KeyValuePairList;
                                    newRow.Insert(0, label + " | " + fieldKvP.ElementAt(option).Value);
                                    foreach (int i in caseIds)
                                    {
                                        string value = "";
                                        foreach (KeyValuePair kvP in lst)
                                        {
                                            if (kvP.Key == i.ToString())
                                            {
                                                value = kvP.Value;
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //  fieldValue generate end

            return dataSet;
        }

        private async Task<List<string>> GenerateDataSetFromCasesSubSet(List<string> lstReturn, int? checkListId,
            string preLabel, Language language, bool includeCheckListText)
        {
            string sep = " / ";
            await using MicrotingDbContext dbContext = DbContextHelper.GetDbContext();
            if (checkListId != null)
            {
                if (dbContext.CheckLists.Any(x => x.ParentId == checkListId))
                {
                    foreach (CheckList checkList in await dbContext.CheckLists.Where(x => x.ParentId == checkListId)
                                 .OrderBy(x => x.DisplayIndex).ToListAsync())
                    {
                        CheckList parentCheckList =
                            await dbContext.CheckLists.FirstAsync(x => x.Id == checkListId);
                        if (parentCheckList.ParentId != null)
                        {
                            if (preLabel != "")
                            {
                                CheckListTranslation checkListTranslation =
                                    await dbContext.CheckListTranslations.FirstAsync(x =>
                                        x.CheckListId == parentCheckList.Id && x.LanguageId == language.Id);
                                preLabel = preLabel + sep + checkListTranslation.Text;
                            }
                        }

                        await GenerateDataSetFromCasesSubSet(lstReturn, checkList.Id, preLabel, language,
                            includeCheckListText);
                    }
                }
                else
                {
                    foreach (Microting.eForm.Infrastructure.Data.Entities.Field field in await dbContext.Fields
                                 .Where(x => x.CheckListId == checkListId && x.ParentFieldId == null)
                                 .OrderBy(x => x.DisplayIndex).ToListAsync())
                    {
                        if (dbContext.Fields.Any(x => x.ParentFieldId == field.Id))
                        {
                            foreach (var subField in await dbContext.Fields.Where(x => x.ParentFieldId == field.Id)
                                         .OrderBy(x => x.DisplayIndex).ToListAsync())
                            {
                                if (field.FieldTypeId != 3 && field.FieldTypeId != 18)
                                {
                                    FieldTranslation fieldTranslation =
                                        await dbContext.FieldTranslations.FirstAsync(x =>
                                            x.FieldId == field.Id && x.LanguageId == language.Id);
                                    FieldTranslation subFieldTranslation =
                                        await dbContext.FieldTranslations.FirstAsync(x =>
                                            x.FieldId == subField.Id && x.LanguageId == language.Id);

                                    if (preLabel != "")
                                        lstReturn.Add(subField.Id + "|" + preLabel + sep + fieldTranslation.Text +
                                                      sep +
                                                      subFieldTranslation.Text);
                                    else
                                        lstReturn.Add(subField.Id + "|" + fieldTranslation.Text + sep +
                                                      subFieldTranslation.Text);
                                }
                            }
                        }
                        else
                        {
                            if (field.FieldTypeId != 3 && field.FieldTypeId != 18)
                            {
                                FieldTranslation fieldTranslation =
                                    await dbContext.FieldTranslations.FirstAsync(x =>
                                        x.FieldId == field.Id && x.LanguageId == language.Id);
                                if (includeCheckListText)
                                {
                                    CheckListTranslation checkListTranslation =
                                        await dbContext.CheckListTranslations.FirstAsync(x =>
                                            x.CheckListId == field.CheckListId && x.LanguageId == language.Id);
                                    preLabel = checkListTranslation.Text;
                                }

                                if (preLabel != "")
                                    lstReturn.Add(field.Id + "|" + preLabel + sep + fieldTranslation.Text);
                                else
                                    lstReturn.Add(field.Id + "|" + fieldTranslation.Text);
                            }
                        }
                    }
                }
            }

            return lstReturn;
        }

        private async Task<List<string>> PdfValidate(string pdfString, int pdfId)
        {
            await Task.Run(() => { }); // TODO FIX ME
            List<string> errorLst = new List<string>();

            if (pdfString.ToLower().Contains("microting.com"))
                errorLst.Add("Element showPdf.Id:'" + pdfId +
                             "' contains an URL that points to Microting's builder temporary hosting. Indicating that it's not a proper hash");
            if (pdfString.ToLower().Contains("http") || pdfString.ToLower().Contains("https"))
                errorLst.Add("Element showPdf.Id:'" + pdfId +
                             "' contains an HTTP or HTTPS. Indicating that it's not a proper hash");
            if (pdfString.Length != 32)
                errorLst.Add("Element showPdf.Id:'" + pdfId +
                             "' length is not the correct length (32). Indicating that it's not a proper hash");

            if (errorLst.Count > 0)
                errorLst.Add("Element showPdf.Id:'" + pdfId +
                             "' please check 'value' input, and consider running PdfUpload");

            return errorLst;
        }

        private async Task<string> GetJasperFieldValue(Field field, FieldValue answer, string customPathForUploadedData)
        {
            var token = await GetSdkSetting(Settings.token);
            StringBuilder jasperFieldXml = new StringBuilder();
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
                            if (answer.UploadedDataObj.FileName != null)
                            {
                                string bigFilename =
                                    $"{answer.UploadedDataObj.Id}_700_{answer.UploadedDataObj.Checksum}{answer.UploadedDataObj.Extension}";
                                jasperFieldXml.Append(Environment.NewLine);
                                jasperFieldXml.Append("<F" + field.Id + "_value field_value_id=\"" +
                                                      answer.Id + "\" " + gps + "><![CDATA[" +
                                                      customPathForUploadedData +
                                                      bigFilename + "&token=" + token + "]]></F" + field.Id +
                                                      "_value>");
                            }
                        }
                        else
                        {
                            if (answer.UploadedDataObj.FileName != null)
                            {
                                string bigFilename =
                                    $"{answer.UploadedDataObj.Id}_700_{answer.UploadedDataObj.Checksum}{answer.UploadedDataObj.Extension}";
                                jasperFieldXml.Append(Environment.NewLine);
                                jasperFieldXml.Append("<F" + field.Id + "_value field_value_id=\"" +
                                                      answer.Id + "\" " + gps + "><![CDATA[" + bigFilename +
                                                      "]]></F" + field.Id + "_value>");
                            }
                        }
                    }

                    break;
                case Constants.FieldTypes.Number:
                case Constants.FieldTypes.NumberStepper:
                    jasperFieldXml.Append(Environment.NewLine);
                    jasperFieldXml.Append("<F" + field.Id +
                                          "_value field_value_id=\"" + answer.Id + "\" " + gps + "><![CDATA[" +
                                          (answer.ValueReadable.Replace(",", ".") ?? string.Empty) + "]]></F" +
                                          field.Id +
                                          "_value>");
                    break;
                default:
                {
                    jasperFieldXml.Append(Environment.NewLine);
                    jasperFieldXml.Append("<F" + field.Id +
                                          "_value field_value_id=\"" + answer.Id + "\" " + gps + "><![CDATA[" +
                                          (answer.ValueReadable ?? string.Empty) + "]]></F" + field.Id +
                                          "_value>");
                    break;
                }
            }

            return jasperFieldXml.ToString();
        }

        private async Task<string> GetExtraFieldValues(int caseId, string customPathForUploadedData, Language language)
        {
            var token = await GetSdkSetting(Settings.token);
            var db = DbContextHelper.GetDbContext();
            StringBuilder jasperFieldXml = new StringBuilder();

            var extraFieldValues = await db.ExtraFieldValues.Where(x => x.CaseId == caseId).OrderBy(x => x.CheckListId)
                .ThenBy(x => x.FieldType).ToListAsync();

            int lastCheckListId = 0;
            string lastType = "";
            int i = 1;
            int total = extraFieldValues.Count;
            foreach (ExtraFieldValue extraFieldValue in extraFieldValues)
            {
                var cl = db.CheckLists.FirstOrDefault(x => x.Id == extraFieldValue.CheckListId);
                var clt = db.CheckListTranslations.FirstOrDefault(x =>
                    x.CheckListId == cl.Id && x.LanguageId == language.Id);

                if (lastCheckListId != extraFieldValue.CheckListId)
                {
                    switch (lastType)
                    {
                        case "picture":
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("</pictures>");
                            jasperFieldXml.Append("</extra_field>");
                            break;
                        case "comment":
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("</comments>");
                            jasperFieldXml.Append("</extra_field>");
                            break;
                        case "audio":
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("</audios>");
                            jasperFieldXml.Append("</extra_field>");
                            break;
                    }

                    lastType = "";
                    jasperFieldXml.Append(Environment.NewLine);
                    jasperFieldXml.Append($"<extra_field name=\"{clt.Text}\">");
                }

                if (lastType != extraFieldValue.FieldType)
                {
                    switch (lastType)
                    {
                        case "picture":
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("</pictures>");
                            break;
                        case "comment":
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("</comments>");
                            break;
                        case "audio":
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("</audios>");
                            break;
                    }
                }

                switch (extraFieldValue.FieldType)
                {
                    case "picture":
                        if (lastType != "picture")
                        {
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("<pictures>");
                        }

                        var uploadedData = db.UploadedDatas.FirstOrDefault(x => x.Id == extraFieldValue.UploadedDataId);
                        if (uploadedData != null)
                        {
                            if (customPathForUploadedData != null)
                            {
                                if (uploadedData.FileName != null)
                                {
                                    jasperFieldXml.Append(Environment.NewLine);
                                    string bigFilename =
                                        $"{uploadedData.Id}_700_{uploadedData.Checksum}{uploadedData.Extension}";
                                    jasperFieldXml.Append(
                                        $"<picture id=\"{extraFieldValue.Id}\"><![CDATA[{customPathForUploadedData}{bigFilename}&token={token}]]></picture>");
                                }
                            }
                            else
                            {
                                if (uploadedData.FileName != null)
                                {
                                    string bigFilename =
                                        $"{uploadedData.Id}_700_{uploadedData.Checksum}{uploadedData.Extension}";
                                    jasperFieldXml.Append(Environment.NewLine);
                                    jasperFieldXml.Append(
                                        $"<picture id=\"{extraFieldValue.Id}\"><![CDATA[{bigFilename}&token={token}]]></picture>");
                                }
                            }
                        }

                        lastType = "picture";

                        // if (i == total)
                        // {
                        //     jasperFieldXml.Append(Environment.NewLine);
                        //     jasperFieldXml.Append("</pictures>");
                        // }
                        break;
                    case "comment":
                        if (lastType != "comment")
                        {
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("<comments>");
                        }

                        jasperFieldXml.Append(Environment.NewLine);
                        jasperFieldXml.Append(
                            $"<comment id=\"{extraFieldValue.Id}\"><![CDATA[{extraFieldValue.Value}]]></comment>");
                        // if (i == total)
                        // {
                        //     jasperFieldXml.Append(Environment.NewLine);
                        //     jasperFieldXml.Append("</comments>");
                        // }
                        lastType = "comment";
                        break;
                    case "audio":
                        if (lastType != "audio")
                        {
                            jasperFieldXml.Append(Environment.NewLine);
                            jasperFieldXml.Append("<audios>");
                        }

                        // if (i == total)
                        // {
                        //     jasperFieldXml.Append(Environment.NewLine);
                        //     jasperFieldXml.Append("</audios>");
                        // }
                        lastType = "audio";
                        break;
                }

                // if (lastCheckListId != extraFieldValue.CheckListId)
                // {
                //     switch (lastType)
                //     {
                //         case "picture":
                //             jasperFieldXml.Append(Environment.NewLine);
                //             jasperFieldXml.Append("</pictures>");
                //             break;
                //         case "comment":
                //             jasperFieldXml.Append(Environment.NewLine);
                //             jasperFieldXml.Append("</comments>");
                //             break;
                //         case "audio":
                //             jasperFieldXml.Append(Environment.NewLine);
                //             jasperFieldXml.Append("</audios>");
                //             break;
                //     }
                //     jasperFieldXml.Append(Environment.NewLine);
                //     jasperFieldXml.Append("</extra_field>");
                // }

                lastCheckListId = (int)extraFieldValue.CheckListId;
                // Environment.NewLine + "<F" + extraFieldValue.Id + "_value field_value_id=\"" +
                // answer.Id + "\" " + gps + "><![CDATA[" + answer.UploadedDataObj.FileName +
                // "]]></F" + field.Id + "_value>";
                i++;
            }

            switch (lastType)
            {
                case "picture":
                    jasperFieldXml.Append(Environment.NewLine);
                    jasperFieldXml.Append("</pictures>");
                    break;
                case "comment":
                    jasperFieldXml.Append(Environment.NewLine);
                    jasperFieldXml.Append("</comments>");
                    break;
                case "audio":
                    jasperFieldXml.Append(Environment.NewLine);
                    jasperFieldXml.Append("</audios>");
                    break;
            }

            if (total != 0)
            {
                jasperFieldXml.Append(Environment.NewLine);
                jasperFieldXml.Append("</extra_field>");
            }

            return jasperFieldXml.ToString();
        }

        private void GetChecksAndFields(ref string clsLst, ref string fldLst, List<Element> elementLst,
            string customPathForUploadedData)
        {
            var db = DbContextHelper.GetDbContext();
            string jasperFieldXml = "";
            string jasperCheckXml = "";
            elementLst = elementLst.OrderBy(x => x.Id).ToList();

            foreach (Element element in elementLst)
            {
                if (element.GetType() == typeof(CheckListValue))
                {
                    CheckListValue dataE = (CheckListValue)element;
                    var clc = db.CheckLists.Single(x => x.Id == dataE.Id);
                    var clp = db.CheckLists.Single(x => x.Id == clc.ParentId);
                    var clpt = db.CheckListTranslations.First(x => x.CheckListId == clp.Id);

                    jasperCheckXml += Environment.NewLine + "<C" + dataE.Id + " name=\"" + dataE.Label +
                                      "\" parent=\"" + clpt.Text + "\">" + dataE.Status + "</C" + dataE.Id + ">";

                    foreach (var item in dataE.DataItemList)
                    {
                        var f = db.Fields.Single(x => x.Id == item.Id);
                        var cl = db.CheckLists.Single(x => x.Id == f.CheckListId);
                        var clt = db.CheckListTranslations.First(x => x.CheckListId == cl.Id);
                        jasperFieldXml += Environment.NewLine + "<F" + item.Id + " name=\"" + item.Label +
                                          "\" parent=\"" + clt.Text + "\">";

                        if (item is Field)
                        {
                            Field field = (Field)item;
                            foreach (FieldValue answer in field.FieldValues)
                            {
                                jasperFieldXml += GetJasperFieldValue(field, answer, customPathForUploadedData)
                                    .GetAwaiter().GetResult();
                            }
                        }
                        else if (item is FieldContainer)
                        {
                            FieldContainer fieldC = (FieldContainer)item;

                            foreach (var dataItem in fieldC.DataItemList)
                            {
                                var field = (Field)dataItem;
                                jasperFieldXml += Environment.NewLine + "<F" + field.Id + " name=\"" + field.Label +
                                                  "\" parent=\"" + dataE.Label + "\">";
                                foreach (FieldValue answer in field.FieldValues)
                                {
                                    jasperFieldXml += GetJasperFieldValue(field, answer, customPathForUploadedData)
                                        .GetAwaiter().GetResult();
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
        //

        // intrepidation threads
        private async Task CoreThread()
        {
            _coreThreadRunning = true;

            string methodName = "Core.CoreThread";
            Log.LogEverything(methodName, "initiated");
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
                    Log.LogWarning(methodName, "catch of ThreadAbortException");
                }
                catch (Exception ex)
                {
                    await FatalExpection(methodName + "failed", ex).ConfigureAwait(false);
                }
            }

            Log.LogEverything(methodName, "completed");

            _coreThreadRunning = false;
        }

        public async Task<bool> DownloadUploadedData(int uploadedDataId)
        {
            string methodName = "Core.DownloadUploadedData";
            Microting.eForm.Infrastructure.Data.Entities.UploadedData uploadedData =
                await _sqlController.GetUploadedData(uploadedDataId).ConfigureAwait(false);

            try
            {
                if (uploadedData != null)
                {
                    string urlStr = uploadedData.FileLocation;

                    if (urlStr == "/tmp/pictures")
                    {
                        // https://srv05.microting.com/app_files/inspection_app/uploads/1092/4926e2815deed8fedfa8d016b91631c0.jpeg
                        urlStr = $"{await _sqlController.SettingRead(Settings.comAddressApi).ConfigureAwait(false)}/app_files/inspection_app/uploads/{await _sqlController.SettingRead(Settings.comOrganizationId).ConfigureAwait(false)}/{uploadedData.Checksum}{uploadedData.Extension}";
                    }

                    Log.LogEverything(methodName, "Received file:" + uploadedData);

                    int index = urlStr.LastIndexOf("/", StringComparison.Ordinal) + 1;
                    string fileName = uploadedData.Id + "_" + urlStr.Remove(0, index);

                    // download file
                    using var client = new HttpClient();
                    try
                    {
                        Log.LogStandard(methodName, $"Downloading file {fileName}");
                        var result = await client.GetAsync(urlStr);
                        if (result.StatusCode != HttpStatusCode.OK)
                        {
                            return false;
                        }

                        var stream = await result.Content.ReadAsStreamAsync();

                        MemoryStream baseMemoryStream = new MemoryStream();
                        await stream.CopyToAsync(baseMemoryStream);
                        await stream.DisposeAsync();
                        stream.Close();

                        string fileCheckSum = fileName.AsSpan().Slice(fileName.LastIndexOf(".") - 32, 32).ToString();

                        baseMemoryStream.Seek(0, SeekOrigin.Begin);
                        var dbContext = DbContextHelper.GetDbContext();
                        var fv = await dbContext.FieldValues.FirstAsync(x => x.UploadedDataId == uploadedData.Id);
                        var theCase = await dbContext.Cases.FirstAsync(x => x.Id == fv.CaseId);

                        var unit = await dbContext.Units.FirstOrDefaultAsync(x => x.Id == theCase.UnitId);
                        if (unit == null)
                        {
                            if (dbContext.Units.Count(x => x.SiteId == theCase.SiteId) == 1)
                            {
                                unit = await dbContext.Units.FirstAsync(x => x.SiteId == theCase.SiteId);
                                theCase.UnitId = unit.Id;
                                await theCase.Update(dbContext);
                            }
                            else
                            {
                                Log.LogWarning(methodName, $"No unit found for case {theCase.Id}");
                                return false;
                            }
                        }

                        using (var image = new MagickImage(baseMemoryStream))
                        {
                            try
                            {
                                var profile = image.GetExifProfile();
                                if (profile != null)
                                {
                                    foreach (var value in profile.Values)
                                    {
                                        Console.WriteLine($"value: {value}");
                                        if (value.Tag == ExifTag.Orientation)
                                        {
                                            Console.WriteLine($"rotate value is {value.GetValue()}");
                                            if (unit.Manufacturer == "iOS")
                                            {
                                                // CW90, Normal, 270 CW, Rotate 180
                                                if (value.GetValue().ToString() == "6")
                                                {
                                                    image.Rotate(90);
                                                    image.Orientation = OrientationType.TopLeft;
                                                }
                                                else if (value.GetValue().ToString() == "8")
                                                {
                                                    image.Rotate(270);
                                                    image.Orientation = OrientationType.TopLeft;
                                                }
                                                else if (value.GetValue().ToString() == "3")
                                                {
                                                    image.Rotate(180);
                                                    image.Orientation = OrientationType.TopLeft;
                                                }
                                            }
                                            else
                                            {
                                                // Console.WriteLine($"rotate value is {value.GetValue()}");
                                                if (value.GetValue().ToString() == "6")
                                                {
                                                    image.Rotate(90);
                                                    image.Orientation = OrientationType.TopLeft;
                                                }
                                                else if (value.GetValue().ToString() == "8")
                                                {
                                                    image.Rotate(270);
                                                    image.Orientation = OrientationType.TopLeft;
                                                }
                                                else if (value.GetValue().ToString() == "3")
                                                {
                                                    image.Rotate(180);
                                                    image.Orientation = OrientationType.TopLeft;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                // Console.WriteLine(e);
                            }

                            MemoryStream memoryStream = new MemoryStream();
                            await image.WriteAsync(memoryStream);
                            await PutFileToS3Storage(memoryStream, fileName);
                            await memoryStream.DisposeAsync();
                            memoryStream.Close();
                            image.Dispose();
                            baseMemoryStream.Seek(0, SeekOrigin.Begin);
                        }

                        Log.LogStandard(methodName, $"Download of '{urlStr}' completed");

                        if (fileName.Contains("png") || fileName.Contains("jpg") || fileName.Contains("jpeg"))
                        {
                            string smallFilename = uploadedData.Id + "_300_" + urlStr.Remove(0, index);
                            string bigFilename = uploadedData.Id + "_700_" + urlStr.Remove(0, index);
                            using (var image = new MagickImage(baseMemoryStream))
                            {
                                try
                                {
                                    var profile = image.GetExifProfile();
                                    if (profile != null)
                                    {
                                        foreach (var value in profile.Values)
                                        {
                                            if (value.Tag == ExifTag.Orientation)
                                            {
                                                if (unit.Manufacturer == "iOS")
                                                {
                                                    Console.WriteLine($"rotate value is {value.GetValue()}");
                                                    // CW90, Normal, 270 CW, Rotate 180
                                                    if (value.GetValue().ToString() == "6")
                                                    {
                                                        image.Rotate(90);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "8")
                                                    {
                                                        image.Rotate(270);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "3")
                                                    {
                                                        image.Rotate(180);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                }
                                                else
                                                {
                                                    if (value.GetValue().ToString() == "6")
                                                    {
                                                        image.Rotate(90);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "8")
                                                    {
                                                        image.Rotate(270);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "3")
                                                    {
                                                        image.Rotate(180);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    // Console.WriteLine(e);
                                }

                                decimal currentRation = image.Height / (decimal)image.Width;
                                int newWidth = 300;
                                int newHeight = (int)Math.Round((currentRation * newWidth));

                                image.Resize(newWidth, newHeight);
                                image.Crop(newWidth, newHeight);
                                MemoryStream memoryStream = new MemoryStream();
                                await image.WriteAsync(memoryStream);
                                await PutFileToS3Storage(memoryStream, smallFilename);
                                await memoryStream.DisposeAsync();
                                memoryStream.Close();
                                image.Dispose();
                                baseMemoryStream.Seek(0, SeekOrigin.Begin);
                            }

                            using (var image = new MagickImage(baseMemoryStream))
                            {
                                try
                                {
                                    var profile = image.GetExifProfile();
                                    if (profile != null)
                                    {
                                        foreach (var value in profile.Values)
                                        {
                                            if (value.Tag == ExifTag.Orientation)
                                            {
                                                if (unit.Manufacturer == "iOS")
                                                {
                                                    Console.WriteLine($"rotate value is {value.GetValue()}");
                                                    // CW90, Normal, 270 CW, Rotate 180
                                                    if (value.GetValue().ToString() == "6")
                                                    {
                                                        image.Rotate(90);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "8")
                                                    {
                                                        image.Rotate(270);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "3")
                                                    {
                                                        image.Rotate(180);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                }
                                                else
                                                {
                                                    if (value.GetValue().ToString() == "6")
                                                    {
                                                        image.Rotate(90);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "8")
                                                    {
                                                        image.Rotate(270);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                    else if (value.GetValue().ToString() == "3")
                                                    {
                                                        image.Rotate(180);
                                                        image.Orientation = OrientationType.TopLeft;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    // Console.WriteLine(e);
                                }

                                decimal currentRation = image.Height / (decimal)image.Width;
                                int newWidth = 700;
                                int newHeight = (int)Math.Round((currentRation * newWidth));

                                image.Resize(newWidth, newHeight);
                                image.Crop(newWidth, newHeight);
                                MemoryStream memoryStream = new MemoryStream();
                                await image.WriteAsync(memoryStream);
                                await PutFileToS3Storage(memoryStream, bigFilename);
                                await memoryStream.DisposeAsync();
                                memoryStream.Close();
                                image.Dispose();
                            }
                        }

                        await baseMemoryStream.DisposeAsync();
                        baseMemoryStream.Close();

                        await _sqlController
                            .FileProcessed(urlStr, fileCheckSum, _fileLocationPicture, fileName, uploadedData.Id)
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Log.LogWarning(methodName, "We got an error " + ex.Message);
                        throw new Exception("Downloading and creating fil locally failed.", ex);
                    }

                    Log.LogEverything(methodName, $"{urlStr} was processed correctly");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<GetObjectResponse> GetFileFromS3Storage(string fileName, bool isRetry = false)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName =
                        $"{await _sqlController.SettingRead(Settings.s3BucketName).ConfigureAwait(false)}",
                    Key = $"{_customerNo}/{fileName}"
                };

                return await _s3Client.GetObjectAsync(request);
            }
            catch (AmazonS3Exception ex)
            {
                if (isRetry)
                {
                    throw new UnauthorizedAccessException("Access denied for S3 storage", ex);
                }

                var dbContext = DbContextHelper.GetDbContext();
                var uD = await dbContext.UploadedDatas.SingleAsync(x => x.FileName == fileName);
                await DownloadUploadedData(uD.Id);
                return await GetFileFromS3Storage(fileName, true);
            }

            catch (Exception ex)
            {
                throw new Exception($"Unable to auto recover for file: {fileName}", ex);
            }
        }

        // public async Task<SwiftObjectGetResponse> GetFileFromSwiftStorage(string fileName)
        // {
        //     try
        //     {
        //         return await GetFileFromSwiftStorage(fileName, 0).ConfigureAwait(false);
        //     }
        //     catch (UnauthorizedAccessException)
        //     {
        //         _swiftClient.AuthenticateAsyncV2(_keystoneEndpoint, _swiftUserName, _swiftPassword);
        //
        //         return await GetFileFromSwiftStorage(fileName, 0).ConfigureAwait(false);
        //     }
        // }
        //
        // private async Task<SwiftObjectGetResponse> GetFileFromSwiftStorage(string fileName, int retries)
        // {
        //     string methodName = "Core.GetFileFromSwiftStorage";
        //     if (!_swiftEnabled) throw new FileNotFoundException();
        //     Log.LogStandard(methodName, $"Trying to get file {fileName} from {_customerNo}_uploaded_data");
        //     SwiftObjectGetResponse response = await _swiftClient.ObjectGetAsync(_customerNo + "_uploaded_data", fileName).ConfigureAwait(false);
        //     if (response.IsSuccess)
        //     {
        //         return response;
        //     }
        //
        //     if (response.Reason == "Unauthorized")
        //     {
        //         Log.LogWarning(methodName, "Check swift credentials : Unauthorized");
        //         throw new UnauthorizedAccessException();
        //     }
        //
        //     Log.LogCritical(methodName, $"Could not get file {fileName}, reason is {response.Reason}");
        //     throw new Exception($"Could not get file {fileName}");
        //
        // }

        public async Task PutFileToStorageSystem(string filePath, string fileName)
        {
            try
            {
                await PutFileToStorageSystem(filePath, fileName, 0).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
                // _swiftClient.AuthenticateAsyncV2(_keystoneEndpoint, _swiftUserName, _swiftPassword);
                await PutFileToStorageSystem(filePath, fileName, 0).ConfigureAwait(false);
            }
        }

        private async Task PutFileToStorageSystem(String filePath, string fileName, int tryCount)
        {
            // if (_swiftEnabled)
            // {
            //     await PutFileToSwiftStorage(filePath, fileName, tryCount).ConfigureAwait(false);
            // }

            if (_s3Enabled)
            {
                await PutFileToS3Storage(filePath, fileName, tryCount).ConfigureAwait(false);
            }
        }

// #pragma warning disable 1998
//         private async Task PutFileToSwiftStorage(string filePath, string fileName, int tryCount)
// #pragma warning restore 1998
//         {
//             string methodName = "Core.PutFileToSwiftStorage";
//             Log.LogStandard(methodName, $"Trying to upload file {fileName} to {_customerNo}_uploaded_data");
//             try
//             {
//                 var fileStream = new FileStream(filePath, FileMode.Open);
//
//                 SwiftBaseResponse response = _swiftClient
//                     .ObjectPutAsync(_customerNo + "_uploaded_data", fileName, fileStream).GetAwaiter().GetResult();
//
//                 if (!response.IsSuccess)
//                 {
//                     if (response.Reason == "Unauthorized")
//                     {
//                         fileStream.Close();
//                         await fileStream.DisposeAsync();
//                         Log.LogWarning(methodName, "Check swift credentials : Unauthorized");
//                         throw new UnauthorizedAccessException();
//                     }
//
//                     Log.LogWarning(methodName, $"Something went wrong, message was {response.Reason}");
//
//                     response = _swiftClient.ContainerPutAsync(_customerNo + "_uploaded_data").GetAwaiter().GetResult();
//                     if (response.IsSuccess)
//                     {
//                         response = _swiftClient
//                             .ObjectPutAsync(_customerNo + "_uploaded_data", fileName, fileStream).GetAwaiter().GetResult();
//                         if (!response.IsSuccess)
//                         {
//                             fileStream.Close();
//                             await fileStream.DisposeAsync();
//                             throw new Exception($"Could not upload file {fileName}");
//                         }
//                     }
//                 }
//
//                 fileStream.Close();
//                 await fileStream.DisposeAsync();
//             }
//             catch (FileNotFoundException ex)
//             {
//                 Log.LogCritical(methodName, $"File not found at {filePath}");
//                 Log.LogCritical(methodName, ex.Message);
//             }
//         }

        public async Task PutFileToS3Storage(Stream stream, string fileName)
        {
            string methodName = "Core.PutFileToS3Storage";
            string bucketName = await _sqlController.SettingRead(Settings.s3BucketName);
            Log.LogStandard(methodName, $"Trying to upload file {fileName} to {bucketName}");

            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName =
                    $"{await _sqlController.SettingRead(Settings.s3BucketName).ConfigureAwait(false)}",
                Key = $"{_customerNo}/{fileName}",
                InputStream = stream
            };
            try
            {
                await _s3Client.PutObjectAsync(putObjectRequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                putObjectRequest = new PutObjectRequest
                {
                    BucketName =
                        $"{await _sqlController.SettingRead(Settings.s3BucketName).ConfigureAwait(false)}",
                    Key = $"{_customerNo}/{fileName}",
                    InputStream = stream
                };
                try
                {
                    await _s3Client.PutObjectAsync(putObjectRequest).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log.LogWarning(methodName, $"Something went wrong, message was {ex.Message}");
                }

                Log.LogWarning(methodName, $"Something went wrong, message was {e.Message}");
            }
        }

        private async Task PutFileToS3Storage(string filePath, string fileName, int tryCount)
        {
            string methodName = "Core.PutFileToS3Storage";
            string bucketName = await _sqlController.SettingRead(Settings.s3BucketName);
            Log.LogStandard(methodName, $"Trying to upload file {fileName} to {bucketName}");

            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName =
                    $"{await _sqlController.SettingRead(Settings.s3BucketName).ConfigureAwait(false)}",
                Key = $"{_customerNo}/{fileName}",
                FilePath = filePath
            };
            try
            {
                await _s3Client.PutObjectAsync(putObjectRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogWarning(methodName, $"Something went wrong, message was {ex.Message}");
            }
        }

        public async Task<bool> CheckStatusByMicrotingUid(int microtingUid)
        {
            string methodName = "Core.CheckStatusByMicrotingUid";
            List<CaseDto> lstCase = new List<CaseDto>();
            MainElement mainElement = new MainElement();

            CaseDto concreteCase = await _sqlController.CaseReadByMUId(microtingUid).ConfigureAwait(false);
            Log.LogEverything(methodName, concreteCase + " has been matched");

            if (concreteCase.CaseUId == "" || concreteCase.CaseUId == "ReversedCase")
                lstCase.Add(concreteCase);
            else
                lstCase = await _sqlController.CaseReadByCaseUId(concreteCase.CaseUId).ConfigureAwait(false);

            foreach (CaseDto aCase in lstCase)
            {
                if (aCase.SiteUId == concreteCase.SiteUId)
                {
                    // get response's data and update DB with data
                    int? checkIdLastKnown = await _sqlController.CaseReadLastCheckIdByMicrotingUId(microtingUid)
                        .ConfigureAwait(false); //null if NOT a checkListSite
                    Log.LogVariable(methodName, nameof(checkIdLastKnown), checkIdLastKnown);

                    string respXml;
                    if (checkIdLastKnown == null)
                        respXml = await _communicator.Retrieve(microtingUid.ToString(), concreteCase.SiteUId)
                            .ConfigureAwait(false);
                    else
                        respXml = await _communicator
                            .RetrieveFromId(microtingUid.ToString(), concreteCase.SiteUId, checkIdLastKnown.ToString())
                            .ConfigureAwait(false);
                    Log.LogVariable(methodName, nameof(respXml), respXml);

                    Response resp = new Response();
                    resp = resp.XmlToClassUsingXmlDocument(respXml);
                    //resp = resp.XmlToClass(respXml);

                    if (resp.Type == Response.ResponseTypes.Success)
                    {
                        Log.LogEverything(methodName, "resp.Type == Response.ResponseTypes.Success (true)");
                        if (resp.Checks.Count > 0)
                        {
                            XmlDocument xDoc = new XmlDocument();

                            xDoc.LoadXml(respXml);
                            XmlNode checks = xDoc.DocumentElement.LastChild;
                            int i = 0;
                            foreach (Check check in resp.Checks)
                            {
                                int unitUId = _sqlController.UnitRead(int.Parse(check.UnitId)).GetAwaiter().GetResult()
                                    .UnitUId;
                                Log.LogVariable(methodName, nameof(unitUId), unitUId);
                                int workerUId = _sqlController.WorkerRead(int.Parse(check.WorkerId)).GetAwaiter()
                                    .GetResult().WorkerUId;
                                Log.LogVariable(methodName, nameof(workerUId), workerUId);

                                await _sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml, i)
                                    .ConfigureAwait(false);

                                await _sqlController.CaseUpdateCompleted(microtingUid, (int)check.Id,
                                    DateTime.Parse(check.Date), workerUId, unitUId).ConfigureAwait(false);
                                Log.LogEverything(methodName, "sqlController.CaseUpdateCompleted(...)");

                                // IF needed retract case, thereby completing the process
                                if (checkIdLastKnown == null)
                                {
                                    string responseRetractionXml = await _communicator
                                        .Delete(aCase.MicrotingUId.ToString(), aCase.SiteUId).ConfigureAwait(false);
                                    Response respRet = new Response();
                                    respRet = respRet.XmlToClass(respXml);

                                    if (respRet.Type == Response.ResponseTypes.Success)
                                    {
                                        Log.LogEverything(methodName, aCase + " has been retracted");
                                    }
                                    else
                                        Log.LogWarning(methodName,
                                            "Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" +
                                            aCase.SiteUId +
                                            ". Not a critical issue, but needs to be fixed if repeated");
                                }
                                //

                                await _sqlController.CaseRetract(microtingUid, (int)check.Id).ConfigureAwait(false);
                                Log.LogEverything(methodName, "sqlController.CaseRetract(...)");
                                // TODO add case.Id
                                CaseDto cDto = await _sqlController.CaseReadByMUId(microtingUid);
                                //InteractionCaseUpdate(cDto);
                                await FireHandleCaseCompleted(cDto).ConfigureAwait(false);
                                //try { HandleCaseCompleted?.Invoke(cDto, EventArgs.Empty); }
                                //catch { log.LogWarning(t.GetMethodName("Core"), "HandleCaseCompleted event's external logic suffered an Expection"); }
                                Log.LogStandard(methodName, cDto + " has been completed");
                                i++;
                            }
                        }
                    }
                    else
                    {
                        Log.LogEverything(methodName, "resp.Type == Response.ResponseTypes.Success (false)");
                        throw new Exception("Failed to retrive eForm " + microtingUid + " from site " + aCase.SiteUId);
                    }
                    //
                }
                else
                {
                    await CaseDelete((int)aCase.MicrotingUId).ConfigureAwait(false);
                }
            }

            return true;
        }
        //


        public List<KeyValuePair> PairRead(string str)
        {
            return _sqlController.PairRead(str);
        }

        // fireEvents

#pragma warning disable 1998
        public async Task FireHandleCaseCompleted(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseCompleted";
            Log.LogStandard(methodName, $"FireHandleCaseCompleted for MicrotingUId {caseDto.MicrotingUId}");
            try
            {
                HandleCaseCompleted?.Invoke(caseDto, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.LogWarning(methodName,
                    $"HandleCaseCompleted event's external logic suffered an Expection: {ex.Message}");
                throw;
            }
        }

#pragma warning disable 1998
        public async Task FireHandleCaseDeleted(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseDeleted";
            try
            {
                HandleCaseDeleted?.Invoke(caseDto, EventArgs.Empty);
            }
            catch
            {
                Log.LogWarning(methodName, "HandleCaseCompleted event's external logic suffered an Expection");
            }
        }

#pragma warning disable 1998
        public async Task FireHandleNotificationNotFound(NoteDto notification)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleNotificationNotFound";
            try
            {
                HandleNotificationNotFound?.Invoke(notification, EventArgs.Empty);
            }
            catch
            {
                Log.LogWarning(methodName, "HandleNotificationNotFound event's external logic suffered an Expection");
            }
        }

#pragma warning disable 1998
        public async Task FireHandleSiteActivated(NoteDto notification)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleSiteActivated";
            try
            {
                HandleSiteActivated?.Invoke(notification, EventArgs.Empty);
            }
            catch
            {
                Log.LogWarning(methodName, "HandleSiteActivated event's external logic suffered an Expection");
            }
        }

#pragma warning disable 1998
        public async Task FireHandleCaseProcessedByServer(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseProcessedByServer";
            Log.LogStandard(methodName, $"HandleCaseProcessedByServer for MicrotingUId {caseDto.MicrotingUId}");

            try
            {
                HandleeFormProcessedByServer?.Invoke(caseDto, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.LogWarning(methodName,
                    $"HandleCaseProcessedByServer event's external logic suffered an Expection: {ex.Message}");
                throw;
            }
        }

#pragma warning disable 1998
        public async Task FireHandleCaseProcessingError(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseProcessingError";
            Log.LogStandard(methodName, $"HandleCaseProcessingError for MicrotingUId {caseDto.MicrotingUId}");

            try
            {
                HandleeFormProsessingError?.Invoke(caseDto, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.LogWarning(methodName,
                    $"HandleCaseProcessingError event's external logic suffered an Expection: {ex.Message}");
                throw;
            }
        }

#pragma warning disable 1998
        public async Task FireHandleCaseRetrived(CaseDto caseDto)
#pragma warning restore 1998
        {
            string methodName = "Core.FireHandleCaseRetrived";
            Log.LogStandard(methodName, $"FireHandleCaseRetrived for MicrotingUId {caseDto.MicrotingUId}");

            try
            {
                HandleCaseRetrived?.Invoke(caseDto, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.LogWarning(methodName,
                    $"HandleCaseRetrived event's external logic suffered an Expection: {ex.Message}");
                throw;
            }
        }
        //
    }
}