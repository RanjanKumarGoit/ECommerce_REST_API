using Microsoft.EntityFrameworkCore;
using trying.Data;
using trying.Model;
using trying.Interfaces;

namespace trying.Services
{

    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public bool AddToCart(int userId, int productId, int quantity)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                // Create a new cart if the user doesn't have one
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                // If the product is not already in the cart, create a new cart item
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                // If the product is already in the cart, update the quantity
                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Cart> GetCart(int id)
        {
            return _context.Carts
                .Where(c => c.CartId == id)
                .Include(c => c.CartItems) // Include CartItems
                    .ThenInclude(ci => ci.Product) // Include Product in CartItems
                    .ThenInclude(im => im.Images)
                .ToList();
        }

        public IEnumerable<Cart> GetCartByUserId(int userId)
        {
            return _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .ThenInclude(im => im.Images)
                .ToList();
        }

        public bool RemoveFromCart(int userId, int productId)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    _context.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public bool IncreaseCartItemQuantity(int userId, int productId)
        {
            return UpdateQuantity(userId, productId, increase: true);
        }

        public bool DecreaseCartItemQuantity(int userId, int productId)
        {
            return UpdateQuantity(userId, productId, increase: false);
        }

        private bool UpdateQuantity(int userId, int productId, bool increase)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                if (increase)
                {
                    cartItem.Quantity++;
                }
                else if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    // If quantity is already 1 and trying to decrease, you might want to remove the item from the cart.
                    cart.CartItems.Remove(cartItem);
                }

                _context.SaveChanges();
                return true;
            }

            return false;
        }


    }
}