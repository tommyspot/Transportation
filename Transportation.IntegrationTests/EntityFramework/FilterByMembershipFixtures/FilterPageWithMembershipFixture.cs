using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using NUnit.Framework;

namespace Epinion.Clarity.IntegrationTests.EntityFramework
{
    public class FilterPageWithMembershipFixture : IntegrationFixture
    {
        [Test]
        public void FiltersOtherPersonCanvasPage()
        {
            var page = Presto.Persist<Page>();
            var user = CreateUser(page.Canvas.Tenant);

            ClarityDB.CurrentTenant = page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(0, ClarityDB.Instance.Pages.Count());
        }

        [Test]
        public void ReturnsOwnCanvasPages()
        {
            var page = Presto.Persist<Page>();
            ClarityDB.CurrentTenant = page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(page.Canvas.MenuTile.Owner);

            ClarityDB.Instance.SaveChanges();
            Assert.AreEqual(1, ClarityDB.Instance.Pages.Count());
        }

        [Test]
        public void ReturnsPagesOfCanvasThatUserIsAddedAsUser()
        {
            var page = Presto.Persist<Page>();
            var user = CreateUser(page.Canvas.Tenant);
            page.Canvas.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });

            ClarityDB.CurrentTenant = page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.Pages.Count());
        }

        [Test]
        public void ReturnsPagesOfCanvasThatUserIsAddedAsGroup()
        {
            var page = Presto.Persist<Page>();

            var group = CreateGroup(page.Canvas.Tenant);
            page.Canvas.MenuTile.Members.Add(new Membership { Group = group, GroupID = group.ID });

            var user = CreateUser(page.Canvas.Tenant);
            group.Users.Add(user);

            ClarityDB.CurrentTenant = page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.Pages.Count());
        }

        private static User CreateUser(Tenant tenant)
        {
            var user = Presto.Create<User>();
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
