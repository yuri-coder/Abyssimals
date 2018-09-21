using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class Shop{
        [Key]
        public int ShopId {get; set;}

        [Required(ErrorMessage="What's the name of your store?")]
        [MaxLength(32, ErrorMessage="Hey, we wanted a shop name, not your whole life story... could you shorten that to 32 characters?")]
        public string Name {get; set;}

        public int UserId {get; set;}
        public User User {get; set;}

        public List<ListedItem> Items {get; set;}


        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Shop(){
            Items = new List<ListedItem>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}