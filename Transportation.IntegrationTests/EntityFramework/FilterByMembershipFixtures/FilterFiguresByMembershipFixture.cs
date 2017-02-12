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
    public class FilterFigureWithMembershipFixture : IntegrationFixture
    {
        [Test]
        public void FiltersOtherPersonCanvasFigure() {
            var figure = Presto.Persist<Figure>();
            var user = CreateUser(figure.Page.Canvas.Tenant);

            ClarityDB.CurrentTenant = figure.Page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(figure.Page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(0, ClarityDB.Instance.Figures.Count());
        }

        [Test]
        public void ReturnsOwnCanvasFigures()
        {
            var figure = Presto.Persist<Figure>();
            ClarityDB.CurrentTenant = figure.Page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(figure.Page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(figure.Page.Canvas.MenuTile.Owner);
            
            ClarityDB.Instance.SaveChanges();
            Assert.AreEqual(1, ClarityDB.Instance.Figures.Count());
        }

        [Test]
        public void ReturnsFiguresOfCanvasThatUserIsAddedAsUser()
        {
            var figure = Presto.Persist<Figure>();
            var user = CreateUser(figure.Page.Canvas.Tenant);            
            figure.Page.Canvas.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });

            ClarityDB.CurrentTenant = figure.Page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(figure.Page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.Figures.Count());
        }

        [Test]
        public void ReturnsFiguresOfCanvasThatUserIsAddedAsGroup()
        {
            var figure = Presto.Persist<Figure>();

            var group = CreateGroup(figure.Page.Canvas.Tenant);
            figure.Page.Canvas.MenuTile.Members.Add(new Membership { Group = group, GroupID = group.ID });

            var user = CreateUser(figure.Page.Canvas.Tenant);
            group.Users.Add(user);

            ClarityDB.CurrentTenant = figure.Page.Canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(figure.Page.Canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.Figures.Count());
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
