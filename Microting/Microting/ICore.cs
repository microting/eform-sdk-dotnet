using System;
using System.Collections.Generic;
using eFormRequest;
using eFormSqlController;

namespace Microting
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
        void Start(string serverConnectionString);

        /// <summary>
        /// Closes the Core and disables Events
        /// </summary>
        void Close();

        /// <summary>
        /// Tells if the Core is useable
        /// </summary>
        bool Running();

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Converts XML from ex. eForm Builder or other sources, into a MainElement
        /// </summary>
        /// <param name="xmlString">XML string to be converted</param>
        MainElement     TemplatFromXml(string xmlString);

        /// <summary>
        /// Tries to create an eForm template in the Microting local DB. Returns that templat's templatId
        /// </summary>
        /// <param name="mainElement">Templat MainElement to be inserted into the Microting local DB</param>
        int             TemplatCreate(MainElement mainElement);

        /// <summary>
        /// Tries to retrieve the templat MainElement from the Microting DB
        /// </summary>
        /// <param name="templatId">Templat MainElement's ID to be retrieved from the Microting local DB</param>
        MainElement     TemplatRead(int templatId);

        List<MainElement> TemplatReadAll();

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
        /// Tries to retrieve the answered full case from the DB
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        /// <param name="checkUId">If left empty, "0" or NULL it will try to retrieve the first check. Alternative is stating the Id of the specific check wanted to retrieve</param>
        ReplyElement    CaseRead(string microtingUId, string checkUId);

        /// <summary>
        /// Tries to set the resultats of a case to new values
        /// </summary>
        /// <param name="newValueList">List of '[fieldId]|[new value]'</param>
        bool            CaseUpdate(int caseId, List<string> newValueList);

        /// <summary>
        /// Marks a case as deleted, and will remove it from the device, if needed
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        bool            CaseDelete(string microtingUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Looks up the case's markers, from the matches
        /// </summary>
        /// <param name="microtingUId">Microting unique ID of the eForm case</param>
        Case_Dto        CaseLookupMUId(string microtingUId);

        /// <summary>
        /// Looks up the case's markers, from the matches
        /// </summary>
        /// <param name="CaseId">Microting DB's ID of the eForm case</param>
        Case_Dto        CaseLookupCaseId(int CaseId);

        /// <summary>
        /// Looks up the case's markers, from the matches
        /// </summary>
        /// <param name="caseUId">Case's unique ID of the set of case(s)</param>
        List<Case_Dto>  CaseLookupCaseUId(string caseUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Creates an EntityGroup, and returns its unique microting id for further use
        /// </summary>
        /// <param name="entityType">Entity type, either "EntitySearch" or "EntitySelect"</param>
        /// <param name="name">Templat MainElement's ID to be retrieved from the Microting local DB</param>
        string          EntityGroupCreate(string entityType, string name);

        /// <summary>
        /// Returns the EntityGroup and its EntityItems
        /// </summary>
        /// <param name="entityGroupMUId">The unique microting id of the EntityGroup</param>
        EntityGroup     EntityGroupRead(string entityGroupMUId);

        /// <summary>
        /// Updates the EntityGroup and its EntityItems for those needed
        /// </summary>
        /// <param name="entityGroup">The EntityGroup and its EntityItems</param>
        void            EntityGroupUpdate(EntityGroup entityGroup);

        /// <summary>
        /// Deletes an EntityGroup, both its items should be deleted before using
        /// </summary>
        /// <param name="entityGroupMUId">The unique microting id of the EntityGroup</param>
        void            EntityGroupDelete(string entityGroupMUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Tries to retrieve all connected cases to a templat, and delivers them as a Excel fil, at the returned path's location
        /// </summary>
        /// <param name="templatId">The templat's ID to be used</param>
        /// <param name="start">Only cases from after this time limit. Null will remove this limit</param>
        /// <param name="end">Only cases from before this time limit. Null will remove this limit</param>
        /// <param name="path">Location where fil is to be placed. Relative or absolut. WARNING: Excel might its default location</param>
        /// <param name="name">Name of the Excel fil</param>
        string          CasesToExcel(int templatId, DateTime? start, DateTime? end, string fullPathName);

        /// <summary>
        /// Tries to retrieve all connected cases to a templat, and delivers them as a CSV fil, at the returned path's location
        /// </summary>
        /// <param name="templatId">The templat's ID to be used</param>
        /// <param name="start">Only cases from after this time limit. Null will remove this limit</param>
        /// <param name="end">Only cases from before this time limit. Null will remove this limit</param>
        /// <param name="path">Location where fil is to be placed. Relative or absolut</param>
        /// <param name="name">Name of the CSV fil</param>
        string          CasesToCsv(int templatId, DateTime? start, DateTime? end, string fullPathName);
    }
}