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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using eFormShared;

namespace eFormSqlController
{
    public partial class options : base_entity
    {
        public int nextQuestionId { get; set; }
        
        public int weight { get; set; }
        
        public int weightValue { get; set; }
        
        public int continuousOptionId { get; set; }
        
        [ForeignKey("question")]
        public int questionId { get; set; }
        
        public int optionsIndex { get; set; }
        
        public virtual questions Question { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.options.Add(this);
            dbContext.SaveChanges();

            dbContext.option_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            options option = dbContext.options.FirstOrDefault(x => x.id == id);

            if (option == null)
            {
                throw new NullReferenceException($"Could not find option with ID: {id}");
            }

            option.questionId = questionId;
            option.weight = weight;
            option.weightValue = weightValue;
            option.nextQuestionId = nextQuestionId;
            option.continuousOptionId = continuousOptionId;
            option.optionsIndex = optionsIndex;

            if (dbContext.ChangeTracker.HasChanges())
            {
                version += 1;
                updated_at = DateTime.Now;

                dbContext.option_versions.Add(MapVersions(option));
                dbContext.SaveChanges();
            }

        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            options option = dbContext.options.FirstOrDefault(x => x.id == id);

            if (option == null)
            {
                throw new NullReferenceException($"Could not find option with ID: {id}");
            }

            option.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                version += 1;
                updated_at = DateTime.Now;

                dbContext.option_versions.Add(MapVersions(option));
                dbContext.SaveChanges();
            }
        }

        private option_versions MapVersions(options option)
        {
            option_versions optionVersions = new option_versions();

            optionVersions.questionId = option.questionId;
            optionVersions.weight = option.weight;
            optionVersions.weightValue = option.weightValue;
            optionVersions.nextQuestionId = option.nextQuestionId;
            optionVersions.continuousOptionId = option.continuousOptionId;
            optionVersions.optionsIndex = option.optionsIndex;
            optionVersions.optionId = option.id;

            return optionVersions;

        }
    }
}