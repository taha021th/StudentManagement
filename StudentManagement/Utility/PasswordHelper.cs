using System.Security.Cryptography;
using System.Text;

namespace StudentManagement.Utility
{
    public class PasswordHelper
    {
        public static string EncodePasswordMd5(string pass)
        {
            byte[] originalBytes;
            byte[] encodedBytes;

            // استفاده از MD5.Create() به جای MD5CryptoServiceProvider
            using (var md5 = MD5.Create())
            {
                originalBytes = Encoding.Default.GetBytes(pass);
                encodedBytes = md5.ComputeHash(originalBytes);
            }

            
            return BitConverter.ToString(encodedBytes).Replace("-", "").ToLower();
        }

        public static string EncodeProSecurity(string pass)
        {
            var first = EncodePasswordMd5(pass);
            var second = EncodePasswordMd5(first);
            return EncodePasswordMd5(second);
        }
    }
}
