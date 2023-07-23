using Mango.Services.ShoppingCartAPI.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public Guid CartDetailsId { get; set; }
        public Guid CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set;}
        public Guid ProductId { get; set; }
        [NotMapped]
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
    }
}
