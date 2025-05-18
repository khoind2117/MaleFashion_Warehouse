using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository 
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User?> FindByUserNameAsync(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username.ToLower());
            return user;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }
    }
}
