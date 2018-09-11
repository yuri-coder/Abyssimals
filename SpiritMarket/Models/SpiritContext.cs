using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
 
namespace SpiritMarket.Models
{
    public class SpiritContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public SpiritContext(DbContextOptions<SpiritContext> options) : base(options) { }

        public DbSet<User> Users {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<Shop> Shops {get; set;}
        public DbSet<ListedProduct> Listed_Products{get; set;}
        public DbSet<Inventory> Inventories{get; set;}

        public User GetOneUser(string Username){
            return this.Users.SingleOrDefault(user => user.Username == Username);
        }

        public User GetOneUser(int? UserId){
            if(UserId == null){
                return null;
            }
            return this.Users.SingleOrDefault(user => user.UserId == UserId);
        }

        public Shop GetOneShop(int? UserId){
            if(UserId == null){
                return null;
            }
            return this.Shops.SingleOrDefault(shop => shop.UserId == UserId);
        }

        public Product GetOneProduct(int? ProductId){
            if(ProductId == null){
                return null;
            }
            return this.Products.SingleOrDefault(product => product.ProductId == ProductId);
        }
        public Product GetOneProduct(string ProductName){
            return this.Products.SingleOrDefault(product => product.Name == ProductName);
        }
    }
}