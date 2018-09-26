using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class Subtype{
        [Key]
        public int SubtypeId {get; set;}

        public int ItemId {get; set;}
        public Item Item {get; set;}

        public int SubItemTypeId {get; set;}
        public SubItemType SubItemType {get; set;}
        
        
    }
}