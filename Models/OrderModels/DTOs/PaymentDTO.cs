namespace PetFeast_Backend2.Models.OrderModels.DTOs
{
    public class PaymentDTO
    {
        public string? razorpay_payment_id { get; set; }
        public string? razorpay_order_id { get; set; }
        public string? razorpay_signature { get; set; }
    }
}
