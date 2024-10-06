using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Hosting;


namespace FurnitureStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminIndex()
        {
            var products = _unitOfWork.ProductRepository.GetAll().Include(p => p.Category).Where(p => p.IsActive == true);
            return View(products.ToList());
        }
        public IActionResult UserIndex()
        {
            var products = _unitOfWork.ProductRepository.GetAll(p => p.IsActive, new[] { "Category" });
            return View(products);
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
                string fileName = string.Empty;
                if (product.ProductImage != null)
                {
                    string productImagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "productImages");

                    if (!Directory.Exists(productImagesPath))
                    {
                        Directory.CreateDirectory(productImagesPath);
                    }

                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ProductImage.FileName);
                    string fullPath = Path.Combine(productImagesPath, fileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        product.ProductImage.CopyTo(fileStream);
                    }
                }
                product.ImageFileName = fileName;
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Save();
                return RedirectToAction("AdminIndex");
            }

            ViewBag.Categories = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name");
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
        [HttpPost, ActionName("Edit")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                if (product.ProductImage != null)
                {
                    string productImages = Path.Combine(_webHostEnvironment.WebRootPath, "productImages");
                    fileName = product.ProductImage.FileName;
                    string fullPath = Path.Combine(productImages, fileName);
                    product.ProductImage.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                product.ImageFileName = fileName;
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                return RedirectToAction("AdminIndex");
            }
            ViewBag.Categories = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name");
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var entity = _unitOfWork.ProductRepository.GetById(id);
            _unitOfWork.ProductRepository.SoftDelete(entity);
            _unitOfWork.Save();
            return RedirectToAction("AdminIndex");
        }
    }
}
