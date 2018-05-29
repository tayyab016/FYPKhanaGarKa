using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TinifyAPI;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class Utils
    {

        //Helper Methods
        public static string GetUniqueName(string FileName)
        {
            return DateTime.Now.ToString("yyyyMMddHHmm") + "_" + Guid.NewGuid().ToString().Substring(0, 4) +
                        Path.GetExtension(FileName);
        }

        public static string GetCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }

        private static bool UploadImage(IHostingEnvironment env, IFormFile Image, string path, string name)
        {
            if (Image != null && Image.Length > 0 && Image.Length < 10000000)
            {
                string ext = Path.GetExtension(Image.FileName);

                if (string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = env.WebRootPath + path + "/" + name;
                    FileStream fs = new FileStream(filePath.Trim(), FileMode.Create);
                    Image.CopyTo(fs);
                    fs.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return true;
        }

        public static string UploadImageR(IHostingEnvironment env, string Dir, IFormFile Image)
        {
            string dir = env.WebRootPath + Dir;

            if (!Directory.Exists(dir.Trim()))
            {
                Directory.CreateDirectory(dir.Trim());
                string imgname = GetUniqueName(Image.FileName);
                bool isUploaded = UploadImage(env, Image, Dir.Trim(),imgname);
                if (isUploaded)
                {
                    return Dir.Trim() + "/" + imgname;
                }
                else
                {
                    // image is not uploaded do somthing
                    return null;
                }
            }
            else
            {
                string imgname = GetUniqueName(Image.FileName);
                bool isUploaded = UploadImage(env, Image, Dir.Trim(),imgname);
                if (isUploaded == true)
                {
                    return Dir.Trim() + "/" + imgname;
                }
                else
                {
                    // image is not uploaded do somthing
                    return null;
                }
            }
        }

        public static string UploadImageU(IHostingEnvironment env, string Dir, IFormFile Image, string ImgUrl)
        {
            string dir = env.WebRootPath + Dir;

            if (!Directory.Exists(dir.Trim()))
            {
                Directory.CreateDirectory(dir.Trim());

                bool isDeleted = DeleteImage(env.WebRootPath + ImgUrl.Trim());
                if (isDeleted == true)
                {
                    string imgname = GetUniqueName(Image.FileName);
                    bool isUploaded = UploadImage(env, Image, Dir.Trim(),imgname);
                    if (isUploaded == true)
                    {
                        return Dir.Trim()+ "/" + imgname;
                    }
                    else
                    {
                        // image is not uploded do somthing
                        return null;
                    }
                }
                else
                {
                    // image is not deleted and uploaded do somthing
                    return null;
                }
            }
            else
            {
                bool isDeleted = DeleteImage(env.WebRootPath + ImgUrl);
                if (isDeleted)
                {
                    string imgname = GetUniqueName(Image.FileName);
                    bool isUploaded = UploadImage(env, Image, Dir.Trim(),imgname);
                    if (isUploaded == true)
                    {
                        return Dir.Trim()+ "/" + imgname;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static void GreetingsEmail(string mailid, string name)
        {
            MailMessage MM = new MailMessage();
            MM.From = new MailAddress("khanagarka@gmail.com");
            MM.To.Add(mailid);
            MM.Subject = ("Welcome to KhanGarKa.com");
            MM.Body = "<h1>Dear " + name + "</h1><br>Thanks for registering on our website.<br><br>----<br>Regards,<br> KhanaGarKa Team";
            MM.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.Credentials = new System.Net.NetworkCredential("khanagarka@gmail.com", "stm-7063");
            sc.EnableSsl = true;

            sc.Send(MM);
        }

        public static void ContactEmail(string mailid, string name, string phone, string msg)
        {
            MailMessage MM = new MailMessage();
            MM.From = new MailAddress("khanagarka@gmail.com");
            MM.To.Add("khanagarka@gmail.com");
            MM.Subject = ("Contact");
            MM.Body = "<h4>Name: " + name + "</h4><br><h4>Phone: " + phone + "</h4><br><h4>Email: " + mailid + "</h4>" + msg;
            MM.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.Credentials = new System.Net.NetworkCredential("khanagarka@gmail.com", "stm-7063");
            sc.EnableSsl = true;

            sc.Send(MM);
        }

        public static void ResetPassEmail(string mailid, string name, string pass)
        {
            MailMessage MM = new MailMessage();
            MM.From = new MailAddress("khanagarka@gmail.com");
            MM.To.Add(mailid);
            MM.Subject = ("Password Reset Request for KhanGarKa.com");
            MM.Body = "<h1>Dear " + name + "</h1><br>Your password for <h2>KhanaGarKa.com</h2> is <h1>" + pass + "</h1>.<br> You can change it after login.<br><br>----<br>Regards,<br> KhanaGarKa Team";
            MM.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.Credentials = new System.Net.NetworkCredential("khanagarka@gmail.com", "stm-7063");
            sc.EnableSsl = true;

            sc.Send(MM);
        }

        public static void OrderEmail(string mailto,  string msg)
        {
            MailMessage MM = new MailMessage();
            MM.From = new MailAddress("khanagarka@gmail.com");
            MM.To.Add(mailto);
            MM.Subject = ("Order");
            MM.Body = msg;
            MM.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.Credentials = new System.Net.NetworkCredential("khanagarka@gmail.com", "stm-7063");
            sc.EnableSsl = true;

            sc.Send(MM);
        }


    }
}
