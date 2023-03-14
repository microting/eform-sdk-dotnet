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

using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microting.eForm.Communication
{
    internal interface IHttp
    {
        Task<string> Post(string xmlData, string siteId, string contentType = "application/xml");

        Task<string> Status(string elementId, string siteId);

        Task<string> Retrieve(string microtingUuid, string microtingCheckUuid, int siteId);

        Task<string> Delete(string elementId, string siteId);

        Task<string> EntitySearchGroupCreate(string name, string id);

        Task<bool> EntitySearchGroupUpdate(int id, string name, string entityGroupMUId);

        Task<bool> EntitySearchGroupDelete(string entityGroupId);

        Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id);

        Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name,
            string description, string id);

        Task<bool> EntitySearchItemDelete(string entitySearchItemId);

        Task<string> EntitySelectGroupCreate(string name, string id);

        Task<bool> EntitySelectGroupUpdate(int id, string name, string entityGroupMUId);

        Task<bool> EntitySelectGroupDelete(string entityGroupId);

        Task<string> EntitySelectItemCreate(string entitySelectGroupId, string name, int displayOrder, string id);

        Task<bool> EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name,
            int displayOrder, string id);

        Task<bool> EntitySelectItemDelete(string entitySelectItemId);

        Task<bool> PdfUpload(string name, string hash);
        Task<bool> PdfUpload(Stream stream, string hash, string fileName);

        Task<string> TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex);

        Task<string> SiteCreate(string name, string languageCode);

        Task<bool> SiteUpdate(int id, string name, string languageCode);

        Task<string> SiteDelete(int id);

        Task<string> SiteLoadAllFromRemote();

        Task<string> WorkerCreate(string firstName, string lastName, string email);

        Task<bool> WorkerUpdate(int id, string firstName, string lastName, string email);

        Task<string> WorkerDelete(int id);

        Task<string> WorkerLoadAllFromRemote();

        Task<string> SiteWorkerCreate(int siteId, int workerId);

        Task<string> SiteWorkerDelete(int id);

        Task<string> SiteWorkerLoadAllFromRemote();

        Task<string> FolderLoadAllFromRemote();

        Task<string> FolderCreate(int uuid, int? parentId);

        Task<bool> FolderUpdate(int id, string name, string description, string languageCode, int? parentId);

        Task<string> FolderDelete(int id);

        Task<string> UnitUpdate(int id, bool newOtp, int siteId, bool pushEnabled, bool syncDelayEnabled,
            bool syncDialogEnabled);

        Task<string> UnitLoadAllFromRemote();

        Task<string> UnitDelete(int id);

        Task<string> UnitMove(int unitId, int siteId);

        Task<string> UnitCreate(int siteMicrotingUid);

        Task<string> OrganizationLoadAllFromRemote();

        Task<int> SpeechToText(Stream pathToAudioFile, string language, string extension);

        Task<JToken> SpeechToText(int requestId);

        Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite);

        Task<string> GetAllSurveyConfigurations();

        Task<string> GetSurveyConfiguration(int id);

        Task<string> GetAllQuestionSets();

        Task<string> GetQuestionSet(int id);

        Task<string> GetLastAnswer(int questionSetId, int lastAnswerId);

        Task SendPushMessage(int microtingSiteId, string header, string body, int microtingUuid);
    }
}