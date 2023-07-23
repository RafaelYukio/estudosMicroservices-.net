using Mango.Services.EmailAPI.Models;

namespace Mango.Services.EmailAPI.Service.IService
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTO cartDTO);
        Task RegisterUserEmailAndLog(string userEmail);
    }
}
