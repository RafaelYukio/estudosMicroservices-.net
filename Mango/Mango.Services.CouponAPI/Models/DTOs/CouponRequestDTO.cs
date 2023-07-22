namespace Mango.Services.CouponAPI.Models.DTOs
{
    public class CouponRequestDTO
    {
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinAmount { get; set; }
    }
}
