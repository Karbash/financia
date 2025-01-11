using Financia.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace Financia.Infrastructure.Security.Cryptography
{
    internal class BCrypt : IPasswordEncrypter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);
            return passwordHash;
        }
    }
}
