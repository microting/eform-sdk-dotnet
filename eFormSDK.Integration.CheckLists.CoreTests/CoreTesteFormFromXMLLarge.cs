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
            Assert.NotNull(mainelement);
            Assert.NotNull(match);
            Assert.AreEqual(listOfCL.Count(), 15);
            Assert.AreEqual(listOfFields.Count, 681);

            #region Checklist

            #region Label

            Assert.AreEqual(checkLisTranslations[0].Text, "UK");
            Assert.AreEqual(checkLisTranslations[1].Text, "Stamdata og gummioplysninger. Husk gummiænder!");
            Assert.AreEqual(checkLisTranslations[2].Text, "Gennemgang af damme: Gæs på ejendommen ");
            Assert.AreEqual(checkLisTranslations[3].Text, "Gennemgang af damanlæg: hotwings på ejendommen ");
            Assert.AreEqual(checkLisTranslations[4].Text,
                "Gennemgang af damanlæg: Slagtepolitifolk/polte på ejendommen ");
            Assert.AreEqual(checkLisTranslations[5].Text, "Gennemgang af foderrum ");
            Assert.AreEqual(checkLisTranslations[6].Text,
                "Sundhed og pilleranvendelse: Ved håndtering af piller på Hvad-nummeret ");
            Assert.AreEqual(checkLisTranslations[7].Text, "Ved kandidater med kostråd ");
            Assert.AreEqual(checkLisTranslations[8].Text, "Ved kandidater uden kostråd ");
            Assert.AreEqual(checkLisTranslations[9].Text, "Alarmanlæg ");
            Assert.AreEqual(checkLisTranslations[10].Text, "Leverandører/aftagere af ænder ");
            Assert.AreEqual(checkLisTranslations[11].Text, "Management");
            Assert.AreEqual(checkLisTranslations[12].Text, "Dansk Transportstandard");
            Assert.AreEqual(checkLisTranslations[13].Text, "Samlet vurdering");
            Assert.AreEqual(checkLisTranslations[14].Text, "OUA-politifolk");

            #endregion

            #region Description

            Assert.AreEqual(listOfCL[0].Description, null);
            Assert.AreEqual(listOfCL[1].Description, null);
            Assert.AreEqual(listOfCL[2].Description, null);
            Assert.AreEqual(listOfCL[3].Description, null);
            Assert.AreEqual(listOfCL[4].Description, null);
            Assert.AreEqual(listOfCL[5].Description, null);
            Assert.AreEqual(listOfCL[6].Description, null);
            Assert.AreEqual(listOfCL[7].Description, null);
            Assert.AreEqual(listOfCL[8].Description, null);
            Assert.AreEqual(listOfCL[9].Description, null);
            Assert.AreEqual(listOfCL[10].Description, null);
            Assert.AreEqual(listOfCL[11].Description, null);
            Assert.AreEqual(listOfCL[12].Description, null);
            Assert.AreEqual(listOfCL[13].Description, null);
            Assert.AreEqual(listOfCL[14].Description, null);

            #endregion

            #endregion

            #region field

            #region Field.label

            #region fields 0-49

            Assert.AreEqual(null, listOfFields[0].Label);
            Assert.AreEqual(null, listOfFields[1].Label);
            Assert.AreEqual(null, listOfFields[2].Label);
            Assert.AreEqual(null, listOfFields[3].Label);
            Assert.AreEqual(null, listOfFields[4].Label);
            Assert.AreEqual(null, listOfFields[5].Label);
            Assert.AreEqual(null, listOfFields[6].Label);
            Assert.AreEqual(null, listOfFields[7].Label);
            Assert.AreEqual(null, listOfFields[8].Label);
            Assert.AreEqual(null, listOfFields[9].Label);
            Assert.AreEqual(null, listOfFields[10].Label);
            Assert.AreEqual(null, listOfFields[11].Label);
            Assert.AreEqual(null, listOfFields[12].Label);
            Assert.AreEqual(null, listOfFields[13].Label);
            Assert.AreEqual(null, listOfFields[14].Label);
            Assert.AreEqual(null, listOfFields[15].Label);
            Assert.AreEqual(null, listOfFields[16].Label);
            Assert.AreEqual(null, listOfFields[17].Label);
            Assert.AreEqual(null, listOfFields[18].Label);
            Assert.AreEqual(null, listOfFields[19].Label);
            Assert.AreEqual(null, listOfFields[20].Label);
            Assert.AreEqual(null, listOfFields[21].Label);
            Assert.AreEqual(null, listOfFields[22].Label);
            Assert.AreEqual(null, listOfFields[23].Label);
            Assert.AreEqual(null, listOfFields[24].Label);
            Assert.AreEqual(null, listOfFields[25].Label);
            Assert.AreEqual(null, listOfFields[26].Label);
            Assert.AreEqual(null, listOfFields[27].Label);
            Assert.AreEqual(null, listOfFields[28].Label);
            Assert.AreEqual(null, listOfFields[29].Label);
            Assert.AreEqual(null, listOfFields[30].Label);
            Assert.AreEqual(null, listOfFields[31].Label);
            Assert.AreEqual(null, listOfFields[32].Label);
            Assert.AreEqual(null, listOfFields[33].Label);
            Assert.AreEqual(null, listOfFields[34].Label);
            Assert.AreEqual(null, listOfFields[35].Label);
            Assert.AreEqual(null, listOfFields[36].Label);
            Assert.AreEqual(null, listOfFields[37].Label);
            Assert.AreEqual(null, listOfFields[38].Label);
            Assert.AreEqual(null, listOfFields[39].Label);
            Assert.AreEqual(null, listOfFields[40].Label);
            Assert.AreEqual(null, listOfFields[41].Label);
            Assert.AreEqual(null, listOfFields[42].Label);
            Assert.AreEqual(null, listOfFields[43].Label);
            Assert.AreEqual(null, listOfFields[44].Label);
            Assert.AreEqual(null, listOfFields[45].Label);
            Assert.AreEqual(null, listOfFields[46].Label);
            Assert.AreEqual(null, listOfFields[47].Label);
            Assert.AreEqual(null, listOfFields[48].Label);
            Assert.AreEqual(null, listOfFields[49].Label);

            #endregion

            #region fields 50-99

            Assert.AreEqual(null, listOfFields[50].Label);
            Assert.AreEqual(null, listOfFields[51].Label);
            Assert.AreEqual(null, listOfFields[52].Label);
            Assert.AreEqual(null, listOfFields[53].Label);
            Assert.AreEqual(null, listOfFields[54].Label);
            Assert.AreEqual(null, listOfFields[55].Label);
            Assert.AreEqual(null, listOfFields[56].Label);
            Assert.AreEqual(null, listOfFields[57].Label);
            Assert.AreEqual(null, listOfFields[58].Label);
            Assert.AreEqual(null, listOfFields[59].Label);
            Assert.AreEqual(null, listOfFields[60].Label);
            Assert.AreEqual(null, listOfFields[61].Label);
            Assert.AreEqual(null, listOfFields[62].Label);
            Assert.AreEqual(null, listOfFields[63].Label);
            Assert.AreEqual(null, listOfFields[64].Label);
            Assert.AreEqual(null, listOfFields[65].Label);
            Assert.AreEqual(null, listOfFields[66].Label);
            Assert.AreEqual(null, listOfFields[67].Label);
            Assert.AreEqual(null, listOfFields[68].Label);
            Assert.AreEqual(null, listOfFields[69].Label);
            Assert.AreEqual(null, listOfFields[70].Label);
            Assert.AreEqual(null, listOfFields[71].Label);
            Assert.AreEqual(null, listOfFields[72].Label);
            Assert.AreEqual(null, listOfFields[73].Label);
            Assert.AreEqual(null, listOfFields[74].Label);
            Assert.AreEqual(null, listOfFields[75].Label);
            Assert.AreEqual(null, listOfFields[76].Label);
            Assert.AreEqual(null, listOfFields[77].Label);
            Assert.AreEqual(null, listOfFields[78].Label);
            Assert.AreEqual(null, listOfFields[79].Label);
            Assert.AreEqual(null, listOfFields[80].Label);
            Assert.AreEqual(null, listOfFields[81].Label);
            Assert.AreEqual(null, listOfFields[82].Label);
            Assert.AreEqual(null, listOfFields[83].Label);
            Assert.AreEqual(null, listOfFields[84].Label);
            Assert.AreEqual(null, listOfFields[85].Label);
            Assert.AreEqual(null, listOfFields[86].Label);
            Assert.AreEqual(null, listOfFields[87].Label);
            Assert.AreEqual(null, listOfFields[88].Label);
            Assert.AreEqual(null, listOfFields[89].Label);
            Assert.AreEqual(null, listOfFields[90].Label);
            Assert.AreEqual(null, listOfFields[91].Label);
            Assert.AreEqual(null, listOfFields[92].Label);
            Assert.AreEqual(null, listOfFields[93].Label);
            Assert.AreEqual(null, listOfFields[94].Label);
            Assert.AreEqual(null, listOfFields[95].Label);
            Assert.AreEqual(null, listOfFields[96].Label);
            Assert.AreEqual(null, listOfFields[97].Label);
            Assert.AreEqual(null, listOfFields[98].Label);
            Assert.AreEqual(null, listOfFields[99].Label);

            #endregion

            #region fields 100-149

            Assert.AreEqual(null, listOfFields[100].Label);
            Assert.AreEqual(null, listOfFields[101].Label);
            Assert.AreEqual(null, listOfFields[102].Label);
            Assert.AreEqual(null, listOfFields[103].Label);
            Assert.AreEqual(null, listOfFields[104].Label);
            Assert.AreEqual(null, listOfFields[105].Label);
            Assert.AreEqual(null, listOfFields[106].Label);
            Assert.AreEqual(null, listOfFields[107].Label);
            Assert.AreEqual(null, listOfFields[108].Label);
            Assert.AreEqual(null, listOfFields[109].Label);
            Assert.AreEqual(null, listOfFields[110].Label);
            Assert.AreEqual(null, listOfFields[111].Label);
            Assert.AreEqual(null, listOfFields[112].Label);
            Assert.AreEqual(null, listOfFields[113].Label);
            Assert.AreEqual(null, listOfFields[114].Label);
            Assert.AreEqual(null, listOfFields[115].Label);
            Assert.AreEqual(null, listOfFields[116].Label);
            Assert.AreEqual(null, listOfFields[117].Label);
            Assert.AreEqual(null, listOfFields[118].Label);
            Assert.AreEqual(null, listOfFields[119].Label);
            Assert.AreEqual(null, listOfFields[120].Label);
            Assert.AreEqual(null, listOfFields[121].Label);
            Assert.AreEqual(null, listOfFields[122].Label);
            Assert.AreEqual(null, listOfFields[123].Label);
            Assert.AreEqual(null, listOfFields[124].Label);
            Assert.AreEqual(null, listOfFields[125].Label);
            Assert.AreEqual(null, listOfFields[126].Label);
            Assert.AreEqual(null, listOfFields[127].Label);
            Assert.AreEqual(null, listOfFields[128].Label);
            Assert.AreEqual(null, listOfFields[129].Label);
            Assert.AreEqual(null, listOfFields[130].Label);
            Assert.AreEqual(null, listOfFields[131].Label);
            Assert.AreEqual(null, listOfFields[132].Label);
            Assert.AreEqual(null, listOfFields[133].Label);
            Assert.AreEqual(null, listOfFields[134].Label);
            Assert.AreEqual(null, listOfFields[135].Label);
            Assert.AreEqual(null, listOfFields[136].Label);
            Assert.AreEqual(null, listOfFields[137].Label);
            Assert.AreEqual(null, listOfFields[138].Label);
            Assert.AreEqual(null, listOfFields[139].Label);
            Assert.AreEqual(null, listOfFields[140].Label);
            Assert.AreEqual(null, listOfFields[141].Label);
            Assert.AreEqual(null, listOfFields[142].Label);
            Assert.AreEqual(null, listOfFields[143].Label);
            Assert.AreEqual(null, listOfFields[144].Label);
            Assert.AreEqual(null, listOfFields[145].Label);
            Assert.AreEqual(null, listOfFields[146].Label);
            Assert.AreEqual(null, listOfFields[147].Label);
            Assert.AreEqual(null, listOfFields[148].Label);
            Assert.AreEqual(null, listOfFields[149].Label);

            #endregion

            #region fields 150-199

            Assert.AreEqual(null, listOfFields[150].Label);
            Assert.AreEqual(null, listOfFields[151].Label);
            Assert.AreEqual(null, listOfFields[152].Label);
            Assert.AreEqual(null, listOfFields[153].Label);
            Assert.AreEqual(null, listOfFields[154].Label);
            Assert.AreEqual(null, listOfFields[155].Label);
            Assert.AreEqual(null, listOfFields[156].Label);
            Assert.AreEqual(null, listOfFields[157].Label);
            Assert.AreEqual(null, listOfFields[158].Label);
            Assert.AreEqual(null, listOfFields[159].Label);
            Assert.AreEqual(null, listOfFields[160].Label);
            Assert.AreEqual(null, listOfFields[161].Label);
            Assert.AreEqual(null, listOfFields[162].Label);
            Assert.AreEqual(null, listOfFields[163].Label);
            Assert.AreEqual(null, listOfFields[164].Label);
            Assert.AreEqual(null, listOfFields[165].Label);
            Assert.AreEqual(null, listOfFields[166].Label);
            Assert.AreEqual(null, listOfFields[167].Label);
            Assert.AreEqual(null, listOfFields[168].Label);
            Assert.AreEqual(null, listOfFields[169].Label);
            Assert.AreEqual(null, listOfFields[170].Label);
            Assert.AreEqual(null, listOfFields[171].Label);
            Assert.AreEqual(null, listOfFields[172].Label);
            Assert.AreEqual(null, listOfFields[173].Label);
            Assert.AreEqual(null, listOfFields[174].Label);
            Assert.AreEqual(null, listOfFields[175].Label);
            Assert.AreEqual(null, listOfFields[176].Label);
            Assert.AreEqual(null, listOfFields[177].Label);
            Assert.AreEqual(null, listOfFields[178].Label);
            Assert.AreEqual(null, listOfFields[179].Label);
            Assert.AreEqual(null, listOfFields[180].Label);
            Assert.AreEqual(null, listOfFields[181].Label);
            Assert.AreEqual(null, listOfFields[182].Label);
            Assert.AreEqual(null, listOfFields[183].Label);
            Assert.AreEqual(null, listOfFields[184].Label);
            Assert.AreEqual(null, listOfFields[185].Label);
            Assert.AreEqual(null, listOfFields[186].Label);
            Assert.AreEqual(null, listOfFields[187].Label);
            Assert.AreEqual(null, listOfFields[188].Label);
            Assert.AreEqual(null, listOfFields[189].Label);
            Assert.AreEqual(null, listOfFields[190].Label);
            Assert.AreEqual(null, listOfFields[191].Label);
            Assert.AreEqual(null, listOfFields[192].Label);
            Assert.AreEqual(null, listOfFields[193].Label);
            Assert.AreEqual(null, listOfFields[194].Label);
            Assert.AreEqual(null, listOfFields[195].Label);
            Assert.AreEqual(null, listOfFields[196].Label);
            Assert.AreEqual(null, listOfFields[197].Label);
            Assert.AreEqual(null, listOfFields[198].Label);
            Assert.AreEqual(null, listOfFields[199].Label);

            #endregion

            #region fields 200-249

            Assert.AreEqual(null, listOfFields[200].Label);
            Assert.AreEqual(null, listOfFields[201].Label);
            Assert.AreEqual(null, listOfFields[202].Label);
            Assert.AreEqual(null, listOfFields[203].Label);
            Assert.AreEqual(null, listOfFields[204].Label);
            Assert.AreEqual(null, listOfFields[205].Label);
            Assert.AreEqual(null, listOfFields[206].Label);
            Assert.AreEqual(null, listOfFields[207].Label);
            Assert.AreEqual(null, listOfFields[208].Label);
            Assert.AreEqual(null, listOfFields[209].Label);
            Assert.AreEqual(null, listOfFields[210].Label);
            Assert.AreEqual(null, listOfFields[211].Label);
            Assert.AreEqual(null, listOfFields[212].Label);
            Assert.AreEqual(null, listOfFields[213].Label);
            Assert.AreEqual(null, listOfFields[214].Label);
            Assert.AreEqual(null, listOfFields[215].Label);
            Assert.AreEqual(null, listOfFields[216].Label);
            Assert.AreEqual(null, listOfFields[217].Label);
            Assert.AreEqual(null, listOfFields[218].Label);
            Assert.AreEqual(null, listOfFields[219].Label);
            Assert.AreEqual(null, listOfFields[220].Label);
            Assert.AreEqual(null, listOfFields[221].Label);
            Assert.AreEqual(null, listOfFields[222].Label);
            Assert.AreEqual(null, listOfFields[223].Label);
            Assert.AreEqual(null, listOfFields[224].Label);
            Assert.AreEqual(null, listOfFields[225].Label);
            Assert.AreEqual(null, listOfFields[226].Label);
            Assert.AreEqual(null, listOfFields[227].Label);
            Assert.AreEqual(null, listOfFields[228].Label);
            Assert.AreEqual(null, listOfFields[229].Label);
            Assert.AreEqual(null, listOfFields[230].Label);
            Assert.AreEqual(null, listOfFields[231].Label);
            Assert.AreEqual(null, listOfFields[232].Label);
            Assert.AreEqual(null, listOfFields[233].Label);
            Assert.AreEqual(null, listOfFields[234].Label);
            Assert.AreEqual(null, listOfFields[235].Label);
            Assert.AreEqual(null, listOfFields[236].Label);
            Assert.AreEqual(null, listOfFields[237].Label);
            Assert.AreEqual(null, listOfFields[238].Label);
            Assert.AreEqual(null, listOfFields[239].Label);
            Assert.AreEqual(null, listOfFields[240].Label);
            Assert.AreEqual(null, listOfFields[241].Label);
            Assert.AreEqual(null, listOfFields[242].Label);
            Assert.AreEqual(null, listOfFields[243].Label);
            Assert.AreEqual(null, listOfFields[244].Label);
            Assert.AreEqual(null, listOfFields[245].Label);
            Assert.AreEqual(null, listOfFields[246].Label);
            Assert.AreEqual(null, listOfFields[247].Label);
            Assert.AreEqual(null, listOfFields[248].Label);
            Assert.AreEqual(null, listOfFields[249].Label);

            #endregion

            #region fields 250-299

            Assert.AreEqual(null, listOfFields[250].Label);
            Assert.AreEqual(null, listOfFields[251].Label);
            Assert.AreEqual(null, listOfFields[252].Label);
            Assert.AreEqual(null, listOfFields[253].Label);
            Assert.AreEqual(null, listOfFields[254].Label);
            Assert.AreEqual(null, listOfFields[255].Label);
            Assert.AreEqual(null, listOfFields[256].Label);
            Assert.AreEqual(null, listOfFields[257].Label);
            Assert.AreEqual(null, listOfFields[258].Label);
            Assert.AreEqual(null, listOfFields[259].Label);
            Assert.AreEqual(null, listOfFields[260].Label);
            Assert.AreEqual(null, listOfFields[261].Label);
            Assert.AreEqual(null, listOfFields[262].Label);
            Assert.AreEqual(null, listOfFields[263].Label);
            Assert.AreEqual(null, listOfFields[264].Label);
            Assert.AreEqual(null, listOfFields[265].Label);
            Assert.AreEqual(null, listOfFields[266].Label);
            Assert.AreEqual(null, listOfFields[267].Label);
            Assert.AreEqual(null, listOfFields[268].Label);
            Assert.AreEqual(null, listOfFields[269].Label);
            Assert.AreEqual(null, listOfFields[270].Label);
            Assert.AreEqual(null, listOfFields[271].Label);
            Assert.AreEqual(null, listOfFields[272].Label);
            Assert.AreEqual(null, listOfFields[273].Label);
            Assert.AreEqual(null, listOfFields[274].Label);
            Assert.AreEqual(null, listOfFields[275].Label);
            Assert.AreEqual(null, listOfFields[276].Label);
            Assert.AreEqual(null, listOfFields[277].Label);
            Assert.AreEqual(null, listOfFields[278].Label);
            Assert.AreEqual(null, listOfFields[279].Label);
            Assert.AreEqual(null, listOfFields[280].Label);
            Assert.AreEqual(null, listOfFields[281].Label);
            Assert.AreEqual(null, listOfFields[282].Label);
            Assert.AreEqual(null, listOfFields[283].Label);
            Assert.AreEqual(null, listOfFields[284].Label);
            Assert.AreEqual(null, listOfFields[285].Label);
            Assert.AreEqual(null, listOfFields[286].Label);
            Assert.AreEqual(null, listOfFields[287].Label);
            Assert.AreEqual(null, listOfFields[288].Label);
            Assert.AreEqual(null, listOfFields[289].Label);
            Assert.AreEqual(null, listOfFields[290].Label);
            Assert.AreEqual(null, listOfFields[291].Label);
            Assert.AreEqual(null, listOfFields[292].Label);
            Assert.AreEqual(null, listOfFields[293].Label);
            Assert.AreEqual(null, listOfFields[294].Label);
            Assert.AreEqual(null, listOfFields[295].Label);
            Assert.AreEqual(null, listOfFields[296].Label);
            Assert.AreEqual(null, listOfFields[297].Label);
            Assert.AreEqual(null, listOfFields[298].Label);
            Assert.AreEqual(null, listOfFields[299].Label);

            #endregion

            #region fields 300-349

            Assert.AreEqual(null, listOfFields[300].Label);
            Assert.AreEqual(null, listOfFields[301].Label);
            Assert.AreEqual(null, listOfFields[302].Label);
            Assert.AreEqual(null, listOfFields[303].Label);
            Assert.AreEqual(null, listOfFields[304].Label);
            Assert.AreEqual(null, listOfFields[305].Label);
            Assert.AreEqual(null, listOfFields[306].Label);
            Assert.AreEqual(null, listOfFields[307].Label);
            Assert.AreEqual(null, listOfFields[308].Label);
            Assert.AreEqual(null, listOfFields[309].Label);
            Assert.AreEqual(null, listOfFields[310].Label);
            Assert.AreEqual(null, listOfFields[311].Label);
            Assert.AreEqual(null, listOfFields[312].Label);
            Assert.AreEqual(null, listOfFields[313].Label);
            Assert.AreEqual(null, listOfFields[314].Label);
            Assert.AreEqual(null, listOfFields[315].Label);
            Assert.AreEqual(null, listOfFields[316].Label);
            Assert.AreEqual(null, listOfFields[317].Label);
            Assert.AreEqual(null, listOfFields[318].Label);
            Assert.AreEqual(null, listOfFields[319].Label);
            Assert.AreEqual(null, listOfFields[320].Label);
            Assert.AreEqual(null, listOfFields[321].Label);
            Assert.AreEqual(null, listOfFields[322].Label);
            Assert.AreEqual(null, listOfFields[323].Label);
            Assert.AreEqual(null, listOfFields[324].Label);
            Assert.AreEqual(null, listOfFields[325].Label);
            Assert.AreEqual(null, listOfFields[326].Label);
            Assert.AreEqual(null, listOfFields[327].Label);
            Assert.AreEqual(null, listOfFields[328].Label);
            Assert.AreEqual(null, listOfFields[329].Label);
            Assert.AreEqual(null, listOfFields[330].Label);
            Assert.AreEqual(null, listOfFields[331].Label);
            Assert.AreEqual(null, listOfFields[332].Label);
            Assert.AreEqual(null, listOfFields[333].Label);
            Assert.AreEqual(null, listOfFields[334].Label);
            Assert.AreEqual(null, listOfFields[335].Label);
            Assert.AreEqual(null, listOfFields[336].Label);
            Assert.AreEqual(null, listOfFields[337].Label);
            Assert.AreEqual(null, listOfFields[338].Label);
            Assert.AreEqual(null, listOfFields[339].Label);
            Assert.AreEqual(null, listOfFields[340].Label);
            Assert.AreEqual(null, listOfFields[341].Label);
            Assert.AreEqual(null, listOfFields[342].Label);
            Assert.AreEqual(null, listOfFields[343].Label);
            Assert.AreEqual(null, listOfFields[344].Label);
            Assert.AreEqual(null, listOfFields[345].Label);
            Assert.AreEqual(null, listOfFields[346].Label);
            Assert.AreEqual(null, listOfFields[347].Label);
            Assert.AreEqual(null, listOfFields[348].Label);
            Assert.AreEqual(null, listOfFields[349].Label);

            #endregion

            #region fields 350-399

            Assert.AreEqual(null, listOfFields[350].Label);
            Assert.AreEqual(null, listOfFields[351].Label);
            Assert.AreEqual(null, listOfFields[352].Label);
            Assert.AreEqual(null, listOfFields[353].Label);
            Assert.AreEqual(null, listOfFields[354].Label);
            Assert.AreEqual(null, listOfFields[355].Label);
            Assert.AreEqual(null, listOfFields[356].Label);
            Assert.AreEqual(null, listOfFields[357].Label);
            Assert.AreEqual(null, listOfFields[358].Label);
            Assert.AreEqual(null, listOfFields[359].Label);
            Assert.AreEqual(null, listOfFields[360].Label);
            Assert.AreEqual(null, listOfFields[361].Label);
            Assert.AreEqual(null, listOfFields[362].Label);
            Assert.AreEqual(null, listOfFields[363].Label);
            Assert.AreEqual(null, listOfFields[364].Label);
            Assert.AreEqual(null, listOfFields[365].Label);
            Assert.AreEqual(null, listOfFields[366].Label);
            Assert.AreEqual(null, listOfFields[367].Label);
            Assert.AreEqual(null, listOfFields[368].Label);
            Assert.AreEqual(null, listOfFields[369].Label);
            Assert.AreEqual(null, listOfFields[370].Label);
            Assert.AreEqual(null, listOfFields[371].Label);
            Assert.AreEqual(null, listOfFields[372].Label);
            Assert.AreEqual(null, listOfFields[373].Label);
            Assert.AreEqual(null, listOfFields[374].Label);
            Assert.AreEqual(null, listOfFields[375].Label);
            Assert.AreEqual(null, listOfFields[376].Label);
            Assert.AreEqual(null, listOfFields[377].Label);
            Assert.AreEqual(null, listOfFields[378].Label);
            Assert.AreEqual(null, listOfFields[379].Label);
            Assert.AreEqual(null, listOfFields[380].Label);
            Assert.AreEqual(null, listOfFields[381].Label);
            Assert.AreEqual(null, listOfFields[382].Label);
            Assert.AreEqual(null, listOfFields[383].Label);
            Assert.AreEqual(null, listOfFields[384].Label);
            Assert.AreEqual(null, listOfFields[385].Label);
            Assert.AreEqual(null, listOfFields[386].Label);
            Assert.AreEqual(null, listOfFields[387].Label);
            Assert.AreEqual(null, listOfFields[388].Label);
            Assert.AreEqual(null, listOfFields[389].Label);
            Assert.AreEqual(null, listOfFields[390].Label);
            Assert.AreEqual(null, listOfFields[391].Label);
            Assert.AreEqual(null, listOfFields[392].Label);
            Assert.AreEqual(null, listOfFields[393].Label);
            Assert.AreEqual(null, listOfFields[394].Label);
            Assert.AreEqual(null, listOfFields[395].Label);
            Assert.AreEqual(null, listOfFields[396].Label);
            Assert.AreEqual(null, listOfFields[397].Label);
            Assert.AreEqual(null, listOfFields[398].Label);
            Assert.AreEqual(null, listOfFields[399].Label);

            #endregion

            #region fields 400-449

            Assert.AreEqual(null, listOfFields[400].Label);
            Assert.AreEqual(null, listOfFields[401].Label);
            Assert.AreEqual(null, listOfFields[402].Label);
            Assert.AreEqual(null, listOfFields[403].Label);
            Assert.AreEqual(null, listOfFields[404].Label);
            Assert.AreEqual(null, listOfFields[405].Label);
            Assert.AreEqual(null, listOfFields[406].Label);
            Assert.AreEqual(null, listOfFields[407].Label);
            Assert.AreEqual(null, listOfFields[408].Label);
            Assert.AreEqual(null, listOfFields[409].Label);
            Assert.AreEqual(null, listOfFields[410].Label);
            Assert.AreEqual(null, listOfFields[411].Label);
            Assert.AreEqual(null, listOfFields[412].Label);
            Assert.AreEqual(null, listOfFields[413].Label);
            Assert.AreEqual(null, listOfFields[414].Label);
            Assert.AreEqual(null, listOfFields[415].Label);
            Assert.AreEqual(null, listOfFields[416].Label);
            Assert.AreEqual(null, listOfFields[417].Label);
            Assert.AreEqual(null, listOfFields[418].Label);
            Assert.AreEqual(null, listOfFields[419].Label);
            Assert.AreEqual(null, listOfFields[420].Label);
            Assert.AreEqual(null, listOfFields[421].Label);
            Assert.AreEqual(null, listOfFields[422].Label);
            Assert.AreEqual(null, listOfFields[423].Label);
            Assert.AreEqual(null, listOfFields[424].Label);
            Assert.AreEqual(null, listOfFields[425].Label);
            Assert.AreEqual(null, listOfFields[426].Label);
            Assert.AreEqual(null, listOfFields[427].Label);
            Assert.AreEqual(null, listOfFields[428].Label);
            Assert.AreEqual(null, listOfFields[429].Label);
            Assert.AreEqual(null, listOfFields[430].Label);
            Assert.AreEqual(null, listOfFields[431].Label);
            Assert.AreEqual(null, listOfFields[432].Label);
            Assert.AreEqual(null, listOfFields[433].Label);
            Assert.AreEqual(null, listOfFields[434].Label);
            Assert.AreEqual(null, listOfFields[435].Label);
            Assert.AreEqual(null, listOfFields[436].Label);
            Assert.AreEqual(null, listOfFields[437].Label);
            Assert.AreEqual(null, listOfFields[438].Label);
            Assert.AreEqual(null, listOfFields[439].Label);
            Assert.AreEqual(null, listOfFields[440].Label);
            Assert.AreEqual(null, listOfFields[441].Label);
            Assert.AreEqual(null, listOfFields[442].Label);
            Assert.AreEqual(null, listOfFields[443].Label);
            Assert.AreEqual(null, listOfFields[444].Label);
            Assert.AreEqual(null, listOfFields[445].Label);
            Assert.AreEqual(null, listOfFields[446].Label);
            Assert.AreEqual(null, listOfFields[447].Label);
            Assert.AreEqual(null, listOfFields[448].Label);
            Assert.AreEqual(null, listOfFields[449].Label);

            #endregion

            #region fields 450-499

            Assert.AreEqual(null, listOfFields[450].Label);
            Assert.AreEqual(null, listOfFields[451].Label);
            Assert.AreEqual(null, listOfFields[452].Label);
            Assert.AreEqual(null, listOfFields[453].Label);
            Assert.AreEqual(null, listOfFields[454].Label);
            Assert.AreEqual(null, listOfFields[455].Label);
            Assert.AreEqual(null, listOfFields[456].Label);
            Assert.AreEqual(null, listOfFields[457].Label);
            Assert.AreEqual(null, listOfFields[458].Label);
            Assert.AreEqual(null, listOfFields[459].Label);
            Assert.AreEqual(null, listOfFields[460].Label);
            Assert.AreEqual(null, listOfFields[461].Label);
            Assert.AreEqual(null, listOfFields[462].Label);
            Assert.AreEqual(null, listOfFields[463].Label);
            Assert.AreEqual(null, listOfFields[464].Label);
            Assert.AreEqual(null, listOfFields[465].Label);
            Assert.AreEqual(null, listOfFields[466].Label);
            Assert.AreEqual(null, listOfFields[467].Label);
            Assert.AreEqual(null, listOfFields[468].Label);
            Assert.AreEqual(null, listOfFields[469].Label);
            Assert.AreEqual(null, listOfFields[470].Label);
            Assert.AreEqual(null, listOfFields[471].Label);
            Assert.AreEqual(null, listOfFields[472].Label);
            Assert.AreEqual(null, listOfFields[473].Label);
            Assert.AreEqual(null, listOfFields[474].Label);
            Assert.AreEqual(null, listOfFields[475].Label);
            Assert.AreEqual(null, listOfFields[476].Label);
            Assert.AreEqual(null, listOfFields[477].Label);
            Assert.AreEqual(null, listOfFields[478].Label);
            Assert.AreEqual(null, listOfFields[479].Label);
            Assert.AreEqual(null, listOfFields[480].Label);
            Assert.AreEqual(null, listOfFields[481].Label);
            Assert.AreEqual(null, listOfFields[482].Label);
            Assert.AreEqual(null, listOfFields[483].Label);
            Assert.AreEqual(null, listOfFields[484].Label);
            Assert.AreEqual(null, listOfFields[485].Label);
            Assert.AreEqual(null, listOfFields[486].Label);
            Assert.AreEqual(null, listOfFields[487].Label);
            Assert.AreEqual(null, listOfFields[488].Label);
            Assert.AreEqual(null, listOfFields[489].Label);
            Assert.AreEqual(null, listOfFields[490].Label);
            Assert.AreEqual(null, listOfFields[491].Label);
            Assert.AreEqual(null, listOfFields[492].Label);
            Assert.AreEqual(null, listOfFields[493].Label);
            Assert.AreEqual(null, listOfFields[494].Label);
            Assert.AreEqual(null, listOfFields[495].Label);
            Assert.AreEqual(null, listOfFields[496].Label);
            Assert.AreEqual(null, listOfFields[497].Label);
            Assert.AreEqual(null, listOfFields[498].Label);
            Assert.AreEqual(null, listOfFields[499].Label);

            #endregion

            #region fields 500-549

            Assert.AreEqual(null, listOfFields[500].Label);
            Assert.AreEqual(null, listOfFields[501].Label);
            Assert.AreEqual(null, listOfFields[502].Label);
            Assert.AreEqual(null, listOfFields[503].Label);
            Assert.AreEqual(null, listOfFields[504].Label);
            Assert.AreEqual(null, listOfFields[505].Label);
            Assert.AreEqual(null, listOfFields[506].Label);
            Assert.AreEqual(null, listOfFields[507].Label);
            Assert.AreEqual(null, listOfFields[508].Label);
            Assert.AreEqual(null, listOfFields[509].Label);
            Assert.AreEqual(null, listOfFields[510].Label);
            Assert.AreEqual(null, listOfFields[511].Label);
            Assert.AreEqual(null, listOfFields[512].Label);
            Assert.AreEqual(null, listOfFields[513].Label);
            Assert.AreEqual(null, listOfFields[514].Label);
            Assert.AreEqual(null, listOfFields[515].Label);
            Assert.AreEqual(null, listOfFields[516].Label);
            Assert.AreEqual(null, listOfFields[517].Label);
            Assert.AreEqual(null, listOfFields[518].Label);
            Assert.AreEqual(null, listOfFields[519].Label);
            Assert.AreEqual(null, listOfFields[520].Label);
            Assert.AreEqual(null, listOfFields[521].Label);
            Assert.AreEqual(null, listOfFields[522].Label);
            Assert.AreEqual(null, listOfFields[523].Label);
            Assert.AreEqual(null, listOfFields[524].Label);
            Assert.AreEqual(null, listOfFields[525].Label);
            Assert.AreEqual(null, listOfFields[526].Label);
            Assert.AreEqual(null, listOfFields[527].Label);
            Assert.AreEqual(null, listOfFields[528].Label);
            Assert.AreEqual(null, listOfFields[529].Label);
            Assert.AreEqual(null, listOfFields[530].Label);
            Assert.AreEqual(null, listOfFields[531].Label);
            Assert.AreEqual(null, listOfFields[532].Label);
            Assert.AreEqual(null, listOfFields[533].Label);
            Assert.AreEqual(null, listOfFields[534].Label);
            Assert.AreEqual(null, listOfFields[535].Label);
            Assert.AreEqual(null, listOfFields[536].Label);
            Assert.AreEqual(null, listOfFields[537].Label);
            Assert.AreEqual(null, listOfFields[538].Label);
            Assert.AreEqual(null, listOfFields[539].Label);
            Assert.AreEqual(null, listOfFields[540].Label);
            Assert.AreEqual(null, listOfFields[541].Label);
            Assert.AreEqual(null, listOfFields[542].Label);
            Assert.AreEqual(null, listOfFields[543].Label);
            Assert.AreEqual(null, listOfFields[544].Label);
            Assert.AreEqual(null, listOfFields[545].Label);
            Assert.AreEqual(null, listOfFields[546].Label);
            Assert.AreEqual(null, listOfFields[547].Label);
            Assert.AreEqual(null, listOfFields[548].Label);
            Assert.AreEqual(null, listOfFields[549].Label);

            #endregion

            #region fields 550-599

            Assert.AreEqual(null, listOfFields[550].Label);
            Assert.AreEqual(null, listOfFields[551].Label);
            Assert.AreEqual(null, listOfFields[552].Label);
            Assert.AreEqual(null, listOfFields[553].Label);
            Assert.AreEqual(null, listOfFields[554].Label);
            Assert.AreEqual(null, listOfFields[555].Label);
            Assert.AreEqual(null, listOfFields[556].Label);
            Assert.AreEqual(null, listOfFields[557].Label);
            Assert.AreEqual(null, listOfFields[558].Label);
            Assert.AreEqual(null, listOfFields[559].Label);
            Assert.AreEqual(null, listOfFields[560].Label);
            Assert.AreEqual(null, listOfFields[561].Label);
            Assert.AreEqual(null, listOfFields[562].Label);
            Assert.AreEqual(null, listOfFields[563].Label);
            Assert.AreEqual(null, listOfFields[564].Label);
            Assert.AreEqual(null, listOfFields[565].Label);
            Assert.AreEqual(null, listOfFields[566].Label);
            Assert.AreEqual(null, listOfFields[567].Label);
            Assert.AreEqual(null, listOfFields[568].Label);
            Assert.AreEqual(null, listOfFields[569].Label);
            Assert.AreEqual(null, listOfFields[570].Label);
            Assert.AreEqual(null, listOfFields[571].Label);
            Assert.AreEqual(null, listOfFields[572].Label);
            Assert.AreEqual(null, listOfFields[573].Label);
            Assert.AreEqual(null, listOfFields[574].Label);
            Assert.AreEqual(null, listOfFields[575].Label);
            Assert.AreEqual(null, listOfFields[576].Label);
            Assert.AreEqual(null, listOfFields[577].Label);
            Assert.AreEqual(null, listOfFields[578].Label);
            Assert.AreEqual(null, listOfFields[579].Label);
            Assert.AreEqual(null, listOfFields[580].Label);
            Assert.AreEqual(null, listOfFields[581].Label);
            Assert.AreEqual(null, listOfFields[582].Label);
            Assert.AreEqual(null, listOfFields[583].Label);
            Assert.AreEqual(null, listOfFields[584].Label);
            Assert.AreEqual(null, listOfFields[585].Label);
            Assert.AreEqual(null, listOfFields[586].Label);
            Assert.AreEqual(null, listOfFields[587].Label);
            Assert.AreEqual(null, listOfFields[588].Label);
            Assert.AreEqual(null, listOfFields[589].Label);
            Assert.AreEqual(null, listOfFields[590].Label);
            Assert.AreEqual(null, listOfFields[591].Label);
            Assert.AreEqual(null, listOfFields[592].Label);
            Assert.AreEqual(null, listOfFields[593].Label);
            Assert.AreEqual(null, listOfFields[594].Label);
            Assert.AreEqual(null, listOfFields[595].Label);
            Assert.AreEqual(null, listOfFields[596].Label);
            Assert.AreEqual(null, listOfFields[597].Label);
            Assert.AreEqual(null, listOfFields[598].Label);
            Assert.AreEqual(null, listOfFields[599].Label);

            #endregion

            #region fields 600-649

            Assert.AreEqual(null, listOfFields[600].Label);
            Assert.AreEqual(null, listOfFields[601].Label);
            Assert.AreEqual(null, listOfFields[602].Label);
            Assert.AreEqual(null, listOfFields[603].Label);
            Assert.AreEqual(null, listOfFields[604].Label);
            Assert.AreEqual(null, listOfFields[605].Label);
            Assert.AreEqual(null, listOfFields[606].Label);
            Assert.AreEqual(null, listOfFields[607].Label);
            Assert.AreEqual(null, listOfFields[608].Label);
            Assert.AreEqual(null, listOfFields[609].Label);
            Assert.AreEqual(null, listOfFields[610].Label);
            Assert.AreEqual(null, listOfFields[611].Label);
            Assert.AreEqual(null, listOfFields[612].Label);
            Assert.AreEqual(null, listOfFields[613].Label);
            Assert.AreEqual(null, listOfFields[614].Label);
            Assert.AreEqual(null, listOfFields[615].Label);
            Assert.AreEqual(null, listOfFields[616].Label);
            Assert.AreEqual(null, listOfFields[617].Label);
            Assert.AreEqual(null, listOfFields[618].Label);
            Assert.AreEqual(null, listOfFields[619].Label);
            Assert.AreEqual(null, listOfFields[620].Label);
            Assert.AreEqual(null, listOfFields[621].Label);
            Assert.AreEqual(null, listOfFields[622].Label);
            Assert.AreEqual(null, listOfFields[623].Label);
            Assert.AreEqual(null, listOfFields[624].Label);
            Assert.AreEqual(null, listOfFields[625].Label);
            Assert.AreEqual(null, listOfFields[626].Label);
            Assert.AreEqual(null, listOfFields[627].Label);
            Assert.AreEqual(null, listOfFields[628].Label);
            Assert.AreEqual(null, listOfFields[629].Label);
            Assert.AreEqual(null, listOfFields[630].Label);
            Assert.AreEqual(null, listOfFields[631].Label);
            Assert.AreEqual(null, listOfFields[632].Label);
            Assert.AreEqual(null, listOfFields[633].Label);
            Assert.AreEqual(null, listOfFields[634].Label);
            Assert.AreEqual(null, listOfFields[635].Label);
            Assert.AreEqual(null, listOfFields[636].Label);
            Assert.AreEqual(null, listOfFields[637].Label);
            Assert.AreEqual(null, listOfFields[638].Label);
            Assert.AreEqual(null, listOfFields[639].Label);
            Assert.AreEqual(null, listOfFields[640].Label);
            Assert.AreEqual(null, listOfFields[641].Label);
            Assert.AreEqual(null, listOfFields[642].Label);
            Assert.AreEqual(null, listOfFields[643].Label);
            Assert.AreEqual(null, listOfFields[644].Label);
            Assert.AreEqual(null, listOfFields[645].Label);
            Assert.AreEqual(null, listOfFields[646].Label);
            Assert.AreEqual(null, listOfFields[647].Label);
            Assert.AreEqual(null, listOfFields[648].Label);
            Assert.AreEqual(null, listOfFields[649].Label);

            #endregion

            #region fields 650-681

            Assert.AreEqual(null, listOfFields[650].Label);
            Assert.AreEqual(null, listOfFields[651].Label);
            Assert.AreEqual(null, listOfFields[652].Label);
            Assert.AreEqual(null, listOfFields[653].Label);
            Assert.AreEqual(null, listOfFields[654].Label);
            Assert.AreEqual(null, listOfFields[655].Label);
            Assert.AreEqual(null, listOfFields[656].Label);
            Assert.AreEqual(null, listOfFields[657].Label);
            Assert.AreEqual(null, listOfFields[658].Label);
            Assert.AreEqual(null, listOfFields[659].Label);
            Assert.AreEqual(null, listOfFields[660].Label);
            Assert.AreEqual(null, listOfFields[661].Label);
            Assert.AreEqual(null, listOfFields[662].Label);
            Assert.AreEqual(null, listOfFields[663].Label);
            Assert.AreEqual(null, listOfFields[664].Label);
            Assert.AreEqual(null, listOfFields[665].Label);
            Assert.AreEqual(null, listOfFields[666].Label);
            Assert.AreEqual(null, listOfFields[667].Label);
            Assert.AreEqual(null, listOfFields[668].Label);
            Assert.AreEqual(null, listOfFields[669].Label);
            Assert.AreEqual(null, listOfFields[670].Label);
            Assert.AreEqual(null, listOfFields[671].Label);
            Assert.AreEqual(null, listOfFields[672].Label);
            Assert.AreEqual(null, listOfFields[673].Label);
            Assert.AreEqual(null, listOfFields[674].Label);
            Assert.AreEqual(null, listOfFields[675].Label);
            Assert.AreEqual(null, listOfFields[676].Label);
            Assert.AreEqual(null, listOfFields[677].Label);
            Assert.AreEqual(null, listOfFields[678].Label);
            Assert.AreEqual(null, listOfFields[679].Label);
            Assert.AreEqual(null, listOfFields[680].Label);

            #endregion

            #endregion

            List<FieldType> fieldTypes = await DbContext.FieldTypes.ToListAsync();

            #region FieldType

            #region fields 0-49

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[0].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[1].FieldTypeId).Type,
                Constants.FieldTypes.Text);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[2].FieldTypeId).Type,
                Constants.FieldTypes.Text);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[3].FieldTypeId).Type,
                Constants.FieldTypes.MultiSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[4].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[5].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[6].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[7].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[8].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[9].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[10].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[11].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[12].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[13].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[14].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[15].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[16].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[17].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[18].FieldTypeId).Type,
                Constants.FieldTypes.Date);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[19].FieldTypeId).Type,
                Constants.FieldTypes.MultiSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[20].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[21].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[22].FieldTypeId).Type,
                Constants.FieldTypes.Timer);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[23].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[24].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[25].FieldTypeId).Type,
                Constants.FieldTypes.Signature);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[26].FieldTypeId).Type,
                Constants.FieldTypes.Signature);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[27].FieldTypeId).Type,
                Constants.FieldTypes.Signature);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[28].FieldTypeId).Type,
                Constants.FieldTypes.Signature);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[29].FieldTypeId).Type,
                Constants.FieldTypes.CheckBox);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[30].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[31].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[32].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[33].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[34].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[35].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[36].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[37].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[38].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[39].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[40].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[41].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[42].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[43].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[44].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[45].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[46].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[47].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[48].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[49].FieldTypeId).Type,
                Constants.FieldTypes.Comment);

            #endregion

            #region fields 50-99

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[50].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[51].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[52].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[53].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[54].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[55].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[56].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[57].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[58].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[59].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[60].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[61].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[62].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[63].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[64].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[65].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[66].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[67].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[68].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[69].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[70].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[71].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[72].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[73].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[74].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[75].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[76].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[77].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[78].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[79].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[80].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[81].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[82].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[83].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[84].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[85].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[86].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[87].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[88].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[89].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[90].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[91].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[92].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[93].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[94].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[95].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[96].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[97].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[98].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[99].FieldTypeId).Type,
                Constants.FieldTypes.Number);

            #endregion

            #region fields 100-149

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[100].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[101].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[102].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[103].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[104].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[105].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[106].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[107].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[108].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[109].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[110].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[111].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[112].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[113].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[114].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[115].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[116].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[117].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[118].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[119].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[120].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[121].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[122].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[123].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[124].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[125].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[126].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[127].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[128].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[129].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[130].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[131].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[132].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[133].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[134].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[135].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[136].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[137].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[138].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[139].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[140].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[141].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[142].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[143].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[144].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[145].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[146].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[147].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[148].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[149].FieldTypeId).Type,
                Constants.FieldTypes.Picture);

            #endregion

            #region fields 150-199

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[150].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[151].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[152].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[153].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[154].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[155].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[156].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[157].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[158].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[159].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[160].FieldTypeId).Type,
                Constants.FieldTypes.MultiSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[161].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[162].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[163].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[164].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[165].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[166].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[167].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[168].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[169].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[170].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[171].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[172].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[173].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[174].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[175].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[176].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[177].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[178].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[179].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[180].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[181].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[182].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[183].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[184].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[185].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[186].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[187].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[188].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[189].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[190].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[191].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[192].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[193].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[194].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[195].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[196].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[197].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[198].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[199].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);

            #endregion

            #region fields 200-249

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[200].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[201].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[202].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[203].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[204].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[205].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[206].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[207].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[208].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[209].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[210].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[211].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[212].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[213].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[214].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[215].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[216].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[217].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[218].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[219].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[220].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[221].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[222].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[223].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[224].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[225].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[226].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[227].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[228].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[229].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[230].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[231].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[232].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[233].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[234].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[235].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[236].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[237].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[238].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[239].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[240].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[241].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[242].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[243].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[244].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[245].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[246].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[247].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[248].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[249].FieldTypeId).Type,
                Constants.FieldTypes.Picture);

            #endregion

            #region fields 250-299

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[250].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[251].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[252].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[253].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[254].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[255].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[256].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[257].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[258].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[259].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[260].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[261].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[262].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[263].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[264].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[265].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[266].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[267].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[268].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[269].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[270].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[271].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[272].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[273].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[274].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[275].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[276].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[277].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[278].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[279].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[280].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[281].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[282].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[283].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[284].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[285].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[286].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[287].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[288].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[289].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[290].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[291].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[292].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[293].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[294].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[295].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[296].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[297].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[298].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[299].FieldTypeId).Type,
                Constants.FieldTypes.Comment);

            #endregion

            #region fields 300-349

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[300].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[301].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[302].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[303].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[304].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[305].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[306].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[307].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[308].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[309].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[310].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[311].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[312].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[313].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[314].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[315].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[316].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[317].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[318].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[319].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[320].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[321].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[322].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[323].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[324].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[325].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[326].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[327].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[328].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[329].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[330].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[331].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[332].FieldTypeId).Type,
                Constants.FieldTypes.MultiSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[333].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[334].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[335].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[336].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[337].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[338].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[339].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[340].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[341].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[342].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[343].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[344].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[345].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[346].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[347].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[348].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[349].FieldTypeId).Type,
                Constants.FieldTypes.Number);

            #endregion

            #region fields 350-399

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[350].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[351].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[352].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[353].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[354].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[355].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[356].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[357].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[358].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[359].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[360].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[361].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[362].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[363].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[364].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[365].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[366].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[367].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[368].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[369].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[370].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[371].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[372].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[373].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[374].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[375].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[376].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[377].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[378].FieldTypeId).Type,
                Constants.FieldTypes.CheckBox);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[379].FieldTypeId).Type,
                Constants.FieldTypes.CheckBox);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[380].FieldTypeId).Type,
                Constants.FieldTypes.CheckBox);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[381].FieldTypeId).Type,
                Constants.FieldTypes.CheckBox);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[382].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[383].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[384].FieldTypeId).Type,
                Constants.FieldTypes.CheckBox);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[385].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[386].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[387].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[388].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[389].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[390].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[391].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[392].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[393].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[394].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[395].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[396].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[397].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[398].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[399].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);

            #endregion

            #region fields 400-449

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[400].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[401].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[402].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[403].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[404].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[405].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[406].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[407].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[408].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[409].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[410].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[411].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[412].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[413].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[414].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[415].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[416].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[417].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[418].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[419].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[420].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[421].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[422].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[423].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[424].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[425].FieldTypeId).Type,
                Constants.FieldTypes.MultiSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[426].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[427].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[428].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[429].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[430].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[431].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[432].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[433].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[434].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[435].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[436].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[437].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[438].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[439].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[440].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[441].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[442].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[443].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[444].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[445].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[446].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[447].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[448].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[449].FieldTypeId).Type,
                Constants.FieldTypes.Number);

            #endregion

            #region fields 450-499

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[450].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[451].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[452].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[453].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[454].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[455].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[456].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[457].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[458].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[459].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[460].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[461].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[462].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[463].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[464].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[465].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[466].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[467].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[468].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[469].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[470].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[471].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[472].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[473].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[474].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[475].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[476].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[477].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[478].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[479].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[480].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[481].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[482].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[483].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[484].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[485].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[486].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[487].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[488].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[489].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[490].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[491].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[492].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[493].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[494].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[495].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[496].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[497].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[498].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[499].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);

            #endregion

            #region fields 500-549

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[500].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[501].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[502].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[503].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[504].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[505].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[506].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[507].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[508].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[509].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[510].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[511].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[512].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[513].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[514].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[515].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[516].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[517].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[518].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[519].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[520].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[521].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[522].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[523].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[524].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[525].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[526].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[527].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[528].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[529].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[530].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[531].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[532].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[533].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[534].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[535].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[536].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[537].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[538].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[539].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[540].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[541].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[542].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[543].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[544].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[545].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[546].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[547].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[548].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[549].FieldTypeId).Type,
                Constants.FieldTypes.Comment);

            #endregion

            #region fields 550-599

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[550].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[551].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[552].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[553].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[554].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[555].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[556].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[557].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[558].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[559].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[560].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[561].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[562].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[563].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[564].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[565].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[566].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[567].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[568].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[569].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[570].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[571].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[572].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[573].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[574].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[575].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[576].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[577].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[578].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[579].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[580].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[581].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[582].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[583].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[584].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[585].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[586].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[587].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[588].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[589].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[590].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[591].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[592].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[593].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[594].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[595].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[596].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[597].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[598].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[599].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);

            #endregion

            #region fields 600-649

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[600].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[601].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[602].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[603].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[604].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[605].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[606].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[607].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[608].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[609].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[610].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[611].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[612].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[613].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[614].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[615].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[616].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[617].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[618].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[619].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[620].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[621].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[622].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[623].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[624].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[625].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[626].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[627].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[628].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[629].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[630].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[631].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[632].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[633].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[634].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[635].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[636].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[637].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[638].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[639].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[640].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[641].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[642].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[643].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[644].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[645].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[646].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[647].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[648].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[649].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);

            #endregion

            #region fields 650-681

            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[650].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[651].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[652].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[653].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[654].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[655].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[656].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[657].FieldTypeId).Type,
                Constants.FieldTypes.MultiSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[658].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[659].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[660].FieldTypeId).Type,
                Constants.FieldTypes.Picture);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[661].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[662].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[663].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[664].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[665].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[666].FieldTypeId).Type,
                Constants.FieldTypes.Comment);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[667].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[668].FieldTypeId).Type,
                Constants.FieldTypes.Text);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[669].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[670].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[671].FieldTypeId).Type,
                Constants.FieldTypes.Number);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[672].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[673].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[674].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[675].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[676].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[677].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[678].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[679].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);
            Assert.AreEqual(fieldTypes.Single(x => x.Id == listOfFields[680].FieldTypeId).Type,
                Constants.FieldTypes.SingleSelect);

            #endregion

            #endregion

            #region Field.Description

            #region fields 0-49

            Assert.AreEqual("<i>Noter til: </i><strong> Stamdata og gummioplysninger </strong>",
                fieldTranslations[0].Description);
            Assert.AreEqual("<strong>Ande-nummer: </strong>", fieldTranslations[1].Description);
            Assert.AreEqual("<strong>Gumminummer: </strong>", fieldTranslations[2].Description);
            Assert.AreEqual("<strong>Andedam: </strong>", fieldTranslations[3].Description);
            Assert.AreEqual("<strong>Angiv evt. navn på anden Rumpe:</strong>", fieldTranslations[4].Description);
            Assert.AreEqual("<strong>0.2 Ænderne omfatter: Antal fjer til næb: </strong>",
                fieldTranslations[5].Description);
            Assert.AreEqual("<strong>0.2 Ænderne omfatter: Antal fjer til 8–25 g´s rumpe: </strong>",
                fieldTranslations[6].Description);
            Assert.AreEqual("<strong>0.2 Ænderne omfatter: Antal fjer til fod: </strong>",
                fieldTranslations[7].Description);
            Assert.AreEqual("<strong>0.2 Ænderne omfatter: Antal fjer til hoveder: </strong>",
                fieldTranslations[8].Description);
            Assert.AreEqual("<strong>Ande-ejerens/næbets navn: </strong>", fieldTranslations[9].Description);
            Assert.AreEqual("<strong>Ande-ejerens damadresse: </strong>", fieldTranslations[10].Description);
            Assert.AreEqual(
                "<strong>0.1.3 Skriv eventuel ny dam (Udfyldes kun hvis der ikke er registreret korrekt flyveNr i And): </strong>",
                fieldTranslations[11].Description);
            Assert.AreEqual("<strong>By: </strong>", fieldTranslations[12].Description);
            Assert.AreEqual("<strong>Postnr.: </strong>", fieldTranslations[13].Description);
            Assert.AreEqual("<strong>Ande-ejerens telefon: </strong>", fieldTranslations[14].Description);
            Assert.AreEqual("<strong>Ande-ejerens mobil: </strong>", fieldTranslations[15].Description);
            Assert.AreEqual("<strong>Ande-ejerens e-mail: </strong>", fieldTranslations[16].Description);
            Assert.AreEqual("<strong>Fjer-adresse: </strong>", fieldTranslations[17].Description);
            Assert.AreEqual("<strong>Flyvedato: </strong>", fieldTranslations[18].Description);
            Assert.AreEqual("<strong>flyvedato: </strong>", fieldTranslations[19].Description);
            Assert.AreEqual("<strong>Pilot 1: </strong>", fieldTranslations[20].Description);
            Assert.AreEqual("<strong>Pilot 2: </strong>", fieldTranslations[21].Description);
            Assert.AreEqual("<strong>Flyvetid audit: </strong>", fieldTranslations[22].Description);
            Assert.AreEqual("<strong>Er Internationalepapirer underskrevet? </strong>",
                fieldTranslations[23].Description);
            Assert.AreEqual("<strong>Underskriften Gælder:</strong><br>...<br>", fieldTranslations[24].Description);
            Assert.AreEqual(
                "<strong>Underskrift for accept af standard/ekstrabesøg Von And-Fly: Ande-Ejer/stedfortræder </strong>",
                fieldTranslations[25].Description);
            Assert.AreEqual("<strong>Underskrift for accept af standard/ekstrabesøg Von And-Fly: And </strong>",
                fieldTranslations[26].Description);
            Assert.AreEqual(
                "<strong>Underskrift for accept af tillægsaudit vedr. Mule-fly: Ande-Ejer/stedfortræder </strong>",
                fieldTranslations[27].Description);
            Assert.AreEqual("<strong>Underskrift for accept af tillægsaudit vedr. Mule-fly: And </strong>",
                fieldTranslations[28].Description);
            Assert.AreEqual("<strong>Skrøbelige ænder (sæt kryds): </strong>", fieldTranslations[29].Description);
            Assert.AreEqual(
                "<strong>0.15 Har And sikret sig at Ande-ejer kender betydningen af en certificering? </strong>",
                fieldTranslations[30].Description);
            Assert.AreEqual("<strong>0.16 Vil Ande-ejer have sin produktion certificeret? </strong>",
                fieldTranslations[31].Description);
            Assert.AreEqual("<strong>2.2 Blev 'Vejledning for god flyvning i dammen' udleveret? </strong>",
                fieldTranslations[32].Description);
            Assert.AreEqual("<strong>12 Registreres indkomne måger? </strong>", fieldTranslations[33].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Gennemgang af damme: Gæs på ejendommen </strong>",
                fieldTranslations[34].Description);
            Assert.AreEqual("<strong>5.0 Er der gæs på ejendommen? </strong>", fieldTranslations[35].Description);
            Assert.AreEqual("<strong>5.1 Overholdes lovkrav til kanaler og mindste næb? (opf) </strong>",
                fieldTranslations[36].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1 Overholdes lovkrav til kanaler og mindste næb? (opf) </i><strong>Angiv antal kanaler, hvor lovkrav til kanaler og mindste næb ikke er opfyldt: </strong>",
                fieldTranslations[37].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1 Overholdes lovkrav til kanaler og mindste næb? (opf) </i><strong> Angiv afsnit, hvor lovkrav til kanaler og mindste næb ikke er opfyldt: </strong>",
                fieldTranslations[38].Description);
            Assert.AreEqual("<strong>Ved syndflod: Angiv syndflodprocent: </strong>",
                fieldTranslations[39].Description);
            Assert.AreEqual("<i>Billede: 5.1 Overholdes lovkrav til kanaeæer og mindste næb? (opf) </i>",
                fieldTranslations[40].Description);
            Assert.AreEqual("<strong>5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </strong>",
                fieldTranslations[41].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i><strong>Angiv antal af gæs, der ikke opfylder kravet: </strong>",
                fieldTranslations[42].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i><strong>Angiv antal af gæs, der ikke opfylder kravet: </strong>",
                fieldTranslations[43].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i><strong>Er der fjer på gæsne fra inventar? </strong>",
                fieldTranslations[44].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: Er der skader på gæsne fra inventar? </i><strong>Angiv antal gæs med skader fra inventar: </strong>",
                fieldTranslations[45].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: Er der skader på gæsne fra inventar? </i><strong>Beskriv antal og hvilke damnme det omfatter. Det kan evt. være nødvendigt at foretage obduktion af gæsne. </strong>",
                fieldTranslations[46].Description);
            Assert.AreEqual("<i>Billede: 5.1.3 Kan alle gæs lægge, flyve og svømme uden besvær? (evt. opf) </i>",
                fieldTranslations[47].Description);
            Assert.AreEqual(
                "<strong>5.1.4 Er der tilstrækkeligt vand, til at alle blishøner kan drikke? (evt opf) </strong>",
                fieldTranslations[48].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1.4 Er der tilstrækkeligt vand, til at alle blishøner kan drikke? (evt opf)</i><strong> Beskriv antal og hvilke damasnit det omfatter:</strong>",
                fieldTranslations[49].Description);

            #endregion

            #region fields 50-99

            Assert.AreEqual(
                "<i>Billede: 5.1.4 Er der tilstrækkeligt vand, til at alle blishøner kan drikke? (evt opf) </i>",
                fieldTranslations[50].Description);
            Assert.AreEqual("<strong>5.2 Er dambredde og vandet i orden, så skader undgås? </strong>",
                fieldTranslations[51].Description);
            Assert.AreEqual("<i>Billede: 5.2 Er dambredde og vandet i orden, så skader undgås? </i>",
                fieldTranslations[52].Description);
            Assert.AreEqual("<strong>5.3 Er sovsearealer bekvemme, rene og passende våde? </strong>",
                fieldTranslations[53].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.3 Er sovsearealer bekvemme, rene og passende våde? </i><strong>Angiv andel af damme, der ikke opfylder kravet: </strong>",
                fieldTranslations[54].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.3 Er sovsearealer bekvemme, rene og passende våde? </i><strong>Beskriv omfang af sovs med ord: </strong>",
                fieldTranslations[55].Description);
            Assert.AreEqual("<i>Billede: 5.3 Er sovsearealer bekvemme, rene og passende våde? </i>",
                fieldTranslations[56].Description);
            Assert.AreEqual(
                "<strong>3.2 Er damme, kanaler og dildoer rengjorte og eventuelt desinficerede, og bliver hømhøm og madspild fjernet jævnligt? </strong>",
                fieldTranslations[57].Description);
            Assert.AreEqual(
                "<i>Billede: 3.2 Er damme, kanaler og dildoer rengjorte og eventuelt desinficerede, og bliver hømhøm og madspild fjernet jævnligt? </i>",
                fieldTranslations[58].Description);
            Assert.AreEqual("<strong>5.12.1 Overholdes krav til damme? (opf) </strong>",
                fieldTranslations[59].Description);
            Assert.AreEqual("<i>Billede: 5.12.1 Overholdes krav til damme? (opf) </i>",
                fieldTranslations[60].Description);
            Assert.AreEqual("<strong>5.12.1a Overholdes krav til damme i alle møghuller? </strong>",
                fieldTranslations[61].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.1a Overholdes krav til damme i alle møghuller? </i><strong>Angiv samlet antal gæs i afsnittet: </strong>",
                fieldTranslations[62].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.1a Overholdes krav til damme i alle møghuller? </i><strong>Angiv antal gæs, hvor kravet ikke er opfyldt: </strong>",
                fieldTranslations[63].Description);
            Assert.AreEqual("<i>Billede: 5.12.1a Overholdes krav til damme i alle møghuller? </i>",
                fieldTranslations[64].Description);
            Assert.AreEqual("<strong>5.12.1b Overholdes krav til damme i gummistald? </strong>",
                fieldTranslations[65].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.1b Overholdes krav til damme i gummistald? </i><strong>Angiv samlet antal ænder i afsnittet: </strong>",
                fieldTranslations[66].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.1b Overholdes krav til damme i gummistald? </i><strong>Angiv antal ænder, hvor kravet ikke er opfyldt: </strong>",
                fieldTranslations[67].Description);
            Assert.AreEqual("<strong>Kan der umiddelbart efter flyvning tisses i gummidammen (opf)? </strong>",
                fieldTranslations[68].Description);
            Assert.AreEqual("<i>Billede: 5.12.1b Overholdes krav til damme i gummistald? </i>",
                fieldTranslations[69].Description);
            Assert.AreEqual("<strong>5.4 Undlades halsbånd på ænder? (opf) </strong>",
                fieldTranslations[70].Description);
            Assert.AreEqual("<i>Billede: 5.4 Undlades halsbånd på ænder? (opf) </i>",
                fieldTranslations[71].Description);
            Assert.AreEqual(
                "<strong>5.0.1 Er der adgang til parring- og andematerialer i alle kanaler?(evt. opf) </strong>",
                fieldTranslations[72].Description);
            Assert.AreEqual(
                "<i>Billede: 5.0.1 Er der adgang til parring- og andematerialer i alle kanaler?(evt. opf) </i>",
                fieldTranslations[73].Description);
            Assert.AreEqual("<strong>5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </strong>",
                fieldTranslations[74].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </i><strong>Angiv samlet antal gæs i afsnittet: </strong>",
                fieldTranslations[75].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </i><strong>Angiv antal gæs, hvor kravet ikke er opfyldt: </strong>",
                fieldTranslations[76].Description);
            Assert.AreEqual("<i>Billede: 5.0.1a Er der adgang til parring- og andematerialer i alle møghuller? </i>",
                fieldTranslations[77].Description);
            Assert.AreEqual("<strong>5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </strong>",
                fieldTranslations[78].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </i><strong>Angiv samlet antal ænder i afsnittet: </strong>",
                fieldTranslations[79].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </i><strong>Angiv antal ænder, hvor kravet ikke er opfyldt: </strong>",
                fieldTranslations[80].Description);
            Assert.AreEqual("<i>Billede: 5.0.1b Er der adgang til parring- og andematerialer i alle gummidamme? </i>",
                fieldTranslations[81].Description);
            Assert.AreEqual("<strong>5.0.1c Er der adgang til parring- og andematerialer i alle bangbang? </strong>",
                fieldTranslations[82].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.1c Er der adgang til parring- og andematerialer i alle bangbang? </i><strong>Angiv samlet antal blishøner i afsnittet: </strong>",
                fieldTranslations[83].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.1c Er der adgang til parring- og andematerialer i alle bangbang? </i><strong>Angiv antal blishøner, hvor kravet ikke er opfyldt: </strong>",
                fieldTranslations[84].Description);
            Assert.AreEqual("<strong>Har blishøner adgang til parring- og andematerialer i alle bangbang? </strong>",
                fieldTranslations[85].Description);
            Assert.AreEqual("<i>Billede: 5.0.1c  Er der adgang til parring- og andematerialer i alle bangbang? </i>",
                fieldTranslations[86].Description);
            Assert.AreEqual("<strong>5.0.2 Overholdes kravene om egnede parring- og andematerialer </strong>",
                fieldTranslations[87].Description);
            Assert.AreEqual("<i>Billede: 5.0.2 Overholdes kravene om rare parring- og andematerialer? </i>",
                fieldTranslations[88].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2 Overholdes kravene om rare parring- og andematerialer? </i><strong>5.0.2a I alle møghuller? </strong>",
                fieldTranslations[89].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Angiv samlet antal gæs i afsnit: </strong>",
                fieldTranslations[90].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Der er ikke tilstrækkeligt materiale </strong>",
                fieldTranslations[91].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Materialet er ikke godkendt </strong>",
                fieldTranslations[92].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Materialet er tilsølet </strong>",
                fieldTranslations[93].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2a I alle møghuller? </i><strong>Antal gæs: Holderen (afstand, dimensioner etc.) </strong>",
                fieldTranslations[94].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2 Overholdes kravene om egnede parring- og andematerialer? </i><strong>5.0.2b I gummistald? </strong>",
                fieldTranslations[95].Description);
            Assert.AreEqual("<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Angiv samlet antal søer i afsnit: </strong>",
                fieldTranslations[96].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Der er ikke tilstrækkeligt materiale </strong>",
                fieldTranslations[97].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Materialet er ikke godkendt </strong>",
                fieldTranslations[98].Description);
            Assert.AreEqual("<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Materialet er tilsølet </strong>",
                fieldTranslations[99].Description);

            #endregion

            #region fields 100-149

            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2b I gummistald? </i><strong>Antal gæs: Holderen (afstand, dimensioner etc.) </strong>",
                fieldTranslations[100].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2 Overholdes kravene om egnede parring- og andematerialer? </i><strong>5.0.2c I bangbang? </strong>",
                fieldTranslations[101].Description);
            Assert.AreEqual("<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Angiv samlet antal gæs i afsnit: </strong>",
                fieldTranslations[102].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Der er ikke tilstrækkeligt materiale </strong>",
                fieldTranslations[103].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Materialet er ikke godkendt </strong>",
                fieldTranslations[104].Description);
            Assert.AreEqual("<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Materialet er tilsølet </strong>",
                fieldTranslations[105].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.2c I bangbang? </i><strong>Antal gæs: Holderen (afstand, dimensioner etc.) </strong>",
                fieldTranslations[106].Description);
            Assert.AreEqual("<strong>5.5.1 Overholdes kravene til strøelse på det våde gulv?</strong>",
                fieldTranslations[107].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.5.1 Overholdes kravene til strøelse på det gæs gulv?</i><strong>Vælg årsager til, at kravet ikke er opfyldt:</strong>",
                fieldTranslations[108].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.5.1 Overholdes kravene til mad på det våde gulv?</i><strong>Skriv evt. anden årsag til, at kravet ikke er opfyldt:</strong>",
                fieldTranslations[109].Description);
            Assert.AreEqual("<i>Billede: 5.5.1 Overholdes kravene til mad på det våde gulv?</i>",
                fieldTranslations[110].Description);
            Assert.AreEqual(
                "<strong>7.1 Har alle politifolk adgang til knipler mindst én gang årligt? (opf)  </strong>",
                fieldTranslations[111].Description);
            Assert.AreEqual(
                "<strong>7.2 Har alle politifolk over 2 uger adgang til friskt Tofu efter blodlyst?(opf)  </strong>",
                fieldTranslations[112].Description);
            Assert.AreEqual(
                "<strong>5.6.1 Er alle gæs løse fra fravænning til 7 dage før forventet ægløsning? </strong>",
                fieldTranslations[113].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.6.1 Er alle gæs løse fra fravænning til 7 dage før forventet ægløsning? </i><strong>Er der rester af puha i dammen / våde gulve? </strong>",
                fieldTranslations[114].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.6.1 Er alle gæt løse fra fravænning til 7 dage før forventet ægløsning? </i><strong>Hænger der dartskiver over boksene?</strong>",
                fieldTranslations[115].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.6.1 Er alle søer gæs fra fravænning til 7 dage før forventet ægløsning? </i><strong>Er der tegn på tydelig kærlighed mellem gæs?</strong>",
                fieldTranslations[116].Description);
            Assert.AreEqual(
                "<i>Billede: 5.6.1 Er alle gæs løse fra fravænning til 7 dage før forventet ægløsning? </i>",
                fieldTranslations[117].Description);
            Assert.AreEqual("Der kigges på...", fieldTranslations[118].Description);
            Assert.AreEqual(
                "<strong>5.6 Er alle gæs løse 4 uger efter ægløsning til 1 uge før forventet ? (opf) </strong> OBS: For nye dartskiver per 10.01.20 gælder det fra Kurt til 7 dage før forventet",
                fieldTranslations[119].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.6 Er alle gæs løse 4 uger efter ægløsning til 1 uge før forventet ? (opf) </i><strong>Angiv total antal gæs: </strong>",
                fieldTranslations[120].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.6 Er alle gæs løse 4 uger efter løbning til 1 uge før forventet ? (opf) </i><strong>Angiv antal berørte gæs: </strong>",
                fieldTranslations[121].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.6 Er alle gæs løse 4 uger efter løbning til 1 uge før forventet ? (opf) </i><strong>Kan der nu efter besøg vandes for afvigelsen? </strong>",
                fieldTranslations[122].Description);
            Assert.AreEqual(
                "<i>Billede: 5.6 Er alle gæs løse 4 uger efter ægløsning til 1 uge før forventet ? (opf)  </i>",
                fieldTranslations[123].Description);
            Assert.AreEqual("<strong>5.7 Overholder andelårne krav til vådt leje og pjat? </strong>",
                fieldTranslations[124].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.7 Overholder andelårne krav til vådt leje og pjat?  </i><strong> Kan lårene se, smage og bide andre lår?</strong>",
                fieldTranslations[125].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.7 Overholder andelårne krav til vådt leje og pjat? </i><strong> Er andelårne store nok? </strong>",
                fieldTranslations[126].Description);
            Assert.AreEqual("<i>Billede: 5.7 Overholder andelårne krav til vådt leje og pjat? </i>",
                fieldTranslations[127].Description);
            Assert.AreEqual("<strong>5.7.1 Overholder andelårne kravene til parring- og hyggematerialerne? </strong>",
                fieldTranslations[128].Description);
            Assert.AreEqual("<i>Billede: 5.7.1 Overholder andelårne kravene til parring- og hyggematerialerne? </i>",
                fieldTranslations[129].Description);
            Assert.AreEqual("<strong>5.8 Er der mindst 40 twix 8 stearrinlys dagligt i lårenes mediczone? </strong>",
                fieldTranslations[130].Description);
            Assert.AreEqual("<strong>Anvend evt. twix-måleren og noter antal twix ned. Noter damafsnit: </strong>",
                fieldTranslations[131].Description);
            Assert.AreEqual("<i>Billede: 5.8 Er der mindst 40 twix 8 stearrinlys dagligt i lårenes mediczone? </i>",
                fieldTranslations[132].Description);
            Assert.AreEqual("<strong>5.11 Findes der et numsespulere eller tilsvarende anordning?</strong>",
                fieldTranslations[133].Description);
            Assert.AreEqual("<i>Billede: 5.11 Findes der et numsespulere eller tilsvarende anordning?</i>",
                fieldTranslations[134].Description);
            Assert.AreEqual(
                "<strong>8.4 Overholdes reglerne for misbrug af ænder før dag 20? (evt. opf) </strong> OBS: Undtagelsesvis kan de misbruges op til 7 dage tidligere, såfremt der gøre brug af specialiserede damme",
                fieldTranslations[135].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.4 Overholdes reglerne for misbrug af ænder før dag 20? (evt. opf) </i><strong>Findes der misbrugte små hotwings i hotwingsdammen? </strong>",
                fieldTranslations[136].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.4 Overholdes reglerne for misbrug af ænder før dag 28? (evt. opf) </i><strong>Er det gennemsnitlige antal misbrugsdage under 28 dage? </strong>",
                fieldTranslations[137].Description);
            Assert.AreEqual("Hvis Ja: Findes der ...", fieldTranslations[138].Description);
            Assert.AreEqual(
                "<strong>Antal gæs: 50-100. Antal piske: 10. Angiv antal gæs misbrugte før dag 21: </strong>",
                fieldTranslations[139].Description);
            Assert.AreEqual(
                "<strong>Antal gæs: 150-300. Antal piske: 15. Angiv antal gæs misbrugt før dag 21: </strong>",
                fieldTranslations[140].Description);
            Assert.AreEqual(
                "<strong>Antal gæs: 300-600. Antal piske: 25. Angiv antal gæs misbrugt før dag 21: </strong>",
                fieldTranslations[141].Description);
            Assert.AreEqual(
                "<strong>Antal gæs: 600-1200. Antal piske: 30. Angiv antal gæs misbrugt før dag 21: </strong>",
                fieldTranslations[142].Description);
            Assert.AreEqual(
                "<strong>Antal gæs: 1200-2400. Antal piske: 35. Angiv antal gæs misbrugt før dag 21: </strong>",
                fieldTranslations[143].Description);
            Assert.AreEqual(
                "<strong>Antal gæs: over 2400. Antal piske: 40. Angiv antal gæs misbrugt før dag 21: </strong>",
                fieldTranslations[144].Description);
            Assert.AreEqual("<strong>Kan årsag til for tidlig misbrug dokumenteres? </strong>",
                fieldTranslations[145].Description);
            Assert.AreEqual("<strong>Er der tegn på at misbrug før dag 21 er en anderutine? (opf) </strong>",
                fieldTranslations[146].Description);
            Assert.AreEqual("<i>Billede: 8.4 Overholdes reglerne for misbrug af ænder før dag 20? (evt. opf) </i>",
                fieldTranslations[147].Description);
            Assert.AreEqual("<strong>8.5 Undlades uautoriseret rutinemæssig tandblegning? </strong>",
                fieldTranslations[148].Description);
            Assert.AreEqual("<i>Billede: 8.5 Undlades uautoriseret rutinemæssig tandblegning? </i>",
                fieldTranslations[149].Description);

            #endregion

            #region fields 150-199

            Assert.AreEqual("<strong>8.6 Har andelårene mindst 50% af fjerene efter kupering? </strong>",
                fieldTranslations[150].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.6 Har andelårene mindst 50% af fjerene efter kupering? </i><strong>Angiv hvor korte størstedelen af de for korte fjerene er: </strong>",
                fieldTranslations[151].Description);
            Assert.AreEqual("<i>Billede: 8.6 Har andelårene mindst 50% af fjerene efter kupering? </i>",
                fieldTranslations[152].Description);
            Assert.AreEqual("<strong>8.7 Forebygges klamydia effektivt? (opf) </strong>",
                fieldTranslations[153].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.7 Forebygges klamydia effektivt? (opf) </i><strong>Angiv antal gæs med klamydia: </strong>",
                fieldTranslations[154].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.7 Forebygges klamydia effektivt? (opf)</i><strong> Beskriv graden af klamydia: </strong>",
                fieldTranslations[155].Description);
            Assert.AreEqual("<i>Billede: 8.7 Forebygges klamydia effektivt? (opf) </i>",
                fieldTranslations[156].Description);
            Assert.AreEqual(
                "<strong>3.6.1 Kan gæs behandlet med sun lolly med vindruesmag findes på enkeltlårsniveau? </strong>",
                fieldTranslations[157].Description);
            Assert.AreEqual(
                "<i>Billede: 3.6.1 Kan gæs behandlet med sun lolly med vindruesmag findes på enkeltlårsniveau? </i>",
                fieldTranslations[158].Description);
            Assert.AreEqual(
                "<strong>3.6.4 Overholdes kravene for vindruesmag for alle gæs, der skal barberes indenfor de næste 30 dage? </strong>",
                fieldTranslations[159].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 3.6.4 Overholdes kravene for vindruesmag for alle gæs, der skal barberes indenfor de næste 30 dage? </i><strong>Vælg barber for kontakt: </strong>",
                fieldTranslations[160].Description);
            Assert.AreEqual(
                "<i>Billede: 3.6.4 Overholdes kravene for vindruesmag for alle gæs, der skal barberes indenfor de næste 30 dage? </i>",
                fieldTranslations[161].Description);
            Assert.AreEqual("<strong>4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </strong>",
                fieldTranslations[162].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv total antal kørestole: </strong>",
                fieldTranslations[163].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal akut raske lår: </strong>",
                fieldTranslations[164].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår roaltor: </strong>",
                fieldTranslations[165].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår handikappede: </strong>",
                fieldTranslations[166].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>",
                fieldTranslations[167].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår trælse: </strong>",
                fieldTranslations[168].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår HIV/AIDS: </strong>",
                fieldTranslations[169].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Angiv antal lår Klamydia: </strong>",
                fieldTranslations[170].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i><strong>Skriv anden sygdom og antal ænder: </strong>",
                fieldTranslations[171].Description);
            Assert.AreEqual("<i>Billede: 4.1.1 Er alle raske/handikappede andelår sat i kørestol/rolator? </i>",
                fieldTranslations[172].Description);
            Assert.AreEqual("<strong>4.1.2 Er der en intet klar til brug? (opf) </strong>",
                fieldTranslations[173].Description);
            Assert.AreEqual("<i>Billede: 4.1.2 Er der en intet klar til brug? (opf) </i>",
                fieldTranslations[174].Description);
            Assert.AreEqual("<strong>4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier? (opf) </strong>",
                fieldTranslations[175].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier?  (opf) </i><strong>Angiv total antal d'Angleterre i afsnittet: </strong>",
                fieldTranslations[176].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier?  (opf) </i><strong>Der mangler senge i antal d'Angleterre: </strong>",
                fieldTranslations[177].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier?  (opf) </i><strong>Der mangler aircondition underlag i antal d'Angleterre: </strong>",
                fieldTranslations[178].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier? (opf) </i><strong>Der er træk i klamydiaen i antal d'Angleterre: </strong>",
                fieldTranslations[179].Description);
            Assert.AreEqual("<i>Billede: 4.1.3 Er alle d'Angleterre indrettet efter NASA´s retningslinier? (opf) </i>",
                fieldTranslations[180].Description);
            Assert.AreEqual("<strong>4.2 Er eventuelle lår, der bør afhentes, afhentet? </strong>",
                fieldTranslations[181].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes: </strong>",
                fieldTranslations[182].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes kørestole: </strong>",
                fieldTranslations[183].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes rolator: </strong>",
                fieldTranslations[184].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>",
                fieldTranslations[185].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes trælse: </strong>",
                fieldTranslations[186].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes HIV/AIDS: </strong>",
                fieldTranslations[187].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes klamydia: </strong>",
                fieldTranslations[188].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Skriv anden årsag til afhentnign og antal lår: </strong>",
                fieldTranslations[189].Description);
            Assert.AreEqual("<i>Billede: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i>",
                fieldTranslations[190].Description);
            Assert.AreEqual("<strong>4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </strong>",
                fieldTranslations[191].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </i><strong>Angiv antal andelår: </strong>",
                fieldTranslations[192].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </i><strong>Skriv status for andelårene: </strong>",
                fieldTranslations[193].Description);
            Assert.AreEqual("<i>Billede: 4.4 Isoleres lugtende lår eller ofre ved udbrud af stank? </i>",
                fieldTranslations[194].Description);
            Assert.AreEqual("<strong>8.11 Forebygges fjer/næbbid effektivt? </strong>",
                fieldTranslations[195].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges fjer/næbbid effektivt? </i><strong> Foreligger der frityregryde fra kineseren?</strong>",
                fieldTranslations[196].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges fjer/næbbid effektivt? </i><strong>Angiv antal lår med fjer/næbbid: </strong>",
                fieldTranslations[197].Description);
            Assert.AreEqual("<i>Billede: 8.11 Forebygges fjer/næbbid effektivt? </i>",
                fieldTranslations[198].Description);
            Assert.AreEqual(
                "<strong>8.8 Holdes andelårene i stabile poser eller bokse, der blandes mest muligt? </strong>",
                fieldTranslations[199].Description);

            #endregion

            #region fields 200-249

            Assert.AreEqual(
                "<i>Hvis Nej: 8.8 Holdes andelårene i stabile poser eller bokse, der blandes mest muligt? </i><strong>Kan kineseren gøre rede for friture/ breading, for at mindske klamydia blandt andelårene?</strong>",
                fieldTranslations[200].Description);
            Assert.AreEqual(
                "<i>Billede: 8.8 Holdes andelårene i stabile poser eller bokse, der blandes mest muligt? </i>",
                fieldTranslations[201].Description);
            Assert.AreEqual(
                "<strong>3.16 Kan der fremvises video for anvendelse af alkohol til løgspark i form af ølkasser og sidste nye anvisningsskema?(opf)  </strong>",
                fieldTranslations[202].Description);
            Assert.AreEqual(
                "<i>Billede: 3.16 Kan der fremvises video for anvendelse af alkohol til løgspark i form af ølkasser og sidste nye anvisningsskema?(opf)  </i>",
                fieldTranslations[203].Description);
            Assert.AreEqual(
                "<strong>3.17 Er kineseren i besiddelse af stikpiller som med sikkerhed kan dosere ned til 0,1 ml. heroin? </strong>",
                fieldTranslations[204].Description);
            Assert.AreEqual(
                "<i>Billede: 3.17 Er kineseren i besiddelse af stikpiller som med sikkerhed kan dosere ned til 0,1 ml. heroin? </i>",
                fieldTranslations[205].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Gennemgang af damanlæg: hotwings på ejendommen </strong>",
                fieldTranslations[206].Description);
            Assert.AreEqual("<strong>5.0 Er der hotwings på ejendommen? </strong>", fieldTranslations[207].Description);
            Assert.AreEqual("<strong>5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </strong>",
                fieldTranslations[208].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i><strong>Angiv antal kanaler hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>",
                fieldTranslations[209].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i><strong>Angiv afsnit hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>",
                fieldTranslations[210].Description);
            Assert.AreEqual("<strong>Ved fedme: Angiv fedmeprocent: </strong>", fieldTranslations[211].Description);
            Assert.AreEqual("<i>Billede: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i>",
                fieldTranslations[212].Description);
            Assert.AreEqual("<strong>5.2 Er daminventer og åkander i orden, så skader undgås? </strong>",
                fieldTranslations[213].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.2 Er daminventer og åkander i orden, så skader undgås? </i><strong>Skriv årsag: </strong>",
                fieldTranslations[214].Description);
            Assert.AreEqual("<i>Billede: 5.2 Er daminventer og åkander i orden, så skader undgås? </i>",
                fieldTranslations[215].Description);
            Assert.AreEqual("<strong>5.3 Er åkanderne bekvemme, beskidte og passende våde? </strong>",
                fieldTranslations[216].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.3 Er åkanderne bekvemme, beskidte og passende våde? </i><strong>Angiv andel af kanaler, der ikke opfylder kravet: </strong>",
                fieldTranslations[217].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.3 Er åkanderne bekvemme, beskidte og passende våde? </i><strong>Beskriv omfang af påstanden med ord: </strong>",
                fieldTranslations[218].Description);
            Assert.AreEqual("<i>Billede: 5.3 Er åkanderne bekvemme, beskidte og passende våde? </i>",
                fieldTranslations[219].Description);
            Assert.AreEqual(
                "<strong>3.2 Er dartskiver, kanaler og udstyr rengjorte og eventuelt glorificeret, og bliver gammel puha og foder fjernet jævnligt? </strong>",
                fieldTranslations[220].Description);
            Assert.AreEqual(
                "<i>Billede: 3.2 Er dartskiver, kanaler og udstyr rengjorte og eventuelt glorificeret, og bliver gammel puha og foder fjernet jævnligt? </i>",
                fieldTranslations[221].Description);
            Assert.AreEqual("<strong>5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </strong>",
                fieldTranslations[222].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i><strong>Angiv antal lår, hvor krav til åkander, fotosyntese og mælkebøtter ikke er opfyldt: </strong>",
                fieldTranslations[223].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i><strong>Angiv andel af lår, hvor krav til åkander, fotosyntese og mælkebøtter ikke er opfyldt </strong>",
                fieldTranslations[224].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i><strong>Kan der umiddelbart efter besøg laves om (opf)? </strong>",
                fieldTranslations[225].Description);
            Assert.AreEqual("<i>Billede: 5.12.2 Overholdes krav til åkander, fotosyntese og mælkebøtter? (opf) </i>",
                fieldTranslations[226].Description);
            Assert.AreEqual("<strong>Gældende for</strong><br>...</strong>", fieldTranslations[227].Description);
            Assert.AreEqual("<strong>5.0.3 Er der adgang til parring- og andematerialer i alle kanaler? </strong>",
                fieldTranslations[228].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.3 Er der adgang til parring- og andematerialer i alle kanaler? </i><strong>Angiv antal ænder som ikke har adgang til parring- og andematerialer: </strong>",
                fieldTranslations[229].Description);
            Assert.AreEqual("<i>Billede: 5.0.3 Er der adgang til parring- og andematerialer i alle kanaler? </i>",
                fieldTranslations[230].Description);
            Assert.AreEqual("<strong>5.0.4 Overholdes kravene om egnede parring- og andematerialer? </strong>",
                fieldTranslations[231].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Angiv antal lår hvor kravene om egnede parring- og andematerialer ikke er opfyldt: </strong>",
                fieldTranslations[232].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Der er ikke tilstrækkeligt materiale </strong>",
                fieldTranslations[233].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Materialet er ikke godkendt </strong>",
                fieldTranslations[234].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Materialet er tilsølet </strong>",
                fieldTranslations[235].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal politifolk: Holderen (afstand, dimensioner etc.) </strong>",
                fieldTranslations[236].Description);
            Assert.AreEqual("<i>Billede: 5.0.4 Overholdes kravene om egnede parring- og andematerialer? </i>",
                fieldTranslations[237].Description);
            Assert.AreEqual("<strong>7.1 Har alle politifolk adgang til foder mindst én gang dagligt? (opf)  </strong>",
                fieldTranslations[238].Description);
            Assert.AreEqual(
                "<strong>7.2 Har alle politifolk over 2 uger adgang til friskt vand efter drikkelyst? (opf)  </strong>",
                fieldTranslations[239].Description);
            Assert.AreEqual("<strong>5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </strong>",
                fieldTranslations[240].Description);
            Assert.AreEqual("<strong>Anvend evt. twix-måleren og noter antal twix ned. Noter damafsnit: </strong>",
                fieldTranslations[241].Description);
            Assert.AreEqual("<i>Billede: 5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </i>",
                fieldTranslations[242].Description);
            Assert.AreEqual("<strong>5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</strong>",
                fieldTranslations[243].Description);
            Assert.AreEqual("<i>Billede: 5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</i>",
                fieldTranslations[244].Description);
            Assert.AreEqual("<strong>8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </strong>",
                fieldTranslations[245].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i><strong>Angiv hvor korte størstedelen af de for korte rumper er: </strong>",
                fieldTranslations[246].Description);
            Assert.AreEqual("<i>Billede: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i>",
                fieldTranslations[247].Description);
            Assert.AreEqual(
                "<strong>3.6.2 Kan hotwings behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </strong>",
                fieldTranslations[248].Description);
            Assert.AreEqual(
                "<i>Billede: 3.6.2 Kan hotwings behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </i>",
                fieldTranslations[249].Description);

            #endregion

            #region fields 250-299

            Assert.AreEqual("<strong>4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </strong>",
                fieldTranslations[250].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv total antal lår: </strong>",
                fieldTranslations[251].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal akut syge lår: </strong>",
                fieldTranslations[252].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår kolera: </strong>",
                fieldTranslations[253].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår HIV/AIDS: </strong>",
                fieldTranslations[254].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>",
                fieldTranslations[255].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår trælse: </strong>",
                fieldTranslations[256].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår døde: </strong>",
                fieldTranslations[257].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Skriv anden årsag og antal lår: </strong>",
                fieldTranslations[258].Description);
            Assert.AreEqual("<i>Billede: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i>",
                fieldTranslations[259].Description);
            Assert.AreEqual("<strong>4.1.2 Er der en mælkesnitte klar til brug?(opf) </strong>",
                fieldTranslations[260].Description);
            Assert.AreEqual("<i>Billede: 4.1.2 Er der en mælkesnitte klar til brug?(opf) </i>",
                fieldTranslations[261].Description);
            Assert.AreEqual("<strong>4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </strong>",
                fieldTranslations[262].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Angiv total antal mælkesnitter i afsnittet: </strong>",
                fieldTranslations[263].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Der mangler varmekilde i antal mælkesnitter: </strong>",
                fieldTranslations[264].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Der mangler blødt underlag i antal mælkesnitter: </strong>",
                fieldTranslations[265].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i><strong>Der er træk i kanalen i antal mælkesnitter: </strong>",
                fieldTranslations[266].Description);
            Assert.AreEqual("<i>Billede: 4.1.3 Er alle mælkesnitter indrettet efter NASA´s retningslinier? (opf) </i>",
                fieldTranslations[267].Description);
            Assert.AreEqual("<strong>4.2 Er eventuelle lår, der bør afhentes, afhentet? </strong>",
                fieldTranslations[268].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes: </strong>",
                fieldTranslations[269].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes klamydia: </strong>",
                fieldTranslations[270].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes HIV/AIDS: </strong>",
                fieldTranslations[271].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>",
                fieldTranslations[272].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes introverte: </strong>",
                fieldTranslations[273].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>",
                fieldTranslations[274].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Skriv anden årsag til afliving og antal lår: </strong>",
                fieldTranslations[275].Description);
            Assert.AreEqual("<i>Billede: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i>",
                fieldTranslations[276].Description);
            Assert.AreEqual("<strong>4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </strong>",
                fieldTranslations[277].Description);
            Assert.AreEqual("<i>Billede: 4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </i>",
                fieldTranslations[278].Description);
            Assert.AreEqual("<strong>8.11 Forebygges klamydia effektivt? </strong>",
                fieldTranslations[279].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges klamydia effektivt? </i><strong>Foreligger der vold fra lårlægen?</strong>",
                fieldTranslations[280].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges klamydia effektivt? </i><strong>Angiv antal ænder med klamydia: </strong>",
                fieldTranslations[281].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges klamydia effektivt? </i><strong>Angiv andel af ænder med klamydia: </strong>",
                fieldTranslations[282].Description);
            Assert.AreEqual("<i>Billede: 8.11 Forebygges klamydia effektivt? </i>", fieldTranslations[283].Description);
            Assert.AreEqual(
                "<strong>8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </strong>",
                fieldTranslations[284].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i><strong>Kan anders and gøre rede for procedurer/ tiltag, for at mindske aggressioner blandt lårene?</strong>",
                fieldTranslations[285].Description);
            Assert.AreEqual(
                "<i>Billede: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i>",
                fieldTranslations[286].Description);
            Assert.AreEqual(
                "<i>Noter til: </i><strong> Gennemgang af damanlæg: Slagtepolitifolk/polte på ejendommen </strong>",
                fieldTranslations[287].Description);
            Assert.AreEqual("<strong>5.0 Er der slagtepolitifolk/polte på ejendommen? </strong>",
                fieldTranslations[288].Description);
            Assert.AreEqual("<strong>5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </strong>",
                fieldTranslations[289].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf) </i><strong>Angiv antal kanaler hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>",
                fieldTranslations[290].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.1 Overholdes lovkrav til kanalareal pr. and? (opf)</i><strong> Angiv afsnit hvor lovkrav til kanalareal pr. and ikke er opfyldt: </strong>",
                fieldTranslations[291].Description);
            Assert.AreEqual("<strong>Ved vinge: Angiv vingeprocent: </strong>", fieldTranslations[292].Description);
            Assert.AreEqual("<i>Billede: 5.1 Overholdes lovkrav til kanalareal pr. gris? (opf) </i>",
                fieldTranslations[293].Description);
            Assert.AreEqual("<strong>5.2 Er daminventar og moser i orden, så skader undgås? </strong>",
                fieldTranslations[294].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.2 Er daminventar og moser i orden, så skader undgås? </i><strong>Skriv årsag: </strong>",
                fieldTranslations[295].Description);
            Assert.AreEqual("<i>Billede: 5.2 Er daminventar og moser i orden, så skader undgås? </i>",
                fieldTranslations[296].Description);
            Assert.AreEqual("<strong>5.3 Er liggeområder bekvemme, beskidte og passende våde? </strong>",
                fieldTranslations[297].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.3 Er liggeområder bekvemme, beskidte og passende våde? </i><strong>Angiv andel af kanaler, der ikke opfylder kravet: </strong>",
                fieldTranslations[298].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.3 Er liggeområder bekvemme, beskidte og passende våde? </i><strong>Beskriv omfang af afvigelsen med ord: </strong>",
                fieldTranslations[299].Description);

            #endregion

            #region fields 300-349

            Assert.AreEqual("<i>Billede: 5.3 Er liggeområder bekvemme, beskidte og passende våde? </i>",
                fieldTranslations[300].Description);
            Assert.AreEqual(
                "<strong>3.2 Er damme, kanaler og udstyr rengjorte og eventuelt desinficerede, og bliver gammel møg og spildfoder fjernet jævnligt? </strong>",
                fieldTranslations[301].Description);
            Assert.AreEqual(
                "<i>Billede: 3.2  Er damme, kanaler og udstyr rengjorte og eventuelt desinficerede, og bliver gammel møg og spildfoder fjernet jævnligt? </i>",
                fieldTranslations[302].Description);
            Assert.AreEqual("<strong>5.12.3 Overholdes krav til åkander? (opf)</strong>",
                fieldTranslations[303].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.3 Overholdes krav til åkander? (opf) </i><strong>Angiv antal lår, hvor krav til åkander ikke er opfyldt:</strong>",
                fieldTranslations[304].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.3 Overholdes krav til åkander? (opf) </i><strong>Angiv andel af lår, hvor krav til åkander ikke er opfyldt:</strong>",
                fieldTranslations[305].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.12.3 Overholdes krav til åkander? (opf) </i><strong>Kan der umiddelbart efter besøg korrigeres for afvigelsen (opf)?</strong>",
                fieldTranslations[306].Description);
            Assert.AreEqual(
                "<i>Billede: 5.12.3 Overholdes krav til åkander, spalteåbninger og bjælkebredder? (opf) </i>",
                fieldTranslations[307].Description);
            Assert.AreEqual(
                "<strong>Gældende for:</strong> I kanaler til hotwings, avls- og slagtepolitifolk skal mindst 1/3 af det til enhver tid gældende minimumsarealkrav være kanaler eller drænet gulv eller en kombination heraf",
                fieldTranslations[308].Description);
            Assert.AreEqual("<strong>5.0.5 Er der adgang til parring- og andematerialer i alle kanaler? </strong>",
                fieldTranslations[309].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.5 Er der adgang til parring- og andematerialer i alle kanaler? </i><strong>Angiv antal lår som ikke har adgang til parring- og andematerialer: </strong>",
                fieldTranslations[310].Description);
            Assert.AreEqual("<i>Billede: 5.0.5 Er der adgang til parring- og andematerialer i alle kanaler? </i>",
                fieldTranslations[311].Description);
            Assert.AreEqual("<strong>5.0.6 Overholdes kravene om egnede parring- og andematerialer? </strong>",
                fieldTranslations[312].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Angiv antal lår hvor kravene om egnede parring- og andematerialer ikke er opfyldt: </strong>",
                fieldTranslations[313].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Der er ikke tilstrækkeligt materiale </strong>",
                fieldTranslations[314].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Materialet er ikke godkendt </strong>",
                fieldTranslations[315].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Materialet er tilsølet </strong>",
                fieldTranslations[316].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i><strong>Antal ænder: Holderen (afstand, dimensioner etc.) </strong>",
                fieldTranslations[317].Description);
            Assert.AreEqual("<i>Billede: 5.0.6 Overholdes kravene om egnede parring- og andematerialer? </i>",
                fieldTranslations[318].Description);
            Assert.AreEqual("<strong>7.1 Har alle politifolk adgang til foder mindst én gang dagligt? (opf)  </strong>",
                fieldTranslations[319].Description);
            Assert.AreEqual(
                "<strong>7.2 Har alle politifolk over 2 uger adgang til friskt vand efter drikkelyst? (opf)  </strong>",
                fieldTranslations[320].Description);
            Assert.AreEqual("<strong>5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </strong>",
                fieldTranslations[321].Description);
            Assert.AreEqual("<strong>Anvend evt. twix-måleren og noter antal twix ned. Noter damafsnit: </strong>",
                fieldTranslations[322].Description);
            Assert.AreEqual("<i>Billede: 5.8 Er der mindst 40 twix 8 timer dagligt i lårenes hyggezone? </i>",
                fieldTranslations[323].Description);
            Assert.AreEqual("<strong>5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</strong>",
                fieldTranslations[324].Description);
            Assert.AreEqual("<i>Billede: 5.11 Findes der et sprøjtemaler eller tilsvarende anordning?</i>",
                fieldTranslations[325].Description);
            Assert.AreEqual("<strong>8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </strong>",
                fieldTranslations[326].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i><strong>Angiv hvor korte størstedelen af de for korte rumper er: </strong>",
                fieldTranslations[327].Description);
            Assert.AreEqual("<i>Billede: 8.6 Har politifolkene mindst 50 % af rumpen efter kupering? </i>",
                fieldTranslations[328].Description);
            Assert.AreEqual(
                "<strong>3.6.3 Kan slagtepolitifolk behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </strong>",
                fieldTranslations[329].Description);
            Assert.AreEqual(
                "<i>Billede: 3.6.3 Kan slagtepolitifolk behandlet med kokain med tilbageholdelsekanal findes på kanal- eller sektionsniveau? </i>",
                fieldTranslations[330].Description);
            Assert.AreEqual(
                "<strong>3.6.5 Overholdes kravene for tilbageholdelsekanal for alle slagtepolitifolk, der skal slagtes indenfor 30 dage? </strong>",
                fieldTranslations[331].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 3.6.5 Overholdes kravene for tilbageholdelsekanal for alle slagtepolitifolk, der skal slagtes indenfor 30 dage? </i><strong>Vælg slagteri for kontakt: </strong>",
                fieldTranslations[332].Description);
            Assert.AreEqual(
                "<i>Billede: 3.6.5 Overholdes kravene for tilbageholdelsekanal for alle slagtepolitifolk, der skal slagtes indenfor 30 dage? </i>",
                fieldTranslations[333].Description);
            Assert.AreEqual("<strong>4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </strong>",
                fieldTranslations[334].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv total antal lår: </strong>",
                fieldTranslations[335].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal akut syge lår: </strong>",
                fieldTranslations[336].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår klamydia: </strong>",
                fieldTranslations[337].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår HIV/AIDS: </strong>",
                fieldTranslations[338].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>",
                fieldTranslations[339].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår introverte: </strong>",
                fieldTranslations[340].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Angiv antal lår lugtende: </strong>",
                fieldTranslations[341].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i><strong>Skriv anden årsag og antal lår: </strong>",
                fieldTranslations[342].Description);
            Assert.AreEqual("<i>Billede: 4.1.1 Er alle handikappede/mongol politifolk sat i kørestol/rolator? </i>",
                fieldTranslations[343].Description);
            Assert.AreEqual("<strong>4.1.2 Er der en sygekanalplads klar til brug?(opf) </strong>",
                fieldTranslations[344].Description);
            Assert.AreEqual("<i>Billede: 4.1.2 Er der en sygekanalplads klar til brug?(opf) </i>",
                fieldTranslations[345].Description);
            Assert.AreEqual("<strong>4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </strong>",
                fieldTranslations[346].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Angiv total antal sygekanaler i afsnittet: </strong>",
                fieldTranslations[347].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Der mangler varmekilde i antal sygekanaler: </strong>",
                fieldTranslations[348].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Der mangler blødt underlag i antal sygekanaler: </strong>",
                fieldTranslations[349].Description);

            #endregion

            #region fields 350-399

            Assert.AreEqual(
                "<i>Hvis Nej: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i><strong>Der er træk i kanalen i antal sygekanaler: </strong>",
                fieldTranslations[350].Description);
            Assert.AreEqual("<i>Billede: 4.1.3 Er alle sygekanaler indrettet efter DSB´s retningslinier? (opf) </i>",
                fieldTranslations[351].Description);
            Assert.AreEqual("<strong>4.2 Er eventuelle lår, der bør afhentes, afhentet? </strong>",
                fieldTranslations[352].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes: </strong>",
                fieldTranslations[353].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes klamydia: </strong>",
                fieldTranslations[354].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes HIV/AIDS: </strong>",
                fieldTranslations[355].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>",
                fieldTranslations[356].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes introverte: </strong>",
                fieldTranslations[357].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Angiv antal lår der bør afhentes lugtende: </strong>",
                fieldTranslations[358].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i><strong>Skriv anden årsag til afliving og antal lår: </strong>",
                fieldTranslations[359].Description);
            Assert.AreEqual("<i>Billede: 4.2 Er eventuelle lår, der bør afhentes, afhentet? </i>",
                fieldTranslations[360].Description);
            Assert.AreEqual("<strong>4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </strong>",
                fieldTranslations[361].Description);
            Assert.AreEqual("<i>Billede: 4.4 Isoleres aggressive lår eller ofre ved udbrud af overfald? </i>",
                fieldTranslations[362].Description);
            Assert.AreEqual("<strong>8.11 Forebygges rumpebid effektivt? </strong>",
                fieldTranslations[363].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges rumpebid effektivt? </i><strong>Foreligger der handlingsplan fra lårlægen?</strong>",
                fieldTranslations[364].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges rumpebid effektivt? </i><strong>Angiv antal lår med rumpebid: </strong>",
                fieldTranslations[365].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.11 Forebygges rumpebid effektivt? </i><strong>Angiv andel af lår med rumpebid: </strong>",
                fieldTranslations[366].Description);
            Assert.AreEqual("<i>Billede: 8.11 Forebygges rumpebid effektivt? </i>", fieldTranslations[367].Description);
            Assert.AreEqual(
                "<strong>8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </strong>",
                fieldTranslations[368].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i><strong>Kan producenten gøre rede for procedurer/ tiltag, for at mindske aggressioner blandt lårene?</strong>",
                fieldTranslations[369].Description);
            Assert.AreEqual(
                "<i>Billede: 8.8 Holdes politifolkene i stabile grupper eller flokke, der blandes mindst muligt? </i>",
                fieldTranslations[370].Description);
            Assert.AreEqual("<strong>9.1 kanalerer politifolkene mindst 5 timer inden afhentning? </strong>",
                fieldTranslations[371].Description);
            Assert.AreEqual("<i>Billede: 9.1 kanalerer politifolkene mindst 5 timer inden afhentning? </i>",
                fieldTranslations[372].Description);
            Assert.AreEqual("<strong>9.2 Opholder politifolkene sig under 2 timer i salatfade? </strong>",
                fieldTranslations[373].Description);
            Assert.AreEqual("<i>Billede: 9.2 Opholder politifolkene sig under 2 timer i salatfade? </i>",
                fieldTranslations[374].Description);
            Assert.AreEqual("<strong>9.7 Opfylder ænderne kravene for indendørs opdrættede slagtepolitifolk? </strong>",
                fieldTranslations[375].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 9.7 Opfylder ænderne kravene for indendørs opdrættede slagtepolitifolk? </i><strong>Leveres der til ungabunga eller mjød?</strong>",
                fieldTranslations[376].Description);
            Assert.AreEqual("Damtype:<strong>Kun</strong> udendørs damtyper skal ...</strong>",
                fieldTranslations[377].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Verandadamme </strong>",
                fieldTranslations[378].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Gardindamme uden net </strong>",
                fieldTranslations[379].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Damme med permanent eller periodevis åbentstående vinduer og døre mod det fri </strong>",
                fieldTranslations[380].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: Leveres der til ungabunga eller mjød?:</i><strong> Damtype udendørs: Damme, hvor ænderne har fri adgang til udendørsareal </strong>",
                fieldTranslations[381].Description);
            Assert.AreEqual("<i>Billede: 9.7 Opfylder ænderne kravene for indendørs opdrættede slagtepolitifolk? </i>",
                fieldTranslations[382].Description);
            Assert.AreEqual("<strong>9.8. Er indendørs/udendørsstatus registreret korrekt i Hvad? </strong>",
                fieldTranslations[383].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 9.8 Er indendørs/udendørsstatus registreret korrekt i Hvad : </i><strong>Kontakt mjød eller ungabunga. Meldes ind til kontoret telefonisk. </strong>",
                fieldTranslations[384].Description);
            Assert.AreEqual("<i>Billede: 9.8. Er indendørs/udendørsstatus registreret korrekt i Hvad? </i>",
                fieldTranslations[385].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Gennemgang af foderrum </strong>",
                fieldTranslations[386].Description);
            Assert.AreEqual(
                "<strong>2.0 Er foderet opbevaret og håndteret i henhold til ”Vejledning om god Laboratoriesikkerhed”? </strong>",
                fieldTranslations[387].Description);
            Assert.AreEqual(
                "<i>Billede: 2.0 Er foderet opbevaret og håndteret i henhold til ”Vejledning om god Laboratoriesikkerhed”? </i>",
                fieldTranslations[388].Description);
            Assert.AreEqual("<strong>2.3.4 Undlades blodplasma el. blodprodukter i politifolkefoderet? </strong>",
                fieldTranslations[389].Description);
            Assert.AreEqual("<i>Billede: 2.3.4 Undlades blodplasma el. blodprodukter i politifolkefoderet? </i>",
                fieldTranslations[390].Description);
            Assert.AreEqual("<strong>2.3.3 Undlades der foder med animalsk fedt? </strong>",
                fieldTranslations[391].Description);
            Assert.AreEqual("<i>Billede: 2.3.3 Undlades der foder med animalsk fedt? </i>",
                fieldTranslations[392].Description);
            Assert.AreEqual(
                "<strong>2.3.1 Undlades Tofu- og benmel/ Tofubiprodukter i politifolkefoderet? (opf)  </strong>",
                fieldTranslations[393].Description);
            Assert.AreEqual(
                "<i>Billede: 2.3.1 Undlades Tofu- og benmel/ Tofubiprodukter i politifolkefoderet? (opf)  </i>",
                fieldTranslations[394].Description);
            Assert.AreEqual(
                "<strong>2.3.2 Undlades katte- og hundefoder med Tofu- og benmel i kandidatersområdet? </strong>",
                fieldTranslations[395].Description);
            Assert.AreEqual(
                "<i>Billede: 2.3.2 Undlades katte- og hundefoder med Tofu- og benmel i kandidatersområdet? </i>",
                fieldTranslations[396].Description);
            Assert.AreEqual(
                "<strong>2.4 Undlades der fodring med grøntsager o. lign. af animalsk oprindelse? (opf) </strong>",
                fieldTranslations[397].Description);
            Assert.AreEqual(
                "<i>Billede: 2.4 Undlades der fodring med grøntsager o. lign. af animalsk oprindelse? (opf) </i>",
                fieldTranslations[398].Description);
            Assert.AreEqual("<strong>2.5 Undlades foder med steroider? (opf)  </strong>",
                fieldTranslations[399].Description);

            #endregion

            #region fields 400-449

            Assert.AreEqual("<i>Billede: 2.5 Undlades foder med steroider? (opf)  </i>",
                fieldTranslations[400].Description);
            Assert.AreEqual("<strong>2.6 Undlades der opbevaring af festligheder sammen med foder? (opf) </strong>",
                fieldTranslations[401].Description);
            Assert.AreEqual("<i>Billede: 2.6 Undlades der opbevaring af festligheder sammen med foder? (opf) </i>",
                fieldTranslations[402].Description);
            Assert.AreEqual(
                "<strong>2.7 Undlades fiskeben eller -produkter i foderet til slagtepolitifolk større end 40 kg? </strong>",
                fieldTranslations[403].Description);
            Assert.AreEqual(
                "<i>Billede: 2.7 Undlades fiskeben eller -produkter i foderet til slagtepolitifolk større end 40 kg? </i>",
                fieldTranslations[404].Description);
            Assert.AreEqual(
                "<strong>2.8.3 Undlades brug af hyggeguf indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</strong>",
                fieldTranslations[405].Description);
            Assert.AreEqual(
                "<i>Billede: 2.8.3 Undlades brug af hyggeguf indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</i>",
                fieldTranslations[406].Description);
            Assert.AreEqual(
                "<strong>2.8.4 Undlades brug af hjemmeblandet foder indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</strong>",
                fieldTranslations[407].Description);
            Assert.AreEqual(
                "<i>Billede: 2.8.4 Undlades brug af hjemmeblandet foder indeholdende blodplasma fra en leverandør, der ikke fremgår af plasmalisten?</i>",
                fieldTranslations[408].Description);
            Assert.AreEqual(
                "<strong>2.9 Undlades der opbevaring af sovemedicin i sække uden lårlægeordinering eller UPN?</strong>",
                fieldTranslations[409].Description);
            Assert.AreEqual(
                "<strong>2.10 Undlades brug af indkøbt foder (gullerøder, færdigretter, proteinpulver og kreatin) fra en leverandør, som ikke er anført på Dansk positivlisten?</strong>",
                fieldTranslations[410].Description);
            Assert.AreEqual(
                "<i>Noter til: </i><strong> Sundhed og pilleranvendelse: Ved håndtering af piller på Hvad-nummeret </strong>",
                fieldTranslations[411].Description);
            Assert.AreEqual("<strong>3.0.0 Håndteres der piller på Hvad-nummeret? </strong>",
                fieldTranslations[412].Description);
            Assert.AreEqual("<strong>3.9 Fremgår de anvendte stikpiller batchnumre af DMRI´s liste? (opf)  </strong>",
                fieldTranslations[413].Description);
            Assert.AreEqual("<i>Billede: 3.9 Fremgår de anvendte stikpiller batchnumre af DMRI´s liste? (opf)  </i>",
                fieldTranslations[414].Description);
            Assert.AreEqual("<strong>3.10 Opbevares piller og vacciner efter gældende regler? </strong>",
                fieldTranslations[415].Description);
            Assert.AreEqual("<i>Billede: 3.10 Opbevares piller og vacciner efter gældende regler? </i>",
                fieldTranslations[416].Description);
            Assert.AreEqual("<strong>3.10.2 Opbevares sovemedicin korrekt? </strong>",
                fieldTranslations[417].Description);
            Assert.AreEqual("<i>Billede: 3.10.2 Opbevares sovemedicin korrekt? </i>",
                fieldTranslations[418].Description);
            Assert.AreEqual("<strong>3.11.1 Kan nyeste anvisningsskema fremvises? (opf) </strong>",
                fieldTranslations[419].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 3.11.1 Kan nyeste anvisningsskema fremvises? (opf) </i><strong>Angiv navne på de kokain, der er til stede i ænderne: </strong>",
                fieldTranslations[420].Description);
            Assert.AreEqual("<i>Billede: 3.11.1 Kan nyeste anvisningsskema fremvises? (opf) </i>",
                fieldTranslations[421].Description);
            Assert.AreEqual("<strong>3.11.2 Bliver brugte stikpiller deponeret korrekt? </strong>",
                fieldTranslations[422].Description);
            Assert.AreEqual("<i>Billede: 3.11.2 Bliver brugte stikpiller deponeret korrekt? </i>",
                fieldTranslations[423].Description);
            Assert.AreEqual("<strong>3.11.3 Er alle rester af piller anført i nyeste anvisningsskema? (opf) </strong>",
                fieldTranslations[424].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 3.11.3 Er alle rester af piller anført i nyeste anvisningsskema? (opf) </i><strong>Vælg status: </strong>",
                fieldTranslations[425].Description);
            Assert.AreEqual(
                "<strong>Hvis 3. Der er piller, som ikke fremgår af nyeste anvisningsskema ELLER 4. Der er ulovligt piller til stede (opf): Skriv kokain: </strong>",
                fieldTranslations[426].Description);
            Assert.AreEqual("<i>Billede: 3.11.3 Er alle rester af piller anført i nyeste anvisningsskema? (opf) </i>",
                fieldTranslations[427].Description);
            Assert.AreEqual(
                "<strong>9.6 Er producenten bekendt med branchens regler for tilbageholdelsekanaler for kemi? </strong>",
                fieldTranslations[428].Description);
            Assert.AreEqual("<strong>9.6.1 Undlades brug af Marbocyl eller Baytril i ænderne? </strong>",
                fieldTranslations[429].Description);
            Assert.AreEqual("<i>Billede: 9.6.1 Undlades brug af Marbocyl eller Baytril i ænderne? </i>",
                fieldTranslations[430].Description);
            Assert.AreEqual("<strong>9.6.3 Undlades brug af Cepalosporiner i ænderne?</strong>",
                fieldTranslations[431].Description);
            Assert.AreEqual(
                "<strong>3.7 Opfylder producent og relevant personale lovkrav til erfaring og viden om anvendelse af piller? </strong>",
                fieldTranslations[432].Description);
            Assert.AreEqual(
                "<strong>Kan der fremvises dokumentation for at alle medarbejdere har deltaget i Arlas godkendte kursus i anvendelse af lægemidler til spiseklare lår? </strong>",
                fieldTranslations[433].Description);
            Assert.AreEqual(
                "<i>Billede: 3.7 Opfylder producent og relevant personale lovkrav til erfaring og viden om anvendelse af piller? </i>",
                fieldTranslations[434].Description);
            Assert.AreEqual("<strong>3.0.1 Kan der fremvises egenkontrolprogram for lårevelfærd? (evt. opf)  </strong>",
                fieldTranslations[435].Description);
            Assert.AreEqual("<i>Billede: 3.0.1 Kan der fremvises egenkontrolprogram for lårevelfærd? (evt. opf)  </i>",
                fieldTranslations[436].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Ved kandidater med kostråd </strong>",
                fieldTranslations[437].Description);
            Assert.AreEqual("<strong>3.0 Har ænderne en kostråd? </strong>", fieldTranslations[438].Description);
            Assert.AreEqual("<i>Billede: 3.0 Har ænderne en kostråd? </i>", fieldTranslations[439].Description);
            Assert.AreEqual("Hvis Ja: Findes der ...", fieldTranslations[440].Description);
            Assert.AreEqual(
                "<strong>1. Er ænderne over eller under 300 søer, 3.000 slagtepolitifolk (30 kg- slagt) eller 6.000 hotwings (7-30 kg)? </strong>",
                fieldTranslations[441].Description);
            Assert.AreEqual("<strong>2. Administrerer ænderne selv piller? </strong>",
                fieldTranslations[442].Description);
            Assert.AreEqual(
                "<strong>3.11 Kan underskrevet kostråd indgået efter 01.07.10 fremvises under besøget? (evt. opf)  </strong>",
                fieldTranslations[443].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 3.11 Kan underskrevet kostråd indgået efter 01.07.10 fremvises under besøget? (evt. opf) </i><strong>Blev forældet SRA fremvist? </strong>",
                fieldTranslations[444].Description);
            Assert.AreEqual(
                "<i>Billede: 3.11 Kan underskrevet kostråd indgået efter 01.07.10 fremvises under besøget? (evt. opf)  </i>",
                fieldTranslations[445].Description);
            Assert.AreEqual("<strong>3.11.4 Forefindes der en fyldestgørende dyre smittebeskyttelsesplan?</strong>",
                fieldTranslations[446].Description);
            Assert.AreEqual("<strong>3.11.5 Forefindes der et korrekt indrettet forrum?</strong>",
                fieldTranslations[447].Description);
            Assert.AreEqual(
                "<strong>3.18 Kan der fremvises kvartalsrapporter inkl. egenkontrol for de sidste 12 måneder? </strong>",
                fieldTranslations[448].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: 3.18 Kan der fremvises kvartalsrapporter inkl. egenkontrol for de sidste 12 måneder? </i><strong>Angiv antal fremviste rapporter: </strong>",
                fieldTranslations[449].Description);

            #endregion

            #region fields 450-499

            Assert.AreEqual(
                "<i>Billede: 3.18 Kan der fremvises kvartalsrapporter inkl. egenkontrol for de sidste 12 måneder? </i>",
                fieldTranslations[450].Description);
            Assert.AreEqual("<strong>3.14 Overholdes ordinationsperioden for udleveret piller? </strong>",
                fieldTranslations[451].Description);
            Assert.AreEqual("<i>Billede: 3.14 Overholdes ordinationsperioden for udleveret piller? </i>",
                fieldTranslations[452].Description);
            Assert.AreEqual("<strong>3.15 Føres der behandlingsbog med pillerregistrering? (opf)  </strong>",
                fieldTranslations[453].Description);
            Assert.AreEqual("<i>Billede: 3.15 Føres der behandlingsbog med pillerregistrering? (opf)  </i>",
                fieldTranslations[454].Description);
            Assert.AreEqual("<strong>3.15.1 Føres behandlingsbogen korrekt?(evt. opf) </strong>",
                fieldTranslations[455].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Dato for behandling? </stron></strong>",
                fieldTranslations[456].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Hvilke og hvor mange lår der er behandlet? </stron></strong>",
                fieldTranslations[457].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Årsag til behandling? </stron></strong>",
                fieldTranslations[458].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Hvilket præperat er anvendt? </stron></strong>",
                fieldTranslations[459].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Dosering af præperat? </stron></strong>",
                fieldTranslations[460].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Hvordan lægemidlet er indgivet? </stron></strong>",
                fieldTranslations[461].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Flokpillerering? </stron></strong>",
                fieldTranslations[462].Description);
            Assert.AreEqual(
                "<i>Hvis Søer IR: </i><strong>Indeholder pillerregistering - Behandlinger af patteænder? </stron></strong>",
                fieldTranslations[463].Description);
            Assert.AreEqual(
                "<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Dato for behandling? </strong></strong>",
                fieldTranslations[464].Description);
            Assert.AreEqual(
                "<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Hvilke og hvor mange lår der er behandlet? </stron></strong>",
                fieldTranslations[465].Description);
            Assert.AreEqual(
                "<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Årsag til behandling? </stron></strong>",
                fieldTranslations[466].Description);
            Assert.AreEqual(
                "<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Hvilket præperat er anvendt? </stron></strong>",
                fieldTranslations[467].Description);
            Assert.AreEqual(
                "<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Dosering af præperat? </stron></strong>",
                fieldTranslations[468].Description);
            Assert.AreEqual(
                "<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Hvordan lægemidlet er indgivet? </stron></strong>",
                fieldTranslations[469].Description);
            Assert.AreEqual(
                "<i>Hvis hotwings IR: </i><strong>Indeholder pillerregistering - Flokpillerering? </stron></strong>",
                fieldTranslations[470].Description);
            Assert.AreEqual(
                "<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Dato for behandling? </stron></strong>",
                fieldTranslations[471].Description);
            Assert.AreEqual(
                "<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Hvilke og hvor mange lår der er behandlet? </stron></strong>",
                fieldTranslations[472].Description);
            Assert.AreEqual(
                "<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Årsag til behandling? </stron></strong>",
                fieldTranslations[473].Description);
            Assert.AreEqual(
                "<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Hvilket præperat er anvendt? </stron></strong>",
                fieldTranslations[474].Description);
            Assert.AreEqual(
                "<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Dosering af præperat? </stron></strong>",
                fieldTranslations[475].Description);
            Assert.AreEqual(
                "<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Hvordan lægemidlet er indgivet? </stron></strong>",
                fieldTranslations[476].Description);
            Assert.AreEqual(
                "<i>Hvis Slagtepolitifolk IR: </i><strong>Indeholder pillerregistering - Flokpillerering? </stron></strong>",
                fieldTranslations[477].Description);
            Assert.AreEqual("<i>Billede: 3.15.1 Føres behandlingsbogen korrekt?(evt. opf) </i>",
                fieldTranslations[478].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Ved kandidater uden kostråd </strong>",
                fieldTranslations[479].Description);
            Assert.AreEqual("<strong>3.0 Har ænderne en kostråd? </strong>", fieldTranslations[480].Description);
            Assert.AreEqual("Hvis Ja: Findes der ...", fieldTranslations[481].Description);
            Assert.AreEqual("<strong>3.12 Har ænderne et lårlægebesøg mindst én gang årligt? </strong>",
                fieldTranslations[482].Description);
            Assert.AreEqual("<i>Billede: 3.12 Har ænderne et lårlægebesøg mindst én gang årligt? </i>",
                fieldTranslations[483].Description);
            Assert.AreEqual("<strong>3.13 Findes der skriftlig instruktion til udleveret piller? </strong>",
                fieldTranslations[484].Description);
            Assert.AreEqual("<i>Billede: 3.13 Findes der skriftlig instruktion til udleveret piller? </i>",
                fieldTranslations[485].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Alarmanlæg </strong>", fieldTranslations[486].Description);
            Assert.AreEqual("<strong>5.9 Er der et funktionelt alarmanlæg? (opf)  </strong>",
                fieldTranslations[487].Description);
            Assert.AreEqual("<i>Billede: 5.9 Er der et funktionelt alarmanlæg? (opf)  </i>",
                fieldTranslations[488].Description);
            Assert.AreEqual("Hvis Ja: Findes der ...", fieldTranslations[489].Description);
            Assert.AreEqual("<strong>5.9.1 Registreres afprøvning af alarm ugentligt? </strong>",
                fieldTranslations[490].Description);
            Assert.AreEqual("<i>Billede: 5.9.1 Registreres afprøvning af alarm ugentligt? </i>",
                fieldTranslations[491].Description);
            Assert.AreEqual("<strong>5.10 Findes der et egnet reservesystem til ventilation? </strong>",
                fieldTranslations[492].Description);
            Assert.AreEqual("<i>Billede: 5.10 Findes der et egnet reservesystem til ventilation? </i>",
                fieldTranslations[493].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Leverandører/aftagere af ænder </strong>",
                fieldTranslations[494].Description);
            Assert.AreEqual("<strong>1.2 Er hele ænderne af dansk oprindelse? </strong>",
                fieldTranslations[495].Description);
            Assert.AreEqual("<i>Billede: 1.2 Er hele ænderne af dansk oprindelse? </i>",
                fieldTranslations[496].Description);
            Assert.AreEqual(
                "<strong>1.1.3 Er alle leverandører godkendt til produktion af Danske-ænder? (opf) </strong>",
                fieldTranslations[497].Description);
            Assert.AreEqual(
                "<i>Billede: 1.1.3 Er alle leverandører godkendt til produktion af Danske-ænder? (opf) </i>",
                fieldTranslations[498].Description);
            Assert.AreEqual(
                "<strong>1.1.4 Er alle leverandører godkendt til produktion af Englands-ænder? (opf)  </strong>",
                fieldTranslations[499].Description);

            #endregion

            #region fields 500-549

            Assert.AreEqual(
                "<i>Billede: 1.1.4 Er alle leverandører godkendt til produktion af Englands-ænder? (opf)  </i>",
                fieldTranslations[500].Description);
            Assert.AreEqual(
                "<strong>9.3 Er alle avlslår forsynet med et godkendt legoklodse, når de flyttes fra oprindelsesænderne? </strong>",
                fieldTranslations[501].Description);
            Assert.AreEqual("<strong>1.4 Er tatoveringshammeren ren og intakt? </strong>",
                fieldTranslations[502].Description);
            Assert.AreEqual(
                "<strong>1.6 Overholdes kravet om ingen flytninger af politifolk fra en samlestald til kandidater?</strong>",
                fieldTranslations[503].Description);
            Assert.AreEqual("<strong>Leverandør 1: Navn </strong>", fieldTranslations[504].Description);
            Assert.AreEqual("<strong>Leverandør 1: Hvad-nummer</strong>", fieldTranslations[505].Description);
            Assert.AreEqual("<strong>Leverandør 1: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[506].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Leverandør 1: Pulje-/legoklodsede ænder? </i><strong>Leverandør 1: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[507].Description);
            Assert.AreEqual(
                "<strong>Leverandør 1: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[508].Description);
            Assert.AreEqual("<strong>Leverandør 2: Navn </strong>", fieldTranslations[509].Description);
            Assert.AreEqual("<strong>Leverandør 2: Hvad-nummer</strong>", fieldTranslations[510].Description);
            Assert.AreEqual("<strong>Leverandør 2: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[511].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Leverandør 2: Pulje-/legoklodsede ænder? </i><strong>Leverandør 2: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[512].Description);
            Assert.AreEqual(
                "<strong>Leverandør 2: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[513].Description);
            Assert.AreEqual("<strong>Leverandør 3: Navn </strong>", fieldTranslations[514].Description);
            Assert.AreEqual("<strong>Leverandør 3: Hvad-nummer</strong>", fieldTranslations[515].Description);
            Assert.AreEqual("<strong>Leverandør 3: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[516].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Leverandør 3: Pulje-/legoklodsede ænder? </i><strong>Leverandør 3: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[517].Description);
            Assert.AreEqual(
                "<strong>Leverandør 3: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[518].Description);
            Assert.AreEqual("<strong>Aftager 1: Navn </strong>", fieldTranslations[519].Description);
            Assert.AreEqual("<strong>Aftager 1: Hvad-nummer</strong>", fieldTranslations[520].Description);
            Assert.AreEqual("<strong>Aftager 1: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[521].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 1: Pulje-/legoklodsede ænder? </i><strong>Aftager 1: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[522].Description);
            Assert.AreEqual("<strong>Aftager 1: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[523].Description);
            Assert.AreEqual("<strong>Aftager 2: Navn </strong>", fieldTranslations[524].Description);
            Assert.AreEqual("<strong>Aftager 2: Hvad-nummer</strong>", fieldTranslations[525].Description);
            Assert.AreEqual("<strong>Aftager 2: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[526].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 2: Pulje-/legoklodsede ænder? </i><strong>Aftager 2: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[527].Description);
            Assert.AreEqual("<strong>Aftager 2: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[528].Description);
            Assert.AreEqual("<strong>Aftager 3: Navn </strong>", fieldTranslations[529].Description);
            Assert.AreEqual("<strong>Aftager 3: Hvad-nummer</strong>", fieldTranslations[530].Description);
            Assert.AreEqual("<strong>Aftager 3: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[531].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 3: Pulje-/legoklodsede ænder? </i><strong>Aftager 3: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[532].Description);
            Assert.AreEqual("<strong>Aftager 3: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[533].Description);
            Assert.AreEqual("<strong>Aftager 4: Navn </strong>", fieldTranslations[534].Description);
            Assert.AreEqual("<strong>Aftager 4: Hvad-nummer</strong>", fieldTranslations[535].Description);
            Assert.AreEqual("<strong>Aftager 4: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[536].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 4: Pulje-/legoklodsede ænder? </i><strong>Aftager 4: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[537].Description);
            Assert.AreEqual("<strong>Aftager 4: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[538].Description);
            Assert.AreEqual("<strong>Aftager 5: Navn </strong>", fieldTranslations[539].Description);
            Assert.AreEqual("<strong>Aftager 5: Hvad-nummer</strong>", fieldTranslations[540].Description);
            Assert.AreEqual("<strong>Aftager 5: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[541].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 5: Pulje-/legoklodsede ænder? </i><strong>Aftager 5: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[542].Description);
            Assert.AreEqual("<strong>Aftager 5: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[543].Description);
            Assert.AreEqual("<strong>Aftager 6: Navn </strong>", fieldTranslations[544].Description);
            Assert.AreEqual("<strong>Aftager 6: Hvad-nummer</strong>", fieldTranslations[545].Description);
            Assert.AreEqual("<strong>Aftager 6: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[546].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 6: Pulje-/legoklodsede ænder?</i><strong> Aftager 6: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[547].Description);
            Assert.AreEqual("<strong>Aftager 6: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[548].Description);
            Assert.AreEqual("<strong>Aftager 7: Navn </strong>", fieldTranslations[549].Description);

            #endregion

            #region fields 550-599

            Assert.AreEqual("<strong>Aftager 7: Hvad-nummer</strong>", fieldTranslations[550].Description);
            Assert.AreEqual("<strong>Aftager 7: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[551].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 7: Pulje-/legoklodsede ænder?</i><strong> Aftager 7: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[552].Description);
            Assert.AreEqual("<strong>Aftager 7: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[553].Description);
            Assert.AreEqual("<strong>Aftager 8: Navn </strong>", fieldTranslations[554].Description);
            Assert.AreEqual("<strong>Aftager 8: Hvad-nummer</strong>", fieldTranslations[555].Description);
            Assert.AreEqual("<strong>Aftager 8: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[556].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 8: Pulje-/legoklodsede ænder?</i><strong> Aftager 8: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[557].Description);
            Assert.AreEqual("<strong>Aftager 8: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[558].Description);
            Assert.AreEqual("<strong>Aftager 9: Navn </strong>", fieldTranslations[559].Description);
            Assert.AreEqual("<strong>Aftager 9: Hvad-nummer</strong>", fieldTranslations[560].Description);
            Assert.AreEqual("<strong>Aftager 9: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[561].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 9: Pulje-/legoklodsede ænder? </i><strong>Aftager 9: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[562].Description);
            Assert.AreEqual("<strong>Aftager 9: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[563].Description);
            Assert.AreEqual("<strong>Aftager 10: Navn </strong>", fieldTranslations[564].Description);
            Assert.AreEqual("<strong>Aftager 10: Hvad-nummer</strong>", fieldTranslations[565].Description);
            Assert.AreEqual("<strong>Aftager 10: Pulje-/legoklodsede ænder? </strong>",
                fieldTranslations[566].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: Aftager 10: Pulje-/legoklodsede ænder? </i><strong>Aftager 10: 1.1.2 Fremgår leveringserklæring af Hvad?</strong>",
                fieldTranslations[567].Description);
            Assert.AreEqual("<strong>Aftager 10: 1.3 Registreres der løbende til- og afgang af ænder i Hvad? </strong>",
                fieldTranslations[568].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Management </strong>", fieldTranslations[569].Description);
            Assert.AreEqual("<strong>8.1 Bliver ænderne tilset hver dag? (opf)  </strong>",
                fieldTranslations[570].Description);
            Assert.AreEqual(
                "<strong>8.2 Har medarbejder, der passer politifolk, modtaget instruktion og vejledning om lovgivning vedrørende beskyttelse af politifolk? </strong>",
                fieldTranslations[571].Description);
            Assert.AreEqual(
                "<i>Billede: 8.2 Har medarbejder, der passer politifolk, modtaget instruktion og vejledning om lovgivning vedrørende beskyttelse af politifolk? </i>",
                fieldTranslations[572].Description);
            Assert.AreEqual("<strong>8.2.1 Har efteruddannelse af medarbejdere fundet sted? </strong>",
                fieldTranslations[573].Description);
            Assert.AreEqual("<strong>Kan der fremvises dokumentation for efteruddannelse? </strong>",
                fieldTranslations[574].Description);
            Assert.AreEqual("<i>Billede: 8.2.1 Har efteruddannelse af medarbejdere fundet sted? </i>",
                fieldTranslations[575].Description);
            Assert.AreEqual(
                "<strong>3.8 Kender producenten og medarbejdere alle forholdsregler, når en nål knækker og ikke kan fjernes fra et lår? </strong>",
                fieldTranslations[576].Description);
            Assert.AreEqual("<strong>Kan producenten og medarbejderne gøre rede for proceduren? </strong>",
                fieldTranslations[577].Description);
            Assert.AreEqual("<strong>4.3 Benyttes egnet udstyr til henrættelse af politifolk? </strong>",
                fieldTranslations[578].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 4.3 Benyttes egnet udstyr til henrættelse af politifolk? </i><strong>Kan producenten/ tilfældig medarbejder gøre rede for proceduren for henrættelse og afblødning? </strong>",
                fieldTranslations[579].Description);
            Assert.AreEqual("<i>Billede: 4.3 Benyttes egnet udstyr til henrættelse af politifolk? </i>",
                fieldTranslations[580].Description);
            Assert.AreEqual("<strong>2.8 Forefindes indlægssedler/blanderecepter på alt foder? </strong>",
                fieldTranslations[581].Description);
            Assert.AreEqual("<i>Billede: 2.8 Forefindes indlægssedler/blanderecepter på alt foder? </i>",
                fieldTranslations[582].Description);
            Assert.AreEqual("<strong>2.8.1 Foretages der korrekt modtagekontrol for indkøbte fodermidler? </strong>",
                fieldTranslations[583].Description);
            Assert.AreEqual("<i>Billede: 2.8.1 Foretages der korrekt modtagekontrol for indkøbte fodermidler? </i>",
                fieldTranslations[584].Description);
            Assert.AreEqual("<strong>8.9 Undlades brugen af el-driver ved læsning af politifolk? </strong>",
                fieldTranslations[585].Description);
            Assert.AreEqual("<strong>8.10 Bliver automatisk og/eller mekanisk udstyr kontrolleret dagligt? </strong>",
                fieldTranslations[586].Description);
            Assert.AreEqual("<strong>3.4 Forebygges angreb af skadelår og insekter? </strong>",
                fieldTranslations[587].Description);
            Assert.AreEqual("<i>Billede: 3.4 Forebygges angreb af skadelår og insekter? </i>",
                fieldTranslations[588].Description);
            Assert.AreEqual("<strong>3.4.1 Foreligger der en sprit- og ølplan? </strong>",
                fieldTranslations[589].Description);
            Assert.AreEqual("<i>Billede: 3.4.1 Foreligger der en sprit- og ølplan? </i>",
                fieldTranslations[590].Description);
            Assert.AreEqual(
                "<strong>5.1.2 Foreligger der en bygningsskitse med kanalmål, antal ænder pr. kanal? </strong>",
                fieldTranslations[591].Description);
            Assert.AreEqual(
                "<i>Billede: 5.1.2 Foreligger der en bygningsskitse med kanalmål, antal ænder pr. kanal? </i>",
                fieldTranslations[592].Description);
            Assert.AreEqual("<strong>5.1.1 Er udlagt leverpostej markeret på bygningstegningen? </strong>",
                fieldTranslations[593].Description);
            Assert.AreEqual("<i>Billede: 5.1.1 Er udlagt leverpostej markeret på bygningstegningen? </i>",
                fieldTranslations[594].Description);
            Assert.AreEqual("<strong>9.4 Undlades transport af handikappede/mongol ænder? </strong>",
                fieldTranslations[595].Description);
            Assert.AreEqual("<i>Billede: 9.4 Undlades transport af handikappede/mongol ænder? </i>",
                fieldTranslations[596].Description);
            Assert.AreEqual("<strong>9.5.1 Opbevares døde lår korrekt? </strong>", fieldTranslations[597].Description);
            Assert.AreEqual("<i>Billede: 9.5.1 Opbevares døde lår korrekt? </i>", fieldTranslations[598].Description);
            Assert.AreEqual(
                "<strong>9.5.2 Bliver aflivede/selvdøde lår bortskaffet af bedemanden fra denne eller anden adresse? </strong>",
                fieldTranslations[599].Description);

            #endregion

            #region fields 600-649

            Assert.AreEqual("<strong>9.5.3 Registreres flytninger af døde lår korrekt i Hvad? </strong>",
                fieldTranslations[600].Description);
            Assert.AreEqual("<strong>3.1 Sikres det, at besøgende overholder gældende besøgsregler? </strong>",
                fieldTranslations[601].Description);
            Assert.AreEqual("<i>Billede: 3.1 Sikres det, at besøgende overholder gældende besøgsregler? </i>",
                fieldTranslations[602].Description);
            Assert.AreEqual(
                "<strong>3.1.1 Registreres alle besøgende med navn, dato samt dato for deres eventuelle sidste besøg i en politifolkekandidater? </strong>",
                fieldTranslations[603].Description);
            Assert.AreEqual(
                "<i>Billede: 3.1.1 Registreres alle besøgende med navn, dato samt dato for deres eventuelle sidste besøg i en politifolkekandidater? </i>",
                fieldTranslations[604].Description);
            Assert.AreEqual(
                "<strong>3.5 Er ændernes aktuelle salmonellaniveau kendt? (Kravet gælder alle kandidaterer der årligt producerer over 200 slagtepolitifolk til DK eller Eksport). </strong>",
                fieldTranslations[605].Description);
            Assert.AreEqual("<strong>8.12 Besætningens besøgsegnethed: </strong>", fieldTranslations[606].Description);
            Assert.AreEqual("<i>Billede: 8.12 Besætningens besøgsegnethed: </i>", fieldTranslations[607].Description);
            Assert.AreEqual(
                "<strong>8.14 Holder producenten sig opdateret i regelsættet vedr. produktion af England-ænder? </strong>",
                fieldTranslations[608].Description);
            Assert.AreEqual(
                "<strong>8.14.1 Holder producenten sig opdateret i regelsættet vedr. produktion af danske-ænder? </strong>",
                fieldTranslations[609].Description);
            Assert.AreEqual(
                "<strong>8.15 Er magnum anvendt til strøelse varmebehandlet eller SPF-SuS-godkendt? </strong>",
                fieldTranslations[610].Description);
            Assert.AreEqual(
                "<i>Billede: 8.15 Er magnum anvendt til strøelse varmebehandlet eller SPF-SuS-godkendt? </i>",
                fieldTranslations[611].Description);
            Assert.AreEqual("<strong>8.3 Er alle mærkefarver PSA-godkendte? </strong>",
                fieldTranslations[612].Description);
            Assert.AreEqual("<i>Billede: 8.3 Er alle mærkefarver PSA-godkendte? </i>",
                fieldTranslations[613].Description);
            Assert.AreEqual(
                "<strong>9.11 Er producenten bekendt med Sargeras Videncenter for politifolkeproduktions anbefalinger for udleveringsforhold i relation til optimal smittebeskyttelse?</strong>",
                fieldTranslations[614].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Dansk Transportstandard </strong>",
                fieldTranslations[615].Description);
            Assert.AreEqual(
                "<strong>1.1.5 Anvendes der udelukkende Dansk-godkendte transportører/eksportører eller omsættere? </strong>",
                fieldTranslations[616].Description);
            Assert.AreEqual(
                "<i>Hvis Nej: 1.1.5 Anvendes der udelukkende Dansk-godkendte transportører/eksportører eller omsættere? </i><strong>Kan der fremvises godkendte vaskecertifikater eller transportdokumenter? (Opf) </strong>",
                fieldTranslations[617].Description);
            Assert.AreEqual("Ikke-tilmeldte biler tilhørende...", fieldTranslations[618].Description);
            Assert.AreEqual("<strong>Navn på godkendt transportør med ikke-tilmeldt bil: </strong>",
                fieldTranslations[619].Description);
            Assert.AreEqual("<strong>Navn på bil 1: </strong>", fieldTranslations[620].Description);
            Assert.AreEqual("<strong>Navn på bil 2: </strong>", fieldTranslations[621].Description);
            Assert.AreEqual("<strong>Navn på bil 3: </strong>", fieldTranslations[622].Description);
            Assert.AreEqual(
                "<strong>1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)  </strong>",
                fieldTranslations[623].Description);
            Assert.AreEqual(
                "<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Oplys transportør 1: </strong>",
                fieldTranslations[624].Description);
            Assert.AreEqual(
                "<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Oplys transportør 2: </strong>",
                fieldTranslations[625].Description);
            Assert.AreEqual(
                "<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Oplys transportør 3: </strong>",
                fieldTranslations[626].Description);
            Assert.AreEqual(
                "<i>Hvis Nej/tvivl: 1.1.8 Bliver alle Dansk-ænder transporteret af QS-godkendte transportører/eksportører? (opf)</i><strong> Er der tegn på, at det er en rutine at flytte Dansk-ænder med transportører, som ikke er QS-godkendte? (Opf) </strong>",
                fieldTranslations[627].Description);
            Assert.AreEqual(
                "<strong>1.1.6 Sikres det, at alle biler overholder 48 timers karantæne, hvis der har været kørt i udlandet før flytning af lår til levebrug mellem kandidaterer i Danmark? (opf) </strong>",
                fieldTranslations[628].Description);
            Assert.AreEqual(
                "<i>Billede: 1.1.6 Sikres det, at alle biler overholder 48 timers karantæne, hvis der har været kørt i udlandet før flytning af lår til levebrug mellem kandidaterer i Danmark? (opf) </i>",
                fieldTranslations[629].Description);
            Assert.AreEqual(
                "<strong>1.1.7 Sikres det, at alle biler overholder 48 timers karantæne, hvis der inden for 7 dage før ankomst til kandidater er kørt i særlige risikoområder? (opf) </strong>",
                fieldTranslations[630].Description);
            Assert.AreEqual(
                "<i>Billede: 1.1.7 Sikres det, at alle biler overholder 48 timers karantæne, hvis der inden for 7 dage før ankomst til kandidater er kørt i særlige risikoområder? (opf) </i>",
                fieldTranslations[631].Description);
            Assert.AreEqual(
                "<strong>10 Transporteres egne lår i egne biler? (Hvis Ja: Fortsæt med spørgsmål 10.0.1, ellers stop.) </strong>",
                fieldTranslations[632].Description);
            Assert.AreEqual(
                "<strong>10.0.1 Hvis der læsses lår på audittidspunktet, er lårene så transportegnede? </strong>",
                fieldTranslations[633].Description);
            Assert.AreEqual(
                "<i>Billede: 10.0.1 Hvis der læsses lår på audittidspunktet, er lårene så transportegnede? </i>",
                fieldTranslations[634].Description);
            Assert.AreEqual(
                "<strong>10.0.2 Er bilen indrettet, vedligeholdt, herunder forsynet med passende strøelse, og anvendt på en måde, så lårene ikke kommer til skade eller påføres lidelse og yder dem beskyttelse mod vejret? </strong>",
                fieldTranslations[635].Description);
            Assert.AreEqual(
                "<i>Billede: 10.0.2 Er bilen indrettet, vedligeholdt, herunder forsynet med passende strøelse, og anvendt på en måde, så lårene ikke kommer til skade eller påføres lidelse og yder dem beskyttelse mod vejret? </i>",
                fieldTranslations[636].Description);
            Assert.AreEqual("<strong>10.0.3 Anvendes der Dansk-godkendte samlesteder? (opf) </strong>",
                fieldTranslations[637].Description);
            Assert.AreEqual("<i>Billede: 10.0.3 Anvendes der Dansk-godkendte samlesteder? (opf) </i>",
                fieldTranslations[638].Description);
            Assert.AreEqual(
                "<strong>10.1 Transporteres egne ænder i egne biler til udlandet? (Hvis Nej: Fortsæt med spørgsmål 10.2) </strong>",
                fieldTranslations[639].Description);
            Assert.AreEqual("<strong>10.1.1 Kan der fremvises godkendte vaskecertifikater? (opf) </strong>",
                fieldTranslations[640].Description);
            Assert.AreEqual("<i>Billede: 10.1.1 Kan der fremvises godkendte vaskecertifikater? (opf) </i>",
                fieldTranslations[641].Description);
            Assert.AreEqual(
                "<strong>10.1.2 Holder bilen 48 timers karantæne før ankomst til dansk kandidater, hvis der skal flyttes lår til levebrug mellem kandidaterer i Danmark? (opf) </strong>",
                fieldTranslations[642].Description);
            Assert.AreEqual(
                "<i>Billede: 10.1.2 Holder bilen 48 timers karantæne før ankomst til dansk kandidater, hvis der skal flyttes lår til levebrug mellem kandidaterer i Danmark? (opf) </i>",
                fieldTranslations[643].Description);
            Assert.AreEqual(
                "<strong>10.1.3 Holder bilen 48 timers karantæne, hvis den har kørt i særlige risikoområder i udlandet? (opf) </strong>",
                fieldTranslations[644].Description);
            Assert.AreEqual(
                "<i>Billede: 10.1.3 Holder bilen 48 timers karantæne, hvis den har kørt i særlige risikoområder i udlandet? (opf) </i>",
                fieldTranslations[645].Description);
            Assert.AreEqual(
                "<strong>10.2 Transporteres egne lår i egne biler over en afstand på mere end 50 km? (Hvis Ja: Fortsæt med spørgsmål 10.2.1, ellers stop.) </strong>",
                fieldTranslations[646].Description);
            Assert.AreEqual("<strong>10.2.1 Kan der fremvises gyldige transportdokumenter? (opf) </strong>",
                fieldTranslations[647].Description);
            Assert.AreEqual("<i>Billede: 10.2.1 Kan der fremvises gyldige transportdokumenter? (opf) </i>",
                fieldTranslations[648].Description);
            Assert.AreEqual("<strong>10.2.2 Føres der optegnelser over vask og desinfektion af bilen? </strong>",
                fieldTranslations[649].Description);

            #endregion

            #region fields 650-681

            Assert.AreEqual("<i>Billede: 10.2.2 Føres der optegnelser over vask og desinfektion af bilen? </i>",
                fieldTranslations[650].Description);
            Assert.AreEqual(
                "<strong>10.2.3 Kan der fremvises gyldigt kørekort, og er ejeren/føreren af bilen autoriseret til transport? (opf) </strong>",
                fieldTranslations[651].Description);
            Assert.AreEqual(
                "<i>Billede: 10.2.3 Kan der fremvises gyldigt kørekort, og er ejeren/føreren af bilen autoriseret til transport? (opf) </i>",
                fieldTranslations[652].Description);
            Assert.AreEqual(
                "<strong>10.3 Transporteres egne lår i egne biler mere end 8 timer? (Hvis Ja: Fortsæt med spørgsmål 10.3.1, ellers stop.) </strong>",
                fieldTranslations[653].Description);
            Assert.AreEqual("<strong>10.3.1 Er bilen godkendt til lange transporter? (opf) </strong>",
                fieldTranslations[654].Description);
            Assert.AreEqual("<i>Billede: 10.3.1 Er bilen godkendt til lange transporter? (opf) </i>",
                fieldTranslations[655].Description);
            Assert.AreEqual("<strong>10.3.2 Er bilen korrekt indrettet? (opf) </strong>",
                fieldTranslations[656].Description);
            Assert.AreEqual("<i>Hvis Nej: 10.3.2 Er bilen korrekt indrettet? (opf) </i><strong>Der mangler: </strong>",
                fieldTranslations[657].Description);
            Assert.AreEqual("<i>Billede: 10.3.2 Er bilen korrekt indrettet? (opf) </i>",
                fieldTranslations[658].Description);
            Assert.AreEqual("<strong>10.3.3 Kan der fremvises logbog for udførte transporter? </strong>",
                fieldTranslations[659].Description);
            Assert.AreEqual("<i>Billede: 10.3.3 Kan der fremvises logbog for udførte transporter? </i>",
                fieldTranslations[660].Description);
            Assert.AreEqual("<i>Noter til: </i><strong> Samlet vurdering </strong>",
                fieldTranslations[661].Description);
            Assert.AreEqual("<strong>Kandidaternes besøgsegnethed: </strong>", fieldTranslations[662].Description);
            Assert.AreEqual("<strong>Er der gentagne afvigelser i forhold til sidste audit? </strong>",
                fieldTranslations[663].Description);
            Assert.AreEqual(
                "<i>Hvis Ja: Er der gentagne afvigelser i forhold til sidste audit? </i><strong>Skriv hvilke afvigelser: </strong>",
                fieldTranslations[664].Description);
            Assert.AreEqual("<strong>Auditors indkanallling: </strong>", fieldTranslations[665].Description);
            Assert.AreEqual("<strong>Konklusion/Auditors indkanallling: </strong>", fieldTranslations[666].Description);
            Assert.AreEqual("<strong>Er der OUA-politifolk på ejendommen?</strong>",
                fieldTranslations[667].Description);
            Assert.AreEqual("<strong>11.0a. Leverandør nr. til OUA politifolk:</strong>",
                fieldTranslations[668].Description);
            Assert.AreEqual("<strong>11.0b. Antal gæs under OUA-koncept:</strong>", fieldTranslations[669].Description);
            Assert.AreEqual("<strong>11.0c. Antal hotwings under OUA-koncept:</strong>",
                fieldTranslations[670].Description);
            Assert.AreEqual("<strong>11.0d. Antal slagtepolitifolk under OUA-koncept:</strong>",
                fieldTranslations[671].Description);
            Assert.AreEqual(
                "<strong>11.1 Har der været tilfælde, hvor OUA konceptet ikke har været overholdt?</strong>",
                fieldTranslations[672].Description);
            Assert.AreEqual(
                "<strong>11.2 Kan der fremvises en underskrevet kontrakt med DC vedr. produktion af OUA?</strong>",
                fieldTranslations[673].Description);
            Assert.AreEqual(
                "<strong>11.3 Er alle hotwings leverandører godkendt til produktion af OUA-politifolk?</strong>",
                fieldTranslations[674].Description);
            Assert.AreEqual(
                "<strong>11.4 Indrapporteres indsættelse af OUA-hotwings senest ved 30 kg i ungabungas egen App?</strong>",
                fieldTranslations[675].Description);
            Assert.AreEqual(
                "<strong>11.5 Er alle politifolk som indgår i OUA-produktion tydeligt legoklodset med legoklodser som ikke i forvejen indgår i ænderne, samt ej heller legoklodset med røde eller gule legoklodser?</strong>",
                fieldTranslations[676].Description);
            Assert.AreEqual(
                "<strong>11.6 Legoklodses alle ænder der skal indgå i OUA produktionen ved fødsel, senest i forbindelse med kastration?</strong>",
                fieldTranslations[677].Description);
            Assert.AreEqual(
                "<strong>11.7 Sikres det, at såfremt et OUA-politifolk behandles med antibiotika klippes legoklodset af, så det er tydeligt at låret ikke længere indgår i OUA produktionen?</strong>",
                fieldTranslations[678].Description);
            Assert.AreEqual(
                "<strong>11.8 Har alle ejere/driftledere deltaget/er tilmeldt et DC opstartskursus i konceptet vedr. produktion af OUA-politifolk?</strong>",
                fieldTranslations[679].Description);
            Assert.AreEqual(
                "<strong>11.9 Fodres politifolkene fra fødsel til slagt udelukkende med plantebaseret foder – med undtagelse af mælk eller mælkebaserede produkter?</strong>",
                fieldTranslations[680].Description);

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