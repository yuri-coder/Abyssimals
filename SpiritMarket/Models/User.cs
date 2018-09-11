using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiritMarket.Models{
    public class User{
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage = "Please provide a username")]
        [MinLength(3)]
        [MaxLength(32)]
        public string Username {get; set;}

        [Required(ErrorMessage = "We kinda need this")]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        [Required(ErrorMessage = "This wasn't optional")]
        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        [NotMapped]
        public string ConfirmPassword {get; set;}


        public decimal Balance {get; set;}

        public Shop Shop {get; set;}

        public List<Inventory> Items {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Boolean IsAdmin {get; set;}

        public User(){
            Items = new List<Inventory>();
            Balance = 100;
            IsAdmin = false;
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }

        public bool SubtractMoney(decimal amt){
            if(amt > Balance){
                return false;
            }
            else{
                Balance -= amt;
                return true;
            }
        }
    }
}