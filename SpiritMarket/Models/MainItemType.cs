using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class MainItemType{
        [Key]
        public int MainItemTypeId {get; set;}

        [Required(ErrorMessage = "What is the name of this new Main Item Type?")]
        [MaxLength(45)]
        public string Name {get; set;}

        [Required(ErrorMessage = "What is the description of this new Main Item Type?")]
        [MaxLength(255)]
        public string Description {get; set;}

        public List<Item> Items {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public MainItemType(){
            Items = new List<Item>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}