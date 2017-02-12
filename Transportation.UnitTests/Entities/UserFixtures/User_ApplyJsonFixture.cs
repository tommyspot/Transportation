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
    public class User_ApplyJsonFixture: UnitFixture
    {
        private JObject json;
        private User user;
        private User updatedUser;

        [SetUp]
        public void SetUp()
        {
            updatedUser = new User
            {
                ID = 10,
                FirstName = "New First Name",
                LastName = "New Last Name",
                Email = "newEmail@yahoo.com",
                UserName = "New User Name",
                Password = "New Password",
                Role = Role.AccountOwner,
                HomeMenuID = 10,
                UseAccountNameAsHomeMenu = true,
                Disabled = true
            };
            json = updatedUser.ToJson();

            user = Presto.Create<User>();
        }

        [Test]
        public void SetsID()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.ID, user.ID);
        }

        [Test]
        public void SetsFirstName()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.FirstName, user.FirstName);
        }

        [Test]
        public void SetsLastName()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.LastName, user.LastName);
        }

        [Test]
        public void SetsEmail()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.Email, user.Email);
        }

        [Test]
        public void SetsUserName()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.UserName, user.UserName);
        }

        [Test]
        public void SetsPassword()
        {
            json["password"] = updatedUser.Password;
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.Password, user.Password);
        }

        [Test]
        public void SetsRole()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.Role, user.Role);
        }

        [Test]
        public void ApplyJson_HasGroups_SetsGroups()
        {
            var jsonString = "{ groups: [{ 'id' : '1', 'name' : 'Admin' }]}";
            var json = JObject.Parse(jsonString);
            user.ApplyJson(json, true);

            Assert.AreEqual(1, user.Groups.Count);
        }

        [Test]
        public void SetsHomeMenuID()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.HomeMenuID, user.HomeMenuID);
        }

        [Test]
        public void SetsUseAccountnameAsHomeMenu()
        {
            user.ApplyJson(json, false);
            Assert.AreEqual(updatedUser.UseAccountNameAsHomeMenu, user.UseAccountNameAsHomeMenu);
        }

        [Test]
        public void IsDisabled_SetsIsDisabledToTrue()
        {
            json["isDisabled"] = true;
            user.ApplyJson(json, false);
            Assert.IsTrue(user.Disabled);
        }

        [Test]
        public void IsNotDisabled_SetsIsDisabledToFalse()
        {
            json["isDisabled"] = false;
            user.ApplyJson(json, false);
            Assert.IsFalse(user.Disabled);
        }        

        [Test]
        public void HomeMenuIDIsEmpty_ClearsHomeMenuID()
        {
            json["homeMenuId"] = "";
            user.ApplyJson(json, false);
            Assert.AreEqual(null, user.HomeMenuID);
        }
    }
}
