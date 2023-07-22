namespace Mango.Services.CouponAPI.Models.DTOs
{
    public class CouponResponseDTO
    {
        public Guid CouponId { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinAmount { get; set; }
    }
}
