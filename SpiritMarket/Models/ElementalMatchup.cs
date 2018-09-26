using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiritMarket.Models{
    public class ElementalMatchup{
        [Key]
        public int ElementalMatchupId {get; set;}

        public int PercentEffectiveness {get; set;}
        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        // public int OriginalElementalTypeId {get; set;}
        // public ElementalType OriginalElementalType {get; set;}

        // public int MatchupElementalTypeId {get; set;}
        // public ElementalType MatchupElementalType {get; set;}

        public ElementalMatchup(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}