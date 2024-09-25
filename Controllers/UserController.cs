using FurnitureStore.IRepository;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace FurnitureStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var users =_unitOfWork.UserRepository.GetAll(ur=>ur.IsActive);
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            return View(user);
        }

        public IActionResult Edit()
        {
            var user = _unitOfWork.UserRepository.GetById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Country = user.Country,
                City = user.City,
                ZipCode = user.ZipCode,
                State = user.State,
                ImageFileName = TempData.Peek("ImageProfile")?.ToString()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = _unitOfWork.UserRepository.Find(u => u.Id == int.Parse( User.FindFirst(ClaimTypes.NameIdentifier).Value));
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Country = model.Country;
                user.City = model.City;
                user.ZipCode = model.ZipCode;
                user.State = model.State;

                
                if (model.ProfileImage != null) 
                {
                    string filename = string.Empty;
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "userImages");
                    filename= model.ProfileImage.FileName;
                    string fullPath = Path.Combine(uploadsFolder,filename);
                    model.ProfileImage.CopyTo(new FileStream(fullPath , FileMode.Create)); // save the image
                    user.ImageFileName = filename;
                    model.ImageFileName = filename;
                    TempData["ImageProfile"]=filename;
                }

               
                _unitOfWork.Save();
                TempData["Message"] = "Successfully updated.";
                return RedirectToAction("Edit");
            }
            return View(model);
        }

        public IActionResult ConfirmDelete(int id)
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            var entity= _unitOfWork.UserRepository.GetById(id);
            _unitOfWork.UserRepository.SoftDelete(entity);
            return RedirectToAction("Index");
        }


    }
}
