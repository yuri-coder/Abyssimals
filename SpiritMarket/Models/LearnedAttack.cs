using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class LearnedAttack{
        [Key]
        public int LearnedAttackId{get; set;}

        public int Power {get; set;}
        public int Accuracy {get; set;}
        public int MPCost {get; set;}
        public int RefinementLevel {get; set;}
        public bool IsEquipped {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public int AbyssimalId {get; set;}
        public Abyssimal Abyssimal {get; set;}

        public int AttackId {get; set;}
        public Attack Attack {get; set;}

        public List<LearnedAttackStatus> Statuses {get; set;}

        public LearnedAttack(){
            Statuses = new List<LearnedAttackStatus>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }

        public void InitializeAttack(Attack BaseAttack){
            Power = BaseAttack.BasePower;
            Accuracy = BaseAttack.BaseAccuracy;
            MPCost = BaseAttack.BaseMPCost;
            RefinementLevel = 0;
            IsEquipped = false;
            AttackId = BaseAttack.AttackId;
        }
    }
}