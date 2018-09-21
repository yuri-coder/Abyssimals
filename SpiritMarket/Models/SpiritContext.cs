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
        public DbSet<Item> Items {get; set;}
        public DbSet<Shop> Shops {get; set;}
        public DbSet<ListedItem> ListedItems{get; set;}
        public DbSet<InventoryItem> InventoryItems{get; set;}

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

        public Item GetOneItem(int? ItemId){
            if(ItemId == null){
                return null;
            }
            return this.Items.SingleOrDefault(Item => Item.ItemId == ItemId);
        }
        public Item GetOneItem(string ItemName){
            return this.Items.SingleOrDefault(Item => Item.Name == ItemName);
        }

        public ListedItem GetOneListedItem(int? ListedItemId){
            if(ListedItemId == null){
                return null;
            }
            return this.ListedItems.SingleOrDefault(listed => listed.ListedItemId == ListedItemId);
        }

        public List<InventoryItem> GetUserInventory(int? UserId){
            if(UserId == null){
                return null;
            }
            return this.InventoryItems.Where(inventory => inventory.UserId == UserId).ToList();
        }

        public void AddToInventory(InventoryItem Item){
            if(Item.Amount <= 0){
                return;
            }
            int UserId = Item.UserId;
            List<InventoryItem> UserInventory = this.Users.Include(user => user.Items).
                        SingleOrDefault(user => user.UserId == UserId).Items;
            bool ItemExists = false;
            InventoryItem ExistingItem = null;
            foreach(InventoryItem HeldItem in UserInventory){
                if(Item.ItemId == HeldItem.ItemId){
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

        public bool AddToShop(InventoryItem Item, Shop UserShop, int Stock, long Price){
            if(Item.Amount < Stock){
                return false;
            }
            ListedItem NewItem = new ListedItem();
            if(UserShop == null){
                return false;
            }
            List<ListedItem> ShopStock = UserShop.Items;
            bool AlreadyOnSale = false;
            ListedItem OnSaleItem = null;
            foreach(ListedItem OnSale in ShopStock){
                if(OnSale.ItemId == Item.ItemId){
                    AlreadyOnSale = true;
                    OnSaleItem = OnSale;
                    break;
                }
            }
            if(AlreadyOnSale){
                OnSaleItem.Stock += Stock;
                if(Price != 0){
                    OnSaleItem.Price = Price;
                }
            }
            else{
                NewItem.ItemId = Item.ItemId;
                NewItem.Price = Price;
                NewItem.ShopId = UserShop.ShopId;
                NewItem.Stock = Stock;
                this.Add(NewItem);
            }
            RemoveItemFromInventory(Item, Stock);
            this.SaveChanges();

            return true;
        }

        public bool PurchaseItem(User Buyer, ListedItem Item, int Amount){
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

            InventoryItem BoughtItem = new InventoryItem();
            BoughtItem.UserId = Buyer.UserId;
            BoughtItem.ItemId = Item.ItemId;
            BoughtItem.Amount = Amount;
            AddToInventory(BoughtItem);
            
            return true;
        }

        public bool RemoveItemFromInventory(InventoryItem Item, int Amount){
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