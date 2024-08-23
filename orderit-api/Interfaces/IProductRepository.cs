using orderit_api.Models;

namespace orderit_api.Interfaces
{
    public interface IProductRepository
    {
        Product GetProductById(int id);
        Product GetByName(string name);
        Product AssignFKs(Brand brand, Category category, Product product);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetByCategory(int categoryId);
        IEnumerable<Product> GetByBrand(int brandId);
        bool CreateProduct(Product product);
        bool DeleteProduct(Product product);
        bool UpdateProduct(Product product);
        bool ProductExist(int id);
        bool Save();

    }
}
