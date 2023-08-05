namespace Mango.Web.Models
{
    public class OrderDetailsDTO
    {
        public Guid OrderDetailsId { get; set; }
        public Guid OrderHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
