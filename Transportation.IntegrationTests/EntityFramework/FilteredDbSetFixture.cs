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
    public class FilteredDbSetFixture: IntegrationFixture
    {
        Expression<Func<User, bool>> filter;
        FilteredDbSet<User> users;
        Tenant firstTenant;
        Tenant secondTenant;
        User firstUser;
        User secondUser;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstTenant = Presto.Persist<Tenant>();
            firstUser = Presto.Create<User>();
            firstTenant.Users.Add(firstUser);

            secondTenant = Presto.Persist<Tenant>();
            secondUser = Presto.Create<User>();
            secondTenant.Users.Add(secondUser);

            ClarityDB.Instance.SaveChanges();

            filter = x => x.TenantID == firstTenant.ID;
            users = new FilteredDbSet<User>((ClarityDB)ClarityDB.Instance, filter);
        }

        [Test]
        public void Constructor_SetsFilter()
        {
            Assert.AreEqual(filter, users.Filter);
        }

        [Test]
        public void MatchesFilter_Matches_ReturnsTrue()
        {
            Assert.IsTrue(users.MatchesFilter(firstUser));
        }

        [Test]
        public void MatchesFilter_DoesNotMatch_ReturnsFalse()
        {
            Assert.IsFalse(users.MatchesFilter(secondUser));
        }

        [Test]
        public void ThrowIfEntityDoesNotMatchFilter_Matches_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => users.ThrowIfEntityDoesNotMatchFilter(firstUser));
        }

        [Test]
        public void ThrowIfEntityDoesNotMatchFilter_DoesNotMatch_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => users.ThrowIfEntityDoesNotMatchFilter(secondUser));
        }

        [Test]
        public void Add_EntityDoesNotMatchFilter_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => users.Add(secondUser));
        }

        [Test]
        public void Add_EntityMatchesFilter_AddsEntityToCollection()
        {
            var user = Presto.Create<User>(x =>
            {
                x.UserName = "User 2";
                x.TenantID = firstTenant.ID;
                x.Tenant = firstTenant;
            }); 
            
            var actual = users.Add(user);

            ClarityDB.Instance.SaveChanges();

            Assert.AreSame(user, actual);
            Assert.AreEqual(2, users.Count());
        }

        [Test]
        public void Attach_EntityDoesNotMatchFilter_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => users.Attach(secondUser));
        }

        [Test]
        public void Attach_EntityMatchesFilter_AttachesEntityToCollection()
        {
            var actual = users.Attach(firstUser);

            ClarityDB.Instance.SaveChanges();

            Assert.AreSame(firstUser, actual);
            Assert.AreEqual(1, users.Count());
        }

        [Test]
        public void Create_InitializesEntity()
        {
            var actual = users.Create();
            Assert.IsNotNull(actual);
        }

        [Test]
        public void GenericCreate_InitializesEntity()
        {
            var actual = users.Create<User>();
            Assert.IsNotNull(actual);
        }

        [Test]
        public void Find_EntityDoesNotMatch_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => users.Find(secondUser.ID));
        }

        [Test]
        public void Find_EntityMatches_ReturnsEntity()
        {
            var actual = users.Find(firstUser.ID);
            Assert.AreSame(firstUser, actual);
        }

        [Test]
        public void Remove_EntityDoesNotMatch_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => users.Remove(secondUser));
        }

        [Test]
        public void Remove_EntityMatches_RemovesEntity()
        {
            users.Remove(firstUser);
            ClarityDB.Instance.SaveChanges();
            Assert.AreEqual(0, users.Count());
        }

        [Test]
        public void GenericGetEnumerator_AppliesFilter()
        {
            foreach (var u in users)
            {
                Assert.AreEqual(firstTenant.ID, u.TenantID);
            }
        }
    }
}
