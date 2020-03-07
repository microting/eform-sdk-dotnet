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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class uploaded_data : BaseEntity
    {
        public int? UploaderId { get; set; }

        [StringLength(255)]
        public string Checksum { get; set; }

        [StringLength(255)]
        public string Extension { get; set; }

        [StringLength(255)]
        public string CurrentFile { get; set; }

        [StringLength(255)]
        public string UploaderType { get; set; }

        [StringLength(255)]
        public string FileLocation { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public short? Local { get; set; }

        public int? TranscriptionId { get; set; }
        
        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.uploaded_data.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.uploaded_data_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            uploaded_data uploadedData = await dbContext.uploaded_data.FirstOrDefaultAsync(x => x.Id == Id);

            if (uploadedData == null)
            {
                throw new NullReferenceException($"Could not find Uploaded Data with Id: {Id}");
            }

            uploadedData.UploaderId = UploaderId;
            uploadedData.UploaderType = UploaderType;
            uploadedData.Checksum = Checksum;
            uploadedData.Extension = Extension;
            uploadedData.Local = Local;
            uploadedData.FileName = FileName;
            uploadedData.CurrentFile = CurrentFile;
            uploadedData.FileLocation = FileLocation;
            uploadedData.ExpirationDate = ExpirationDate;
            uploadedData.TranscriptionId = TranscriptionId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                uploadedData.Version += 1;
                uploadedData.UpdatedAt = DateTime.Now;

                dbContext.uploaded_data_versions.Add(MapVersions(uploadedData));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            uploaded_data uploadedData = await dbContext.uploaded_data.FirstOrDefaultAsync(x => x.Id == Id);

            if (uploadedData == null)
            {
                throw new NullReferenceException($"Could not find Uploaded Data with Id: {Id}");
            }

            uploadedData.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                uploadedData.Version += 1;
                uploadedData.UpdatedAt = DateTime.Now;

                dbContext.uploaded_data_versions.Add(MapVersions(uploadedData));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        
        private uploaded_data_versions MapVersions(uploaded_data uploadedData)
        {
            return new uploaded_data_versions
            {
                CreatedAt = uploadedData.CreatedAt,
                UpdatedAt = uploadedData.UpdatedAt,
                Checksum = uploadedData.Checksum,
                Extension = uploadedData.Extension,
                CurrentFile = uploadedData.CurrentFile,
                UploaderId = uploadedData.UploaderId,
                UploaderType = uploadedData.UploaderType,
                WorkflowState = uploadedData.WorkflowState,
                ExpirationDate = uploadedData.ExpirationDate,
                Version = uploadedData.Version,
                Local = uploadedData.Local,
                FileLocation = uploadedData.FileLocation,
                FileName = uploadedData.FileName,
                TranscriptionId = uploadedData.TranscriptionId,
                DataUploadedId = uploadedData.Id
            };
        }
    }
}
