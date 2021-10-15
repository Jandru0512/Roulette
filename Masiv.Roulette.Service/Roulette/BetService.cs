using Masiv.Roulette.Common;
using System;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public class BetService : IBetService
    {
        private readonly IBetDependencies _dependencies;
        private readonly ILoginDependencies _loginDependencies;
        private readonly IRouletteDependencies _rouletteDependencies;

        public BetService(IBetDependencies dependencies, ILoginDependencies loginDependencies, IRouletteDependencies rouletteDependencies)
        {
            _dependencies = dependencies;
            _loginDependencies = loginDependencies;
            _rouletteDependencies = rouletteDependencies;
        }

        public async Task<int> CreateBet(BetDto bet, string username)
        {
            UserDto user = await _loginDependencies.GetUser(username);
            if (user.Credit >= bet.Value)
            {
                if (await ValidateBet(bet))
                {
                    bet.CreatedAt = DateTime.UtcNow;
                    bet.Prize = 0;
                    bet.Status = true;
                    bet.User = user;
                    bet.User.Credit -= bet.Value;
                    bet.UserId = user.Id;

                    return await _dependencies.CreateBet(bet);
                }
            }
            throw new Exception("El usuario no posee crédito suficiente.");
        }

        private async Task<bool> ValidateBet(BetDto bet)
        {
            ValidateBetType(bet);
            await ValidateRoulette(bet);
            if (bet.Value > 10000)
            {
                throw new Exception("El valor apostado supera el monto máximo permitido.");
            }

            return true;
        }

        private static void ValidateBetType(BetDto bet)
        {
            if (bet.Color == string.Empty)
            {
                if (bet.Number < (int)Number.min || bet.Number > (int)Number.max)
                {
                    throw new Exception("El número se encuentra fuera del rango de la ruleta.");
                }
            }
            else
            {
                if (bet.Color != Color.Black && bet.Color != Color.Red)
                {
                    throw new Exception("El color es inválido, debe ser rojo o negro.");
                }
            }
        }

        private async Task<bool> ValidateRoulette(BetDto bet)
        {
            RouletteDto roulette = await _rouletteDependencies.GetRoulette(bet.RouletteId);
            if (roulette != null)
            {
                if (!roulette.Status)
                {
                    throw new Exception("La ruleta seleccionada se encuentra cerrada.");
                }

                return true;
            }
            throw new Exception("El id de ruleta ingresado no es válido.");
        }
    }
}
