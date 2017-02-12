using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Transportation
{
    public class PasswordHash: IPasswordHash
    {
        public const int SALT_BYTE_SIZE = 32;
        public const int HASH_BYTE_SIZE = 32;
        public const int PBKDF2_ITERATIONS = 1000;

        public byte[] CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[64];
            rng.GetBytes(salt);

            return salt;
        }

        public string CreatePasswordHash(string password, byte[] salt)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
            return Convert.ToBase64String(rfc.GetBytes(64));
        }
    }
}
