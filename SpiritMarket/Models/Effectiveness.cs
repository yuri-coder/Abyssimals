using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace SpiritMarket.Models{
    public class Effectiveness{
        [Key]
        public int EffectivenessId {get; set;}

        [Required]
        public double Multiplier {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public List<Matchup> Matchups {get; set;}

        public Effectiveness(){
            Matchups = new List<Matchup>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}