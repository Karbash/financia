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

        public bool Verify(string password, string passwordHash)
        {
            Console.WriteLine("Senha fornecida: " + password);
            Console.WriteLine("Hash armazenado: " + passwordHash);

            bool match = BC.Verify(password, passwordHash);
            Console.WriteLine("Senha corresponde ao hash: " + match);

            return match;
        }    
            
    }
}
