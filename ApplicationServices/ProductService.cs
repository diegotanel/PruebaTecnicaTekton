using Models;
using LazyCache;
using Repositories;

namespace ApplicationServices
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IAppCache _cache;
        private readonly TimeSpan _cacheExpire;
        public ProductService(IProductRepository repository, IAppCache cache)
        {
            _repository = repository;
            _cache = cache;
            _cacheExpire = TimeSpan.FromMinutes(5);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _cache.GetOrAddAsync($"product_{id}", async () => await _repository.GetProductByIdAsync(id), TimeSpan.FromMinutes(5));
        }
        public async Task AddProductAsync(Product product)
        {
            await _repository.AddProductAsync(product);
            await this.UpdateCacheProduct(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _repository.UpdateProductAsync(product);
            await this.UpdateCacheProduct(product);
        }

        private async Task UpdateCacheProduct(Product product)
        {
            _cache.Remove($"product_{product.ProductId}");
            _cache.Add($"product_{product.ProductId}", product, _cacheExpire);
        }
    }

}
