namespace PetsApp.Models
{
    internal class VnPaymentRequestModel
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}