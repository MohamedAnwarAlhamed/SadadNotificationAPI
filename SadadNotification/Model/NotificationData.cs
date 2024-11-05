namespace SadadNotification.Model
{
    public class NotificationData
    {
        public string branchCode { get; set; }
        public string bankId { get; set; }
        public string districtCode { get; set; }
        public string accessChannel { get; set; }
        public string sadadPaymentId { get; set; }
        public string sadadNumber { get; set; }
        public string paymentMethod { get; set; }
        public string billNumber { get; set; }
        public string bankPaymentId { get; set; }
        public string paymentDate { get; set; }
        public decimal paymentAmount { get; set; }
        public string paymentStatus { get; set; }
    }
}
