using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Color;
using MaleFashion_Warehouse.Server.Models.Dtos.Product;
using MaleFashion_Warehouse.Server.Models.Dtos.ProductVariant;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.UnitOfWork;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using MaleFashion_Warehouse.Server.Utils;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class ColorsService : IColorsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ColorsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseApi<ColorDto>> CreateAsync(ColorRequestDto colorRequestDto)
        {
            var color = new Color
            {
                Name = colorRequestDto.Name,
                ColorHex = colorRequestDto.ColorHex,
            };

            await _unitOfWork.ColorsRepository.CreateAsync(color);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(color.Id);
        }

        public async Task<ResponseApi<object>> UpdateAsync(int id, ColorRequestDto colorRequestDto)
        {
            var color = await _unitOfWork.ColorsRepository.GetByIdAsync(id);
            if (color == null)
            {
                return new ResponseApi<object>
                {
                    Status = 404,
                    Success = false,
                    Message = "Product not found"
                };
            }

            color.Name = colorRequestDto.Name;
            color.ColorHex = colorRequestDto.ColorHex;

            await _unitOfWork.ColorsRepository.UpdateAsync(color);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseApi<object>
            {
                Status = 200,
                Success = true,
                Message = "Updated successfully.",
            };
        }

        public async Task<ResponseApi<object>> DeleteAsync(int id)
        {
            var product = await _unitOfWork.ColorsRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ResponseApi<object>
                {
                    Status = 404,
                    Success = false,
                    Message = "Product not found"
                };
            }

            await _unitOfWork.ColorsRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseApi<object>
            {
                Status = 200,
                Success = true,
                Message = "Deleted successfully",
            };
        }

        public async Task<ResponseApi<ColorDto>> GetByIdAsync(int id)
        {
            var color = await _unitOfWork.ColorsRepository.GetByIdAsync(
                id: id
            );

            if (color == null)
            {
                return new ResponseApi<ColorDto>
                {
                    Status = 404,
                    Success = false,
                    Message = "not_found",
                };
            }

            var colorDto = new ColorDto
            {
                Id = color.Id,
                Name = color.Name,
                ColorHex = color.ColorHex,
            };

            return new ResponseApi<ColorDto>
            {
                Status = 200,
                Success = true,
                Data = colorDto,
            };
        }

        public async Task<ResponseApi<PageableResponse<ColorDto>>> GetPaged(PagableRequest<ColorFilterDto> pagableRequest)
        {
            var criteria = pagableRequest.Criteria;
            var page = pagableRequest.Page;
            var size = pagableRequest.Size;
            var sortField = pagableRequest.SortBy;
            var sortDirection = pagableRequest.SortDirection;

            Func<ColorFilterDto?, IQueryable<Color>, IQueryable<Color>>? filter = null;
            if (criteria != null)
            {
                filter = (criteria, query) =>
                {
                    if (!string.IsNullOrWhiteSpace(criteria?.Name))
                        query = query.Where(p => p.Name.Contains(criteria.Name));
                    if (!string.IsNullOrWhiteSpace(criteria?.ColorHex))
                        query = query.Where(p => p.Name.Contains(criteria.ColorHex));

                    return query;
                };
            }

            var pagedData = await _unitOfWork.ColorsRepository.GetPagedAsync(
                pagableRequest: pagableRequest,
                filter: filter,
                include: null
            );

            var dtoPaged = new PageableResponse<ColorDto>
            {
                Content = pagedData.Content?.Select(c => new ColorDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ColorHex = c.ColorHex,
                }).ToList(),
                TotalElements = pagedData.TotalElements,
                TotalPages = pagedData.TotalPages,
                Number = pagedData.Number,
                Size = pagedData.Size,
                NumberOfElements = pagedData.NumberOfElements,
                First = pagedData.First,
                Last = pagedData.Last,
                Empty = pagedData.Empty,
                Sort = pagedData.Sort,
                Pageable = pagedData.Pageable
            };

            return new ResponseApi<PageableResponse<ColorDto>>
            {
                Status = 200,
                Success = true,
                Data = dtoPaged,
            };
        }
    }
}