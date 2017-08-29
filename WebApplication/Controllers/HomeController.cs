using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private static UserModel _db = new UserModel();

        public ActionResult Index()
        {
            return View(_db.Users);
        }

        public ActionResult Register()
        {
            ViewBag.Genders = new SelectList(_db.Genders.Select(g => g.Name));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                User userInDb = _db.Users.FirstOrDefault(u => u.Login == model.Login);
                if (userInDb == null)
                {
                    User u = new User
                    {
                        Name = model.Name,
                        Login = model.Login,
                        Email = model.Email,
                        Password = model.Password,
                        Gender_Id = _db.Genders.FirstOrDefault(i => i.Name == model.Gender.Name).Id
                    };
                    _db.Users.Add(u);
                    _db.SaveChanges();

                    userInDb = _db.Users.Where(i => i.Login == model.Login && i.Password == model.Password).FirstOrDefault();
                    // если пользователь удачно добавлен в бд
                    if (userInDb != null)
                    {
                        FormsAuthentication.SetAuthCookie(userInDb.Login, true);
                        return RedirectToAction("GoodRegistration");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            ViewBag.Genders = new SelectList(_db.Genders.Select(g => g.Name));
            return View(model);
        }

        public ActionResult GoodRegistration()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                User user = _db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Boys()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            User user = _db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (user.Gender.Name == "girl")
            {
                return RedirectToAction("UserProfile");
            }
            ViewBag.Email = user.Email;
            ViewBag.Boys = _db.Users.Where(u => u.Gender.Name == "boy").Count();
            ViewBag.Girls = _db.Users.Where(u => u.Gender.Name == "girl").Count();
            return View();
        }

        public ActionResult UserProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            User user = _db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            return View(user);
        }
    }
}