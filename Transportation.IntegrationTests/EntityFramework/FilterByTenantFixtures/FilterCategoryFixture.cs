using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using NUnit.Framework;

namespace Epinion.Clarity.IntegrationTests.EntityFramework
{
    public class FilterCategoryFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Category firstCategory;
        Category secondCategory;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant; 
            firstCategory = Presto.Create<NumericCategory>();
            firstTenant.Datasets.Add(firstCategory.Variable.Dataset);

            secondTenant = Presto.Persist<Tenant>();
            secondCategory = Presto.Create<NumericCategory>();
            secondTenant.Datasets.Add(secondCategory.Variable.Dataset);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersCategory()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Categories.Count());
            Assert.AreEqual(firstCategory.ID, ClarityDB.Instance.Categories.First().ID);
        }

        [Test]
        public void CannotAddCategoryOfAnotherTenant()
        {
            var category = Presto.Create<NumericCategory>(x =>
            {
                x.Variable = secondTenant.Datasets.First().Variables.First();
                x.VariableID = x.Variable.ID;
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Categories.Add(category));
        }

        [Test]
        public void CannotRemoveCategoryOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Categories.Remove(secondCategory));
        }
    }
}
