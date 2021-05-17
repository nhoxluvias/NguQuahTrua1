using System.Security.Cryptography;
using System.Text;

namespace Data.Common.Hash
{
    public class PBKDF2_Hash
    {
        public static string Hash(string input, string salt, int length)
        {

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, Encoding.ASCII.GetBytes(salt), iterations: 5000);
            StringBuilder hashSb = new StringBuilder();
            byte[] hash = pbkdf2.GetBytes(length);
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("x2"));
            }
            return hashSb.ToString();
        }
    }
}
