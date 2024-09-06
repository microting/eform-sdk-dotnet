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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using Field = Microting.eForm.Infrastructure.Data.Entities.Field;

namespace eFormSDK.Integration.CheckLists.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class CoreTesteFormFromXMLLarge : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");

            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            await sut.StartSqlOnly(ConnectionString);
            path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            //sut.StartLog(new CoreBase());
        }


        [Test] // Core_Template_TemplateFromXml_ReturnsTemplate()
        public async Task Core_eForm_LargeeFormFromXML_ReturnseMainElement()
        {
            // Arrange

            #region Arrance

            string xmlstring = await File.ReadAllTextAsync(@"LargeXml.xml");

            #endregion

            // Act
            MainElement mainelement = await sut.TemplateFromXml(xmlstring);
            var match = await sut.TemplateCreate(mainelement);

            List<CheckList> listOfCL = await DbContext.CheckLists.AsNoTracking().ToListAsync();
            List<Field> listOfFields = await DbContext.Fields.ToListAsync();
            List<CheckListTranslation> checkLisTranslations = await DbContext.CheckListTranslations.ToListAsync();
            List<FieldTranslation> fieldTranslations = await DbContext.FieldTranslations.ToListAsync();

            // Assert
            Assert.That(mainelement, Is.Not.EqualTo(null));
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(listOfCL.Count(), Is.EqualTo(15));
            Assert.That(listOfFields.Count, Is.EqualTo(681));

            #region Checklist

            #region Label

            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("UK"));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Stamdata og gummioplysninger. Husk gummiænder!"));
            Assert.That(checkLisTranslations[2].Text, Is.EqualTo("Gennemgang af damme: Gæs på ejendommen "));
            Assert.That(checkLisTranslations[3].Text, Is.EqualTo("Gennemgang af damanlæg: hotwings på ejendommen "));
            Assert.That(checkLisTranslations[4].Text,
                Is.EqualTo("Gennemgang af damanlæg: Slagtepolitifolk/polte på ejendommen "));
            Assert.That(checkLisTranslations[5].Text, Is.EqualTo("Gennemgang af foderrum "));
            Assert.That(checkLisTranslations[6].Text,
                Is.EqualTo("Sundhed og pilleranvendelse: Ved håndtering af piller på Hvad-nummeret "));
            Assert.That(checkLisTranslations[7].Text, Is.EqualTo("Ved kandidater med kostråd "));
            Assert.That(checkLisTranslations[8].Text, Is.EqualTo("Ved kandidater uden kostråd "));
            Assert.That(checkLisTranslations[9].Text, Is.EqualTo("Alarmanlæg "));
            Assert.That(checkLisTranslations[10].Text, Is.EqualTo("Leverandører/aftagere af ænder "));
            Assert.That(checkLisTranslations[11].Text, Is.EqualTo("Management"));
            Assert.That(checkLisTranslations[12].Text, Is.EqualTo("Dansk Transportstandard"));
            Assert.That(checkLisTranslations[13].Text, Is.EqualTo("Samlet vurdering"));
            Assert.That(checkLisTranslations[14].Text, Is.EqualTo("OUA-politifolk"));

            #endregion

            #region Description

            Assert.That(listOfCL[0].Description, Is.Null);
            Assert.That(listOfCL[1].Description, Is.Null);
            Assert.That(listOfCL[2].Description, Is.Null);
            Assert.That(listOfCL[3].Description, Is.Null);
            Assert.That(listOfCL[4].Description, Is.Null);
            Assert.That(listOfCL[5].Description, Is.Null);
            Assert.That(listOfCL[6].Description, Is.Null);
            Assert.That(listOfCL[7].Description, Is.Null);
            Assert.That(listOfCL[8].Description, Is.Null);
            Assert.That(listOfCL[9].Description, Is.Null);
            Assert.That(listOfCL[10].Description, Is.Null);
            Assert.That(listOfCL[11].Description, Is.Null);
            Assert.That(listOfCL[12].Description, Is.Null);
            Assert.That(listOfCL[13].Description, Is.Null);
            Assert.That(listOfCL[14].Description, Is.Null);

            #endregion

            #endregion

            #region field

            #region Field.label

            #region fields 0-49

            Assert.That(listOfFields[0].Label, Is.EqualTo(null));
            Assert.That(listOfFields[1].Label, Is.EqualTo(null));
            Assert.That(listOfFields[2].Label, Is.EqualTo(null));
            Assert.That(listOfFields[3].Label, Is.EqualTo(null));
            Assert.That(listOfFields[4].Label, Is.EqualTo(null));
            Assert.That(listOfFields[5].Label, Is.EqualTo(null));
            Assert.That(listOfFields[6].Label, Is.EqualTo(null));
            Assert.That(listOfFields[7].Label, Is.EqualTo(null));
            Assert.That(listOfFields[8].Label, Is.EqualTo(null));
            Assert.That(listOfFields[9].Label, Is.EqualTo(null));
            Assert.That(listOfFields[10].Label, Is.EqualTo(null));
            Assert.That(listOfFields[11].Label, Is.EqualTo(null));
            Assert.That(listOfFields[12].Label, Is.EqualTo(null));
            Assert.That(listOfFields[13].Label, Is.EqualTo(null));
            Assert.That(listOfFields[14].Label, Is.EqualTo(null));
            Assert.That(listOfFields[15].Label, Is.EqualTo(null));
            Assert.That(listOfFields[16].Label, Is.EqualTo(null));
            Assert.That(listOfFields[17].Label, Is.EqualTo(null));
            Assert.That(listOfFields[18].Label, Is.EqualTo(null));
            Assert.That(listOfFields[19].Label, Is.EqualTo(null));
            Assert.That(listOfFields[20].Label, Is.EqualTo(null));
            Assert.That(listOfFields[21].Label, Is.EqualTo(null));
            Assert.That(listOfFields[22].Label, Is.EqualTo(null));
            Assert.That(listOfFields[23].Label, Is.EqualTo(null));
            Assert.That(listOfFields[24].Label, Is.EqualTo(null));
            Assert.That(listOfFields[25].Label, Is.EqualTo(null));
            Assert.That(listOfFields[26].Label, Is.EqualTo(null));
            Assert.That(listOfFields[27].Label, Is.EqualTo(null));
            Assert.That(listOfFields[28].Label, Is.EqualTo(null));
            Assert.That(listOfFields[29].Label, Is.EqualTo(null));
            Assert.That(listOfFields[30].Label, Is.EqualTo(null));
            Assert.That(listOfFields[31].Label, Is.EqualTo(null));
            Assert.That(listOfFields[32].Label, Is.EqualTo(null));
            Assert.That(listOfFields[33].Label, Is.EqualTo(null));
            Assert.That(listOfFields[34].Label, Is.EqualTo(null));
            Assert.That(listOfFields[35].Label, Is.EqualTo(null));
            Assert.That(listOfFields[36].Label, Is.EqualTo(null));
            Assert.That(listOfFields[37].Label, Is.EqualTo(null));
            Assert.That(listOfFields[38].Label, Is.EqualTo(null));
            Assert.That(listOfFields[39].Label, Is.EqualTo(null));
            Assert.That(listOfFields[40].Label, Is.EqualTo(null));
            Assert.That(listOfFields[41].Label, Is.EqualTo(null));
            Assert.That(listOfFields[42].Label, Is.EqualTo(null));
            Assert.That(listOfFields[43].Label, Is.EqualTo(null));
            Assert.That(listOfFields[44].Label, Is.EqualTo(null));
            Assert.That(listOfFields[45].Label, Is.EqualTo(null));
            Assert.That(listOfFields[46].Label, Is.EqualTo(null));
            Assert.That(listOfFields[47].Label, Is.EqualTo(null));
            Assert.That(listOfFields[48].Label, Is.EqualTo(null));
            Assert.That(listOfFields[49].Label, Is.EqualTo(null));

            #endregion

            #region fields 50-99

            Assert.That(listOfFields[50].Label, Is.EqualTo(null));
            Assert.That(listOfFields[51].Label, Is.EqualTo(null));
            Assert.That(listOfFields[52].Label, Is.EqualTo(null));
            Assert.That(listOfFields[53].Label, Is.EqualTo(null));
            Assert.That(listOfFields[54].Label, Is.EqualTo(null));
            Assert.That(listOfFields[55].Label, Is.EqualTo(null));
            Assert.That(listOfFields[56].Label, Is.EqualTo(null));
            Assert.That(listOfFields[57].Label, Is.EqualTo(null));
            Assert.That(listOfFields[58].Label, Is.EqualTo(null));
            Assert.That(listOfFields[59].Label, Is.EqualTo(null));
            Assert.That(listOfFields[60].Label, Is.EqualTo(null));
            Assert.That(listOfFields[61].Label, Is.EqualTo(null));
            Assert.That(listOfFields[62].Label, Is.EqualTo(null));
            Assert.That(listOfFields[63].Label, Is.EqualTo(null));
            Assert.That(listOfFields[64].Label, Is.EqualTo(null));
            Assert.That(listOfFields[65].Label, Is.EqualTo(null));
            Assert.That(listOfFields[66].Label, Is.EqualTo(null));
            Assert.That(listOfFields[67].Label, Is.EqualTo(null));
            Assert.That(listOfFields[68].Label, Is.EqualTo(null));
            Assert.That(listOfFields[69].Label, Is.EqualTo(null));
            Assert.That(listOfFields[70].Label, Is.EqualTo(null));
            Assert.That(listOfFields[71].Label, Is.EqualTo(null));
            Assert.That(listOfFields[72].Label, Is.EqualTo(null));
            Assert.That(listOfFields[73].Label, Is.EqualTo(null));
            Assert.That(listOfFields[74].Label, Is.EqualTo(null));
            Assert.That(listOfFields[75].Label, Is.EqualTo(null));
            Assert.That(listOfFields[76].Label, Is.EqualTo(null));
            Assert.That(listOfFields[77].Label, Is.EqualTo(null));
            Assert.That(listOfFields[78].Label, Is.EqualTo(null));
            Assert.That(listOfFields[79].Label, Is.EqualTo(null));
            Assert.That(listOfFields[80].Label, Is.EqualTo(null));
            Assert.That(listOfFields[81].Label, Is.EqualTo(null));
            Assert.That(listOfFields[82].Label, Is.EqualTo(null));
            Assert.That(listOfFields[83].Label, Is.EqualTo(null));
            Assert.That(listOfFields[84].Label, Is.EqualTo(null));
            Assert.That(listOfFields[85].Label, Is.EqualTo(null));
            Assert.That(listOfFields[86].Label, Is.EqualTo(null));
            Assert.That(listOfFields[87].Label, Is.EqualTo(null));
            Assert.That(listOfFields[88].Label, Is.EqualTo(null));
            Assert.That(listOfFields[89].Label, Is.EqualTo(null));
            Assert.That(listOfFields[90].Label, Is.EqualTo(null));
            Assert.That(listOfFields[91].Label, Is.EqualTo(null));
            Assert.That(listOfFields[92].Label, Is.EqualTo(null));
            Assert.That(listOfFields[93].Label, Is.EqualTo(null));
            Assert.That(listOfFields[94].Label, Is.EqualTo(null));
            Assert.That(listOfFields[95].Label, Is.EqualTo(null));
            Assert.That(listOfFields[96].Label, Is.EqualTo(null));
            Assert.That(listOfFields[97].Label, Is.EqualTo(null));
            Assert.That(listOfFields[98].Label, Is.EqualTo(null));
            Assert.That(listOfFields[99].Label, Is.EqualTo(null));

            #endregion

            #region fields 100-149

            Assert.That(listOfFields[100].Label, Is.EqualTo(null));
            Assert.That(listOfFields[101].Label, Is.EqualTo(null));
            Assert.That(listOfFields[102].Label, Is.EqualTo(null));
            Assert.That(listOfFields[103].Label, Is.EqualTo(null));
            Assert.That(listOfFields[104].Label, Is.EqualTo(null));
            Assert.That(listOfFields[105].Label, Is.EqualTo(null));
            Assert.That(listOfFields[106].Label, Is.EqualTo(null));
            Assert.That(listOfFields[107].Label, Is.EqualTo(null));
            Assert.That(listOfFields[108].Label, Is.EqualTo(null));
            Assert.That(listOfFields[109].Label, Is.EqualTo(null));
            Assert.That(listOfFields[110].Label, Is.EqualTo(null));
            Assert.That(listOfFields[111].Label, Is.EqualTo(null));
            Assert.That(listOfFields[112].Label, Is.EqualTo(null));
            Assert.That(listOfFields[113].Label, Is.EqualTo(null));
            Assert.That(listOfFields[114].Label, Is.EqualTo(null));
            Assert.That(listOfFields[115].Label, Is.EqualTo(null));
            Assert.That(listOfFields[116].Label, Is.EqualTo(null));
            Assert.That(listOfFields[117].Label, Is.EqualTo(null));
            Assert.That(listOfFields[118].Label, Is.EqualTo(null));
            Assert.That(listOfFields[119].Label, Is.EqualTo(null));
            Assert.That(listOfFields[120].Label, Is.EqualTo(null));
            Assert.That(listOfFields[121].Label, Is.EqualTo(null));
            Assert.That(listOfFields[122].Label, Is.EqualTo(null));
            Assert.That(listOfFields[123].Label, Is.EqualTo(null));
            Assert.That(listOfFields[124].Label, Is.EqualTo(null));
            Assert.That(listOfFields[125].Label, Is.EqualTo(null));
            Assert.That(listOfFields[126].Label, Is.EqualTo(null));
            Assert.That(listOfFields[127].Label, Is.EqualTo(null));
            Assert.That(listOfFields[128].Label, Is.EqualTo(null));
            Assert.That(listOfFields[129].Label, Is.EqualTo(null));
            Assert.That(listOfFields[130].Label, Is.EqualTo(null));
            Assert.That(listOfFields[131].Label, Is.EqualTo(null));
            Assert.That(listOfFields[132].Label, Is.EqualTo(null));
            Assert.That(listOfFields[133].Label, Is.EqualTo(null));
            Assert.That(listOfFields[134].Label, Is.EqualTo(null));
            Assert.That(listOfFields[135].Label, Is.EqualTo(null));
            Assert.That(listOfFields[136].Label, Is.EqualTo(null));
            Assert.That(listOfFields[137].Label, Is.EqualTo(null));
            Assert.That(listOfFields[138].Label, Is.EqualTo(null));
            Assert.That(listOfFields[139].Label, Is.EqualTo(null));
            Assert.That(listOfFields[140].Label, Is.EqualTo(null));
            Assert.That(listOfFields[141].Label, Is.EqualTo(null));
            Assert.That(listOfFields[142].Label, Is.EqualTo(null));
            Assert.That(listOfFields[143].Label, Is.EqualTo(null));
            Assert.That(listOfFields[144].Label, Is.EqualTo(null));
            Assert.That(listOfFields[145].Label, Is.EqualTo(null));
            Assert.That(listOfFields[146].Label, Is.EqualTo(null));
            Assert.That(listOfFields[147].Label, Is.EqualTo(null));
            Assert.That(listOfFields[148].Label, Is.EqualTo(null));
            Assert.That(listOfFields[149].Label, Is.EqualTo(null));

            #endregion

            #region fields 150-199

            Assert.That(listOfFields[150].Label, Is.EqualTo(null));
            Assert.That(listOfFields[151].Label, Is.EqualTo(null));
            Assert.That(listOfFields[152].Label, Is.EqualTo(null));
            Assert.That(listOfFields[153].Label, Is.EqualTo(null));
            Assert.That(listOfFields[154].Label, Is.EqualTo(null));
            Assert.That(listOfFields[155].Label, Is.EqualTo(null));
            Assert.That(listOfFields[156].Label, Is.EqualTo(null));
            Assert.That(listOfFields[157].Label, Is.EqualTo(null));
            Assert.That(listOfFields[158].Label, Is.EqualTo(null));
            Assert.That(listOfFields[159].Label, Is.EqualTo(null));
            Assert.That(listOfFields[160].Label, Is.EqualTo(null));
            Assert.That(listOfFields[161].Label, Is.EqualTo(null));
            Assert.That(listOfFields[162].Label, Is.EqualTo(null));
            Assert.That(listOfFields[163].Label, Is.EqualTo(null));
            Assert.That(listOfFields[164].Label, Is.EqualTo(null));
            Assert.That(listOfFields[165].Label, Is.EqualTo(null));
            Assert.That(listOfFields[166].Label, Is.EqualTo(null));
            Assert.That(listOfFields[167].Label, Is.EqualTo(null));
            Assert.That(listOfFields[168].Label, Is.EqualTo(null));
            Assert.That(listOfFields[169].Label, Is.EqualTo(null));
            Assert.That(listOfFields[170].Label, Is.EqualTo(null));
            Assert.That(listOfFields[171].Label, Is.EqualTo(null));
            Assert.That(listOfFields[172].Label, Is.EqualTo(null));
            Assert.That(listOfFields[173].Label, Is.EqualTo(null));
            Assert.That(listOfFields[174].Label, Is.EqualTo(null));
            Assert.That(listOfFields[175].Label, Is.EqualTo(null));
            Assert.That(listOfFields[176].Label, Is.EqualTo(null));
            Assert.That(listOfFields[177].Label, Is.EqualTo(null));
            Assert.That(listOfFields[178].Label, Is.EqualTo(null));
            Assert.That(listOfFields[179].Label, Is.EqualTo(null));
            Assert.That(listOfFields[180].Label, Is.EqualTo(null));
            Assert.That(listOfFields[181].Label, Is.EqualTo(null));
            Assert.That(listOfFields[182].Label, Is.EqualTo(null));
            Assert.That(listOfFields[183].Label, Is.EqualTo(null));
            Assert.That(listOfFields[184].Label, Is.EqualTo(null));
            Assert.That(listOfFields[185].Label, Is.EqualTo(null));
            Assert.That(listOfFields[186].Label, Is.EqualTo(null));
            Assert.That(listOfFields[187].Label, Is.EqualTo(null));
            Assert.That(listOfFields[188].Label, Is.EqualTo(null));
            Assert.That(listOfFields[189].Label, Is.EqualTo(null));
            Assert.That(listOfFields[190].Label, Is.EqualTo(null));
            Assert.That(listOfFields[191].Label, Is.EqualTo(null));
            Assert.That(listOfFields[192].Label, Is.EqualTo(null));
            Assert.That(listOfFields[193].Label, Is.EqualTo(null));
            Assert.That(listOfFields[194].Label, Is.EqualTo(null));
            Assert.That(listOfFields[195].Label, Is.EqualTo(null));
            Assert.That(listOfFields[196].Label, Is.EqualTo(null));
            Assert.That(listOfFields[197].Label, Is.EqualTo(null));
            Assert.That(listOfFields[198].Label, Is.EqualTo(null));
            Assert.That(listOfFields[199].Label, Is.EqualTo(null));

            #endregion

            #region fields 200-249

            Assert.That(listOfFields[200].Label, Is.EqualTo(null));
            Assert.That(listOfFields[201].Label, Is.EqualTo(null));
            Assert.That(listOfFields[202].Label, Is.EqualTo(null));
            Assert.That(listOfFields[203].Label, Is.EqualTo(null));
            Assert.That(listOfFields[204].Label, Is.EqualTo(null));
            Assert.That(listOfFields[205].Label, Is.EqualTo(null));
            Assert.That(listOfFields[206].Label, Is.EqualTo(null));
            Assert.That(listOfFields[207].Label, Is.EqualTo(null));
            Assert.That(listOfFields[208].Label, Is.EqualTo(null));
            Assert.That(listOfFields[209].Label, Is.EqualTo(null));
            Assert.That(listOfFields[210].Label, Is.EqualTo(null));
            Assert.That(listOfFields[211].Label, Is.EqualTo(null));
            Assert.That(listOfFields[212].Label, Is.EqualTo(null));
            Assert.That(listOfFields[213].Label, Is.EqualTo(null));
            Assert.That(listOfFields[214].Label, Is.EqualTo(null));
            Assert.That(listOfFields[215].Label, Is.EqualTo(null));
            Assert.That(listOfFields[216].Label, Is.EqualTo(null));
            Assert.That(listOfFields[217].Label, Is.EqualTo(null));
            Assert.That(listOfFields[218].Label, Is.EqualTo(null));
            Assert.That(listOfFields[219].Label, Is.EqualTo(null));
            Assert.That(listOfFields[220].Label, Is.EqualTo(null));
            Assert.That(listOfFields[221].Label, Is.EqualTo(null));
            Assert.That(listOfFields[222].Label, Is.EqualTo(null));
            Assert.That(listOfFields[223].Label, Is.EqualTo(null));
            Assert.That(listOfFields[224].Label, Is.EqualTo(null));
            Assert.That(listOfFields[225].Label, Is.EqualTo(null));
            Assert.That(listOfFields[226].Label, Is.EqualTo(null));
            Assert.That(listOfFields[227].Label, Is.EqualTo(null));
            Assert.That(listOfFields[228].Label, Is.EqualTo(null));
            Assert.That(listOfFields[229].Label, Is.EqualTo(null));
            Assert.That(listOfFields[230].Label, Is.EqualTo(null));
            Assert.That(listOfFields[231].Label, Is.EqualTo(null));
            Assert.That(listOfFields[232].Label, Is.EqualTo(null));
            Assert.That(listOfFields[233].Label, Is.EqualTo(null));
            Assert.That(listOfFields[234].Label, Is.EqualTo(null));
            Assert.That(listOfFields[235].Label, Is.EqualTo(null));
            Assert.That(listOfFields[236].Label, Is.EqualTo(null));
            Assert.That(listOfFields[237].Label, Is.EqualTo(null));
            Assert.That(listOfFields[238].Label, Is.EqualTo(null));
            Assert.That(listOfFields[239].Label, Is.EqualTo(null));
            Assert.That(listOfFields[240].Label, Is.EqualTo(null));
            Assert.That(listOfFields[241].Label, Is.EqualTo(null));
            Assert.That(listOfFields[242].Label, Is.EqualTo(null));
            Assert.That(listOfFields[243].Label, Is.EqualTo(null));
            Assert.That(listOfFields[244].Label, Is.EqualTo(null));
            Assert.That(listOfFields[245].Label, Is.EqualTo(null));
            Assert.That(listOfFields[246].Label, Is.EqualTo(null));
            Assert.That(listOfFields[247].Label, Is.EqualTo(null));
            Assert.That(listOfFields[248].Label, Is.EqualTo(null));
            Assert.That(listOfFields[249].Label, Is.EqualTo(null));

            #endregion

            #region fields 250-299

            Assert.That(listOfFields[250].Label, Is.EqualTo(null));
            Assert.That(listOfFields[251].Label, Is.EqualTo(null));
            Assert.That(listOfFields[252].Label, Is.EqualTo(null));
            Assert.That(listOfFields[253].Label, Is.EqualTo(null));
            Assert.That(listOfFields[254].Label, Is.EqualTo(null));
            Assert.That(listOfFields[255].Label, Is.EqualTo(null));
            Assert.That(listOfFields[256].Label, Is.EqualTo(null));
            Assert.That(listOfFields[257].Label, Is.EqualTo(null));
            Assert.That(listOfFields[258].Label, Is.EqualTo(null));
            Assert.That(listOfFields[259].Label, Is.EqualTo(null));
            Assert.That(listOfFields[260].Label, Is.EqualTo(null));
            Assert.That(listOfFields[261].Label, Is.EqualTo(null));
            Assert.That(listOfFields[262].Label, Is.EqualTo(null));
            Assert.That(listOfFields[263].Label, Is.EqualTo(null));
            Assert.That(listOfFields[264].Label, Is.EqualTo(null));
            Assert.That(listOfFields[265].Label, Is.EqualTo(null));
            Assert.That(listOfFields[266].Label, Is.EqualTo(null));
            Assert.That(listOfFields[267].Label, Is.EqualTo(null));
            Assert.That(listOfFields[268].Label, Is.EqualTo(null));
            Assert.That(listOfFields[269].Label, Is.EqualTo(null));
            Assert.That(listOfFields[270].Label, Is.EqualTo(null));
            Assert.That(listOfFields[271].Label, Is.EqualTo(null));
            Assert.That(listOfFields[272].Label, Is.EqualTo(null));
            Assert.That(listOfFields[273].Label, Is.EqualTo(null));
            Assert.That(listOfFields[274].Label, Is.EqualTo(null));
            Assert.That(listOfFields[275].Label, Is.EqualTo(null));
            Assert.That(listOfFields[276].Label, Is.EqualTo(null));
            Assert.That(listOfFields[277].Label, Is.EqualTo(null));
            Assert.That(listOfFields[278].Label, Is.EqualTo(null));
            Assert.That(listOfFields[279].Label, Is.EqualTo(null));
            Assert.That(listOfFields[280].Label, Is.EqualTo(null));
            Assert.That(listOfFields[281].Label, Is.EqualTo(null));
            Assert.That(listOfFields[282].Label, Is.EqualTo(null));
            Assert.That(listOfFields[283].Label, Is.EqualTo(null));
            Assert.That(listOfFields[284].Label, Is.EqualTo(null));
            Assert.That(listOfFields[285].Label, Is.EqualTo(null));
            Assert.That(listOfFields[286].Label, Is.EqualTo(null));
            Assert.That(listOfFields[287].Label, Is.EqualTo(null));
            Assert.That(listOfFields[288].Label, Is.EqualTo(null));
            Assert.That(listOfFields[289].Label, Is.EqualTo(null));
            Assert.That(listOfFields[290].Label, Is.EqualTo(null));
            Assert.That(listOfFields[291].Label, Is.EqualTo(null));
            Assert.That(listOfFields[292].Label, Is.EqualTo(null));
            Assert.That(listOfFields[293].Label, Is.EqualTo(null));
            Assert.That(listOfFields[294].Label, Is.EqualTo(null));
            Assert.That(listOfFields[295].Label, Is.EqualTo(null));
            Assert.That(listOfFields[296].Label, Is.EqualTo(null));
            Assert.That(listOfFields[297].Label, Is.EqualTo(null));
            Assert.That(listOfFields[298].Label, Is.EqualTo(null));
            Assert.That(listOfFields[299].Label, Is.EqualTo(null));

            #endregion

            #region fields 300-349

            Assert.That(listOfFields[300].Label, Is.EqualTo(null));
            Assert.That(listOfFields[301].Label, Is.EqualTo(null));
            Assert.That(listOfFields[302].Label, Is.EqualTo(null));
            Assert.That(listOfFields[303].Label, Is.EqualTo(null));
            Assert.That(listOfFields[304].Label, Is.EqualTo(null));
            Assert.That(listOfFields[305].Label, Is.EqualTo(null));
            Assert.That(listOfFields[306].Label, Is.EqualTo(null));
            Assert.That(listOfFields[307].Label, Is.EqualTo(null));
            Assert.That(listOfFields[308].Label, Is.EqualTo(null));
            Assert.That(listOfFields[309].Label, Is.EqualTo(null));
            Assert.That(listOfFields[310].Label, Is.EqualTo(null));
            Assert.That(listOfFields[311].Label, Is.EqualTo(null));
            Assert.That(listOfFields[312].Label, Is.EqualTo(null));
            Assert.That(listOfFields[313].Label, Is.EqualTo(null));
            Assert.That(listOfFields[314].Label, Is.EqualTo(null));
            Assert.That(listOfFields[315].Label, Is.EqualTo(null));
            Assert.That(listOfFields[316].Label, Is.EqualTo(null));
            Assert.That(listOfFields[317].Label, Is.EqualTo(null));
            Assert.That(listOfFields[318].Label, Is.EqualTo(null));
            Assert.That(listOfFields[319].Label, Is.EqualTo(null));
            Assert.That(listOfFields[320].Label, Is.EqualTo(null));
            Assert.That(listOfFields[321].Label, Is.EqualTo(null));
            Assert.That(listOfFields[322].Label, Is.EqualTo(null));
            Assert.That(listOfFields[323].Label, Is.EqualTo(null));
            Assert.That(listOfFields[324].Label, Is.EqualTo(null));
            Assert.That(listOfFields[325].Label, Is.EqualTo(null));
            Assert.That(listOfFields[326].Label, Is.EqualTo(null));
            Assert.That(listOfFields[327].Label, Is.EqualTo(null));
            Assert.That(listOfFields[328].Label, Is.EqualTo(null));
            Assert.That(listOfFields[329].Label, Is.EqualTo(null));
            Assert.That(listOfFields[330].Label, Is.EqualTo(null));
            Assert.That(listOfFields[331].Label, Is.EqualTo(null));
            Assert.That(listOfFields[332].Label, Is.EqualTo(null));
            Assert.That(listOfFields[333].Label, Is.EqualTo(null));
            Assert.That(listOfFields[334].Label, Is.EqualTo(null));
            Assert.That(listOfFields[335].Label, Is.EqualTo(null));
            Assert.That(listOfFields[336].Label, Is.EqualTo(null));
            Assert.That(listOfFields[337].Label, Is.EqualTo(null));
            Assert.That(listOfFields[338].Label, Is.EqualTo(null));
            Assert.That(listOfFields[339].Label, Is.EqualTo(null));
            Assert.That(listOfFields[340].Label, Is.EqualTo(null));
            Assert.That(listOfFields[341].Label, Is.EqualTo(null));
            Assert.That(listOfFields[342].Label, Is.EqualTo(null));
            Assert.That(listOfFields[343].Label, Is.EqualTo(null));
            Assert.That(listOfFields[344].Label, Is.EqualTo(null));
            Assert.That(listOfFields[345].Label, Is.EqualTo(null));
            Assert.That(listOfFields[346].Label, Is.EqualTo(null));
            Assert.That(listOfFields[347].Label, Is.EqualTo(null));
            Assert.That(listOfFields[348].Label, Is.EqualTo(null));
            Assert.That(listOfFields[349].Label, Is.EqualTo(null));

            #endregion

            #region fields 350-399

            Assert.That(listOfFields[350].Label, Is.EqualTo(null));
            Assert.That(listOfFields[351].Label, Is.EqualTo(null));
            Assert.That(listOfFields[352].Label, Is.EqualTo(null));
            Assert.That(listOfFields[353].Label, Is.EqualTo(null));
            Assert.That(listOfFields[354].Label, Is.EqualTo(null));
            Assert.That(listOfFields[355].Label, Is.EqualTo(null));
            Assert.That(listOfFields[356].Label, Is.EqualTo(null));
            Assert.That(listOfFields[357].Label, Is.EqualTo(null));
            Assert.That(listOfFields[358].Label, Is.EqualTo(null));
            Assert.That(listOfFields[359].Label, Is.EqualTo(null));
            Assert.That(listOfFields[360].Label, Is.EqualTo(null));
            Assert.That(listOfFields[361].Label, Is.EqualTo(null));
            Assert.That(listOfFields[362].Label, Is.EqualTo(null));
            Assert.That(listOfFields[363].Label, Is.EqualTo(null));
            Assert.That(listOfFields[364].Label, Is.EqualTo(null));
            Assert.That(listOfFields[365].Label, Is.EqualTo(null));
            Assert.That(listOfFields[366].Label, Is.EqualTo(null));
            Assert.That(listOfFields[367].Label, Is.EqualTo(null));
            Assert.That(listOfFields[368].Label, Is.EqualTo(null));
            Assert.That(listOfFields[369].Label, Is.EqualTo(null));
            Assert.That(listOfFields[370].Label, Is.EqualTo(null));
            Assert.That(listOfFields[371].Label, Is.EqualTo(null));
            Assert.That(listOfFields[372].Label, Is.EqualTo(null));
            Assert.That(listOfFields[373].Label, Is.EqualTo(null));
            Assert.That(listOfFields[374].Label, Is.EqualTo(null));
            Assert.That(listOfFields[375].Label, Is.EqualTo(null));
            Assert.That(listOfFields[376].Label, Is.EqualTo(null));
            Assert.That(listOfFields[377].Label, Is.EqualTo(null));
            Assert.That(listOfFields[378].Label, Is.EqualTo(null));
            Assert.That(listOfFields[379].Label, Is.EqualTo(null));
            Assert.That(listOfFields[380].Label, Is.EqualTo(null));
            Assert.That(listOfFields[381].Label, Is.EqualTo(null));
            Assert.That(listOfFields[382].Label, Is.EqualTo(null));
            Assert.That(listOfFields[383].Label, Is.EqualTo(null));
            Assert.That(listOfFields[384].Label, Is.EqualTo(null));
            Assert.That(listOfFields[385].Label, Is.EqualTo(null));
            Assert.That(listOfFields[386].Label, Is.EqualTo(null));
            Assert.That(listOfFields[387].Label, Is.EqualTo(null));
            Assert.That(listOfFields[388].Label, Is.EqualTo(null));
            Assert.That(listOfFields[389].Label, Is.EqualTo(null));
            Assert.That(listOfFields[390].Label, Is.EqualTo(null));
            Assert.That(listOfFields[391].Label, Is.EqualTo(null));
            Assert.That(listOfFields[392].Label, Is.EqualTo(null));
            Assert.That(listOfFields[393].Label, Is.EqualTo(null));
            Assert.That(listOfFields[394].Label, Is.EqualTo(null));
            Assert.That(listOfFields[395].Label, Is.EqualTo(null));
            Assert.That(listOfFields[396].Label, Is.EqualTo(null));
            Assert.That(listOfFields[397].Label, Is.EqualTo(null));
            Assert.That(listOfFields[398].Label, Is.EqualTo(null));
            Assert.That(listOfFields[399].Label, Is.EqualTo(null));

            #endregion

            #region fields 400-449

            Assert.That(listOfFields[400].Label, Is.EqualTo(null));
            Assert.That(listOfFields[401].Label, Is.EqualTo(null));
            Assert.That(listOfFields[402].Label, Is.EqualTo(null));
            Assert.That(listOfFields[403].Label, Is.EqualTo(null));
            Assert.That(listOfFields[404].Label, Is.EqualTo(null));
            Assert.That(listOfFields[405].Label, Is.EqualTo(null));
            Assert.That(listOfFields[406].Label, Is.EqualTo(null));
            Assert.That(listOfFields[407].Label, Is.EqualTo(null));
            Assert.That(listOfFields[408].Label, Is.EqualTo(null));
            Assert.That(listOfFields[409].Label, Is.EqualTo(null));
            Assert.That(listOfFields[410].Label, Is.EqualTo(null));
            Assert.That(listOfFields[411].Label, Is.EqualTo(null));
            Assert.That(listOfFields[412].Label, Is.EqualTo(null));
            Assert.That(listOfFields[413].Label, Is.EqualTo(null));
            Assert.That(listOfFields[414].Label, Is.EqualTo(null));
            Assert.That(listOfFields[415].Label, Is.EqualTo(null));
            Assert.That(listOfFields[416].Label, Is.EqualTo(null));
            Assert.That(listOfFields[417].Label, Is.EqualTo(null));
            Assert.That(listOfFields[418].Label, Is.EqualTo(null));
            Assert.That(listOfFields[419].Label, Is.EqualTo(null));
            Assert.That(listOfFields[420].Label, Is.EqualTo(null));
            Assert.That(listOfFields[421].Label, Is.EqualTo(null));
            Assert.That(listOfFields[422].Label, Is.EqualTo(null));
            Assert.That(listOfFields[423].Label, Is.EqualTo(null));
            Assert.That(listOfFields[424].Label, Is.EqualTo(null));
            Assert.That(listOfFields[425].Label, Is.EqualTo(null));
            Assert.That(listOfFields[426].Label, Is.EqualTo(null));
            Assert.That(listOfFields[427].Label, Is.EqualTo(null));
            Assert.That(listOfFields[428].Label, Is.EqualTo(null));
            Assert.That(listOfFields[429].Label, Is.EqualTo(null));
            Assert.That(listOfFields[430].Label, Is.EqualTo(null));
            Assert.That(listOfFields[431].Label, Is.EqualTo(null));
            Assert.That(listOfFields[432].Label, Is.EqualTo(null));
            Assert.That(listOfFields[433].Label, Is.EqualTo(null));
            Assert.That(listOfFields[434].Label, Is.EqualTo(null));
            Assert.That(listOfFields[435].Label, Is.EqualTo(null));
            Assert.That(listOfFields[436].Label, Is.EqualTo(null));
            Assert.That(listOfFields[437].Label, Is.EqualTo(null));
            Assert.That(listOfFields[438].Label, Is.EqualTo(null));
            Assert.That(listOfFields[439].Label, Is.EqualTo(null));
            Assert.That(listOfFields[440].Label, Is.EqualTo(null));
            Assert.That(listOfFields[441].Label, Is.EqualTo(null));
            Assert.That(listOfFields[442].Label, Is.EqualTo(null));
            Assert.That(listOfFields[443].Label, Is.EqualTo(null));
            Assert.That(listOfFields[444].Label, Is.EqualTo(null));
            Assert.That(listOfFields[445].Label, Is.EqualTo(null));
            Assert.That(listOfFields[446].Label, Is.EqualTo(null));
            Assert.That(listOfFields[447].Label, Is.EqualTo(null));
            Assert.That(listOfFields[448].Label, Is.EqualTo(null));
            Assert.That(listOfFields[449].Label, Is.EqualTo(null));

            #endregion

            #region fields 450-499

            Assert.That(listOfFields[450].Label, Is.EqualTo(null));
            Assert.That(listOfFields[451].Label, Is.EqualTo(null));
            Assert.That(listOfFields[452].Label, Is.EqualTo(null));
            Assert.That(listOfFields[453].Label, Is.EqualTo(null));
            Assert.That(listOfFields[454].Label, Is.EqualTo(null));
            Assert.That(listOfFields[455].Label, Is.EqualTo(null));
            Assert.That(listOfFields[456].Label, Is.EqualTo(null));
            Assert.That(listOfFields[457].Label, Is.EqualTo(null));
            Assert.That(listOfFields[458].Label, Is.EqualTo(null));
            Assert.That(listOfFields[459].Label, Is.EqualTo(null));
            Assert.That(listOfFields[460].Label, Is.EqualTo(null));
            Assert.That(listOfFields[461].Label, Is.EqualTo(null));
            Assert.That(listOfFields[462].Label, Is.EqualTo(null));
            Assert.That(listOfFields[463].Label, Is.EqualTo(null));
            Assert.That(listOfFields[464].Label, Is.EqualTo(null));
            Assert.That(listOfFields[465].Label, Is.EqualTo(null));
            Assert.That(listOfFields[466].Label, Is.EqualTo(null));
            Assert.That(listOfFields[467].Label, Is.EqualTo(null));
            Assert.That(listOfFields[468].Label, Is.EqualTo(null));
            Assert.That(listOfFields[469].Label, Is.EqualTo(null));
            Assert.That(listOfFields[470].Label, Is.EqualTo(null));
            Assert.That(listOfFields[471].Label, Is.EqualTo(null));
            Assert.That(listOfFields[472].Label, Is.EqualTo(null));
            Assert.That(listOfFields[473].Label, Is.EqualTo(null));
            Assert.That(listOfFields[474].Label, Is.EqualTo(null));
            Assert.That(listOfFields[475].Label, Is.EqualTo(null));
            Assert.That(listOfFields[476].Label, Is.EqualTo(null));
            Assert.That(listOfFields[477].Label, Is.EqualTo(null));
            Assert.That(listOfFields[478].Label, Is.EqualTo(null));
            Assert.That(listOfFields[479].Label, Is.EqualTo(null));
            Assert.That(listOfFields[480].Label, Is.EqualTo(null));
            Assert.That(listOfFields[481].Label, Is.EqualTo(null));
            Assert.That(listOfFields[482].Label, Is.EqualTo(null));
            Assert.That(listOfFields[483].Label, Is.EqualTo(null));
            Assert.That(listOfFields[484].Label, Is.EqualTo(null));
            Assert.That(listOfFields[485].Label, Is.EqualTo(null));
            Assert.That(listOfFields[486].Label, Is.EqualTo(null));
            Assert.That(listOfFields[487].Label, Is.EqualTo(null));
            Assert.That(listOfFields[488].Label, Is.EqualTo(null));
            Assert.That(listOfFields[489].Label, Is.EqualTo(null));
            Assert.That(listOfFields[490].Label, Is.EqualTo(null));
            Assert.That(listOfFields[491].Label, Is.EqualTo(null));
            Assert.That(listOfFields[492].Label, Is.EqualTo(null));
            Assert.That(listOfFields[493].Label, Is.EqualTo(null));
            Assert.That(listOfFields[494].Label, Is.EqualTo(null));
            Assert.That(listOfFields[495].Label, Is.EqualTo(null));
            Assert.That(listOfFields[496].Label, Is.EqualTo(null));
            Assert.That(listOfFields[497].Label, Is.EqualTo(null));
            Assert.That(listOfFields[498].Label, Is.EqualTo(null));
            Assert.That(listOfFields[499].Label, Is.EqualTo(null));

            #endregion

            #region fields 500-549

            Assert.That(listOfFields[500].Label, Is.EqualTo(null));
            Assert.That(listOfFields[501].Label, Is.EqualTo(null));
            Assert.That(listOfFields[502].Label, Is.EqualTo(null));
            Assert.That(listOfFields[503].Label, Is.EqualTo(null));
            Assert.That(listOfFields[504].Label, Is.EqualTo(null));
            Assert.That(listOfFields[505].Label, Is.EqualTo(null));
            Assert.That(listOfFields[506].Label, Is.EqualTo(null));
            Assert.That(listOfFields[507].Label, Is.EqualTo(null));
            Assert.That(listOfFields[508].Label, Is.EqualTo(null));
            Assert.That(listOfFields[509].Label, Is.EqualTo(null));
            Assert.That(listOfFields[510].Label, Is.EqualTo(null));
            Assert.That(listOfFields[511].Label, Is.EqualTo(null));
            Assert.That(listOfFields[512].Label, Is.EqualTo(null));
            Assert.That(listOfFields[513].Label, Is.EqualTo(null));
            Assert.That(listOfFields[514].Label, Is.EqualTo(null));
            Assert.That(listOfFields[515].Label, Is.EqualTo(null));
            Assert.That(listOfFields[516].Label, Is.EqualTo(null));
            Assert.That(listOfFields[517].Label, Is.EqualTo(null));
            Assert.That(listOfFields[518].Label, Is.EqualTo(null));
            Assert.That(listOfFields[519].Label, Is.EqualTo(null));
            Assert.That(listOfFields[520].Label, Is.EqualTo(null));
            Assert.That(listOfFields[521].Label, Is.EqualTo(null));
            Assert.That(listOfFields[522].Label, Is.EqualTo(null));
            Assert.That(listOfFields[523].Label, Is.EqualTo(null));
            Assert.That(listOfFields[524].Label, Is.EqualTo(null));
            Assert.That(listOfFields[525].Label, Is.EqualTo(null));
            Assert.That(listOfFields[526].Label, Is.EqualTo(null));
            Assert.That(listOfFields[527].Label, Is.EqualTo(null));
            Assert.That(listOfFields[528].Label, Is.EqualTo(null));
            Assert.That(listOfFields[529].Label, Is.EqualTo(null));
            Assert.That(listOfFields[530].Label, Is.EqualTo(null));
            Assert.That(listOfFields[531].Label, Is.EqualTo(null));
            Assert.That(listOfFields[532].Label, Is.EqualTo(null));
            Assert.That(listOfFields[533].Label, Is.EqualTo(null));
            Assert.That(listOfFields[534].Label, Is.EqualTo(null));
            Assert.That(listOfFields[535].Label, Is.EqualTo(null));
            Assert.That(listOfFields[536].Label, Is.EqualTo(null));
            Assert.That(listOfFields[537].Label, Is.EqualTo(null));
            Assert.That(listOfFields[538].Label, Is.EqualTo(null));
            Assert.That(listOfFields[539].Label, Is.EqualTo(null));
            Assert.That(listOfFields[540].Label, Is.EqualTo(null));
            Assert.That(listOfFields[541].Label, Is.EqualTo(null));
            Assert.That(listOfFields[542].Label, Is.EqualTo(null));
            Assert.That(listOfFields[543].Label, Is.EqualTo(null));
            Assert.That(listOfFields[544].Label, Is.EqualTo(null));
            Assert.That(listOfFields[545].Label, Is.EqualTo(null));
            Assert.That(listOfFields[546].Label, Is.EqualTo(null));
            Assert.That(listOfFields[547].Label, Is.EqualTo(null));
            Assert.That(listOfFields[548].Label, Is.EqualTo(null));
            Assert.That(listOfFields[549].Label, Is.EqualTo(null));

            #endregion

            #region fields 550-599

            Assert.That(listOfFields[550].Label, Is.EqualTo(null));
            Assert.That(listOfFields[551].Label, Is.EqualTo(null));
            Assert.That(listOfFields[552].Label, Is.EqualTo(null));
            Assert.That(listOfFields[553].Label, Is.EqualTo(null));
            Assert.That(listOfFields[554].Label, Is.EqualTo(null));
            Assert.That(listOfFields[555].Label, Is.EqualTo(null));
            Assert.That(listOfFields[556].Label, Is.EqualTo(null));
            Assert.That(listOfFields[557].Label, Is.EqualTo(null));
            Assert.That(listOfFields[558].Label, Is.EqualTo(null));
            Assert.That(listOfFields[559].Label, Is.EqualTo(null));
            Assert.That(listOfFields[560].Label, Is.EqualTo(null));
            Assert.That(listOfFields[561].Label, Is.EqualTo(null));
            Assert.That(listOfFields[562].Label, Is.EqualTo(null));
            Assert.That(listOfFields[563].Label, Is.EqualTo(null));
            Assert.That(listOfFields[564].Label, Is.EqualTo(null));
            Assert.That(listOfFields[565].Label, Is.EqualTo(null));
            Assert.That(listOfFields[566].Label, Is.EqualTo(null));
            Assert.That(listOfFields[567].Label, Is.EqualTo(null));
            Assert.That(listOfFields[568].Label, Is.EqualTo(null));
            Assert.That(listOfFields[569].Label, Is.EqualTo(null));
            Assert.That(listOfFields[570].Label, Is.EqualTo(null));
            Assert.That(listOfFields[571].Label, Is.EqualTo(null));
            Assert.That(listOfFields[572].Label, Is.EqualTo(null));
            Assert.That(listOfFields[573].Label, Is.EqualTo(null));
            Assert.That(listOfFields[574].Label, Is.EqualTo(null));
            Assert.That(listOfFields[575].Label, Is.EqualTo(null));
            Assert.That(listOfFields[576].Label, Is.EqualTo(null));
            Assert.That(listOfFields[577].Label, Is.EqualTo(null));
            Assert.That(listOfFields[578].Label, Is.EqualTo(null));
            Assert.That(listOfFields[579].Label, Is.EqualTo(null));
            Assert.That(listOfFields[580].Label, Is.EqualTo(null));
            Assert.That(listOfFields[581].Label, Is.EqualTo(null));
            Assert.That(listOfFields[582].Label, Is.EqualTo(null));
            Assert.That(listOfFields[583].Label, Is.EqualTo(null));
            Assert.That(listOfFields[584].Label, Is.EqualTo(null));
            Assert.That(listOfFields[585].Label, Is.EqualTo(null));
            Assert.That(listOfFields[586].Label, Is.EqualTo(null));
            Assert.That(listOfFields[587].Label, Is.EqualTo(null));
            Assert.That(listOfFields[588].Label, Is.EqualTo(null));
            Assert.That(listOfFields[589].Label, Is.EqualTo(null));
            Assert.That(listOfFields[590].Label, Is.EqualTo(null));
            Assert.That(listOfFields[591].Label, Is.EqualTo(null));
            Assert.That(listOfFields[592].Label, Is.EqualTo(null));
            Assert.That(listOfFields[593].Label, Is.EqualTo(null));
            Assert.That(listOfFields[594].Label, Is.EqualTo(null));
            Assert.That(listOfFields[595].Label, Is.EqualTo(null));
            Assert.That(listOfFields[596].Label, Is.EqualTo(null));
            Assert.That(listOfFields[597].Label, Is.EqualTo(null));
            Assert.That(listOfFields[598].Label, Is.EqualTo(null));
            Assert.That(listOfFields[599].Label, Is.EqualTo(null));

            #endregion

            #region fields 600-649

            Assert.That(listOfFields[600].Label, Is.EqualTo(null));
            Assert.That(listOfFields[601].Label, Is.EqualTo(null));
            Assert.That(listOfFields[602].Label, Is.EqualTo(null));
            Assert.That(listOfFields[603].Label, Is.EqualTo(null));
            Assert.That(listOfFields[604].Label, Is.EqualTo(null));
            Assert.That(listOfFields[605].Label, Is.EqualTo(null));
            Assert.That(listOfFields[606].Label, Is.EqualTo(null));
            Assert.That(listOfFields[607].Label, Is.EqualTo(null));
            Assert.That(listOfFields[608].Label, Is.EqualTo(null));
            Assert.That(listOfFields[609].Label, Is.EqualTo(null));
            Assert.That(listOfFields[610].Label, Is.EqualTo(null));
            Assert.That(listOfFields[611].Label, Is.EqualTo(null));
            Assert.That(listOfFields[612].Label, Is.EqualTo(null));
            Assert.That(listOfFields[613].Label, Is.EqualTo(null));
            Assert.That(listOfFields[614].Label, Is.EqualTo(null));
            Assert.That(listOfFields[615].Label, Is.EqualTo(null));
            Assert.That(listOfFields[616].Label, Is.EqualTo(null));
            Assert.That(listOfFields[617].Label, Is.EqualTo(null));
            Assert.That(listOfFields[618].Label, Is.EqualTo(null));
            Assert.That(listOfFields[619].Label, Is.EqualTo(null));
            Assert.That(listOfFields[620].Label, Is.EqualTo(null));
            Assert.That(listOfFields[621].Label, Is.EqualTo(null));
            Assert.That(listOfFields[622].Label, Is.EqualTo(null));
            Assert.That(listOfFields[623].Label, Is.EqualTo(null));
            Assert.That(listOfFields[624].Label, Is.EqualTo(null));
            Assert.That(listOfFields[625].Label, Is.EqualTo(null));
            Assert.That(listOfFields[626].Label, Is.EqualTo(null));
            Assert.That(listOfFields[627].Label, Is.EqualTo(null));
            Assert.That(listOfFields[628].Label, Is.EqualTo(null));
            Assert.That(listOfFields[629].Label, Is.EqualTo(null));
            Assert.That(listOfFields[630].Label, Is.EqualTo(null));
            Assert.That(listOfFields[631].Label, Is.EqualTo(null));
            Assert.That(listOfFields[632].Label, Is.EqualTo(null));
            Assert.That(listOfFields[633].Label, Is.EqualTo(null));
            Assert.That(listOfFields[634].Label, Is.EqualTo(null));
            Assert.That(listOfFields[635].Label, Is.EqualTo(null));
            Assert.That(listOfFields[636].Label, Is.EqualTo(null));
            Assert.That(listOfFields[637].Label, Is.EqualTo(null));
            Assert.That(listOfFields[638].Label, Is.EqualTo(null));
            Assert.That(listOfFields[639].Label, Is.EqualTo(null));
            Assert.That(listOfFields[640].Label, Is.EqualTo(null));
            Assert.That(listOfFields[641].Label, Is.EqualTo(null));
            Assert.That(listOfFields[642].Label, Is.EqualTo(null));
            Assert.That(listOfFields[643].Label, Is.EqualTo(null));
            Assert.That(listOfFields[644].Label, Is.EqualTo(null));
            Assert.That(listOfFields[645].Label, Is.EqualTo(null));
            Assert.That(listOfFields[646].Label, Is.EqualTo(null));
            Assert.That(listOfFields[647].Label, Is.EqualTo(null));
            Assert.That(listOfFields[648].Label, Is.EqualTo(null));
            Assert.That(listOfFields[649].Label, Is.EqualTo(null));

            #endregion

            #region fields 650-681

            Assert.That(listOfFields[650].Label, Is.EqualTo(null));
            Assert.That(listOfFields[651].Label, Is.EqualTo(null));
            Assert.That(listOfFields[652].Label, Is.EqualTo(null));
            Assert.That(listOfFields[653].Label, Is.EqualTo(null));
            Assert.That(listOfFields[654].Label, Is.EqualTo(null));
            Assert.That(listOfFields[655].Label, Is.EqualTo(null));
            Assert.That(listOfFields[656].Label, Is.EqualTo(null));
            Assert.That(listOfFields[657].Label, Is.EqualTo(null));
            Assert.That(listOfFields[658].Label, Is.EqualTo(null));
            Assert.That(listOfFields[659].Label, Is.EqualTo(null));
            Assert.That(listOfFields[660].Label, Is.EqualTo(null));
            Assert.That(listOfFields[661].Label, Is.EqualTo(null));
            Assert.That(listOfFields[662].Label, Is.EqualTo(null));
            Assert.That(listOfFields[663].Label, Is.EqualTo(null));
            Assert.That(listOfFields[664].Label, Is.EqualTo(null));
            Assert.That(listOfFields[665].Label, Is.EqualTo(null));
            Assert.That(listOfFields[666].Label, Is.EqualTo(null));
            Assert.That(listOfFields[667].Label, Is.EqualTo(null));
            Assert.That(listOfFields[668].Label, Is.EqualTo(null));
            Assert.That(listOfFields[669].Label, Is.EqualTo(null));
            Assert.That(listOfFields[670].Label, Is.EqualTo(null));
            Assert.That(listOfFields[671].Label, Is.EqualTo(null));
            Assert.That(listOfFields[672].Label, Is.EqualTo(null));
            Assert.That(listOfFields[673].Label, Is.EqualTo(null));
            Assert.That(listOfFields[674].Label, Is.EqualTo(null));
            Assert.That(listOfFields[675].Label, Is.EqualTo(null));
            Assert.That(listOfFields[676].Label, Is.EqualTo(null));
            Assert.That(listOfFields[677].Label, Is.EqualTo(null));
            Assert.That(listOfFields[678].Label, Is.EqualTo(null));
            Assert.That(listOfFields[679].Label, Is.EqualTo(null));
            Assert.That(listOfFields[680].Label, Is.EqualTo(null));

            #endregion

            #endregion

            List<FieldType> fieldTypes = await DbContext.FieldTypes.ToListAsync();

            #region FieldType

            #region fields 0-49

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[0].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[1].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Text));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[2].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Text));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[3].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.MultiSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[4].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[5].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[6].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[7].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[8].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[9].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[10].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[11].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[12].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[13].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[14].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[15].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[16].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[17].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[18].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Date));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[19].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.MultiSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[20].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[21].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[22].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Timer));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[23].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[24].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[25].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Signature));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[26].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Signature));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[27].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Signature));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[28].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Signature));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[29].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.CheckBox));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[30].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[31].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[32].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[33].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[34].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[35].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[36].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[37].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[38].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[39].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[40].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[41].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[42].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[43].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[44].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[45].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[46].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[47].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[48].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[49].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));

            #endregion

            #region fields 50-99

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[50].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[51].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[52].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[53].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[54].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[55].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[56].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[57].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[58].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[59].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[60].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[61].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[62].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[63].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[64].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[65].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[66].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[67].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[68].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[69].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[70].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[71].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[72].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[73].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[74].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[75].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[76].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[77].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[78].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[79].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[80].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[81].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[82].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[83].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[84].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[85].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[86].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[87].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[88].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[89].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[90].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[91].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[92].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[93].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[94].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[95].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[96].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[97].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[98].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[99].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));

            #endregion

            #region fields 100-149

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[100].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[101].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[102].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[103].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[104].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[105].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[106].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[107].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[108].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[109].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[110].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[111].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[112].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[113].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[114].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[115].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[116].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[117].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[118].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[119].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[120].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[121].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[122].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[123].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[124].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[125].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[126].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[127].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[128].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[129].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[130].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[131].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[132].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[133].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[134].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[135].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[136].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[137].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[138].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[139].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[140].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[141].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[142].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[143].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[144].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[145].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[146].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[147].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[148].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[149].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));

            #endregion

            #region fields 150-199

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[150].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[151].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[152].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[153].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[154].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[155].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[156].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[157].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[158].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[159].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[160].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.MultiSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[161].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[162].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[163].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[164].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[165].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[166].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[167].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[168].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[169].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[170].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[171].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[172].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[173].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[174].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[175].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[176].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[177].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[178].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[179].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[180].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[181].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[182].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[183].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[184].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[185].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[186].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[187].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[188].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[189].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[190].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[191].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[192].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[193].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[194].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[195].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[196].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[197].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[198].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[199].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));

            #endregion

            #region fields 200-249

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[200].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[201].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[202].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[203].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[204].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[205].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[206].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[207].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[208].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[209].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[210].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[211].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[212].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[213].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[214].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[215].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[216].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[217].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[218].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[219].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[220].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[221].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[222].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[223].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[224].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[225].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[226].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[227].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[228].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[229].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[230].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[231].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[232].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[233].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[234].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[235].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[236].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[237].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[238].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[239].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[240].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[241].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[242].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[243].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[244].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[245].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[246].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[247].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[248].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[249].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));

            #endregion

            #region fields 250-299

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[250].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[251].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[252].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[253].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[254].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[255].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[256].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[257].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[258].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[259].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[260].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[261].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[262].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[263].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[264].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[265].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[266].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[267].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[268].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[269].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[270].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[271].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[272].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[273].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[274].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[275].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[276].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[277].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[278].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[279].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[280].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[281].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[282].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[283].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[284].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[285].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[286].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[287].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[288].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[289].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[290].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[291].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[292].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[293].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[294].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[295].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[296].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[297].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[298].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[299].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));

            #endregion

            #region fields 300-349

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[300].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[301].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[302].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[303].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[304].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[305].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[306].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[307].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[308].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[309].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[310].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[311].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[312].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[313].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[314].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[315].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[316].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[317].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[318].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[319].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[320].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[321].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[322].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[323].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[324].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[325].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[326].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[327].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[328].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[329].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[330].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[331].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[332].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.MultiSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[333].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[334].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[335].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[336].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[337].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[338].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[339].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[340].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[341].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[342].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[343].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[344].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[345].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[346].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[347].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[348].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[349].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));

            #endregion

            #region fields 350-399

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[350].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[351].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[352].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[353].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[354].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[355].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[356].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[357].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[358].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[359].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[360].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[361].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[362].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[363].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[364].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[365].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[366].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[367].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[368].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[369].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[370].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[371].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[372].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[373].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[374].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[375].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[376].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[377].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[378].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.CheckBox));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[379].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.CheckBox));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[380].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.CheckBox));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[381].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.CheckBox));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[382].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[383].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[384].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.CheckBox));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[385].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[386].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[387].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[388].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[389].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[390].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[391].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[392].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[393].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[394].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[395].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[396].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[397].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[398].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[399].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));

            #endregion

            #region fields 400-449

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[400].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[401].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[402].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[403].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[404].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[405].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[406].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[407].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[408].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[409].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[410].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[411].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[412].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[413].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[414].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[415].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[416].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[417].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[418].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[419].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[420].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[421].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[422].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[423].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[424].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[425].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.MultiSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[426].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[427].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[428].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[429].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[430].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[431].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[432].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[433].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[434].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[435].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[436].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[437].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[438].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[439].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[440].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[441].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[442].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[443].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[444].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[445].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[446].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[447].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[448].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[449].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));

            #endregion

            #region fields 450-499

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[450].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[451].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[452].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[453].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[454].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[455].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[456].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[457].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[458].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[459].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[460].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[461].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[462].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[463].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[464].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[465].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[466].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[467].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[468].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[469].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[470].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[471].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[472].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[473].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[474].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[475].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[476].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[477].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[478].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[479].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[480].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[481].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[482].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[483].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[484].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[485].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[486].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[487].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[488].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[489].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[490].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[491].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[492].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[493].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[494].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[495].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[496].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[497].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[498].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[499].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));

            #endregion

            #region fields 500-549

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[500].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[501].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[502].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[503].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[504].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[505].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[506].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[507].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[508].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[509].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[510].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[511].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[512].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[513].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[514].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[515].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[516].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[517].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[518].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[519].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[520].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[521].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[522].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[523].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[524].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[525].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[526].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[527].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[528].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[529].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[530].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[531].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[532].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[533].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[534].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[535].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[536].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[537].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[538].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[539].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[540].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[541].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[542].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[543].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[544].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[545].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[546].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[547].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[548].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[549].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));

            #endregion

            #region fields 550-599

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[550].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[551].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[552].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[553].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[554].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[555].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[556].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[557].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[558].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[559].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[560].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[561].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[562].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[563].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[564].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[565].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[566].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[567].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[568].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[569].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[570].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[571].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[572].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[573].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[574].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[575].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[576].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[577].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[578].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[579].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[580].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[581].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[582].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[583].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[584].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[585].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[586].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[587].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[588].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[589].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[590].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[591].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[592].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[593].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[594].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[595].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[596].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[597].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[598].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[599].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));

            #endregion

            #region fields 600-649

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[600].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[601].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[602].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[603].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[604].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[605].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[606].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[607].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[608].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[609].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[610].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[611].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[612].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[613].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[614].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[615].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[616].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[617].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[618].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[619].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[620].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[621].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[622].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[623].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[624].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[625].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[626].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[627].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[628].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[629].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[630].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[631].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[632].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[633].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[634].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[635].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[636].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[637].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[638].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[639].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[640].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[641].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[642].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[643].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[644].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[645].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[646].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[647].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[648].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[649].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));

            #endregion

            #region fields 650-681

            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[650].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[651].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[652].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[653].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[654].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[655].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[656].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[657].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.MultiSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[658].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[659].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[660].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Picture));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[661].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[662].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[663].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[664].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[665].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[666].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Comment));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[667].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[668].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Text));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[669].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[670].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[671].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.Number));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[672].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[673].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[674].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[675].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[676].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[677].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[678].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[679].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));
            Assert.That(fieldTypes.Single(x => x.Id == listOfFields[680].FieldTypeId).Type,
                Is.EqualTo(Constants.FieldTypes.SingleSelect));

            #endregion

            #endregion

            #region Field.Description

            #region fields 0-49

            Assert.That(fieldTranslations[0].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Stamdata og gummioplysninger </strong>"));
            Assert.That(fieldTranslations[1].Description, Is.EqualTo("<strong>Ande-nummer: </strong>"));
            Assert.That(fieldTranslations[2].Description, Is.EqualTo("<strong>Gumminummer: </strong>"));
            Assert.That(fieldTranslations[3].Description, Is.EqualTo("<strong>Andedam: </strong>"));
            Assert.That(fieldTranslations[4].Description, Is.EqualTo("<strong>Angiv evt. navn på anden Rumpe:</strong>"));
            Assert.That(fieldTranslations[5].Description,
                Is.EqualTo("<strong>0.2 Ænderne omfatter: Antal fjer til næb: </strong>"));
            Assert.That(fieldTranslations[6].Description,
                Is.EqualTo("<strong>0.2 Ænderne omfatter: Antal fjer til 8–25 g´s rumpe: </strong>"));
            Assert.That(fieldTranslations[7].Description,
                Is.EqualTo("<strong>0.2 Ænderne omfatter: Antal fjer til fod: </strong>"));
            Assert.That(fieldTranslations[8].Description,
                Is.EqualTo("<strong>0.2 Ænderne omfatter: Antal fjer til hoveder: </strong>"));
            Assert.That(fieldTranslations[9].Description, Is.EqualTo("<strong>Ande-ejerens/næbets navn: </strong>"));
            Assert.That(fieldTranslations[10].Description, Is.EqualTo("<strong>Ande-ejerens damadresse: </strong>"));
            Assert.That(
                fieldTranslations[11].Description,
                Is.EqualTo("<strong>0.1.3 Skriv eventuel ny dam (Udfyldes kun hvis der ikke er registreret korrekt flyveNr i And): </strong>"));
            Assert.That(fieldTranslations[12].Description, Is.EqualTo("<strong>By: </strong>"));
            Assert.That(fieldTranslations[13].Description, Is.EqualTo("<strong>Postnr.: </strong>"));
            Assert.That(fieldTranslations[14].Description, Is.EqualTo("<strong>Ande-ejerens telefon: </strong>"));
            Assert.That(fieldTranslations[15].Description, Is.EqualTo("<strong>Ande-ejerens mobil: </strong>"));
            Assert.That(fieldTranslations[16].Description, Is.EqualTo("<strong>Ande-ejerens e-mail: </strong>"));
            Assert.That(fieldTranslations[17].Description, Is.EqualTo("<strong>Fjer-adresse: </strong>"));
            Assert.That(fieldTranslations[18].Description, Is.EqualTo("<strong>Flyvedato: </strong>"));
            Assert.That(fieldTranslations[19].Description, Is.EqualTo("<strong>flyvedato: </strong>"));
            Assert.That(fieldTranslations[20].Description, Is.EqualTo("<strong>Pilot 1: </strong>"));
            Assert.That(fieldTranslations[21].Description, Is.EqualTo("<strong>Pilot 2: </strong>"));
            Assert.That(fieldTranslations[22].Description, Is.EqualTo("<strong>Flyvetid audit: </strong>"));
            Assert.That(fieldTranslations[23].Description,
                Is.EqualTo("<strong>Er Internationalepapirer underskrevet? </strong>"));
            Assert.That(fieldTranslations[24].Description, Is.EqualTo("<strong>Underskriften Gælder:</strong><br>...<br>"));
            Assert.That(
                fieldTranslations[25].Description,
                Is.EqualTo("<strong>Underskrift for accept af standard/ekstrabesøg Von And-Fly: Ande-Ejer/stedfortræder </strong>"));
            Assert.That(fieldTranslations[26].Description,
                Is.EqualTo("<strong>Underskrift for accept af standard/ekstrabesøg Von And-Fly: And </strong>"));
            Assert.That(
                fieldTranslations[27].Description,
                Is.EqualTo("<strong>Underskrift for accept af tillægsaudit vedr. Mule-fly: Ande-Ejer/stedfortræder </strong>"));
            Assert.That(fieldTranslations[28].Description,
                Is.EqualTo("<strong>Underskrift for accept af tillægsaudit vedr. Mule-fly: And </strong>"));
            Assert.That(fieldTranslations[29].Description, Is.EqualTo("<strong>Skrøbelige ænder (sæt kryds): </strong>"));
            Assert.That(
                fieldTranslations[30].Description,
                Is.EqualTo("<strong>0.15 Har And sikret sig at Ande-ejer kender betydningen af en certificering? </strong>"));
            Assert.That(fieldTranslations[31].Description,
                Is.EqualTo("<strong>0.16 Vil Ande-ejer have sin produktion certificeret? </strong>"));
            Assert.That(fieldTranslations[32].Description,
                Is.EqualTo("<strong>2.2 Blev 'Vejledning for god flyvning i dammen' udleveret? </strong>"));
            Assert.That(fieldTranslations[33].Description, Is.EqualTo("<strong>12 Registreres indkomne måger? </strong>"));
            Assert.That(fieldTranslations[34].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Gennemgang af damme: Gæs på ejendommen </strong>"));
            Assert.That(fieldTranslations[35].Description, Is.EqualTo("<strong>5.0 Er der gæs på ejendommen? </strong>"));
            Assert.That(fieldTranslations[36].Description,
                Is.EqualTo("<strong>5.1 Overholdes lovkrav til kanaler og mindste næb? (opf) </strong>"));
            Assert.That(
                fieldTranslations[37].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1 Overholdes lovkrav til kanaler og mindste næb? (opf) </i><strong>Angiv antal kanaler, hvor lovkrav til kanaler og mindste næb ikke er opfyldt: </strong>"));
            Assert.That(
                fieldTranslations[38].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1 Overholdes lovkrav til kanaler og mindste næb? (opf) </i><strong> Angiv afsnit, hvor lovkrav til kanaler og mindste næb ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[39].Description,
                Is.EqualTo("<strong>Ved syndflod: Angiv syndflodprocent: </strong>"));
            Assert.That(fieldTranslations[40].Description,
                Is.EqualTo("<i>Billede: 5.1 Overholdes lovkrav til kanaeæer og mindste næb? (opf) </i>"));
            Assert.That(fieldTranslations[41].Description,
                Is.EqualTo("<strong>5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </strong>"));
            Assert.That(
                fieldTranslations[42].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i><strong>Angiv antal af gæs, der ikke opfylder kravet: </strong>"));
            Assert.That(
                fieldTranslations[43].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i><strong>Angiv antal af gæs, der ikke opfylder kravet: </strong>"));
            Assert.That(
                fieldTranslations[44].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i><strong>Er der fjer på gæsne fra inventar? </strong>"));
            Assert.That(
                fieldTranslations[45].Description,
                Is.EqualTo("<i>Hvis Ja: Er der skader på gæsne fra inventar? </i><strong>Angiv antal gæs med skader fra inventar: </strong>"));
            Assert.That(
                fieldTranslations[46].Description,
                Is.EqualTo("<i>Hvis Ja: Er der skader på gæsne fra inventar? </i><strong>Beskriv antal og hvilke damnme det omfatter. Det kan evt. være nødvendigt at foretage obduktion af gæsne. </strong>"));
            Assert.That(fieldTranslations[47].Description,
                Is.EqualTo("<i>Billede: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i>"));
            Assert.That(
                fieldTranslations[48].Description,
                Is.EqualTo("<strong>5.1.4 Er der tilstrækkeligt vand, til at alle blishøner kan drikke? (evt opf) </strong>"));
            Assert.That(
                fieldTranslations[49].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1.4 Er der tilstrækkeligt vand, til at alle blishøner kan drikke? (evt opf)</i><strong> Beskriv antal og hvilke damasnit det omfatter:</strong>"));

            #endregion

            #region fields 50-99

            Assert.That(
                fieldTranslations[50].Description,
                Is.EqualTo("<i>Billede: 5.1.4 Er der tilstrækkeligt vand, til at alle blishøner kan drikke? (evt opf) </i>"));
            Assert.That(fieldTranslations[51].Description,
                Is.EqualTo("<strong>5.2 Er dambredde og vandet i orden, så skader undgås? </strong>"));
            Assert.That(fieldTranslations[52].Description,
                Is.EqualTo("<i>Billede: 5.2 Er dambredde og vandet i orden, så skader undgås? </i>"));
            Assert.That(fieldTranslations[53].Description,
                Is.EqualTo("<strong>5.3 Er sovsearealer bekvemme, rene og passende våde? </strong>"));
            Assert.That(
                fieldTranslations[54].Description,
                Is.EqualTo("<i>Hvis Nej: 5.3 Er sovsearealer bekvemme, rene og passende våde? </i><strong>Angiv andel af damme, der ikke opfylder kravet: </strong>"));
            Assert.That(
                fieldTranslations[55].Description,
                Is.EqualTo("<i>Hvis Nej: 5.3 Er sovsearealer bekvemme, rene og passende våde? </i><strong>Beskriv omfang af sovs med ord: </strong>"));
            Assert.That(fieldTranslations[56].Description,
                Is.EqualTo("<i>Billede: 5.3 Er sovsearealer bekvemme, rene og passende våde? </i>"));
            Assert.That(
                fieldTranslations[57].Description,
                Is.EqualTo("<strong>3.2 Er damme, kanaler og dildoer rengjorte og eventuelt desinficerede, og bliver hømhøm og madspild fjernet jævnligt? </strong>"));
            Assert.That(
                fieldTranslations[58].Description,
                Is.EqualTo("<i>Billede: 3.2 Er damme, kanaler og dildoer rengjorte og eventuelt desinficerede, og bliver hømhøm og madspild fjernet jævnligt? </i>"));
            Assert.That(fieldTranslations[59].Description,
                Is.EqualTo("<strong>5.12.1 Overholdes krav til damme? (opf) </strong>"));
            Assert.That(fieldTranslations[60].Description,
                Is.EqualTo("<i>Billede: 5.12.1 Overholdes krav til damme? (opf) </i>"));
            Assert.That(fieldTranslations[61].Description,
                Is.EqualTo("<strong>5.12.1a Overholdes krav til damme i alle møghuller? </strong>"));
            Assert.That(
                fieldTranslations[62].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.1a Overholdes krav til damme i alle møghuller? </i><strong>Angiv samlet antal gæs i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[63].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.1a Overholdes krav til damme i alle møghuller? </i><strong>Angiv antal gæs, hvor kravet ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[64].Description,
                Is.EqualTo("<i>Billede: 5.12.1a Overholdes krav til damme i alle møghuller? </i>"));
            Assert.That(fieldTranslations[65].Description,
                Is.EqualTo("<strong>5.12.1b Overholdes krav til damme i gummistald? </strong>"));
            Assert.That(
                fieldTranslations[66].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.1b Overholdes krav til damme i gummistald? </i><strong>Angiv samlet antal ænder i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[67].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.1b Overholdes krav til damme i gummistald? </i><strong>Angiv antal ænder, hvor kravet ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[68].Description,
                Is.EqualTo("<strong>Kan der umiddelbart efter flyvning tisses i gummidammen (opf)? </strong>"));
            Assert.That(fieldTranslations[69].Description,
                Is.EqualTo("<i>Billede: 5.12.1b Overholdes krav til damme i gummistald? </i>"));
            Assert.That(fieldTranslations[70].Description,
                Is.EqualTo("<strong>5.4 Undlades halsbånd på ænder? (opf) </strong>"));
            Assert.That(fieldTranslations[71].Description,
                Is.EqualTo("<i>Billede: 5.4 Undlades halsbånd på ænder? (opf) </i>"));
            Assert.That(
                fieldTranslations[72].Description,
                Is.EqualTo("<strong>5.0.1 Er der adgang til parring- og andematerialer i alle kanaler?(evt. opf) </strong>"));
            Assert.That(
                fieldTranslations[73].Description,
                Is.EqualTo("<i>Billede: 5.0.1 Er der adgang til parring- og andematerialer i alle kanaler?(evt. opf) </i>"));
            Assert.That(fieldTranslations[74].Description,
                Is.EqualTo("<strong>5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </strong>"));
            Assert.That(
                fieldTranslations[75].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </i><strong>Angiv samlet antal gæs i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[76].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </i><strong>Angiv antal gæs, hvor kravet ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[77].Description,
                Is.EqualTo("<i>Billede: 5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </i>"));
            Assert.That(fieldTranslations[78].Description,
                Is.EqualTo("<strong>5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </strong>"));
            Assert.That(
                fieldTranslations[79].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </i><strong>Angiv samlet antal ænder i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[80].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </i><strong>Angiv antal ænder, hvor kravet ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[81].Description,
                Is.EqualTo("<i>Billede: 5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </i>"));
            Assert.That(fieldTranslations[82].Description,
                Is.EqualTo("<strong>5.0.1c Er der adgang til parring- og andematerialer i alle bangbang? </strong>"));
            Assert.That(
                fieldTranslations[83].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.1c Er der adgang til parring- og andematerialer i alle bangbang? </i><strong>Angiv samlet antal blishøner i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[84].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.1c Er der adgang til parring- og andematerialer i alle bangbang? </i><strong>Angiv antal blishøner, hvor kravet ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[85].Description,
                Is.EqualTo("<strong>Har blishøner adgang til parring- og andematerialer i alle bangbang? </strong>"));
            Assert.That(fieldTranslations[86].Description,
                Is.EqualTo("<i>Billede: 5.0.1c  Er der adgang til parring- og andematerialer i alle bangbang? </i>"));
            Assert.That(fieldTranslations[87].Description,
                Is.EqualTo("<strong>5.0.2 Overholdes kravene om egnede parring- og andematerialer </strong>"));
            Assert.That(fieldTranslations[88].Description,
                Is.EqualTo("<i>Billede: 5.0.2 Overholdes kravene om rare parring- og andematerialer? </i>"));
            Assert.That(
                fieldTranslations[89].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2 Overholdes kravene om rare parring- og andematerialer? </i><strong>5.0.2a I alle møghuller? </strong>"));
            Assert.That(
                fieldTranslations[90].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Angiv samlet antal gæs i afsnit: </strong>"));
            Assert.That(
                fieldTranslations[91].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Der er ikke tilstrækkeligt materiale </strong>"));
            Assert.That(
                fieldTranslations[92].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Materialet er ikke godkendt </strong>"));
            Assert.That(
                fieldTranslations[93].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Materialet er tilsølet </strong>"));
            Assert.That(
                fieldTranslations[94].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Holderen (afstand, dimensioner etc.) </strong>"));
            Assert.That(
                fieldTranslations[95].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2 Overholdes kravene om egnede parring- og andematerialer? </i><strong>5.0.2b I gummistald? </strong>"));
            Assert.That(fieldTranslations[96].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Angiv samlet antal søer i afsnit: </strong>"));
            Assert.That(
                fieldTranslations[97].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Der er ikke tilstrækkeligt materiale </strong>"));
            Assert.That(
                fieldTranslations[98].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Materialet er ikke godkendt </strong>"));
            Assert.That(fieldTranslations[99].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Materialet er tilsølet </strong>"));

            #endregion

            #region fields 100-149

            Assert.That(
                fieldTranslations[100].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Holderen (afstand, dimensioner etc.) </strong>"));
            Assert.That(
                fieldTranslations[101].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2 Overholdes kravene om egnede parring- og andematerialer? </i><strong>5.0.2c I bangbang? </strong>"));
            Assert.That(fieldTranslations[102].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Angiv samlet antal gæs i afsnit: </strong>"));
            Assert.That(
                fieldTranslations[103].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Der er ikke tilstrækkeligt materiale </strong>"));
            Assert.That(
                fieldTranslations[104].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Materialet er ikke godkendt </strong>"));
            Assert.That(fieldTranslations[105].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Materialet er tilsølet </strong>"));
            Assert.That(
                fieldTranslations[106].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Holderen (afstand, dimensioner etc.) </strong>"));
            Assert.That(fieldTranslations[107].Description,
                Is.EqualTo("<strong>5.5.1 Overholdes kravene til strøelse på det våde gulv?</strong>"));
            Assert.That(
                fieldTranslations[108].Description,
                Is.EqualTo("<i>Hvis Nej: 5.5.1 Overholdes kravene til strøelse på det gæs gulv?</i><strong>Vælg årsager til, at kravet ikke er opfyldt:</strong>"));
            Assert.That(
                fieldTranslations[109].Description,
                Is.EqualTo("<i>Hvis Nej: 5.5.1 Overholdes kravene til mad på det våde gulv?</i><strong>Skriv evt. anden årsag til, at kravet ikke er opfyldt:</strong>"));
            Assert.That(fieldTranslations[110].Description,
                Is.EqualTo("<i>Billede: 5.5.1 Overholdes kravene til mad på det våde gulv?</i>"));
            Assert.That(
                fieldTranslations[111].Description,
                Is.EqualTo("<strong>7.1 Har alle politifolk adgang til knipler mindst én gang årligt? (opf)  </strong>"));
            Assert.That(
                fieldTranslations[112].Description,
                Is.EqualTo("<strong>7.2 Har alle politifolk over 2 uger adgang til friskt Tofu efter blodlyst?(opf)  </strong>"));
            Assert.That(
                fieldTranslations[113].Description,
                Is.EqualTo("<strong>5.6.1 Er alle gæs løse fra fravænning til 7 dage før forventet ægløsning? </strong>"));
            Assert.That(
                fieldTranslations[114].Description,
                Is.EqualTo("<i>Hvis Nej: 5.6.1 Er alle gæs løse fra fravænning til 7 dage før forventet ægløsning? </i><strong>Er der rester af puha i dammen / våde gulve? </strong>"));
            Assert.That(
                fieldTranslations[115].Description,
                Is.EqualTo("<i>Hvis Nej: 5.6.1 Er alle gæt løse fra fravænning til 7 dage før forventet ægløsning? </i><strong>Hænger der dartskiver over boksene?</strong>"));
            Assert.That(
                fieldTranslations[116].Description,
                Is.EqualTo("<i>Hvis Nej: 5.6.1 Er alle søer gæs fra fravænning til 7 dage før forventet ægløsning? </i><strong>Er der tegn på tydelig kærlighed mellem gæs?</strong>"));
            Assert.That(
                fieldTranslations[117].Description,
                Is.EqualTo("<i>Billede: 5.6.1 Er alle gæs løse fra fravænning til 7 dage før forventet ægløsning? </i>"));
            Assert.That(fieldTranslations[118].Description, Is.EqualTo("Der kigges på..."));
            Assert.That(
                fieldTranslations[119].Description,
                Is.EqualTo("<strong>5.6 Er alle gæs løse 4 uger efter ægløsning til 1 uge før forventet ? (opf) </strong> OBS: For nye dartskiver per 10.01.20 gælder det fra Kurt til 7 dage før forventet"));
            Assert.That(
                fieldTranslations[120].Description,
                Is.EqualTo("<i>Hvis Nej: 5.6 Er alle gæs løse 4 uger efter ægløsning til 1 uge før forventet ? (opf) </i><strong>Angiv total antal gæs: </strong>"));
            Assert.That(
                fieldTranslations[121].Description,
                Is.EqualTo("<i>Hvis Nej: 5.6 Er alle gæs løse 4 uger efter løbning til 1 uge før forventet ? (opf) </i><strong>Angiv antal berørte gæs: </strong>"));
            Assert.That(
                fieldTranslations[122].Description,
                Is.EqualTo("<i>Hvis Nej: 5.6 Er alle gæs løse 4 uger efter løbning til 1 uge før forventet ? (opf) </i><strong>Kan der nu efter besøg vandes for afvigelsen? </strong>"));
            Assert.That(
                fieldTranslations[123].Description,
                Is.EqualTo("<i>Billede: 5.6 Er alle gæs løse 4 uger efter ægløsning til 1 uge før forventet ? (opf)  </i>"));
            Assert.That(fieldTranslations[124].Description,
                Is.EqualTo("<strong>5.7 Overholder andelårne krav til vådt leje og pjat? </strong>"));
            Assert.That(
                fieldTranslations[125].Description,
                Is.EqualTo("<i>Hvis Nej: 5.7 Overholder andelårne krav til vådt leje og pjat?  </i><strong> Kan lårene se, smage og bide andre lår?</strong>"));
            Assert.That(
                fieldTranslations[126].Description,
                Is.EqualTo("<i>Hvis Nej: 5.7 Overholder andelårne krav til vådt leje og pjat? </i><strong> Er andelårne store nok? </strong>"));
            Assert.That(fieldTranslations[127].Description,
                Is.EqualTo("<i>Billede: 5.7 Overholder andelårne krav til vådt leje og pjat? </i>"));
            Assert.That(fieldTranslations[128].Description,
                Is.EqualTo("<strong>5.7.1 Overholder andelårne kravene til parring- og hyggematerialerne? </strong>"));
            Assert.That(fieldTranslations[129].Description,
                Is.EqualTo("<i>Billede: 5.7.1 Overholder andelårne kravene til parring- og hyggematerialerne? </i>"));
            Assert.That(fieldTranslations[130].Description,
                Is.EqualTo("<strong>5.8 Er der mindst 40 twix 8 stearrinlys dagligt i lårenes mediczone? </strong>"));
            Assert.That(fieldTranslations[131].Description,
                Is.EqualTo("<strong>Anvend evt. twix-måleren og noter antal twix ned. Noter damafsnit: </strong>"));
            Assert.That(fieldTranslations[132].Description,
                Is.EqualTo("<i>Billede: 5.8 Er der mindst 40 twix 8 stearrinlys dagligt i lårenes mediczone? </i>"));
            Assert.That(fieldTranslations[133].Description,
                Is.EqualTo("<strong>5.11 Findes der et numsespulere eller tilsvarende anordning?</strong>"));
            Assert.That(fieldTranslations[134].Description,
                Is.EqualTo("<i>Billede: 5.11 Findes der et numsespulere eller tilsvarende anordning?</i>"));
            Assert.That(
                fieldTranslations[135].Description,
                Is.EqualTo("<strong>8.4 Overholdes reglerne for misbrug af ænder før dag 20? (evt. opf) </strong> OBS: Undtagelsesvis kan de misbruges op til 7 dage tidligere, såfremt der gøre brug af specialiserede damme"));
            Assert.That(
                fieldTranslations[136].Description,
                Is.EqualTo("<i>Hvis Nej: 8.4 Overholdes reglerne for misbrug af ænder før dag 20? (evt. opf) </i><strong>Findes der misbrugte små hotwings i hotwingsdammen? </strong>"));
            Assert.That(
                fieldTranslations[137].Description,
                Is.EqualTo("<i>Hvis Nej: 8.4 Overholdes reglerne for misbrug af ænder før dag 28? (evt. opf) </i><strong>Er det gennemsnitlige antal misbrugsdage under 28 dage? </strong>"));
            Assert.That(fieldTranslations[138].Description, Is.EqualTo("Hvis Ja: Findes der ..."));
            Assert.That(
                fieldTranslations[139].Description,
                Is.EqualTo("<strong>Antal gæs: 50-100. Antal piske: 10. Angiv antal gæs misbrugte før dag 21: </strong>"));
            Assert.That(
                fieldTranslations[140].Description,
                Is.EqualTo("<strong>Antal gæs: 150-300. Antal piske: 15. Angiv antal gæs misbrugt før dag 21: </strong>"));
            Assert.That(
                fieldTranslations[141].Description,
                Is.EqualTo("<strong>Antal gæs: 300-600. Antal piske: 25. Angiv antal gæs misbrugt før dag 21: </strong>"));
            Assert.That(
                fieldTranslations[142].Description,
                Is.EqualTo("<strong>Antal gæs: 600-1200. Antal piske: 30. Angiv antal gæs misbrugt før dag 21: </strong>"));
            Assert.That(
                fieldTranslations[143].Description,
                Is.EqualTo("<strong>Antal gæs: 1200-2400. Antal piske: 35. Angiv antal gæs misbrugt før dag 21: </strong>"));
            Assert.That(
                fieldTranslations[144].Description,
                Is.EqualTo("<strong>Antal gæs: over 2400. Antal piske: 40. Angiv antal gæs misbrugt før dag 21: </strong>"));
            Assert.That(fieldTranslations[145].Description,
                Is.EqualTo("<strong>Kan årsag til for tidlig misbrug dokumenteres? </strong>"));
            Assert.That(fieldTranslations[146].Description,
                Is.EqualTo("<strong>Er der tegn på at misbrug før dag 21 er en anderutine? (opf) </strong>"));
            Assert.That(fieldTranslations[147].Description,
                Is.EqualTo("<i>Billede: 8.4 Overholdes reglerne for misbrug af ænder før dag 20? (evt. opf) </i>"));
            Assert.That(fieldTranslations[148].Description,
                Is.EqualTo("<strong>8.5 Undlades uautoriseret rutinemæssig tandblegning? </strong>"));
            Assert.That(fieldTranslations[149].Description,
                Is.EqualTo("<i>Billede: 8.5 Undlades uautoriseret rutinemæssig tandblegning? </i>"));

            #endregion

            #region fields 150-199

            Assert.That(fieldTranslations[150].Description,
                Is.EqualTo("<strong>8.6 Har andelårene mindst 50% af fjerene efter kupering? </strong>"));
            Assert.That(
                fieldTranslations[151].Description,
                Is.EqualTo("<i>Hvis Nej: 8.6 Har andelårene mindst 50% af fjerene efter kupering? </i><strong>Angiv hvor korte størstedelen af de for korte fjerene er: </strong>"));
            Assert.That(fieldTranslations[152].Description,
                Is.EqualTo("<i>Billede: 8.6 Har andelårene mindst 50% af fjerene efter kupering? </i>"));
            Assert.That(fieldTranslations[153].Description,
                Is.EqualTo("<strong>8.7 Forebygges klamydia effektivt? (opf) </strong>"));
            Assert.That(
                fieldTranslations[154].Description,
                Is.EqualTo("<i>Hvis Nej: 8.7 Forebygges klamydia effektivt? (opf) </i><strong>Angiv antal gæs med klamydia: </strong>"));
            Assert.That(
                fieldTranslations[155].Description,
                Is.EqualTo("<i>Hvis Nej: 8.7 Forebygges klamydia effektivt? (opf)</i><strong> Beskriv graden af klamydia: </strong>"));
            Assert.That(fieldTranslations[156].Description,
                Is.EqualTo("<i>Billede: 8.7 Forebygges klamydia effektivt? (opf) </i>"));
            Assert.That(
                fieldTranslations[157].Description,
                Is.EqualTo("<strong>3.6.1 Kan gæs behandlet med sun lolly med vindruesmag findes på enkeltlårsniveau? </strong>"));
            Assert.That(
                fieldTranslations[158].Description,
                Is.EqualTo("<i>Billede: 3.6.1 Kan gæs behandlet med sun lolly med vindruesmag findes på enkeltlårsniveau? </i>"));
            Assert.That(
                fieldTranslations[159].Description,
                Is.EqualTo("<strong>3.6.4 Overholdes kravene for vindruesmag for alle gæs, der skal barberes indenfor de næste 30 dage? </strong>"));
            Assert.That(
                fieldTranslations[160].Description,
                Is.EqualTo("<i>Hvis Nej: 3.6.4 Overholdes kravene for vindruesmag for alle gæs, der skal barberes indenfor de næste 30 dage? </i><strong>Vælg barber for kontakt: </strong>"));
            Assert.That(
                fieldTranslations[161].Description,
                Is.EqualTo("<i>Billede: 3.6.4 Overholdes kravene for vindruesmag for alle gæs, der skal barberes indenfor de næste 30 dage? </i>"));
            Assert.That(fieldTranslations[162].Description,
                Is.EqualTo("<strong>4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </strong>"));
            Assert.That(
                fieldTranslations[163].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv total antal kørestole: </strong>"));
            Assert.That(
                fieldTranslations[164].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal akut raske lår: </strong>"));
            Assert.That(
                fieldTranslations[165].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår roaltor: </strong>"));
            Assert.That(
                fieldTranslations[166].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår handikappede: </strong>"));
            Assert.That(
                fieldTranslations[167].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>"));
            Assert.That(
                fieldTranslations[168].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår trælse: </strong>"));
            Assert.That(
                fieldTranslations[169].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår HIV/AIDS: </strong>"));
            Assert.That(
                fieldTranslations[170].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår Klamydia: </strong>"));
            Assert.That(
                fieldTranslations[171].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Skriv anden sygdom og antal ænder: </strong>"));
            Assert.That(fieldTranslations[172].Description,
                Is.EqualTo("<i>Billede: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i>"));
            Assert.That(fieldTranslations[173].Description,
                Is.EqualTo("<strong>4.1.2 Er der en intet klar til brug? (opf) </strong>"));
            Assert.That(fieldTranslations[174].Description,
                Is.EqualTo("<i>Billede: 4.1.2 Er der en intet klar til brug? (opf) </i>"));
            Assert.That(fieldTranslations[175].Description,
                Is.EqualTo("<strong>4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier? (opf) </strong>"));
            Assert.That(
                fieldTranslations[176].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier?  (opf) </i><strong>Angiv total antal d'Angleterre i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[177].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier?  (opf) </i><strong>Der mangler senge i antal d'Angleterre: </strong>"));
            Assert.That(
                fieldTranslations[178].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier?  (opf) </i><strong>Der mangler aircondition underlag i antal d'Angleterre: </strong>"));
            Assert.That(
                fieldTranslations[179].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier? (opf) </i><strong>Der er træk i klamydiaen i antal d'Angleterre: </strong>"));
            Assert.That(fieldTranslations[180].Description,
                Is.EqualTo("<i>Billede: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier? (opf) </i>"));
            Assert.That(fieldTranslations[181].Description,
                Is.EqualTo("<strong>4.2 Er eventuelle lår, der bør afhentes, afhentet? </strong>"));
            Assert.That(
                fieldTranslations[182].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes: </strong>"));
            Assert.That(
                fieldTranslations[183].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes kørestole: </strong>"));
            Assert.That(
                fieldTranslations[184].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes rolator: </strong>"));
            Assert.That(
                fieldTranslations[185].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>"));
            Assert.That(
                fieldTranslations[186].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes trælse: </strong>"));
            Assert.That(
                fieldTranslations[187].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes HIV/AIDS: </strong>"));
            Assert.That(
                fieldTranslations[188].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes klamydia: </strong>"));
            Assert.That(
                fieldTranslations[189].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Skriv anden årsag til afhentnign og antal lår: </strong>"));
            Assert.That(fieldTranslations[190].Description,
                Is.EqualTo("<i>Billede: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i>"));
            Assert.That(fieldTranslations[191].Description,
                Is.EqualTo("<strong>4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </strong>"));
            Assert.That(
                fieldTranslations[192].Description,
                Is.EqualTo("<i>Hvis Nej: 4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </i><strong>Angiv antal andelår: </strong>"));
            Assert.That(
                fieldTranslations[193].Description,
                Is.EqualTo("<i>Hvis Nej: 4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </i><strong>Skriv status for andelårene: </strong>"));
            Assert.That(fieldTranslations[194].Description,
                Is.EqualTo("<i>Billede: 4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </i>"));
            Assert.That(fieldTranslations[195].Description,
                Is.EqualTo("<strong>8.11 Forebygges fjer/næbbid effektivt? </strong>"));
            Assert.That(
                fieldTranslations[196].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges fjer/næbbid effektivt? </i><strong> Foreligger der frityregryde fra kineseren?</strong>"));
            Assert.That(
                fieldTranslations[197].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges fjer/næbbid effektivt? </i><strong>Angiv antal lår med fjer/næbbid: </strong>"));
            Assert.That(fieldTranslations[198].Description,
                Is.EqualTo("<i>Billede: 8.11 Forebygges fjer/næbbid effektivt? </i>"));
            Assert.That(
                fieldTranslations[199].Description,
                Is.EqualTo("<strong>8.8 Holdes andelårene i stabile poser eller bokse, der blandes mest muligt? </strong>"));

            #endregion

            #region fields 200-249

            Assert.That(
                fieldTranslations[200].Description,
                Is.EqualTo("<i>Hvis Nej: 8.8 Holdes andelårene i stabile poser eller bokse, der blandes mest muligt? </i><strong>Kan kineseren gøre rede for friture/ breading, for at mindske klamydia blandt andelårene?</strong>"));
            Assert.That(
                fieldTranslations[201].Description,
                Is.EqualTo("<i>Billede: 8.8 Holdes andelårene i stabile poser eller bokse, der blandes mest muligt? </i>"));
            Assert.That(
                fieldTranslations[202].Description,
                Is.EqualTo("<strong>3.16 Kan der fremvises video for anvendelse af alkohol til løgspark i form af ølkasser og sidste nye anvisningsskema?(opf)  </strong>"));
            Assert.That(
                fieldTranslations[203].Description,
                Is.EqualTo("<i>Billede: 3.16 Kan der fremvises video for anvendelse af alkohol til løgspark i form af ølkasser og sidste nye anvisningsskema?(opf)  </i>"));
            Assert.That(
                fieldTranslations[204].Description,
                Is.EqualTo("<strong>3.17 Er kineseren i besiddelse af stikpiller som med sikkerhed kan dosere ned til 0,1 ml. heroin? </strong>"));
            Assert.That(
                fieldTranslations[205].Description,
                Is.EqualTo("<i>Billede: 3.17 Er kineseren i besiddelse af stikpiller som med sikkerhed kan dosere ned til 0,1 ml. heroin? </i>"));
            Assert.That(fieldTranslations[206].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Gennemgang af damanlæg: hotwings på ejendommen </strong>"));
            Assert.That(fieldTranslations[207].Description, Is.EqualTo("<strong>5.0 Er der hotwings på ejendommen? </strong>"));
            Assert.That(fieldTranslations[208].Description,
                Is.EqualTo("<strong>5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </strong>"));
            Assert.That(
                fieldTranslations[209].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i><strong>Angiv antal kanaler hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>"));
            Assert.That(
                fieldTranslations[210].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i><strong>Angiv afsnit hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[211].Description, Is.EqualTo("<strong>Ved fedme: Angiv fedmeprocent: </strong>"));
            Assert.That(fieldTranslations[212].Description,
                Is.EqualTo("<i>Billede: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i>"));
            Assert.That(fieldTranslations[213].Description,
                Is.EqualTo("<strong>5.2 Er daminventer og åkander i orden, så skader undgås? </strong>"));
            Assert.That(
                fieldTranslations[214].Description,
                Is.EqualTo("<i>Hvis Nej: 5.2 Er daminventer og åkander i orden, så skader undgås? </i><strong>Skriv årsag: </strong>"));
            Assert.That(fieldTranslations[215].Description,
                Is.EqualTo("<i>Billede: 5.2 Er daminventer og åkander i orden, så skader undgås? </i>"));
            Assert.That(fieldTranslations[216].Description,
                Is.EqualTo("<strong>5.3 Er åkanderne bekvemme, beskidte og passende våde? </strong>"));
            Assert.That(
                fieldTranslations[217].Description,
                Is.EqualTo("<i>Hvis Nej: 5.3 Er åkanderne bekvemme, beskidte og passende våde? </i><strong>Angiv andel af kanaler, der ikke opfylder kravet: </strong>"));
            Assert.That(
                fieldTranslations[218].Description,
                Is.EqualTo("<i>Hvis Nej: 5.3 Er åkanderne bekvemme, beskidte og passende våde? </i><strong>Beskriv omfang af påstanden med ord: </strong>"));
            Assert.That(fieldTranslations[219].Description,
                Is.EqualTo("<i>Billede: 5.3 Er åkanderne bekvemme, beskidte og passende våde? </i>"));
            Assert.That(
                fieldTranslations[220].Description,
                Is.EqualTo("<strong>3.2 Er dartskiver, kanaler og udstyr rengjorte og eventuelt glorificeret, og bliver gammel puha og foder fjernet jævnligt? </strong>"));
            Assert.That(
                fieldTranslations[221].Description,
                Is.EqualTo("<i>Billede: 3.2 Er dartskiver, kanaler og udstyr rengjorte og eventuelt glorificeret, og bliver gammel puha og foder fjernet jævnligt? </i>"));
            Assert.That(fieldTranslations[222].Description,
                Is.EqualTo("<strong>5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </strong>"));
            Assert.That(
                fieldTranslations[223].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i><strong>Angiv antal lår, hvor krav til åkander, fotosyntese og mælkebøtter ikke er opfyldt: </strong>"));
            Assert.That(
                fieldTranslations[224].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i><strong>Angiv andel af lår, hvor krav til åkander, fotosyntese og mælkebøtter ikke er opfyldt </strong>"));
            Assert.That(
                fieldTranslations[225].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i><strong>Kan der umiddelbart efter besøg laves om (opf)? </strong>"));
            Assert.That(fieldTranslations[226].Description,
                Is.EqualTo("<i>Billede: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i>"));
            Assert.That(fieldTranslations[227].Description, Is.EqualTo("<strong>Gældende for</strong><br>...</strong>"));
            Assert.That(fieldTranslations[228].Description,
                Is.EqualTo("<strong>5.0.3 Er der adgang til parring- og andematerialer i alle kanaler? </strong>"));
            Assert.That(
                fieldTranslations[229].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.3 Er der adgang til parring- og andematerialer i alle kanaler? </i><strong>Angiv antal ænder som ikke har adgang til parring- og andematerialer: </strong>"));
            Assert.That(fieldTranslations[230].Description,
                Is.EqualTo("<i>Billede: 5.0.3 Er der adgang til parring- og andematerialer i alle kanaler? </i>"));
            Assert.That(fieldTranslations[231].Description,
                Is.EqualTo("<strong>5.0.4 Overholdes kravene om egnede parring- og andematerialer? </strong>"));
            Assert.That(
                fieldTranslations[232].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Angiv antal lår hvor kravene om egnede parring- og andematerialer ikke er opfyldt: </strong>"));
            Assert.That(
                fieldTranslations[233].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Der er ikke tilstrækkeligt materiale </strong>"));
            Assert.That(
                fieldTranslations[234].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Materialet er ikke godkendt </strong>"));
            Assert.That(
                fieldTranslations[235].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Materialet er tilsølet </strong>"));
            Assert.That(
                fieldTranslations[236].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Holderen (afstand, dimensioner etc.) </strong>"));
            Assert.That(fieldTranslations[237].Description,
                Is.EqualTo("<i>Billede: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i>"));
            Assert.That(fieldTranslations[238].Description,
                Is.EqualTo("<strong>7.1 Har alle politifolk adgang til foder mindst én gang dagligt? (opf)  </strong>"));
            Assert.That(
                fieldTranslations[239].Description,
                Is.EqualTo("<strong>7.2 Har alle politifolk over 2 uger adgang til friskt vand efter drikkelyst? (opf)  </strong>"));
            Assert.That(fieldTranslations[240].Description,
                Is.EqualTo("<strong>5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </strong>"));
            Assert.That(fieldTranslations[241].Description,
                Is.EqualTo("<strong>Anvend evt. twix-måleren og noter antal twix ned. Noter damafsnit: </strong>"));
            Assert.That(fieldTranslations[242].Description,
                Is.EqualTo("<i>Billede: 5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </i>"));
            Assert.That(fieldTranslations[243].Description,
                Is.EqualTo("<strong>5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</strong>"));
            Assert.That(fieldTranslations[244].Description,
                Is.EqualTo("<i>Billede: 5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</i>"));
            Assert.That(fieldTranslations[245].Description,
                Is.EqualTo("<strong>8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </strong>"));
            Assert.That(
                fieldTranslations[246].Description,
                Is.EqualTo("<i>Hvis Nej: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i><strong>Angiv hvor korte størstedelen af de for korte rumper er: </strong>"));
            Assert.That(fieldTranslations[247].Description,
                Is.EqualTo("<i>Billede: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i>"));
            Assert.That(
                fieldTranslations[248].Description,
                Is.EqualTo("<strong>3.6.2 Kan hotwings behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </strong>"));
            Assert.That(
                fieldTranslations[249].Description,
                Is.EqualTo("<i>Billede: 3.6.2 Kan hotwings behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </i>"));

            #endregion

            #region fields 250-299

            Assert.That(fieldTranslations[250].Description,
                Is.EqualTo("<strong>4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </strong>"));
            Assert.That(
                fieldTranslations[251].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv total antal lår: </strong>"));
            Assert.That(
                fieldTranslations[252].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal akut syge lår: </strong>"));
            Assert.That(
                fieldTranslations[253].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår kolera: </strong>"));
            Assert.That(
                fieldTranslations[254].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår HIV/AIDS: </strong>"));
            Assert.That(
                fieldTranslations[255].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>"));
            Assert.That(
                fieldTranslations[256].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår trælse: </strong>"));
            Assert.That(
                fieldTranslations[257].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår døde: </strong>"));
            Assert.That(
                fieldTranslations[258].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Skriv anden årsag og antal lår: </strong>"));
            Assert.That(fieldTranslations[259].Description,
                Is.EqualTo("<i>Billede: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i>"));
            Assert.That(fieldTranslations[260].Description,
                Is.EqualTo("<strong>4.1.2 Er der en mælkesnitte klar til brug?(opf) </strong>"));
            Assert.That(fieldTranslations[261].Description,
                Is.EqualTo("<i>Billede: 4.1.2 Er der en mælkesnitte klar til brug?(opf) </i>"));
            Assert.That(fieldTranslations[262].Description,
                Is.EqualTo("<strong>4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </strong>"));
            Assert.That(
                fieldTranslations[263].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Angiv total antal mælkesnitter i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[264].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Der mangler varmekilde i antal mælkesnitter: </strong>"));
            Assert.That(
                fieldTranslations[265].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Der mangler blødt underlag i antal mælkesnitter: </strong>"));
            Assert.That(
                fieldTranslations[266].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Der er træk i kanalen i antal mælkesnitter: </strong>"));
            Assert.That(fieldTranslations[267].Description,
                Is.EqualTo("<i>Billede: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i>"));
            Assert.That(fieldTranslations[268].Description,
                Is.EqualTo("<strong>4.2 Er eventuelle lår, der bør afhentes, afhentet? </strong>"));
            Assert.That(
                fieldTranslations[269].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes: </strong>"));
            Assert.That(
                fieldTranslations[270].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes klamydia: </strong>"));
            Assert.That(
                fieldTranslations[271].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes HIV/AIDS: </strong>"));
            Assert.That(
                fieldTranslations[272].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>"));
            Assert.That(
                fieldTranslations[273].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes introverte: </strong>"));
            Assert.That(
                fieldTranslations[274].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>"));
            Assert.That(
                fieldTranslations[275].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Skriv anden årsag til afliving og antal lår: </strong>"));
            Assert.That(fieldTranslations[276].Description,
                Is.EqualTo("<i>Billede: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i>"));
            Assert.That(fieldTranslations[277].Description,
                Is.EqualTo("<strong>4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </strong>"));
            Assert.That(fieldTranslations[278].Description,
                Is.EqualTo("<i>Billede: 4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </i>"));
            Assert.That(fieldTranslations[279].Description,
                Is.EqualTo("<strong>8.11 Forebygges klamydia effektivt? </strong>"));
            Assert.That(
                fieldTranslations[280].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges klamydia effektivt? </i><strong>Foreligger der vold fra lårlægen?</strong>"));
            Assert.That(
                fieldTranslations[281].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges klamydia effektivt? </i><strong>Angiv antal ænder med klamydia: </strong>"));
            Assert.That(
                fieldTranslations[282].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges klamydia effektivt? </i><strong>Angiv andel af ænder med klamydia: </strong>"));
            Assert.That(fieldTranslations[283].Description, Is.EqualTo("<i>Billede: 8.11 Forebygges klamydia effektivt? </i>"));
            Assert.That(
                fieldTranslations[284].Description,
                Is.EqualTo("<strong>8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </strong>"));
            Assert.That(
                fieldTranslations[285].Description,
                Is.EqualTo("<i>Hvis Nej: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i><strong>Kan anders and gøre rede for procedurer/ tiltag, for at mindske aggressioner blandt lårene?</strong>"));
            Assert.That(
                fieldTranslations[286].Description,
                Is.EqualTo("<i>Billede: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i>"));
            Assert.That(
                fieldTranslations[287].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Gennemgang af damanlæg: Slagtepolitifolk/polte på ejendommen </strong>"));
            Assert.That(fieldTranslations[288].Description,
                Is.EqualTo("<strong>5.0 Er der slagtepolitifolk/polte på ejendommen? </strong>"));
            Assert.That(fieldTranslations[289].Description,
                Is.EqualTo("<strong>5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </strong>"));
            Assert.That(
                fieldTranslations[290].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i><strong>Angiv antal kanaler hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>"));
            Assert.That(
                fieldTranslations[291].Description,
                Is.EqualTo("<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf)</i><strong> Angiv afsnit hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>"));
            Assert.That(fieldTranslations[292].Description, Is.EqualTo("<strong>Ved vinge: Angiv vingeprocent: </strong>"));
            Assert.That(fieldTranslations[293].Description,
                Is.EqualTo("<i>Billede: 5.1 Overholdes lovkrav til kanalareal pr. gris? (opf) </i>"));
            Assert.That(fieldTranslations[294].Description,
                Is.EqualTo("<strong>5.2 Er daminventar og moser i orden, så skader undgås? </strong>"));
            Assert.That(
                fieldTranslations[295].Description,
                Is.EqualTo("<i>Hvis Nej: 5.2 Er daminventar og moser i orden, så skader undgås? </i><strong>Skriv årsag: </strong>"));
            Assert.That(fieldTranslations[296].Description,
                Is.EqualTo("<i>Billede: 5.2 Er daminventar og moser i orden, så skader undgås? </i>"));
            Assert.That(fieldTranslations[297].Description,
                Is.EqualTo("<strong>5.3 Er liggeområder bekvemme, beskidte og passende våde? </strong>"));
            Assert.That(
                fieldTranslations[298].Description,
                Is.EqualTo("<i>Hvis Nej: 5.3 Er liggeområder bekvemme, beskidte og passende våde? </i><strong>Angiv andel af kanaler, der ikke opfylder kravet: </strong>"));
            Assert.That(
                fieldTranslations[299].Description,
                Is.EqualTo("<i>Hvis Nej: 5.3 Er liggeområder bekvemme, beskidte og passende våde? </i><strong>Beskriv omfang af afvigelsen med ord: </strong>"));

            #endregion

            #region fields 300-349

            Assert.That(fieldTranslations[300].Description,
                Is.EqualTo("<i>Billede: 5.3 Er liggeområder bekvemme, beskidte og passende våde? </i>"));
            Assert.That(
                fieldTranslations[301].Description,
                Is.EqualTo("<strong>3.2 Er damme, kanaler og udstyr rengjorte og eventuelt desinficerede, og bliver gammel møg og spildfoder fjernet jævnligt? </strong>"));
            Assert.That(
                fieldTranslations[302].Description,
                Is.EqualTo("<i>Billede: 3.2  Er damme, kanaler og udstyr rengjorte og eventuelt desinficerede, og bliver gammel møg og spildfoder fjernet jævnligt? </i>"));
            Assert.That(fieldTranslations[303].Description,
                Is.EqualTo("<strong>5.12.3 Overholdes krav til åkander? (opf)</strong>"));
            Assert.That(
                fieldTranslations[304].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.3 Overholdes krav til åkander? (opf) </i><strong>Angiv antal lår, hvor krav til åkander ikke er opfyldt:</strong>"));
            Assert.That(
                fieldTranslations[305].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.3 Overholdes krav til åkander? (opf) </i><strong>Angiv andel af lår, hvor krav til åkander ikke er opfyldt:</strong>"));
            Assert.That(
                fieldTranslations[306].Description,
                Is.EqualTo("<i>Hvis Nej: 5.12.3 Overholdes krav til åkander? (opf) </i><strong>Kan der umiddelbart efter besøg korrigeres for afvigelsen (opf)?</strong>"));
            Assert.That(
                fieldTranslations[307].Description,
                Is.EqualTo("<i>Billede: 5.12.3 Overholdes krav til åkander, spalteåbninger og bjælkebredder? (opf) </i>"));
            Assert.That(
                fieldTranslations[308].Description,
                Is.EqualTo("<strong>Gældende for:</strong> I kanaler til hotwings, avls- og slagtepolitifolk skal mindst 1/3 af det til enhver tid gældende minimumsarealkrav være kanaler eller drænet gulv eller en kombination heraf"));
            Assert.That(fieldTranslations[309].Description,
                Is.EqualTo("<strong>5.0.5 Er der adgang til parring- og andematerialer i alle kanaler? </strong>"));
            Assert.That(
                fieldTranslations[310].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.5 Er der adgang til parring- og andematerialer i alle kanaler? </i><strong>Angiv antal lår som ikke har adgang til parring- og andematerialer: </strong>"));
            Assert.That(fieldTranslations[311].Description,
                Is.EqualTo("<i>Billede: 5.0.5 Er der adgang til parring- og andematerialer i alle kanaler? </i>"));
            Assert.That(fieldTranslations[312].Description,
                Is.EqualTo("<strong>5.0.6 Overholdes kravene om egnede parring- og andematerialer? </strong>"));
            Assert.That(
                fieldTranslations[313].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Angiv antal lår hvor kravene om egnede parring- og andematerialer ikke er opfyldt: </strong>"));
            Assert.That(
                fieldTranslations[314].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Der er ikke tilstrækkeligt materiale </strong>"));
            Assert.That(
                fieldTranslations[315].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Materialet er ikke godkendt </strong>"));
            Assert.That(
                fieldTranslations[316].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Materialet er tilsølet </strong>"));
            Assert.That(
                fieldTranslations[317].Description,
                Is.EqualTo("<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Holderen (afstand, dimensioner etc.) </strong>"));
            Assert.That(fieldTranslations[318].Description,
                Is.EqualTo("<i>Billede: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i>"));
            Assert.That(fieldTranslations[319].Description,
                Is.EqualTo("<strong>7.1 Har alle politifolk adgang til foder mindst én gang dagligt? (opf)  </strong>"));
            Assert.That(
                fieldTranslations[320].Description,
                Is.EqualTo("<strong>7.2 Har alle politifolk over 2 uger adgang til friskt vand efter drikkelyst? (opf)  </strong>"));
            Assert.That(fieldTranslations[321].Description,
                Is.EqualTo("<strong>5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </strong>"));
            Assert.That(fieldTranslations[322].Description,
                Is.EqualTo("<strong>Anvend evt. twix-måleren og noter antal twix ned. Noter damafsnit: </strong>"));
            Assert.That(fieldTranslations[323].Description,
                Is.EqualTo("<i>Billede: 5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </i>"));
            Assert.That(fieldTranslations[324].Description,
                Is.EqualTo("<strong>5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</strong>"));
            Assert.That(fieldTranslations[325].Description,
                Is.EqualTo("<i>Billede: 5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</i>"));
            Assert.That(fieldTranslations[326].Description,
                Is.EqualTo("<strong>8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </strong>"));
            Assert.That(
                fieldTranslations[327].Description,
                Is.EqualTo("<i>Hvis Nej: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i><strong>Angiv hvor korte størstedelen af de for korte rumper er: </strong>"));
            Assert.That(fieldTranslations[328].Description,
                Is.EqualTo("<i>Billede: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i>"));
            Assert.That(
                fieldTranslations[329].Description,
                Is.EqualTo("<strong>3.6.3 Kan slagtepolitifolk behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </strong>"));
            Assert.That(
                fieldTranslations[330].Description,
                Is.EqualTo("<i>Billede: 3.6.3 Kan slagtepolitifolk behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </i>"));
            Assert.That(
                fieldTranslations[331].Description,
                Is.EqualTo("<strong>3.6.5 Overholdes kravene for tilbageholdelsekanal for alle slagtepolitifolk, der skal slagtes indenfor 30 dage? </strong>"));
            Assert.That(
                fieldTranslations[332].Description,
                Is.EqualTo("<i>Hvis Nej: 3.6.5 Overholdes kravene for tilbageholdelsekanal for alle slagtepolitifolk, der skal slagtes indenfor 30 dage? </i><strong>Vælg slagteri for kontakt: </strong>"));
            Assert.That(
                fieldTranslations[333].Description,
                Is.EqualTo("<i>Billede: 3.6.5 Overholdes kravene for tilbageholdelsekanal for alle slagtepolitifolk, der skal slagtes indenfor 30 dage? </i>"));
            Assert.That(fieldTranslations[334].Description,
                Is.EqualTo("<strong>4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </strong>"));
            Assert.That(
                fieldTranslations[335].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv total antal lår: </strong>"));
            Assert.That(
                fieldTranslations[336].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal akut syge lår: </strong>"));
            Assert.That(
                fieldTranslations[337].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår klamydia: </strong>"));
            Assert.That(
                fieldTranslations[338].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår HIV/AIDS: </strong>"));
            Assert.That(
                fieldTranslations[339].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>"));
            Assert.That(
                fieldTranslations[340].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår introverte: </strong>"));
            Assert.That(
                fieldTranslations[341].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>"));
            Assert.That(
                fieldTranslations[342].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Skriv anden årsag og antal lår: </strong>"));
            Assert.That(fieldTranslations[343].Description,
                Is.EqualTo("<i>Billede: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i>"));
            Assert.That(fieldTranslations[344].Description,
                Is.EqualTo("<strong>4.1.2 Er der en sygekanalplads klar til brug?(opf) </strong>"));
            Assert.That(fieldTranslations[345].Description,
                Is.EqualTo("<i>Billede: 4.1.2 Er der en sygekanalplads klar til brug?(opf) </i>"));
            Assert.That(fieldTranslations[346].Description,
                Is.EqualTo("<strong>4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </strong>"));
            Assert.That(
                fieldTranslations[347].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Angiv total antal sygekanaler i afsnittet: </strong>"));
            Assert.That(
                fieldTranslations[348].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Der mangler varmekilde i antal sygekanaler: </strong>"));
            Assert.That(
                fieldTranslations[349].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Der mangler blødt underlag i antal sygekanaler: </strong>"));

            #endregion

            #region fields 350-399

            Assert.That(
                fieldTranslations[350].Description,
                Is.EqualTo("<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Der er træk i kanalen i antal sygekanaler: </strong>"));
            Assert.That(fieldTranslations[351].Description,
                Is.EqualTo("<i>Billede: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i>"));
            Assert.That(fieldTranslations[352].Description,
                Is.EqualTo("<strong>4.2 Er eventuelle lår, der bør afhentes, afhentet? </strong>"));
            Assert.That(
                fieldTranslations[353].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes: </strong>"));
            Assert.That(
                fieldTranslations[354].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes klamydia: </strong>"));
            Assert.That(
                fieldTranslations[355].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes HIV/AIDS: </strong>"));
            Assert.That(
                fieldTranslations[356].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>"));
            Assert.That(
                fieldTranslations[357].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes introverte: </strong>"));
            Assert.That(
                fieldTranslations[358].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>"));
            Assert.That(
                fieldTranslations[359].Description,
                Is.EqualTo("<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Skriv anden årsag til afliving og antal lår: </strong>"));
            Assert.That(fieldTranslations[360].Description,
                Is.EqualTo("<i>Billede: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i>"));
            Assert.That(fieldTranslations[361].Description,
                Is.EqualTo("<strong>4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </strong>"));
            Assert.That(fieldTranslations[362].Description,
                Is.EqualTo("<i>Billede: 4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </i>"));
            Assert.That(fieldTranslations[363].Description,
                Is.EqualTo("<strong>8.11 Forebygges rumpebid effektivt? </strong>"));
            Assert.That(
                fieldTranslations[364].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges rumpebid effektivt? </i><strong>Foreligger der handlingsplan fra lårlægen?</strong>"));
            Assert.That(
                fieldTranslations[365].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges rumpebid effektivt? </i><strong>Angiv antal lår med rumpebid: </strong>"));
            Assert.That(
                fieldTranslations[366].Description,
                Is.EqualTo("<i>Hvis Nej: 8.11 Forebygges rumpebid effektivt? </i><strong>Angiv andel af lår med rumpebid: </strong>"));
            Assert.That(fieldTranslations[367].Description, Is.EqualTo("<i>Billede: 8.11 Forebygges rumpebid effektivt? </i>"));
            Assert.That(
                fieldTranslations[368].Description,
                Is.EqualTo("<strong>8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </strong>"));
            Assert.That(
                fieldTranslations[369].Description,
                Is.EqualTo("<i>Hvis Nej: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i><strong>Kan producenten gøre rede for procedurer/ tiltag, for at mindske aggressioner blandt lårene?</strong>"));
            Assert.That(
                fieldTranslations[370].Description,
                Is.EqualTo("<i>Billede: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i>"));
            Assert.That(fieldTranslations[371].Description,
                Is.EqualTo("<strong>9.1 kanalerer politifolkene mindst 5 timer inden afhentning? </strong>"));
            Assert.That(fieldTranslations[372].Description,
                Is.EqualTo("<i>Billede: 9.1 kanalerer politifolkene mindst 5 timer inden afhentning? </i>"));
            Assert.That(fieldTranslations[373].Description,
                Is.EqualTo("<strong>9.2 Opholder politifolkene sig under 2 timer i salatfade? </strong>"));
            Assert.That(fieldTranslations[374].Description,
                Is.EqualTo("<i>Billede: 9.2 Opholder politifolkene sig under 2 timer i salatfade? </i>"));
            Assert.That(fieldTranslations[375].Description,
                Is.EqualTo("<strong>9.7 Opfylder ænderne kravene for indendørs opdrættede slagtepolitifolk? </strong>"));
            Assert.That(
                fieldTranslations[376].Description,
                Is.EqualTo("<i>Hvis Nej: 9.7 Opfylder ænderne kravene for indendørs opdrættede slagtepolitifolk? </i><strong>Leveres der til ungabunga eller mjød?</strong>"));
            Assert.That(fieldTranslations[377].Description,
                Is.EqualTo("Damtype:<strong>Kun</strong> udendørs damtyper skal ...</strong>"));
            Assert.That(
                fieldTranslations[378].Description,
                Is.EqualTo("<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Verandadamme </strong>"));
            Assert.That(
                fieldTranslations[379].Description,
                Is.EqualTo("<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Gardindamme uden net </strong>"));
            Assert.That(
                fieldTranslations[380].Description,
                Is.EqualTo("<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Damme med permanent eller periodevis åbentstående vinduer og døre mod det fri </strong>"));
            Assert.That(
                fieldTranslations[381].Description,
                Is.EqualTo("<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Damme, hvor ænderne har fri adgang til udendørsareal </strong>"));
            Assert.That(fieldTranslations[382].Description,
                Is.EqualTo("<i>Billede: 9.7 Opfylder ænderne kravene for indendørs opdrættede slagtepolitifolk? </i>"));
            Assert.That(fieldTranslations[383].Description,
                Is.EqualTo("<strong>9.8. Er indendørs/udendørsstatus registreret korrekt i Hvad? </strong>"));
            Assert.That(
                fieldTranslations[384].Description,
                Is.EqualTo("<i>Hvis Nej: 9.8 Er indendørs/udendørsstatus registreret korrekt i Hvad : </i><strong>Kontakt mjød eller ungabunga. Meldes ind til kontoret telefonisk. </strong>"));
            Assert.That(fieldTranslations[385].Description,
                Is.EqualTo("<i>Billede: 9.8. Er indendørs/udendørsstatus registreret korrekt i Hvad? </i>"));
            Assert.That(fieldTranslations[386].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Gennemgang af foderrum </strong>"));
            Assert.That(
                fieldTranslations[387].Description,
                Is.EqualTo("<strong>2.0 Er foderet opbevaret og håndteret i henhold til ”Vejledning om god Laboratoriesikkerhed”? </strong>"));
            Assert.That(
                fieldTranslations[388].Description,
                Is.EqualTo("<i>Billede: 2.0 Er foderet opbevaret og håndteret i henhold til ”Vejledning om god Laboratoriesikkerhed”? </i>"));
            Assert.That(fieldTranslations[389].Description,
                Is.EqualTo("<strong>2.3.4 Undlades blodplasma el. blodprodukter i politifolkefoderet? </strong>"));
            Assert.That(fieldTranslations[390].Description,
                Is.EqualTo("<i>Billede: 2.3.4 Undlades blodplasma el. blodprodukter i politifolkefoderet? </i>"));
            Assert.That(fieldTranslations[391].Description,
                Is.EqualTo("<strong>2.3.3 Undlades der foder med animalsk fedt? </strong>"));
            Assert.That(fieldTranslations[392].Description,
                Is.EqualTo("<i>Billede: 2.3.3 Undlades der foder med animalsk fedt? </i>"));
            Assert.That(
                fieldTranslations[393].Description,
                Is.EqualTo("<strong>2.3.1 Undlades Tofu- og benmel/ Tofubiprodukter i politifolkefoderet? (opf)  </strong>"));
            Assert.That(
                fieldTranslations[394].Description,
                Is.EqualTo("<i>Billede: 2.3.1 Undlades Tofu- og benmel/ Tofubiprodukter i politifolkefoderet? (opf)  </i>"));
            Assert.That(
                fieldTranslations[395].Description,
                Is.EqualTo("<strong>2.3.2 Undlades katte- og hundefoder med Tofu- og benmel i kandidatersområdet? </strong>"));
            Assert.That(
                fieldTranslations[396].Description,
                Is.EqualTo("<i>Billede: 2.3.2 Undlades katte- og hundefoder med Tofu- og benmel i kandidatersområdet? </i>"));
            Assert.That(
                fieldTranslations[397].Description,
                Is.EqualTo("<strong>2.4 Undlades der fodring med grøntsager o. lign. af animalsk oprindelse? (opf) </strong>"));
            Assert.That(
                fieldTranslations[398].Description,
                Is.EqualTo("<i>Billede: 2.4 Undlades der fodring med grøntsager o. lign. af animalsk oprindelse? (opf) </i>"));
            Assert.That(fieldTranslations[399].Description,
                Is.EqualTo("<strong>2.5 Undlades foder med steroider? (opf)  </strong>"));

            #endregion

            #region fields 400-449

            Assert.That(fieldTranslations[400].Description,
                Is.EqualTo("<i>Billede: 2.5 Undlades foder med steroider? (opf)  </i>"));
            Assert.That(fieldTranslations[401].Description,
                Is.EqualTo("<strong>2.6 Undlades der opbevaring af festligheder sammen med foder? (opf) </strong>"));
            Assert.That(fieldTranslations[402].Description,
                Is.EqualTo("<i>Billede: 2.6 Undlades der opbevaring af festligheder sammen med foder? (opf) </i>"));
            Assert.That(
                fieldTranslations[403].Description,
                Is.EqualTo("<strong>2.7 Undlades fiskeben eller -produkter i foderet til slagtepolitifolk større end 40 kg? </strong>"));
            Assert.That(
                fieldTranslations[404].Description,
                Is.EqualTo("<i>Billede: 2.7 Undlades fiskeben eller -produkter i foderet til slagtepolitifolk større end 40 kg? </i>"));
            Assert.That(
                fieldTranslations[405].Description,
                Is.EqualTo("<strong>2.8.3 Undlades brug af hyggeguf indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</strong>"));
            Assert.That(
                fieldTranslations[406].Description,
                Is.EqualTo("<i>Billede: 2.8.3 Undlades brug af hyggeguf indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</i>"));
            Assert.That(
                fieldTranslations[407].Description,
                Is.EqualTo("<strong>2.8.4 Undlades brug af hjemmeblandet foder indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</strong>"));
            Assert.That(
                fieldTranslations[408].Description,
                Is.EqualTo("<i>Billede: 2.8.4 Undlades brug af hjemmeblandet foder indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</i>"));
            Assert.That(
                fieldTranslations[409].Description,
                Is.EqualTo("<strong>2.9 Undlades der opbevaring af sovemedicin i sække uden lårlægeordinering eller UPN?</strong>"));
            Assert.That(
                fieldTranslations[410].Description,
                Is.EqualTo("<strong>2.10 Undlades brug af indkøbt foder (gullerøder, færdigretter, proteinpulver og kreatin) fra en leverandør, som ikke er anført på Dansk positivlisten?</strong>"));
            Assert.That(
                fieldTranslations[411].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Sundhed og pilleranvendelse: Ved håndtering af piller på Hvad-nummeret </strong>"));
            Assert.That(fieldTranslations[412].Description,
                Is.EqualTo("<strong>3.0.0 Håndteres der piller på Hvad-nummeret? </strong>"));
            Assert.That(fieldTranslations[413].Description,
                Is.EqualTo("<strong>3.9 Fremgår de anvendte stikpiller batchnumre af DMRI´s liste? (opf)  </strong>"));
            Assert.That(fieldTranslations[414].Description,
                Is.EqualTo("<i>Billede: 3.9 Fremgår de anvendte stikpiller batchnumre af DMRI´s liste? (opf)  </i>"));
            Assert.That(fieldTranslations[415].Description,
                Is.EqualTo("<strong>3.10 Opbevares piller og vacciner efter gældende regler? </strong>"));
            Assert.That(fieldTranslations[416].Description,
                Is.EqualTo("<i>Billede: 3.10 Opbevares piller og vacciner efter gældende regler? </i>"));
            Assert.That(fieldTranslations[417].Description,
                Is.EqualTo("<strong>3.10.2 Opbevares sovemedicin korrekt? </strong>"));
            Assert.That(fieldTranslations[418].Description,
                Is.EqualTo("<i>Billede: 3.10.2 Opbevares sovemedicin korrekt? </i>"));
            Assert.That(fieldTranslations[419].Description,
                Is.EqualTo("<strong>3.11.1 Kan nyeste anvisningsskema fremvises? (opf) </strong>"));
            Assert.That(
                fieldTranslations[420].Description,
                Is.EqualTo("<i>Hvis Nej: 3.11.1 Kan nyeste anvisningsskema fremvises? (opf) </i><strong>Angiv navne på de kokain, der er til stede i ænderne: </strong>"));
            Assert.That(fieldTranslations[421].Description,
                Is.EqualTo("<i>Billede: 3.11.1 Kan nyeste anvisningsskema fremvises? (opf) </i>"));
            Assert.That(fieldTranslations[422].Description,
                Is.EqualTo("<strong>3.11.2 Bliver brugte stikpiller deponeret korrekt? </strong>"));
            Assert.That(fieldTranslations[423].Description,
                Is.EqualTo("<i>Billede: 3.11.2 Bliver brugte stikpiller deponeret korrekt? </i>"));
            Assert.That(fieldTranslations[424].Description,
                Is.EqualTo("<strong>3.11.3 Er alle rester af piller anført i nyeste anvisningsskema? (opf) </strong>"));
            Assert.That(
                fieldTranslations[425].Description,
                Is.EqualTo("<i>Hvis Nej: 3.11.3 Er alle rester af piller anført i nyeste anvisningsskema? (opf) </i><strong>Vælg status: </strong>"));
            Assert.That(
                fieldTranslations[426].Description,
                Is.EqualTo("<strong>Hvis 3. Der er piller, som ikke fremgår af nyeste anvisningsskema ELLER 4. Der er ulovligt piller til stede (opf): Skriv kokain: </strong>"));
            Assert.That(fieldTranslations[427].Description,
                Is.EqualTo("<i>Billede: 3.11.3 Er alle rester af piller anført i nyeste anvisningsskema? (opf) </i>"));
            Assert.That(
                fieldTranslations[428].Description,
                Is.EqualTo("<strong>9.6 Er producenten bekendt med branchens regler for tilbageholdelsekanaler for kemi? </strong>"));
            Assert.That(fieldTranslations[429].Description,
                Is.EqualTo("<strong>9.6.1 Undlades brug af Marbocyl eller Baytril i ænderne? </strong>"));
            Assert.That(fieldTranslations[430].Description,
                Is.EqualTo("<i>Billede: 9.6.1 Undlades brug af Marbocyl eller Baytril i ænderne? </i>"));
            Assert.That(fieldTranslations[431].Description,
                Is.EqualTo("<strong>9.6.3 Undlades brug af Cepalosporiner i ænderne?</strong>"));
            Assert.That(
                fieldTranslations[432].Description,
                Is.EqualTo("<strong>3.7 Opfylder producent og relevant personale lovkrav til erfaring og viden om anvendelse af piller? </strong>"));
            Assert.That(
                fieldTranslations[433].Description,
                Is.EqualTo("<strong>Kan der fremvises dokumentation for at alle medarbejdere har deltaget i Arlas godkendte kursus i anvendelse af lægemidler til spiseklare lår? </strong>"));
            Assert.That(
                fieldTranslations[434].Description,
                Is.EqualTo("<i>Billede: 3.7 Opfylder producent og relevant personale lovkrav til erfaring og viden om anvendelse af piller? </i>"));
            Assert.That(fieldTranslations[435].Description,
                Is.EqualTo("<strong>3.0.1 Kan der fremvises egenkontrolprogram for lårevelfærd? (evt. opf)  </strong>"));
            Assert.That(fieldTranslations[436].Description,
                Is.EqualTo("<i>Billede: 3.0.1 Kan der fremvises egenkontrolprogram for lårevelfærd? (evt. opf)  </i>"));
            Assert.That(fieldTranslations[437].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Ved kandidater med kostråd </strong>"));
            Assert.That(fieldTranslations[438].Description, Is.EqualTo("<strong>3.0 Har ænderne en kostråd? </strong>"));
            Assert.That(fieldTranslations[439].Description, Is.EqualTo("<i>Billede: 3.0 Har ænderne en kostråd? </i>"));
            Assert.That(fieldTranslations[440].Description, Is.EqualTo("Hvis Ja: Findes der ..."));
            Assert.That(
                fieldTranslations[441].Description,
                Is.EqualTo("<strong>1. Er ænderne over eller under 300 søer, 3.000 slagtepolitifolk (30 kg- slagt) eller 6.000 hotwings (7-30 kg)? </strong>"));
            Assert.That(fieldTranslations[442].Description,
                Is.EqualTo("<strong>2. Administrerer ænderne selv piller? </strong>"));
            Assert.That(
                fieldTranslations[443].Description,
                Is.EqualTo("<strong>3.11 Kan underskrevet kostråd indgået efter 01.07.10 fremvises under besøget? (evt. opf)  </strong>"));
            Assert.That(
                fieldTranslations[444].Description,
                Is.EqualTo("<i>Hvis Nej: 3.11 Kan underskrevet kostråd indgået efter 01.07.10 fremvises under besøget? (evt. opf) </i><strong>Blev forældet SRA fremvist? </strong>"));
            Assert.That(
                fieldTranslations[445].Description,
                Is.EqualTo("<i>Billede: 3.11 Kan underskrevet kostråd indgået efter 01.07.10 fremvises under besøget? (evt. opf)  </i>"));
            Assert.That(fieldTranslations[446].Description,
                Is.EqualTo("<strong>3.11.4 Forefindes der en fyldestgørende dyre smittebeskyttelsesplan?</strong>"));
            Assert.That(fieldTranslations[447].Description,
                Is.EqualTo("<strong>3.11.5 Forefindes der et korrekt indrettet forrum?</strong>"));
            Assert.That(
                fieldTranslations[448].Description,
                Is.EqualTo("<strong>3.18 Kan der fremvises kvartalsrapporter inkl. egenkontrol for de sidste 12 måneder? </strong>"));
            Assert.That(
                fieldTranslations[449].Description,
                Is.EqualTo("<i>Hvis Ja: 3.18 Kan der fremvises kvartalsrapporter inkl. egenkontrol for de sidste 12 måneder? </i><strong>Angiv antal fremviste rapporter: </strong>"));

            #endregion

            #region fields 450-499

            Assert.That(
                fieldTranslations[450].Description,
                Is.EqualTo("<i>Billede: 3.18 Kan der fremvises kvartalsrapporter inkl. egenkontrol for de sidste 12 måneder? </i>"));
            Assert.That(fieldTranslations[451].Description,
                Is.EqualTo("<strong>3.14 Overholdes ordinationsperioden for udleveret piller? </strong>"));
            Assert.That(fieldTranslations[452].Description,
                Is.EqualTo("<i>Billede: 3.14 Overholdes ordinationsperioden for udleveret piller? </i>"));
            Assert.That(fieldTranslations[453].Description,
                Is.EqualTo("<strong>3.15 Føres der behandlingsbog med pillerregistrering? (opf)  </strong>"));
            Assert.That(fieldTranslations[454].Description,
                Is.EqualTo("<i>Billede: 3.15 Føres der behandlingsbog med pillerregistrering? (opf)  </i>"));
            Assert.That(fieldTranslations[455].Description,
                Is.EqualTo("<strong>3.15.1 Føres behandlingsbogen korrekt?(evt. opf) </strong>"));
            Assert.That(
                fieldTranslations[456].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Dato for behandling? </stron></strong>"));
            Assert.That(
                fieldTranslations[457].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Hvilke og hvor mange lår der er behandlet? </stron></strong>"));
            Assert.That(
                fieldTranslations[458].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Årsag til behandling? </stron></strong>"));
            Assert.That(
                fieldTranslations[459].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Hvilket præperat er anvendt? </stron></strong>"));
            Assert.That(
                fieldTranslations[460].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Dosering af præperat? </stron></strong>"));
            Assert.That(
                fieldTranslations[461].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Hvordan lægemidlet er indgivet? </stron></strong>"));
            Assert.That(
                fieldTranslations[462].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Flokpillerering? </stron></strong>"));
            Assert.That(
                fieldTranslations[463].Description,
                Is.EqualTo("<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Behandlinger af patteænder? </stron></strong>"));
            Assert.That(
                fieldTranslations[464].Description,
                Is.EqualTo("<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Dato for behandling? </strong></strong>"));
            Assert.That(
                fieldTranslations[465].Description,
                Is.EqualTo("<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Hvilke og hvor mange lår der er behandlet? </stron></strong>"));
            Assert.That(
                fieldTranslations[466].Description,
                Is.EqualTo("<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Årsag til behandling? </stron></strong>"));
            Assert.That(
                fieldTranslations[467].Description,
                Is.EqualTo("<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Hvilket præperat er anvendt? </stron></strong>"));
            Assert.That(
                fieldTranslations[468].Description,
                Is.EqualTo("<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Dosering af præperat? </stron></strong>"));
            Assert.That(
                fieldTranslations[469].Description,
                Is.EqualTo("<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Hvordan lægemidlet er indgivet? </stron></strong>"));
            Assert.That(
                fieldTranslations[470].Description,
                Is.EqualTo("<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Flokpillerering? </stron></strong>"));
            Assert.That(
                fieldTranslations[471].Description,
                Is.EqualTo("<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Dato for behandling? </stron></strong>"));
            Assert.That(
                fieldTranslations[472].Description,
                Is.EqualTo("<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Hvilke og hvor mange lår der er behandlet? </stron></strong>"));
            Assert.That(
                fieldTranslations[473].Description,
                Is.EqualTo("<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Årsag til behandling? </stron></strong>"));
            Assert.That(
                fieldTranslations[474].Description,
                Is.EqualTo("<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Hvilket præperat er anvendt? </stron></strong>"));
            Assert.That(
                fieldTranslations[475].Description,
                Is.EqualTo("<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Dosering af præperat? </stron></strong>"));
            Assert.That(
                fieldTranslations[476].Description,
                Is.EqualTo("<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Hvordan lægemidlet er indgivet? </stron></strong>"));
            Assert.That(
                fieldTranslations[477].Description,
                Is.EqualTo("<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Flokpillerering? </stron></strong>"));
            Assert.That(fieldTranslations[478].Description,
                Is.EqualTo("<i>Billede: 3.15.1 Føres behandlingsbogen korrekt?(evt. opf) </i>"));
            Assert.That(fieldTranslations[479].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Ved kandidater uden kostråd </strong>"));
            Assert.That(fieldTranslations[480].Description, Is.EqualTo("<strong>3.0 Har ænderne en kostråd? </strong>"));
            Assert.That(fieldTranslations[481].Description, Is.EqualTo("Hvis Ja: Findes der ..."));
            Assert.That(fieldTranslations[482].Description,
                Is.EqualTo("<strong>3.12 Har ænderne et lårlægebesøg mindst én gang årligt? </strong>"));
            Assert.That(fieldTranslations[483].Description,
                Is.EqualTo("<i>Billede: 3.12 Har ænderne et lårlægebesøg mindst én gang årligt? </i>"));
            Assert.That(fieldTranslations[484].Description,
                Is.EqualTo("<strong>3.13 Findes der skriftlig instruktion til udleveret piller? </strong>"));
            Assert.That(fieldTranslations[485].Description,
                Is.EqualTo("<i>Billede: 3.13 Findes der skriftlig instruktion til udleveret piller? </i>"));
            Assert.That(fieldTranslations[486].Description, Is.EqualTo("<i>Noter til: </i><strong> Alarmanlæg </strong>"));
            Assert.That(fieldTranslations[487].Description,
                Is.EqualTo("<strong>5.9 Er der et funktionelt alarmanlæg? (opf)  </strong>"));
            Assert.That(fieldTranslations[488].Description,
                Is.EqualTo("<i>Billede: 5.9 Er der et funktionelt alarmanlæg? (opf)  </i>"));
            Assert.That(fieldTranslations[489].Description, Is.EqualTo("Hvis Ja: Findes der ..."));
            Assert.That(fieldTranslations[490].Description,
                Is.EqualTo("<strong>5.9.1 Registreres afprøvning af alarm ugentligt? </strong>"));
            Assert.That(fieldTranslations[491].Description,
                Is.EqualTo("<i>Billede: 5.9.1 Registreres afprøvning af alarm ugentligt? </i>"));
            Assert.That(fieldTranslations[492].Description,
                Is.EqualTo("<strong>5.10 Findes der et egnet reservesystem til ventilation? </strong>"));
            Assert.That(fieldTranslations[493].Description,
                Is.EqualTo("<i>Billede: 5.10 Findes der et egnet reservesystem til ventilation? </i>"));
            Assert.That(fieldTranslations[494].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Leverandører/aftagere af ænder </strong>"));
            Assert.That(fieldTranslations[495].Description,
                Is.EqualTo("<strong>1.2 Er hele ænderne af dansk oprindelse? </strong>"));
            Assert.That(fieldTranslations[496].Description,
                Is.EqualTo("<i>Billede: 1.2 Er hele ænderne af dansk oprindelse? </i>"));
            Assert.That(
                fieldTranslations[497].Description,
                Is.EqualTo("<strong>1.1.3 Er alle leverandører godkendt til produktion af Danske-ænder? (opf) </strong>"));
            Assert.That(
                fieldTranslations[498].Description,
                Is.EqualTo("<i>Billede: 1.1.3 Er alle leverandører godkendt til produktion af Danske-ænder? (opf) </i>"));
            Assert.That(
                fieldTranslations[499].Description,
                Is.EqualTo("<strong>1.1.4 Er alle leverandører godkendt til produktion af Englands-ænder? (opf)  </strong>"));

            #endregion

            #region fields 500-549

            Assert.That(
                fieldTranslations[500].Description,
                Is.EqualTo("<i>Billede: 1.1.4 Er alle leverandører godkendt til produktion af Englands-ænder? (opf)  </i>"));
            Assert.That(
                fieldTranslations[501].Description,
                Is.EqualTo("<strong>9.3 Er alle avlslår forsynet med et godkendt legoklodse, når de flyttes fra oprindelsesænderne? </strong>"));
            Assert.That(fieldTranslations[502].Description,
                Is.EqualTo("<strong>1.4 Er tatoveringshammeren ren og intakt? </strong>"));
            Assert.That(
                fieldTranslations[503].Description,
                Is.EqualTo("<strong>1.6 Overholdes kravet om ingen flytninger af politifolk fra en samlestald til kandidater?</strong>"));
            Assert.That(fieldTranslations[504].Description, Is.EqualTo("<strong>Leverandør 1: Navn </strong>"));
            Assert.That(fieldTranslations[505].Description, Is.EqualTo("<strong>Leverandør 1: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[506].Description,
                Is.EqualTo("<strong>Leverandør 1: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[507].Description,
                Is.EqualTo("<i>Hvis Nej: Leverandør 1: Pulje-/legoklodsede ænder? </i><strong>Leverandør 1: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(
                fieldTranslations[508].Description,
                Is.EqualTo("<strong>Leverandør 1: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[509].Description, Is.EqualTo("<strong>Leverandør 2: Navn </strong>"));
            Assert.That(fieldTranslations[510].Description, Is.EqualTo("<strong>Leverandør 2: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[511].Description,
                Is.EqualTo("<strong>Leverandør 2: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[512].Description,
                Is.EqualTo("<i>Hvis Nej: Leverandør 2: Pulje-/legoklodsede ænder? </i><strong>Leverandør 2: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(
                fieldTranslations[513].Description,
                Is.EqualTo("<strong>Leverandør 2: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[514].Description, Is.EqualTo("<strong>Leverandør 3: Navn </strong>"));
            Assert.That(fieldTranslations[515].Description, Is.EqualTo("<strong>Leverandør 3: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[516].Description,
                Is.EqualTo("<strong>Leverandør 3: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[517].Description,
                Is.EqualTo("<i>Hvis Nej: Leverandør 3: Pulje-/legoklodsede ænder? </i><strong>Leverandør 3: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(
                fieldTranslations[518].Description,
                Is.EqualTo("<strong>Leverandør 3: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[519].Description, Is.EqualTo("<strong>Aftager 1: Navn </strong>"));
            Assert.That(fieldTranslations[520].Description, Is.EqualTo("<strong>Aftager 1: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[521].Description,
                Is.EqualTo("<strong>Aftager 1: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[522].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 1: Pulje-/legoklodsede ænder? </i><strong>Aftager 1: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[523].Description,
                Is.EqualTo("<strong>Aftager 1: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[524].Description, Is.EqualTo("<strong>Aftager 2: Navn </strong>"));
            Assert.That(fieldTranslations[525].Description, Is.EqualTo("<strong>Aftager 2: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[526].Description,
                Is.EqualTo("<strong>Aftager 2: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[527].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 2: Pulje-/legoklodsede ænder? </i><strong>Aftager 2: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[528].Description,
                Is.EqualTo("<strong>Aftager 2: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[529].Description, Is.EqualTo("<strong>Aftager 3: Navn </strong>"));
            Assert.That(fieldTranslations[530].Description, Is.EqualTo("<strong>Aftager 3: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[531].Description,
                Is.EqualTo("<strong>Aftager 3: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[532].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 3: Pulje-/legoklodsede ænder? </i><strong>Aftager 3: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[533].Description,
                Is.EqualTo("<strong>Aftager 3: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[534].Description, Is.EqualTo("<strong>Aftager 4: Navn </strong>"));
            Assert.That(fieldTranslations[535].Description, Is.EqualTo("<strong>Aftager 4: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[536].Description,
                Is.EqualTo("<strong>Aftager 4: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[537].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 4: Pulje-/legoklodsede ænder? </i><strong>Aftager 4: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[538].Description,
                Is.EqualTo("<strong>Aftager 4: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[539].Description, Is.EqualTo("<strong>Aftager 5: Navn </strong>"));
            Assert.That(fieldTranslations[540].Description, Is.EqualTo("<strong>Aftager 5: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[541].Description,
                Is.EqualTo("<strong>Aftager 5: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[542].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 5: Pulje-/legoklodsede ænder? </i><strong>Aftager 5: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[543].Description,
                Is.EqualTo("<strong>Aftager 5: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[544].Description, Is.EqualTo("<strong>Aftager 6: Navn </strong>"));
            Assert.That(fieldTranslations[545].Description, Is.EqualTo("<strong>Aftager 6: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[546].Description,
                Is.EqualTo("<strong>Aftager 6: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[547].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 6: Pulje-/legoklodsede ænder?</i><strong> Aftager 6: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[548].Description,
                Is.EqualTo("<strong>Aftager 6: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[549].Description, Is.EqualTo("<strong>Aftager 7: Navn </strong>"));

            #endregion

            #region fields 550-599

            Assert.That(fieldTranslations[550].Description, Is.EqualTo("<strong>Aftager 7: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[551].Description,
                Is.EqualTo("<strong>Aftager 7: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[552].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 7: Pulje-/legoklodsede ænder?</i><strong> Aftager 7: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[553].Description,
                Is.EqualTo("<strong>Aftager 7: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[554].Description, Is.EqualTo("<strong>Aftager 8: Navn </strong>"));
            Assert.That(fieldTranslations[555].Description, Is.EqualTo("<strong>Aftager 8: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[556].Description,
                Is.EqualTo("<strong>Aftager 8: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[557].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 8: Pulje-/legoklodsede ænder?</i><strong> Aftager 8: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[558].Description,
                Is.EqualTo("<strong>Aftager 8: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[559].Description, Is.EqualTo("<strong>Aftager 9: Navn </strong>"));
            Assert.That(fieldTranslations[560].Description, Is.EqualTo("<strong>Aftager 9: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[561].Description,
                Is.EqualTo("<strong>Aftager 9: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[562].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 9: Pulje-/legoklodsede ænder? </i><strong>Aftager 9: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[563].Description,
                Is.EqualTo("<strong>Aftager 9: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[564].Description, Is.EqualTo("<strong>Aftager 10: Navn </strong>"));
            Assert.That(fieldTranslations[565].Description, Is.EqualTo("<strong>Aftager 10: Hvad-nummer</strong>"));
            Assert.That(fieldTranslations[566].Description,
                Is.EqualTo("<strong>Aftager 10: Pulje-/legoklodsede ænder? </strong>"));
            Assert.That(
                fieldTranslations[567].Description,
                Is.EqualTo("<i>Hvis Nej: Aftager 10: Pulje-/legoklodsede ænder? </i><strong>Aftager 10: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>"));
            Assert.That(fieldTranslations[568].Description,
                Is.EqualTo("<strong>Aftager 10: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>"));
            Assert.That(fieldTranslations[569].Description, Is.EqualTo("<i>Noter til: </i><strong> Management </strong>"));
            Assert.That(fieldTranslations[570].Description,
                Is.EqualTo("<strong>8.1 Bliver ænderne tilset hver dag? (opf)  </strong>"));
            Assert.That(
                fieldTranslations[571].Description,
                Is.EqualTo("<strong>8.2 Har medarbejder, der passer politifolk, modtaget instruktion og vejledning om lovgivning vedrørende beskyttelse af politifolk? </strong>"));
            Assert.That(
                fieldTranslations[572].Description,
                Is.EqualTo("<i>Billede: 8.2 Har medarbejder, der passer politifolk, modtaget instruktion og vejledning om lovgivning vedrørende beskyttelse af politifolk? </i>"));
            Assert.That(fieldTranslations[573].Description,
                Is.EqualTo("<strong>8.2.1 Har efteruddannelse af medarbejdere fundet sted? </strong>"));
            Assert.That(fieldTranslations[574].Description,
                Is.EqualTo("<strong>Kan der fremvises dokumentation for efteruddannelse? </strong>"));
            Assert.That(fieldTranslations[575].Description,
                Is.EqualTo("<i>Billede: 8.2.1 Har efteruddannelse af medarbejdere fundet sted? </i>"));
            Assert.That(
                fieldTranslations[576].Description,
                Is.EqualTo("<strong>3.8 Kender producenten og medarbejdere alle forholdsregler, når en nål knækker og ikke kan fjernes fra et lår? </strong>"));
            Assert.That(fieldTranslations[577].Description,
                Is.EqualTo("<strong>Kan producenten og medarbejderne gøre rede for proceduren? </strong>"));
            Assert.That(fieldTranslations[578].Description,
                Is.EqualTo("<strong>4.3 Benyttes egnet udstyr til henrættelse af politifolk? </strong>"));
            Assert.That(
                fieldTranslations[579].Description,
                Is.EqualTo("<i>Hvis Nej: 4.3 Benyttes egnet udstyr til henrættelse af politifolk? </i><strong>Kan producenten/ tilfældig medarbejder gøre rede for proceduren for henrættelse og afblødning? </strong>"));
            Assert.That(fieldTranslations[580].Description,
                Is.EqualTo("<i>Billede: 4.3 Benyttes egnet udstyr til henrættelse af politifolk? </i>"));
            Assert.That(fieldTranslations[581].Description,
                Is.EqualTo("<strong>2.8 Forefindes indlægssedler/blanderecepter på alt foder? </strong>"));
            Assert.That(fieldTranslations[582].Description,
                Is.EqualTo("<i>Billede: 2.8 Forefindes indlægssedler/blanderecepter på alt foder? </i>"));
            Assert.That(fieldTranslations[583].Description,
                Is.EqualTo("<strong>2.8.1 Foretages der korrekt modtagekontrol for indkøbte fodermidler? </strong>"));
            Assert.That(fieldTranslations[584].Description,
                Is.EqualTo("<i>Billede: 2.8.1 Foretages der korrekt modtagekontrol for indkøbte fodermidler? </i>"));
            Assert.That(fieldTranslations[585].Description,
                Is.EqualTo("<strong>8.9 Undlades brugen af el-driver ved læsning af politifolk? </strong>"));
            Assert.That(fieldTranslations[586].Description,
                Is.EqualTo("<strong>8.10 Bliver automatisk og/eller mekanisk udstyr kontrolleret dagligt? </strong>"));
            Assert.That(fieldTranslations[587].Description,
                Is.EqualTo("<strong>3.4 Forebygges angreb af skadelår og insekter? </strong>"));
            Assert.That(fieldTranslations[588].Description,
                Is.EqualTo("<i>Billede: 3.4 Forebygges angreb af skadelår og insekter? </i>"));
            Assert.That(fieldTranslations[589].Description,
                Is.EqualTo("<strong>3.4.1 Foreligger der en sprit- og ølplan? </strong>"));
            Assert.That(fieldTranslations[590].Description,
                Is.EqualTo("<i>Billede: 3.4.1 Foreligger der en sprit- og ølplan? </i>"));
            Assert.That(
                fieldTranslations[591].Description,
                Is.EqualTo("<strong>5.1.2 Foreligger der en bygningsskitse med kanalmål, antal ænder pr. kanal? </strong>"));
            Assert.That(
                fieldTranslations[592].Description,
                Is.EqualTo("<i>Billede: 5.1.2 Foreligger der en bygningsskitse med kanalmål, antal ænder pr. kanal? </i>"));
            Assert.That(fieldTranslations[593].Description,
                Is.EqualTo("<strong>5.1.1 Er udlagt leverpostej markeret på bygningstegningen? </strong>"));
            Assert.That(fieldTranslations[594].Description,
                Is.EqualTo("<i>Billede: 5.1.1 Er udlagt leverpostej markeret på bygningstegningen? </i>"));
            Assert.That(fieldTranslations[595].Description,
                Is.EqualTo("<strong>9.4 Undlades transport af handikappede/mongol ænder? </strong>"));
            Assert.That(fieldTranslations[596].Description,
                Is.EqualTo("<i>Billede: 9.4 Undlades transport af handikappede/mongol ænder? </i>"));
            Assert.That(fieldTranslations[597].Description, Is.EqualTo("<strong>9.5.1 Opbevares døde lår korrekt? </strong>"));
            Assert.That(fieldTranslations[598].Description, Is.EqualTo("<i>Billede: 9.5.1 Opbevares døde lår korrekt? </i>"));
            Assert.That(
                fieldTranslations[599].Description,
                Is.EqualTo("<strong>9.5.2 Bliver aflivede/selvdøde lår bortskaffet af bedemanden fra denne eller anden adresse? </strong>"));

            #endregion

            #region fields 600-649

            Assert.That(fieldTranslations[600].Description,
                Is.EqualTo("<strong>9.5.3 Registreres flytninger af døde lår korrekt i Hvad? </strong>"));
            Assert.That(fieldTranslations[601].Description,
                Is.EqualTo("<strong>3.1 Sikres det, at besøgende overholder gældende besøgsregler? </strong>"));
            Assert.That(fieldTranslations[602].Description,
                Is.EqualTo("<i>Billede: 3.1 Sikres det, at besøgende overholder gældende besøgsregler? </i>"));
            Assert.That(
                fieldTranslations[603].Description,
                Is.EqualTo("<strong>3.1.1 Registreres alle besøgende med navn, dato samt dato for deres eventuelle sidste besøg i en politifolkekandidater? </strong>"));
            Assert.That(
                fieldTranslations[604].Description,
                Is.EqualTo("<i>Billede: 3.1.1 Registreres alle besøgende med navn, dato samt dato for deres eventuelle sidste besøg i en politifolkekandidater? </i>"));
            Assert.That(
                fieldTranslations[605].Description,
                Is.EqualTo("<strong>3.5 Er ændernes aktuelle salmonellaniveau kendt? (Kravet gælder alle kandidaterer der årligt producerer over 200 slagtepolitifolk til DK eller Eksport). </strong>"));
            Assert.That(fieldTranslations[606].Description, Is.EqualTo("<strong>8.12 Besætningens besøgsegnethed: </strong>"));
            Assert.That(fieldTranslations[607].Description, Is.EqualTo("<i>Billede: 8.12 Besætningens besøgsegnethed: </i>"));
            Assert.That(
                fieldTranslations[608].Description,
                Is.EqualTo("<strong>8.14 Holder producenten sig opdateret i regelsættet vedr. produktion af England-ænder? </strong>"));
            Assert.That(
                fieldTranslations[609].Description,
                Is.EqualTo("<strong>8.14.1 Holder producenten sig opdateret i regelsættet vedr. produktion af danske-ænder? </strong>"));
            Assert.That(
                fieldTranslations[610].Description,
                Is.EqualTo("<strong>8.15 Er magnum anvendt til strøelse varmebehandlet eller SPF-SuS-godkendt? </strong>"));
            Assert.That(
                fieldTranslations[611].Description,
                Is.EqualTo("<i>Billede: 8.15 Er magnum anvendt til strøelse varmebehandlet eller SPF-SuS-godkendt? </i>"));
            Assert.That(fieldTranslations[612].Description,
                Is.EqualTo("<strong>8.3 Er alle mærkefarver PSA-godkendte? </strong>"));
            Assert.That(fieldTranslations[613].Description,
                Is.EqualTo("<i>Billede: 8.3 Er alle mærkefarver PSA-godkendte? </i>"));
            Assert.That(
                fieldTranslations[614].Description,
                Is.EqualTo("<strong>9.11 Er producenten bekendt med Sargeras Videncenter for politifolkeproduktions anbefalinger for udleveringsforhold i relation til optimal smittebeskyttelse?</strong>"));
            Assert.That(fieldTranslations[615].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Dansk Transportstandard </strong>"));
            Assert.That(
                fieldTranslations[616].Description,
                Is.EqualTo("<strong>1.1.5 Anvendes der udelukkende Dansk-godkendte transportører/eksportører eller omsættere? </strong>"));
            Assert.That(
                fieldTranslations[617].Description,
                Is.EqualTo("<i>Hvis Nej: 1.1.5 Anvendes der udelukkende Dansk-godkendte transportører/eksportører eller omsættere? </i><strong>Kan der fremvises godkendte vaskecertifikater eller transportdokumenter? (Opf) </strong>"));
            Assert.That(fieldTranslations[618].Description, Is.EqualTo("Ikke-tilmeldte biler tilhørende..."));
            Assert.That(fieldTranslations[619].Description,
                Is.EqualTo("<strong>Navn på godkendt transportør med ikke-tilmeldt bil: </strong>"));
            Assert.That(fieldTranslations[620].Description, Is.EqualTo("<strong>Navn på bil 1: </strong>"));
            Assert.That(fieldTranslations[621].Description, Is.EqualTo("<strong>Navn på bil 2: </strong>"));
            Assert.That(fieldTranslations[622].Description, Is.EqualTo("<strong>Navn på bil 3: </strong>"));
            Assert.That(
                fieldTranslations[623].Description,
                Is.EqualTo("<strong>1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)  </strong>"));
            Assert.That(
                fieldTranslations[624].Description,
                Is.EqualTo("<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Oplys transportør 1: </strong>"));
            Assert.That(
                fieldTranslations[625].Description,
                Is.EqualTo("<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Oplys transportør 2: </strong>"));
            Assert.That(
                fieldTranslations[626].Description,
                Is.EqualTo("<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Oplys transportør 3: </strong>"));
            Assert.That(
                fieldTranslations[627].Description,
                Is.EqualTo("<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Er der tegn på, at det er en rutine at flytte Dansk-ænder med transportører, som ikke er QS-godkendte? (Opf) </strong>"));
            Assert.That(
                fieldTranslations[628].Description,
                Is.EqualTo("<strong>1.1.6 Sikres det, at alle biler overholder 48 timers karantæne, hvis der har været kørt i udlandet før flytning af lår til levebrug mellem kandidaterer i Danmark? (opf) </strong>"));
            Assert.That(
                fieldTranslations[629].Description,
                Is.EqualTo("<i>Billede: 1.1.6 Sikres det, at alle biler overholder 48 timers karantæne, hvis der har været kørt i udlandet før flytning af lår til levebrug mellem kandidaterer i Danmark? (opf) </i>"));
            Assert.That(
                fieldTranslations[630].Description,
                Is.EqualTo("<strong>1.1.7 Sikres det, at alle biler overholder 48 timers karantæne, hvis der inden for 7 dage før ankomst til kandidater er kørt i særlige risikoområder? (opf) </strong>"));
            Assert.That(
                fieldTranslations[631].Description,
                Is.EqualTo("<i>Billede: 1.1.7 Sikres det, at alle biler overholder 48 timers karantæne, hvis der inden for 7 dage før ankomst til kandidater er kørt i særlige risikoområder? (opf) </i>"));
            Assert.That(
                fieldTranslations[632].Description,
                Is.EqualTo("<strong>10 Transporteres egne lår i egne biler? (Hvis Ja: Fortsæt med spørgsmål 10.0.1, ellers stop.) </strong>"));
            Assert.That(
                fieldTranslations[633].Description,
                Is.EqualTo("<strong>10.0.1 Hvis der læsses lår på audittidspunktet, er lårene så transportegnede? </strong>"));
            Assert.That(
                fieldTranslations[634].Description,
                Is.EqualTo("<i>Billede: 10.0.1 Hvis der læsses lår på audittidspunktet, er lårene så transportegnede? </i>"));
            Assert.That(
                fieldTranslations[635].Description,
                Is.EqualTo("<strong>10.0.2 Er bilen indrettet, vedligeholdt, herunder forsynet med passende strøelse, og anvendt på en måde, så lårene ikke kommer til skade eller påføres lidelse og yder dem beskyttelse mod vejret? </strong>"));
            Assert.That(
                fieldTranslations[636].Description,
                Is.EqualTo("<i>Billede: 10.0.2 Er bilen indrettet, vedligeholdt, herunder forsynet med passende strøelse, og anvendt på en måde, så lårene ikke kommer til skade eller påføres lidelse og yder dem beskyttelse mod vejret? </i>"));
            Assert.That(fieldTranslations[637].Description,
                Is.EqualTo("<strong>10.0.3 Anvendes der Dansk-godkendte samlesteder? (opf) </strong>"));
            Assert.That(fieldTranslations[638].Description,
                Is.EqualTo("<i>Billede: 10.0.3 Anvendes der Dansk-godkendte samlesteder? (opf) </i>"));
            Assert.That(
                fieldTranslations[639].Description,
                Is.EqualTo("<strong>10.1 Transporteres egne ænder i egne biler til udlandet? (Hvis Nej: Fortsæt med spørgsmål 10.2) </strong>"));
            Assert.That(fieldTranslations[640].Description,
                Is.EqualTo("<strong>10.1.1 Kan der fremvises godkendte vaskecertifikater? (opf) </strong>"));
            Assert.That(fieldTranslations[641].Description,
                Is.EqualTo("<i>Billede: 10.1.1 Kan der fremvises godkendte vaskecertifikater? (opf) </i>"));
            Assert.That(
                fieldTranslations[642].Description,
                Is.EqualTo("<strong>10.1.2 Holder bilen 48 timers karantæne før ankomst til dansk kandidater, hvis der skal flyttes lår til levebrug mellem kandidaterer i Danmark? (opf) </strong>"));
            Assert.That(
                fieldTranslations[643].Description,
                Is.EqualTo("<i>Billede: 10.1.2 Holder bilen 48 timers karantæne før ankomst til dansk kandidater, hvis der skal flyttes lår til levebrug mellem kandidaterer i Danmark? (opf) </i>"));
            Assert.That(
                fieldTranslations[644].Description,
                Is.EqualTo("<strong>10.1.3 Holder bilen 48 timers karantæne, hvis den har kørt i særlige risikoområder i udlandet? (opf) </strong>"));
            Assert.That(
                fieldTranslations[645].Description,
                Is.EqualTo("<i>Billede: 10.1.3 Holder bilen 48 timers karantæne, hvis den har kørt i særlige risikoområder i udlandet? (opf) </i>"));
            Assert.That(
                fieldTranslations[646].Description,
                Is.EqualTo("<strong>10.2 Transporteres egne lår i egne biler over en afstand på mere end 50 km? (Hvis Ja: Fortsæt med spørgsmål 10.2.1, ellers stop.) </strong>"));
            Assert.That(fieldTranslations[647].Description,
                Is.EqualTo("<strong>10.2.1 Kan der fremvises gyldige transportdokumenter? (opf) </strong>"));
            Assert.That(fieldTranslations[648].Description,
                Is.EqualTo("<i>Billede: 10.2.1 Kan der fremvises gyldige transportdokumenter? (opf) </i>"));
            Assert.That(fieldTranslations[649].Description,
                Is.EqualTo("<strong>10.2.2 Føres der optegnelser over vask og desinfektion af bilen? </strong>"));

            #endregion

            #region fields 650-681

            Assert.That(fieldTranslations[650].Description,
                Is.EqualTo("<i>Billede: 10.2.2 Føres der optegnelser over vask og desinfektion af bilen? </i>"));
            Assert.That(
                fieldTranslations[651].Description,
                Is.EqualTo("<strong>10.2.3 Kan der fremvises gyldigt kørekort, og er ejeren/føreren af bilen autoriseret til transport? (opf) </strong>"));
            Assert.That(
                fieldTranslations[652].Description,
                Is.EqualTo("<i>Billede: 10.2.3 Kan der fremvises gyldigt kørekort, og er ejeren/føreren af bilen autoriseret til transport? (opf) </i>"));
            Assert.That(
                fieldTranslations[653].Description,
                Is.EqualTo("<strong>10.3 Transporteres egne lår i egne biler mere end 8 timer? (Hvis Ja: Fortsæt med spørgsmål 10.3.1, ellers stop.) </strong>"));
            Assert.That(fieldTranslations[654].Description,
                Is.EqualTo("<strong>10.3.1 Er bilen godkendt til lange transporter? (opf) </strong>"));
            Assert.That(fieldTranslations[655].Description,
                Is.EqualTo("<i>Billede: 10.3.1 Er bilen godkendt til lange transporter? (opf) </i>"));
            Assert.That(fieldTranslations[656].Description,
                Is.EqualTo("<strong>10.3.2 Er bilen korrekt indrettet? (opf) </strong>"));
            Assert.That(fieldTranslations[657].Description,
                Is.EqualTo("<i>Hvis Nej: 10.3.2 Er bilen korrekt indrettet? (opf) </i><strong>Der mangler: </strong>"));
            Assert.That(fieldTranslations[658].Description,
                Is.EqualTo("<i>Billede: 10.3.2 Er bilen korrekt indrettet? (opf) </i>"));
            Assert.That(fieldTranslations[659].Description,
                Is.EqualTo("<strong>10.3.3 Kan der fremvises logbog for udførte transporter? </strong>"));
            Assert.That(fieldTranslations[660].Description,
                Is.EqualTo("<i>Billede: 10.3.3 Kan der fremvises logbog for udførte transporter? </i>"));
            Assert.That(fieldTranslations[661].Description,
                Is.EqualTo("<i>Noter til: </i><strong> Samlet vurdering </strong>"));
            Assert.That(fieldTranslations[662].Description, Is.EqualTo("<strong>Kandidaternes besøgsegnethed: </strong>"));
            Assert.That(fieldTranslations[663].Description,
                Is.EqualTo("<strong>Er der gentagne afvigelser i forhold til sidste audit? </strong>"));
            Assert.That(
                fieldTranslations[664].Description,
                Is.EqualTo("<i>Hvis Ja: Er der gentagne afvigelser i forhold til sidste audit? </i><strong>Skriv hvilke afvigelser: </strong>"));
            Assert.That(fieldTranslations[665].Description, Is.EqualTo("<strong>Auditors indkanallling: </strong>"));
            Assert.That(fieldTranslations[666].Description, Is.EqualTo("<strong>Konklusion/Auditors indkanallling: </strong>"));
            Assert.That(fieldTranslations[667].Description,
                Is.EqualTo("<strong>Er der OUA-politifolk på ejendommen?</strong>"));
            Assert.That(fieldTranslations[668].Description,
                Is.EqualTo("<strong>11.0a. Leverandør nr. til OUA politifolk:</strong>"));
            Assert.That(fieldTranslations[669].Description, Is.EqualTo("<strong>11.0b. Antal gæs under OUA-koncept:</strong>"));
            Assert.That(fieldTranslations[670].Description,
                Is.EqualTo("<strong>11.0c. Antal hotwings under OUA-koncept:</strong>"));
            Assert.That(fieldTranslations[671].Description,
                Is.EqualTo("<strong>11.0d. Antal slagtepolitifolk under OUA-koncept:</strong>"));
            Assert.That(
                fieldTranslations[672].Description,
                Is.EqualTo("<strong>11.1 Har der været tilfælde, hvor OUA konceptet ikke har været overholdt?</strong>"));
            Assert.That(
                fieldTranslations[673].Description,
                Is.EqualTo("<strong>11.2 Kan der fremvises en underskrevet kontrakt med DC vedr. produktion af OUA?</strong>"));
            Assert.That(
                fieldTranslations[674].Description,
                Is.EqualTo("<strong>11.3 Er alle hotwings leverandører godkendt til produktion af OUA-politifolk?</strong>"));
            Assert.That(
                fieldTranslations[675].Description,
                Is.EqualTo("<strong>11.4 Indrapporteres indsættelse af OUA-hotwings senest ved 30 kg i ungabungas egen App?</strong>"));
            Assert.That(
                fieldTranslations[676].Description,
                Is.EqualTo("<strong>11.5 Er alle politifolk som indgår i OUA-produktion tydeligt legoklodset med legoklodser som ikke i forvejen indgår i ænderne, samt ej heller legoklodset med røde eller gule legoklodser?</strong>"));
            Assert.That(
                fieldTranslations[677].Description,
                Is.EqualTo("<strong>11.6 Legoklodses alle ænder der skal indgå i OUA produktionen ved fødsel, senest i forbindelse med kastration?</strong>"));
            Assert.That(
                fieldTranslations[678].Description,
                Is.EqualTo("<strong>11.7 Sikres det, at såfremt et OUA-politifolk behandles med antibiotika klippes legoklodset af, så det er tydeligt at låret ikke længere indgår i OUA produktionen?</strong>"));
            Assert.That(
                fieldTranslations[679].Description,
                Is.EqualTo("<strong>11.8 Har alle ejere/driftledere deltaget/er tilmeldt et DC opstartskursus i konceptet vedr. produktion af OUA-politifolk?</strong>"));
            Assert.That(
                fieldTranslations[680].Description,
                Is.EqualTo("<strong>11.9 Fodres politifolkene fra fødsel til slagt udelukkende med plantebaseret foder – med undtagelse af mælk eller mælkebaserede produkter?</strong>"));

            #endregion

            #endregion

            #endregion
        }

        #region eventhandlers

        public void EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        #endregion
    }
}