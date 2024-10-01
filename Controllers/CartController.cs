using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using FurnitureStore.Repository;


public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork; // Assuming you're using UnitOfWork
    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //// View the cart for the current user
    //public ActionResult Index()
    //{
    //    var userId = GetUserId(); // Fetch the logged-in user ID
    //    var cart = _unitOfWork.CartRepository.GetCartByUserId(userId);

    //    if (cart == null || !_unitOfWork.CartProducts.Any())
    //    {
    //        return View(new List<CartProduct>());
    //    }

    //    return View(cart.CartProducts.ToList());
    //}

    ////Add product to cart
    //public ActionResult AddToCart(int productId, int quantity)
    //{
    //    var userId = GetUserId(); // Fetch the logged-in user ID
    //    var cart = _unitOfWork.CartRepository.GetCartByUserId(userId) ?? CreateNewCart(userId);

    //    var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
    //    if (cartProduct == null)
    //    {
    //        cartProduct = new CartProduct
    //        {
    //            CartId = cart.Id,
    //            ProductId = productId,
    //            Quantity = quantity
    //        };
    //        cart.CartProducts.Add(cartProduct);
    //    }
    //    else
    //    {
    //        cartProduct.Quantity += quantity;
    //    }

    //    _unitOfWork.CartRepository.Update(cart);
    //    _unitOfWork.Save();

    //    return RedirectToAction("Index");
    //}

    //// Remove product from cart
    //public ActionResult RemoveFromCart(int productId)
    //{
    //    var userId = GetUserId(); // Fetch the logged-in user ID
    //    var cart = _unitOfWork.CartRepository.GetCartByUserId(userId);

    //    if (cart != null)
    //    {
    //        var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
    //        if (cartProduct != null)
    //        {
    //            cart.CartProducts.Remove(cartProduct);
    //            _unitOfWork.CartRepository.Update(cart);
    //            _unitOfWork.Save();
    //        }
    //    }

    //    return RedirectToAction("Index");
    //}

    // Helper to get logged-in user ID
    private int GetUserId()
    {
        return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
    }


    // Helper to create a new cart
    private Cart CreateNewCart(int userId)
    {
        var cart = new Cart
        {
            UserId = userId,
            CartProducts = new List<CartProduct>()
        };

        _unitOfWork.CartRepository.Add(cart);
        _unitOfWork.Save();

        return cart;
    }
}

