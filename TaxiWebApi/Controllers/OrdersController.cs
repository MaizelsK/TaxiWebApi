using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DTO;
using Services;

namespace TaxiWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            IEnumerable<OrderInfoDto> orderDtos;

            if (HttpContext.User.IsInRole("client"))
            {
                int userId = int.Parse(HttpContext.User.Identity.Name);
                orderDtos = await _orderService.GetOrdersInfoesAsync(userId);
            }
            else 
                orderDtos = await _orderService.GetOrdersInfoesAsync();

            return Ok(orderDtos);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]NewOrderDto dto)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);
            var newOrder = await _orderService.CreateOrder(dto, userId);

            return Ok(newOrder);
        }

        [Authorize(Roles = "driver")]
        [HttpPost("take-order/{id}")]
        public async Task<IActionResult> TakeOrder(int id)
        {
            var order = await _orderService.OrderOnRoadAsync(id);

            if (order == null)
                return NotFound($"Order #{id} not found");

            return Ok(order);
        }

        [Authorize(Roles = "driver")]
        [HttpPost("finish-order/{id}")]
        public async Task<IActionResult> FinishOrder(int id)
        {
            var order = await _orderService.FinishOrderAsync(id);

            if (order == null)
                return NotFound($"Order #{id} not found");

            return Ok(order);
        }
    }
}
