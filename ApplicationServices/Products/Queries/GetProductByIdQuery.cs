using MediatR;
using Shared.DTOs;

namespace ApplicationServices.Products.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int ProductId { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductService _productService;

        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductByIdAsync(request.ProductId);
        }
    }

}
