using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Api.Framework;

namespace Transportation.UnitTests
{
    public class FakePasswordHash : IPasswordHash
    {
        private bool createSaltCalled = false;

        public bool CreateSaltCalled
        {
            get { return createSaltCalled; }
        }

        private bool createPasswordHashCalled = false;

        public bool CreatePasswordHashCalled
        {
            get { return createPasswordHashCalled; }
        }

        public byte[] CreateSalt()
        {
            createSaltCalled = true;
            return new byte[] { Convert.ToByte("1") };
        }

        public string CreatePasswordHash(string password, byte[] salt)
        {
            createPasswordHashCalled = true;
            return password;
        }
    }
}
