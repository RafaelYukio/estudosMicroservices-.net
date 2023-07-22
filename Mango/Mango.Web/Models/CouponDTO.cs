namespace Mango.Web.Models
{
    public class CouponDTO
    {
        public Guid CouponId { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinAmount { get; set; }
    }
}
