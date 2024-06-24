using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;
using LazyCache;
using Models;
using Repositories;
using Shared.Cache;
using Shared.DTOs;
using ApplicationServices;

namespace Test
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _repositoryMock;
        private Mock<ICache> _cacheMock;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _cacheMock = new Mock<ICache>();
            _productService = new ProductService(_repositoryMock.Object, _cacheMock.Object);
        }

        [Test]
        public async Task GetProductByIdAsync_ProductExistsInCache_ShouldReturnProductDto()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Test Product" };
            _cacheMock.Setup(c => c.GetOrAddAsync($"product_{product.ProductId}", It.IsAny<Func<Task<Product>>>(), It.IsAny<TimeSpan>()))
                      .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.ProductId, result.ProductId);
            Assert.AreEqual(product.Name, result.Name);
        }

        [Test]
        public async Task GetProductByIdAsync_ProductNotInCacheOrRepo_ShouldReturnNull()
        {
            // Arrange
            _cacheMock.Setup(c => c.GetOrAddAsync($"product_{1}", It.IsAny<Func<Task<Product>>>(), It.IsAny<TimeSpan>()))
                      .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddProductAsync_ShouldAddProductToRepoAndCache()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Test Product" };

            // Act
            await _productService.AddProductAsync(product);

            // Assert
            _repositoryMock.Verify(r => r.AddProductAsync(product), Times.Once);
            _cacheMock.Verify(c => c.Remove($"product_{product.ProductId}"), Times.Once);
            _cacheMock.Verify(c => c.Add($"product_{product.ProductId}", product, It.IsAny<TimeSpan>()), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateProductInRepoAndCache()
        {
            // Arrange
            var productDto = new ProductDto { ProductId = 1, Name = "Updated Product", StatusName = "Active", Stock = 20, Price = 150.0m, Discount = 10.0m };
            var product = new Product { ProductId = 1, Name = "Original Product", Status = 0, Stock = 10, Price = 100.0m };

            _cacheMock.Setup(c => c.GetOrAddAsync($"productDto_{productDto.ProductId}", It.IsAny<Func<Task<Product>>>(), It.IsAny<TimeSpan>()))
                      .ReturnsAsync(product);
            _repositoryMock.Setup(r => r.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            await _productService.UpdateProductAsync(productDto);

            // Assert
            _repositoryMock.Verify(r => r.UpdateProductAsync(It.Is<Product>(p => p.Name == productDto.Name && p.ProductId == productDto.ProductId)), Times.Once);
            _cacheMock.Verify(c => c.Remove($"product_{productDto.ProductId}"), Times.Once);
            _cacheMock.Verify(c => c.Add($"product_{productDto.ProductId}", It.IsAny<Product>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Test]
        public void MapDtoToProduct_ShouldMapDtoToProduct()
        {
            // Arrange
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

            var product = new Product();

            // Act
            _productService.MapDtoToProduct(ref product, productDto);

            // Assert
            Assert.AreEqual(productDto.Name, product.Name);
            Assert.AreEqual(1, product.Status);
            Assert.AreEqual(productDto.Stock, product.Stock);
            Assert.AreEqual(productDto.Description, product.Description);
            Assert.AreEqual(productDto.Price, product.Price);
            Assert.AreEqual(productDto.Discount, product.Discount);
        }

        [Test]
        public async Task MapProductToDto_ShouldMapProductToDto()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 1,
                Name = "Test Product",
                Status = 1,
                Stock = 10,
                Description = "Test Description",
                Price = 100.0m,
                Discount = 10.0m,
            };

            // Act
            var result = await _productService.MapProductToDto(product);

            // Assert
            Assert.AreEqual(product.ProductId, result.ProductId);
            Assert.AreEqual(product.Name, result.Name);
            Assert.AreEqual("Active", result.StatusName);
            Assert.AreEqual(product.Stock, result.Stock);
            Assert.AreEqual(product.Description, result.Description);
            Assert.AreEqual(product.Price, result.Price);
            Assert.AreEqual(product.Discount, result.Discount);
            Assert.AreEqual(product.FinalPrice, result.FinalPrice);
        }
    }
}
