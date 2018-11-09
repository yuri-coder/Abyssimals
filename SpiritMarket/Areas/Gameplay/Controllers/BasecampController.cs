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
using Microsoft.AspNetCore.Hosting;

namespace SpiritMarket.Areas.Gameplay
{
    [Route("basecamp")]
    public class BasecampController : GameplayController
    {
        public BasecampController(SpiritContext c, IHostingEnvironment env) : base(c, env) { }

        [HttpGet]
        [Route("")]
        [Route("home")]
        public IActionResult Home(){
            //links to view/create my shop, list other people's shops
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            // ViewBag.WebRoot = _env.WebRootPath;
            return View();
        }
    }
}