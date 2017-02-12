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
    public class FilterFitlerFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Filter firstFilter;
        Filter secondFilter;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            ClarityDB.CurrentTenant = Presto.Persist<Tenant>();
            firstFilter = Presto.Persist<Filter>();
            firstTenant = firstFilter.MenuTile.Tenant;

            ClarityDB.CurrentTenant = firstFilter.MenuTile.Tenant;
            ClarityDB.CurrentTenant.MenuTiles.Add(firstFilter.MenuTile);
            ClarityDB.Instance.SaveChanges();

            secondFilter = Presto.Persist<Filter>();
            secondTenant = secondFilter.MenuTile.Tenant;
            secondTenant.MenuTiles.Add(secondFilter.MenuTile);
            ClarityDB.CurrentTenant.MenuTiles.Add(secondFilter.MenuTile);
            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersFilter()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Filters.Count());
            Assert.AreEqual(firstFilter.ID, ClarityDB.Instance.Filters.First().ID);
        }

        [Test]
        public void CannotAddFilterOfAnotherTenant()
        {
            var filter = Presto.Create<Filter>(x => { x.MenuTile = secondTenant.MenuTiles.First(); x.MenuTileID = x.MenuTile.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Filters.Add(filter));
        }

        [Test]
        public void CannotRemoveFilterOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Filters.Remove(secondFilter));
        }
    }
}
