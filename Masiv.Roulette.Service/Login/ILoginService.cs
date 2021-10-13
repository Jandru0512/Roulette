using Masiv.Roulette.Common;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface ILoginService
    {
        Task<AuthenticationDto> Login(LoginDto login, TokenDto configuration);
    }
}