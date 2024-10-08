using FurnitureStore.Enums;
using FurnitureStore.IRepository;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            var orders = _unitOfWork.OrderRepo.GetAll();
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _unitOfWork.OrderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            var orderProducts = new List<OrderProduct>();
            var products= order.User?.Cart?.CartProducts.Select(p=> new {p.Product , p.Quantity});
            foreach (var product in products) 
            {
                orderProducts.Add(new OrderProduct { ProductId= product.Product.Id ,Product=product.Product, Quantity= product.Quantity ,OrderId= id, Order=order});
            }
            order.TotalAmount= orderProducts.Sum(p=>p.Quantity*p.Product.Price);
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
                OrderProducts= orderProducts
            };

            ViewBag.OrderStatus = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            return View(model);
        }

        [HttpPost]
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
                order.OrderProducts = model.OrderProducts;
                _unitOfWork.OrderRepo.Update(order);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.OrderStatus = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            return View(model);
        }
    }
}
