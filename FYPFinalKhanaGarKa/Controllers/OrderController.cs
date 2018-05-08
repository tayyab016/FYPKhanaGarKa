using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class OrderController : Controller
    {
        private const string SessionUser = "_User";
        private KhanaGarKaFinalContext db = null;

        public OrderController(KhanaGarKaFinalContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int id)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    IEnumerable<Menu> menus = db.Menu.Where(i => i.ChefId == id).ToList().OrderByDescending(i => i.ModifiedDate);
                    IEnumerable<Offer> offers = db.Offer.Where(i => i.ChefId == id).ToList().OrderByDescending(i => i.ModifiedDate);
                    MenuOfferViewModel ViewModel = new MenuOfferViewModel
                    {
                        Menus = menus,
                        Offers = offers
                    };
                    return View(ViewModel);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }
        
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                List<OrderLine> Dishs = db.OrderLine.Where(i => i.OrderId == id).ToList();
                Orders Order = db.Orders.Where(i => i.OrderId == id).FirstOrDefault();
                Chef c = db.Chef.Where(i => i.ChefId == Order.ChefId).FirstOrDefault();
                

                OrderDetailViewModel ViewModel = new OrderDetailViewModel
                {
                    Dishis = Dishs,
                    Chef = c,
                    Order = Order,
                    Role = HttpContext.Session.Get<SessionData>(SessionUser).Role
                };

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public IActionResult Success(ItemGroup itemGroup)
        {
            if (HttpContext.Session.GetString(SessionUser) != null)
            {
                ItemGroup i = HttpContext.Session.Get<ItemGroup>("CartData");
                if (i != null)
                {
                    List<OrderLine> ol = new List<OrderLine>();
                    foreach (var items in i.Items)
                    {
                        ol.Add(new OrderLine
                        {
                            Name = items.Name,
                            Price = items.Price,
                            Quantity = items.Quantity
                        });
                    }
                    Orders o = new Orders
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = false,
                        OrderType = i.OrderType,
                        ChefId = i.Cid,
                        CustomerId = (int)HttpContext.Session.Get<SessionData>(SessionUser).Id,
                        OrderLine = ol,
                        SpReq = itemGroup.SpReq,
                        City = itemGroup.City,
                        Area = itemGroup.Area,
                        Street = itemGroup.Street
                    };
                    
                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Orders.Add(o);
                            db.SaveChanges();
                            tr.Commit();

                            HttpContext.Session.Set<ItemGroup>("CartData", null);
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        
        public IActionResult History()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {

                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View(db.Orders.Where(i => i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id).ToList());
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    return View(db.Orders.Where(i => i.CustomerId == HttpContext.Session.Get<SessionData>(SessionUser).Id).ToList());
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    return View(db.Orders.Where(i => i.DeliveryBoyId == HttpContext.Session.Get<SessionData>(SessionUser).Id).ToList());
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Summary()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Process()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                return View(HttpContext.Session.Get<ItemGroup>("CartData"));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public JsonResult RatingManag([FromBody]RatingViewModel data)
        {
            Chef c = db.Chef.Where(i => i.ChefId == data.Id).FirstOrDefault();
            c.Rating = (c.Rating + data.CRating) / 2;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Chef.Update(c);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Json(new {
                state = 0,
                msg = string.Empty
            });
        }

        [HttpPost]
        public JsonResult PostJson([FromBody]ItemGroup data)
        {
            if (data != null)
            {
                HttpContext.Session.Set<ItemGroup>("CartData", data);
            }

            return Json(new
            {
                state = 0,
                msg = string.Empty
            });
        }

        
    }
}