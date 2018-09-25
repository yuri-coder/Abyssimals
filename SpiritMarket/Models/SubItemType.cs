using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class SubItemType{
        [Key]
        public int SubItemTypeId {get; set;}

        [Required(ErrorMessage = "What is the name of this new Sub Item Type?")]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required(ErrorMessage = "What is the description of this new Sub Item Type?")]
        [MaxLength(255)]
        public string Description {get; set;}

        //List of items with this subtype
        public List<Subtype> Items {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public SubItemType(){
            Items = new List<Subtype>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}