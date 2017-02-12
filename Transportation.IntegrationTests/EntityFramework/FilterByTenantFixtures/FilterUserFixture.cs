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
    public class FilterUserFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        User firstUser;
        User secondUser;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstUser = Presto.Persist<User>();
            firstTenant = firstUser.Tenant;

            secondUser = Presto.Persist<User>();
            secondTenant = secondUser.Tenant;

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(firstUser);
        }

        [Test]
        public void FiltersUser()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Users.Count());
            Assert.AreEqual(firstUser.ID, ClarityDB.Instance.Users.First().ID);
        }

        [Test]
        public void CannotAddUserOfAnotherTenant()
        {
            var user = Presto.Create<User>(x => { x.Tenant = secondTenant; x.TenantID = secondTenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Users.Add(user));
        }

        [Test]
        public void CannotRemoveUserOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Users.Remove(secondUser));
        }
    }
}
