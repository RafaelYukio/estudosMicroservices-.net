using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _db;
        // DTO para retorno padrão de todas as APIs
        private ResponseDTO _response;
        private IMapper _mapper;

        public CouponController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponResponseDTO>>(objList);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public ResponseDTO GetById(Guid id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(coupon => coupon.CouponId == id);
                _response.Result = _mapper.Map<CouponResponseDTO>(obj);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDTO GetByCode(string code)
        {
            try
            {
                Coupon obj = _db.Coupons.First(coupon => coupon.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponResponseDTO>(obj);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO CreateCoupon([FromBody] CouponRequestDTO couponDTO)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                // Mesmo sem o CouponId, o EF gera o Id na hora de salvar
                var savedObj = _db.Coupons.Add(obj).Entity;
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponResponseDTO>(savedObj);

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO UpdateCoupon([FromBody] CouponRequestDTO couponDTO, Guid id)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                obj.CouponId = id;
                var savedObj = _db.Coupons.Update(obj).Entity;
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponResponseDTO>(savedObj);

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO RemoveCoupon(Guid id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(coupon => coupon.CouponId == id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
