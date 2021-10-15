using Masiv.Roulette.Data;
using Masiv.Roulette.Model;
using Masiv.Roulette.Service;
using Masiv.Roulette.ServiceDependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Text;

namespace Masiv.Roulette.Api
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AddIndentity(services);
            services.AddAuthentication()
                .AddJwtBearer(config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = _configuration["Tokens:Issuer"],
                        ValidAudience = _configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
                    };
                });

            services.AddDbContext<DataContext>(x => x.UseMySQL(Database.BuildConnectionString(_configuration)));
            services.AddScoped<DbConnection>(x => new MySqlConnection(Database.BuildConnectionString(_configuration)));
            services.AddTransient<SeedDb>();
            AddInterfaces(services);
            services.AddAutoMapper(typeof(Startup));
            services.AddMemoryCache();
            services.AddDistributedRedisCache(configuration =>
            {
                configuration.Configuration = Cache.BuildConnectionString(_configuration);
                configuration.InstanceName = _configuration["redis:InstanceName"];
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddIndentity(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(x =>
            {
                x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                x.SignIn.RequireConfirmedEmail = false;
                x.User.RequireUniqueEmail = true;
                x.Password.RequireDigit = false;
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DataContext>();
        }

        private static void AddInterfaces(IServiceCollection services)
        {
            services.AddScoped<IBetDependencies, BetDependencies>();
            services.AddScoped<IBetRepository, BetRepository>();
            services.AddScoped<IBetService, BetService>();
            services.AddScoped<ICacheHelper, CacheHelper>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ILoginDependencies, LoginDependencies>();
            services.AddScoped<IQueryHelper, QueryHelper>();
            services.AddScoped<IRouletteDependencies, RouletteDependencies>();
            services.AddScoped<IRouletteRepository, RouletteRepository>();
            services.AddScoped<IRouletteService, RouletteService>();
            services.AddScoped<ITransactionalWrapper, TransactionalWrapper>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
