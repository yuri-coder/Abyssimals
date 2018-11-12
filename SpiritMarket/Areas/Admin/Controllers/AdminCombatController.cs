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
    [Route("admin")]
    public class AdminCombatController : AdminController
    {
        private IHostingEnvironment _env;
        public AdminCombatController(SpiritContext c, IHostingEnvironment env) : base(c) { _env = env; }

        #region Status CRUD
        [HttpGet]
        [Route("status/edit")]
        public IActionResult AllStatuses(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.AdminMessage = TempData["AdminMessage"];
            ViewBag.AllStatuses = context.Statuses.ToList();
            return View();
        }

        [HttpGet]
        [Route("status/new")]
        public IActionResult NewStatus(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return View();
        }

        [HttpPost]
        [Route("status/new")]
        public IActionResult CreateStatus(Status status){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            if(ModelState.IsValid){
                bool StillValid = true;
                Status existingName = context.GetOneStatus(status.Name);
                if(existingName != null && existingName.StatusId != status.StatusId){
                    ViewBag.NameError = "A Status with that name already exists!";
                    StillValid = false;
                }
                Status existingShortName = context.GetOneStatusByShortName(status.ShortName);
                if(existingShortName != null && existingShortName.StatusId != status.StatusId){
                    ViewBag.ShortNameError = "A Status with that short name already exists!";
                    StillValid = false;
                }
                if(StillValid){
                    status.ShortName = status.ShortName.ToUpper();
                    context.Add(status);
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"{status.Name} successfully added to the database!";
                    return RedirectToAction("AllStatuses");  
                }
            }
            return View("NewStatus");
        }

        [HttpGet]
        [Route("status/edit/{sid}")]
        public IActionResult EditStatus(int sid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.Status = context.GetOneStatus(sid);
            if(ViewBag.Status == null){
                TempData["AdminMessage"] = $"Status with the requested id {sid} not found!";
                return RedirectToAction("AllStatuses");
            }
            return View();
        }

        [HttpPost]
        [Route("status/edit/{sid}")]
        public IActionResult EditStatus(Status status, int sid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            Status original = context.GetOneStatus(sid);
            if(original == null){
                TempData["AdminMessage"] = $"Status with the requested id {sid} not found!";
                return RedirectToAction("AllStatuses");
            }

            ViewBag.Status = original;
            if(ModelState.IsValid){
                bool StillValid = true;
                Status existingName = context.GetOneStatus(status.Name);
                if(existingName != null && existingName.StatusId != sid){
                    ViewBag.NameError = "A Status with that name already exists!";
                    StillValid = false;
                }
                Status existingShortName = context.GetOneStatusByShortName(status.ShortName);
                if(existingShortName != null && existingShortName.StatusId != sid){
                    ViewBag.ShortNameError = "A Status with that short name already exists!";
                    StillValid = false;
                }
                if(StillValid){
                    original.Name = status.Name;
                    original.Description = status.Description;
                    original.ShortName = status.ShortName.ToUpper();
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"Status #{sid} successfully edited!";
                    return RedirectToAction("AllStatuses");
                }
            }
            return View("EditStatus");
        }

        [HttpGet]
        [Route("status/delete/{sid}")]
        public IActionResult DeleteStatus(int sid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            context.DeleteStatus(sid);
            context.SaveChanges();
            TempData["AdminMessage"] = $"Status #{sid} successfully deleted! I hope you knew what you were doing!";
            return RedirectToAction("AllStatuses");

        }
        #endregion

        #region Elemental Type CRUD
        [HttpGet]
        [Route("elementaltypes/edit")]
        public IActionResult AllElementalTypes(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.AdminMessage = TempData["AdminMessage"];
            ViewBag.AllElementalTypes = context.ElementalTypes.ToList();
            return View();
        }

        [HttpGet]
        [Route("elementaltypes/new")]
        public IActionResult NewElementalType(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return View();
        }

        [HttpPost]
        [Route("elementaltypes/new")]
        public IActionResult CreateElementalType(ElementalType element){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            if(ModelState.IsValid){
                bool StillValid = true;
                ElementalType existingName = context.GetOneElementalType(element.Name);
                if(existingName != null && existingName.ElementalTypeId != element.ElementalTypeId){
                    ViewBag.NameError = "An Elemental Type with that name already exists!";
                    StillValid = false;
                }
                ElementalType existingShortName = context.GetOneElementalTypeByShortName(element.ShortName);
                if(existingShortName != null && existingShortName.ElementalTypeId != element.ElementalTypeId){
                    ViewBag.ShortNameError = "An Elemental Type with that short name already exists!";
                    StillValid = false;
                }
                if(StillValid){
                    element.ShortName = element.ShortName.ToUpper();
                    context.Add(element);
                    UpdateMatchups(); //not sure why this doesn't execute - async issue?
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"{element.Name} successfully added to the database!";
                    return RedirectToAction("AllElementalTypes");  
                }
            }
            return View("NewElementalType");
        }

        [HttpGet]
        [Route("elementaltypes/edit/{eid}")]
        public IActionResult EditElementalType(int eid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.ElementalType = context.GetOneElementalType(eid);
            if(ViewBag.ElementalType == null){
                TempData["AdminMessage"] = $"Elemental Item Type with the requested id {eid} not found!";
                return RedirectToAction("AllElementalTypes");
            }
            return View();
        }

        [HttpPost]
        [Route("elementaltypes/edit/{eid}")]
        public IActionResult EditElementalType(ElementalType element, int eid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ElementalType original = context.GetOneElementalType(eid);
            if(original == null){
                TempData["AdminMessage"] = $"Elemental Type with the requested id {eid} not found!";
                return RedirectToAction("AllElementalTypes");
            }

            ViewBag.ElementalType = original;
            if(ModelState.IsValid){
                bool StillValid = true;
                ElementalType existingName = context.GetOneElementalType(element.Name);
                if(existingName != null && existingName.ElementalTypeId != eid){
                    ViewBag.NameError = "An Elemental Type with that name already exists!";
                    StillValid = false;
                }
                ElementalType existingShortName = context.GetOneElementalTypeByShortName(element.ShortName);
                if(existingShortName != null && existingShortName.ElementalTypeId != eid){
                    ViewBag.ShortNameError = "An Elemental Type with that short name already exists!";
                    StillValid = false;
                }
                if(StillValid){
                    original.Name = element.Name;
                    original.Description = element.Description;
                    original.ShortName = element.ShortName.ToUpper();
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"Elemental Type #{eid} successfully edited!";
                    return RedirectToAction("AllElementalTypes");
                }
            }
            return View("EditElementalType");
        }

        [HttpGet]
        [Route("elementaltypes/delete/{eid}")]
        public IActionResult DeleteElementalType(int eid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            context.DeleteElementalType(eid);
            context.SaveChanges();
            TempData["AdminMessage"] = $"Elemental Type #{eid} successfully deleted! I hope you knew what you were doing!";
            return RedirectToAction("AllElementalTypes");
        }
        #endregion

        #region Attack CRUD
        #endregion

        #region Type Matchups Editing 
        [HttpGet]
        [Route("matchups/edit")]
        public IActionResult EditTypeChart(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            List<ElementalType> AllElementalTypes = context.AllElementalTypesAndMatchups();
            List<Matchup> AllMatchups = context.AllMatchups();
            Dictionary<Tuple<int, int>, Matchup> Effectivenesses = new Dictionary<Tuple<int, int>, Matchup>();
            foreach(Matchup matchup in AllMatchups){
                Effectivenesses.Add(new Tuple<int, int>(matchup.AttackingElementalTypeId, matchup.DefendingElementalTypeId), matchup);
            }
            ViewBag.AllElementalTypes = AllElementalTypes;
            ViewBag.Matchups = Effectivenesses;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        [HttpPost]
        [Route("matchups/edit")]
        public IActionResult EditTypeChart(IDictionary<int, Matchup> Matchups){
            ViewBag.User = context.GetOneUser(HttpContext.Session.GetInt32("UserId"));
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            foreach(KeyValuePair<int, Matchup> matchup in Matchups){
                Matchup ExistingMatchup = context.GetOneMatchup(matchup.Key);
                if(ExistingMatchup == null){
                    TempData["ErrorMessage"] = "One of the types couldn't be found anymore - perhaps it was deleted?";
                    return RedirectToAction("EditTypeChart");
                }
                ExistingMatchup.EffectivenessId = matchup.Value.EffectivenessId;
            }
            context.SaveChanges();
            TempData["SuccessMessage"] = "Type chart successfully updated!";
            TypeChart NewTypeChart = new TypeChart(Matchups, context);
            string filelocation = Path.Combine(_env.WebRootPath + "\\jsondata\\typechart.json");
            FileIO.WriteAllText(filelocation, JsonConvert.SerializeObject(NewTypeChart));
            return RedirectToAction("EditTypeChart");
        }
        #endregion


        #region Special Development Routes & Methods
        [HttpGet]
        [Route("typechart/update")]
        public IActionResult UpdateTypeChart(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            UpdateMatchups();
            return RedirectToAction("AdminHome");
        }

        public void UpdateMatchups(){
            List<ElementalType> AllElementalTypes = context.ElementalTypes.ToList();
            foreach(ElementalType AttackingType in AllElementalTypes){
                if(AttackingType.Matchups == null){
                    AttackingType.Matchups = new List<Matchup>();
                }
                foreach(ElementalType DefendingType in AllElementalTypes){
                    Matchup matchup = context.GetOneMatchup(AttackingType, DefendingType);
                    if(matchup == null){
                        matchup = new Matchup();
                        matchup.EffectivenessId = 1;
                        matchup.AttackingElementalTypeId = AttackingType.ElementalTypeId;
                        matchup.DefendingElementalTypeId = DefendingType.ElementalTypeId;
                        context.Add(matchup);
                    }
                }
            }
            context.SaveChanges();
        }

        #endregion
    
    }
}