namespace Mango.Services.AuthAPI.Models.DTOs
{
    public class LoginResponseDTO
    {
        public UserResponseDTO User { get; set; }
        public string Token { get; set; }
    }
}
