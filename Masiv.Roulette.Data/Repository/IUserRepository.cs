using Masiv.Roulette.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public interface IUserRepository
    {
        Task<IdentityResult> AddUser(User user, string password);
        Task<User> GetUser(string username);
        Task<User> InsertSingle(User user);
        Task<User> UpdateSingle(User user);
    }
}