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
    public class QuestionSetsUTest : DbTestFixture
    {
        [Test]
        public async Task QuestionSets_Create_DoesCreate()
        {
            //Arrange

            Random rnd = new Random();
            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };

            //Act

            await questionSet.Create(DbContext).ConfigureAwait(false);

            List<QuestionSet> questionSets = DbContext.QuestionSets.AsNoTracking().ToList();
            List<QuestionSetVersion> questionSetVersions = DbContext.QuestionSetVersions.AsNoTracking().ToList();

            Assert.NotNull(questionSets);
            Assert.NotNull(questionSetVersions);

            Assert.AreEqual(1, questionSets.Count());
            Assert.AreEqual(1, questionSetVersions.Count());

            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSets[0].CreatedAt.ToString());
            Assert.AreEqual(questionSet.Version, questionSets[0].Version);
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());
            Assert.AreEqual(questionSets[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSets[0].Id);
            Assert.AreEqual(questionSet.Name, questionSets[0].Name);
            Assert.AreEqual(questionSet.Share, questionSets[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSets[0].HasChild);
            Assert.AreEqual(questionSet.PossiblyDeployed, questionSets[0].PossiblyDeployed);

            //Versions

            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[0].CreatedAt.ToString());
            Assert.AreEqual(questionSet.Version, questionSetVersions[0].Version);
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[0].Id);
            Assert.AreEqual(questionSet.Name, questionSetVersions[0].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[0].HasChild);
            Assert.AreEqual(questionSet.PossiblyDeployed, questionSetVersions[0].PossiblyDeployed);
        }

        [Test]
        public async Task QuestionSets_Update_DoesUpdate()
        {
            //Arrange

            Random rnd = new Random();
            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = questionSet.UpdatedAt;
            string oldName = questionSet.Name;
            bool oldShare = questionSet.Share;
            bool oldHasChild = questionSet.HasChild;
            bool oldPossiblyDeployed = questionSet.PossiblyDeployed;

            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PossiblyDeployed = randomBool;

            await questionSet.Update(DbContext).ConfigureAwait(false);

            List<QuestionSet> questionSets = DbContext.QuestionSets.AsNoTracking().ToList();
            List<QuestionSetVersion> questionSetVersions = DbContext.QuestionSetVersions.AsNoTracking().ToList();

            Assert.NotNull(questionSets);
            Assert.NotNull(questionSetVersions);

            Assert.AreEqual(1, questionSets.Count());
            Assert.AreEqual(2, questionSetVersions.Count());

            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSets[0].CreatedAt.ToString());
            Assert.AreEqual(questionSet.Version, questionSets[0].Version);
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());
            Assert.AreEqual(questionSets[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSets[0].Id);
            Assert.AreEqual(questionSet.Name, questionSets[0].Name);
            Assert.AreEqual(questionSet.Share, questionSets[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSets[0].HasChild);
            Assert.AreEqual(questionSet.PossiblyDeployed, questionSets[0].PossiblyDeployed);

            //Old Version

            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, questionSetVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldName, questionSetVersions[0].Name);
            Assert.AreEqual(oldShare, questionSetVersions[0].Share);
            Assert.AreEqual(oldHasChild, questionSetVersions[0].HasChild);
            Assert.AreEqual(oldPossiblyDeployed, questionSetVersions[0].PossiblyDeployed);

            //New Version
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, questionSetVersions[1].Version);
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[1].QuestionSetId);
            Assert.AreEqual(questionSet.Name, questionSetVersions[1].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[1].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[1].HasChild);
            Assert.AreEqual(questionSet.PossiblyDeployed, questionSetVersions[1].PossiblyDeployed);
        }

        [Test]
        public async Task QuestionSets_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Random rnd = new Random();
            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = questionSet.UpdatedAt;

            await questionSet.Delete(DbContext);

            List<QuestionSet> questionSets = DbContext.QuestionSets.AsNoTracking().ToList();
            List<QuestionSetVersion> questionSetVersions = DbContext.QuestionSetVersions.AsNoTracking().ToList();

            Assert.NotNull(questionSets);
            Assert.NotNull(questionSetVersions);

            Assert.AreEqual(1, questionSets.Count());
            Assert.AreEqual(2, questionSetVersions.Count());

            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSets[0].CreatedAt.ToString());
            Assert.AreEqual(questionSet.Version, questionSets[0].Version);
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());
            Assert.AreEqual(questionSets[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(questionSet.Id, questionSets[0].Id);
            Assert.AreEqual(questionSet.Name, questionSets[0].Name);
            Assert.AreEqual(questionSet.Share, questionSets[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSets[0].HasChild);
            Assert.AreEqual(questionSet.PossiblyDeployed, questionSets[0].PossiblyDeployed);

            //Old Version

            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, questionSetVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[0].QuestionSetId);
            Assert.AreEqual(questionSet.Name, questionSetVersions[0].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[0].HasChild);
            Assert.AreEqual(questionSet.PossiblyDeployed, questionSetVersions[0].PossiblyDeployed);

            //New Version
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, questionSetVersions[1].Version);
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(questionSetVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(questionSet.Id, questionSetVersions[1].QuestionSetId);
            Assert.AreEqual(questionSet.Name, questionSetVersions[1].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[1].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[1].HasChild);
            Assert.AreEqual(questionSet.PossiblyDeployed, questionSetVersions[1].PossiblyDeployed);
        }
    }
}