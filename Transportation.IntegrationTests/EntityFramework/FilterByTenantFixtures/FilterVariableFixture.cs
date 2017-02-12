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
    public class FilterVariableFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Variable firstVariable;
        Variable secondVariable;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant;
            firstVariable = Presto.Create<NumericVariable>();
            firstTenant.Datasets.Add(firstVariable.Dataset);

            secondTenant = Presto.Persist<Tenant>();
            secondVariable = Presto.Create<NumericVariable>();
            secondTenant.Datasets.Add(secondVariable.Dataset);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersVariable()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Variables.Count());
            Assert.AreEqual(firstVariable.ID, ClarityDB.Instance.Variables.First().ID);
        }

        [Test]
        public void CannotAddVariableOfAnotherTenant()
        {
            var variable = Presto.Create<NumericVariable>(x => { x.Dataset = secondTenant.Datasets.First(); x.DatasetID = x.Dataset.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Variables.Add(variable));
        }

        [Test]
        public void CannotRemoveVariableOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Variables.Remove(secondVariable));
        }
    }
}
