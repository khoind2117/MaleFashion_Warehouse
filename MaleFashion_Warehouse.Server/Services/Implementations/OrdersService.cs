using MaleFashion_Warehouse.Server.Common.Dtos;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Dtos.Order;
using MaleFashion_Warehouse.Server.Models.Dtos.OrderItem;
using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.UnitOfWork;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public OrdersService(IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ResponseApi<OrderDetailDto>> CreateAsync(OrderRequestDto productRequestDto)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                return new ResponseApi<OrderDetailDto>
                {
                    Status = 401,
                    Success = false,
                    Message = "not_authenticated"
                };
            }

            var cartItems = await _unitOfWork.CartItemsRepository.GetAllNotPagedAsync(
                filter: ci => ci.Cart.UserId == user.Id,
                include: q => q.Include(ci => ci.ProductVariant)
                                .ThenInclude(pv => pv.Product)
            );
            if (cartItems == null || !cartItems.Any())
            {
                return new ResponseApi<OrderDetailDto>
                {
                    Status = 400,
                    Success = false,
                    Message = "empty_cart"
                };
            }

            foreach (var item in cartItems)
            {
                if (item.ProductVariant.Stock < item.Quantity)
                {
                    return new ResponseApi<OrderDetailDto>
                    {
                        Status = 400,
                        Success = false,
                        Message = $"Not enough stock for {item.ProductVariant.Product.Name}"
                    };
                }
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var total = cartItems.Sum(ci => ci.Quantity * (decimal)ci.ProductVariant.Product.PriceVnd);

                var order = new Order
                {
                    FirstName = productRequestDto.FirstName,
                    LastName = productRequestDto.LastName,
                    Address = productRequestDto.Address,
                    PhoneNumber = productRequestDto.PhoneNumber,
                    Email = productRequestDto.Email,
                    Note = productRequestDto.Note,
                    Total = total,
                    Currency = productRequestDto.Currency,
                    PaymentMethod = productRequestDto.PaymentMethod,
                    Status = OrderStatus.Pending,
                    CreatedDate = DateTimeOffset.Now,
                    UserId = user.Id,
                };

                await _unitOfWork.OrdersRepository.CreateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                var orderItems = cartItems.Select(item =>
                {
                    item.ProductVariant.Stock -= item.Quantity;

                    return new OrderItem
                    {
                        OrderId = order.Id,
                        ProductVariantId = item.ProductVariantId!.Value,
                        ProductName = item.ProductVariant.Product.Name,
                        ProductColor = item.ProductVariant.Color.Name,
                        ProductSize = item.ProductVariant.Size,
                        Quantity = item.Quantity,
                        UnitPrice = (decimal)item.ProductVariant.Product.PriceVnd
                    };
                }).ToList();

                await _unitOfWork.OrderItemsRepository.CreateManyAsync(orderItems);

                await _unitOfWork.CartItemsRepository.DeleteManyAsync(cartItems);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return new ResponseApi<OrderDetailDto>
                {
                    Status = 200,
                    Success = true,
                    Message = "Order created successfully",
                    Data = new OrderDetailDto
                    {
                        FirstName = order.FirstName,
                        LastName = order.LastName,
                        Email = order.Email,
                        Address = order.Address,
                        PhoneNumber = order.PhoneNumber,
                        Currency = order.Currency,
                        Total = order.Total,
                        CreatedDate = order.CreatedDate,
                        Status = order.Status,
                        OrderItems = cartItems.Select(ci => new OrderItemDetailDto
                        {
                            ProductName = ci.ProductVariant.Product.Name,
                            ProductColor = ci.ProductVariant.Color.Name,
                            ProductSize = ci.ProductVariant.Size,
                            ProductVariantId = ci.ProductVariantId,
                            Quantity = ci.Quantity,
                            UnitPrice = (decimal)ci.ProductVariant.Product.PriceVnd
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ResponseApi<OrderDetailDto>
                {
                    Status = 500,
                    Success = false,
                    Message = $"Order creation failed: {ex.Message}"
                };
            }
        }

        public Task<ResponseApi<object>> UpdateAsync(int id, OrderRequestDto productRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseApi<object>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseApi<object>> ChangeStatusAsync(int id, OrderStatus status)
        {
            var order = await _unitOfWork.OrdersRepository.GetByIdAsync(id);
            if (order == null)
            {
                return new ResponseApi<object>
                {
                    Status = 404,
                    Success = false,
                    Message = "not_found"
                };
            }

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "User"))
            {
                if (order.UserId != user.Id || order.Status != OrderStatus.Pending)
                {
                    return new ResponseApi<object>
                    {
                        Status = 403,
                        Success = false,
                        Message = "permission_denied"
                    };
                }
            }

            var isSuccess = await _unitOfWork.OrdersRepository.ChangeStatusAsync(id, status);

            if (!isSuccess)
            {
                return new ResponseApi<object>
                {
                    Status = 400,
                    Success = false,
                    Message = "Failed to change status",
                };
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResponseApi<object>
            {
                Status = 200,
                Success = true,
                Message = "Status updated successfully",
            };
        }

        public async Task<ResponseApi<OrderDetailDto>> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.OrdersRepository.GetByIdAsync(
                id: id,
                include: o => o.Include(o => o.OrderItems)
            );

            if (order == null)
            {
                return new ResponseApi<OrderDetailDto>
                {
                    Status = 404,
                    Success = false,
                    Message = "not_found"
                };
            }

            var orderDetailDto = new OrderDetailDto
            {
                Id = order.Id,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                Email = order.Email,
                Note = order.Note,
                Total = order.Total,
                Currency = order.Currency,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                CreatedDate = order.CreatedDate,
                UserId = order.UserId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDetailDto
                {
                    ProductName = oi.ProductName,
                    ProductColor = oi.ProductColor,
                    ProductSize = oi.ProductSize,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Currency = oi.Currency,
                    ProductVariantId = oi.ProductVariantId,
                    OrderId = oi.OrderId,
                }).ToList(),
            };

            return new ResponseApi<OrderDetailDto>
            {
                Status = 200,
                Success = true,
                Data = orderDetailDto,
            };
        }

        public async Task<ResponseApi<PageableResponse<OrderListDto>>> GetPaged(PagableRequest<OrderFilterDto> pagableRequest)
        {
            var criteria = pagableRequest.Criteria;
            var page = pagableRequest.Page;
            var size = pagableRequest.Size;
            var sortField = pagableRequest.SortBy;
            var sortDirection = pagableRequest.SortDirection;

            Func<OrderFilterDto?, IQueryable<Order>, IQueryable<Order>>? filter = null;
            if (criteria != null)
            {
                filter = (criteria, query) =>
                {
                    if (!string.IsNullOrWhiteSpace(criteria.FirstName))
                        query = query.Where(o => o.FirstName.Contains(criteria.FirstName));
                    if (!string.IsNullOrWhiteSpace(criteria.LastName))
                        query = query.Where(o => o.LastName.Contains(criteria.LastName));
                    if (!string.IsNullOrWhiteSpace(criteria.Email))
                        query = query.Where(o => o.Email.Contains(criteria.Email));
                    if (criteria.Total > 0)
                        query = query.Where(o => o.Total == criteria.Total);
                    if (criteria.Currency != default)
                        query = query.Where(o => o.Currency == criteria.Currency);
                    if (criteria.PaymentMethod != default)
                        query = query.Where(o => o.PaymentMethod == criteria.PaymentMethod);
                    if (criteria.CreatedDate != default)
                        query = query.Where(o => o.CreatedDate.Date == criteria.CreatedDate.Date);

                    return query;
                };
            }

            var pagedData = await _unitOfWork.OrdersRepository.GetPagedAsync(
                pagableRequest: pagableRequest,
                filter: filter,
                include: null
            );

            var dtoPaged = new PageableResponse<OrderListDto>
            {
                Content = pagedData.Content?.Select(o => new OrderListDto
                {
                    Id = o.Id,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    Address = o.Address,
                    PhoneNumber = o.PhoneNumber,
                    Email = o.Email,
                    Total = o.Total,
                    Currency = o.Currency,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate,
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

            return new ResponseApi<PageableResponse<OrderListDto>>
            {
                Status = 200,
                Success = true,
                Data = dtoPaged,
            };
        }
    }
}
