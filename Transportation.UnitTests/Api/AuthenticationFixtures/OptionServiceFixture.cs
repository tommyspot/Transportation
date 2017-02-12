using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class OptionServiceFixture : UnitFixture
    {
        [Test]
        public void OptionService_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckTypeHasAuthenticateAttributeAndRolePropertise(typeof(OptionService), Role.Normal));
        }
    }
}
