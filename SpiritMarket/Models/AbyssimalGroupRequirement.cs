using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class AbyssimalGroupRequirement{
        public int AttackId {get; set;}
        public Attack Attack {get; set;}

        public int AbyssimalGroupId {get; set;}
        public AbyssimalGroup AbyssimalGroup {get; set;}
    }
}