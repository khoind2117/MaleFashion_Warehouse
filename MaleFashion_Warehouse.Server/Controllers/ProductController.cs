using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequestDto productRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await _productService.AddAsync(productRequestDto);
                return Ok(productRequestDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequestDto productRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var existingProduct = await _productService.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                await _productService.UpdateAsync(id, productRequestDto);
                return Ok("Updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "", error = ex.Message });
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] ProductFilterDto? productFilterDto)
        {
            try
            {
                var pagedProducts = await _productService.GetPagedAsync(productFilterDto);
                return Ok(pagedProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "", error = ex.Message });
            }
        }
    }
}
