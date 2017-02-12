using System.Linq;
using Conjurer;
using Epinion.Clarity.Api;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net;
using System.IO;
using System;
using Epinion.Clarity.Api.Framework;
using System.Web;
using Moq;
using System.Web.Routing;

namespace Epinion.Clarity.UnitTests.Api.CategoryServiceFixtures
{
    public class UpdateCategoryFixture : UnitFixture
    {
        private FakeDB fakeDB;
        private CategoryService service;

        [SetUp]
        public void Setup()
        { 
            fakeDB = new FakeDB();
            ClarityDB.Instance = fakeDB;
            ClarityDB.CurrentTenant = Presto.Persist<Tenant>();

            service = new CategoryService();
        }       

        [Test]
        public void CategoryDoesNotExist_ReturnsNotFound()
        {
            NumericCategory category = Presto.Create<NumericCategory>(); 
            var result = service.UpdateCategory(category.ID, category.ToJson());

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void CategoryIsNull_ReturnsBadRequest()
        {
            var result = service.UpdateCategory(1, null);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void CategoryIsNotValid_ReturnsBadRequest()
        {
            NumericCategory category = Presto.Persist<NumericCategory>(X => X.ID = 1);
            var json = JObject.Parse("{ id: 1 }");         
            var result = service.UpdateCategory(1, json);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void CategoryIsValid_SavesChanges()
        {
            NumericCategory category = Presto.Persist<NumericCategory>();         
            var json = category.ToJson();
            var result = service.UpdateCategory(category.ID, json);

            Assert.IsTrue(fakeDB.SaveChangesCalled);
        }

        [Test]
        public void CategoryIsValid_ReturnsOK()
        {
            NumericCategory category = Presto.Persist<NumericCategory>();
            var json = category.ToJson();
            var result = service.UpdateCategory(category.ID, json);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void CategoryIsValid_ReturnsJson()
        {
            NumericCategory category = Presto.Persist<NumericCategory>();
            var json = category.ToJson();
            var result = service.UpdateCategory(category.ID, json);

            Assert.NotNull(result.Json);
        }
    }
}
