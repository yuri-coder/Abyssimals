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
    [Route("admin")]
    public class AdminAbyssimalController : AdminController
    {
        public AdminAbyssimalController(SpiritContext c) : base(c) { }

        #region Abyssimal Group CRUD
        [HttpGet]
        [Route("abyssimalgroup/edit")]
        public IActionResult AllAbyssimalGroups(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.Success = TempData["SuccessMessage"];
            ViewBag.Error = TempData["ErrorMessage"];
            ViewBag.AllAbyssimalGroups = context.GetAllAbyssimalGroupsFull();
            return View();
        }

        [HttpGet]
        [Route("abyssimalgroup/new")]
        public IActionResult NewAbyssimalGroup(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return View();
        }

        [HttpPost]
        [Route("abyssimalgroup/new")]
        public IActionResult CreateAbyssimalGroup(AbyssimalGroup NewGroup){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            if(ModelState.IsValid){
                AbyssimalGroup existing = context.GetOneAbyssimalGroup(NewGroup.Name);
                if(existing != null && existing.AbyssimalGroupId != NewGroup.AbyssimalGroupId){
                    ViewBag.NameError = "An Abyssimal Group with that name already exists!";
                    return View("NewAbyssimalGroup");
                }
                context.Add(NewGroup);
                context.SaveChanges();
                TempData["SuccessMessage"] = $"{NewGroup.Name} successfully added to the database!";
                return RedirectToAction("AllAbyssimalGroups");
            }
            return View("NewAbyssimalGroup");
        }
        
        [HttpGet]
        [Route("abyssimalgroup/edit/{gid}")]
        public IActionResult EditAbyssimalGroup(int gid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return View();
        }

        [HttpPost]
        [Route("abyssimalgroup/edit/{gid}")]
        public IActionResult EditAbyssimalGroup(AbyssimalGroup group, int gid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return RedirectToAction("AllAbyssimalGroups");
        }

        [HttpGet]
        [Route("abyssimalgroup/delete/{gid}")]
        public IActionResult DeleteAbyssimalGroup(int gid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return RedirectToAction("AllAbyssimalGroups");
        }
        #endregion

        #region Abyssimal Species CRUD
        #endregion
        
        #region Admin Related Abyssimal CRUD
        #endregion


    }
}