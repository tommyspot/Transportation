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
    public class CreateCategoryFixture : UnitFixture
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
        public void CategoryDoesNotExist_ReturnsBadRequest()
        {
            var result = service.Create(null);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void VariableIsNull_ReturnsNotFound()
        {
            NumericCategory category = Presto.Create<NumericCategory>();
            var result = service.Create(category.ToJson());
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void CategoryIsValid_SavesChanges()
        {
            NumericCategory category = Presto.Persist<NumericCategory>();
            var json = category.ToJson();
            var result = service.Create(json);

            Assert.IsTrue(fakeDB.SaveChangesCalled);
        }

        [Test]
        public void CategoryIsValid_ReturnsOK()
        {
            NumericCategory category = Presto.Persist<NumericCategory>();
            var json = category.ToJson();
            var result = service.Create(json);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
