using ApplicationServices;
using LazyCache;
using Moq;
using Repositories;
using Models;
using Shared.External.ApiClientLibrary;

namespace Test
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _repositoryMock;
        private IAppCache _cache;
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _cache = new CachingService();
            _service = new ProductService(_repositoryMock.Object, _cache);
        }

        //[Test]
        //public async Task GetProductByIdAsync_ReturnsProduct()
        //{
        //    var product = new Product { ProductId = 1, Name = "Test Product" };
        //    _repositoryMock.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(product);

        //    var result = await _service.GetProductByIdAsync(1);

        //    Assert.AreEqual(product, result);
        //    _repositoryMock.Verify(r => r.GetProductByIdAsync(1), Times.Once);
        //}

        [Test]
        public async Task TestConectionMockAPI()
        {
            //string apiUrl = "https://6675ff2ba8d2b4d072f21eb8.mockapi.io/api/preciodedescuento/numerosaleatorios/1";
            string apiUrl = "https://api.generadordni.es/v2/bank/card";
            var mock = new MockApi();
            var datos = mock.GetApiDataAsync(apiUrl);
        }
    }

}