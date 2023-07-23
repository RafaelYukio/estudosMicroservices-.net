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

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDTOBasedOnLoggedUser());
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
