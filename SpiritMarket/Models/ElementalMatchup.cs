using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class ElementalMatchup{

        public int PercentEffectiveness {get; set;}
        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public int OriginalElementalTypeId {get; set;}
        public ElementalType OriginalType {get; set;}

        public int MatchupElementalTypeId {get; set;}
        public ElementalType MatchupType {get; set;}

        public ElementalMatchup(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}