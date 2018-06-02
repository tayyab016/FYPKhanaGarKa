using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [HttpGet]
        public IActionResult Index(int id)
        {
            var vm = db.Chef
                       .Where(i => i.ChefId == id && i.Status == true)
                       .Select(i => new ChefAccountViewModel
                       {
                           ChefId = i.ChefId,
                           FirstName = i.FirstName,
                           LastName = i.LastName,
                           Orders = i.Orders.Where(z => z.OrderStatus == true).Count(),
                           Rating = (int)i.Rating,
                           Menu = i.Menu.OrderByDescending(z => z.ModifiedDate)
                           .Select(x => new Menu
                           {
                               Serving = x.Serving,
                               MenuId = x.MenuId,
                               DishLike = x.DishLike,
                               DishDislike = x.DishDislike,
                               ImgUrl = x.ImgUrl,
                               Description = x.Description,
                               DishName = x.DishName,
                               Status = x.Status,
                               Price = x.Price
                           }).ToList(),

                           Offer = i.Offer.Where(x =>  x.EndDate > DateTime.Now)
                           .Select(x => new Offer
                           {
                               ImgUrl = x.ImgUrl,
                               OfferName = x.OfferName,
                               Description = x.Description,
                               Percentage = x.Percentage,
                               EndDate = x.EndDate,
                               Price = x.Price
                           }).ToList()
                       }).FirstOrDefault();
            
            return View(vm);
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                List<OrderLine> Dishs = db.OrderLine.Where(i => i.OrderId == id).ToList();
                Orders Order = db.Orders.Where(i => i.OrderId == id).FirstOrDefault();
                Chef c = db.Chef.Where(i => i.ChefId == Order.ChefId).FirstOrDefault();

                var vm = db.Orders.Where(i => i.OrderId == id).Select(i => new OrderDetailViewModel
                {
                    FirstName = i.Chef.FirstName,
                    LastName = i.Chef.LastName,
                    OrderType = i.OrderType,
                    OrderDate = i.OrderDate,
                    OrderStatus = i.OrderStatus,
                    Received = i.Received,
                    Confirmed = (bool)i.Confirmed,
                    Canceled = (bool) i.Canceled,
                    OrderId = i.OrderId,
                    Orderline = i.OrderLine.ToList()
                    
                }).FirstOrDefault();

                return View(vm);
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
                        Street = itemGroup.Street,
                        Received = false,
                        Confirmed = false,
                        Canceled = false,
                        DeliveryDay = itemGroup.DeliveryDay,
                        DeliveryTime = itemGroup.DeliveryTime
                    };
                    
                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Orders.Add(o);
                            db.SaveChanges();
                            tr.Commit();

                            HttpContext.Session.Set<ItemGroup>("CartData", null);

                            if (itemGroup.DeliveryDay == 0 && itemGroup.DeliveryTime == "")
                            {
                                //Utils.OrderEmail("khanagarka@gmail.com", "Order placed from customer ID: " + o.ChefId + " to chef ID: " + o.CustomerId);
                                //Utils.OrderEmail(db.Chef.Where(x => x.ChefId == o.ChefId).Select(x => x.Email).FirstOrDefault(),
                                //    "You Have an order please visit your account and confirm order.");
                            }
                            else
                            {
                               // Utils.OrderEmail("khanagarka@gmail.com", "Order placed from customer ID: " + o.ChefId + " to chef ID: " + o.CustomerId+" and scheduled " + itemGroup.DeliveryDay + "  at " + itemGroup.DeliveryTime);
                               // Utils.OrderEmail(db.Chef.Where(x => x.ChefId == o.ChefId).Select(x => x.Email).FirstOrDefault(),
                               //     "You Have an order and customer want to receive order "+itemGroup.DeliveryDay+"  at "+itemGroup.DeliveryTime+" .Please, visit your account and confirm order.");
                            }
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
                return RedirectToAction("Login", "Account");
            }
        }
        
        [HttpGet]
        public IActionResult History()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {

                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    var vm = db.Orders.Where(i => i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id)
                        .Select(x => new OrderHistoryViewModel
                        {
                            OrderId = x.OrderId,
                            OrderDate = x.OrderDate,
                            OrderStatus = x.OrderStatus,
                            Received = x.Received,
                            Confirmed =(bool) x.Confirmed,
                            Canceled = (bool) x.Canceled,
                            Total = x.OrderLine.Sum(i => i.Price)
                        }).ToList();
                    return View(vm);
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    IList<OrderHistoryViewModel> vm = db.Orders.Where(i => i.CustomerId == HttpContext.Session.Get<SessionData>(SessionUser).Id)
                        .Select(x => new OrderHistoryViewModel
                        {
                            OrderId = x.OrderId,
                            OrderDate = x.OrderDate,
                            OrderStatus = x.OrderStatus,
                            Received = x.Received,
                            Confirmed = (bool)x.Confirmed,
                            Total = x.OrderLine.Sum(i => i.Price)
                        }).ToList();
                    return View(vm);
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    var vm = db.Orders.Where(i => i.DeliveryBoyId == HttpContext.Session.Get<SessionData>(SessionUser).Id)
                        .Select(x => new OrderHistoryViewModel
                        {
                            OrderId = x.OrderId,
                            OrderDate = x.OrderDate,
                            OrderStatus = x.OrderStatus,
                            Received = x.Received,
                            Total = x.OrderLine.Sum(i => i.Price)
                        }).ToList();
                    return View(vm);
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public IActionResult Process()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>("_User").Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    return View(HttpContext.Session.Get<ItemGroup>("CartData"));
                }
                else
                {
                    return Redirect("/Home/Page404");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public string Voting(int MenuId,int ChefId,int Likes,int Dislikes)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                Menu m = db.Menu.Where(i => i.ChefId == ChefId && i.MenuId == MenuId).FirstOrDefault();
                m.DishLike = Likes;
                m.DishDislike = Dislikes;
                using (var tr = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Menu.Update(m);
                        db.SaveChanges();

                        tr.Commit();
                        return "OK";
                    }
                    catch
                    {
                        tr.Rollback();
                        return "";
                    }
                }
            }
            else
            {
                return "Login";
            }
        }

        [HttpPost]
        public string RatingManag(int Id, int CRating)
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                Chef c = db.Chef.Where(i => i.ChefId == Id).FirstOrDefault();
                c.Rating = (c.Rating + CRating) / 2;
                using (var tr = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Chef.Update(c);
                        db.SaveChanges();

                        tr.Commit();
                        return "OK";
                    }
                    catch
                    {
                        tr.Rollback();
                        return "";
                    }
                }
            }
            else
            {
                return "Login";
            }
        }

        [HttpPost]
        public string OrderDisRec(int Id,string Role)
        {
            if (string.Equals(Role, "Customer", StringComparison.OrdinalIgnoreCase))
            {
                Orders o = db.Orders.Where(i => i.OrderId == Id).FirstOrDefault();
                o.Received = true;
                using (var tr = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Orders.Update(o);
                        db.SaveChanges();

                        tr.Commit();
                        
                        Utils.OrderEmail("khanagarka@gmail.com", "<p>Order ID: " + Id + " is recieved by Customer ID " + o.CustomerId + "</p>");

                        return "OK";
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                }
            }
            else if (string.Equals(Role, "Chef", StringComparison.OrdinalIgnoreCase))
            {
                Orders o = db.Orders.Where(i => i.OrderId == Id).FirstOrDefault();
                o.OrderStatus = true;
                using (var tr = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Orders.Update(o);
                        db.SaveChanges();

                        tr.Commit();

                        Utils.OrderEmail("khanagarka@gmail.com", "<p>Order ID: " + Id + " is Dispatched by Chef ID " + o.ChefId + "</p>");

                        return "OK";
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                }
            }
            return "";
        }

        [HttpPost]
        public string OrderConfirm(int Id)
        {
            Orders o = db.Orders.Where(i => i.OrderId == Id).FirstOrDefault();
            o.Confirmed = true;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Orders.Update(o);
                    db.SaveChanges();

                    tr.Commit();
                    Utils.OrderEmail("khanagarka@gmail.com", "Order ID: "+o.OrderId+" is confirmed by Chef ID: " + o.ChefId + " for Customer ID: " + o.CustomerId);
                    Utils.OrderEmail(db.Customer.Where(x => x.CustomerId == o.CustomerId).Select(x => x.Email).FirstOrDefault(),
                        "Your order is confirmed by the chef and deliverd to you within 150 min");
                    return "OK";
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return null;
        }

        [HttpPost]
        public string OrderCancel(int Id)
        {
            Orders o = db.Orders.Where(i => i.OrderId == Id).FirstOrDefault();
            o.Canceled = true;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Orders.Update(o);
                    db.SaveChanges();

                    tr.Commit();
                   // Utils.OrderEmail("khanagarka@gmail.com", "Order ID: " + o.OrderId + " is canceled by Chef ID: " + o.ChefId + " for Customer ID: " + o.CustomerId);
                   // Utils.OrderEmail(db.Customer.Where(x => x.CustomerId == o.CustomerId).Select(x => x.Email).FirstOrDefault(),
                   //     "Your order is canceled by the chef. Please, give order to another chef. THANK YOU.");
                    return "OK";
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return null;
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