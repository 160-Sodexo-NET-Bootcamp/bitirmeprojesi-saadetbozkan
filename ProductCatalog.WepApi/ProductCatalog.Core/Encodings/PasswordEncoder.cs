using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ProductCatalog.Data.DataModels.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Core.Encodings
{
    public class PasswordEncoder
    {
        public static string Encoder(string password, string value1)
        {
            var salt = Encoding.UTF8.GetBytes(value1);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
