using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;

namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface IColorsService
    {
        Task<ResponseApi<ColorDto>> CreateAsync(ColorRequestDto colorRequestDto);
        Task<ResponseApi<object>> UpdateAsync(int id, ColorRequestDto colorRequestDto);
        Task<ResponseApi<object>> DeleteAsync(int id);

        Task<ResponseApi<ColorDto>> GetByIdAsync(int id);
        Task<ResponseApi<PageableResponse<ColorDto>>> GetPaged(PagableRequest<ColorFilterDto> pagableRequest);
    }
}
