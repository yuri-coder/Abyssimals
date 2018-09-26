using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class ElementalRequirement{
        [Key]
        public int ElementalRequirementId {get; set;}

        public int ElementalTypeId {get; set;}
        public ElementalType ElementalType {get; set;}

        public int AttackId {get; set;}
        public Attack Attack {get; set;}
    }
}