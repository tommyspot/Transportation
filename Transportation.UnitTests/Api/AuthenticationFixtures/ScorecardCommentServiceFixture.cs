using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Epinion.Clarity.Api;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class ScorecardCommentServiceFixture : UnitFixture
    {
        [Test]
        public void GetByScorecard_HasAuthentcationAtributeWhichRoleIsNormal_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ScorecardCommentService), "GetByScorecard", Role.Normal));
        }

        [Test]
        public void Create_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ScorecardCommentService), "Create", Role.Super));
        }

        [Test]
        public void Update_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ScorecardCommentService), "Update", Role.Super));
        }
        
        [Test]
        public void Delete_HasAuthentcationAtributeWhichRoleIsSuper_ReturnsTrue()
        {
            Assert.IsTrue(AuthenticationFixtureHelper.CheckMethodHasAuthenticateAttributeAndRolePropertise(typeof(ScorecardCommentService), "Delete", Role.Super));
        }
    }
}