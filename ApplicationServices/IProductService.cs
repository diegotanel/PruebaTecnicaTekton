using Models;

namespace ApplicationServices
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
    }
}