using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FurnitureStore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int productId)
        {
            var product = _unitOfWork.ProductRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            var reviews = product.Reviews.Where(r => r.IsActive).ToList();
            ViewBag.Product = product;
            return View(reviews);
        }

        public IActionResult AddReview(int productId)
        {
            var review = new Review
            {
                ProductId = productId
            };
            return View(review);
        }

        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;

                _unitOfWork.ReviewRepository.Add(review);
                _unitOfWork.Save();

                return RedirectToAction("Index", new { productId = review.ProductId });
            }
            return View(review);
        }

    }
}
