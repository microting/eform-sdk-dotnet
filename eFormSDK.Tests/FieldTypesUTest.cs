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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class FieldTypesUTest : DbTestFixture
    {
        [Test]
        public async Task FieldType_Create_DoesCreate()
        {
            //Arrange

            field_types fieldType = new field_types
            {
                Description = Guid.NewGuid().ToString(),
                FieldType = Guid.NewGuid().ToString()
            };

            //Act
            
            List<field_types> fieldTypes = DbContext.field_types.AsNoTracking().ToList();
            
            //Assert Before creating new field type
            
            Assert.NotNull(fieldTypes);
            Assert.AreEqual(19, fieldTypes.Count());
            
            //Assert after creating new field type
            
            await fieldType.Create(DbContext).ConfigureAwait(false);
            fieldTypes = DbContext.field_types.AsNoTracking().ToList();
            Assert.AreEqual(20, fieldTypes.Count());
            
            Assert.AreEqual(fieldType.Description, fieldTypes[19].Description);
            Assert.AreEqual(fieldType.Id, fieldTypes[19].Id);
            Assert.AreEqual(fieldType.FieldType, fieldTypes[19].FieldType);
        }

        [Test]
        public async Task FieldType_Update_DoesUpdate()
        {
            //Arrange

            field_types fieldType = new field_types
            {
                Description = Guid.NewGuid().ToString(),
                FieldType = Guid.NewGuid().ToString()
            };
            await fieldType.Create(DbContext).ConfigureAwait(false);
            
            //Act

            fieldType.Description = Guid.NewGuid().ToString();
            fieldType.FieldType = Guid.NewGuid().ToString();
            
            await fieldType.Update(DbContext).ConfigureAwait(false);

            
            List<field_types> fieldTypes = DbContext.field_types.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(fieldTypes);
            Assert.AreEqual(20, fieldTypes.Count());
            
            Assert.AreEqual(fieldType.Description, fieldTypes[19].Description);
            Assert.AreEqual(fieldType.Id, fieldTypes[19].Id);
            Assert.AreEqual(fieldType.FieldType, fieldTypes[19].FieldType);
        }

        [Test]
        public async Task FieldType_Delete_DoesDelete()
        {
            //Arrange

            field_types fieldType = new field_types
            {
                Description = Guid.NewGuid().ToString(),
                FieldType = Guid.NewGuid().ToString()
            };
            await fieldType.Create(DbContext).ConfigureAwait(false);
            
            //Act

            List<field_types> fieldTypes = DbContext.field_types.AsNoTracking().ToList();
            
            //Assert before delete
            
            Assert.NotNull(fieldTypes);
            Assert.AreEqual(20, fieldTypes.Count());
            
            //Assert after delete
            
            await fieldType.Delete(DbContext);
            
            fieldTypes = DbContext.field_types.AsNoTracking().ToList();
            
            Assert.AreEqual(19, fieldTypes.Count());
        }
    }
}