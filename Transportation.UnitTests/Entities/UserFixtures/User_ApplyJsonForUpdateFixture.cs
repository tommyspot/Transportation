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
    public class User_ApplyJsonForUpdateFixture: UnitFixture
    {
        private JObject json;
        private User user;
        private User updatedUser;

        [SetUp]
        public void SetUp()
        {
            updatedUser = new User { 
                ID = 10, 
                FirstName = "New First Name", 
                LastName = "New Last Name", 
                Email = "newEmail@yahoo.com", 
                UserName = "New User Name", 
                Password = "New Password", 
                Role = Role.AccountOwner, 
                HomeMenuID = 10, 
                UseAccountNameAsHomeMenu= true,
                Disabled = true
            };

            json = updatedUser.ToJson();

            user = Presto.Create<User>();
        }

        [Test]
        public void SetsID()
        {
            user.ApplyJsonForUpdate(json);
            Assert.AreEqual(updatedUser.ID, user.ID);
        }

        [Test]
        public void SetsFirstName()
        {
            user.ApplyJsonForUpdate(json);
            Assert.AreEqual(updatedUser.FirstName, user.FirstName);
        }

        [Test]
        public void SetsLastName()
        {
            user.ApplyJsonForUpdate(json);
            Assert.AreEqual(updatedUser.LastName, user.LastName);
        }

        [Test]
        public void SetsEmail()
        {
            user.ApplyJsonForUpdate(json);
            Assert.AreEqual(updatedUser.Email, user.Email);
        }

        [Test]
        public void SetsHomeMenuID()
        {
            user.ApplyJsonForUpdate(json);
            Assert.AreEqual(updatedUser.HomeMenuID, user.HomeMenuID);
        }

        [Test]
        public void ToJson_HomeMenuIDIsNull_ReturnsJObjectWithEmptyHomeMenu()
        {
            var user = Presto.Create<User>(x => { x.HomeMenuID = null; });
            var jObject = user.ToJson();
            var homeMenuID = jObject.Value<string>("homeMenuId");
            Assert.AreEqual("", homeMenuID);
        }

        [Test]
        public void SetsUseAccountnameAsHomeMenu()
        {
            user.ApplyJsonForUpdate(json);
            Assert.AreEqual(updatedUser.UseAccountNameAsHomeMenu, user.UseAccountNameAsHomeMenu);
        }

        [Test]
        public void IsDisabled_SetsIsDisabledToTrue()
        {
            json["isDisabled"] = true;
            user.ApplyJsonForUpdate(json);
            Assert.IsTrue(user.Disabled);
        }

        [Test]
        public void IsNotDisabled_SetsIsDisabledToFalse()
        {
            json["isDisabled"] = false;
            user.ApplyJsonForUpdate(json);
            Assert.IsFalse(user.Disabled);
        }

        [Test]
        public void SetsRole()
        {
            user.ApplyJsonForUpdate(json);
            Assert.AreEqual(updatedUser.Role, user.Role);
        }
    }
}
