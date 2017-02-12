using Epinion.Clarity.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class FilterServiceFixtures : UnitFixture
    {
        [Test]
        public void Get_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(FilterService), "Get", Role.Normal));
        }

        [Test]
        public void GetAll_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(FilterService), "GetAll", Role.Normal));
        }

        [Test]
        public void GetByCanvas_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(FilterService), "GetByMenuTile", Role.Normal));
        }

        [Test]
        public void Create_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(FilterService), "Create", Role.AccountOwner));
        }

        [Test]
        public void Update_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(FilterService), "Update", Role.AccountOwner));
        }

        [Test]
        public void Delete_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(FilterService), "Delete", Role.AccountOwner));
        }
    }
}
