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
using System.Linq;
using System.Threading.Tasks;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Newtonsoft.Json.Linq;

namespace Microting.eForm.Helpers
{
    public class TestHelperReturnXML
    {

        private TestHelpers testHelpers;
        Tools t = new Tools();

        public TestHelperReturnXML()
        {        Tools t = new Tools();
            testHelpers = new TestHelpers();

        }

        #region "prep sever information for full load"
        public async Task<string> CreateSiteUnitWorkersForFullLoaed(bool create)
        {
            if (create)
            {
                int id = t.GetRandomInt(6);
                string siteName = Guid.NewGuid().ToString();
                sites site = await testHelpers.CreateSite(siteName, id);

                id = t.GetRandomInt(6);
                string userFirstName = Guid.NewGuid().ToString();
                string userLastName = Guid.NewGuid().ToString();
                workers worker = await testHelpers.CreateWorker("sfsdfsdf23ref@invalid.com", userFirstName, userLastName, id);
                return "";
            }
            
            try
            {
                sites site = testHelpers.dbContext.sites.First();
                workers worker = testHelpers.dbContext.workers.First();

                int id = t.GetRandomInt(6);
                JObject result = JObject.FromObject(new JArray(new
                {
                    id = id, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z",
                    workflow_state = Constants.WorkflowStates.Created, person_type = "",
                    site_id = site.MicrotingUid, user_id = worker.MicrotingUid
                }));
                return result.ToString();
            }
            catch
            {
                return "{}";
            }
        }
        #endregion

        #region
        public async Task<string> CreateMultiPictureXMLResult(bool create)
        {
            if (create)
            {
                sites site = await testHelpers.CreateSite("TestSite1", 12334);
                units unit = await testHelpers.CreateUnit(20934, 234234, site, 24234);
                workers worker = await testHelpers.CreateWorker("sfsdfsdf23ref@invalid.com", "John", "Doe", 2342341);
                site_workers sw = await testHelpers.CreateSiteWorker(242345, site, worker);
                DateTime cl1_ca = DateTime.UtcNow;
                DateTime cl1_ua = DateTime.UtcNow;
                check_lists cl1 = await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "MultiPictureXMLResult", "MultiPictureXMLResult_Description", "", "", 0, 0);
                check_lists cl2 = await testHelpers.CreateSubTemplate("Sub1", "Sub1Description", "", 0, 0, cl1);
                fields f1 = await testHelpers.CreateField(0, "", cl2, Constants.FieldColors.Blue, "", null, "", "PictureDescription", 0, 0, testHelpers.dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 0, 0, "Take picture", 0, 0, "", "", 0, 0, "", 0, 0, 0, 0, "", 0);
                check_list_sites cls = await testHelpers.CreateCheckListSite(cl1, cl1_ca, site, cl1_ua, 0, Constants.WorkflowStates.Created, 555);
                //returnXML = ;
                return "555";
//                return 12;
            }
            else
            {
                sites site = testHelpers.dbContext.sites.First();
                units unit = testHelpers.dbContext.units.First();
                workers worker = testHelpers.dbContext.workers.First();
                check_lists cl1 = testHelpers.dbContext.check_lists.ToList()[0];
                check_lists cl2 = testHelpers.dbContext.check_lists.ToList()[1];
                fields f1 = testHelpers.dbContext.fields.First();

                #region return xml
                return $@"<?xml version='1.0' encoding='UTF-8'?>
                <Response>
                    <Value type='success'>555</Value>
                    <Checks>
                        <Check worker='John Doe' worker_id='{worker.MicrotingUid}' date='2018-04-25 14:29:21 +0200' unit_id='{unit.MicrotingUid}' id='7'>
                            <ElementList>
                                <Id>{cl2.Id}</Id>
                                <Status>approved</Status>
                                <DataItemList>
                                    <DataItem>
                                        <Id>{f1.Id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                </DataItemList>
                                <ExtraDataItemList></ExtraDataItemList>
                            </ElementList>
                        </Check>
                        <Check worker='John Doe' worker_id='{worker.MicrotingUid}' date='2018-04-25 14:29:52 +0200' unit_id='{unit.MicrotingUid}' id='12'>
                            <ElementList>
                                <Id>{cl2.Id}</Id>
                                <Status>approved</Status>
                                <DataItemList>
                                    <DataItem>
                                        <Id>{f1.Id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                    <DataItem>
                                        <Id>{f1.Id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                </DataItemList>
                                <ExtraDataItemList></ExtraDataItemList>
                            </ElementList>
                        </Check>
                        <Check worker='John Doe' worker_id='{worker.MicrotingUid}' date='2018-04-25 14:39:43 +0200' unit_id='{unit.MicrotingUid}' id='17'>
                            <ElementList>
                                <Id>{cl2.Id}</Id>
                                <Status>approved</Status>
                                <DataItemList>
                                    <DataItem>
                                        <Id>{f1.Id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                    <DataItem>
                                        <Id>{f1.Id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                </DataItemList>
                                <ExtraDataItemList></ExtraDataItemList>
                            </ElementList>
                        </Check>
                    </Checks>
                </Response>".Replace("'", "\"");
                #endregion
            }


        }
        #endregion
    }
}
