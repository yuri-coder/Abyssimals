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

namespace SpiritMarket.Controllers
{
    [Route("shops")]
    public class ShopsController : Controller
    {
        private SpiritContext context;

        public ShopsController(SpiritContext c){
            context = c;
        }

        [HttpGet]
        [Route("")]
        public IActionResult AllShops(){
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        [Route("me")]
        public IActionResult MyShop(){
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            //if i have a shop, go to edit shop, otherwise go to create a shop
            ViewBag.MyShop = context.GetOneShop(HttpContext.Session.GetInt32("UserId"));
            ViewBag.NoMoney = TempData["NoMoney"];
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateShop(Shop shop){
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            if(ModelState.IsValid){
                User CurUser = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
                if(CurUser.SubtractMoney(75)){
                    shop.UserId = CurUser.UserId;
                    context.Add(shop);
                    context.SaveChanges(); 
                }
                else{
                    TempData["NoMoney"] = "I said shops were easy to set up, not free! Come back once you have 75 SG!";
                }
                return RedirectToAction("MyShop");
            }
            else{
                return View("MyShop");
            }
        }

        [HttpGet]
        [Route("{shop_id}")]
        public IActionResult OtherShop(int shop_id){
            //if my id, redirect to shops/me
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            return View();
        } 

        

        [HttpGet]
        [Route("lose")]
        public IActionResult LoseMoney(){
            User CurUser = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(CurUser != null){
                CurUser.SubtractMoney(50);
                context.SaveChanges();  
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("add")]
        public IActionResult AddMoney(){
            User CurUser = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(CurUser != null){
                CurUser.Balance += (decimal) 25.5;
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}