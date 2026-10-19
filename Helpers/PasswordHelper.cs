using System.Security.Cryptography;
using System.Text;

namespace Anonim.Helpers
{
    /// <summary>
    /// Şifre hashleme ve doğrulama işlemleri için yardımcı sınıf.
    /// SHA-256 CPU-bound (bellekte) çalıştığı için basit ve senkron metotlar kullanılır.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Düz metin şifreyi SHA-256 ile hashler ve 64 karakterli onaltılık (hex) dize olarak döner.
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = SHA256.HashData(inputBytes);

            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        /// <summary>
        /// Girilen şifrenin hash'i ile kayıtlı hash'i karşılaştırır.
        /// </summary>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            string inputHash = HashPassword(inputPassword);
            return string.Equals(inputHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
