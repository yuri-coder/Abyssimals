using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using SpiritMarket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace SpiritMarket.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private SpiritContext context;
        private IHostingEnvironment _env;

        public AdminController(SpiritContext c, IHostingEnvironment env){
            context = c;
            _env = env;
        }

        [HttpGet]
        [Route("")]
        public IActionResult AdminHome(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AdminMessage = TempData["AdminMessage"];
            return View("Home");
        }

        [HttpGet]
        [Route("new")]
        public IActionResult NewAdmin(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = TempData["AdminMessage"];
            return View();
        }

        [HttpGet]
        [Route("item/new")]
        public IActionResult NewItem(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [Route("item/new")]
        public IActionResult CreateItem(Product p){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }

            if(ModelState.IsValid){
                if(context.GetOneProduct(p.Name) != null){
                    ViewBag.NameError = "An item with that name already exists!";
                    return View("NewItem");
                }
                var webRoot = _env.WebRootPath;
                var file = System.IO.Path.Combine(webRoot + "\\images", p.Image);
                Console.WriteLine("File path: " + file);
                if(System.IO.File.Exists(file)){
                    //Console.WriteLine("File exists!");
                    p.IsTradeable = p.IsTradeable ?? false;
                    //p.Image = file;
                    context.Add(p);
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"{p.Name} successfully added to the database!";
                    return RedirectToAction("AdminHome");
                }
                else{
                    Console.WriteLine("File didn't exist");
                    ViewBag.ImageError = "The file couldn't be found. Please check the spelling and file extension!";
                    return View("NewItem");
                }
            }
            return View("NewItem");
        }

        [HttpGet]
        [Route("item/edit")]
        public IActionResult ItemList(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AdminMessage = TempData["AdminMessage"];
            ViewBag.AllProducts = context.Products.ToList();
            return View();
        }

        [HttpGet]
        [Route("item/edit/{pid}")]
        public IActionResult EditItem(int pid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Product = context.GetOneProduct(pid);
            if(ViewBag.Product == null){
                TempData["AdminMessage"] = $"Item with the requested id {pid} not found!";
                return RedirectToAction("AdminHome");
            }
            return View();
        }

        [HttpPost]
        [Route("item/edit/{pid}")]
        public IActionResult EditItem(Product p, int pid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            Product original = context.GetOneProduct(pid);
            if(original == null){
                TempData["AdminMessage"] = $"Item with the requested id {pid} not found!";
                return RedirectToAction("AdminHome");
            }

            ViewBag.Product = original;
            if(ModelState.IsValid){
                Product existing = context.GetOneProduct(p.Name);
                if(existing != null && existing.ProductId != pid){
                    ViewBag.NameError = "An item with that name already exists!";
                    return View("EditItem");
                }
                var webRoot = _env.WebRootPath;
                var file = System.IO.Path.Combine(webRoot + "\\images", p.Image);
                Console.WriteLine("File path: " + file);
                if(System.IO.File.Exists(file)){
                    Console.WriteLine("File exists!");
                    p.IsTradeable = p.IsTradeable ?? false;
                    Console.WriteLine("P Tradeable is " + p.IsTradeable);
                    //p.Image = file;
                    original.Name = p.Name;
                    original.Description = p.Description;
                    original.Image = p.Image;
                    original.IsTradeable = p.IsTradeable;
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"Item #{pid} successfully edited!";
                    return RedirectToAction("ItemList");
                }
                else{
                    Console.WriteLine("File didn't exist");
                    ViewBag.ImageError = "The file couldn't be found. Please check the spelling and file extension!";
                    return View("EditItem");
                }
            }
            Console.WriteLine("IsTradeable is " + p.IsTradeable);
            return View("EditItem");
        }

        [HttpPost]
        [Route("giveadmin")]
        public IActionResult GiveAdmin(string username){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home");
            }
            User user = context.GetOneUser(username);
            if(user != null){
                if(user.IsAdmin){
                    TempData["AdminMessage"] = $"{username} is already an admin!";
                }
                else{
                    user.IsAdmin = true;
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"{username} is now an admin!";
                }
            }
            else{
                TempData["AdminMessage"] = $"No user with the username \"{username}\" found!";
            }
            return RedirectToAction("NewAdmin");
        }

        public User HasAccess(){
            if(HttpContext.Session.GetInt32("UserId") == null){
                return null;
            }
            User u = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(u == null || !u.IsAdmin){
                return null;
            }
            return u;
        }
    }
}
