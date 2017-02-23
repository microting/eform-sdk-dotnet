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

using eFormShared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace eFormCommunicator
{
    public class BasicCom
    {
        string comAddressBasic;

        #region con
        public BasicCom(string comAddressBasic)
        {
            this.comAddressBasic = comAddressBasic;
        }
        #endregion

        #region public
        public List<string>     GetSettings(string token)
        {
            string[] lines = new string[] { };
            try
            {
                // This line is here for "normal" programs.
                lines = File.ReadAllLines(@"input\first_run.txt");
            }
            catch (Exception)
            {
                try
                {
                    // This line is here because the core might get startet inside an web app, therefore the first file location is to ambiguos.
                    lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"bin\input\first_run.txt");
                }
                catch (Exception ex)
                {
                    throw new Exception(@"input\first_run.txt not found", ex);
                }
            }





            if (!bool.Parse(SettingRead("firstRunDone")))
            {
                string[] lines = new string[] { };
                try
                {
                    // This line is here for "normal" programs.
                    lines = File.ReadAllLines(@"input\first_run.txt");
                }
                catch (Exception)
                {
                    try
                    {
                        // This line is here because the core might get startet inside an web app, therefore the first file location is to ambiguos.
                        lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"bin\input\first_run.txt");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(@"input\first_run.txt not found", ex);
                    }
                }
                string name;
                string value;

                foreach (var item in lines)
                {
                    string[] line = item.Split('|');

                    name = line[0];
                    value = line[1];

                    SettingUpdate(name, value);
                }
                SettingUpdate("logLevel", "true");
            }







            return null;
        }

        //public Organization_Dto GetOrganization()
        //{
        //    JToken orgResult = JRaw.Parse(http.OrganizationLoadAllFromRemote());

        //    Organization_Dto organizationDto = new Organization_Dto(int.Parse(orgResult.First.First["id"].ToString()),
        //        orgResult.First.First["name"].ToString(),
        //        int.Parse(orgResult.First.First["customer_no"].ToString()),
        //        int.Parse(orgResult.First.First["unit_license_number"].ToString()));

        //    return organizationDto;
        //}
        #endregion

        #region remove unwanted/uneeded methods from finished DLL
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
    }
}
