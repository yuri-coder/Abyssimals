using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class PotentialAttack{
        [Key]
        public int PotentialAttackId {get; set;}

        public int LevelLearned {get; set;}
        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public int AbyssimalSpeciesId {get; set;}
        public AbyssimalSpecies AbyssimalSpecies {get; set;}

        public int AttackId {get; set;}
        public Attack Attack {get; set;}

        public PotentialAttack(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}