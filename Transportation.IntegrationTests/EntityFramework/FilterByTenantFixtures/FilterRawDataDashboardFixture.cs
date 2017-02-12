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
    public class FilterRawDataDashboardFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        RawDataDashboard firstDashboard;
        RawDataDashboard secondDashboard;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstDashboard = Presto.Persist<RawDataDashboard>();
            firstTenant = firstDashboard.Tenant;

            secondDashboard = Presto.Persist<RawDataDashboard>();
            secondTenant = secondDashboard.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersDashboard()
        {
            Assert.AreEqual(1, ClarityDB.Instance.RawDataDashboards.Count());
            Assert.AreEqual(firstDashboard.ID, ClarityDB.Instance.RawDataDashboards.First().ID);
        }

        [Test]
        public void CannotAddDashboardOfAnotherTenant()
        {
            var canvas = Presto.Create<RawDataDashboard>(x => { x.Tenant = secondTenant; x.TenantID = x.Tenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.RawDataDashboards.Add(canvas));
        }

        [Test]
        public void CannotRemoveDashboardOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.RawDataDashboards.Remove(secondDashboard));
        }
    }
}
