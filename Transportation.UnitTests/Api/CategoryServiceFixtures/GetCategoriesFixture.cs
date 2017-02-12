using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;
using Conjurer;
using Epinion.Clarity.Api.Framework;
using System.Net;
using System.Web;
namespace Epinion.Clarity.UnitTests.Api.CategoryServiceFixtures
{
    public class GetCategoriesFixture : UnitFixture
    {     
        [Test]
        public void GetCategoriesByVariable_VariableExists_ReturnsOK()
        {
            ClarityDB.Instance = new FakeDB();
            var variable = Presto.Persist<NumericVariable>();
            var service = new CategoryService();
            RestApiResult result = service.GetCategoriesByVariable(variable.ID);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void GetCategories_VariableHasOneCategory_ReturnsOneCategorie()
        {
            ClarityDB.Instance = new FakeDB();
            var category = Presto.Persist<NumericCategory>();
            var service = new CategoryService();
            RestApiResult result = service.GetCategoriesByVariable(category.VariableID);
            Assert.AreEqual(1, result.Json.Count());
        }

        [Test]
        public void GetCategories_VariableHasOneCategory_ReturnsCorrectCategory()
        {
            ClarityDB.Instance = new FakeDB();
            var category = Presto.Persist<NumericCategory>();
            var service = new CategoryService();
            RestApiResult result = service.GetCategoriesByVariable(category.VariableID);
            var json = result.Json.First();
            Assert.AreEqual(category.ID, json.Value<long>("id"));
            Assert.AreEqual(category.Label, json.Value<string>("label"));
        }

        [Test]
        public void GetCategories_VariableHasManyCategories_ReturnsCollectionSortedByLabel()
        {
            ClarityDB.Instance = new FakeDB();
            var category1 = Presto.Persist<NumericCategory>(x => { x.NumericValue = 0; x.Label = "Male"; });
            var category2 = Presto.Persist<NumericCategory>(x => { x.NumericValue = 1; x.Label = "Female"; x.Variable = category1.Variable; x.Variable.Categories.Add(x); });
            var service = new CategoryService();

            RestApiResult result = service.GetCategoriesByVariable(category1.VariableID);
            Assert.AreEqual(category2.Label, result.Json.First().Value<string>("label"));
            Assert.AreEqual(category1.Label, result.Json.ElementAt(1).Value<string>("label"));
        }

        [Test]
        public void GetCategories_VariableDoesNotExist_ReturnsEmpty()
        {
            ClarityDB.Instance = new FakeDB();
            var variable = Presto.Persist<NumericVariable>();
            var service = new CategoryService();
            RestApiResult result = service.GetCategoriesByVariable(variable.ID);
            Assert.AreEqual(0, result.Json.Count());
        }
    }
}
