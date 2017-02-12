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
    public class FilterAssociatedVariableFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        AssociatedVariable firstAssociatedVariable;
        AssociatedVariable secondAssociatedVariable;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant;
            firstAssociatedVariable = Presto.Create<AssociatedVariable>();
            firstTenant.Datasets.Add(firstAssociatedVariable.VariableGroup.Dataset);

            secondTenant = Presto.Persist<Tenant>();
            secondAssociatedVariable = Presto.Create<AssociatedVariable>();
            secondTenant.Datasets.Add(secondAssociatedVariable.VariableGroup.Dataset);

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersAssociatedVariable()
        {
            Assert.AreEqual(1, ClarityDB.Instance.AssociatedVariables.Count());
            Assert.AreEqual(firstAssociatedVariable.ID, ClarityDB.Instance.AssociatedVariables.First().ID);
        }

        [Test]
        public void CannotAddAssociatedVariableOfAnotherTenant()
        {
            var associatedVariable = Presto.Create<AssociatedVariable>(x =>
            {
                x.VariableGroup = secondTenant.Datasets.First().VariableGroups.First();
                x.VariableGroupID = x.VariableGroup.ID;
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.AssociatedVariables.Add(associatedVariable));
        }

        [Test]
        public void CannotRemoveAssociatedVariableOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.AssociatedVariables.Remove(secondAssociatedVariable));
        }
    }
}
