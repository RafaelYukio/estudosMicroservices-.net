using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTOs;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ShoppingCartController(AppDbContext db, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("get-cart/{userId}")]
        public async Task<ResponseDTO> GetCart(Guid userId)
        {
            try
            {
                CartDTO cartDTO = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(_db.CartHeaders.First(cartHeader => cartHeader.UserId == userId))
                };
                cartDTO.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(_db.CartDetails
                    .Where(cartDetails => cartDetails.CartHeaderId == cartDTO.CartHeader.CartHeaderId));

                IEnumerable<ProductDTO> productsDTOs = await _productService.GetProducts();

                foreach (var item in cartDTO.CartDetails)
                {
                    item.Product = productsDTOs.FirstOrDefault(product => product.ProductId == item.ProductId);
                    cartDTO.CartHeader.CartTotal += item.Quantity * item.Product.Price;
                }

                if(!string.IsNullOrEmpty(cartDTO.CartHeader.CouponCode))
                {
                    CouponDTO couponDTO = await _couponService.GetCoupon(cartDTO.CartHeader.CouponCode);
                    if(couponDTO != null && cartDTO.CartHeader.CartTotal > couponDTO.MinAmount)
                    {
                        cartDTO.CartHeader.CartTotal -= couponDTO.DiscountAmount;
                        cartDTO.CartHeader.Discount = couponDTO.DiscountAmount;
                    }
                }

                _response.Result = cartDTO;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("apply-coupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders.FirstAsync(cartHeader => cartHeader.UserId == cartDTO.CartHeader.UserId);
                cartFromDb.CouponCode = cartDTO.CartHeader.CouponCode;

                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("remove-coupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders.FirstAsync(cartHeader => cartHeader.UserId == cartDTO.CartHeader.UserId);
                cartFromDb.CouponCode = "";

                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("cart-upsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(
                    cartHeader => cartHeader.UserId == cartDTO.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    // Create header e details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDTO.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // Verifica se já há o item adicionado no carrinho
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        cartDetails => cartDetails.ProductId == cartDTO.CartDetails.First().ProductId &&
                        cartDetails.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        // Se não houver o produto adicionar, é criado:
                        cartDTO.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // Se já há o produto, sua quantidade é atualizada
                        cartDTO.CartDetails.First().Quantity += cartDetailsFromDb.Quantity;
                        cartDTO.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDTO.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }

                _response.Result = cartDTO;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("remove-cart")]
        public async Task<ResponseDTO> RemoveCart([FromBody]Guid cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(cartDetails => cartDetails.CartDetailsId == cartDetailsId);

                // Conta se há produtos no carrinho
                int totalCountOfCartItems = _db.CartDetails.Where(cartDetails => cartDetails.CartHeaderId == cartDetails.CartHeaderId).Count();

                _db.CartDetails.Remove(cartDetails);

                // Caso o produto removido seja o último no carrinho, o CartHeader é removido
                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(cartHeader => cartHeader.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
