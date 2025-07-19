using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var products = await _productsService.GetAllAsync();
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

                var responseApi = await _productsService.CreateAsync(productRequestDto);

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

                var responseApi = await _productsService.UpdateAsync(id, productRequestDto);

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
                var responseApi = await _productsService.DeleteAsync(id);

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

        [HttpPatch]
        [Route("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ProductStatus status)
        {
            try
            {
                var responseApi = await _productsService.ChangeStatusAsync(id, status);

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
                var responseApi = await _productsService.GetByIdAsync(id);

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
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseApi<object>
                    {
                        Status = 400,
                        Success = false,
                        Message = "Invalid request data.",
                    });
                }

                var responseApi = await _productsService.GetPaged(pagableRequest);

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
