using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class ChefController : Controller
    {
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;
        private const string SessionUser = "_User";

        public ChefController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpGet]
        public IActionResult Index(string City, string Area)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "customer", StringComparison.OrdinalIgnoreCase))
                
                {
                    List<Chef> chefs = db.Chef.Where(i => i.City.Contains(City) && i.Area.Contains(Area)).ToList();
                    return View(chefs);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        [HttpGet]
        public IActionResult Account()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    IEnumerable<Menu> menus = db.Menu.Where(i => i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id).ToList().OrderByDescending(i => i.ModifiedDate);
                    IEnumerable<Offer> offers = db.Offer.Where(i => i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id).ToList().OrderByDescending(i => i.ModifiedDate);

                    MenuOfferViewModel MenuOffer = new MenuOfferViewModel
                    {
                        Menus = menus,
                        Offers = offers
                    };
                    return View(MenuOffer);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult Menu()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public IActionResult Menu(MenuViewModel vm)
        {
            Menu m = new Menu {
                DishName = vm.DishName,
                Price = vm.Price,
                Description = vm.Description,
                Status = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ChefId = HttpContext.Session.Get<SessionData>(SessionUser).Id
            };
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        m.ImgUrl = Utils.UploadImageR(env, "/Uploads/Chefs/" + HttpContext.Session.Get<SessionData>(SessionUser).CNIC+"/Menu", vm.Image);
                    }

                    db.Menu.Add(m);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("Account");
        }

        [HttpGet]
        public IActionResult Offer()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public IActionResult Offer(OfferViewModel vm)
        {
            Offer o = new Offer
            {
                OfferName = vm.OfferName,
                Price = vm.Price,
                Description = vm.Description,
                EndDate = vm.EndDate,
                Percentage = vm.Percentage,
                Status = true,
                ChefId = HttpContext.Session.Get<SessionData>(SessionUser).Id,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now
            };
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        o.ImgUrl = Utils.UploadImageR(env, "/Uploads/Chefs/" + HttpContext.Session.Get<SessionData>(SessionUser).CNIC+"/Offer", vm.Image);
                    }
                    db.Offer.Add(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("Account");
        }

        [HttpPost]
        public IActionResult DeleteMenu(MenuViewModel vm)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    Utils.DeleteImage(env.WebRootPath+vm.ImgUrl);
                    Menu m = new Menu() { MenuId = vm.MenuId };
                    db.Menu.Attach(m);
                    db.Menu.Remove(m);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("Account");
        }

        [HttpPost]
        public IActionResult DeleteOffer(OfferViewModel vm)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    Utils.DeleteImage(env.WebRootPath + vm.ImgUrl);
                    Offer o = new Offer() { OfferId = vm.OfferId };
                    db.Offer.Remove(o);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("Account");
        }

        [HttpGet]
        public IActionResult EditMenu(int Id)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Menu m = db.Menu.Where(i => i.MenuId == Id && i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    return View(new MenuViewModel {
                        MenuId = m.MenuId,
                        DishName = m.DishName,
                        Description = m.Description,
                        ImgUrl = m.ImgUrl,
                        Status = m.Status,
                        Price = m.Price
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        [HttpPost]
        public IActionResult EditMenu(MenuViewModel vm)
        {
            Menu menu = db.Menu.Where(i => i.MenuId == vm.MenuId).FirstOrDefault();
            menu.ModifiedDate = DateTime.Now;
            menu.DishName = vm.DishName;
            menu.Price = vm.Price;
            menu.Status = vm.Status;
            menu.Description = vm.Description;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        menu.ImgUrl = Utils.UploadImageU(env, "/Uploads/Chefs/" + HttpContext.Session.Get<SessionData>(SessionUser).CNIC + "/Menu", vm.Image, menu.ImgUrl);
                    }
                    db.Menu.Update(menu);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("Account");
        }

        [HttpGet]
        public IActionResult EditOffer(int id)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Offer o = db.Offer.Where<Offer>(i => i.OfferId == id && i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault<Offer>();
                    return View(new OfferViewModel {
                        OfferId = o.OfferId,
                        OfferName = o.OfferName,
                        Percentage = o.Percentage,
                        EndDate = o.EndDate,
                        ImgUrl = o.ImgUrl,
                        Price = o.Price,
                        Description = o.Description
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        [HttpPost]
        public IActionResult EditOffer(OfferViewModel vm)
        {
            Offer o = db.Offer.Where(i => i.OfferId == vm.OfferId).FirstOrDefault();
            o.ModifiedDate = DateTime.Now;
            o.OfferName = vm.OfferName;
            o.Description = vm.Description;
            o.Price = vm.Price;
            o.EndDate = vm.EndDate;
            o.Percentage = vm.Percentage;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if(vm.Image != null && vm.Image.Length > 0)
                    {
                        o.ImgUrl = Utils.UploadImageU(env, "/Uploads/Chefs/" + HttpContext.Session.Get<SessionData>(SessionUser).CNIC + "/Offer", vm.Image, o.ImgUrl);
                    }
                    db.Offer.Update(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("Account");
        }
    }
}