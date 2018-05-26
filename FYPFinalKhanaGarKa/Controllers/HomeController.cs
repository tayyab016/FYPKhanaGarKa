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
        public IActionResult Login()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) == null)
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (string.Equals(vm.Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = db.Chef.Where(
                        i => i.Email == vm.Email &&
                        i.Password == vm.Password &&
                        i.Approved == true).FirstOrDefault();
                    if(c != null)
                    {
                        HttpContext.Session.Set<SessionData>(SessionUser, new SessionData {
                            Id = c.ChefId,
                            Name = c.FirstName+" "+c.LastName,
                            CNIC = c.Cnic,
                            PhNo = c.PhoneNo,
                            Email = c.Email,
                            ImgUrl = c.ImgUrl,
                            Role = c.Role
                        });
                        return Redirect("/Chef/Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Email or Password.Do you selected your role?");
                    }

                }
                else if (string.Equals(vm.Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer cu = db.Customer.Where(
                        i => i.Email == vm.Email &&
                        i.Password == vm.Password).FirstOrDefault();
                    if (cu != null)
                    {
                        HttpContext.Session.Set<SessionData>(SessionUser, new SessionData
                        {
                            Id = cu.CustomerId,
                            Name = cu.FirstName + " " + cu.LastName,
                            CNIC = cu.Cnic,
                            PhNo = cu.PhoneNo,
                            Email = cu.Email,
                            ImgUrl = cu.ImgUrl,
                            Role = cu.Role
                        });
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Email or Password.Do you selected your role?");
                    }
                }
                else if (string.Equals(vm.Role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy d = db.DeliveryBoy.Where(
                        i => i.Email == vm.Email &&
                        i.Password == vm.Password &&
                        i.Approved == true).FirstOrDefault();
                    if (d != null)
                    {
                        HttpContext.Session.Set<SessionData>(SessionUser, new SessionData
                        {
                            Id = d.DeliveryBoyId,
                            Name = d.FirstName + " " + d.LastName,
                            CNIC = d.Cnic,
                            PhNo = d.PhoneNo,
                            Email = d.Email,
                            ImgUrl = d.ImgUrl,
                            Role = d.Role
                        });
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Email or Password.Do you selected your role?");
                    }
                }
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (string.Equals(vm.Role.Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = new Chef
                    {
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Category = 3,
                        Role = vm.Role.Trim(),
                        Status = true,
                        FirstName = vm.FirstName.Trim(),
                        LastName = vm.LastName.Trim(),
                        Gender = vm.Gender,
                        Dob = vm.Dob,
                        Email = vm.Email.Trim(),
                        Password = vm.Password,
                        PhoneNo = vm.PhoneNo.Trim(),
                        City = vm.City,
                        Area = vm.Area,
                        Street = vm.Street,
                        Cnic = vm.Cnic.Trim(),
                        Rating = 0,
                        Approved = false
                    };

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (vm.Image != null && vm.Image.Length > 0)
                            {
                                c.ImgUrl = Utils.UploadImageR(env, "/Uploads/Chefs/"+vm.Cnic, vm.Image);
                            }

                            db.Chef.Add(c);
                            db.SaveChanges();

                            tr.Commit();

                            if (c.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + c.ImgUrl))
                            {
                                var source = Tinify.FromFile(env.WebRootPath + c.ImgUrl);
                                var resized = source.Resize(new
                                {
                                    method = "fit",
                                    width = 300,
                                    height = 168
                                });
                                await resized.ToFile(env.WebRootPath + c.ImgUrl);
                            }

                            //GreetingsEmail(c.Email, c.FirstName, c.LastName);

                            return RedirectToAction("Registration", "Home");
                        }

                        catch
                        {
                            tr.Rollback();

                            ModelState.AddModelError("", "We are having some problem in registration");
                            return View(vm);
                        }
                    }
                }
                else if (string.Equals(vm.Role.Trim(), "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy d = new DeliveryBoy
                    {
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Status = true,
                        Role = "DBoy",
                        FirstName = vm.FirstName.Trim(),
                        LastName = vm.LastName.Trim(),
                        Gender = vm.Gender,
                        Dob = vm.Dob,
                        Email = vm.Email.Trim(),
                        Password = vm.Password,
                        PhoneNo = vm.PhoneNo.Trim(),
                        City = vm.City,
                        Area = vm.Area,
                        Street = vm.Street,
                        Cnic = vm.Cnic.Trim(),
                        Approved = false
                        
                    };

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (vm.Image != null && vm.Image.Length > 0)
                            {
                                d.ImgUrl = Utils.UploadImageR(env, "/Uploads/DBoy/"+vm.Cnic, vm.Image);
                            }

                            db.DeliveryBoy.Add(d);
                            db.SaveChanges();

                            tr.Commit();

                            if (d.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + d.ImgUrl))
                            {
                                var source = Tinify.FromFile(env.WebRootPath + d.ImgUrl);
                                var resized = source.Resize(new
                                {
                                    method = "cover",
                                    width = 300,
                                    height = 168
                                });
                                await resized.ToFile(env.WebRootPath + d.ImgUrl);
                            }

                            //GreetingsEmail(d.Email, d.FirstName, d.LastName);

                            return RedirectToAction("Registration", "Home");
                        }
                        catch
                        {
                            tr.Rollback();

                            ModelState.AddModelError("", "We are having some problem in registration");
                            return View(vm);
                        }
                    }
                }
                else if (string.Equals(vm.Role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer cu = new Customer
                    {
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Role = vm.Role.Trim(),
                        Status = true,
                        FirstName = vm.FirstName.Trim(),
                        LastName = vm.LastName.Trim(),
                        Gender = vm.Gender,
                        Dob = vm.Dob,
                        Email = vm.Email.Trim(),
                        Password = vm.Password,
                        PhoneNo = vm.PhoneNo.Trim(),
                        City = vm.City,
                        Area = vm.Area,
                        Street = vm.Street,
                        Cnic = vm.Cnic.Trim()
                    };

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (vm.Image != null && vm.Image.Length > 0)
                            {
                                cu.ImgUrl = Utils.UploadImageR(env, "/Uploads/Customer/"+vm.Cnic, vm.Image);
                            }

                            db.Customer.Add(cu);
                            db.SaveChanges();

                            tr.Commit();

                            if (cu.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + cu.ImgUrl))
                            {
                                var source = Tinify.FromFile(env.WebRootPath + cu.ImgUrl);
                                var resized = source.Resize(new
                                {
                                    method = "cover",
                                    width = 300,
                                    height = 168
                                });
                                await resized.ToFile(env.WebRootPath + cu.ImgUrl);
                            }

                            //GreetingsEmail(cu.Email, cu.FirstName, cu.LastName);

                            return RedirectToAction("Login", "Home");
                        }
                        catch
                        {
                            tr.Rollback();
                            
                            ModelState.AddModelError("", "We are having some problem in registration.");
                            return View();
                        }
                    }
                }
            }
            ModelState.AddModelError("", "We are having some problem in registration");
            return View(vm);
        }
        
        [HttpGet]
        public IActionResult ModifyDetails()
        {
            if (HttpContext.Session.Get<SessionData>(SessionUser) != null)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = db.Chef.Where(i => i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    return View(new RegisterViewModel
                    {
                        Id = c.ChefId,
                        Role = c.Role,
                        Status = c.Status,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Gender = c.Gender,
                        Dob = c.Dob,
                        Email = c.Email,
                        Password = c.Password,
                        PhoneNo = c.PhoneNo,
                        City = c.City,
                        Area = c.Area,
                        Street = c.Street,
                        Cnic = c.Cnic,
                        ImgUrl = c.ImgUrl,
                        About = c.About
                    });
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    return View(new RegisterViewModel
                    {
                        Id = c.CustomerId,
                        Role = c.Role,
                        Status = c.Status,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Gender = c.Gender,
                        Dob = c.Dob,
                        Email = c.Email,
                        Password = c.Password,
                        PhoneNo = c.PhoneNo,
                        City = c.City,
                        Area = c.Area,
                        Street = c.Street,
                        Cnic = c.Cnic,
                        ImgUrl = c.ImgUrl
                    });
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy d = db.DeliveryBoy.Where(i => i.DeliveryBoyId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    return View(new RegisterViewModel
                    {
                        Id = d.DeliveryBoyId,
                        Role = d.Role,
                        Status = d.Status,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Gender = d.Gender,
                        Dob = d.Dob,
                        Email = d.Email,
                        Password = d.Password,
                        PhoneNo = d.PhoneNo,
                        City = d.City,
                        Area = d.Area,
                        Street = d.Street,
                        Cnic = d.Cnic,
                        ImgUrl = d.ImgUrl
                    });
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(RegisterViewModel vm)
        {
            //if (ModelState.IsValid)
            //{
                if (string.Equals(vm.Role.Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                Chef c = db.Chef.Where(i => i.ChefId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                c.FirstName = vm.FirstName;
                c.LastName = vm.LastName;
                c.Gender = vm.Gender;
                c.Dob = vm.Dob;
                c.Email = vm.Email;
                c.ModifiedDate = DateTime.Now;
                c.PhoneNo = vm.PhoneNo;
                c.City = vm.City;
                c.Area = vm.Area;
                c.Street = vm.Street;
                c.Status = vm.Status;
                c.About = vm.About;

                using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                                c.ImgUrl = Utils.UploadImageU(env, "/Uploads/Chefs/"+vm.Cnic, vm.Image, vm.ImgUrl);
                        }
                        db.Chef.Update(c);
                        db.SaveChanges();
                        tr.Commit();

                        if (c.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + c.ImgUrl))
                        {
                            var source = Tinify.FromFile(env.WebRootPath + c.ImgUrl);
                            var resized = source.Resize(new
                            {
                                method = "cover",
                                width = 300,
                                height = 168
                            });
                            await resized.ToFile(env.WebRootPath + c.ImgUrl);
                        }

                        return RedirectToAction("Account", "Chef");
                    }
                        catch
                        {
                            tr.Rollback();

                            ModelState.AddModelError("", "We are having some problem in Updating your record.");
                            return ModifyDetails();
                    }
                    }
                }
                else if (string.Equals(vm.Role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    c.FirstName = vm.FirstName;
                    c.LastName = vm.LastName;
                    c.Gender = vm.Gender;
                    c.Dob = vm.Dob;
                    c.Email = vm.Email;
                    c.ModifiedDate = DateTime.Now;
                    c.PhoneNo = vm.PhoneNo;
                    c.City = vm.City;
                    c.Area = vm.Area;
                    c.Street = vm.Street;
                    c.Status = vm.Status;

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                            c.ImgUrl = Utils.UploadImageU(env,"/Uploads/Customer/"+vm.Cnic, vm.Image, vm.ImgUrl);
                        }

                            db.Customer.Update(c);
                            db.SaveChanges();

                            tr.Commit();

                        if (c.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + c.ImgUrl))
                        {
                            var source = Tinify.FromFile(env.WebRootPath + c.ImgUrl);
                            var resized = source.Resize(new
                            {
                                method = "cover",
                                width = 300,
                                height = 168
                            });
                            await resized.ToFile(env.WebRootPath + c.ImgUrl);
                        }
                    }
                        catch
                        {
                            tr.Rollback();
                        ModelState.AddModelError("", "We are having some problem in Updating your record.");
                        return ModifyDetails();
                    }
                    }
                }
                else if (string.Equals(vm.Role.Trim(), "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy c = db.DeliveryBoy.Where(i => i.DeliveryBoyId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    c.FirstName = vm.FirstName;
                    c.LastName = vm.LastName;
                    c.Gender = vm.Gender;
                    c.Dob = vm.Dob;
                    c.Email = vm.Email;
                    c.ModifiedDate = DateTime.Now;
                    c.PhoneNo = vm.PhoneNo;
                    c.City = vm.City;
                    c.Area = vm.Area;
                    c.Street = vm.Street;
                    c.Status = vm.Status;

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                            c.ImgUrl = Utils.UploadImageU(env,"/Uploads/DBoy/"+vm.Cnic, vm.Image, vm.ImgUrl);
                        }
                            db.DeliveryBoy.Update(c);
                            db.SaveChanges();
                            tr.Commit();
                        if (c.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + c.ImgUrl))
                        {
                            var source = Tinify.FromFile(env.WebRootPath + c.ImgUrl);
                            var resized = source.Resize(new
                            {
                                method = "cover",
                                width = 300,
                                height = 168
                            });
                            await resized.ToFile(env.WebRootPath + c.ImgUrl);
                        }
                    }
                        catch
                        {
                            tr.Rollback();
                        ModelState.AddModelError("", "We are having some problem in Updating your record.");
                        return ModifyDetails();
                    }
                    }
                }
            //}
            return RedirectToAction("ModifyDetails", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                
                if (string.Equals(vm.Role.Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = db.Chef.Where(i => i.Email == vm.Email).FirstOrDefault();
                    if (c != null)
                    {
                        using (var tr = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.Chef.Update(c);
                                db.SaveChanges();
                                tr.Commit();
                                Utils.ResetPassEmail(c.Email, c.FirstName + " " + c.LastName, c.Password);

                            }
                            catch
                            {
                                tr.Rollback();
                                ModelState.AddModelError("", "We are having some problems in reseting.");
                                return View(vm);
                            }
                        }
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "invalid Email or Role");
                        return View(vm);
                    }
                }
                else if (string.Equals(vm.Role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.Email == vm.Email).FirstOrDefault();
                    if (c != null)
                    {
                        using (var tr = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.Customer.Update(c);
                                db.SaveChanges();
                                tr.Commit();
                                Utils.ResetPassEmail(c.Email, c.FirstName, c.Password);
                            }
                            catch
                            {
                                tr.Rollback();
                                ModelState.AddModelError("", "We are having some problems in reseting.");
                                return View(vm);
                            }
                        }
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "invalid Email or Role");
                        return View(vm);
                    }
                    
                }
                else if (string.Equals(vm.Role.Trim(), "Delivery Boy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy b = db.DeliveryBoy.Where(i => i.Email == vm.Email).FirstOrDefault();
                    if (b != null)
                    {
                        using (var tr = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.DeliveryBoy.Update(b);
                                db.SaveChanges();
                                tr.Commit();
                                Utils.ResetPassEmail(b.Email, b.FirstName, b.Password);
                            }
                            catch
                            {
                                tr.Rollback();
                                ModelState.AddModelError("", "We are having some problems in reseting.");
                                return View(vm);
                            }
                        }
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "invalid Email or Role");
                        return View(vm);
                    }
                }
            }
            return View(vm);
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
            Utils.ContactEmail(vm.Email, vm.Name, vm.PhoneNo, vm.Msg);
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
        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}