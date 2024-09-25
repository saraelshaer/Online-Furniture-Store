using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace FurnitureStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.ProductRepository.GetAll().Include(p => p.Category).Where(p => p.IsActive == true);
            return View(products.ToList());
        }

        public IActionResult Details(int id)
        {
            var product = _unitOfWork.ProductRepository.GetAll().Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public IActionResult Edit(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound();  
            }
            ViewBag.Categories = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name");
            return View(product);
        }
        [HttpPost , ActionName("Edit")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name");
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var entity = _unitOfWork.ProductRepository.GetById(id);
            _unitOfWork.ProductRepository.SoftDelete(entity);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
