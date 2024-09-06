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

            Assert.That(questionSets.Count(), Is.EqualTo(1));
            Assert.That(questionSetVersions.Count(), Is.EqualTo(1));

            Assert.That(questionSets[0].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSets[0].Version, Is.EqualTo(questionSet.Version));
            //            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionSets[0].WorkflowState));
            Assert.That(questionSets[0].Id, Is.EqualTo(questionSet.Id));
            Assert.That(questionSets[0].Name, Is.EqualTo(questionSet.Name));
            Assert.That(questionSets[0].Share, Is.EqualTo(questionSet.Share));
            Assert.That(questionSets[0].HasChild, Is.EqualTo(questionSet.HasChild));
            Assert.That(questionSets[0].PossiblyDeployed, Is.EqualTo(questionSet.PossiblyDeployed));

            //Versions

            Assert.That(questionSetVersions[0].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSetVersions[0].Version, Is.EqualTo(questionSet.Version));
            //            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionSetVersions[0].WorkflowState));
            Assert.That(questionSetVersions[0].Id, Is.EqualTo(questionSet.Id));
            Assert.That(questionSetVersions[0].Name, Is.EqualTo(questionSet.Name));
            Assert.That(questionSetVersions[0].Share, Is.EqualTo(questionSet.Share));
            Assert.That(questionSetVersions[0].HasChild, Is.EqualTo(questionSet.HasChild));
            Assert.That(questionSetVersions[0].PossiblyDeployed, Is.EqualTo(questionSet.PossiblyDeployed));
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

            Assert.That(questionSets.Count(), Is.EqualTo(1));
            Assert.That(questionSetVersions.Count(), Is.EqualTo(2));

            Assert.That(questionSets[0].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSets[0].Version, Is.EqualTo(questionSet.Version));
            //            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionSets[0].WorkflowState));
            Assert.That(questionSets[0].Id, Is.EqualTo(questionSet.Id));
            Assert.That(questionSets[0].Name, Is.EqualTo(questionSet.Name));
            Assert.That(questionSets[0].Share, Is.EqualTo(questionSet.Share));
            Assert.That(questionSets[0].HasChild, Is.EqualTo(questionSet.HasChild));
            Assert.That(questionSets[0].PossiblyDeployed, Is.EqualTo(questionSet.PossiblyDeployed));

            //Old Version

            Assert.That(questionSetVersions[0].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSetVersions[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionSetVersions[0].WorkflowState));
            Assert.That(questionSetVersions[0].QuestionSetId, Is.EqualTo(questionSet.Id));
            Assert.That(questionSetVersions[0].Name, Is.EqualTo(oldName));
            Assert.That(questionSetVersions[0].Share, Is.EqualTo(oldShare));
            Assert.That(questionSetVersions[0].HasChild, Is.EqualTo(oldHasChild));
            Assert.That(questionSetVersions[0].PossiblyDeployed, Is.EqualTo(oldPossiblyDeployed));

            //New Version
            Assert.That(questionSetVersions[1].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSetVersions[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionSetVersions[0].WorkflowState));
            Assert.That(questionSetVersions[1].QuestionSetId, Is.EqualTo(questionSet.Id));
            Assert.That(questionSetVersions[1].Name, Is.EqualTo(questionSet.Name));
            Assert.That(questionSetVersions[1].Share, Is.EqualTo(questionSet.Share));
            Assert.That(questionSetVersions[1].HasChild, Is.EqualTo(questionSet.HasChild));
            Assert.That(questionSetVersions[1].PossiblyDeployed, Is.EqualTo(questionSet.PossiblyDeployed));
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

            Assert.That(questionSets.Count(), Is.EqualTo(1));
            Assert.That(questionSetVersions.Count(), Is.EqualTo(2));

            Assert.That(questionSets[0].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSets[0].Version, Is.EqualTo(questionSet.Version));
            //            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(questionSets[0].WorkflowState));
            Assert.That(questionSets[0].Id, Is.EqualTo(questionSet.Id));
            Assert.That(questionSets[0].Name, Is.EqualTo(questionSet.Name));
            Assert.That(questionSets[0].Share, Is.EqualTo(questionSet.Share));
            Assert.That(questionSets[0].HasChild, Is.EqualTo(questionSet.HasChild));
            Assert.That(questionSets[0].PossiblyDeployed, Is.EqualTo(questionSet.PossiblyDeployed));

            //Old Version

            Assert.That(questionSetVersions[0].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSetVersions[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionSetVersions[0].WorkflowState));
            Assert.That(questionSetVersions[0].QuestionSetId, Is.EqualTo(questionSet.Id));
            Assert.That(questionSetVersions[0].Name, Is.EqualTo(questionSet.Name));
            Assert.That(questionSetVersions[0].Share, Is.EqualTo(questionSet.Share));
            Assert.That(questionSetVersions[0].HasChild, Is.EqualTo(questionSet.HasChild));
            Assert.That(questionSetVersions[0].PossiblyDeployed, Is.EqualTo(questionSet.PossiblyDeployed));

            //New Version
            Assert.That(questionSetVersions[1].CreatedAt.ToString(), Is.EqualTo(questionSet.CreatedAt.ToString()));
            Assert.That(questionSetVersions[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(questionSetVersions[1].WorkflowState));
            Assert.That(questionSetVersions[1].QuestionSetId, Is.EqualTo(questionSet.Id));
            Assert.That(questionSetVersions[1].Name, Is.EqualTo(questionSet.Name));
            Assert.That(questionSetVersions[1].Share, Is.EqualTo(questionSet.Share));
            Assert.That(questionSetVersions[1].HasChild, Is.EqualTo(questionSet.HasChild));
            Assert.That(questionSetVersions[1].PossiblyDeployed, Is.EqualTo(questionSet.PossiblyDeployed));
        }
    }
}