using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using Transportation.Api;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Transportation.UnitTests.Core.UserFixtures
{
    public class User_FromJsonFixture: UnitFixture
    {
        private JObject json;
        private User user;

        [SetUp]
        public void SetUp()
        {
            user = Presto.Create<User>(x => {
                x.ID = 10; 
                x.Role = Role.Admin; 
                x.UserName = "user1"; 
                x.LastName = "Pan"; 
                x.FirstName = "Peter"; 
            });

            json = user.ToJson();            
        }

        [Test]
        public void ReturnsUserWithID()
        {
            var result = User.FromJson(json);
            Assert.AreEqual(user.ID, result.ID);
        }

        [Test]
        public void ReturnsUserWithFirstName()
        {
            var result = User.FromJson(json);
            Assert.AreEqual(user.FirstName, result.FirstName);
        }

        [Test]
        public void ReturnsUserWithLastName()
        {
            var result = User.FromJson(json);
            Assert.AreEqual(user.LastName, result.LastName);
        }

        [Test]
        public void ReturnsUserWithUserName()
        {
            var result = User.FromJson(json);
            Assert.AreEqual(user.UserName, result.UserName);
        }

        [Test]
        public void ReturnsUserWithRole()
        {
            var result = User.FromJson(json);
            Assert.AreEqual(user.Role, result.Role);
        }

        [Test]
        public void FromJson_InValidRole_ReturnsDefaultRole()
        {
            JToken role = JToken.Parse(@"'ABC'");
            json.SelectToken("role").Replace(role);
            var result = User.FromJson(json);
            Assert.AreEqual(Role.Admin, result.Role);
        }
    }
}
