using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorsService _colorsService;

        public ColorsController(IColorsService colorsService)
        {
            _colorsService = colorsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ColorRequestDto colorRequestDto)
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

                var responseApi = await _colorsService.CreateAsync(colorRequestDto);

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
        public async Task<IActionResult> Update(int id, [FromBody] ColorRequestDto colorRequestDto)
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

                var responseApi = await _colorsService.UpdateAsync(id, colorRequestDto);

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
                var responseApi = await _colorsService.DeleteAsync(id);

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
                var responseApi = await _colorsService.GetByIdAsync(id);

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
        public async Task<IActionResult> GetPaged([FromBody] PagableRequest<ColorFilterDto> pagableRequest)
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

                var responseApi = await _colorsService.GetPaged(pagableRequest);

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
