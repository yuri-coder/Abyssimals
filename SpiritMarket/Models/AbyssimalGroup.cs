using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class AbyssimalGroup{
        [Key]
        public int AbyssimalGroupId {get; set;}

        [Required]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required]
        [MaxLength(255)]
        public string Description {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public List<AbyssimalSpeciesGroup> SpeciesWithThisGroup {get; set;}
        public List<AbyssimalGroupRequirement> AttacksRequiringThisGroup {get; set;}

        public AbyssimalGroup(){
            SpeciesWithThisGroup = new List<AbyssimalSpeciesGroup>();
            AttacksRequiringThisGroup = new List<AbyssimalGroupRequirement>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}