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
    public class FilterScorecardSectionFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        ScorecardSection firstScorecardSection;
        ScorecardSection secondScorecardSection;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstScorecardSection = Presto.Persist<ScorecardSection>();
            firstTenant = firstScorecardSection.Scorecard.Tenant;

            secondScorecardSection = Presto.Persist<ScorecardSection>();
            secondTenant = secondScorecardSection.Scorecard.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersScorecardSection()
        {
            Assert.AreEqual(1, ClarityDB.Instance.ScorecardSections.Count());
            Assert.AreEqual(firstScorecardSection.ID, ClarityDB.Instance.ScorecardSections.First().ID);
        }

        [Test]
        public void CannotAddScorecardSectionOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.ScorecardSections.Add(secondScorecardSection));
        }

        [Test]
        public void CannotRemoveScorecardSectionOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.ScorecardSections.Remove(secondScorecardSection));
        }
    }
}
