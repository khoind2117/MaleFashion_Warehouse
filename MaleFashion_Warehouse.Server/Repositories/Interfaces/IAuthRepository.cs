using MaleFashion_Warehouse.Server.Models.Entities;

namespace MaleFashion_Warehouse.Server.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> FindByUserNameAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
