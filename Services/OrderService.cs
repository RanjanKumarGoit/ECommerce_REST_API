using trying.Data;
using trying.Model;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using trying.Interfaces;


namespace trying.Services
{

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateOrder(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || cart.CartItems.Count == 0)
            {
                // Cart is empty, cannot create an order
                return false;
            }

            // Create a new order
            var order = new Order
            {
                UserId = userId,
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in cart.CartItems)
            {
                // Create order items from cart items
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Status = "confirmed"
                    // You can copy other properties from cartItem if needed
                };

                order.OrderItems.Add(orderItem);
            }

            // Add the order to the context and save changes
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Optionally, you can clear the cart after creating the order
            cart.CartItems.Clear();
            _context.SaveChanges();

            return true;
        }

        public List<OrderItem> GetOrderItemsForUser(int userId)
        {
            // Retrieve all orders for the given user ID
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                .ThenInclude(i => i.Images)
                .Where(o => o.UserId == userId)
                .ToList();

            // Flatten the orderItems into a single array
            var orderItems = orders.SelectMany(o => o.OrderItems).ToList();

            return orderItems;
        }



    }
}