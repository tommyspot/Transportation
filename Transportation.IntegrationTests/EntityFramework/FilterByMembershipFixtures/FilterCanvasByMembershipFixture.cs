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
    public class FilterCanvasByMembershipFixture : IntegrationFixture
    {
        [Test]
        public void FiltersOtherPersonCanvas() {
            var canvas = Presto.Persist<Canvas>();
            var user = CreateUser(canvas.Tenant);

            ClarityDB.CurrentTenant = canvas.Tenant;
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(0, ClarityDB.Instance.Canvases.Count());
        }

        [Test]
        public void ReturnsOwnCanvas()
        {
            var canvas = Presto.Persist<Canvas>();
            ClarityDB.CurrentTenant = canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(canvas);
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(canvas.MenuTile.Owner);
            Assert.AreEqual(1, ClarityDB.Instance.Canvases.Count());
        }

        [Test]
        public void ReturnsCanvasUserIsAddedAsUser()
        {
            var canvas = Presto.Persist<Canvas>();
            ClarityDB.CurrentTenant = canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(canvas);
            var user = CreateUser(canvas.Tenant);
            canvas.MenuTile.Members.Add(new Membership { User = user, UserID = user.ID });
            
            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.Canvases.Count());
        }

        [Test]
        public void ReturnsCanvasUserIsAddedAsGroup()
        {
            var canvas = Presto.Persist<Canvas>();
            ClarityDB.CurrentTenant = canvas.Tenant;
            ClarityDB.CurrentTenant.Canvases.Add(canvas);
            var group = CreateGroup(canvas.Tenant);
            canvas.MenuTile.Members.Add(new Membership { Group = group, GroupID = group.ID });

            var user = CreateUser(canvas.Tenant);
            group.Users.Add(user);

            try
            {
                ClarityDB.Instance.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex )
            {
                throw ex;
            }

            ClarityDB.CreateInstance(user);
            Assert.AreEqual(1, ClarityDB.Instance.Canvases.Count());
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
