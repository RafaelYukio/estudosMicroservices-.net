using Mango.MessageBus;
using Mango.Services.OrderAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Mango.Services.OrderAPI.Models.DTOs;
using Mango.Services.OrderAPI.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Mango.Services.OrderAPI.Utility;
using Mango.Services.OrderAPI.Models;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private ResponseDTO _response;
        private IMapper _mapper;

        public OrderController(AppDbContext db,
                               IProductService productService,
                               IMessageBus messageBus,
                               IConfiguration configuration,
                               ResponseDTO response,
                               IMapper mapper)
        {
            _db = db;
            _productService = productService;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = response;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ResponseDTO> CreateOrder([FromBody] CartDTO cartDTO)
        {
            try
            {
                OrderHeaderDTO orderHeaderDTO = _mapper.Map<OrderHeaderDTO>(cartDTO.CartHeader);
                orderHeaderDTO.OrderTime = DateTime.Now;
                orderHeaderDTO.Status = StaticDetails.Status_Pending;
                orderHeaderDTO.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDTO>>(cartDTO.CartDetails);

                OrderHeader orderCreated = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDTO)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDTO.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDTO;
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
