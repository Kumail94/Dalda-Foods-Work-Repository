using DaldaProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vechile_Tracking_Systems.Models;

namespace Vechile_Tracking_Systems.Controllers
{
    public class LoginController : Controller
    {
        DFLDBEntiti db = new DFLDBEntiti();
        UserLog userLog = new UserLog();
        List<User> users = new List<User>();
        User dbUser = new User();
        
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginUserVM login)
        {
            var us = db.Users.Where(user => user.User_Name == login.UserName).FirstOrDefault();
            string decryptedPassword = Decrypt_Password(us.Password);
            var u = db.Users.Where(user => user.User_Name == login.UserName && decryptedPassword == login.Password).FirstOrDefault();
            if (u != null)
            {
                Session["userId"] = u.User_Id;
                Session["userName"] = u.User_Name;
                Session["loginTime"] = DateTime.Now;
                Session["hostName"] = Server.MachineName.ToString();
                return RedirectToAction("Index", "VehicleManagement");
            }
            else
            {
                ViewBag.ErrorMessage = "UserName or Password is incorrect.";
            }
            return View();
        }

        public ActionResult Logout()
        {
            userLog.User_Id = (int)Session["userId"];
            userLog.User_name = Session["userName"].ToString();
            userLog.Login_Time = (DateTime)Session["loginTime"];
            userLog.Logout_Time = DateTime.Now;
            userLog.Host_Name = Session["hostName"].ToString();
            db.UserLogs.Add(userLog);
            db.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
       
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserVM user)
        {
            //dbUser.User_Id = 1;
            dbUser.First_Name = user.FirstName;
            dbUser.Last_Name = user.LastName;
            dbUser.User_Name = user.UserName;
            dbUser.Password = Encrypt_Password(user.Password);
            dbUser.Email = user.Email;
            dbUser.Contact_Number = user.Contact;
            dbUser.Branch_Id = 1;

            if (ModelState.IsValid)
            {
                var validate = db.Users.Where(x => x.User_Name == user.UserName && x.Password == user.Password).Count();
                if (validate == 0)
                {
                    users.Add(dbUser);
                    db.Users.Add(users.SingleOrDefault());
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ValidateError = "Entered UserName or Password is already exist";
                }
                return RedirectToAction("Index", "Login");
            }
            return View();

        }
        private string Decrypt_Password(string cipherText)
        {
            byte[] encoded = Convert.FromBase64String(cipherText);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
        private string Encrypt_Password(string password)
        {
            string pswstr = string.Empty;
            byte[] psw_encode = new byte[password.Length];
            psw_encode = System.Text.Encoding.UTF8.GetBytes(password);
            pswstr = Convert.ToBase64String(psw_encode);
            return pswstr;

        }
    }
}