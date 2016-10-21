using System;
using System.Collections.Generic;
using eFormRequest;
using eFormSqlController;

namespace Microting
{
    interface ICore
    {
        event EventHandler  HandleCaseCreated;
        event EventHandler  HandleCaseRetrived;
        event EventHandler  HandleCaseUpdated;
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
        /// <param name="caseUId">NEEDS TO BE UNIQUE. The unique identifier that you can assign yourself to the case(s)</param>
        /// <param name="siteIds">xxx</param>
        /// <param name="reversed">xxx</param>
        void CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, bool reversed);
 
        /// <summary>
        /// xxx
        /// </summary>
        /// <param name="mainElement">xxx</param>
        /// <param name="caseUId">xxx</param>
        /// <param name="siteIds">xxx</param>
        /// <param name="reversed">xxx</param>
        /// <param name="navisionTime">xxx</param>
        /// <param name="numberPlate">xxx</param>
        /// <param name="roadNumber">xxx</param>
        void CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, bool reversed, DateTime navisionTime, string numberPlate, string roadNumber);

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
        /// <param name="caseUId">Case's unique ID of the eForm case</param>
        ReplyElement CaseReadAllSites(string caseUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// xxx
        /// </summary>
        /// <param name="microtingUId">xxx</param>
        Case_Dto CaseLookup(string microtingUId);

        /// <summary>
        /// xxx
        /// </summary>
        /// <param name="caseUId">xxx</param>
        List<Case_Dto> CaseLookupAllSites(string caseUId);

        //---------------------------------------------------------------------------------------

        /// <summary>
        /// xxx
        /// </summary>
        /// <param name="microtingUId">xxx</param>
        bool CaseDelete(string microtingUId);

        /// <summary>
        /// xxx
        /// </summary>
        /// <param name="caseUId">xxx</param>
        int CaseDeleteAllSites(string caseUId);
    }
}