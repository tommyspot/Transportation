using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class CategoryServiceFixture : UnitFixture
    {
        [Test]
        public void Get_HasAuthentcationAtributeWhichRoleIsNormal_ReturnTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(CategoryService), "GetCategoriesByVariable", Role.Normal));
        }

        [Test]
        public void Update_HasAuthentcationAtributeWhichRoleIsSuper_ReturnTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(CategoryService), "UpdateCategory", Role.AccountOwner));
        }
    }
}
