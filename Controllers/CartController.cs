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
            var response = new { isInCart = false };

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

            if (product.StockQuantity < quantity)
            {
                return BadRequest("Not enough stock available");
            }

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero");
            }

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
                   
                    

                }
                else
                {
                    cartItem.Quantity += quantity;
                  
                }
                
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();

               

                response = new { isInCart = true };
            }
            var size = Convert.ToInt32(TempData.Peek("NoOfCart")) + quantity;
            TempData["NoOfCart"] = size;
            return Json(response);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _unitOfWork.CartRepository.Find(c => c.UserId == userId);
            var response = new { success = false};

            if (cart != null)
            {
                var cartItem = cart.CartProducts.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    var product = _unitOfWork.ProductRepository.Find(p => p.Id == productId);
                 
                    cart.CartProducts.Remove(cartItem);

                    _unitOfWork.ProductRepository.Update(product);
                    _unitOfWork.Save();

                    var size = Convert.ToInt32(TempData.Peek("NoOfCart")) - cartItem.Quantity;
                    TempData["NoOfCart"] = size;
                    response = new { success = true};
                   
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
                        var size = 0;
                        if (cartItem.Quantity >= quantity)
                        {
                             size = Convert.ToInt32(TempData.Peek("NoOfCart")) - (cartItem.Quantity - quantity);
                        }
                        else 
                        {
                            size = Convert.ToInt32(TempData.Peek("NoOfCart")) +(quantity - cartItem.Quantity );
                         
                        }

                        TempData["NoOfCart"] = size;
                        cartItem.Quantity = quantity; 

                        _unitOfWork.ProductRepository.Update(product);
                        _unitOfWork.Save();

                     
                        return Json(new { success = true });
                    }
                }
            }

            return Json(new { success = false });
        }


    }
}


