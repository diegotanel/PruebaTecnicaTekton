using Moq;
using ApplicationServices;
using ApplicationServices.Products.Queries;
using Shared.DTOs;


namespace Test
{
    [TestFixture]
    public class GetProductByIdQueryHandlerTests
    {
        private Mock<IProductService> _productServiceMock;
        private GetProductByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _productServiceMock = new Mock<IProductService>();
            _handler = new GetProductByIdQueryHandler(_productServiceMock.Object);
        }

        [Test]
        public async Task Handle_ProductExists_ShouldReturnProductDto()
        {
            // Arrange
            var query = new GetProductByIdQuery { ProductId = 1 };
            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                StatusName = "1",
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m
            };

            _productServiceMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(productDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(query.ProductId, result.ProductId);
            Assert.AreEqual(productDto.Name, result.Name);
            _productServiceMock.Verify(x => x.GetProductByIdAsync(query.ProductId), Times.Once);
        }

        [Test]
        public async Task Handle_ProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var query = new GetProductByIdQuery { ProductId = 1 };

            _productServiceMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
            _productServiceMock.Verify(x => x.GetProductByIdAsync(query.ProductId), Times.Once);
        }
    }
}