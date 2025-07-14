using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;

        public ProductsController(IProductsService productService)
        {
            _productService = productService;
        }

        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var products = await _productService.GetAllAsync();
        //        return Ok(products);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "", error = ex.Message });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequestDto productRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi<object>
                    {
                        Status = 400,
                        Success = false,
                        Message = "Invalid request data.",
                    });
                }

                var responseApi = await _productService.AddAsync(productRequestDto);

                if (!responseApi.Success)
                {
                    return StatusCode(responseApi.Status, responseApi);
                }

                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = ex.Message,
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequestDto productRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi<object>
                    {
                        Status = 400,
                        Success = false,
                        Message = "Invalid request data.",
                    });
                }

                var responseApi = await _productService.UpdateAsync(id, productRequestDto);

                if (!responseApi.Success)
                {
                    return StatusCode(responseApi.Status, responseApi);
                }

                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = ex.Message,
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var responseApi = await _productService.DeleteAsync(id);

                if (!responseApi.Success)
                {
                    return StatusCode(responseApi.Status, responseApi);
                }

                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = ex.Message,
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var responseApi = await _productService.GetByIdAsync(id);

                if (!responseApi.Success)
                {
                    return StatusCode(responseApi.Status, responseApi);
                }

                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = ex.Message,
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Route("paged")]
        public async Task<IActionResult> GetPaged([FromBody] PagableRequest<ProductFilterDto> pagableRequest)
        {
            try
            {
                var responseApi = await _productService.GetPaged(pagableRequest);

                if (!responseApi.Success)
                {
                    return StatusCode(responseApi.Status, responseApi);
                }

                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseApi<object>
                {
                    Status = 500,
                    Success = false,
                    Message = ex.Message,
                };
                return StatusCode(500, errorResponse);
            }
        }

    }
}
