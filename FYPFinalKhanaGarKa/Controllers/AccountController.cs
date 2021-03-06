﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TinifyAPI;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class AccountController : Controller
    {
        private const string SessionUser = "_User";
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;

        public AccountController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
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
                    if (c != null)
                    {
                        HttpContext.Session.Set<SessionData>(SessionUser, new SessionData
                        {
                            Id = c.ChefId,
                            Name = c.FirstName + " " + c.LastName,
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
                        return Redirect("/Home/Index");
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
                        return Redirect("/Home/Index");
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
            return RedirectToAction("Index","Home");
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
                                c.ImgUrl = Utils.UploadImageR(env, "/Uploads/Chefs/" + vm.Cnic, vm.Image);
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

                            return RedirectToAction("Registration", "Account");
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
                                d.ImgUrl = Utils.UploadImageR(env, "/Uploads/DBoy/" + vm.Cnic, vm.Image);
                            }

                            db.DeliveryBoy.Add(d);
                            db.SaveChanges();

                            tr.Commit();

                            if (d.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + d.ImgUrl))
                            {
                                var source = Tinify.FromFile(env.WebRootPath + d.ImgUrl);
                                var resized = source.Resize(new
                                {
                                    method = "fit",
                                    width = 300,
                                    height = 168
                                });
                                await resized.ToFile(env.WebRootPath + d.ImgUrl);
                            }

                            //GreetingsEmail(d.Email, d.FirstName, d.LastName);

                            return RedirectToAction("Registration", "Account");
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
                                cu.ImgUrl = Utils.UploadImageR(env, "/Uploads/Customer/" + vm.Cnic, vm.Image);
                            }

                            db.Customer.Add(cu);
                            db.SaveChanges();

                            tr.Commit();

                            if (cu.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + cu.ImgUrl))
                            {
                                var source = Tinify.FromFile(env.WebRootPath + cu.ImgUrl);
                                var resized = source.Resize(new
                                {
                                    method = "fit",
                                    width = 300,
                                    height = 168
                                });
                                await resized.ToFile(env.WebRootPath + cu.ImgUrl);
                            }

                            //GreetingsEmail(cu.Email, cu.FirstName, cu.LastName);
                            ViewBag.Success = "Now you are the part of KhanaGarKa.com.Stay Blessed and enjoy healty home made food.";
                            ViewBag.Login = "You can Login Now.";
                            return View();
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
                    return View(new ModifyDetailsViewModel
                    {
                        Role = c.Role,
                        Status = c.Status,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Gender = c.Gender,
                        Dob = c.Dob,
                        Email = c.Email,
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
                    return View(new ModifyDetailsViewModel
                    {
                        Role = c.Role,
                        Status = c.Status,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Gender = c.Gender,
                        Dob = c.Dob,
                        Email = c.Email,
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
                    return View(new ModifyDetailsViewModel
                    {
                        Role = d.Role,
                        Status = d.Status,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Gender = d.Gender,
                        Dob = d.Dob,
                        Email = d.Email,
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
        public async Task<IActionResult> ModifyDetails(ModifyDetailsViewModel vm)
        {
            if (ModelState.IsValid)
            {
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
                            c.ImgUrl = Utils.UploadImageU(env, "/Uploads/Chefs/" + vm.Cnic, vm.Image, vm.ImgUrl);
                        }
                        db.Chef.Update(c);
                        db.SaveChanges();
                        tr.Commit();

                        if (c.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + c.ImgUrl) && vm.Image != null)
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
                        ViewBag.Success = " Your data is successfully modified.";

                        return View(new ModifyDetailsViewModel
                        {
                            Role = c.Role,
                            Status = c.Status,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Gender = c.Gender,
                            Dob = c.Dob,
                            Email = c.Email,
                            PhoneNo = c.PhoneNo,
                            City = c.City,
                            Area = c.Area,
                            Street = c.Street,
                            Cnic = c.Cnic,
                            ImgUrl = c.ImgUrl,
                            About = c.About
                        });
                    }
                    catch
                    {
                        tr.Rollback();

                        ModelState.AddModelError("", "We are having some problem in Updating your record.");
                        return View(new ModifyDetailsViewModel
                        {
                            Role = c.Role,
                            Status = c.Status,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Gender = c.Gender,
                            Dob = c.Dob,
                            Email = c.Email,
                            PhoneNo = c.PhoneNo,
                            City = c.City,
                            Area = c.Area,
                            Street = c.Street,
                            Cnic = c.Cnic,
                            ImgUrl = c.ImgUrl,
                            About = c.About
                        });
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
                            c.ImgUrl = Utils.UploadImageU(env, "/Uploads/Customer/" + vm.Cnic, vm.Image, vm.ImgUrl);
                        }

                        db.Customer.Update(c);
                        db.SaveChanges();

                        tr.Commit();

                        if (c.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + c.ImgUrl) && vm.Image != null)
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
                        ViewBag.Success = " Your data is successfully modified.";

                        return View(new ModifyDetailsViewModel
                        {
                            Role = c.Role,
                            Status = c.Status,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Gender = c.Gender,
                            Dob = c.Dob,
                            Email = c.Email,
                            PhoneNo = c.PhoneNo,
                            City = c.City,
                            Area = c.Area,
                            Street = c.Street,
                            Cnic = c.Cnic,
                            ImgUrl = c.ImgUrl
                        });
                    }
                    catch
                    {
                        tr.Rollback();
                        ModelState.AddModelError("", "We are having some problem in Updating your record.");
                        return View(new ModifyDetailsViewModel
                        {
                            Role = c.Role,
                            Status = c.Status,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Gender = c.Gender,
                            Dob = c.Dob,
                            Email = c.Email,
                            PhoneNo = c.PhoneNo,
                            City = c.City,
                            Area = c.Area,
                            Street = c.Street,
                            Cnic = c.Cnic,
                            ImgUrl = c.ImgUrl
                        });
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
                            c.ImgUrl = Utils.UploadImageU(env, "/Uploads/DBoy/" + vm.Cnic, vm.Image, vm.ImgUrl);
                        }
                        db.DeliveryBoy.Update(c);
                        db.SaveChanges();
                        tr.Commit();
                        if (c.ImgUrl != null && System.IO.File.Exists(env.WebRootPath + c.ImgUrl) && vm.Image != null)
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
                        ViewBag.Success = " Your data is successfully modified.";

                        return View(new ModifyDetailsViewModel
                        {
                            Role = c.Role,
                            Status = c.Status,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Gender = c.Gender,
                            Dob = c.Dob,
                            Email = c.Email,
                            PhoneNo = c.PhoneNo,
                            City = c.City,
                            Area = c.Area,
                            Street = c.Street,
                            Cnic = c.Cnic,
                            ImgUrl = c.ImgUrl
                        });
                    }
                    catch
                    {
                        tr.Rollback();
                        ModelState.AddModelError("", "We are having some problem in Updating your record.");
                        return View(new ModifyDetailsViewModel
                        {
                            Role = c.Role,
                            Status = c.Status,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Gender = c.Gender,
                            Dob = c.Dob,
                            Email = c.Email,
                            PhoneNo = c.PhoneNo,
                            City = c.City,
                            Area = c.Area,
                            Street = c.Street,
                            Cnic = c.Cnic,
                            ImgUrl = c.ImgUrl
                        });
                    }
                }
            }
            }
            return RedirectToAction("ModifyDetails", "Account");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
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
        public IActionResult ModifyPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ModifyPassword(ModifyPasswordViewModel mp)
        {
            if (ModelState.IsValid)
            {
                if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    if (mp.OldPassword == c.Password)
                    {
                        c.Password = mp.NewPassword;
                        using (var tr = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.Customer.Update(c);
                                db.SaveChanges();
                                tr.Commit();
                            }
                            catch
                            {
                                tr.Rollback();
                                ModelState.AddModelError("", "Cannot modify your password.");
                                return View(mp);
                            }
                        }
                        ViewBag.Success = "Your Password is successfuly Modified.";
                        return View();

                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry your password Does not match with record.");
                        return View(mp);
                    }
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    if (mp.OldPassword == c.Password)
                    {
                        c.Password = mp.NewPassword;
                        using (var tr = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.Customer.Update(c);
                                db.SaveChanges();
                                tr.Commit();
                            }
                            catch
                            {
                                tr.Rollback();
                                ModelState.AddModelError("", "Cannot modify your password.");
                                return View(mp);
                            }
                        }
                        ViewBag.Success = "Your Password is successfuly Modified.";
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry your password Does not match with record.");
                        return View(mp);
                    }
                }
                else if (string.Equals(HttpContext.Session.Get<SessionData>(SessionUser).Role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == HttpContext.Session.Get<SessionData>(SessionUser).Id).FirstOrDefault();
                    if (mp.OldPassword == c.Password)
                    {
                        if (mp.NewPassword == mp.ConfirmPassword)
                        {
                            c.Password = mp.NewPassword;
                            using (var tr = db.Database.BeginTransaction())
                            {
                                try
                                {
                                    db.Customer.Update(c);
                                    db.SaveChanges();
                                    tr.Commit();
                                }
                                catch
                                {
                                    tr.Rollback();
                                    ModelState.AddModelError("", "Cannot modify your password.");
                                    return View(mp);
                                }
                            }
                            ViewBag.Success = "Your Password is successfuly Modified.";
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry your password Does not match with record.");
                        return View(mp);
                    }
                }

            }
            return View();
        }
    }
}