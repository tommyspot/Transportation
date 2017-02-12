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
    public class FilterOpenAnswerDashboardFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        OpenAnswerDashboard firstOpenAnswerDashboard;
        OpenAnswerDashboard secondOpenAnswerDashboard;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstOpenAnswerDashboard = Presto.Persist<OpenAnswerDashboard>();
            firstTenant = firstOpenAnswerDashboard.Tenant;

            secondOpenAnswerDashboard = Presto.Persist<OpenAnswerDashboard>();
            secondTenant = secondOpenAnswerDashboard.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersOpenAnswerDashboard()
        {
            Assert.AreEqual(1, ClarityDB.Instance.OpenAnswerDashboards.Count());
            Assert.AreEqual(firstOpenAnswerDashboard.ID, ClarityDB.Instance.OpenAnswerDashboards.First().ID);
        }

        [Test]
        public void CannotAddOpenAnswerDashboardOfAnotherTenant()
        {
            var canvas = Presto.Create<OpenAnswerDashboard>(x => { x.Tenant = secondTenant; x.TenantID = x.Tenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.OpenAnswerDashboards.Add(canvas));
        }

        [Test]
        public void CannotRemoveOpenAnswerDashboardOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.OpenAnswerDashboards.Remove(secondOpenAnswerDashboard));
        }
    }
}
