using Epinion.Clarity.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class ImageServiceFixture : UnitFixture
    {
        [Test]
        public void Get_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ImageService), "Get", Role.Normal));
        }

        [Test]
        public void Upload_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ImageService), "Upload", Role.AccountOwner));
        }

        [Test]
        public void Remove_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ImageService), "Remove", Role.AccountOwner));
        }
    }
}
