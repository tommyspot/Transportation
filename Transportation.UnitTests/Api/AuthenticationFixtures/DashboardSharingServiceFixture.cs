using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class DashboardSharingServiceFixture : UnitFixture
    {
        [Test]
        public void DataServiceClass_HasAuthentcationAtributeWhichRoleIsSuper_ReturnTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckTypeHasAuthenticateAttributeAndRolePropertise(typeof(DashboardSharingService), Role.Super));
        }
    }
}
