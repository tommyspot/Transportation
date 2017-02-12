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
    public class FilterScorecardComementFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        ScorecardComment firstScorecardComment;
        ScorecardComment secondScorecardComment;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstScorecardComment = Presto.Persist<ScorecardComment>();
            firstTenant = firstScorecardComment.Scorecard.Tenant;

            secondScorecardComment = Presto.Persist<ScorecardComment>();
            secondTenant = secondScorecardComment.Scorecard.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersScorecardComment()
        {
            Assert.AreEqual(1, ClarityDB.Instance.ScorecardComments.Count());
            Assert.AreEqual(firstScorecardComment.ID, ClarityDB.Instance.ScorecardComments.First().ID);
        }

        [Test]
        public void CannotAddScorecardCommentOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.ScorecardComments.Add(secondScorecardComment));
        }

        [Test]
        public void CannotRemoveScorecardCommentOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.ScorecardComments.Remove(secondScorecardComment));
        }
    }
}
