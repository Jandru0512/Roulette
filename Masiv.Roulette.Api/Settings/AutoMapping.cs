using AutoMapper;
using Masiv.Roulette.Common;
using Masiv.Roulette.Model;

namespace Masiv.Roulette.Api
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Model.Roulette, RouletteDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
