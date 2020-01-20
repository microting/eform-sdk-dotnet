using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class language_question_sets : BaseEntity
    {
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        [ForeignKey("question_set")]
        public int QuestionSetId { get; set; }
        
        public virtual question_sets QuestionSet { get; set; }
        
        public virtual languages Language { get; set; }
        
        public int? MicrotingUid { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.LanguageQuestionSets.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.LanguageQuestionSetVersions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            language_question_sets languageQuestionSet = 
                await dbContext.LanguageQuestionSets.SingleOrDefaultAsync(x => x.Id == Id);

            if (languageQuestionSet == null)
            {
                throw new NullReferenceException($"Could not find language_question_set with id {Id}");
            }
            
            languageQuestionSet.LanguageId = LanguageId;
            languageQuestionSet.QuestionSetId = QuestionSetId;
            languageQuestionSet.WorkflowState = WorkflowState;
            languageQuestionSet.MicrotingUid = MicrotingUid;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                languageQuestionSet.UpdatedAt = DateTime.UtcNow;
                languageQuestionSet.Version += 1;

                dbContext.LanguageQuestionSetVersions.Add(MapVersions(languageQuestionSet));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            language_question_sets languageQuestionSet = 
                await dbContext.LanguageQuestionSets.SingleOrDefaultAsync(x => x.Id == Id);

            if (languageQuestionSet == null)
            {
                throw new NullReferenceException($"Could not find language_question_set with id {Id}");
            }
            
            languageQuestionSet.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                languageQuestionSet.UpdatedAt = DateTime.UtcNow;
                languageQuestionSet.Version += 1;

                dbContext.LanguageQuestionSetVersions.Add(MapVersions(languageQuestionSet));
                await dbContext.SaveChangesAsync();
            }
        }

        private language_question_set_versions MapVersions(language_question_sets languageQuestionSets)
        {
            return new language_question_set_versions()
            {
                CreatedAt = languageQuestionSets.CreatedAt,
                UpdatedAt = languageQuestionSets.UpdatedAt,
                WorkflowState = languageQuestionSets.WorkflowState,
                Version = languageQuestionSets.Version,
                MicrotingUid = languageQuestionSets.MicrotingUid,
                LanguageId = languageQuestionSets.LanguageId,
                QuestionSetId = languageQuestionSets.QuestionSetId
            };
        }
    }
}