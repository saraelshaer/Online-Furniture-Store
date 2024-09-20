using FurnitureStore.Data;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context =new AppDbContext();
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return Content("Added : )))");
            }
            else
            {
                return View(user);
            }
           
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {

            }
            else
            {
                return View(model);
            }
            return Content("Added : )");
        }

        public IActionResult Logout()
        {
            return View();
        }

        //public JsonResult CheckEmail(string email)
        //{
        //    var user = _context.Users.SingleOrDefault(u => u.Email == email);
        //    if (user != null)
        //    {
        //        return Json(false);  // Email is already in use
        //    }
        //    return Json(true);  // Email is available
        //}


    }
}
