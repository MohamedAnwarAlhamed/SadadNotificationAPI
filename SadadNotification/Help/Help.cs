using System.Security.Cryptography;

namespace SadadNotification.Help
{
    public static class Helping
    {
        public static bool VerifySignature(byte[] data, byte[] signature, RSA publicKey)
        {
            using (var sha256 = SHA256.Create())
            {
                // حساب هاش البيانات الأصلية
                byte[] hash = sha256.ComputeHash(data);

                // التحقق من التوقيع
                return publicKey.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
