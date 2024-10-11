using FurnitureStore.IRepository;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace FurnitureStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.Save();
                var role = _unitOfWork.RoleRepository.Find(r=>r.Name == "Regular user");
                if(role != null)
                {
                    _unitOfWork.UserRoleRepo.Add(new UserRole { RoleId = role.Id, UserId = user.Id });
                    _unitOfWork.Save();
                }
                await Login(new LoginViewModel { Email = user.Email, Password = user.Password });

                return RedirectToAction("Index","home");
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
               
                var loginUser = _unitOfWork.UserRepository.Find(u=>u.Email== model.Email && u.Password == model.Password );
                if(loginUser != null)
                {
                    var cliams = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, loginUser.Email),
                        new Claim(ClaimTypes.NameIdentifier, loginUser.Id.ToString()),
                        new Claim(ClaimTypes.Name, loginUser.FirstName),
                    };

                    TempData["ImageProfile"] = loginUser.ImageFileName;
                    TempData["NoOfWishlist"] = loginUser.WishList?.WishListProducts?.Count() ?? 0;
                    TempData["NoOfCart"] = loginUser.Cart?.CartProducts?.Where(p=>p.Product.IsActive).Sum(p=>p.Quantity) ?? 0;
                    var roles = _unitOfWork.UserRoleRepo.FindAll<string>(ur => ur.UserId == loginUser.Id, ur => ur.Role.Name, new[] {"Role"});

                    foreach (var role in roles)
                    {
                        cliams.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var claimIdentity=new ClaimsIdentity(cliams, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe, 
                        ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(4) : DateTime.UtcNow.AddHours(1) 
                    };

                   
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);


                    return RedirectToAction("Index", "home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "❗Incorrect email or password");
                    return  View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","home");
        }


        [HttpGet]
        public JsonResult CheckEmail(string Email)
        {
            var user = _unitOfWork.UserRepository.Find(u => u.Email == Email);
            if (user != null)
            {
                return Json($"Email {Email} is already in use."); 
            }
            return Json(true);  
        }


    }
}
