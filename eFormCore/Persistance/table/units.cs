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

    public partial class units : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }

        public int? microting_uid { get; set; }

        public int? otp_code { get; set; }

        public int? customer_no { get; set; }
//
//        public int? version { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        [ForeignKey("site")]
        public int? site_id { get; set; }

        public virtual sites site { get; set; }
        
        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.units.Add(this);
            dbContext.SaveChanges();

            dbContext.unit_versions.Add(MapUnitVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            units unit = dbContext.units.FirstOrDefault(x => x.Id == Id);

            if (unit == null)
            {
                throw new NullReferenceException($"Could not find Unit with Id: {Id}");
            }

            unit.site_id = site_id;
            unit.microting_uid = microting_uid;
            unit.otp_code = otp_code;
            unit.customer_no = customer_no;

            if (dbContext.ChangeTracker.HasChanges())
            {
                unit.version += 1;
                unit.updated_at = DateTime.Now;

                dbContext.unit_versions.Add(MapUnitVersions(unit));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            
            units unit = dbContext.units.FirstOrDefault(x => x.Id == Id);

            if (unit == null)
            {
                throw new NullReferenceException($"Could not find Unit with Id: {Id}");
            }

            unit.workflow_state = Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                unit.version += 1;
                unit.updated_at = DateTime.Now;

                dbContext.unit_versions.Add(MapUnitVersions(unit));
                dbContext.SaveChanges();
            }
        }

        
        
        private unit_versions MapUnitVersions(units units)
        {
            unit_versions unitVer = new unit_versions();
            unitVer.workflow_state = units.workflow_state;
            unitVer.version = units.version;
            unitVer.created_at = units.created_at;
            unitVer.updated_at = units.updated_at;
            unitVer.microting_uid = units.microting_uid;
            unitVer.site_id = units.site_id;
            unitVer.customer_no = units.customer_no;
            unitVer.otp_code = units.otp_code;

            unitVer.unit_id = units.Id; //<<--

            return unitVer;
        }
    }
}
