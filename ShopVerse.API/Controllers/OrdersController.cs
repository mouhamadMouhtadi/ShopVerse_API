using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopVerse.API.Errors;
using ShopVerse.Core;
using ShopVerse.Core.Dtos.Orders;
using ShopVerse.Core.Entities.Order;
using ShopVerse.Core.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopVerse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (UserEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var address = _mapper.Map<Address>(model.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(UserEmail, model.BasketId, model.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Problem creating order."));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (UserEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var orders = await _orderService.GetOrdersForSpecificUserAsync(UserEmail);
            if (orders is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "No orders found."));
            return Ok(_mapper.Map<IEnumerable<OrderToReturnDto>>(orders));
        }
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrdersForSpecificUser(int orderId)
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (UserEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var orders = await _orderService.GetOrderByIdForSpecificUserAsync(UserEmail, orderId);
            if (orders is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(_mapper.Map<OrderToReturnDto>(orders));
        }
        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (deliveryMethods is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "No delivery methods found."));
            return Ok(deliveryMethods);
        }
    }
}
