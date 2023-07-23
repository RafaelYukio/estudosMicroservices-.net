using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ProductController(AppDbContext db, IMapper mapper)
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
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductResponseDTO>>(objList);
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
                Product obj = _db.Products.First(product => product.ProductId == id);
                _response.Result = _mapper.Map<ProductResponseDTO>(obj);
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
        [Route("GetByName/{name}")]
        public ResponseDTO GetByCode(string code)
        {
            try
            {
                Product obj = _db.Products.First(product => product.Name.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<ProductResponseDTO>(obj);
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
        public ResponseDTO CreateProduct([FromBody] ProductRequestDTO productDTO)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDTO);

                var savedObj = _db.Products.Add(obj).Entity;
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductResponseDTO>(savedObj);

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
        public ResponseDTO UpdateProduct([FromBody] ProductRequestDTO productDTO, Guid id)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDTO);
                obj.ProductId = id;
                var savedObj = _db.Products.Update(obj).Entity;
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductResponseDTO>(savedObj);

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
        public ResponseDTO RemoveProduct(Guid id)
        {
            try
            {
                Product obj = _db.Products.First(product => product.ProductId == id);
                _db.Products.Remove(obj);
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
