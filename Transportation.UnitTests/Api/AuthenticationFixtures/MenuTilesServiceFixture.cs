using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class MenuTilesServiceFixture : UnitFixture
    {
        [Test]
        public void Get_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "Get", Role.Normal));
        }

        [Test]
        public void GetAll_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "GetAll", Role.Normal));
        }

        [Test]
        public void GetSubMenusVisibleToUser_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "GetSubMenusVisibleToUser", Role.Super));
        }

        [Test]
        public void Create_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "Create", Role.AccountOwner));
        }

        [Test]
        public void Update_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "Update", Role.AccountOwner));
        }

        [Test]
        public void Delete_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "Delete", Role.AccountOwner));
        }

        [Test]
        public void CreateDashboards_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "CreateDashboards", Role.AccountOwner));
        }

        [Test]
        public void Clone_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(MenuTilesService), "Clone", Role.AccountOwner));
        }
    }
}
