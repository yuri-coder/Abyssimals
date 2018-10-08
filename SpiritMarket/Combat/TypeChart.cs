using System;
using System.Collections.Generic;
using SpiritMarket.Models;

namespace SpiritMarket.Combat{
    public class TypeChart{

        public List<string> TypeNames {get; set;}
        public List<string> TypeShortNames {get; set;}
        public Dictionary<int, Dictionary<int, double>> Matchups {get; set;}

        public TypeChart(){
            TypeNames = new List<string>();
            TypeShortNames = new List<string>();
            Matchups = new Dictionary<int, Dictionary<int, double>>();
        }

        public TypeChart(List<string> typeNames, List<string> typeShortNames, 
                        Dictionary<int, Dictionary<int, double>> matchups){
            TypeNames = typeNames;
            TypeShortNames = typeShortNames;
            Matchups = matchups;
        }

        public double GetEffectiveness(Attack attack, List<ElementalType> defender){
            double effectiveness = 1.0;
            foreach(ElementalType defendingType in defender){
                effectiveness *= Matchups[attack.ElementalTypeId][defendingType.ElementalTypeId];
            }
            return effectiveness;
        }
    }
}
