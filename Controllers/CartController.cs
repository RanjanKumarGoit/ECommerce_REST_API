using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using trying.Model;
using trying.Services;
using trying.Controllers.Dto;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Net;
using trying.Data;
using trying.Interfaces;

namespace trying.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetCartByUserId(int userId)
        {
            var carts = _cartService.GetCartByUserId(userId);
            if (carts == null || !carts.Any())
            {
                return NotFound("No carts found for the given user ID.");
            }

            return Ok(carts);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCartByLoggedInUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var carts = _cartService.GetCartByUserId(userId);
            if (carts == null || !carts.Any())
            {
                return NotFound("No carts found for the given user ID.");
            }

            return Ok(carts);
        }

        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] CartItemDto cartItemRequest)
        {
            if (cartItemRequest == null || cartItemRequest.ProductId <= 0 || cartItemRequest.Quantity <= 0)
            {
                var response = new { Success = false, Message = "Invalid payload. ProductId and Quantity are required." };
                return BadRequest(response);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim.Value);
                var result = _cartService.AddToCart(userId, cartItemRequest.ProductId, cartItemRequest.Quantity);

                if (result)
                {
                    var response = new { Success = true, Message = "Item added to the cart successfully." };
                    return Ok(response);
                }

                var responseBad = new { Success = false, Message = "Failed to add the item to the cart." };
                return BadRequest(responseBad);
            }
            else
            {
                return NotFound("LogIn first");
            }
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [Authorize]
        [HttpDelete("remove/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _cartService.RemoveFromCart(userId, productId);

            if (result)
            {
                var response = new { Success = true, Message = $"Product with ID {productId} removed from the cart successfully." };
                return Ok(response);
            }

            return NotFound($"Product with ID {productId} not found in the cart.");
        }

        [Authorize]
        [HttpPatch("increase-quantity/{productId}")]
        public IActionResult IncreaseQuantity(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _cartService.IncreaseCartItemQuantity(userId, productId);

            if (result)
            {
                var response = new { Success = true, Message = $"Quantity of product with ID {productId} increased in the cart." };
                return Ok(response);
            }

            return NotFound($"Product with ID {productId} not found in the cart.");
        }

        [Authorize]
        [HttpPatch("decrease-quantity/{productId}")]
        public IActionResult DecreaseQuantity(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _cartService.DecreaseCartItemQuantity(userId, productId);

            if (result)
            {
                var response = new { Success = true, Message = $"Quantity of product with ID {productId} decreased in the cart." };
                return Ok(response);
            }

            return NotFound($"Product with ID {productId} not found in the cart.");
        }

    }
}