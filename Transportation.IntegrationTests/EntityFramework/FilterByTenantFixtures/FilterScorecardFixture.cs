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
    public class FilterScorecardFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Scorecard firstScorecard;
        Scorecard secondScorecard;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstScorecard = Presto.Persist<Scorecard>();
            firstTenant = firstScorecard.Tenant;

            secondScorecard = Presto.Persist<Scorecard>();
            secondTenant = secondScorecard.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersScorecard()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Scorecards.Count());
            Assert.AreEqual(firstScorecard.ID, ClarityDB.Instance.Scorecards.First().ID);
        }

        [Test]
        public void CannotAddScorecardOfAnotherTenant()
        {
            var canvas = Presto.Create<Scorecard>(x => { x.Tenant = secondTenant; x.TenantID = x.Tenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Scorecards.Add(canvas));
        }

        [Test]
        public void CannotRemoveScorecardOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Scorecards.Remove(secondScorecard));
        }
    }
}
