using MaleFashion_Warehouse.Server.Repositories.UnitOfWork;
using MaleFashion_Warehouse.Server.Services.Interfaces;

namespace MaleFashion_Warehouse.Server.Services.Implementations
{
    public class CartsService : ICartsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CleanupAbandonedCartsAsync()
        {
            var oneWeekAgo = DateTimeOffset.UtcNow.AddDays(-7);

            var oldCarts = await _unitOfWork.CartsRepository.GetAbandonedCartsAsync(oneWeekAgo);

            if (oldCarts.Any()) 
            {
                await _unitOfWork.CartsRepository.DeleteManyAsync(oldCarts);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
