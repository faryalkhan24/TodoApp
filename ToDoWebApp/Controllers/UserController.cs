using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoWebApp.Models;
using ToDoWebApp.ViewModels;
using BCrypt.Net;

namespace ToDoWebApp.Controllers
{
    public class UserController : Controller
    {
        private Interfaces.IUserRepository userRepository;
        public UserController() {
            userRepository = new Models.UserRepository();
        }

        // GET: User/Index
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Password field is required");
            }
            else
            {
                if (!userRepository.IsUserExists(model.Username))
                {
                    string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    bool isCreated = userRepository.CreateUser(model.Username, model.Email, hashPassword);
                    if (!isCreated)
                    {
                        ModelState.AddModelError("", "Unable to create user");
                    }
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ModelState.AddModelError("Username", "Username already exists");
                }
            }
            return View(model);
        }



        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (userRepository.IsUserExists(model.Username))
            {
                Models.DAL.User user = userRepository.GetUser(model.Username);

                if(user!=null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    HttpContext.Session.SetString("LoggedInUsername", model.Username);
                    HttpContext.Session.SetInt32("LoggedInUserID",user.UserId);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid Username or Password");
                }
            }
            else
            {
                ModelState.AddModelError("Username", "Invalid Username");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedInUsername");
            HttpContext.Session.Remove("LoggedInUserID");

            return RedirectToAction("Login", "User");

        }


    }

}
