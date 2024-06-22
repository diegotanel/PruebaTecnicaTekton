using MediatR;
using Models;
using Repositories;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOs;
using Shared.External.ApiClientLibrary;
using Shared.External;

namespace ApplicationServices.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
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
            string apiUrl = $"https://6675ff2ba8d2b4d072f21eb8.mockapi.io/api/preciodedescuento/numerosaleatorios/{product.ProductId}";
            string descuento = await _api.GetApiDataAsync(apiUrl);
            ProductDto productDto;
            if (descuento != null)
            {
                productDto = await _productService.GetProductByIdAsync(product.ProductId);
                productDto.Discount = Convert.ToDecimal(descuento);
                await _productService.UpdateProductAsync(productDto);
                return productDto;
            }
            else
            {
                productDto = await _productService.MapProductToDto(product);
                return productDto;
            }

        }
    }

}
