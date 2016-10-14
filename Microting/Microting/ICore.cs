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


        void            Start();
        void            Close();

        int             TemplatCreate(string xmlString, string caseType);
        int             TemplatCreate(MainElement mainElement, string caseType);

        MainElement     TemplatRead(int templatId);

        void            CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, bool reversed);
        void            CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, bool reversed, DateTime navisionTime, string numberPlate, string roadNumber);

        ReplyElement    CaseRead(string microtingUId);
        ReplyElement    CaseReadAllSites(string caseUId);

        Case_Dto        CaseLookup(string microtingUId);
        List<Case_Dto>  CaseLookupAllSites(string caseUId);

        bool            CaseDelete(string microtingUId);
        int             CaseDeleteAllSites(string caseUId);
    }
}