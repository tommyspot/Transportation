using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web;
using Conjurer;
using Epinion.Clarity.Api.Framework;
using System.Reflection;
using System.Security.Principal;
using System.Web.Security;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class FeedbackServiceFixtures: UnitFixture
    {
        [Test]
        public void Create_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(FeedbackService), "Create", Role.Normal));           
        }     
    }
}
