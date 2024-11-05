using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SadadNotification.Help;
using SadadNotification.Model;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;


namespace SadadNotification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private DatabaseOperations _databaseOperations;
        public NotificationsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseOperations = new DatabaseOperations(_configuration);
        }
        [HttpPost("PaymentNotification")]
        public async Task<IActionResult> ReceivePaymentNotificationAsync([FromBody] NotificationData request)
        {

            // قراءة التوقيع من رأس الطلب
            if (!Request.Headers.TryGetValue("signature", out var signature))
            {
                return Unauthorized(new { status = 401, message = "Signature is missing." });
            }
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            string certificatePath = projectDirectory + "/Files/efaa_public_64.pem";

            X509Certificate2 certificate = new X509Certificate2(certificatePath);

            var publicKey = certificate.GetRSAPublicKey();

            string originalJson = JsonConvert.SerializeObject(request);

            byte[] originalData = Encoding.UTF8.GetBytes(originalJson);

            bool isValid = Helping.VerifySignature(originalData, Convert.FromBase64String(signature), publicKey);

            if (!isValid)
            {


                return Unauthorized(new { status = 401, message = "Invalid Signature." });

            }
            else
            {
                var sadadNotification = new
                {
                    Guid = Guid.NewGuid(), // توليد GUID جديد
                    BranchCode = request.branchCode,
                    BankId = request.bankId,
                    DistrictCode = request.districtCode,
                    AccessChannel = request.accessChannel,
                    SadadPaymentId = request.sadadPaymentId,
                    SadadNumber = request.sadadNumber,
                    PaymentMethod = request.paymentMethod,
                    BillNumber = request.billNumber,
                    BankPaymentId = request.bankPaymentId,
                    PaymentDate = request.paymentDate,
                    PaymentAmount = request.paymentAmount,
                    PaymentStatus = request.paymentStatus,
                    NotificationType = 1
                };

                var result = await _databaseOperations.InsertSadadNotificationAsync(sadadNotification);
                if (result)
                {
                    return Ok(new { status = 200, message = "Operation Done Successfully" });
                }
                else
                {
                    return BadRequest(new { status = 400, message = "Failed to insert data" });
                }
            }


        }

        [HttpPost("SettlementNotification")]
        public async Task<IActionResult> ReceiveSettlementNotificationAsync([FromBody] NotificationData request)
        {
            // قراءة التوقيع من رأس الطلب
            if (!Request.Headers.TryGetValue("signature", out var signature))
            {
                return Unauthorized(new { status = 401, message = "Signature is missing." });
            }
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            string certificatePath = projectDirectory + "/Files/efaa_public_64.pem";

            X509Certificate2 certificate = new X509Certificate2(certificatePath);

            var publicKey = certificate.GetRSAPublicKey();

            string originalJson = JsonConvert.SerializeObject(request);

            byte[] originalData = Encoding.UTF8.GetBytes(originalJson);

            bool isValid = Helping.VerifySignature(originalData, Convert.FromBase64String(signature), publicKey);

            if (!isValid)
            {
                return Unauthorized(new { status = 401, message = "Invalid Signature." });
            }
            else
            {
                var sadadNotification = new
                {
                    Guid = Guid.NewGuid(), // توليد GUID جديد
                    BranchCode = request.branchCode,
                    BankId = request.bankId,
                    DistrictCode = request.districtCode,
                    AccessChannel = request.accessChannel,
                    SadadPaymentId = request.sadadPaymentId,
                    SadadNumber = request.sadadNumber,
                    PaymentMethod = request.paymentMethod,
                    BillNumber = request.billNumber,
                    BankPaymentId = request.bankPaymentId,
                    PaymentDate = request.paymentDate,
                    PaymentAmount = request.paymentAmount,
                    PaymentStatus = request.paymentStatus,
                    NotificationType = 2
                };

                var result = await _databaseOperations.InsertSadadNotificationAsync(sadadNotification);
                if (result)
                {
                    return Ok(new { status = 200, message = "Operation Done Successfully" });
                }
                else
                {
                    return BadRequest(new { status = 400, message = "Failed to insert data" });
                }
            }

        }
    }
}
    
