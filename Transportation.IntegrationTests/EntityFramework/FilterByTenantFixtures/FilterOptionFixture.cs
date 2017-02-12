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
    public class FilterOptionFixture : IntegrationFixture
    {
        Option firstOption;
        Option secondOption;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstOption = Presto.Persist<Option>();
            secondOption = Presto.Persist<Option>();

            var user = Presto.Create<User>();
            user.Tenant = firstOption.Tenant;
            user.TenantID = firstOption.TenantID;
            firstOption.Tenant.Users.Add(user);

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersOptions()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Options.Count());
            Assert.AreEqual(firstOption.ID, ClarityDB.Instance.Options.First().ID);
        }

        [Test]
        public void CannotAddOptionOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Options.Add(secondOption));
        }

        [Test]
        public void CannotRemoveOptionOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Options.Remove(secondOption));
        }

    }
}
