using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public Guid CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public decimal DiscountAmount { get; set; }
        public decimal MinAmount { get; set; }
    }
}
