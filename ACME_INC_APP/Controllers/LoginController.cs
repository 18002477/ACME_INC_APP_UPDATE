using ACME_INC_APP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME_INC_APP.Controllers
{
    public class LoginController : Controller
    {
        // Context model declaration
        ACMEContext db = new ACMEContext();

        // Instantiating the login controller
        public LoginController(ACMEContext context)
        {
            db = context;
        }

        // Returning the logged in user view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        /* public IActionResult Login(string username, string password)
         {
             try
             {
                 var getUser = (from s in db.Users where (s.Username == username) select s).FirstOrDefault();
                 if (getUser != null)
                 {
                     var hashCode = getUser.Vcode;
                     //Password Hasing Process Call from the Helper Class   
                     var encodingPasswordString = HelperClass.EncodePassword(password, hashCode);
                     //Check Login Detail User Name Or Password    
                     var query = (from s in db.Users where (s.Username == username) && s.Password.Equals(encodingPasswordString) select s).FirstOrDefault();
                     if (query != null)
                     {
                         return RedirectToAction("Index", "Products");// Change this
                     }
                     ViewBag.Error = "Invalid Credentials";
                     return View();
                 }
                 ViewBag.Error = "Invalid Credentials";
                 return View();
             }
             catch (Exception ex)
             {
                 ViewBag.Error = " Error!!! contact 18002477@vcconect.co.za" + ex;
                 return View();
             }

         }*/

        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var getUser = (from s in db.Users where (s.Username == username) select s).FirstOrDefault();
                if (getUser != null)
                {
                    var hashCode = getUser.Vcode;
                    //Password Hasing Process Call from the Helper Class   
                    var encodingPasswordString = HelperClass.EncodePassword(password, hashCode);
                    //Check Login Detail User Name Or Password
                    User userQuery = await db.Users.Where(u => u.Username.Equals(username) && u.Password.Equals(encodingPasswordString)).FirstOrDefaultAsync();
                    if (userQuery != null)
                    {
                        HttpContext.Session.SetInt32("UserID", userQuery.UserId);
                        HttpContext.Session.SetString("LoggedInUser", userQuery.Username);
                        HttpContext.Session.SetInt32("UserRoleID", userQuery.UserRoleId);
                        TempData["UsernameAsTempData"] = userQuery.Username;
                        return RedirectToAction("Index", "Products");
                    }
                    else
                    {
                        ViewBag.Error = "Incorect Details";
                        return View(getUser);
                    }
                }
                else
                {
                    ViewBag.Error = "Incorect Details";
                    return View(getUser);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = " Error!!! contact 18002477@vcconect.co.za" + ex;
                return View();
            }
        }

        // Returning the SignUp view 
        [HttpGet]
        public IActionResult SignUp()
        {
            // Binding the User Role
            ViewData["UserRoleId"] = new SelectList(db.UserRoles, "UserRoleId", "UserRole1");
            return View();
        }

        // Post method used to write to the database and create a new user in the DB
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignUp(string Username, string Email, int UserRoleId, string Password)
        {
            try
            {
                var chkUser = (from s in db.Users where s.Username == Username select s).FirstOrDefault();
                if (chkUser == null)
                {
                    User user = new User();
                    var keyNew = HelperClass.GeneratePassword(10);
                    var Pass = HelperClass.EncodePassword(Password, keyNew);
                    user.Username = Username;
                    user.Email = Email;
                    user.UserRoleId = UserRoleId;
                    user.Password = Pass;
                    user.Vcode = keyNew;

                    if (ModelState.IsValid)
                    {
                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                        ViewBag.Success = "User Successfully added !";
                        ModelState.Clear();// This will clear whatever form items have been populated
                        return View();
                    } // Here I'm returning the model as there's an error and the user needs to see what has been entered. 

                    return View(user);
                }
                ViewBag.Error = "Username Already Exists !";
                ViewData["UserRoleId"] = new SelectList(db.UserRoles, "UserRoleId", "UserRole1");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Some exception occured" + ex;
                return View();
            }
        }

        // This method is used to clear the session when the user logs out of the application 
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View();
        }
    }
}
