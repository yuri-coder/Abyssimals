﻿using System;
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
    public class HomeController : Controller
    {
        private SpiritContext context;

        public HomeController(SpiritContext c){
            context = c;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserId") != null){
                return RedirectToAction("Home");
            }
            ViewBag.UsernameError = TempData["UsernameError"];
            ViewBag.PasswordError = TempData["PasswordError"];
            ViewBag.TakenUsername = TempData["TakenUsername"];
            return View();
        }

        [HttpPost]
        [Route("user/login")]
        public IActionResult Login(string Username, string Password){
            Console.WriteLine("Username is " + Username);
            Console.WriteLine($"Password is {Password}");
            bool valid = true;

            if(Username == null || Username.Length == 0){
                TempData["UsernameError"] = "Username is required!";
                valid = false;
            }
            if(Password == null || Password.Length == 0){
                TempData["PasswordError"] = "Password is required!";
                valid = false;
            }
            if(valid){
                User CheckUser = context.GetOneUser(Username);
                if(CheckUser != null){
                    var Hasher = new PasswordHasher<User>();
                    var result = Hasher.VerifyHashedPassword(CheckUser, CheckUser.Password, Password);
                    if(result != 0)
                    {
                        Console.WriteLine("Successfully logged in");
                        HttpContext.Session.SetInt32("UserId", CheckUser.UserId);
                        return RedirectToAction("Home");
                    }
                    else{
                        Console.WriteLine("Incorrect password");
                    }
                } 
                TempData["UsernameError"] = "Either this was wrong...";
                TempData["PasswordError"] = "... or this was wrong!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("user/register")]
        public IActionResult Register(User user){
            if(ModelState.IsValid){
                Console.WriteLine("Model was valid!");
                if(context.GetOneUser(user.Username) != null){
                    TempData["TakenUsername"] = "Another Spirit already has that username!";
                    return RedirectToAction("Index");
                }
                if(context.Users.ToList().Count == 0){
                    user.IsAdmin = true;
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                context.Add(user);
                context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Home");
            }
            else{
                Console.WriteLine("Model was invalid!");
                return View("Index");
            }
        }

        [HttpGet]
        [Route("user/logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("home")]
        public IActionResult Home(){
            //links to view/create my shop, list other people's shops
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User.IsAdmin)
                return RedirectToAction("AdminHome", "Admin");
            return View();
        }

        [Route("{*url}", Order = 999)]
        public IActionResult CatchAll()
        {
            Response.StatusCode = 404;
            return RedirectToAction("Home");
        }

        
        
    }
}