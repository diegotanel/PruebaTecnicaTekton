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

        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Status = request.Status,
                Stock = request.Stock,
                Description = request.Description,
                Price = request.Price,
                Discount = request.Discount
            };

            await _productService.AddProductAsync(product);
            var productDto = _productService.MapProductToDto(product);
            productDto.Result.StatusName = product.Status == 1 ? "Active" : "Inactive";

            return productDto.Result;
        }
    }

}
