//using System;
//using System.Threading.Tasks;
//using ApplicationServices;
//using LazyCache;
//using LazyCache.Testing.Moq;
//using Models;
//using Moq;
//using NUnit.Framework;
//using Repositories;
//using Shared.DTOs;

//namespace ApplicationServices.Tests
//{
//    [TestFixture]
//    public class ProductServiceTests
//    {
//        private Mock<IProductRepository> _repositoryMock;
//        private IAppCache _cacheMock;
//        private ProductService _productService;

//        [SetUp]
//        public void SetUp()
//        {
//            _repositoryMock = new Mock<IProductRepository>();
//            _cacheMock = Create.MockedCachingService();
//            _productService = new ProductService(_repositoryMock.Object, _cacheMock);
//        }

//        [Test]
//        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
//        {
//            // Arrange
//            int productId = 1;
//            var product = new Product { ProductId = productId };
//            _repositoryMock.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);
//            _cacheMock.Setup(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<Product>>>(), It.IsAny<TimeSpan>())).ReturnsAsync(product);

//            // Act
//            var result = await _productService.GetProductByIdAsync(productId);

//            // Assert
//            Assert.IsNotNull(result);
//            _repositoryMock.Verify(r => r.GetProductByIdAsync(productId), Times.Once);
//            _cacheMock.Verify(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<Product>>>(), It.IsAny<TimeSpan>()), Times.Once);
//        }

//        [Test]
//        public async Task AddProductAsync_ShouldCallRepositoryAndCache()
//        {
//            // Arrange
//            var product = new Product { ProductId = 1 };


//            // Act
//            await _productService.AddProductAsync(product);

//            // Assert
//            //_repositoryMock.Verify(r => r.AddProductAsync(product), Times.Once);
//            //_cacheMock.Verify(c => c.Remove(It.IsAny<string>()), Times.Once);
//            //_cacheMock.Verify(c => c.Add(It.IsAny<string>(), product, It.IsAny<TimeSpan>()), Times.Once);
            
//        }

//        [Test]
//        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
//        {
//            // Arrange
//            var productDto = new ProductDto { ProductId = 1 };
//            var product = new Product { ProductId = productDto.ProductId };
//            _repositoryMock.Setup(r => r.GetProductByIdAsync(productDto.ProductId)).ReturnsAsync(product);

//            // Act
//            await _productService.UpdateProductAsync(productDto);

//            // Assert
//            _repositoryMock.Verify(r => r.GetProductByIdAsync(productDto.ProductId), Times.Once);
//            _repositoryMock.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
//            _cacheMock.Verify(c => c.Remove(It.IsAny<string>()), Times.Once);
//            _cacheMock.Verify(c => c.Add(It.IsAny<string>(), product, It.IsAny<TimeSpan>()), Times.Once);
//        }

//        [Test]
//        public async Task TestsPruebaLazyCacheMoq()
//        {
//            var cacheEntryKey = "prueba";
//            var expectedResult = "24a4d5a5-bd7a-49b6-ba36-cf4ef3ec5510";

//            var mockedCache = Create.MockedCachingService();

//            var actualResult = mockedCache.GetOrAdd(cacheEntryKey, () => expectedResult, DateTimeOffset.Now.AddMinutes(30));

//            Assert.That(actualResult, Is.EqualTo(expectedResult));
//        }
        
//    }
//}

