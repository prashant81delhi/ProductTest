
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Repository;
using ProductsApi.Controllers;
using ProductsApi.Models;

namespace ProductsControllerTests
{
        [TestFixture]
        public class UnitTest
        {
            private Mock<IJsonRepository<Product>> _mockRepo;
            private ProductsApiController _controller;

            [SetUp]
            public void Setup()
            {
                _mockRepo = new Mock<IJsonRepository<Product>>(); // Pass mock file path
                _controller = new ProductsApiController(_mockRepo.Object);  // Inject the mocked repo
            }

            // Unit Test: Get all products
            [Test]
            public void GetAll_ReturnsOkResult_WithListOfProducts()
            {
                // Arrange
                var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Color = "Red", Price = 10.5M },
                new Product { Id = 2, Name = "Product 2", Color = "Blue", Price = 15.5M }
            };

                _mockRepo.Setup(repo => repo.GetAll()).Returns(products);

                // Act
                var result = _controller.GetAll();

                // Assert
                var actionResult = result;
                Assert.That(() => actionResult, Throws.Nothing);
                var returnValue = actionResult.Value as List<Product>;
                Assert.IsNotNull(returnValue);
                Assert.AreEqual(2, returnValue.Count);
            }

            // Unit Test: Get products by color
            [Test]
            public void GetByColor_ReturnsOkResult_WithFilteredProducts()
            {
                // Arrange
                var products = new List<Product>
{
new Product { Id = 1, Name = "Product 1", Color = "Red", Price = 10.5M },
                new Product { Id = 2, Name = "Product 2", Color = "Red", Price = 15.5M },
                new Product { Id = 3, Name = "Product 3", Color = "Blue", Price = 12.0M }
            };

                _mockRepo.Setup(repo => repo.GetByColor("Red")).Returns(products.FindAll(p => p.Color == "Red"));

                // Act
                var result = _controller.GetByColor("Red");

                // Assert
                var actionResult = result as ActionResult<List<Product>>;
                Assert.That(() => actionResult, Throws.Nothing);

                var returnValue = actionResult.Value as List<Product>;
                Assert.IsNotNull(returnValue);
                Assert.AreEqual(2, returnValue.Count);  // Expecting 2 products with the "Red" color
            }

            // Unit Test: Create a new product
            [Test]
            public void CreateProduct_ReturnsCreatedAtActionResult_WhenProductIsCreated()
            {
                // Arrange
                var newProduct = new Product { Id = 3, Name = "Product 3", Color = "Green", Price = 20.5M };

                _mockRepo.Setup(repo => repo.Add(It.IsAny<Product>()));

                // Act
                var result = _controller.Create(newProduct);

                // Assert

                var actionResult = result as CreatedAtActionResult;
                Assert.NotNull(actionResult);
                var returnValue = actionResult.Value as Product;
                Assert.IsNotNull(returnValue);
                Assert.AreEqual(newProduct.Id, returnValue.Id);
            }

            // Unit Test: Update a product
            [Test]
            public void UpdateProduct_ReturnsNoContent_WhenProductIsUpdated()
            {
                // Arrange
                var updatedProduct = new Product { Id = 1, Name = "Updated Product", Color = "Yellow", Price = 12.0M };

                _mockRepo.Setup(repo => repo.Update(1, updatedProduct)).Returns(true);

                // Act
                var result = _controller.Update(1, updatedProduct);

                // Assert
                Assert.IsInstanceOf<NoContentResult>(result);  // Expecting "NoContent" response after successful update
            }

            // Unit Test: Delete a product
            [Test]
            public void DeleteProduct_ReturnsNoContent_WhenProductIsDeleted()
            {
                // Arrange
                _mockRepo.Setup(repo => repo.Delete(1)).Returns(true);

                // Act
                var result = _controller.Delete(1);

                // Assert
                Assert.IsInstanceOf<NoContentResult>(result);  // Expecting "NoContent" response after successful deletion

            }
        }
    
}




