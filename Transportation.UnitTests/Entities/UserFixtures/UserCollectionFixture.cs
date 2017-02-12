using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using NUnit.Framework;

namespace Transportation.UnitTests.Core
{
    public class UserCollectionFixture : UnitFixture
    {
        [Test]
        public void AddUser_AddsToCollection()
        {
            var users = new UserCollection();
            users.Add(new User());

            Assert.AreEqual(1, users.Count);
        }
    }
}
