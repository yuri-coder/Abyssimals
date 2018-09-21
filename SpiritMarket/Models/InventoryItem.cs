using System.ComponentModel.DataAnnotations;
using System;
namespace SpiritMarket.Models{
    public class InventoryItem{
        [Key]
        public int InventoryItemId {get; set;}
        public int Amount {get; set;}

        public int UserId {get; set;}
        public User User {get; set;}

        public int ItemId {get; set;}
        public Item Item {get; set;}


        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public InventoryItem(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
            Amount = 0;
        }
    }
}