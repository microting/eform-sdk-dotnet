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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)] public string Type { get; set; }

        [StringLength(255)] public string Description { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            dbContext.FieldTypes.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            FieldType fieldType = await dbContext.FieldTypes.FirstOrDefaultAsync(x => x.Id == Id);

            if (fieldType == null)
            {
                throw new NullReferenceException($"Could not find Field Type with Id: {Id}");
            }

            fieldType.Description = Description;
            fieldType.Type = Type;
            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            FieldType fieldType = await dbContext.FieldTypes.FirstOrDefaultAsync(x => x.Id == Id);

            if (fieldType == null)
            {
                throw new NullReferenceException($"Could not find Field Type with Id: {Id}");
            }

            dbContext.Remove(fieldType);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}