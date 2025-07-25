using Microsoft.AspNetCore.Mvc;
using RestVSoapDemo.Models;
using RestVSoapDemo.Services;

namespace RestVSoapDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }
            var createdProduct = await _productRepository.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (product == null || id != product.Id)
            {
                return BadRequest("Invalid product data");
            }
            var updatedProduct = await _productRepository.UpdateAsync(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productRepository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}