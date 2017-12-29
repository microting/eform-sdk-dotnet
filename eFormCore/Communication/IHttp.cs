namespace eFormCommunicator
{
     internal interface IHttp
     {
         string Post(string xmlData, string siteId);

         string Status(string elementId, string siteId);

         string Retrieve(string microtingUuid, string microtingCheckUuid, int siteId);

         string Delete(string elementId, string siteId);

         string EntitySearchGroupCreate(string name, string id);

         bool EntitySearchGroupUpdate(int id, string name, string entityGroupMUId);

         bool EntitySearchGroupDelete(string entityGroupId);

         string EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id);

         bool EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id);

         bool EntitySearchItemDelete(string entitySearchItemId);

         string EntitySelectGroupCreate(string name, string id);

         bool EntitySelectGroupUpdate(int id, string name, string entityGroupMUId);

         bool EntitySelectGroupDelete(string entityGroupId);

         string EntitySelectItemCreate(string entitySelectGroupId, string name, int displayOrder, string id);

         bool EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name, int displayOrder, string id);

         bool EntitySelectItemDelete(string entitySelectItemId);

         bool PdfUpload(string name, string hash);

         string TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex);

         string SiteCreate(string name);

         bool SiteUpdate(int id, string name);

         string SiteDelete(int id);

         string SiteLoadAllFromRemote();

         string WorkerCreate(string firstName, string lastName, string email);

         bool WorkerUpdate(int id, string firstName, string lastName, string email);

         string WorkerDelete(int id);

         string WorkerLoadAllFromRemote();

         string SiteWorkerCreate(int siteId, int workerId);

         string SiteWorkerDelete(int id);

         string SiteWorkerLoadAllFromRemote();

        int UnitRequestOtp(int id);

        string UnitLoadAllFromRemote();

        string OrganizationLoadAllFromRemote();
    }
}