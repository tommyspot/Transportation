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
    public class FilterGroupFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Group firstGroup;
        Group secondGroup;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant;
            firstGroup = Presto.Create<Group>();
            firstTenant.Groups.Add(firstGroup);

            secondTenant = Presto.Persist<Tenant>();
            secondGroup = Presto.Create<Group>();
            secondTenant.Groups.Add(secondGroup);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersGroup()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Groups.Count());
            Assert.AreEqual(firstGroup.ID, ClarityDB.Instance.Groups.First().ID);
        }

        [Test]
        public void CannotAddGroupOfAnotherTenant()
        {
            var group = Presto.Create<Group>(x => { x.Tenant = secondTenant; x.TenantID = secondTenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Groups.Add(group));
        }

        [Test]
        public void CannotRemoveGroupOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Groups.Remove(secondGroup));
        }
    }
}
