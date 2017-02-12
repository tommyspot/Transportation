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
    public class FilterMenuTileFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        MenuTile firstMenuTile;
        MenuTile secondMenuTile;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstMenuTile = Presto.Persist<MenuTile>( x => x.Title = "First");
            firstTenant = firstMenuTile.Tenant;

            secondMenuTile = Presto.Persist<MenuTile>(x => x.Title = "Second");
            secondTenant = secondMenuTile.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstMenuTile.Owner);
        }

        [Test]
        public void FiltersMenuTile()
        {
            Assert.AreEqual(1, ClarityDB.Instance.MenuTiles.Count());
            Assert.AreEqual(firstMenuTile.ID, ClarityDB.Instance.MenuTiles.First().ID);
        }

        [Test]
        public void CannotAddMenuTileOfAnotherTenant()
        {
            var menuTile = Presto.Create<MenuTile>(x => { x.Tenant = secondTenant; x.TenantID = secondTenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.MenuTiles.Add(menuTile));
        }

        [Test]
        public void CannotRemoveMenuTileOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.MenuTiles.Remove(secondMenuTile));
        }

    }
}
