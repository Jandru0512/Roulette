using AutoMapper;
using Masiv.Roulette.Common;
using Masiv.Roulette.Data;
using Masiv.Roulette.Model;
using Masiv.Roulette.Service;
using System.Collections.Generic;
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

        public async Task<List<BetDto>> CloseRoulette(RouletteDto rouletteDto)
        {
            Model.Roulette roulette = _mapper.Map<Model.Roulette>(rouletteDto);
            List<Bet> bets = await _repository.CloseRoulette(roulette);
            List<BetDto> betsDto = _mapper.Map<List<BetDto>>(bets);
            await _cache.AddCache(rouletteDto, rouletteDto.Id.ToString());
            foreach (BetDto bet in betsDto)
            {
                await _cache.AddCache(bet, bet.Id.ToString());
            }

            return betsDto;
        }

        public async Task<int> CreateRoulette(RouletteDto rouletteDto)
        {
            Model.Roulette roulette = _mapper.Map<Model.Roulette>(rouletteDto);
            int id = await _repository.CreateRoulette(roulette);
            rouletteDto.Id = id;
            await _cache.AddCache(rouletteDto, rouletteDto.Id.ToString());

            return id;
        }

        public async Task<RouletteDto> GetRoulette(int id)
        {
            RouletteDto rouletteDto = await _cache.ResolveCache<RouletteDto>(id.ToString());
            if (rouletteDto == null)
            {
                Model.Roulette roulette = await _repository.GetRoulette(id);
                if (roulette != null)
                {
                    rouletteDto = _mapper.Map<RouletteDto>(roulette);
                    await _cache.AddCache(rouletteDto, id.ToString());
                }
            }

            return rouletteDto;
        }

        public async Task<List<RouletteDto>> List()
        {
            List<RouletteDto> roulettesDto = await _cache.ResolveListCache<RouletteDto>();
            if (roulettesDto == null)
            {
                List<Model.Roulette> roulettes = await _repository.List();
                if (roulettes != null)
                {
                    roulettesDto = _mapper.Map<List<RouletteDto>>(roulettes);
                    await _cache.AddListCache(roulettesDto);
                }
            }

            return roulettesDto;
        }

        public async Task<bool> OpenRoulette(RouletteDto rouletteDto)
        {
            Model.Roulette roulette = _mapper.Map<Model.Roulette>(rouletteDto);
            bool status = await _repository.OpenRoulette(roulette);
            await _cache.AddCache(rouletteDto, rouletteDto.Id.ToString());

            return status;
        }
    }
}
