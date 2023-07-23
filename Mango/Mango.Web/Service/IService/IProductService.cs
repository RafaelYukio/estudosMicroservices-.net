using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetAllProductsAsync();
        Task<ResponseDTO?> GetProductByNamesync(string productName);
        Task<ResponseDTO?> GetProductByIdAsync(Guid id);
        Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteProductAsync(Guid id);
    }
}
