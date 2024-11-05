using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
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

    public class DatabaseOperations
    {
        private readonly IConfiguration _configuration;
        public DatabaseOperations(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> InsertSadadNotificationAsync(dynamic sadadNotification)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                var query = @"
                INSERT INTO SadadNotificationInfo (Guid, BranchCode, BankId, DistrictCode, AccessChannel, SadadPaymentId, 
                                                    SadadNumber, PaymentMethod, BillNumber, BankPaymentId, 
                                                    PaymentDate, PaymentAmount, PaymentStatus, NotificationType)
                VALUES (@Guid, @BranchCode, @BankId, @DistrictCode, @AccessChannel, @SadadPaymentId, 
                        @SadadNumber, @PaymentMethod, @BillNumber, @BankPaymentId, 
                        @PaymentDate, @PaymentAmount, @PaymentStatus, @NotificationType)";

                var result = await connection.ExecuteAsync(query, (object)sadadNotification);
                return result > 0; // إرجاع true إذا تم الإدراج بنجاح
            }
        }
    }
}
