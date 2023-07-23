using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = productDTO,
                Url = ProductAPIBase + "/api/product/"
            });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(Guid id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.DELETE,
                Url = ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Url = ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDTO?> GetProductByNameAsync(string productName)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Url = ProductAPIBase + "/api/product/GetByName/" + productName
            });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(Guid id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Url = ProductAPIBase + "/api/product/" + id
            });
        }

        public Task<ResponseDTO?> GetProductByNamesync(string productName)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.PUT,
                Data = productDTO,
                Url = ProductAPIBase + "/api/product/" + productDTO.ProductId
            });
        }
    }
}
