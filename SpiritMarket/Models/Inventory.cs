using System.ComponentModel.DataAnnotations;
using System;
namespace SpiritMarket.Models{
    public class Inventory{
        [Key]
        public int InventoryId {get; set;}
        public int Amount {get; set;}

        public int UserId {get; set;}
        public User User {get; set;}

        public int ProductId {get; set;}
        public Product Product {get; set;}


        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Inventory(){
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
            Amount = 0;
        }
    }
}