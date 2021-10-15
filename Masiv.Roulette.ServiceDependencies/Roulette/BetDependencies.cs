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
        private readonly IMapper _mapper;

        public BetDependencies(IBetRepository betRepository, IMapper mapper)
        {
            _betRepository = betRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateBet(BetDto betDto)
        {
            Bet bet = _mapper.Map<Bet>(betDto);

            return await _betRepository.CreateBet(bet);
        }
    }
}
