using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Order;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Services.Implementations;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaleFashion_Warehouse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] OrderRequestDto orderRequestDto)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(new ResponseApi<object>
        //            {
        //                Status = 400,
        //                Success = false,
        //                Message = "Invalid request data.",
        //            });
        //        }

        //        var responseApi = await _ordersService.CreateAsync(orderRequestDto);

        //        if (!responseApi.Success)
        //        {
        //            return StatusCode(responseApi.Status, responseApi);
        //        }

        //        return Ok(responseApi);
        //    }
        //    catch (Exception ex)
        //    {
        //        var errorResponse = new ResponseApi<object>
        //        {
        //            Status = 500,
        //            Success = false,
        //            Message = ex.Message,
        //        };
        //        return StatusCode(500, errorResponse);
        //    }
        //}

        [HttpPatch]
        [Route("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] OrderStatus status)
        {
            try
            {
                var responseApi = await _ordersService.ChangeStatusAsync(id, status);

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
                var responseApi = await _ordersService.GetByIdAsync(id);

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
        public async Task<IActionResult> GetPaged([FromBody] PagableRequest<OrderFilterDto> pagableRequest)
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

                var responseApi = await _ordersService.GetPaged(pagableRequest);

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
