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
    public class FilterDataBatchFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        DataBatch firstDataBatch;
        DataBatch secondDataBatch;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstDataBatch = Presto.Persist<DataBatch>();
            firstTenant = firstDataBatch.Tenant;

            secondDataBatch = Presto.Persist<DataBatch>();
            secondTenant = secondDataBatch.Tenant;

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(firstDataBatch.UploadedBy);
        }

        [Test]
        public void FiltersDataBatch()
        {
            Assert.AreEqual(1, ClarityDB.Instance.DataBatches.Count());
            Assert.AreEqual(firstDataBatch.ID, ClarityDB.Instance.DataBatches.First().ID);
        }

        [Test]
        public void CannotAddDataBatchOfAnotherTenant()
        {
            var dataBatch = Presto.Create<DataBatch>(x => { x.Tenant = secondTenant; x.TenantID = secondTenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.DataBatches.Add(dataBatch));
        }

        [Test]
        public void CannotRemoveDataBatchOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.DataBatches.Remove(secondDataBatch));
        }
    }
}
