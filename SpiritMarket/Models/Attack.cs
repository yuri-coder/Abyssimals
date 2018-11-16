using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class Attack{
        [Key]
        public int AttackId {get; set;}

        [Required]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required]
        [MaxLength(255)]
        public string Description {get; set;}

        [Required]
        public int BasePower {get; set;}

        [Required]
        public int BaseAccuracy {get; set;}

        [Required]
        public int BaseMPCost {get; set;}

        [Required]
        public int MaxRefinementLevel {get; set;}

        [Required]
        public int DamageType {get; set;}

        public List<ElementalRequirement> ElementalRequirements {get; set;}
        public List<AbyssimalGroupRequirement> AbyssimalGroupRequirements {get; set;}
        public List<BaseAttackStatus> BaseAttackStatuses {get; set;}

        //Abyssimals that can learn this attack (via level up)
        public List<PotentialAttack> PotentialAbyssimals {get; set;}

        //Learned Attacks that use this Attack as their base
        public List<LearnedAttack> AttacksBasedOnThis {get; set;}

        public int ElementalTypeId {get; set;}
        public ElementalType ElementalType {get; set;}


        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Attack(){
            ElementalRequirements = new List<ElementalRequirement>();
            AbyssimalGroupRequirements = new List<AbyssimalGroupRequirement>();
            BaseAttackStatuses = new List<BaseAttackStatus>();
            PotentialAbyssimals = new List<PotentialAttack>();
            AttacksBasedOnThis = new List<LearnedAttack>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }

        public string GetDamageType(){
            switch(DamageType){
                case 0:
                    return "Physical";
                case 1:
                    return "Magic";
                default:
                    return "Other";
            }
        }
    }
}