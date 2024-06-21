using MediatR;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOs;

namespace ApplicationServices.Products.Commands
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductService _productService;

        public UpdateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        async Task<ProductDto> IRequestHandler<UpdateProductCommand, ProductDto>.Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productDto = await _productService.GetProductByIdAsync(request.ProductId);

            if (productDto == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            productDto.ProductId = request.ProductId;
            productDto.Name = request.Name;
            productDto.StatusName = request.Status.ToString();
            productDto.Stock = request.Stock;
            productDto.Description = request.Description;
            productDto.Price = request.Price;
            productDto.Discount = request.Discount;

            await _productService.UpdateProductAsync(productDto);

            return productDto;
        }
    }

}
