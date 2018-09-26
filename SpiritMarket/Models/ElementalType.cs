using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiritMarket.Models{
    public class ElementalType{
        [Key]
        public int ElementalTypeId {get; set;}

        [Required]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required]
        [MaxLength(255)]
        public string Description {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public List<AbyssimalElementalType> AbyssimalsWithThisElementalType {get; set;}
        public List<Attack> AttacksWithThisElementalType {get; set;}
        public List<ElementalRequirement> AttacksRequiringThisElementalType {get; set;}

        [InverseProperty("OriginalType")]
        public List<ElementalMatchup> AttackingMatchups {get; set;}

        [InverseProperty("MatchupType")]
        public List<ElementalMatchup> DefendingMatchups {get; set;}

        public ElementalType(){
            AbyssimalsWithThisElementalType = new List<AbyssimalElementalType>();
            AttacksWithThisElementalType = new List<Attack>();
            AttacksRequiringThisElementalType = new List<ElementalRequirement>();
            AttackingMatchups = new List<ElementalMatchup>();
            DefendingMatchups = new List<ElementalMatchup>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}