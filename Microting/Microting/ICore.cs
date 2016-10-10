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
        event EventHandler  HandleEventWarning;
        event EventHandler  HandleEventException;


        void        Start();
        void        Close();

        int             TemplatCreate(string xmlString);
        int             TemplatCreate(MainElement mainElement);

        MainElement     TemplatRead(int templatId);

        void            CaseCreateOneSite(MainElement mainElement, string caseUId, string caseType, int siteId);
        void            CaseCreateSomeSites(MainElement mainElement, string caseUId, string caseType, List<int> siteIds);
        void            CaseCreateAllSites(MainElement mainElement, string caseUId, string caseType);
        void                CaseCreateAllSitesExtended(MainElement mainElement, string caseUId, string caseType, DateTime navisionTime, string numberPlate, string roadNumber);

        ReplyElement    CaseRead(string microtingUId);
        ReplyElement    CaseReadAllSites(string caseUId);

        Case_Dto        CaseLookup(string microtingUId);
        List<Case_Dto>  CaseLookupAllSites(string caseUId);

        bool            CaseDelete(string microtingUId);
        int             CaseDeleteAllSites(string caseUId);
    }
}