using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IShoppingCartService
    {
        Task<ResponseDTO?> GetShoppingCartByUserIdAsync(Guid userId);
        Task<ResponseDTO?> UpsertShoppingCartAsync(CartDTO cartDTO);
        Task<ResponseDTO?> RemoveFromShoppingCartAsync(Guid cartDetailsId);
        Task<ResponseDTO?> ApplyCouponAsync(CartDTO cartDTO);
    }
}
