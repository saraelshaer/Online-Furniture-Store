using FurnitureStore.Enums;
using FurnitureStore.IRepository;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var orders = _unitOfWork.OrderRepo.GetAll(o=>o.User.IsActive);
            return View(orders);
        }

        public IActionResult UserIndex()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orders = _unitOfWork.OrderRepo.GetAll(o=> o.UserId == userId);
            return View(orders);
        }
        public IActionResult Details(int id)
        {
            var order = _unitOfWork.OrderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            var model = new UserOrderViewModel
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                Country = order.Country,
                State = order.State,
                City = order.City,
                ZipCode = order.ZipCode,
                FirstName = order.User.FirstName,
                LastName = order.User.LastName,
                Email = order.User.Email,
                Phone = order.User.Phone,
                OrderProducts= order.OrderProducts
            };
            ViewBag.Products = model.OrderProducts;
            ViewBag.TotalPrice = model.TotalAmount;
            ViewBag.OrderStatus = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(UserOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _unitOfWork.OrderRepo.GetById(model.Id);
                order.TotalAmount = model.TotalAmount;
                order.OrderStatus = model.OrderStatus;
                order.OrderDate = model.OrderDate;
                order.Country = model.Country;
                order.State = model.State;
                order.City = model.City;
                order.ZipCode = model.ZipCode;
                order.User.FirstName= model.FirstName;
                order.User.LastName= model.LastName;
                order.User.Email = model.Email;
                order.User.Phone = model.Phone;
                _unitOfWork.OrderRepo.Update(order);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.OrderStatus = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            return View(model);
        }

        public IActionResult Checkout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _unitOfWork.CartRepository.Find(c => c.UserId == userId);
            var products = cart.CartProducts.ToList();
            decimal totalPrice = products.Sum(p=> p.Quantity * p.Product.Price);
            ViewBag.Products= products;
            ViewBag.TotalPrice = totalPrice;    
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order order)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = _unitOfWork.CartRepository.Find(c => c.UserId == userId);
            var products = cart.CartProducts.ToList();
            ViewBag.Products = products;
            ViewBag.TotalPrice = order.TotalAmount;

            if (ModelState.IsValid)
            {
                order.Cart = cart;
                order.UserId = userId;
                _unitOfWork.OrderRepo.Add(order);
                _unitOfWork.Save();

                foreach (var product in products)
                {
                    order.OrderProducts.Add(new OrderProduct { ProductId = product.Product.Id,  Quantity = product.Quantity, OrderId = order.Id});
                }
                _unitOfWork.Save();
                return RedirectToAction("Index","Checkout", new {id = order.Id});
            }
            return View(order);
        }

       
        public IActionResult View(int id)
        {
            var order =_unitOfWork.OrderRepo.GetById(id);
            ViewBag.Products = order.OrderProducts.ToList();
            ViewBag.TotalPrice = order.TotalAmount;
            return View(order);
        }

        public IActionResult Delete(int id)
        {
            var order = _unitOfWork.OrderRepo.GetById(id);
            order.OrderProducts.Clear();
            _unitOfWork.OrderRepo.HardDelete(order);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}
