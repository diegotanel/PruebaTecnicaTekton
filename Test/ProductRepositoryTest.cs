using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Data;

namespace Test
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private ProductContext _context;
        private ProductRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProductContext(options);
            _repository = new ProductRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            _context.Products.AddRange(new Product { ProductId = 1, Name = "Product 1", Description = "desc" }, new Product { ProductId = 2, Name = "Product 2", Description = "desc" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProductsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetProductByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Product 1", Description = "desc" };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProductByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.ProductId, result.ProductId);
            Assert.AreEqual(product.Name, result.Name);
        }

        [Test]
        public async Task GetProductByIdAsync_ProductDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetProductByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddProductAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Product 1", Description = "desc"};

            // Act
            await _repository.AddProductAsync(product);
            var result = await _context.Products.FindAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.ProductId, result.ProductId);
            Assert.AreEqual(product.Name, result.Name);
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateProductInDatabase()
        {
            // Arrange
            var product = new Product { ProductId = 1, Name = "Product 1", Description = "desc" };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            product.Name = "Updated Product";
            await _repository.UpdateProductAsync(product);
            var result = await _context.Products.FindAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.ProductId, result.ProductId);
            Assert.AreEqual("Updated Product", result.Name);
        }
    }
}
