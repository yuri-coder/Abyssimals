using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SpiritMarket.Models{
    public class Product{
        [Key]
        public int ProductId{get; set;}

        [Required(ErrorMessage = "What's the name of this new product?")]
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

        public List<ListedProduct> Shops {get; set;}
        public List<Inventory> Users {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Product(){
            Shops = new List<ListedProduct>();
            Users = new List<Inventory>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}