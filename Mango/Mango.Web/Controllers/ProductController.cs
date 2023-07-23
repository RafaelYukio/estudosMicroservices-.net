using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? list = new();

            ResponseDTO? response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        [Authorize(Roles = RoleAdmin)]
        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _productService.CreateProductAsync(productDTO);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product successfully created!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(productDTO);
        }

        [Authorize(Roles = RoleAdmin)]
        public async Task<IActionResult> ProductDelete(Guid productId)
        {
            ResponseDTO? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO productDTO)
        {
            ResponseDTO? response = await _productService.DeleteProductAsync(productDTO.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product successfully deleted!";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDTO);
        }

        [Authorize(Roles = RoleAdmin)]
        public async Task<IActionResult> ProductEdit(Guid productId)
        {
            ResponseDTO? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDTO productDTO)
        {
            ResponseDTO? response = await _productService.UpdateProductAsync(productDTO);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product successfully updated!";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDTO);
        }
    }
}
