/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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

using Newtonsoft.Json.Linq;

namespace Microting.eForm.Communication
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

        string FolderLoadAllFromRemote();

        string FolderCreate(string name, string description, int? parent_id);

        bool FolderUpdate(int id, string name, string description, int? parent_id);

        string FolderDelete(int id);

        int UnitRequestOtp(int id);

        string UnitLoadAllFromRemote();

        string UnitDelete(int id);

        string OrganizationLoadAllFromRemote();

        int SpeechToText(string pathToAudioFile, string language);

        JToken SpeechToText(int requestId);

    }
}