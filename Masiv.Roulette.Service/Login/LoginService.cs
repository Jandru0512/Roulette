using Masiv.Roulette.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginDependencies _dependencies;

        public LoginService(ILoginDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public async Task<AuthenticationDto> Login(LoginDto login, TokenDto configuration)
        {
            UserDto user = await _dependencies.GetUser(login.Username);
            if (user != null)
            {
                SignInResult result = await _dependencies.ValidateLogin(login);
                if (result.Succeeded)
                {
                    Claim[] claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
                    SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    JwtSecurityToken token = new JwtSecurityToken(
                        configuration.Issuer,
                        configuration.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddDays(99),
                        signingCredentials: credentials);

                    return new AuthenticationDto
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo,
                        User = user
                    };
                }
            }

            return null;
        }
    }
}
