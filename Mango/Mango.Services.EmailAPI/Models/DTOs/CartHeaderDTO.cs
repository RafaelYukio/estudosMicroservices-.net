namespace Mango.Services.EmailAPI.Models

{
    public class CartHeaderDTO
    {
        public Guid CartHeaderId { get; set; }
        public Guid UserId { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public decimal CartTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
