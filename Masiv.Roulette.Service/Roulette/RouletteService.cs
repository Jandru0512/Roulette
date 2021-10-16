using Masiv.Roulette.Common;
using System;
using System.Collections.Generic;
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

        public async Task<List<BetDto>> CloseRoulette(int id)
        {
            RouletteDto roulette = await _dependencies.GetRoulette(id);
            if(roulette != null)
            {
                roulette.Status = false;
                roulette.UpdatedAt = DateTime.UtcNow;
                roulette.Winner = new Random().Next(0, 37);

                return await _dependencies.CloseRoulette(roulette);
            }
            throw new Exception("Id de ruleta no válido.");
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

        public async Task<bool> OpenRoulette(int id)
        {
            RouletteDto roulette = await _dependencies.GetRoulette(id);
            if (roulette != null)
            {
                roulette.UpdatedAt = DateTime.UtcNow;
                roulette.Status = true;

                return await _dependencies.OpenRoulette(roulette);
            }
            throw new Exception("Id de ruleta no válido.");
        }

    }
}
