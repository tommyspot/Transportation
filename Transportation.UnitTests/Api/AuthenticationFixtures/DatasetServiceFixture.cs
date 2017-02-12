using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class DatasetServiceFixture : UnitFixture
    {
        [Test]
        public void GetAll_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "GetAll", Role.Normal));
        }

        [Test]
        public void Get_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "Get", Role.Normal));
        }

        [Test]
        public void Create_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "Create", Role.AccountOwner));
        }

        [Test]
        public void Update_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "Update", Role.AccountOwner));
        }

        [Test]
        public void Delete_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "Delete", Role.AccountOwner));
        }

        [Test]
        public void ClearData_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "ClearData", Role.AccountOwner));
        }

        [Test]
        public void GetByCanvas_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "GetByCanvas", Role.Normal));
        }

        [Test]
        public void CopyDataset_HasAuthentcationAtributeWhichRoleIsAccountOwnerReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(DatasetService), "Copy", Role.AccountOwner));
        }

    }
}
