using ApplicationServices;
using Models;
using Microsoft.AspNetCore.Mvc;
using ApplicationServices.Products.Commands;
using ApplicationServices.Products.Queries;
using MediatR;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            if (id != command.ProductId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var query = new GetProductByIdQuery { ProductId = id };
            var product = await _mediator.Send(query);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }



    //[ApiController]
    //[Route("api/[controller]")]
    //public class ProductsController : ControllerBase
    //{
    //    private readonly IProductService _productService;

    //    public ProductsController(IProductService productService)
    //    {
    //        _productService = productService;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> CreateProduct(Product product)
    //    {
    //        await _productService.AddProductAsync(product);
    //        return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdateProduct(int id, Product product)
    //    {
    //        if (id != product.ProductId)
    //        {
    //            return BadRequest();
    //        }

    //        await _productService.UpdateProductAsync(product);
    //        return NoContent();
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetProductById(int id)
    //    {
    //        var product = await _productService.GetProductByIdAsync(id);
    //        if (product == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(product);
    //    }
    //}

}
