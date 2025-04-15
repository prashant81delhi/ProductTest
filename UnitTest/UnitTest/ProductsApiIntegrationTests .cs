using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using ProductsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductsControllerTests
{
    [TestFixture]
    public class ProductsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // Integration Test: Get all products
        [Test]
        public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            response.EnsureSuccessStatusCode();  // Status Code 200-299
            var products = await response.Content.ReadAsAsync<List<Product>>();
            Assert.IsNotEmpty(products);
        }

        // Integration Test: Create a new product
        [Test]
        public async Task CreateProduct_ReturnsCreatedAtActionResult_WhenProductIsCreated()
        {
            // Arrange
            var newProduct = new Product { Name = "New Product", Color = "Purple", Price = 25.0M };
            var content = new StringContent(JsonConvert.SerializeObject(newProduct), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/products", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadAsAsync<Product>();
            Assert.AreEqual(newProduct.Name, createdProduct.Name);
        }
        // Integration Test: Get products by color
        [Test]
        public async Task GetProductsByColour_ReturnsOkResult_WithFilteredProducts()
        {
            // Act
            var response = await _client.GetAsync("/api/products/colour/Red");

            // Assert
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadAsAsync<List<Product>>();

            foreach (var item in products)
            {
                Assert.AreEqual("Red", item.Color);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        { 
            _client.Dispose();
        }
    }



}
