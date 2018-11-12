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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace SpiritMarket.Areas.Admin
{
    [Route("admin")]
    public class AdminItemController : AdminController
    {
        private IHostingEnvironment _env;
        public AdminItemController(SpiritContext c, IHostingEnvironment env) : base(c) { _env = env; }

        #region Item CRUD
        [HttpGet]
        [Route("item/new")]
        public IActionResult NewItem(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.MainItemTypes = context.MainItemTypes.ToList();
            ViewBag.SubItemTypes = context.SubItemTypes.ToList();
            return View();
        }

        [HttpPost]
        [Route("item/new")]
        public IActionResult CreateItem(Item p){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            if(ModelState.IsValid){
                bool StillValid = true;
                string nameError;
                StillValid = ValidateUniqueItemName(p.Name, out nameError) ? StillValid : false;
                ViewBag.NameError = nameError;

                string imageError;
                StillValid = ValidateImageLocation(p.Image, out imageError) ? StillValid : false;
                ViewBag.ImageError = imageError;

                string mainItemTypeError;
                StillValid = ValidateMainItemType(HttpContext.Request.Form["MainItemType"], out mainItemTypeError) ? StillValid : false;
                ViewBag.MainItemTypeError = mainItemTypeError;

                List<int> SubItemTypeIds = new List<int>();
                string subItemTypeError;
                StillValid = ValidateSubItemTypes(HttpContext.Request.Form["SubItemType"].ToList(), SubItemTypeIds, out subItemTypeError) ? StillValid : false;
                ViewBag.SubItemTypeError = subItemTypeError;
                if(StillValid){
                    p.IsTradeable = p.IsTradeable ?? false;
                    p.MainItemTypeId = Int32.Parse(HttpContext.Request.Form["MainItemType"]);
                    context.Add(p);
                    foreach(int subId in SubItemTypeIds){
                        Subtype s = new Subtype();
                        s.SubItemTypeId = subId;
                        s.ItemId = p.ItemId;
                        context.Add(s);
                    }
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"{p.Name} successfully added to the database!";
                    return RedirectToAction("ItemList");
                }
            }
            ViewBag.MainItemTypes = context.MainItemTypes.ToList();
            ViewBag.SubItemTypes = context.SubItemTypes.ToList();
            return View("NewItem");
        }

        [HttpGet]
        [Route("item/edit")]
        public IActionResult ItemList(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.AdminMessage = TempData["AdminMessage"];
            List<Item> AllItems = context.AllItemsWithTypes();
            Dictionary<int, string> ItemSubtypes = new Dictionary<int, string>();
            foreach(Item item in AllItems){
                string subtypeString = string.Join(", ", (from subitemtype in context.SubItemTypes 
                                       join subtype in context.Subtypes on subitemtype.SubItemTypeId equals subtype.SubItemTypeId
                                       where subtype.ItemId == item.ItemId select subitemtype.Name).ToList());
                subtypeString = subtypeString == "" ? "None" : subtypeString;
                ItemSubtypes[item.ItemId] = subtypeString;
            }
            ViewBag.AllItems = AllItems;
            ViewBag.ItemSubtypes = ItemSubtypes;
            return View();
        }

        [HttpGet]
        [Route("item/edit/{pid}")]
        public IActionResult EditItem(int pid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            Item item = context.GetOneItemWithTypes(pid);
            if(item == null){
                TempData["AdminMessage"] = $"Item with the requested id {pid} not found!";
                return RedirectToAction("AdminHome");
            }
            List<Subtype> subtypes = item.Subtypes;
            ViewBag.Item = item;
            ViewBag.MainItemTypes = context.MainItemTypes.ToList();
            ViewBag.SubItemTypes = context.SubItemTypes.ToList();
            ViewBag.AssignedSubItemTypes = subtypes; 
            return View();
        }

        [HttpPost]
        [Route("item/edit/{pid}")]
        public IActionResult EditItem(Item p, int pid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            Item original = context.GetOneItemWithTypes(pid);
            if(original == null){
                TempData["AdminMessage"] = $"Item with the requested id {pid} not found!";
                return RedirectToAction("AdminHome");
            }

            ViewBag.Item = original;
            if(ModelState.IsValid){
                bool StillValid = true;
                Item existing = context.GetOneItemWithTypes(p.Name);
                if(existing != null && existing.ItemId != pid){
                    ViewBag.NameError = "An item with that name already exists!";
                    StillValid = false;
                }

                string imageError;
                StillValid = ValidateImageLocation(p.Image, out imageError) ? StillValid : false;
                ViewBag.ImageError = imageError;

                string mainItemTypeError;
                StillValid = ValidateMainItemType(HttpContext.Request.Form["MainItemType"], out mainItemTypeError) ? StillValid : false;
                ViewBag.MainItemTypeError = mainItemTypeError;

                List<int> SubItemTypeIds = new List<int>();
                string subItemTypeError;
                StillValid = ValidateSubItemTypes(HttpContext.Request.Form["SubItemType"].ToList(), SubItemTypeIds, out subItemTypeError) ? StillValid : false;
                ViewBag.SubItemTypeError = subItemTypeError;
                if(StillValid){
                    original.Name = p.Name;
                    original.Description = p.Description;
                    original.Image = p.Image;
                    original.IsTradeable = p.IsTradeable ?? false;
                    original.MainItemTypeId = Int32.Parse(HttpContext.Request.Form["MainItemType"]);
                    List<Subtype> ExistingSubtypes = context.Subtypes.Where(subtype => subtype.ItemId == original.ItemId).ToList();
                    foreach(Subtype existingSubtype in ExistingSubtypes){
                        if(SubItemTypeIds.Contains(existingSubtype.SubItemTypeId)){
                            SubItemTypeIds.Remove(existingSubtype.SubItemTypeId); //If the existing SubItemTypeId is there, there's no need to add it
                        }
                        else{
                            context.Subtypes.Remove(existingSubtype); //If it's not there, then that means it is being deleted
                        }
                    }
                    foreach(int subId in SubItemTypeIds){
                        Subtype s = new Subtype();
                        s.SubItemTypeId = subId;
                        s.ItemId = original.ItemId;
                        context.Add(s);
                    }
                    context.SaveChanges();
                    TempData["AdminMessage"] = $"Item {p.Name} successfully edited!";
                    return RedirectToAction("ItemList");
                }
            }
            List<Subtype> subtypes = original.Subtypes;
            ViewBag.MainItemTypes = context.MainItemTypes.ToList();
            ViewBag.SubItemTypes = context.SubItemTypes.ToList();
            ViewBag.AssignedSubItemTypes = subtypes;
            return View("EditItem");
        }

        [HttpGet]
        [Route("item/delete/{pid}")]
        public IActionResult DeleteItem(int pid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            context.DeleteItem(pid);
            context.SaveChanges();
            TempData["AdminMessage"] = $"Main Item Type #{pid} successfully deleted! I hope you knew what you were doing!";
            return RedirectToAction("ItemList");
        }
        #endregion

        #region MainItemType CRUD
        [HttpGet]
        [Route("mainitemtype/edit")]
        public IActionResult AllMainItemTypes(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.AdminMessage = TempData["AdminMessage"];
            ViewBag.AllMainItemTypes = context.MainItemTypes.ToList();
            return View();
        }

        [HttpGet]
        [Route("mainitemtype/new")]
        public IActionResult NewMainItemType(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return View();
        }

        [HttpPost]
        [Route("mainitemtype/new")]
        public IActionResult CreateMainItemType(MainItemType NewType){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            if(ModelState.IsValid){
                MainItemType existing = context.GetOneMainItemType(NewType.Name);
                if(existing != null && existing.MainItemTypeId != NewType.MainItemTypeId){
                    ViewBag.NameError = "A Main Item Type with that name already exists!";
                    return View("NewMainItemType");
                }
                context.Add(NewType);
                context.SaveChanges();
                TempData["AdminMessage"] = $"{NewType.Name} successfully added to the database!";
                return RedirectToAction("AllMainItemTypes");
            }
            return View("NewMainItemType");
        }

        [HttpGet]
        [Route("mainitemtype/edit/{mid}")]
        public IActionResult EditMainItemType(int mid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.MainItemType = context.GetOneMainItemType(mid);
            if(ViewBag.MainItemType == null){
                TempData["AdminMessage"] = $"Main Item Type with the requested id {mid} not found!";
                return RedirectToAction("AllMainItemTypes");
            }
            return View();
        }

        [HttpPost]
        [Route("mainitemtype/edit/{mid}")]
        public IActionResult EditMainItemType(MainItemType ItemType, int mid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            MainItemType original = context.GetOneMainItemType(mid);
            if(original == null){
                TempData["AdminMessage"] = $"Main Item Type with the requested id {mid} not found!";
                return RedirectToAction("AllMainItemTypes");
            }

            ViewBag.MainItemType = original;
            if(ModelState.IsValid){
                MainItemType existing = context.GetOneMainItemType(ItemType.Name);
                if(existing != null && existing.MainItemTypeId != mid){
                    ViewBag.NameError = "A Main Item Type with that name already exists!";
                    return View("EditMainItemType");
                }
                original.Name = ItemType.Name;
                original.Description = ItemType.Description;
                context.SaveChanges();
                TempData["AdminMessage"] = $"Main Item Type #{mid} successfully edited!";
                return RedirectToAction("AllMainItemTypes");
            }
            return View();
        }

        [HttpGet]
        [Route("mainitemtype/delete/{mid}")]
        public IActionResult DeleteMainItemType(int mid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            context.DeleteMainItemType(mid);
            context.SaveChanges();
            TempData["AdminMessage"] = $"Main Item Type #{mid} successfully deleted! I hope you knew what you were doing!";
            return RedirectToAction("AllMainItemTypes");
        }
        #endregion

        #region SubItemType CRUD 
        [HttpGet]
        [Route("subitemtype/edit")]
        public IActionResult AllSubItemTypes(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.AdminMessage = TempData["AdminMessage"];
            ViewBag.AllSubItemTypes = context.SubItemTypes.ToList();
            return View();
        }

        [HttpGet]
        [Route("subitemtype/new")]
        public IActionResult NewSubItemType(){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            return View();
        }

        [HttpPost]
        [Route("subitemtype/new")]
        public IActionResult CreateSubItemType(SubItemType NewType){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            if(ModelState.IsValid){
                SubItemType existing = context.GetOneSubItemType(NewType.Name);
                if(existing != null && existing.SubItemTypeId != NewType.SubItemTypeId){
                    ViewBag.NameError = "A Sub Item Type with that name already exists!";
                    return View("NewSubItemType");
                }
                context.Add(NewType);
                context.SaveChanges();
                TempData["AdminMessage"] = $"{NewType.Name} successfully added to the database!";
                return RedirectToAction("AllSubItemTypes");
            }
            return View("NewSubItemType");
        }

        [HttpGet]
        [Route("subitemtype/edit/{mid}")]
        public IActionResult EditSubItemType(int mid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            ViewBag.SubItemType = context.GetOneSubItemType(mid);
            if(ViewBag.SubItemType == null){
                TempData["AdminMessage"] = $"Sub Item Type with the requested id {mid} not found!";
                return RedirectToAction("AllSubItemTypes");
            }
            return View();
        }

        [HttpPost]
        [Route("subitemtype/edit/{mid}")]
        public IActionResult EditSubItemType(SubItemType ItemType, int mid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            SubItemType original = context.GetOneSubItemType(mid);
            if(original == null){
                TempData["AdminMessage"] = $"Sub Item Type with the requested id {mid} not found!";
                return RedirectToAction("AllSubItemTypes");
            }

            ViewBag.SubItemType = original;
            if(ModelState.IsValid){
                SubItemType existing = context.GetOneSubItemType(ItemType.Name);
                if(existing != null && existing.SubItemTypeId != mid){
                    ViewBag.NameError = "A Sub Item Type with that name already exists!";
                    return View("EditSubItemType");
                }
                original.Name = ItemType.Name;
                original.Description = ItemType.Description;
                context.SaveChanges();
                TempData["AdminMessage"] = $"Sub Item Type #{mid} successfully edited!";
                return RedirectToAction("AllSubItemTypes");
            }
            return View();
        }

        [HttpGet]
        [Route("subitemtype/delete/{mid}")]
        public IActionResult DeleteSubItemType(int mid){
            ViewBag.User = HasAccess();
            if(ViewBag.User == null){
                return RedirectToAction("Index", "Home", new {area = "Account"});
            }
            context.DeleteSubItemType(mid);
            context.SaveChanges();
            TempData["AdminMessage"] = $"Sub Item Type #{mid} successfully deleted! I hope you knew what you were doing!";
            return RedirectToAction("AllSubItemTypes");
        }
        #endregion

        #region ItemValidations
        public bool ValidateUniqueItemName(string name, out string errorMessage){
            errorMessage = "";
            if(context.GetOneItem(name) != null){
                errorMessage = "An item with that name already exists!";
                return false;
            }
            return true;
        }

        public bool ValidateImageLocation(string path, out string errorMessage){
            errorMessage = "";
            var webRoot = _env.WebRootPath;
            var file = Path.Combine(webRoot + "\\images", path);
            if(!FileIO.Exists(file)){
                errorMessage = "The file couldn't be found. Please check the spelling and file extension!";
                return false;
            }
            return true;
        }

        public bool ValidateMainItemType(string typeId, out string errorMessage){
            errorMessage = "";
            int providedMainItemTypeId = -1;
            if(typeId == ""){
                errorMessage = "Please specify a Main Item Type for the item.";
                return false;
            }
            else{
                if (!Int32.TryParse(typeId, out providedMainItemTypeId))
                {
                    errorMessage = "The given Main Item Type Id couldn't be parsed.";
                    return false;
                }
                if(providedMainItemTypeId != -1){
                    MainItemType existingMainType = context.GetOneMainItemType(providedMainItemTypeId);
                    if(existingMainType == null){
                        errorMessage = "No Main Item Type with the specified Id could be found.";
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ValidateSubItemTypes(List<string> stringIds, List<int> intIds, out string errorMessage){
            errorMessage = "";
            if(stringIds.Count == 0){
                errorMessage = "Please specify at least one Sub Item Type for the item.";
                return false;
            }
            else{
                foreach(string stringId in stringIds){
                    int intId = -1;
                    if (!Int32.TryParse(stringId, out intId))
                    {
                        errorMessage = "At least one of the given Sub Item Type Ids couldn't be parsed.";
                        return false;
                    }
                    SubItemType existingSubType = context.GetOneSubItemType(intId);
                    if(existingSubType == null){
                        errorMessage = "At least one of the given Sub Item Type Ids didn't match any Sub Item Type in the database.";
                        return false;
                    }
                    intIds.Add(intId);
                }
            }
            return true;
        }
        #endregion

    }
}