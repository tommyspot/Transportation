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
    public class FilterVariableGroupFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        VariableGroup firstVariableGroup;
        VariableGroup secondVariableGroup;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant;
            firstVariableGroup = Presto.Create<VariableGroup>();
            firstTenant.Datasets.Add(firstVariableGroup.Dataset);

            secondTenant = Presto.Persist<Tenant>();
            secondVariableGroup = Presto.Create<VariableGroup>();
            secondTenant.Datasets.Add(secondVariableGroup.Dataset);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersVariableGroup()
        {
            Assert.AreEqual(1, ClarityDB.Instance.VariableGroups.Count());
            Assert.AreEqual(firstVariableGroup.ID, ClarityDB.Instance.VariableGroups.First().ID);
        }

        [Test]
        public void CannotAddVariableGroupOfAnotherTenant()
        {
            var variableGroup = Presto.Create<VariableGroup>(x => { x.Dataset = secondTenant.Datasets.First(); x.DatasetID = x.Dataset.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.VariableGroups.Add(variableGroup));
        }

        [Test]
        public void CannotRemoveVariableGroupOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.VariableGroups.Remove(secondVariableGroup));
        }
    }
}
