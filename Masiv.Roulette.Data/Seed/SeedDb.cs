using Masiv.Roulette.Model;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public SeedDb(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task SeedAsync()
        {
            await CheckUserAsync("Andrés", "Restrepo", "jandru@yopmail.com", "jandru", "123 123 1324", 50000.75);
        }

        private async Task CheckUserAsync(string firstName, string lastName, string email, string username, string phoneNumber, double credit)
        {
            User user = await _userRepository.GetUser(username);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = username,
                    PhoneNumber = phoneNumber,
                    Credit = credit
                };

                await _userRepository.AddUser(user, "123456");
            }
        }
    }
}
