using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Masiv.Roulette.Model
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public double Credit { get; set; }
    }
}
