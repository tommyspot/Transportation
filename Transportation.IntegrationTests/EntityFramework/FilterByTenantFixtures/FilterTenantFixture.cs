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
    public class FilterTenantFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant;
            secondTenant = Presto.Persist<Tenant>();
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersTenant()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Tenants.Count());
            Assert.AreEqual(firstTenant.ID, ClarityDB.CurrentTenant.ID);
        }

        [Test]
        public void CannotAddMoreTenant()
        {
            var tenant = Presto.Create<Tenant>();
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Tenants.Add(tenant));
        }
    }
}
