using System;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class FieldTypesUTest : DbTestFixture
    {
        [Test]
        public void FieldType_Create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            field_types fieldType = new field_types();
            fieldType.Description = Guid.NewGuid().ToString();
            fieldType.Id = rnd.Next(1, 255);
            fieldType.FieldType = Guid.NewGuid().ToString();
            
            
            //TODO 
            
        }
    }
}