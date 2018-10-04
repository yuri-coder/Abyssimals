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
        public DbSet<Abyssimal> Abyssimals {get; set;}
        public DbSet<AbyssimalElementalType> AbyssimalElementalTypes {get; set;}
        public DbSet<AbyssimalGroup> AbyssimalGroups {get; set;}
        public DbSet<AbyssimalGroupRequirement> AbyssimalGroupRequirements {get; set;}
        public DbSet<AbyssimalSpecies> AbyssimalSpecies {get; set;}
        public DbSet<AbyssimalSpeciesGroup> AbyssimalSpeciesGroups {get; set;}
        public DbSet<AbyssimalStatus> AbyssimalStatuses {get; set;}
        public DbSet<Attack> Attacks {get; set;}
        public DbSet<BaseAttackStatus> BaseAttackStatuses {get; set;}
        public DbSet<ElementalMatchup> ElementalMatchups {get; set;}
        public DbSet<ElementalRequirement> ElementalRequirements {get; set;}
        public DbSet<ElementalType> ElementalTypes {get; set;}
        public DbSet<LearnedAttack> LearnedAttacks {get; set;}
        public DbSet<LearnedAttackStatus> LearnedAttackStatuses {get; set;}
        public DbSet<MainItemType> MainItemTypes {get; set;}
        public DbSet<PotentialAttack> PotentialAttacks {get; set;}
        public DbSet<Status> Statuses {get; set;}
        public DbSet<SubItemType> SubItemTypes {get; set;}
        public DbSet<Subtype> Subtypes {get; set;}

        /*
        Users
         */
        public User GetOneUser(string Username){
            return this.Users.SingleOrDefault(user => user.Username == Username);
        }

        public User GetOneUser(int? UserId){
            return UserId == null ? null : this.Users.SingleOrDefault(user => user.UserId == UserId);
        }


        /*
        Main Item Types
         */
        public MainItemType GetOneMainItemType(int? MainItemTypeId){
            return MainItemTypeId == null ? null : this.MainItemTypes.SingleOrDefault(type => type.MainItemTypeId == MainItemTypeId);
        }

        public MainItemType GetOneMainItemType(string Name){
            return this.MainItemTypes.SingleOrDefault(type => type.Name == Name);
        }

        public void DeleteMainItemType(int? MainItemTypeId){
            if(MainItemTypeId != null){
                this.Remove(this.MainItemTypes.SingleOrDefault(type => type.MainItemTypeId == MainItemTypeId));
            }
        }

        /*
        Sub Item Types
         */
         public SubItemType GetOneSubItemType(int? SubItemTypeId){
            return SubItemTypeId == null ? null : this.SubItemTypes.SingleOrDefault(type => type.SubItemTypeId == SubItemTypeId);
        }

        public SubItemType GetOneSubItemType(string Name){
            return this.SubItemTypes.SingleOrDefault(type => type.Name == Name);
        }

        public void DeleteSubItemType(int? SubItemTypeId){
            if(SubItemTypeId != null){
                this.Remove(this.SubItemTypes.SingleOrDefault(type => type.SubItemTypeId == SubItemTypeId));
            }
        }


        /*
        Items
         */
        public Item GetOneItem(int? ItemId){
            return ItemId == null ? null : this.Items.SingleOrDefault(Item => Item.ItemId == ItemId);
        }

        public Item GetOneItem(string ItemName){
            return this.Items.SingleOrDefault(Item => Item.Name == ItemName);
        }

        public Item GetOneItemWithTypes(string ItemName){
            return this.Items.Include(item => item.MainItemType).Include(item => item.Subtypes).
                    ThenInclude(subtype => subtype.SubItemType).SingleOrDefault(Item => Item.Name == ItemName);
        }

        public Item GetOneItemWithTypes(int? ItemId){
            return ItemId == null ? null : this.Items.Include(item => item.MainItemType).Include(item => item.Subtypes).
                    ThenInclude(subtype => subtype.SubItemType).SingleOrDefault(Item => Item.ItemId == ItemId);
        }

        public List<Item> AllItemsWithTypes(){
            return this.Items.Include(item => item.MainItemType).Include(item => item.Subtypes).
                    ThenInclude(subtype => subtype.SubItemType).ToList();
        }

        public void DeleteItem(int? ItemId){
            if(ItemId != null){
                this.Items.Remove(this.Items.Include(item => item.MainItemType).Include(item => item.Subtypes).
                    ThenInclude(subtype => subtype.SubItemType).SingleOrDefault(Item => Item.ItemId == ItemId));
            }
        }


        /*
        Listed Items
         */
        public ListedItem GetOneListedItem(int? ListedItemId){
            return ListedItemId == null ? null : this.ListedItems.SingleOrDefault(listed => listed.ListedItemId == ListedItemId);
        }


        /*
        Inventory Items
         */
        public List<InventoryItem> GetUserInventory(int? UserId){
            return UserId == null ? null : this.InventoryItems.Where(inventory => inventory.UserId == UserId).ToList();
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

        /*
        Shops
         */
        public Shop GetOneShop(int? UserId){
            return UserId == null ? null : this.Shops.SingleOrDefault(shop => shop.UserId == UserId);
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

        
    }
}