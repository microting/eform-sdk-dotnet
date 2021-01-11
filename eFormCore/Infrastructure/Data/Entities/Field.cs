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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public  class Field : PnBase
    {
        public Field()
        {
            Children = new HashSet<Field>();
            FieldValues = new HashSet<FieldValue>();
            Translations = new HashSet<FieldTranslation>();
            FieldOptions = new HashSet<FieldOption>();
        }

        public int? ParentFieldId { get; set; }

        [ForeignKey("CheckList")]
        public int? CheckListId { get; set; }

        [ForeignKey("FieldType")]
        public int? FieldTypeId { get; set; }

        public short? Mandatory { get; set; }

        public short? ReadOnly { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        [StringLength(255)]
        public string Color { get; set; }

        public int? DisplayIndex { get; set; }

        public short? Dummy { get; set; }

        public string DefaultValue { get; set; }

        [StringLength(255)]
        public string UnitName { get; set; }

        [StringLength(255)]
        public string MinValue { get; set; }

        [StringLength(255)]
        public string MaxValue { get; set; }

        public int? MaxLength { get; set; }

        public int? DecimalCount { get; set; }

        public int? Multi { get; set; }

        public short? Optional { get; set; }

        public short? Selected { get; set; }

        public short? Split { get; set; }

        public short? GeolocationEnabled { get; set; }

        public short? GeolocationForced { get; set; }

        public short? GeolocationHidden { get; set; }

        public short? StopOnSave { get; set; }

        public short? IsNum { get; set; }

        public short? BarcodeEnabled { get; set; }

        [StringLength(255)]
        public string BarcodeType { get; set; }

        [StringLength(255)]
        public string QueryType { get; set; }

        public string KeyValuePairList { get; set; }

        public string Custom { get; set; }

        public int? EntityGroupId { get; set; }

        public string OriginalId { get; set; }

        public virtual FieldType FieldType { get; set; }

        public virtual CheckList CheckList { get; set; }

        public virtual Field Parent { get; set; }

        public virtual ICollection<Field> Children { get; set; }

        public virtual ICollection<FieldValue> FieldValues { get; set; }

        public virtual ICollection<FieldTranslation> Translations { get; set; }

        public virtual ICollection<FieldOption> FieldOptions { get; set; }


        public static async Task MoveTranslations(MicrotingDbContext dbContext)
        {
            List<Field> fields = await dbContext.Fields.ToListAsync();
            Language defaultLanguage = await dbContext.Languages.SingleAsync(x => x.Name == "Danish");
            foreach (Field field in fields)
            {
                if (!string.IsNullOrEmpty(field.Label))
                {
                    FieldTranslation fieldTranslation = new FieldTranslation
                    {
                        Text = field.Label,
                        Description = field.Description,
                        FieldId = field.Id,
                        LanguageId = defaultLanguage.Id
                    };
                    await fieldTranslation.Create(dbContext);
                    field.Label = null;
                    field.Description = null;
                    await field.Update(dbContext);
                }

                if (!string.IsNullOrEmpty(field.KeyValuePairList))
                {
                    List<FieldOption> fieldOptions =
                        await dbContext.FieldOptions.Where(x => x.FieldId == field.Id).ToListAsync();
                    if (!fieldOptions.Any())
                    {
                        List<Dto.KeyValuePair> keyValuePairs = PairRead((field.KeyValuePairList));
                        foreach (Dto.KeyValuePair keyValuePair in keyValuePairs)
                        {
                            FieldOption fieldOption = await dbContext.FieldOptions.SingleOrDefaultAsync(x =>
                                x.FieldId == field.Id && x.Key == keyValuePair.Key);
                            if (fieldOption == null)
                            {
                                fieldOption = new FieldOption()
                                {
                                    FieldId = field.Id,
                                    Key = keyValuePair.Key,
                                    DisplayOrder = keyValuePair.DisplayOrder
                                };
                                await fieldOption.Create(dbContext);
                                FieldOptionTranslation fieldOptionTranslation = new FieldOptionTranslation()
                                {
                                    FieldOptionId = fieldOption.Id,
                                    LanguageId = defaultLanguage.Id,
                                    Text = keyValuePair.Value
                                };
                                await fieldOptionTranslation.Create(dbContext);
                            }
                        }

                        field.KeyValuePairList = null;
                        await field.Update(dbContext);
                    }
                }
            }
        }

        private static List<Dto.KeyValuePair> PairRead(string str)
        {
            List<Dto.KeyValuePair> list = new List<Dto.KeyValuePair>();
            str = Locate(str, "<hash>", "</hash>");

            bool flag = true;
            int index = 1;
            string keyValue, displayIndex;
            bool selected;

            while (flag)
            {
                string inderStr = Locate(str, "<" + index + ">", "</" + index + ">");

                keyValue = Locate(inderStr, "<key>", "</");
                selected = bool.Parse(Locate(inderStr.ToLower(), "<selected>", "</"));
                displayIndex = Locate(inderStr, "<displayIndex>", "</");

                list.Add(new Dto.KeyValuePair(index.ToString(), keyValue, selected, displayIndex));

                index += 1;

                if (Locate(str, "<" + index + ">", "</" + index + ">") == "")
                    flag = false;
            }

            return list;
        }


        private static string Locate(string textStr, string startStr, string endStr)
        {
            try
            {
                if (!textStr.Contains(startStr))
                    return "";

                if (!textStr.Contains(endStr))
                    return "";

                int startIndex = textStr.IndexOf(startStr, StringComparison.Ordinal) + startStr.Length;
                int length = textStr.IndexOf(endStr, startIndex, StringComparison.Ordinal) - startIndex;
                //return textStr.Substring(startIndex, lenght);
                return textStr.AsSpan().Slice(start: startIndex, length).ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
