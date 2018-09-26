using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class BaseAttackStatus{
        public int BaseChance {get; set;}
        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public int AttackId {get; set;}
        public Attack Attack {get; set;}
        
        public int StatusId {get; set;}
        public Status Status {get; set;}

        public BaseAttackStatus(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}