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
    public class FilterScorecardByMembershipFixture : IntegrationFixture
    {
        [Test]
        public void FiltersOtherPersonScorecard() {
            var scorecard = Presto.Persist<Scorecard>();
            var user = CreateUser(scorecard.Tenant);
            scorecard.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });

            var nonMember = CreateUser(scorecard.Tenant);

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(nonMember);

            Assert.AreEqual(0, ClarityDB.Instance.Scorecards.Count());
        }

        [Test]
        public void ReturnsOwnScorecard()
        {
            var scorecard = Presto.Persist<Scorecard>();
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(scorecard.MenuTile.Owner);
            Assert.AreEqual(1, ClarityDB.Instance.Scorecards.Count());
        }

        [Test]
        public void ReturnsScorecardUserIsAddedAsUser()
        {
            var scorecard = Presto.Persist<Scorecard>();
            var user = CreateUser(scorecard.Tenant);
            scorecard.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });
            
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.Scorecards.Count());
        }

        [Test]
        public void ReturnsScorecardUserIsAddedAsGroup()
        {
            var scorecard = Presto.Persist<Scorecard>();
            var group = CreateGroup(scorecard.Tenant);
            scorecard.MenuTile.Members.Add(new Membership { Group = group, GroupID = group.ID });

            var user = CreateUser(scorecard.Tenant);
            group.Users.Add(user);

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(user);

            Assert.AreEqual(1, ClarityDB.Instance.Scorecards.Count());
        }

        [Test]
        public void Remove_UserIsNotMember_ThrowsException()
        {
            var scorecard = Presto.Persist<Scorecard>();
            var user = CreateUser(scorecard.Tenant);
            scorecard.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });

            var nonMember = CreateUser(scorecard.Tenant);

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(nonMember);

            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Scorecards.Remove(scorecard));
        }

        private static User CreateUser(Tenant tenant)
        {
            var user = Presto.Create<User>();
            user.Tenant = tenant;
            user.TenantID = tenant.ID;
            ClarityDB.Instance.Users.Add(user);

            return user;
        }

        private static Group CreateGroup(Tenant tenant)
        {
            var group = Presto.Create<Group>();
            group.TenantID = tenant.ID;
            group.Tenant = tenant;

            ClarityDB.Instance.Groups.Add(group);
            return group;
        }
    }
}
