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

namespace SpiritMarket.Areas.Admin
{
    [Route("admin/supertest")]
    public class TestController : AdminController
    {
        public TestController(SpiritContext c, IHostingEnvironment env) : base(c, env){ }

        [HttpGet("helloworld")]
        public IActionResult PleaseWork(){
            return View();
        }
    }
}