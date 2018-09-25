using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class Item{
        [Key]
        public int ItemId{get; set;}

        [Required(ErrorMessage = "What's the name of this new Item?")]
        [MinLength(2)]
        [MaxLength(64)]
        public string Name{get; set;}

        [Required(ErrorMessage = "What exactly is this thing?")]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description{get; set;}

        [Required(ErrorMessage = "What does it look like?")]
        public string Image {get; set;}

        [Display(Name="Tradeable")]
        public bool? IsTradeable {get; set;}

        public List<ListedItem> Shops {get; set;}
        public List<InventoryItem> Users {get; set;}
        public List<Subtype> Subtypes {get; set;}

        public int MainItemTypeId {get; set;}
        public MainItemType MainItemType {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Item(){
            Shops = new List<ListedItem>();
            Users = new List<InventoryItem>();
            Subtypes = new List<Subtype>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}