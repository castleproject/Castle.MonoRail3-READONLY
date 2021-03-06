﻿#region License
//  Copyright 2004-2011 Castle Project - http://www.castleproject.org/
//  Hamilton Verissimo de Oliveira and individual contributors as indicated. 
//  See the committers.txt/contributors.txt in the distribution for a 
//  full listing of individual contributors.
// 
//  This is free software; you can redistribute it and/or modify it
//  under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 3 of
//  the License, or (at your option) any later version.
// 
//  You should have received a copy of the GNU Lesser General Public
//  License along with this software; if not, write to the Free
//  Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
//  02110-1301 USA, or see the FSF site: http://www.fsf.org.
#endregion

namespace Castle.MonoRail.Tests.Serializers.Form
{
	using System;
	using System.Collections.Generic;
    using Castle.MonoRail.Serialization;
    using NUnit.Framework;

    [TestFixture]
    public class FormBasedSerializerTests
    {
        [Test]
        public void Deserialize_WhenEmptyInput_JustInstantiateModel()
        {
            var ctx = new HttpContextStub();
            
            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new DataAnnotationsModelMetadataProvider());
            
            Assert.IsNotNull(model);
        }

        [Test]
        public void Deserialize_WithDepth0StringInput_FillsProperty()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[name]"] = "hammett";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

            Assert.AreEqual("hammett", model.Name);
        }

        [Test]
        public void Deserialize_WithDepth0Int32Input_FillsProperty()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[age]"] = "32";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

            Assert.AreEqual(32, model.Age);
        }

        [Test]
        public void Deserialize_WithDepth1StringInput_FillsProperty()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[address][city]"] = "kirkland";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

            Assert.IsNotNull(model.Address);
            Assert.AreEqual("kirkland", model.Address.City);
        }

        [Test]
        public void Deserialize_WithDepth1IntForCollection_CreatesAndFillsCollection()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[permissions][id]"] = "10";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

            Assert.IsNotNull(model.Permissions);
            Assert.AreEqual(1, model.Permissions.Count);
            Assert.AreEqual(10, model.Permissions[0].Id);
        }

        [Test]
        public void Deserialize_WithMultipleEntriesForSameCollection_CreatesAndFillsCollection()
        {
            var ctx = new HttpContextStub();
            var form = ctx.RequestStub.Form;
            form["customer[permissions][id]"] = "10,11,12,13";

            var serializer = new FormBasedSerializer<Customer>() as IModelSerializer<Customer>;
            var model = serializer.Deserialize("customer", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

            Assert.IsNotNull(model.Permissions);
            Assert.AreEqual(4, model.Permissions.Count);
            Assert.AreEqual(10, model.Permissions[0].Id);
            Assert.AreEqual(11, model.Permissions[1].Id);
            Assert.AreEqual(12, model.Permissions[2].Id);
            Assert.AreEqual(13, model.Permissions[3].Id);
        }

		[Test]
		public void Deserialize_WithDepth0DecimalInput_FillsProperty()
		{
			var ctx = new HttpContextStub();
			var form = ctx.RequestStub.Form;
			form["company[revenue]"] = "17.3";

			var serializer = new FormBasedSerializer<Company>() as IModelSerializer<Company>;
			var model = serializer.Deserialize("company", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

			Assert.AreEqual(17.3m, model.Revenue);
		}

		[Test]
		public void Deserialize_WithDepth0DatetimeInput_FillsProperty()
		{
			var ctx = new HttpContextStub();
			var form = ctx.RequestStub.Form;
			form["company[since]"] = "01/01/1950";

			var serializer = new FormBasedSerializer<Company>() as IModelSerializer<Company>;
			var model = serializer.Deserialize("company", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

			Assert.AreEqual(new DateTime(1950, 1, 1), model.Since);
		}

		[Test]
		public void Deserialize_WithDepth0EnumInput_FillsProperty()
		{
			var ctx = new HttpContextStub();
			var form = ctx.RequestStub.Form;
			form["company[category]"] = "Industry";

			var serializer = new FormBasedSerializer<Company>() as IModelSerializer<Company>;
			var model = serializer.Deserialize("company", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

			Assert.AreEqual(CompanyCategory.Industry, model.Category);
		}

		[Test]
		public void Deserialize_WithDepth0NullableEnumInput_FillsProperty()
		{
			var ctx = new HttpContextStub();
			var form = ctx.RequestStub.Form;
			form["company[categoryex]"] = "Industry";

			var serializer = new FormBasedSerializer<Company>() as IModelSerializer<Company>;
			var model = serializer.Deserialize("company", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

			Assert.AreEqual(CompanyCategory.Industry, model.CategoryEx);
		}

		[Test]
		public void Deserialize_WithDepth0GuidInput_FillsProperty()
		{
			var ctx = new HttpContextStub();
			var form = ctx.RequestStub.Form;
			form["company[id]"] = "6C8A7A2C-0E37-45D5-B1AF-56A714AB47D5";

			var serializer = new FormBasedSerializer<Company>() as IModelSerializer<Company>;
			var model = serializer.Deserialize("company", "", ctx.Request, new DataAnnotationsModelMetadataProvider());

			Assert.AreEqual(Guid.Parse("6C8A7A2C-0E37-45D5-B1AF-56A714AB47D5"), model.Id);
		}

        class Address
        {
            public string City { get; set; }
        }

        class Permission
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class Customer
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Address Address { get; set; }
            public List<Permission> Permissions { get; set; }
        }

		class Company
		{
			public DateTime Since { get; set; }

			public decimal Revenue { get; set; }

			public CompanyCategory Category { get; set; }

			public CompanyCategory? CategoryEx { get; set; }

			public Guid Id { get; set; }
		}

		enum CompanyCategory
		{
			Undefined,
			Industry,
			Commerce
		}

    }
}
