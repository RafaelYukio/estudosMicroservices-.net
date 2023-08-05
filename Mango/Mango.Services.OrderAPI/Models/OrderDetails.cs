using Mango.Services.OrderAPI.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public Guid OrderDetailsId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public Guid OrderHeaderId { get; set; }
        public OrderHeader? OrderHeader { get; set; }
        public Guid ProductId { get; set; }
        [NotMapped]
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
