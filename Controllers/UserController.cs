using FurnitureStore.IRepository;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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

        public IActionResult Edit(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user == null) 
            {
                return NotFound();
            }
            var model = new ProfileViewModel
            {
                Id= user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Country = user.Country,
                City = user.City,
                ZipCode = user.ZipCode,
                State = user.State,
                ImageFileName = user.ImageFileName,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = _unitOfWork.UserRepository.Find(u => u.Id == model.Id );
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Phone = model.Phone;
                user.Country = model.Country;
                user.City = model.City;
                user.ZipCode = model.ZipCode;
                user.State = model.State;

                var userWithSameEmail = _unitOfWork.UserRepository.Find(u => u.Email == model.Email);
                if(userWithSameEmail != null && userWithSameEmail.Id != model.Id)
                {
                    ModelState.AddModelError("Email", "This email is already assigned to another user.");
                    return View(model);
                }
                else
                {
                    user.Email = model.Email;
                }

                
                if (model.ProfileImage != null) 
                {
                    string filename = string.Empty;
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "userImages");
                    filename= model.ProfileImage.FileName;
                    string fullPath = Path.Combine(uploadsFolder,filename);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        // Copy the uploaded image to the server
                        model.ProfileImage.CopyTo(fileStream);
                    }

                    user.ImageFileName = filename;
                    model.ImageFileName = filename;

                    if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) == model.Id)
                        TempData["ImageProfile"]=filename;
                }

               
                _unitOfWork.Save();
                TempData["Message"] = "Successfully updated.";
                return RedirectToAction("Edit");
            }
            return View(model);
        }

        public IActionResult DeleteImage(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            string imagePath = $"userImages/{user.ImageFileName}";
            if (!string.IsNullOrEmpty(imagePath))
            {
                // Get the full physical path of the image file
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);

                // Check if the file exists
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    user.ImageFileName = "defaultUserImage.jpg";
                    _unitOfWork.Save();
                    if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) == user.Id)
                        TempData["ImageProfile"] = "defaultUserImage.jpg";

                    return RedirectToAction("Edit", new { user.Id });
                }
                
            }
            return NotFound();
        
        }
        
        public IActionResult Delete(int id)
        {
            var entity= _unitOfWork.UserRepository.GetById(id);
            _unitOfWork.UserRepository.SoftDelete(entity);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult ManageRoles(int id)
        {
            var entity = _unitOfWork.UserRepository.GetById(id);
            ViewBag.Roles =_unitOfWork.RoleRepository.GetAll();
            return View(entity);
        }

        [HttpPost]
        public IActionResult ManageRoles(UserRole userRole)
        {
             _unitOfWork.UserRoleRepo.Update(userRole);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }




    }
}
