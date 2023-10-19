using Microsoft.EntityFrameworkCore;
using trying.Data;
using trying.Model;
using trying.Interfaces;
namespace trying.Services
{


    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.Include(p => p.Images).ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == id);
        }

        public void CreateProduct(Product product, List<Image> images)
        {
            _context.Products.Add(product);
            _context.Images.AddRange(images);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}