using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiritMarket.Models{
    public class User{
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage = "Please provide a username!")]
        [MinLength(3, ErrorMessage="Make sure your name is at least 3 characters long.")]
        [MaxLength(32, ErrorMessage="Whoah there, think you can tone that down to 32 characters or less?")]
        [RegularExpression("^[\\w]+$", ErrorMessage="Only alphanumeric and underscores, please!")]
        public string Username {get; set;}

        [Required(ErrorMessage = "We kinda need this...")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Make sure your password is at least 8 characters long (for security!)")]
        [RegularExpression("^[\\w!#\\^\\$\\.\\?\\*\\+-]+$", ErrorMessage="Only alphanumeric as well as special characters !#$^.?*+-_ allowed.")]
        public string Password {get; set;}

        [Required(ErrorMessage = "This wasn't optional!")]
        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        [NotMapped]
        public string ConfirmPassword {get; set;}


        public long Balance {get; set;}

        public Shop Shop {get; set;}

        public List<InventoryItem> Items {get; set;}

        public DateTime Created_At{get; set;}
        public DateTime Updated_At{get; set;}

        public Boolean IsAdmin {get; set;}

        public User(){
            Items = new List<InventoryItem>();
            Balance = 100;
            IsAdmin = false;
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }

        public bool SubtractMoney(long amt){
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