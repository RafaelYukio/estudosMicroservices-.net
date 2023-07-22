using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetAllCouponsAsync();
        Task<ResponseDTO?> GetCouponByCodeAsync(string couponCode);
        Task<ResponseDTO?> GetCouponByIdAsync(Guid id);
        Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO);
        Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO, Guid id);
        Task<ResponseDTO?> DeleteCouponAsync(Guid id);
    }
}
