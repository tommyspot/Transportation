using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Api.Framework;
using NUnit.Framework;

namespace Transportation.UnitTests.Api.Framework
{
    public class PasswordHashFixture: UnitFixture
    {
        [Test]
        public void CreatePasswordHash_CreatesHash()
        {
            var passwordHash = new PasswordHash();
            var salt = passwordHash.CreateSalt();
            var hash = passwordHash.CreatePasswordHash("abc123@X", salt);
            Assert.Less(0, hash.Length);
        }

        [Test]
        public void CreateSalt_CreatesSalt()
        {
            var passwordHash = new PasswordHash();
            var salt = passwordHash.CreateSalt();
            Assert.Less(0, salt.Length);
        }
    }
}
