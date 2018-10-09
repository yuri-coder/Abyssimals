using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SpiritMarket.Models;

namespace SpiritMarket.Combat{
    public class TypeChart{

        public Dictionary<Tuple<int, int>, double> Matchups {get; set;}

        public TypeChart(IDictionary<int, Matchup> Effectivenesses, SpiritContext context){
            Matchups = new Dictionary<Tuple<int, int>, double>();
            foreach(KeyValuePair<int, Matchup> matchup in Effectivenesses){
                Matchup MatchupInfo = context.Matchups.Include(m => m.Effectiveness).
                                        SingleOrDefault(m => m.MatchupId == matchup.Key);
                Matchups.Add(new Tuple<int, int>(MatchupInfo.AttackingElementalTypeId, MatchupInfo.DefendingElementalTypeId), 
                                                MatchupInfo.Effectiveness.Multiplier);
            }
        }

        public double GetEffectiveness(Attack attack, List<ElementalType> defender){
            double effectiveness = 1.0;
            foreach(ElementalType defendingType in defender){
                effectiveness *= Matchups[new Tuple<int, int>(attack.ElementalTypeId, defendingType.ElementalTypeId)];
            }
            return effectiveness;
        }
    }
}
