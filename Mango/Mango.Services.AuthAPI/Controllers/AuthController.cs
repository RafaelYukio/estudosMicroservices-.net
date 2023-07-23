using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.DTOs;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private ResponseDTO _response;

        public AuthController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration)
        {
            _authService = authService;
            _response = new();
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var errorMessage = await _authService.Register(registerRequestDTO);

            if(!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }

            // Manda mensagem para Azure Service Bus
            await _messageBus.PublishMessage(registerRequestDTO.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginReponse = await _authService.Login(loginRequestDTO);

            if (loginReponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }

            _response.Result = loginReponse;
            return Ok(_response);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDTO assignRoleRequestDTO)
        {
            var assignRoleResponse = await _authService.AssignRole(assignRoleRequestDTO.Email, assignRoleRequestDTO.Role);

            if (!assignRoleResponse)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }

            _response.Result = assignRoleResponse;
            return Ok(_response);
        }
    }
}
