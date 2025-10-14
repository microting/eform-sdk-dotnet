/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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

namespace eFormSDK.CheckLists.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class FieldTypesUTest : DbTestFixture
{
    [Test]
    public async Task FieldType_Create_DoesCreate()
    {
        //Arrange

        FieldType fieldType = new FieldType
        {
            Description = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString()
        };

        //Act

        List<FieldType> fieldTypes = DbContext.FieldTypes.AsNoTracking().ToList();

        //Assert Before creating new field type

        Assert.That(fieldTypes, Is.Not.EqualTo(null));
        Assert.That(fieldTypes.Count(), Is.EqualTo(20));

        //Assert after creating new field type

        await fieldType.Create(DbContext).ConfigureAwait(false);
        fieldTypes = DbContext.FieldTypes.AsNoTracking().ToList();
        Assert.That(fieldTypes.Count(), Is.EqualTo(21));

        Assert.That(fieldTypes[20].Description, Is.EqualTo(fieldType.Description));
        Assert.That(fieldTypes[20].Id, Is.EqualTo(fieldType.Id));
        Assert.That(fieldTypes[20].Type, Is.EqualTo(fieldType.Type));
    }

    [Test]
    public async Task FieldType_Update_DoesUpdate()
    {
        //Arrange

        FieldType fieldType = new FieldType
        {
            Description = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString()
        };
        await fieldType.Create(DbContext).ConfigureAwait(false);

        //Act

        fieldType.Description = Guid.NewGuid().ToString();
        fieldType.Type = Guid.NewGuid().ToString();

        await fieldType.Update(DbContext).ConfigureAwait(false);


        List<FieldType> fieldTypes = DbContext.FieldTypes.AsNoTracking().ToList();

        //Assert

        Assert.That(fieldTypes, Is.Not.EqualTo(null));
        Assert.That(fieldTypes.Count(), Is.EqualTo(21));

        Assert.That(fieldTypes[20].Description, Is.EqualTo(fieldType.Description));
        Assert.That(fieldTypes[20].Id, Is.EqualTo(fieldType.Id));
        Assert.That(fieldTypes[20].Type, Is.EqualTo(fieldType.Type));
    }

    [Test]
    public async Task FieldType_Delete_DoesDelete()
    {
        //Arrange

        FieldType fieldType = new FieldType
        {
            Description = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString()
        };
        await fieldType.Create(DbContext).ConfigureAwait(false);

        //Act

        List<FieldType> fieldTypes = DbContext.FieldTypes.AsNoTracking().ToList();

        //Assert before delete

        Assert.That(fieldTypes, Is.Not.EqualTo(null));
        Assert.That(fieldTypes.Count(), Is.EqualTo(21));

        //Assert after delete

        await fieldType.Delete(DbContext);

        fieldTypes = DbContext.FieldTypes.AsNoTracking().ToList();

        Assert.That(fieldTypes.Count(), Is.EqualTo(20));
    }
}