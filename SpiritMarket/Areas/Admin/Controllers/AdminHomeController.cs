using System;
using System.IO;
using FileIO = System.IO.File;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using SpiritMarket.Models;
using SpiritMarket.Combat;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SpiritMarket.Areas.Gameplay;

namespace SpiritMarket.Areas.Admin
{
    [Route("admin")]
    public class AdminHomeController : AdminController
    {
        public AdminHomeController(SpiritContext c) : base(c) { }
        
        [HttpGet]
        [Route("")]
        [Route("home")]
        public IActionResult AdminHome(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.Success = TempData["AdminMessage"];
            return View();
        }
    }
}


        