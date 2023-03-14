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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.InSight.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class LanguageQuestionSetUTest : DbTestFixture
    {
        [Test]
        public async Task LanguageQuestionSet_Create_DoesCreate_W_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            LanguageQuestionSet languageQuestionSet = new LanguageQuestionSet
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };

            //Act
            await languageQuestionSet.Create(DbContext).ConfigureAwait(false);

            List<LanguageQuestionSet> languageQuestionSets = DbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<LanguageQuestionSetVersion> languageQuestionSetVersions =
                DbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);

            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(1, languageQuestionSetVersions.Count);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[0].MicrotingUid);
        }

        [Test]
        public async Task LanguageQuestionSet_Create_DoesCreate_WO_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            LanguageQuestionSet languageQuestionSet = new LanguageQuestionSet
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
            };

            //Act
            await languageQuestionSet.Create(DbContext).ConfigureAwait(false);

            List<LanguageQuestionSet> languageQuestionSets = DbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<LanguageQuestionSetVersion> languageQuestionSetVersions =
                DbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);

            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(1, languageQuestionSetVersions.Count);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSets[0].MicrotingUid);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[0].MicrotingUid);
        }

        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_W_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            LanguageQuestionSet languageQuestionSet = new LanguageQuestionSet
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await languageQuestionSet.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            var oldMicrotingUid = languageQuestionSet.MicrotingUid;

            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;
            languageQuestionSet.MicrotingUid = rnd.Next(1, 255);


            //Act
            await languageQuestionSet.Update(DbContext).ConfigureAwait(false);

            List<LanguageQuestionSet> languageQuestionSets = DbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<LanguageQuestionSetVersion> languageQuestionSetVersions =
                DbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);

            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);

            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldMicrotingUid, languageQuestionSetVersions[0].MicrotingUid);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[1].MicrotingUid);
        }

        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_WO_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            LanguageQuestionSet languageQuestionSet = new LanguageQuestionSet
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
            };
            await languageQuestionSet.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;

            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;

            //Act
            await languageQuestionSet.Update(DbContext).ConfigureAwait(false);

            List<LanguageQuestionSet> languageQuestionSets = DbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<LanguageQuestionSetVersion> languageQuestionSetVersions =
                DbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);

            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSets[0].MicrotingUid);

            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[0].MicrotingUid);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[1].MicrotingUid);
        }

        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_W_MicrotingUid_RemovesUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            LanguageQuestionSet languageQuestionSet = new LanguageQuestionSet
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await languageQuestionSet.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            var oldMicrotingUid = languageQuestionSet.MicrotingUid;

            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;
            languageQuestionSet.MicrotingUid = null;


            //Act
            await languageQuestionSet.Update(DbContext).ConfigureAwait(false);

            List<LanguageQuestionSet> languageQuestionSets = DbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<LanguageQuestionSetVersion> languageQuestionSetVersions =
                DbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);

            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSets[0].MicrotingUid);

            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldMicrotingUid, languageQuestionSetVersions[0].MicrotingUid);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[1].MicrotingUid);
        }

        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_WO_MicrotingUid_AddsUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            LanguageQuestionSet languageQuestionSet = new LanguageQuestionSet
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
            };
            await languageQuestionSet.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;

            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;
            languageQuestionSet.MicrotingUid = rnd.Next(1, 255);

            //Act
            await languageQuestionSet.Update(DbContext).ConfigureAwait(false);

            List<LanguageQuestionSet> languageQuestionSets = DbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<LanguageQuestionSetVersion> languageQuestionSetVersions =
                DbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);

            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);

            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[0].MicrotingUid);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[1].MicrotingUid);
        }

        [Test]
        public async Task LanguageQuestionSet_Delete_DoesDelete()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            LanguageQuestionSet languageQuestionSet = new LanguageQuestionSet
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await languageQuestionSet.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            var oldMicrotingUid = languageQuestionSet.MicrotingUid;

            //Act
            await languageQuestionSet.Delete(DbContext).ConfigureAwait(false);

            List<LanguageQuestionSet> languageQuestionSets = DbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<LanguageQuestionSetVersion> languageQuestionSetVersions =
                DbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);

            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, languageQuestionSets[0].WorkflowState);

            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldMicrotingUid, languageQuestionSetVersions[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Created, languageQuestionSetVersions[0].WorkflowState);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[1].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, languageQuestionSetVersions[1].WorkflowState);
        }
    }
}