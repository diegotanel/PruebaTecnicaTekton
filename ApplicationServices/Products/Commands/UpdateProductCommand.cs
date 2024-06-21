using MediatR;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Products.Commands
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IProductService _productService;

        public UpdateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        async Task<Product> IRequestHandler<UpdateProductCommand, Product>.Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            product.Name = request.Name;
            product.Status = request.Status;
            product.Stock = request.Stock;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Discount = request.Discount;

            await _productService.UpdateProductAsync(product);

            return product;
        }
    }

}
