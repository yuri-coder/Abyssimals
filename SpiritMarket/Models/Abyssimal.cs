using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiritMarket.Models{
    public class Abyssimal{
        [Key]
        public int AbyssimalId {get; set;}

        [Required]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required]
        [MaxLength(255)]
        public string Description {get; set;}

        public int Level {get; set;}
        public int Health {get; set;}
        public int Strength {get; set;}
        public int Defence {get; set;}
        public int Magic {get; set;}
        public int Resistance {get; set;}
        public int Speed {get; set;}
        public int MP {get; set;}
        public int Experience {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public List<AbyssimalStatuses> CurrentStatuses {get; set;}
        public List<LearnedAttack> LearnedAttacks {get; set;}

        public int UserId {get; set;}
        public User User {get; set;}

        public int AbyssimalSpeciesId {get; set;}
        public AbyssimalSpecies AbyssimalSpecies {get; set;}

        public Abyssimal(){
            CurrentStatuses = new List<AbyssimalStatuses>();
            LearnedAttacks = new List<LearnedAttack>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }

        public Abyssimal(AbyssimalSpecies BaseSpecies) : this(){
            Level = 1;
            Health = BaseSpecies.BaseHealth;
            Strength = BaseSpecies.BaseStrength;
            Defence = BaseSpecies.BaseDefence;
            Magic = BaseSpecies.BaseMagic;
            Resistance = BaseSpecies.BaseResistance;
            Speed = BaseSpecies.BaseSpeed;
            MP = BaseSpecies.BaseMP;
            Experience = 0;
        }

        public Abyssimal(AbyssimalSpecies BaseSpecies, User Owner) : this(BaseSpecies){
            UserId = Owner.UserId;
        }

    }
}