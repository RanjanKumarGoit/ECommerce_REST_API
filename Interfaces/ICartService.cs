using trying.Model;


namespace trying.Interfaces
{
    public interface ICartService
    {
        IEnumerable<Cart> GetCart(int id);
        IEnumerable<Cart> GetCartByUserId(int userId);
        bool AddToCart(int userId, int productId, int quantity);
        bool RemoveFromCart(int userId, int productId);
        bool IncreaseCartItemQuantity(int userId, int productId);
        bool DecreaseCartItemQuantity(int userId, int productId);

    }
}
