using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace NPU.Utils.EncriptionServices
{
    public static class PasswordEncription
    {
        private static readonly byte[] salt = Encoding.Unicode.GetBytes("I am error");
        private static readonly byte[] storesalt = Encoding.Unicode.GetBytes("metoo");

        public static string EncryptString(this string input)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                        password: input,
                                                        salt: salt,
                                                        prf: KeyDerivationPrf.HMACSHA256,
                                                        iterationCount: 100000,
                                                        numBytesRequested: 256 / 8));
        }

        public static string EncryptToStoredString(this string input)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                        password: input,
                                                        salt: storesalt,
                                                        prf: KeyDerivationPrf.HMACSHA256,
                                                        iterationCount: 100000,
                                                        numBytesRequested: 256 / 8));
        }
    }
}