using trying.Model;

namespace trying.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        void CreateProduct(Product product, List<Image> images);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
