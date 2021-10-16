using System;
using System.ComponentModel.DataAnnotations;

namespace Masiv.Roulette.Model
{
    public class Roulette
    {
        [Key]
        public int Id { get; set; }
        public bool Status { get; set; }
        public int Winner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
