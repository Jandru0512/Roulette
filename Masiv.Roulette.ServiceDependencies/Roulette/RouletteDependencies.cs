using AutoMapper;
using Masiv.Roulette.Common;
using Masiv.Roulette.Data;
using Masiv.Roulette.Service;
using System.Threading.Tasks;

namespace Masiv.Roulette.ServiceDependencies
{
    public class RouletteDependencies : IRouletteDependencies
    {
        private readonly ICacheHelper _cache;
        private readonly IMapper _mapper;
        private readonly IRouletteRepository _repository;

        public RouletteDependencies(ICacheHelper cache, IMapper mapper, IRouletteRepository repository)
        {
            _cache = cache;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> CreateRoulette(RouletteDto rouletteDto)
        {
            Model.Roulette roulette = _mapper.Map<Model.Roulette>(rouletteDto);

            return await _repository.CreateRoulette(roulette);
        }

        public async Task<RouletteDto> GetRoulette(int id)
        {
            RouletteDto rouletteDto = await _cache.ResolveChace<RouletteDto>(id);
            if (rouletteDto == null)
            {
                Model.Roulette roulette = await _repository.GetRoulette(id);
                if (roulette != null)
                {
                    rouletteDto = _mapper.Map<RouletteDto>(roulette);
                    await _cache.AddCache(id, rouletteDto);
                }
            }

            return rouletteDto;
        }

        public async Task<bool> OpenRoulette(RouletteDto rouletteDto)
        {
            Model.Roulette roulette = _mapper.Map<Model.Roulette>(rouletteDto);

            return await _repository.OpenRoulette(roulette);
        }
    }
}
