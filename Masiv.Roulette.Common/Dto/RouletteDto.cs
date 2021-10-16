using System;

namespace Masiv.Roulette.Common
{
    public class RouletteDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public int Winner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
