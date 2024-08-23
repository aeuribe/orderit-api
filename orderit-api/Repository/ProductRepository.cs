using Microsoft.EntityFrameworkCore;
using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Models;
using System.Xml.Linq;

namespace orderit_api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly BusinessContext _context;
        public ProductRepository(BusinessContext context)
        {
            _context = context;
        }

        public Product AssignFKs(Brand brand, Category category, Product product)
        {
            product.Brand = brand;
            product.BrandId = brand.BrandId;
            product.Category = category;
            product.CategoryId = category.CategoryId;
            return product;
        }

        public bool CreateProduct(Product product)
        {
            _context.Products.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);   
            return Save();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .ToList();
        }

        public IEnumerable<Product> GetByBrand(int brandId)
        {
            return _context.Products
                .Where(p=>p.BrandId == brandId)
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .ToList();
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            return _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .ToList();
        }

        public Product GetByName(string name)
        {
            return _context.Products
                .Where(p=>p.Name == name)
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .FirstOrDefault();
        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Where(p => p.ProductId == id)
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .FirstOrDefault();
        }

        public bool ProductExist(int id)
        {
            return _context.Products.Any(p => p.ProductId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }
    }
}
