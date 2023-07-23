namespace Mango.Services.AuthAPI.Models.DTOs
{
    public class RegisterRequestDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
