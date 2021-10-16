using AutoMapper;
using Masiv.Roulette.Common;
using Masiv.Roulette.Data;
using Masiv.Roulette.Model;
using Masiv.Roulette.Service;
using System.Threading.Tasks;

namespace Masiv.Roulette.ServiceDependencies
{
    public class BetDependencies : IBetDependencies
    {
        private readonly IBetRepository _betRepository;
        private readonly ICacheHelper _cache;
        private readonly IMapper _mapper;

        public BetDependencies(IBetRepository betRepository, ICacheHelper cache, IMapper mapper)
        {
            _betRepository = betRepository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<int> CreateBet(BetDto betDto)
        {
            Bet bet = _mapper.Map<Bet>(betDto);
            int id = await _betRepository.CreateBet(bet);
            betDto.Id = id;
            await _cache.AddCache(betDto, id);

            return id;
        }
    }
}
