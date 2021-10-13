using AutoMapper;
using Masiv.Roulette.Common;
using Masiv.Roulette.Data;
using Masiv.Roulette.Model;
using Masiv.Roulette.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Masiv.Roulette.ServiceDependencies
{
    public class LoginDependencies : ILoginDependencies
    {
        private readonly ICacheHelper _cache;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public LoginDependencies(ICacheHelper cache, IMapper mapper, SignInManager<User> signInManager, IUserRepository userRepository)
        {
            _cache = cache;
            _mapper = mapper;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUser(string username)
        {
            UserDto userDto = await _cache.ResolveChace<UserDto>(username);
            if (userDto == null)
            {
                User user = await _userRepository.GetUser(username);
                if (user != null)
                {
                    userDto = _mapper.Map<UserDto>(user);
                    await _cache.AddCache(username, userDto);
                }
            }

            return userDto;
        }

        public async Task<SignInResult> ValidateLogin(LoginDto login) => await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);
    }
}
