using ApplicationServices;
using Models;
using Microsoft.AspNetCore.Mvc;
using ApplicationServices.Products.Commands;
using ApplicationServices.Products.Queries;
using MediatR;
using Shared.External.ApiClientLibrary;
using System.Text.Json;

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

        [HttpGet]
        public async Task<IActionResult> GetAPIExternal()
        {
            //string apiUrl = "https://api.generadordni.es/v2/bank/card";
            string apiUrl = "https://6675ff2ba8d2b4d072f21eb8.mockapi.io/api/preciodedescuento/numerosaleatorios/1";

            var mock = new MockApi();
            string response = await mock.GetApiDataAsync(apiUrl);

            return Ok(response);
        }

    }
}


   
