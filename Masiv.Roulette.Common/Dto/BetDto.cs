using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masiv.Roulette.Common
{
    public class BetDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Color { get; set; }
        public int Number { get; set; }
        public double Prize { get; set; }
        public bool Status { get; set; }
        public double Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RouletteId { get; set; }
        public string UserId { get; set; }
        public UserDto User { get; set; }
    }
}
