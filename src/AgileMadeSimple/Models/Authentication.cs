using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace AgileMadeSimple.Models
{
    public static class Authentication
    {
        public static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        public static string CalculatePasswordHash(string password, string salt)
        {
            string saltAndPwd = String.Concat(password, salt);
            byte[] saltedPassAsByte = Encoding.ASCII.GetBytes(saltAndPwd);
            SHA512 sha512 = SHA512.Create();
            string hashedPass = Encoding.ASCII.GetString(sha512.ComputeHash(saltedPassAsByte));
            return hashedPass;
        }

        public static bool ComparePassword(string password, int userId)
        {
            //Get hashed password and salt
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                var saltAndHash =
                    (from u in context.User
                     where u.UserID == userId
                     select new
                     {
                         Hash = u.Password,
                         Salt = u.Salt
                     }).FirstOrDefault();

                if(saltAndHash != null)
                {
                    if(CalculatePasswordHash(password, saltAndHash.Salt) == saltAndHash.Hash)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Inputted User ID does not exist");
                }

            }
        }
    }
}
