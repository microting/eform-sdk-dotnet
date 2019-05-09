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

using System.Linq;
using eFormShared;

namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class uploaded_data : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public int? uploader_id { get; set; }

        [StringLength(255)]
        public string checksum { get; set; }

        [StringLength(255)]
        public string extension { get; set; }

        [StringLength(255)]
        public string current_file { get; set; }

        [StringLength(255)]
        public string uploader_type { get; set; }

        [StringLength(255)]
        public string file_location { get; set; }

        [StringLength(255)]
        public string file_name { get; set; }

        public DateTime? expiration_date { get; set; }

        public short? local { get; set; }

        public int? transcription_id { get; set; }
        
        
        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.uploaded_data.Add(this);
            dbContext.SaveChanges();

            dbContext.uploaded_data_versions.Add(MapUploadedDataVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            uploaded_data uploadedData = dbContext.uploaded_data.FirstOrDefault(x => x.id == id);

            if (uploadedData == null)
            {
                throw new NullReferenceException($"Could not find Uploaded Data with ID: {id}");
            }

            uploadedData.uploader_id = uploader_id;
            uploadedData.uploader_type = uploader_type;
            uploadedData.checksum = checksum;
            uploadedData.extension = extension;
            uploadedData.local = local;
            uploadedData.file_name = file_name;
            uploadedData.current_file = current_file;
            uploadedData.file_location = file_location;
            uploadedData.expiration_date = expiration_date;
            uploadedData.transcription_id = transcription_id;


            if (dbContext.ChangeTracker.HasChanges())
            {
                uploadedData.version += 1;
                uploadedData.updated_at = DateTime.Now;

                dbContext.uploaded_data_versions.Add(MapUploadedDataVersions(uploadedData));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            uploaded_data uploadedData = dbContext.uploaded_data.FirstOrDefault(x => x.id == id);

            if (uploadedData == null)
            {
                throw new NullReferenceException($"Could not find Uploaded Data with ID: {id}");
            }

            uploadedData.workflow_state = Constants.WorkflowStates.Removed;


            if (dbContext.ChangeTracker.HasChanges())
            {
                uploadedData.version += 1;
                uploadedData.updated_at = DateTime.Now;

                dbContext.uploaded_data_versions.Add(MapUploadedDataVersions(uploadedData));
                dbContext.SaveChanges();
            }
            
        }

        
        private uploaded_data_versions MapUploadedDataVersions(uploaded_data uploadedData)
        {
            uploaded_data_versions udv = new uploaded_data_versions();

            udv.created_at = uploadedData.created_at;
            udv.updated_at = uploadedData.updated_at;
            udv.checksum = uploadedData.checksum;
            udv.extension = uploadedData.extension;
            udv.current_file = uploadedData.current_file;
            udv.uploader_id = uploadedData.uploader_id;
            udv.uploader_type = uploadedData.uploader_type;
            udv.workflow_state = uploadedData.workflow_state;
            udv.expiration_date = uploadedData.expiration_date;
            udv.version = uploadedData.version;
            udv.local = uploadedData.local;
            udv.file_location = uploadedData.file_location;
            udv.file_name = uploadedData.file_name;

            udv.data_uploaded_id = uploadedData.id; //<<--

            return udv;
        }
    }
}
