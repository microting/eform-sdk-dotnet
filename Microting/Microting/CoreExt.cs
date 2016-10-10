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

using eFormCommunicator;
using eFormRequest;
using eFormResponse;
using eFormSubscriber;
using eFormSqlController;
using Trools;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;

namespace Microting
{
    class CoreExt
    {
        Core core;
        string connectionStr;

        public CoreExt (Core core)
        {
            this.core = core;
            connectionStr = core.serverConnectionString;
        }

        public void CreateInfinityCase(MainElement mainElement, List<int> siteIds, int instances)
        {
            if (mainElement.Repeated > 0)
                throw new Exception("InfinityCase are always Repeated < 1");

            int templatId = core.TemplatCreate(mainElement);

            CreateInfinityCaseMethod(templatId, siteIds, instances);
        }

        private void CreateInfinityCaseMethod(int templatId, List<int> siteIds, int instances)
        {
            int muuId = -1;
            Transmitter transmitter = new Transmitter(core.comToken, core.comAddress);


            foreach (int site in siteIds)
            {
                for (int i = 0; i < instances; i++)
                {
                    //string reply = transmitter.PostXml(mainElement.ClassToXml(), siteId);

                    //Response response = new Response();
                    //response = response.XmlToClass(reply);

                    ////trace msg HandleEvent(reply);
                    ////if reply is "success", it's created
                    //if (response.Type.ToString().ToLower() == "success")
                    //{
                    //    return response.Value;
                    //}
                    ////post
                    ////create 
                }
            }
        }


        private void CreateSql()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //var aCase = db.cases.Where(x => x.microting_api_id == microtingUId).ToList().First();
                    //int caseId = aCase.id;

                    //List<field_values> lst = db.field_values.Where(x => x.case_id == caseId).ToList();
                    //return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateSql failed", ex);
            }
        }
    }
}
