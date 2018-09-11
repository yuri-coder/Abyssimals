using System.ComponentModel.DataAnnotations;
using System;
namespace SpiritMarket.Models{
    public class ListedProduct{
        [Key]
        public int ListedProductId {get; set;}
        public decimal Price {get; set;}
        public int Stock {get; set;}

        public int ShopId {get; set;}
        public Shop Shop {get; set;}

        public int ProductId {get; set;}
        public Product Product {get; set;}


        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public ListedProduct(){
            Stock = 0;
            Price = 0;
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}