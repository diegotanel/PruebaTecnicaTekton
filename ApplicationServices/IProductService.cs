using Models;
using Shared.DTOs;

namespace ApplicationServices
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task UpdateProductAsync(ProductDto productDto);
        Task<ProductDto> MapProductToDto(Product product);
        void MapDtoToProduct(ref Product product, ProductDto productDto);
    }
}