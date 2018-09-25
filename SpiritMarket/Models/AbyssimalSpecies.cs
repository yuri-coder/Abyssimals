using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class AbyssimalSpecies{
        [Key]
        public int AbyssimalSpeciesId {get; set;}

        [Required]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required]
        [MaxLength(255)]
        public string Description {get; set;}

        //Passed onto user Abyssimals and used for leveling
        [Required]
        public int BaseHealth {get; set;}

        [Required]
        public int BaseStrength {get; set;}

        [Required]
        public int BaseDefence {get; set;}

        [Required]
        public int BaseMagic {get; set;}

        [Required]
        public int BaseResistance {get; set;}

        [Required]
        public int BaseSpeed {get; set;}

        [Required] 
        public int BaseMP {get; set;}


        //For use with Wild Abyssimals
        [Required]
        public int SubdueRate {get; set;}

        [Required]
        public int BaseExperience {get; set;}
        
        [Required]
        public int LevelingSpeed {get; set;}

        public List<AbyssimalElementalType> ElementalTypes {get; set;}
        public List<PotentialAttack> PotentialAttacks {get; set;}
        public List<Abyssimal> AbyssimalsOfThisSpecies {get; set;}
        public List<AbyssimalSpeciesGroup> AbyssimalGroups {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public AbyssimalSpecies(){
            ElementalTypes = new List<AbyssimalElementalType>();
            PotentialAttacks = new List<PotentialAttack>();
            AbyssimalsOfThisSpecies = new List<Abyssimal>();
            AbyssimalGroups = new List<AbyssimalSpeciesGroup>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}