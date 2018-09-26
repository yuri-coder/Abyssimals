using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class LearnedAttackStatus{
        [Key]
        public int LearnedAttackStatusId {get; set;}

        public int Chance {get; set;}
        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public int LearnedAttackId {get; set;}
        public LearnedAttack LearnedAttack {get; set;}

        public int StatusId {get; set;}
        public Status Status {get; set;}

        public LearnedAttackStatus(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}