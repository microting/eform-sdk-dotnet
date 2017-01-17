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
        void Start();

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
        MainElement TemplatFromXml(string xmlString);

        /// <summary>
        /// Tries to create an eForm template in the Microting local DB. Returns that templat's templatId
        /// </summary>
        /// <param name="mainElement">Templat MainElement to be inserted into the Microting local DB</param>
        int TemplatCreate(MainElement mainElement);

        /// <summary>
        /// Tries to retrieve the templat MainElement from the Microting DB
        /// </summary>
        /// <param name="templatId">Templat MainElement's ID to be retrieved from the Microting local DB</param>
        MainElement TemplatRead(int templatId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Tries to create an eForm case(s) in the Microting local DB, and creates it in the Microting system
        /// </summary>
        /// <param name="mainElement">The templat MainElement the case(s) will be based on</param>
        /// <param name="caseUId">NEEDS TO BE UNIQUE IF ASSIGNED. The unique identifier that you can assign yourself to the set of case(s)</param>
        /// <param name="siteId">siteId that the case will be sent to</param>
        /// 
        void CaseCreate(MainElement mainElement, string caseUId, int siteId);

        /// <summary>
        /// Tries to create an eForm case(s) in the Microting local DB, and creates it in the Microting system, with extended parameters
        /// </summary>
        /// <param name="mainElement">The templat MainElement the case(s) will be based on</param>
        /// <param name="caseUId">NEEDS TO BE UNIQUE IF ASSIGNED. The unique identifier that you can assign yourself to the set of case(s)</param>
        /// <param name="siteIds">List of siteIds that case(s) will be sent to</param>
        /// <param name="custom">Custom extended parameter</param>
        /// <param name="reversed">Default is false. If true, cases will not be created until a check has been completed</param>
        void CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, string custom, bool reversed);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Tries to retrieve the answered full case from the DB
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        /// <param name="checkUId">If left empty, "0" or NULL it will try to retrieve the first check. Alternative is stating the Id of the specific check wanted to retrieve</param>
        ReplyElement CaseRead(string microtingUId, string checkUId);

        /// <summary>
        /// Tries to retrieve the answered full case in the DB, from the set
        /// </summary>
        /// <param name="caseUId">Case's unique ID of the set of case(s)</param>
        ReplyElement CaseReadAllSites(string caseUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Looks up a case's markers
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        Case_Dto CaseLookup(string microtingUId);

        /// <summary>
        /// Looks up a case's markers, from the set
        /// </summary>
        /// <param name="caseUId">Case's unique ID of the set of case(s)</param>
        List<Case_Dto> CaseLookupAllSites(string caseUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Marks a case as deleted, and will remove it from the device, if needed
        /// </summary>
        /// <param name="microtingUId">Microting ID of the eForm case</param>
        bool CaseDelete(string microtingUId);

        /// <summary>
        /// Marks a set of case(s) as deleted, and will remove it/them from the device(s), if needed
        /// </summary>
        /// <param name="caseUId">Case's unique ID of the set of case(s)</param>
        int CaseDeleteAllSites(string caseUId);


        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Creates an EntityGroup, and returns its unique microting id for further use
        /// </summary>
        /// <param name="entityType">Entity type, either "EntitySearch" or "EntitySelect"</param>
        /// <param name="name">Templat MainElement's ID to be retrieved from the Microting local DB</param>
        string EntityGroupCreate(string entityType, string name);

        EntityGroup EntityGroupRead(string entityGroupMUId);

        void EntityGroupUpdate(EntityGroup entityGroup);

        void EntityGroupDelete(string entityGroupMUId);
    }
}