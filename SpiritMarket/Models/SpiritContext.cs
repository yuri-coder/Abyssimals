using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
 
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

        public ListedProduct GetOneListedProduct(int? ListedProductId){
            if(ListedProductId == null){
                return null;
            }
            return this.Listed_Products.SingleOrDefault(listed => listed.ListedProductId == ListedProductId);
        }

        public List<Inventory> GetUserInventory(int? UserId){
            if(UserId == null){
                return null;
            }
            return this.Inventories.Where(inventory => inventory.UserId == UserId).ToList();
        }

        public void AddToInventory(Inventory Item){
            if(Item.Amount <= 0){
                return;
            }
            int UserId = Item.UserId;
            List<Inventory> UserInventory = this.Users.Include(user => user.Items).
                        SingleOrDefault(user => user.UserId == UserId).Items;
            bool ItemExists = false;
            Inventory ExistingItem = null;
            foreach(Inventory HeldItem in UserInventory){
                if(Item.ProductId == HeldItem.ProductId){
                    ItemExists = true;
                    ExistingItem = HeldItem;
                    break;
                }
            }
            if(ItemExists){
                ExistingItem.Amount += Item.Amount;
            }
            else{
                this.Add(Item);
            }
            this.SaveChanges();
        }

        public bool AddToShop(Inventory Item, Shop UserShop, int Stock, long Price){
            if(Item.Amount < Stock){
                return false;
            }
            ListedProduct NewProduct = new ListedProduct();
            if(UserShop == null){
                return false;
            }
            List<ListedProduct> ShopStock = UserShop.Products;
            bool AlreadyOnSale = false;
            ListedProduct OnSaleProduct = null;
            foreach(ListedProduct OnSale in ShopStock){
                if(OnSale.ProductId == Item.ProductId){
                    AlreadyOnSale = true;
                    OnSaleProduct = OnSale;
                    break;
                }
            }
            if(AlreadyOnSale){
                OnSaleProduct.Stock += Stock;
                if(Price != 0){
                    OnSaleProduct.Price = Price;
                }
            }
            else{
                NewProduct.ProductId = Item.ProductId;
                NewProduct.Price = Price;
                NewProduct.ShopId = UserShop.ShopId;
                NewProduct.Stock = Stock;
                this.Add(NewProduct);
            }
            RemoveItemFromInventory(Item, Stock);
            this.SaveChanges();

            return true;
        }

        public bool PurchaseItem(User Buyer, ListedProduct Item, int Amount){
            if(Buyer == null || Item == null || Amount <= 0 || Amount > Item.Stock){
                return false;
            } 
            User ShopOwner = this.Users.Include(user => user.Shop).SingleOrDefault(user => user.Shop.ShopId == Item.ShopId);
            if(ShopOwner == null){
                return false;
            }
            long TotalPrice = Item.Price * Amount;
            if(TotalPrice > Buyer.Balance){
                return false;
            }

            Buyer.SubtractMoney(TotalPrice);
            ShopOwner.Balance += TotalPrice;

            Item.Stock -= Amount;
            if(Item.Stock == 0){
                this.Remove(Item);
            }

            Inventory BoughtItem = new Inventory();
            BoughtItem.UserId = Buyer.UserId;
            BoughtItem.ProductId = Item.ProductId;
            BoughtItem.Amount = Amount;
            AddToInventory(BoughtItem);
            
            return true;
        }

        public bool RemoveItemFromInventory(Inventory Item, int Amount){
            if(Item.Amount < Amount)
                return false;
            Item.Amount -= Amount;
            if(Item.Amount == 0){
                this.Remove(Item);
                this.SaveChanges();
            }
            return true;
        }
    }
}