using System.Security.Cryptography;
using System.Text;

namespace EducationalPaperworkWeb.Service.Service.Helpers.Hashing
{
    public static class SecurityUtility
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
        public static string EncodeMessage(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(messageBytes);
        }

        public static string DecodeMessage(string encodedMessage)
        {
            var messageBytes = Convert.FromBase64String(encodedMessage);
            return Encoding.UTF8.GetString(messageBytes);
        }
    }
}
