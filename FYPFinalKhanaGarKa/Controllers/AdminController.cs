using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using Microsoft.AspNetCore.Mvc;


namespace FYPFinalKhanaGarKa.Controllers
{
    public class AdminController : Controller
    {
        private KhanaGarKaFinalContext db = null;

        public AdminController(KhanaGarKaFinalContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Chefs() => View(db.Chef.ToList());

        [HttpGet]
        public IActionResult Customers() => View(db.Customer.ToList());

        [HttpGet]
        public IActionResult Orders() => View(db.Orders.ToList());

        [HttpGet]
        public IActionResult DBoy() => View(db.DeliveryBoy.ToList());
        
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
