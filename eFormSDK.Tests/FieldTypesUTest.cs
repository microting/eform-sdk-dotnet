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
            
            List<field_types> fieldTypes = dbContext.field_types.AsNoTracking().ToList();
            
            //Assert Before creating new field type
            
            Assert.NotNull(fieldTypes);
            Assert.AreEqual(19, fieldTypes.Count());
            
            //Assert after creating new field type
            
            await fieldType.Create(dbContext);
            fieldTypes = dbContext.field_types.AsNoTracking().ToList();
            Assert.AreEqual(20, fieldTypes.Count());
            
            Assert.AreEqual(fieldType.Description, fieldTypes[19].Description);
            Assert.AreEqual(fieldType.Id, fieldTypes[19].Id);
            Assert.AreEqual(fieldType.FieldType, fieldTypes[19].FieldType);

            dbContext.field_types.Remove(fieldType);
            await dbContext.SaveChangesAsync();
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
            await fieldType.Create(dbContext);
            
            //Act

            fieldType.Description = Guid.NewGuid().ToString();
            fieldType.FieldType = Guid.NewGuid().ToString();
            
            await fieldType.Update(dbContext);

            
            List<field_types> fieldTypes = dbContext.field_types.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(fieldTypes);
            Assert.AreEqual(20, fieldTypes.Count());
            
            Assert.AreEqual(fieldType.Description, fieldTypes[19].Description);
            Assert.AreEqual(fieldType.Id, fieldTypes[19].Id);
            Assert.AreEqual(fieldType.FieldType, fieldTypes[19].FieldType);
            
            dbContext.field_types.Remove(fieldType);
            await dbContext.SaveChangesAsync();
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
            await fieldType.Create(dbContext);
            
            //Act

            List<field_types> fieldTypes = dbContext.field_types.AsNoTracking().ToList();
            
            //Assert before delete
            
            Assert.NotNull(fieldTypes);
            Assert.AreEqual(20, fieldTypes.Count());
            
            //Assert after delete
            
            await fieldType.Delete(dbContext);
            
            fieldTypes = dbContext.field_types.AsNoTracking().ToList();
            
            Assert.AreEqual(19, fieldTypes.Count());            

        }
    }
}