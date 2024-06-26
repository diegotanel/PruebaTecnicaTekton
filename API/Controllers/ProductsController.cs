﻿using Microsoft.AspNetCore.Mvc;
using ApplicationServices.Products.Commands;
using ApplicationServices.Products.Queries;
using MediatR;
using Shared.DTOs;

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

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="command">Datos del producto.</param>
        /// <returns>Producto</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">Id del producto.</param>
        /// <param name="command">Datos actualizados del producto.</param>
        /// <returns>Producto</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            if (id != command.ProductId)
            {
                return BadRequest();
            }

            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        /// <summary>
        /// Obtiene un producto por ID.
        /// </summary>
        /// <param name="id">Id del producto.</param>
        /// <returns>Producto</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
}


   
