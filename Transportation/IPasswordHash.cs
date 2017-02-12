using System;
namespace Transportation
{
    public interface IPasswordHash
    {
        byte[] CreateSalt();
        string CreatePasswordHash(string password, byte[] salt);
    }
}
