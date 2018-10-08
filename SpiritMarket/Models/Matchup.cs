using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace SpiritMarket.Models{
    public class Matchup{
        [Key]
        public int MatchupId {get; set;}

        [Required]
        public int DefendingElementalTypeId {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public int AttackingElementalTypeId {get; set;}
        public ElementalType AttackingElementalType {get; set;}

        public int EffectivenessId {get; set;}
        public Effectiveness Effectiveness {get; set;}

        public Matchup(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}