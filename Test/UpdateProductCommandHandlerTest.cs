using Moq;
using ApplicationServices;
using ApplicationServices.Products.Commands;
using Shared.DTOs;

namespace Test
{
    [TestFixture]
    public class UpdateProductCommandHandlerTests
    {
        private Mock<IProductService> _productServiceMock;
        private UpdateProductCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _productServiceMock = new Mock<IProductService>();
            _handler = new UpdateProductCommandHandler(_productServiceMock.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Name = "Updated Product",
                Status = 1,
                Stock = 20,
                Description = "Updated Description",
                Price = 150.0m
            };

            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Original Product",
                StatusName = "0",
                Stock = 10,
                Description = "Original Description",
                Price = 100.0m
            };

            _productServiceMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productDto);
            _productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(command.ProductId, result.ProductId);
            Assert.AreEqual(command.Name, result.Name);
            Assert.AreEqual(command.Status.ToString(), result.StatusName);
            Assert.AreEqual(command.Stock, result.Stock);
            Assert.AreEqual(command.Description, result.Description);
            Assert.AreEqual(command.Price, result.Price);

            _productServiceMock.Verify(x => x.GetProductByIdAsync(command.ProductId), Times.Once);
            _productServiceMock.Verify(x => x.UpdateProductAsync(It.Is<ProductDto>(p => p.ProductId == command.ProductId && p.Name == command.Name)), Times.Once);
        }

        [Test]
        public void Handle_InvalidCommand_ProductDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Name = "Updated Product",
                Status = 1,
                Stock = 20,
                Description = "Updated Description",
                Price = 150.0m
            };

            _productServiceMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto)null);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

            _productServiceMock.Verify(x => x.GetProductByIdAsync(command.ProductId), Times.Once);
            _productServiceMock.Verify(x => x.UpdateProductAsync(It.IsAny<ProductDto>()), Times.Never);
        }
    }
}

