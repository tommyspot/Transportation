using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Transportation.UnitTests.Core.UserFixtures
{
    public class User_ToJsonFixture : UnitFixture
    {
        private User user;

        [SetUp]
        public void SetUp()
        {
            user = Presto.Create<User>();
        }

        [Test]
        public void ReturnsJsonWithID()
        {
            var json = user.ToJson();
            Assert.AreEqual(user.ID, json.Value<long>("id"));
        }

        [Test]
        public void ReturnsJsonWithFirstName()
        {
            var json = user.ToJson();
            Assert.AreEqual(user.FirstName, json.Value<string>("firstName"));
        }

        [Test]
        public void FirstNameWithSingleQuote_ReturnsJsonWithFirstName()
        {
            user.FirstName = "abc's";
            var json = user.ToJson();
            Assert.AreEqual(user.FirstName, json.Value<string>("firstName"));
        }

        [Test]
        public void ReturnsJsonWithLastName()
        {
            var json = user.ToJson();
            Assert.AreEqual(user.LastName, json.Value<string>("lastName"));
        }

        [Test]
        public void LastNameWithSingleQuote_ReturnsJsonWithLastName()
        {
            user.LastName = "abc's";
            var json = user.ToJson();
            Assert.AreEqual(user.LastName, json.Value<string>("lastName"));
        }

        [Test]
        public void ReturnsJsonWithUserName()
        {
            var json = user.ToJson();
            Assert.AreEqual(user.UserName, json.Value<string>("username"));
        }

        [Test]
        public void UserNameWithSingleQuote_ReturnsJsonWithUserName()
        {
            user.UserName = "abc's";
            var json = user.ToJson();
            Assert.AreEqual(user.UserName, json.Value<string>("username"));
        }

        [Test]
        public void ReturnsJObjectWithRole()
        {
            var json = user.ToJson();
            Assert.AreEqual(user.Role.ToString(), json.Value<string>("role"));
        }
    }
}
