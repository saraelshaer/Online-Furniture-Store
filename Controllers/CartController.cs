using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using FurnitureStore.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace FurnitureStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var user = _unitOfWork.UserRepository.GetById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

            if (user.Cart == null)
            {
                return View(new List<CartProduct>());
            }

            var cartItems = user.Cart.CartProducts?.ToList() ?? new List<CartProduct>();
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _unitOfWork.CartRepository.Find(c => c.UserId == userId);
            var response = new { isInCart = false, cartCount = 0 };

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartProducts = new List<CartProduct>()
                };
                _unitOfWork.CartRepository.Add(cart);
            }

            var cartItem = cart.CartProducts.FirstOrDefault(ci => ci.ProductId == productId);
            var product = _unitOfWork.ProductRepository.Find(p => p.Id == productId);
            if (product != null)
            {
                if (cartItem == null)
                {
                    var item = new CartProduct
                    {
                        ProductId = productId,
                        CartId = cart.Id,
                        Quantity = quantity
                    };

                    cart.CartProducts.Add(item);
                    product.StockQuantity -= quantity;  

                    response = new { isInCart = true, cartCount = cart.CartProducts.Sum(ci => ci.Quantity) };
                }
                else
                {
                    cartItem.Quantity += quantity;
                    product.StockQuantity -= quantity;
                }

                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                TempData["NoOfItemsInCart"] = cart.CartProducts?.Sum(ci => ci.Quantity) ?? 0;

                
                response = new { isInCart = true, cartCount = cart.CartProducts.Sum(ci => ci.Quantity) };
            }

            return Json(response);
        }





        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _unitOfWork.CartRepository.Find(c => c.UserId == userId);
            var response = new { success = false, cartCount = 0 };

            if (cart != null)
            {
                var cartItem = cart.CartProducts.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    var product = _unitOfWork.ProductRepository.Find(p => p.Id == productId);
                    product.StockQuantity += cartItem.Quantity; 
                    cart.CartProducts.Remove(cartItem);

                    _unitOfWork.ProductRepository.Update(product);
                    _unitOfWork.Save();

                    response = new { success = true, cartCount = cart.CartProducts.Sum(ci => ci.Quantity) };
                }
            }

            return Json(response);
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _unitOfWork.CartRepository.Find(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartProducts.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    var product = _unitOfWork.ProductRepository.Find(p => p.Id == productId);
                    if (product != null && quantity > 0 && quantity <= product.StockQuantity)
                    {
                        product.StockQuantity += cartItem.Quantity - quantity;  
                        cartItem.Quantity = quantity; 

                        _unitOfWork.ProductRepository.Update(product);
                        _unitOfWork.Save();

                        var cartCount = cart.CartProducts.Sum(ci => ci.Quantity);
                        return Json(new { success = true, cartCount = cartCount });
                    }
                }
            }

            return Json(new { success = false });
        }


    }
}


