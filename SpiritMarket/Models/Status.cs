using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class Status{
        [Key]
        public int StatusId {get; set;}

        [Required]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required]
        [MaxLength(255)]
        public string Description {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public List<BaseAttackStatus> BaseAttacksWithThisStatus {get; set;}
        public List<LearnedAttackStatus> LearnedAttacksWithThisStatus {get; set;}
        public List<AbyssimalStatus> AbyssimalsWithThisStatus {get; set;}

        public Status(){
            BaseAttacksWithThisStatus = new List<BaseAttackStatus>();
            LearnedAttacksWithThisStatus = new List<LearnedAttackStatus>();
            AbyssimalsWithThisStatus = new List<AbyssimalStatus>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    
    }
}