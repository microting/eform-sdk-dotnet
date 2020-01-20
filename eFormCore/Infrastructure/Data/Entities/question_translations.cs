using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class question_translations : BaseEntity
    {
        [ForeignKey("question")]
        public int QuestionId { get; set; }
        
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        public string Name { get; set; }
        
        public virtual questions Question { get; set; }
        
        public virtual languages Language { get; set; }
        
        public int? MicrotingUid { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.QuestionTranslations.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.QuestionTranslationVersions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }
        public async Task Update(MicrotingDbContext dbContext)
        {
            question_translations questionTranslation = 
                await dbContext.QuestionTranslations.SingleOrDefaultAsync(x => x.Id == Id);

            if (questionTranslation == null)
            {
                throw new NullReferenceException($"Could not find question_translation with id {Id}");
            }

            questionTranslation.LanguageId = LanguageId;
            questionTranslation.QuestionId = QuestionId;
            questionTranslation.Name = Name;
            questionTranslation.WorkflowState = WorkflowState;
            questionTranslation.MicrotingUid = MicrotingUid;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                questionTranslation.UpdatedAt = DateTime.UtcNow;
                questionTranslation.Version += 1;

                dbContext.QuestionTranslationVersions.Add(MapVersions(questionTranslation));
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task Delete(MicrotingDbContext dbContext)
        {
            question_translations questionTranslation = 
                await dbContext.QuestionTranslations.SingleOrDefaultAsync(x => x.Id == Id);

            if (questionTranslation == null)
            {
                throw new NullReferenceException($"Could not find question_translation with id {Id}");
            }
            
            questionTranslation.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                questionTranslation.UpdatedAt = DateTime.UtcNow;
                questionTranslation.Version += 1;

                dbContext.QuestionTranslationVersions.Add(MapVersions(questionTranslation));
                await dbContext.SaveChangesAsync();
            }
        }

        private question_translation_versions MapVersions(question_translations questionTranslations)
        {
            return new question_translation_versions()
            {
                CreatedAt = questionTranslations.CreatedAt,
                UpdatedAt = questionTranslations.UpdatedAt,
                Version = questionTranslations.Version,
                WorkflowState = questionTranslations.WorkflowState,
                MicrotingUid = questionTranslations.MicrotingUid,
                LanguageId = questionTranslations.LanguageId,
                QuestionId = questionTranslations.QuestionId,
                Name = questionTranslations.Name
            };
        }
    }
}