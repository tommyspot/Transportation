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
    public class FilterMenuTileWithMembershipFixture : IntegrationFixture
    {
        [Test]
        public void FiltersOtherPersonMenuTile() {
            var menuTile = Presto.Persist<MenuTile>();
            var user = CreateUser(menuTile.Tenant);
            
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(0, ClarityDB.Instance.MenuTiles.Count());
        }

        [Test]
        public void ReturnsOwnMenu()
        {
            var menuTile = Presto.Persist<MenuTile>();
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(menuTile.Owner);
            Assert.AreEqual(1, ClarityDB.Instance.MenuTiles.Count());
        }

        [Test]
        public void ReturnsMenuTileUserIsAddedAsUser()
        {
            var menuTile = Presto.Persist<MenuTile>();
            var user = CreateUser(menuTile.Tenant);            
            menuTile.Members.Add(new Membership { User = user, UserID = user.ID });
            
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.MenuTiles.Count());
        }

        [Test]
        public void ReturnsMenuTileUserIsAddedAsGroup()
        {
            var menuTile = Presto.Persist<MenuTile>();

            var group = CreateGroup(menuTile.Tenant);
            menuTile.Members.Add(new Membership { Group = group, GroupID = group.ID });

            var user = CreateUser(menuTile.Tenant);
            group.Users.Add(user);                   
            
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.MenuTiles.Count());
        }

        private static User CreateUser(Tenant tenant)
        {
            var user = Presto.Create<User>(x => x.Role = Role.Super);
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
