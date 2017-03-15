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

using eFormData;
using eFormShared;

using System;
using System.Collections.Generic;

namespace eFormCore
{
    public interface ICore
    {
        event EventHandler  HandleCaseCreated;
        event EventHandler  HandleCaseRetrived;
        event EventHandler  HandleCaseCompleted;
        event EventHandler  HandleCaseDeleted;
        event EventHandler  HandleFileDownloaded;
        event EventHandler      HandleSiteActivated;

        event EventHandler  HandleEventLog;
        event EventHandler  HandleEventMessage;
        event EventHandler  HandleEventWarning;
        event EventHandler  HandleEventException;

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Starts the Core and enables Events. Restarts if needed
        /// </summary>
        bool Start(string serverConnectionString);

        /// <summary>
        /// Closes the Core and disables Events
        /// </summary>
        bool Close();

        /// <summary>
        /// Tells if the Core is useable
        /// </summary>
        bool Running();

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Converts XML from ex. eForm Builder or other sources, into a MainElement
        /// </summary>
        /// <param name="xmlString">XML string to be converted</param>
        MainElement     TemplateFromXml(string xmlString);

        List<string>    TemplateValidation(MainElement mainElement);

        /// <summary>
        /// Tries to create an eForm template in the Microting local DB. Returns that templat's templatId
        /// </summary>
        /// <param name="mainElement">Templat MainElement to be inserted into the Microting local DB</param>
        int             TemplateCreate(MainElement mainElement);

        /// <summary>
        /// Tries to retrieve the template MainElement from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        MainElement     TemplateRead(int templateId);

        /// <summary>
        /// Tries to retrieve the template meta data from the Microting DB
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        Template_Dto    TemplateItemRead(int templateId);

        /// <summary>
        /// Tries to retrieve all templates meta data from the Microting DB
        /// </summary>
        /// <param name="includeRemoved">Filters list to only show all active or all including removed</param>
        List<Template_Dto> TemplateItemReadAll(bool includeRemoved);

        /// <summary>
        /// Tries to delete an eForm template in the Microting local DB. Returns if it was successfully
        /// </summary>
        /// <param name="templateId">Template MainElement's ID to be retrieved from the Microting local DB</param>
        bool            TemplateDelete(int templateId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Tries to create an eForm case(s) in the Microting local DB, and creates it in the Microting system
        /// </summary>
        /// <param name="mainElement">The templat MainElement the case(s) will be based on</param>
        /// <param name="caseUId">NEEDS TO BE UNIQUE IF ASSIGNED. The unique identifier that you can assign yourself to the set of case(s)</param>
        /// <param name="siteId">siteId that the case will be sent to</param>
        /// 
        string          CaseCreate(MainElement mainElement, string caseUId, int siteId);

        /// <summary>
        /// Tries to create an eForm case(s) in the Microting local DB, and creates it in the Microting system, with extended parameters
        /// </summary>
        /// <param name="mainElement">The templat MainElement the case(s) will be based on</param>
        /// <param name="caseUId">NEEDS TO BE UNIQUE IF ASSIGNED. The unique identifier that you can assign yourself to the set of case(s)</param>
        /// <param name="siteIds">List of siteIds that case(s) will be sent to</param>
        /// <param name="custom">Custom extended parameter</param>
        /// <param name="reversed">Default is false. If true, cases will not be created until a check has been completed</param>
        List<string>    CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, string custom, bool reversed);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Tries to retrieve the status of a case
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        string          CaseCheck(string microtingUId);

        /// <summary>
        /// Tries to retrieve the answered full case from the DB
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        /// <param name="checkUId">If left empty, "0" or NULL it will try to retrieve the first check. Alternative is stating the Id of the specific check wanted to retrieve</param>
        ReplyElement    CaseRead(string microtingUId, string checkUId);

        /// <summary>
        /// Tries to set the resultats of a case to new values
        /// </summary>
        /// <param name="newFieldValuePairLst">List of '[fieldValueId]|[new value]'</param>
        /// <param name="newCheckListValuePairLst">List of '[checkListValueId]|[new status]'</param>
        bool            CaseUpdate(int caseId, List<string> newFieldValuePairLst, List<string> newCheckListValuePairLst);

        /// <summary>
        /// Marks a case as deleted, and will remove it from the device, if needed
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        bool            CaseDelete(string microtingUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Looks up the case's markers, from the match
        /// </summary>
        /// <param name="microtingUId">Microting unique ID of the eForm case</param>
        Case_Dto        CaseLookupMUId(string microtingUId);

        /// <summary>
        /// Looks up the case's markers, from the match
        /// </summary>
        /// <param name="CaseId">Microting DB's ID of the eForm case</param>
        Case_Dto        CaseLookupCaseId(int CaseId);

        /// <summary>
        /// Looks up the case's markers, from the matches
        /// </summary>
        /// <param name="caseUId">Case's unique ID of the set of case(s)</param>
        List<Case_Dto>  CaseLookupCaseUId(string caseUId);

        /// <summary>
        /// Looks up the case's ID, from the match
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        /// <param name="checkUId">If left empty, "0" or NULL it will try to retrieve the first check. Alternative is stating the Id of the specific check wanted to retrieve</param>
        int             CaseIdLookup(string microtingUId, string checkUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Tries to retrieve all connected cases to a templat, and delivers them as a Excel fil, at the returned path's location
        /// </summary>
        /// <param name="templateId">The templat's ID to be used. Null will remove this limit</param>
        /// <param name="start">Only cases from after this time limit. Null will remove this limit</param>
        /// <param name="end">Only cases from before this time limit. Null will remove this limit</param>
        /// <param name="pathAndName">Location where fil is to be placed, along with fil name. No extension needed. Relative or absolut. WARNING: Excel might use its default location</param>
        string          CasesToExcel(int? templateId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData);

        /// <summary>
        /// Tries to retrieve all connected cases to a templat, and delivers them as a CSV fil, at the returned path's location
        /// </summary>
        /// <param name="templateId">The templat's ID to be used. Null will remove this limit</param>
        /// <param name="start">Only cases from after this time limit. Null will remove this limit</param>
        /// <param name="end">Only cases from before this time limit. Null will remove this limit</param>
        /// <param name="pathAndName">Location where fil is to be placed, along with fil name. No extension needed. Relative or absolut</param>
        string          CasesToCsv(int? templateId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Tries to create an site in the Microting local DB and Microting API. Returns that site's data
        /// </summary>
        /// <param name="name">Site's name to be created</param>
        /// <param name="userFirstName">Site user's first name to be created</param>
        /// <param name="userLastName">Site user's last name to be created</param>
        /// <param name="userEmail">Site user's email to be created</param>
        Site_Dto        SiteCreate(string name, string userFirstName, string userLastName, string userEmail);

        /// <summary>
        /// Tries to retrieve the site data from the Microting DB
        /// </summary>
        /// <param name="siteId">Site ID to be retrieved from the Microting local DB</param>
        Site_Dto        SiteRead(int siteId);

        /// <summary>
        /// Tries to retrieve all sites data from the Microting DB
        /// </summary>
        /// <param name="includeRemoved">Filters list to only show all active or all including removed</param>
        List<Site_Dto>  SiteReadAll(bool includeRemoved);

        /// <summary>
        /// Tries to retrieve reset the site's access details
        /// </summary>
        /// <param name="siteId">Site ID to be reset</param>
        Site_Dto        SiteReset(int siteId);

        /// <summary>
        /// Tries to update a site's data in the Microting local DB and Microting API. Returns that site's data
        /// </summary>
        /// <param name="siteId">Site ID of the site to be updated</param>
        /// <param name="name">Site's name to be updated</param>
        /// <param name="userFirstName">Site user's first name to be updated</param>
        /// <param name="userLastName">Site user's last name to updated</param>
        /// <param name="userEmail">Site user's email to be updated</param>
        bool            SiteUpdate(int siteId, string name, string userFirstName, string userLastName, string userEmail);

        /// <summary>
        /// Tries to delete the site
        /// </summary>
        /// <param name="siteId">Site ID to be deleted</param>
        bool            SiteDelete(int siteId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Creates an EntityGroup, and returns its unique microting id for further use
        /// </summary>
        /// <param name="entityType">Entity type, either "EntitySearch" or "EntitySelect"</param>
        /// <param name="name">Templat MainElement's ID to be retrieved from the Microting local DB</param>
        string EntityGroupCreate(string entityType, string name);

        /// <summary>
        /// Returns the EntityGroup and its EntityItems
        /// </summary>
        /// <param name="entityGroupMUId">The unique microting id of the EntityGroup</param>
        EntityGroup     EntityGroupRead(string entityGroupMUId);

        /// <summary>
        /// Updates the EntityGroup and its EntityItems for those needed
        /// </summary>
        /// <param name="entityGroup">The EntityGroup and its EntityItems</param>
        bool            EntityGroupUpdate(EntityGroup entityGroup);

        /// <summary>
        /// Deletes an EntityGroup, both its items should be deleted before using
        /// </summary>
        /// <param name="entityGroupMUId">The unique microting id of the EntityGroup</param>
        bool            EntityGroupDelete(string entityGroupMUId);
    }
}