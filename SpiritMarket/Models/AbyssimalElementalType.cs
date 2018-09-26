using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class AbyssimalElementalType{
        public int ElementalTypeId {get; set;}
        public ElementalType ElementalType {get; set;}

        public int AbyssimalSpeciesId {get; set;}
        public AbyssimalSpecies AbyssimalSpecies {get; set;}
    }
}