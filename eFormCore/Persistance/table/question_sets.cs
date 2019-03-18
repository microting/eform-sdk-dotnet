/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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
using eFormShared;

namespace eFormSqlController
{
    public partial class question_sets : base_entity
    {
        public string name { get; set; }
        
        public bool hasChild { get; set; }
        
        public bool posiblyDeployed { get; set; }
        
        public int parentId { get; set; }
        
        public bool share { get; set; }


        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            version = 1;

            dbContext.question_sets.Add(this);
            dbContext.SaveChanges();

            dbContext.question_set_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            question_sets questionSet = dbContext.question_sets.FirstOrDefault(x => x.id == id);

            if (questionSet == null)
            {
                throw new NullReferenceException($"Could not find question set with ID: {id}");
            }

            questionSet.name = name;
            questionSet.share = share;
            questionSet.hasChild = hasChild;
            questionSet.posiblyDeployed = posiblyDeployed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                updated_at = DateTime.Now;
                version += 1;

                dbContext.question_set_versions.Add(MapVersions(questionSet));
                dbContext.SaveChanges();
              
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            question_sets questionSet = dbContext.question_sets.FirstOrDefault(x => x.id == id);

            if (questionSet == null)
            {
                throw new NullReferenceException($"Could not find question set with ID: {id}");
            }

            questionSet.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                updated_at = DateTime.Now;
                version += 1;

                dbContext.question_set_versions.Add(MapVersions(questionSet));
                dbContext.SaveChanges();
              
            }
        }
        
        private question_set_versions MapVersions(question_sets questionSet)
        {
            question_set_versions questionSetVersions = new question_set_versions();

            questionSetVersions.questionSetId = questionSet.id;
            questionSetVersions.name = questionSet.name;
            questionSetVersions.share = questionSet.share;
            questionSetVersions.hasChild = questionSet.hasChild;
            questionSetVersions.posiblyDeployed = questionSet.posiblyDeployed;
            questionSetVersions.version = questionSet.version;
            questionSetVersions.created_at = questionSet.created_at;
            questionSetVersions.updated_at = questionSet.updated_at;
            questionSetVersions.workflow_state = questionSet.workflow_state;

            
            return questionSetVersions;
        }
    }
}