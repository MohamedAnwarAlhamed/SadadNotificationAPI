using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SadadNotification.Help;
using SadadNotification.Model;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;


namespace SadadNotification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase 
    {
        

        [HttpPost("PaymentNotification")]
        public IActionResult ReceivePaymentNotification([FromBody] NotificationData request)
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
           
            if(!isValid)
            {
                return Unauthorized(new { status = 401, message = "Invalid Signature." });
            }
            else
            {
                // يمكن إضافة منطق معالجة الدفع هنا
                return Ok(new { status = 200, message = "Operation Done Successfully" });
            }

            
        }

        [HttpPost("SettlementNotification")]
        public IActionResult ReceiveSettlementNotification([FromBody] NotificationData request)
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
                // يمكن إضافة منطق معالجة الدفع هنا
                return Ok(new { status = 200, message = "Operation Done Successfully" });
            }

        }
    }
}
