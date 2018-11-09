using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpiritMarket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SpiritMarket.Areas.Gameplay
{
    [Area("Gameplay")]
    [Route("inventory")]
    public class InventoryController : Controller
    {
        private SpiritContext context;

        public InventoryController(SpiritContext c){
            context = c;
        }

        [HttpGet]
        [Route("")]
        public IActionResult DisplayInventory(){
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.Inventory = context.Users.Include(user => user.Items).ThenInclude(i => i.Item).
                                SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId")).Items;
            ViewBag.Error = TempData["Error"];
            ViewBag.Success = TempData["Success"];
            return View();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddItemToShop(int InventoryItemId, int Amount, long Price){
            InventoryItem Item = context.InventoryItems.Include(inventory => inventory.Item).SingleOrDefault(inventory => inventory.InventoryItemId == InventoryItemId);
            if(Item == null){
                TempData["Error"] = "It appears as though that item isn't in your inventory! Did you already do something with it?";
                return RedirectToAction("DisplayInventory");
            }
            Shop UserShop = context.Shops.Include(shop => shop.Items).
                            SingleOrDefault(Shop=> Shop.UserId == HttpContext.Session.GetInt32("UserId"));
            if(UserShop == null){
                TempData["Error"] = "Hey! It looks like you don't have a shop! Why don't you go make one now?";
                return RedirectToAction("DisplayInventory");
            }
            if(context.AddToShop(Item, UserShop, Amount, Price)){
                TempData["Success"] = Item.Item.Name + " x" + Amount + " was added to your shop!";
            }
            else{
                TempData["Error"] = "Something went wrong when trying to add " + Item.Item.Name + " to your shop.";
            }
            return RedirectToAction("DisplayInventory");
        }

        [HttpGet]
        [Route("get/starterkit")]
        public IActionResult GetStarterKit(){
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            User current = ViewBag.User;
            InventoryItem TestLeaf = new InventoryItem();
            TestLeaf.Amount = 3;
            TestLeaf.ItemId = 14;
            TestLeaf.UserId = current.UserId;

            InventoryItem TestEssence = new InventoryItem();
            TestEssence.Amount = 2;
            TestEssence.ItemId = 15;
            TestEssence.UserId = current.UserId;

            InventoryItem TestPetal = new InventoryItem();
            TestPetal.Amount = 5;
            TestPetal.ItemId = 16;
            TestPetal.UserId = current.UserId;
            context.AddToInventory(TestLeaf);
            context.AddToInventory(TestEssence);
            context.AddToInventory(TestPetal);
            return RedirectToAction("DisplayInventory");
        }
    }
}