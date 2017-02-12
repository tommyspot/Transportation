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
    public class FilterDatasetFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Dataset firstDataset;
        Dataset secondDataset;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant; 
            firstDataset = Presto.Create<Dataset>();
            firstTenant.Datasets.Add(firstDataset);

            secondTenant = Presto.Persist<Tenant>();
            secondDataset = Presto.Create<Dataset>();
            secondTenant.Datasets.Add(secondDataset);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersDataset()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Datasets.Count());
            Assert.AreEqual(firstDataset.ID, ClarityDB.Instance.Datasets.First().ID);
        }

        [Test]
        public void CannotAddDatasetOfAnotherTenant()
        {
            var dataset = Presto.Create<Dataset>(x => { x.Tenant = secondTenant; x.TenantID = secondTenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Datasets.Add(dataset));
        }

        [Test]
        public void CannotRemoveDatasetOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Datasets.Remove(secondDataset));
        }
    }
}
