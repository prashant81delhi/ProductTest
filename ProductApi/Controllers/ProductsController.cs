using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Repository;
using ProductsApi.Models;

namespace ProductsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsApiController : ControllerBase
    {

        private readonly IJsonRepository<Product> _repository;

        public ProductsApiController(IJsonRepository<Product> repository)
        {
            _repository = repository;
        }


        // Health check endpoint
        [HttpGet("healthcheck")]
        public IActionResult HealthCheck()
        {
            return Ok("API is healthy");
        }

        // GET: api/products
        [HttpGet]
        public ActionResult<List<Product>> GetAll()
        {
            var products = _repository.GetAll();
            return Ok(products);
        }

        // GET: api/products/colour/{colour}
        [HttpGet("colour/{colour}")]
        public ActionResult<List<Product>> GetByColor(string colour)
        {
            var products = _repository.GetByColor(colour);
            return Ok(products);
        }

        // POST: api/products
        [HttpPost]
        public ActionResult Create([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Invalid product data");

            _repository.Add(product);
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest("Invalid product data");

            var success = _repository.Update(id, updatedProduct);

            if (!success)
                return NotFound("Product not found");

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = _repository.Delete(id);

            if (!success)
                return NotFound("Product not found");

            return NoContent();
        }

    }
}
