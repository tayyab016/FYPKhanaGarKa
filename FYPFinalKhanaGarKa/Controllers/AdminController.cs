﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace FYPFinalKhanaGarKa.Controllers
{
    public class AdminController : Controller
    {
        private KhanaGarKaFinalContext db = null;
        private const string SessionEmail = "_Email";

        public AdminController(KhanaGarKaFinalContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Admin a = db.Admin.Where(
                     i => i.Email == vm.Email &&
                     i.Password == vm.Password).FirstOrDefault();
                if (a != null)
                {
                    HttpContext.Session.SetString(SessionEmail, vm.Email);

                    return Redirect("/admin/index");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect Email or Password.");
                }
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }

        [HttpGet]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetString(SessionEmail) == null)

            {
                return Redirect("/admin/login");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Chefs()
        {
            if (HttpContext.Session.GetString(SessionEmail) == null)

            {
                return Redirect("/admin/login");
            }
            return View(db.Chef.ToList());
        }

        [HttpGet]
        public IActionResult Customers()
        {
            if (HttpContext.Session.GetString(SessionEmail) == null)

            {
                return Redirect("/admin/login");
            }
            return View(db.Customer.ToList());
        }

        [HttpGet]
        public IActionResult Orders()
        {
            if (HttpContext.Session.GetString(SessionEmail) == null)

                {
                    return Redirect("/admin/login");
                }
            return View(db.Orders.ToList());
        }

        [HttpGet]
        public IActionResult DBoy()
        {
            if (HttpContext.Session.GetString(SessionEmail) == null)

            {
                return Redirect("/admin/login");
            }
            return View(db.DeliveryBoy.ToList());
        }
        
        [HttpPost]
        public string ApproveReq(int Id, string Role)
        {
            if (string.Equals(Role, "DBoy", StringComparison.OrdinalIgnoreCase))
            {
                DeliveryBoy d = db.DeliveryBoy.Where(i => i.DeliveryBoyId == Id).FirstOrDefault();
                d.Approved = true;
                using(var tr = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.DeliveryBoy.Update(d);
                        db.SaveChanges();

                        tr.Commit();
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
                Chef c = db.Chef.Where(i => i.ChefId == Id).FirstOrDefault();
                c.Approved = true;
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
                    }
                }
            }
            return "";
        }
    }
}
