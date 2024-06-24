using Moq;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ApplicationServices.Products.Commands;
using ApplicationServices.Products.Queries;
using Shared.DTOs;

namespace Test
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private ProductsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Test]
        public async Task CreateProduct_ValidCommand_ReturnsCreatedResult()
        {
            // Arrange
            var command = new CreateProductCommand { Name = "Test Product" };
            var product = new ProductDto { ProductId = 1, Name = "Test Product" };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(product);

            // Act
            var result = await _controller.CreateProduct(command);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(product, createdResult.Value);
        }

        [Test]
        public async Task UpdateProduct_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var command = new UpdateProductCommand { ProductId = 1, Name = "Updated Product" };

            // Act
            var result = await _controller.UpdateProduct(2, command);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task UpdateProduct_ValidCommand_ReturnsCreatedResult()
        {
            // Arrange
            var command = new UpdateProductCommand { ProductId = 1, Name = "Updated Product" };
            var product = new ProductDto { ProductId = 1, Name = "Updated Product" };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(product);

            // Act
            var result = await _controller.UpdateProduct(1, command);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(product, createdResult.Value);
        }

        [Test]
        public async Task GetProductById_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var query = new GetProductByIdQuery { ProductId = 1 };
            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetProductById_ProductFound_ReturnsOkResult()
        {
            // Arrange
            var query = new GetProductByIdQuery { ProductId = 1 };
            var product = new ProductDto { ProductId = 1, Name = "Test Product" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            Assert.IsNotNull(result, "The result should not be null.");
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "The result should be of type OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(product, okResult.Value);
        }
    }
}

