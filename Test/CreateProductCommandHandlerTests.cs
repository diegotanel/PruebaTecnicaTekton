using Moq;
using ApplicationServices.Products.Commands;
using Models;
using Shared.DTOs;
using Shared.External;
using ApplicationServices;

namespace Test
{
    [TestFixture]
    public class CreateProductCommandHandlerTests
    {
        private Mock<IProductService> _productServiceMock;
        private Mock<IApiExterna> _apiMock;
        private CreateProductCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _productServiceMock = new Mock<IProductService>();
            _apiMock = new Mock<IApiExterna>();
            _handler = new CreateProductCommandHandler(_productServiceMock.Object, _apiMock.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ShouldReturnProductDtoWithDiscount()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m
            };

            var product = new Product
            {
                ProductId = 1,
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m
            };

            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                StatusName = "Active",
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m,
                Discount = 10.0m
            };

            _productServiceMock.Setup(x => x.AddProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _apiMock.Setup(x => x.GetApiDataAsync(It.IsAny<string>())).ReturnsAsync("10");
            _productServiceMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productDto);
            _productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productDto, result);
        }

        [Test]
        public async Task Handle_ValidCommand_ShouldReturnProductDtoWithoutDiscount()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m
            };

            var product = new Product
            {
                ProductId = 1,
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m
            };

            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                StatusName = "Active",
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m
            };

            _productServiceMock.Setup(x => x.AddProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _apiMock.Setup(x => x.GetApiDataAsync(It.IsAny<string>())).ReturnsAsync((string)null);
            _productServiceMock.Setup(x => x.MapProductToDto(It.IsAny<Product>())).ReturnsAsync(productDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productDto, result);
        }
    }
}