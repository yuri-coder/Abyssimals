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

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string ShortName {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public List<AbyssimalElementalType> AbyssimalsWithThisElementalType {get; set;}
        public List<Attack> AttacksWithThisElementalType {get; set;}
        public List<ElementalRequirement> AttacksRequiringThisElementalType {get; set;}

        //[InverseProperty("OriginalElementalType")]
        //public List<ElementalMatchup> AttackingMatchups {get; set;}

        //[InverseProperty("MatchupElementalType")]
        //public List<ElementalMatchup> DefendingMatchups {get; set;}

        public ElementalType(){
            AbyssimalsWithThisElementalType = new List<AbyssimalElementalType>();
            AttacksWithThisElementalType = new List<Attack>();
            AttacksRequiringThisElementalType = new List<ElementalRequirement>();
            //AttackingMatchups = new List<ElementalMatchup>();
            //DefendingMatchups = new List<ElementalMatchup>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}