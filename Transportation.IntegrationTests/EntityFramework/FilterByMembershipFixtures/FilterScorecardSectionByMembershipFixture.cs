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
    public class FilterScorecardSectionWithMembershipFixture : IntegrationFixture
    {
        [Test]
        public void FiltersOtherPersonScorecardSection() {
            var section = Presto.Persist<ScorecardSection>();
            var user = CreateUser(section.Scorecard.Tenant);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(0, ClarityDB.Instance.ScorecardSections.Count());
        }

        [Test]
        public void ReturnsOwnScorecardSections()
        {
            var section = Presto.Persist<ScorecardSection>();
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(section.Scorecard.MenuTile.Owner);
            
            Assert.AreEqual(1, ClarityDB.Instance.ScorecardSections.Count());
        }

        [Test]
        public void ReturnsSectionsOfScorecardThatUserIsAddedAsUser()
        {
            var section = Presto.Persist<ScorecardSection>();
            var user = CreateUser(section.Scorecard.Tenant);            
            section.Scorecard.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.ScorecardSections.Count());
        }

        [Test]
        public void ReturnsSectionsOfScorecardThatUserIsAddedAsGroup()
        {
            var section = Presto.Persist<ScorecardSection>();

            var group = CreateGroup(section.Scorecard.Tenant);
            section.Scorecard.MenuTile.Members.Add(new Membership { Group = group, GroupID = group.ID });

            var user = CreateUser(section.Scorecard.Tenant);
            group.Users.Add(user);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.ScorecardSections.Count());
        }

        [Test]
        public void Remove_UserIsNotMember_ThrowsException()
        {
            var section = Presto.Persist<ScorecardSection>();
            var user = CreateUser(section.Scorecard.Tenant);
            section.Scorecard.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });

            var nonMember = CreateUser(section.Scorecard.Tenant);

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(nonMember);

            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.ScorecardSections.Remove(section));
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
