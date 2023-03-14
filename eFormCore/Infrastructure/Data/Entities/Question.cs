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


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class Question : PnBase
    {
        [ForeignKey("QuestionSet")] public int QuestionSetId { get; set; }

        public string QuestionType { get; set; }

        public int Minimum { get; set; }

        public int Maximum { get; set; }

        public string Type { get; set; }

        public int RefId { get; set; }

        public int QuestionIndex { get; set; }

        public bool Image { get; set; }

        public int ContinuousQuestionId { get; set; }

        public string ImagePosition { get; set; }

        public bool Prioritised { get; set; }

        public bool BackButtonEnabled { get; set; }

        public string FontSize { get; set; }

        public int MinDuration { get; set; }

        public int MaxDuration { get; set; }

        public bool ValidDisplay { get; set; }

        public int? MicrotingUid { get; set; }

        public virtual QuestionSet QuestionSet { get; set; }

        public virtual ICollection<QuestionTranslation> QuestionTranslationses { get; set; }

        public virtual ICollection<Option> Options { get; set; }

        public async Task Create(MicrotingDbContext dbContext, bool CreateSpecialQuestionTypes)
        {
            await Create(dbContext);
            if (CreateSpecialQuestionTypes)
            {
                await GenerateSpecialQuestionTypes(dbContext, 1);
            }
        }

        public bool IsSmiley()
        {
            switch (QuestionType)
            {
                case Constants.Constants.QuestionTypes.Smiley:
                case Constants.Constants.QuestionTypes.Smiley2:
                case Constants.Constants.QuestionTypes.Smiley3:
                case Constants.Constants.QuestionTypes.Smiley4:
                case Constants.Constants.QuestionTypes.Smiley5:
                case Constants.Constants.QuestionTypes.Smiley6:
                case Constants.Constants.QuestionTypes.Smiley7:
                case Constants.Constants.QuestionTypes.Smiley8:
                case Constants.Constants.QuestionTypes.Smiley9:
                case Constants.Constants.QuestionTypes.Smiley10:
                    return true;
                default:
                    return false;
            }
        }

        private async Task GenerateSmileyOptions(MicrotingDbContext dbContext, int languageId)
        {
            string[] smileys = { "" };
            switch (QuestionType)
            {
                case Constants.Constants.QuestionTypes.Smiley:
                    smileys = new[] { "smiley1", "smiley2", "smiley3", "smiley5", "smiley6" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley2:
                    smileys = new[] { "smiley1", "smiley2", "smiley3", "smiley4", "smiley5", "smiley6" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley3:
                    smileys = new[] { "smiley1", "smiley5" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley4:
                    smileys = new[] { "smiley1", "smiley5", "smiley6" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley5:
                    smileys = new[] { "smiley1", "smiley3", "smiley5" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley6:
                    smileys = new[] { "smiley1", "smiley3", "smiley5", "smiley6" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley7:
                    smileys = new[] { "smiley1", "smiley2", "smiley3", "smiley5" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley8:
                    smileys = new[] { "smiley1", "smiley2", "smiley4", "smiley5" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley9:
                    smileys = new[] { "smiley1", "smiley2", "smiley4", "smiley5", "smiley6" };
                    break;
                case Constants.Constants.QuestionTypes.Smiley10:
                    smileys = new[] { "smiley1", "smiley2", "smiley3", "smiley4", "smiley5" };
                    break;
            }

            int i = 0;
            foreach (string smiley in smileys)
            {
                await CreateSmileyOption(dbContext, smiley, languageId, i);
                i++;
            }
        }

        private async Task CreateSmileyOption(MicrotingDbContext dbContext, string smiley, int languageId, int index)
        {
            switch (smiley)
            {
                case "smiley1":
                    await CreateSpecialOption(dbContext, 5, 100, smiley, languageId, index);
                    break;
                case "smiley2":
                    await CreateSpecialOption(dbContext, 4, 75, smiley, languageId, index);
                    break;
                case "smiley3":
                    await CreateSpecialOption(dbContext, 3, 50, smiley, languageId, index);
                    break;
                case "smiley4":
                    await CreateSpecialOption(dbContext, 2, 25, smiley, languageId, index);
                    break;
                case "smiley5":
                    await CreateSpecialOption(dbContext, 1, 0, smiley, languageId, index);
                    break;
                case "smiley6":
                    await CreateSpecialOption(dbContext, 0, 999, smiley, languageId, index);
                    break;
            }
        }

        private async Task GenerateSpecialQuestionTypes(MicrotingDbContext dbContext, int languageId)
        {
            switch (QuestionType)
            {
                case Constants.Constants.QuestionTypes.Number:
                    await CreateSpecialOption(dbContext, 1, 0, "number", languageId, 1);
                    await CreateSpecialOption(dbContext, 1, 0, "na", languageId, 2);
                    break;
                case Constants.Constants.QuestionTypes.Text:
                    await CreateSpecialOption(dbContext, 1, 0, "text", languageId, 1);
                    await CreateSpecialOption(dbContext, 1, 0, "na", languageId, 2);
                    break;
                case Constants.Constants.QuestionTypes.Picture:
                    await CreateSpecialOption(dbContext, 1, 0, "next", languageId, 1);
                    break;
                case Constants.Constants.QuestionTypes.InfoText:
                    await CreateSpecialOption(dbContext, 1, 0, "next", languageId, 1);
                    break;
                case Constants.Constants.QuestionTypes.TextEamil:
                    await CreateSpecialOption(dbContext, 1, 0, "text", languageId, 1);
                    break;
                case Constants.Constants.QuestionTypes.ZipCode:
                    await CreateSpecialOption(dbContext, 1, 0, "text", languageId, 1);
                    break;
                case Constants.Constants.QuestionTypes.Smiley:
                case Constants.Constants.QuestionTypes.Smiley2:
                case Constants.Constants.QuestionTypes.Smiley3:
                case Constants.Constants.QuestionTypes.Smiley4:
                case Constants.Constants.QuestionTypes.Smiley5:
                case Constants.Constants.QuestionTypes.Smiley6:
                case Constants.Constants.QuestionTypes.Smiley7:
                case Constants.Constants.QuestionTypes.Smiley8:
                case Constants.Constants.QuestionTypes.Smiley9:
                case Constants.Constants.QuestionTypes.Smiley10:
                    await GenerateSmileyOptions(dbContext, languageId);
                    break;
                    ;
            }
        }

        private async Task CreateSpecialOption(MicrotingDbContext dbContext, int weight, int weightedValue, string text,
            int languageId, int optionIndex)
        {
            var result = (from ot in dbContext.OptionTranslations
                join o in dbContext.Options
                    on ot.OptionId equals o.Id
                where o.Weight == weight
                      && o.WeightValue == weightedValue
                      && ot.Name == text
                      && ot.LanguageId == languageId
                      && o.QuestionId == Id
                select new
                {
                    o.Id
                }).ToList();

            if (!result.Any())
            {
                Option option = new Option
                {
                    Weight = weight,
                    WeightValue = weightedValue,
                    OptionIndex = optionIndex,
                    QuestionId = Id
                };

                await option.Create(dbContext).ConfigureAwait(false);

                OptionTranslation optionTranslation = new OptionTranslation
                {
                    OptionId = option.Id,
                    Name = text,
                    LanguageId = languageId
                };
                await optionTranslation.Create(dbContext).ConfigureAwait(false);
            }
        }

        private QuestionVersion MapVersions(Question question)
        {
            return new QuestionVersion
            {
                QuestionSetId = question.QuestionSetId,
                Type = question.Type,
                Image = question.Image,
                Maximum = question.Maximum,
                Minimum = question.Minimum,
                Prioritised = question.Prioritised,
                RefId = question.RefId,
                FontSize = question.FontSize,
                QuestionId = question.Id,
                MaxDuration = question.MaxDuration,
                MinDuration = question.MinDuration,
                ImagePosition = question.ImagePosition,
                QuestionType = question.QuestionType,
                ValidDisplay = question.ValidDisplay,
                QuestionIndex = question.QuestionIndex,
                BackButtonEnabled = question.BackButtonEnabled,
                ContinuousQuestionId = question.ContinuousQuestionId,
                CreatedAt = question.CreatedAt,
                Version = question.Version,
                UpdatedAt = question.UpdatedAt,
                WorkflowState = question.WorkflowState,
                MicrotingUid = question.MicrotingUid
            };
        }
    }
}