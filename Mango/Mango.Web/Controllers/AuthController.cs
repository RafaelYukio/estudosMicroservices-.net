using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();

            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            ResponseDTO responseDTO = await _authService.LoginAsync(loginRequestDTO);

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(responseDTO.Result.ToString());

                // Aqui é realizado o Signin para que a aplicação saiba que há usuário logado
                await SignInUser(loginResponseDTO);
                
                _tokenProvider.SetToken(loginResponseDTO.Token);

                TempData["success"] = "Login successful";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDTO.Message;
                return View(loginRequestDTO);
            }
        }

        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = RoleAdmin, Value = RoleAdmin },
                new SelectListItem { Text = RoleCustomer, Value = RoleCustomer }
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            ResponseDTO registerResponseDTO = await _authService.RegisterAsync(registerRequestDTO);

            if (registerResponseDTO != null && registerResponseDTO.IsSuccess)
            {
                ResponseDTO assignRoleResponseDTO = await _authService.AssignRoleAsync(new AssignRoleRequestDTO()
                {
                    Email = registerRequestDTO.Email,
                    Role = registerRequestDTO.Role
                });

                if(assignRoleResponseDTO != null &&  assignRoleResponseDTO.IsSuccess)
                {
                    TempData["success"] = "Registration successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = registerResponseDTO.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = RoleAdmin, Value = RoleAdmin },
                new SelectListItem { Text = RoleCustomer, Value = RoleCustomer }
            };

            ViewBag.RoleList = roleList;
            return View(registerRequestDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();

            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDTO loginResponseDTO)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponseDTO.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(user => user.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(user => user.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(user => user.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(user => user.Type == JwtRegisteredClaimNames.Email).Value));
            // Adicionando a Claim de Role, agora podemos autenticar os controller por role
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(user => user.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
