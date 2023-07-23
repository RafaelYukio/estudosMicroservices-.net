using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public Guid CartHeaderId { get; set; }
        public Guid UserId { get; set; }
        public string? CouponCode { get; set; }
        [NotMapped]
        public decimal Discount { get; set; }
        [NotMapped]
        public decimal CartTotal { get; set; }
    }
}
