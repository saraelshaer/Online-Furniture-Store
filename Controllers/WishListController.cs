using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureStore.Controllers
{
    public class WishListController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishListController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var user = _unitOfWork.UserRepository.GetById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
           
            if(user.WishList == null)
            {
                return View(new List<WishListProduct>());
            }
            var wishlistItems = user.WishList.WishListProducts?.ToList()?? new List<WishListProduct>();
            return View(wishlistItems);
        }

        [HttpPost]
      
        public IActionResult ToggleWishlist(int productId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var wishlist = _unitOfWork.WishListRepo.Find(wl => wl.UserId == userId);
            var response = new { isInWishlist = false };

            if (wishlist == null)
            {
                wishlist = new WishList
                {
                    UserId= userId,
                    WishListProducts = new List<WishListProduct>()
                };
                _unitOfWork.WishListRepo.Add(wishlist);
            }
            var wishlistItem = wishlist.WishListProducts.FirstOrDefault(wi => wi.ProductId == productId );
            var product = _unitOfWork.ProductRepository.Find(p => p.Id == productId);
            if (product != null)
            {
                if (wishlistItem == null)
                {
                    var item = new WishListProduct { ProductId = productId, WishListId = wishlist.Id };

                    wishlist.WishListProducts.Add(item);

                    response = new { isInWishlist = true };
                }
                else
                {
                    wishlist.WishListProducts.Remove(wishlistItem);
                     
                }
               
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
               
              
            }
            TempData["NoOfWishlist"] = wishlist?.WishListProducts?.Count() ?? 0;
            return Json(response);
        }
    }
}
