using Epinion.Clarity.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class SpssServiceFixture : UnitFixture
    {
        [Test]
        public void ImportData_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(SpssService), "ImportData", Role.AccountOwner));
        }

        [Test]
        public void ImportData_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsFalse()
        {
            Assert.IsFalse(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(SpssService), "ImportData", Role.Normal));
        }

        [Test]
        public void GetVariables_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(SpssService), "GetVariables", Role.AccountOwner));
        }

        [Test]
        public void GetVariables_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsFalse()
        {
            Assert.IsFalse(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(SpssService), "GetVariables", Role.Normal));
        }

        [Test]
        public void AutoImport_HasAuthentcationAtributeWhichRoleIsSystemAdmin_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(SpssService), "AutoImport", Role.SystemAdmin));
        }

        [Test]
        public void AutoImport_HasAuthentcationAtributeWhichRoleIsSuperUser_ReturnsFalse()
        {
            Assert.IsFalse(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(SpssService), "AutoImport", Role.AccountOwner));
        }

        [Test]
        public void AutoImport_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsFalse()
        {
            Assert.IsFalse(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(SpssService), "AutoImport", Role.Normal));
        }


    }
}
