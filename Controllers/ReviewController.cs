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

        public IActionResult Create(int productId)
        {
            var review = new Review { ProductId = productId };
            return View(review);
        }

        [HttpPost]
        public IActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ReviewRepository.Add(review);
                _unitOfWork.Save();
                return RedirectToAction("Details", "Product"/*new { id = review.ProductId }*/);
            }
            return View(review);
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(review);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Review review)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.ReviewRepository.Update(review);
        //        await _unitOfWork.CompleteAsync();
        //        return RedirectToAction("Details", "Product", new { id = review.ProductId });
        //    }
        //    return View(review);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    _unitOfWork.ReviewRepository.Remove(review);
        //    await _unitOfWork.CompleteAsync();
        //    return RedirectToAction("Details", "Product", new { id = review.ProductId });
        //}
    }
}
