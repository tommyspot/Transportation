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
    public class FilterDashboardWizardFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        DashboardWizard firstDashboardWizard;
        DashboardWizard secondDashboardWizard;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant;
            firstDashboardWizard = Presto.Create<DashboardWizard>(x => { x.TenantID = firstTenant.ID; x.Tenant = firstTenant; });
            ClarityDB.Instance.DashboardWizards.Add(firstDashboardWizard);

            secondTenant = Presto.Persist<Tenant>();
            secondDashboardWizard = Presto.Create<DashboardWizard>(x => { x.TenantID = secondTenant.ID; x.Tenant = secondTenant; });
            ClarityDB.Instance.DashboardWizards.Add(secondDashboardWizard);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersDashboardWizard()
        {
            Assert.AreEqual(1, ClarityDB.Instance.DashboardWizards.Count());
            Assert.AreEqual(firstDashboardWizard.ID, ClarityDB.Instance.DashboardWizards.First().ID);
        }

        [Test]
        public void CannotAddDashboardWizardOfAnotherTenant()
        {
            var wizard = Presto.Create<DashboardWizard>(x => { x.Tenant = secondTenant; x.TenantID = secondTenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.DashboardWizards.Add(wizard));
        }

        [Test]
        public void CannotRemoveDashboardWizardOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.DashboardWizards.Remove(secondDashboardWizard));
        }
    }
}
