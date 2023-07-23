namespace Mango.Web.Models
{
    public class LoginResponseDTO
    {
        public UserResponseDTO User { get; set; }
        public string Token { get; set; }
    }
}
