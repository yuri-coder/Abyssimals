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

namespace SpiritMarket.Areas.Admin
{
    [Route("admin/user")]
    public class AdminUserController : AdminController
    {
        public AdminUserController(SpiritContext c) : base(c) { }

        #region Admin Checking and Editing
        [HttpGet]
        [Route("new")]
        public IActionResult NewAdmin(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.Message = TempData["AdminMessage"];
            return View();
        }

        [HttpPost]
        [Route("giveadmin")]
        public IActionResult GiveAdmin(string username){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
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
        #endregion

    }
}