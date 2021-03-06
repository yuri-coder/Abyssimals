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
    [Area("Gameplay")]
    public class GameplayController : Controller
    {
        protected SpiritContext context;

        public GameplayController(SpiritContext c){
            context = c;
        }
    }
}