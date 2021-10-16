using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Masiv.Roulette.Model
{
    public class Bet
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
        public Roulette Roulette { get; set; }
        public User User { get; set; }
    }
}
