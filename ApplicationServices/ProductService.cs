using Models;
using LazyCache;
using Repositories;
using Shared.DTOs;
using Shared.Configs;

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
            _cacheExpire = TimeSpan.FromMinutes(CacheConfig.Expiration);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _cache.GetOrAddAsync($"product_{id}", async () => await _repository.GetProductByIdAsync(id), _cacheExpire);
            if (product == null)
            {
                return null;
            }
            var productDto = this.MapProductToDto(product);
            return productDto.Result;
        }
        public async Task AddProductAsync(Product product)
        {
            await _repository.AddProductAsync(product);
            await this.UpdateCacheProduct(product);
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            Product product = await _cache.GetOrAddAsync($"productDto_{productDto.ProductId}", async () => await _repository.GetProductByIdAsync(productDto.ProductId), _cacheExpire);
            this.MapDtoToProduct(ref product, productDto);
            await _repository.UpdateProductAsync(product);
            await this.UpdateCacheProduct(product);
        }

        private async Task UpdateCacheProduct(Product product)
        {
            _cache.Remove($"product_{product.ProductId}");
            _cache.Add($"product_{product.ProductId}", product, _cacheExpire);
        }

        public void MapDtoToProduct(ref Product product, ProductDto productDto)
        {
            product.Name = productDto.Name;
            product.Status = productDto.StatusName == "Active" ? 1 : 0;
            product.Stock = productDto.Stock;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Discount = productDto.Discount;
        }

        public async Task<ProductDto> MapProductToDto(Product product)
        {
            ProductDto productDto = new ProductDto();
            productDto.ProductId = product.ProductId;
            productDto.Name = product.Name;
            productDto.StatusName = product.Status == 1 ? "Active" : "Inactive";
            productDto.Stock = product.Stock;
            productDto.Description = product.Description;
            productDto.Price = product.Price;
            productDto.Discount = product.Discount;
            productDto.FinalPrice = product.FinalPrice;
            return productDto;
        }
    }

}
