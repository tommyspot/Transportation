using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using Epinion.Clarity.Api;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Epinion.Clarity.UnitTests.Core.UserFixtures
{
    public class User_ValidationFixture: UnitFixture
    {
        [Test]
        public void Validate_DefaultFactory_Passes()
        {
            var user = Presto.Create<User>();
            Assert.IsTrue(user.IsValid());
        }

        [Test]
        public void Validate_WithoutFirstName_Passes()
        {
            var user = Presto.Create<User>(x => x.FirstName = "");
            Assert.IsTrue(user.IsValid());
        }

        [Test]
        public void Validate_WithoutLastName_Passes()
        {
            var user = Presto.Create<User>(x => x.LastName = "");
            Assert.IsTrue(user.IsValid());
        }

        [Test]
        public void Validate_WithoutEmail_Fails()
        {
            var user = Presto.Create<User>(x => x.Email = "");
            Assert.That(user.IsInvalid());
        }

        [Test]
        [TestCase("abc")]
        [TestCase("123@yahoo")]        
        [TestCase("$$$@com.vn")]
        public void Validate_IncorrectEmailFormat_Fails(string email)
        {
            var user = Presto.Create<User>(x => x.Email = email);
            Assert.That(user.IsInvalid());
        }

        [Test]
        [TestCase("my.email@epinion.dk")]        
        [TestCase("myEmail_123@yahoo.com.vn")]
        [TestCase("MY.EMAIL-123@GOOGLE.COM")]
        public void Validate_CorrectEmailFormat_Passes(string email)
        {
            var user = Presto.Create<User>(x => x.Email = email);
            Assert.That(user.IsValid());
        }
    }
}
