using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class option_translations : BaseEntity
    {
        [ForeignKey("option")]
        public int OptionId { get; set; }
        
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        public string Name { get; set; }

        public virtual options option { get; set; }
        
        public virtual languages Language { get; set; }
        
        public int? MicrotingUid { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.OptionTranslations.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.OptionTranslationVersions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            option_translations optionTranslation = await dbContext.OptionTranslations.SingleOrDefaultAsync(x => x.Id == Id);

            if (optionTranslation == null)
            {
                throw new NullReferenceException($"Could not find option_translation with id {Id}");
            }
            
            optionTranslation.WorkflowState = WorkflowState;
            optionTranslation.LanguageId = LanguageId;
            optionTranslation.OptionId = OptionId;
            optionTranslation.MicrotingUid = MicrotingUid;
            optionTranslation.Name = Name;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                optionTranslation.UpdatedAt = DateTime.UtcNow;
                optionTranslation.Version += 1;

                dbContext.OptionTranslationVersions.Add(MapVersions(optionTranslation));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            option_translations option_translation = await dbContext.OptionTranslations.SingleOrDefaultAsync(x => x.Id == Id);

            if (option_translation == null)
            {
                throw new NullReferenceException($"Could not find notification with id {Id}");
            }
            
            option_translation.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                option_translation.UpdatedAt = DateTime.UtcNow;
                option_translation.Version += 1;

                dbContext.OptionTranslationVersions.Add(MapVersions(option_translation));
                await dbContext.SaveChangesAsync();
            }
        }

        private option_translation_versions MapVersions(option_translations optionTranslations)
        {
            return new option_translation_versions()
            {
                LanguageId = optionTranslations.LanguageId,
                OptionId = optionTranslations.OptionId,
                WorkflowState = optionTranslations.WorkflowState,
                OptionTranslationId = optionTranslations.Id,
                Name = optionTranslations.Name,
                CreatedAt = optionTranslations.CreatedAt,
                UpdatedAt = optionTranslations.UpdatedAt,
                Version = optionTranslations.Version,
                MicrotingUid = optionTranslations.MicrotingUid
            };
        }
    }
}