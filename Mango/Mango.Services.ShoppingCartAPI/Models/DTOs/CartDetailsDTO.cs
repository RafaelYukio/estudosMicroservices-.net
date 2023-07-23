namespace Mango.Services.ShoppingCartAPI.Models.DTOs
{
    public class CartDetailsDTO
    {
        public Guid CartDetailsId { get; set; }
        public Guid CartHeaderId { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }
        public Guid ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }
    }
}
