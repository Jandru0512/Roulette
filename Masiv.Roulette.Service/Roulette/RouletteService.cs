using Masiv.Roulette.Common;
using System;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public class RouletteService : IRouletteService
    {
        private readonly IRouletteDependencies _dependencies;

        public RouletteService(IRouletteDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public async Task<int> CreateRoulette()
        {
            RouletteDto roulette = new()
            {
                CreatedAt = DateTime.UtcNow,
                Status = false
            };

            return await _dependencies.CreateRoulette(roulette);
        }

    }
}
