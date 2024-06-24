using ApplicationServices;
using ApplicationServices.Products.Commands;
using Models;
using Moq;
using Shared.DTOs;
using Shared.External;

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
        public async Task Handle_ShouldAddProduct_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100m
            };

            _apiMock.Setup(s => s.GetApiDataAsync(It.IsAny<string>())).ReturnsAsync("urlmock");
            _productServiceMock.Setup(s => s.AddProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _productServiceMock.Setup(s => s.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProductDto());
            _productServiceMock.Setup(s => s.UpdateProductAsync(It.IsAny<ProductDto>()));


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            _productServiceMock.Verify(s => s.AddProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldCallGetProductByIdAsync_WhenProductIsAdded()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100m
            };

            var productDto = new ProductDto { ProductId = 1 };
            _productServiceMock.Setup(s => s.AddProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _productServiceMock.Setup(s => s.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            _productServiceMock.Verify(s => s.GetProductByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldUpdateProduct_WhenDiscountIsNotNull()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100m
            };

            var productDto = new ProductDto { ProductId = 1, Discount = 5m };
            _productServiceMock.Setup(s => s.AddProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _productServiceMock.Setup(s => s.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productDto);
            _productServiceMock.Setup(s => s.UpdateProductAsync(It.IsAny<ProductDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            _productServiceMock.Verify(s => s.UpdateProductAsync(It.IsAny<ProductDto>()), Times.Once);
        }
    }
}
