using MediatR;
using Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ApplicationServices.Products.Commands
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public int ProductId { get; set; }

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

            await _productService.UpdateProductAsync(productDto);

            return productDto;
        }
    }

}
