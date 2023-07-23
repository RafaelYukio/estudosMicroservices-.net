namespace Mango.Services.EmailAPI.Models

{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
