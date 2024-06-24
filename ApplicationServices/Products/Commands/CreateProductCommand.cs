using MediatR;
using Models;
using Shared.DTOs;
using Shared.External;
using Shared.Configs;
using System.ComponentModel.DataAnnotations;

namespace ApplicationServices.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Range(0, 1, ErrorMessage = "El valor debe ser 0 o 1")]
        public int Status { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor no negativo")]
        public int Stock { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public decimal Price { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductService _productService;
        private readonly IApiExterna _api;

        public CreateProductCommandHandler(IProductService productService, IApiExterna api)
        {
            _productService = productService;
            _api = api;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Status = request.Status,
                Stock = request.Stock,
                Description = request.Description,
                Price = request.Price
            };

            await _productService.AddProductAsync(product);
            string apiUrl = MockApiUrl.Url + product.ProductId;
            string descuento = await _api.GetApiDataAsync(apiUrl);
            ProductDto productDto;
            if (descuento != null)
            {
                productDto = await _productService.GetProductByIdAsync(product.ProductId);
                productDto.Discount = Convert.ToDecimal(descuento);
                await _productService.UpdateProductAsync(productDto);
                return await _productService.GetProductByIdAsync(product.ProductId);
            }
            else
            {
                return await _productService.MapProductToDto(product);
            }

        }
    }

}
