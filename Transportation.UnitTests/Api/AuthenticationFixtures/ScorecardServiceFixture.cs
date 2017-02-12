using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class ScorecardServiceFixture : UnitFixture
    {
        [Test]
        public void Get_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ScorecardService), "Get", Role.Normal));
        }
    }
}

