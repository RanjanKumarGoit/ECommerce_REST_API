using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using trying.Services;
using trying.Controllers.Dto;
using System.Security.Claims;
using trying.Model;
using System.Collections.Generic;
using trying.Interfaces;

namespace trying.Controllers
{
    [ApiController]
    [Route("api/order")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateOrder()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orderId = _orderService.CreateOrder(userId);

            if (orderId)
            {
                var response = new { Success = true, Message = "Order created successfully." };
                return Ok(response);
            }


            var responseBad = new { Success = false, Message = "Failed to create the order." };
            return BadRequest(responseBad);
        }

        [Authorize]
        [HttpGet("getOrders")]
        public IActionResult GetOrdersForUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orders = _orderService.GetOrderItemsForUser(userId);

            if (orders != null && orders.Any())
            {
                var response = new { Success = true, Orders = orders };
                return Ok(response);
            }

            var responseBad = new { Success = false, Message = "No orders found for the user." };
            return NotFound(responseBad);
        }

    }
}