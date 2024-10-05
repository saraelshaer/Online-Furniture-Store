using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.Controllers
{
    [Authorize(Roles="Admin")]
    public class RoleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var roles = _unitOfWork.RoleRepository.GetAll();
            return View(roles);
        }
     

        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.RoleRepository.Exists(r=>r.Name.ToLower().Trim() == role.Name.ToLower().Trim()))
                {
                    ModelState.AddModelError("Name", "❗Role is already exists!");
                    return View("Index", _unitOfWork.RoleRepository.GetAll());
                }
                _unitOfWork.RoleRepository.Add(role);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View("Index", _unitOfWork.RoleRepository.GetAll());
        }

        public IActionResult Delete(int id)
        {
            var entity = _unitOfWork.RoleRepository.GetById(id);
            _unitOfWork.RoleRepository.HardDelete(entity);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
