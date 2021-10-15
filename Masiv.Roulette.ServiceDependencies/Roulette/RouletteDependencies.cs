using AutoMapper;
using Masiv.Roulette.Common;
using Masiv.Roulette.Data;
using Masiv.Roulette.Service;
using System.Threading.Tasks;

namespace Masiv.Roulette.ServiceDependencies
{
    public class RouletteDependencies : IRouletteDependencies
    {
        private readonly IMapper _mapper;
        private readonly IRouletteRepository _repository;

        public RouletteDependencies(IMapper mapper, IRouletteRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> CreateRoulette(RouletteDto rouletteDto)
        {
            Model.Roulette roulette = _mapper.Map<Model.Roulette>(rouletteDto);

            return await _repository.CreateRoulette(roulette);
        }

        public async Task<bool> OpenRoulette(RouletteDto rouletteDto)
        {
            Model.Roulette roulette = _mapper.Map<Model.Roulette>(rouletteDto);

            return await _repository.OpenRoulette(roulette);
        }
    }
}
