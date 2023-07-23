using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService _baseService;

        public ShoppingCartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> ApplyCouponAsync(CartDTO cartDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = cartDTO,
                Url = ShoppingCartAPIBase + "/api/cart/apply-coupon"
            });
        }

        public async Task<ResponseDTO?> GetShoppingCartByUserIdAsync(Guid userId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Url = ShoppingCartAPIBase + "/api/cart/get-cart/" + userId
            });
        }

        public async Task<ResponseDTO?> RemoveFromShoppingCartAsync(Guid cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = cartDetailsId,
                Url = ShoppingCartAPIBase + "/api/cart/remove-cart"
            });
        }

        public async Task<ResponseDTO?> UpsertShoppingCartAsync(CartDTO cartDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = cartDTO,
                Url = ShoppingCartAPIBase + "/api/cart/cart-upsert"
            });
        }
    }
}
