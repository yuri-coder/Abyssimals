using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class Shop{
        [Key]
        public int ShopId {get; set;}

        [Required(ErrorMessage="What's the name of your store?")]
        [MaxLength(32)]
        public string Name {get; set;}

        public int UserId {get; set;}
        public User User {get; set;}

        public List<ListedProduct> Products {get; set;}


        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Shop(){
            Products = new List<ListedProduct>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}