using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDTOBasedOnLoggedUser());
        }

		[Authorize]
		public async Task<IActionResult> Checkout()
		{
			return View(await LoadCartDTOBasedOnLoggedUser());
		}

        [Authorize]
        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDTO cartDTO)
        {
            // Já estamos recebendo o CartDTO por parâmetros, porém é melhor pegar o mais atualizado pelo método:
            CartDTO cart = await LoadCartDTOBasedOnLoggedUser();

            // Os campos no front são aberto para modificar:
            cart.CartHeader.Phone = cartDTO.CartHeader.Phone;
            cart.CartHeader.Email = cartDTO.CartHeader.Email;
            cart.CartHeader.Name = cartDTO.CartHeader.Name;

            var response = await _orderService.CreateOrderAsync(cart);
            OrderHeaderDTO orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(response.Result));

            if(response != null && response.IsSuccess)
            {

            }

            return View();
        }

        public async Task<IActionResult> Remove(Guid cartDetailsId)
        {
            ResponseDTO response = await _shoppingCartService.RemoveFromShoppingCartAsync(cartDetailsId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully!";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            ResponseDTO response = await _shoppingCartService.ApplyCouponAsync(cartDTO);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully!";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            cartDTO.CartHeader.CouponCode = "";
            ResponseDTO response = await _shoppingCartService.ApplyCouponAsync(cartDTO);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully!";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart()
        {
            CartDTO cart = await LoadCartDTOBasedOnLoggedUser();
            cart.CartHeader.Email = User.Claims.Where(user => user.Type == JwtRegisteredClaimNames.Email).FirstOrDefault().Value;

            ResponseDTO response = await _shoppingCartService.EmailCart(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly!";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        private async Task<CartDTO> LoadCartDTOBasedOnLoggedUser()
        {
            var userId = Guid.Parse(User.Claims.Where(user => user.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value);
            ResponseDTO response = await _shoppingCartService.GetShoppingCartByUserIdAsync(userId);

            if(response != null && response.IsSuccess)
            {
                CartDTO cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
                return cartDTO;
            }

            return new CartDTO();
        }
    }
}
