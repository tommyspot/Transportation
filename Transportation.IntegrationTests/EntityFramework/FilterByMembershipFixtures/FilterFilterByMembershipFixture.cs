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
    public class FilterFilterWithMembershipFixture : IntegrationFixture
    {
        //[Test]
        //public void FiltersOtherPersonCanvasFilter() {
        //    var filter = Presto.Persist<Filter>();
        //    var user = CreateUser(filter.Tenant);

        //    ClarityDB.CurrentTenant = filter.Canvas.Tenant;
        //    ClarityDB.CurrentTenant.Canvases.Add(filter.Canvas);
        //    ClarityDB.Instance.SaveChanges();

        //    ClarityDB.CreateInstance(user);
        //    Assert.AreEqual(0, ClarityDB.Instance.Filters.Count());
        //}

        //[Test]
        //public void ReturnsOwnCanvasFilter()
        //{
        //    var filter = Presto.Persist<Filter>();
        //    ClarityDB.CurrentTenant = filter.Canvas.Tenant;
        //    ClarityDB.CurrentTenant.Canvases.Add(filter.Canvas);
        //    ClarityDB.Instance.SaveChanges();

        //    ClarityDB.CreateInstance(filter.Canvas.MenuTile.Owner);
        //    ClarityDB.CurrentTenant = filter.Canvas.Tenant;
        //    ClarityDB.CurrentTenant.Canvases.Add(filter.Canvas);
        //    Assert.AreEqual(1, ClarityDB.Instance.Filters.Count());
        //}

        //[Test]
        //public void ReturnsFilterOfCanvasThatUserIsAddedAsUser()
        //{
        //    var filter = Presto.Persist<Filter>();
        //    var user = CreateUser(filter.Tenant);            
        //    filter.Canvas.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });

        //    ClarityDB.CurrentTenant = filter.Canvas.Tenant;
        //    ClarityDB.CurrentTenant.Canvases.Add(filter.Canvas);
        //    ClarityDB.Instance.SaveChanges();

        //    ClarityDB.CreateInstance(user);
        //    Assert.AreEqual(1, ClarityDB.Instance.Filters.Count());
        //}

        //[Test]
        //public void ReturnsFilterOfCanvasThatUserIsAddedAsGroup()
        //{
        //    var filter = Presto.Persist<Filter>();

        //    var group = CreateGroup(filter.Tenant);
        //    filter.Canvas.MenuTile.Members.Add(new Membership { Group = group, GroupID = group.ID });

        //    var user = CreateUser(filter.Tenant);
        //    group.Users.Add(user);
        //    ClarityDB.CurrentTenant = filter.Canvas.Tenant;
        //    ClarityDB.CurrentTenant.Canvases.Add(filter.Canvas);
        //    ClarityDB.Instance.SaveChanges();

        //    ClarityDB.CreateInstance(user);
        //    Assert.AreEqual(1, ClarityDB.Instance.Filters.Count());
        //}

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
