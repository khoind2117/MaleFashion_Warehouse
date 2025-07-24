namespace MaleFashion_Warehouse.Server.Services.Interfaces
{
    public interface ICartsService
    {
        Task CleanupAbandonedCartsAsync();
    }
}
