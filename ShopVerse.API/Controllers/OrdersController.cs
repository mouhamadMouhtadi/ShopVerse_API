using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopVerse.API.Errors;
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

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
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
    }
}
