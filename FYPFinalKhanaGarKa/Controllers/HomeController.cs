using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TinifyAPI;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionUser = "_User";
        private const string SessionCode = "_FPCode";
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;

        public HomeController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel vm)
        {
            //Utils.ContactEmail(vm.Email, vm.Name, vm.PhoneNo, vm.Msg);
            ViewBag.Success = "Your message is successfully sent to the KhanaGarKa.com team.";
            return View();
        }

        [HttpGet]
        public IActionResult Report()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Page404");
            }
        }

        [HttpPost]
        public IActionResult Report(string Subject, string Msg)
        {
            //Utils.OrderEmail("khanagarka@gmail.com", "<h1>Complain<h1><br>ID: " + HttpContext.Session.Get<SessionData>(SessionUser).Id + "<br>" +
            //    "CNIC: " + HttpContext.Session.Get<SessionData>(SessionUser).CNIC + "<br>" + Msg);
            ViewBag.Success = "Your complain is forward to the admin.";
            return View();
        }

        [HttpPost]
        public string NewsLetter(string Email)
        {
            if (ModelState.IsValid)
            {
                if (Email != null && !Email.Equals(""))
                {
                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.NewsLetter.Add(entity: new NewsLetter { Email = Email });
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
            }
            return null;
        }

        [HttpGet]
        public IActionResult Privacy_Policy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Page404()
        {
            return View();
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}