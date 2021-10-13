using Masiv.Roulette.Common;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface ILoginDependencies
    {
        Task<UserDto> GetUser(string username);
        Task<SignInResult> ValidateLogin(LoginDto login);
    }
}