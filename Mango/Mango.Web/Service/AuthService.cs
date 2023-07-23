using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AssignRoleAsync(AssignRoleRequestDTO assignRoleRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = assignRoleRequestDTO,
                Url = AuthAPIBase + "/api/auth/assign-role"
            });
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = loginRequestDTO,
                Url = AuthAPIBase + "/api/auth/login"
            }, false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = registerRequestDTO,
                Url = AuthAPIBase + "/api/auth/register"
            }, false);
        }
    }
}
