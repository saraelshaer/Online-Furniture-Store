using FurnitureStore.Consts;
using FurnitureStore.IRepository;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace FurnitureStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.ProductRepository.GetAll(p => p.IsActive && p.StockQuantity > 0, null, p => p.CreatedAt, OrderByDirection.Descending, 4);
           
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            ViewBag.NoOfCustomers = _unitOfWork.UserRepository.Count(u=>u.UserRoles.Any(r=>r.Role.Name.ToLower() != "admin") && u.IsActive);
            ViewBag.NoOfOrders = _unitOfWork.OrderRepo.Count(o=>o.User.IsActive);
            ViewBag.TotalSales = _unitOfWork.OrderRepo.Sum(o => o.TotalAmount , o=> o.PaymentStatus == Enums.PaymentStatus.Completed);
            ViewBag.PendingOrders = _unitOfWork.OrderRepo.Count(o => o.OrderStatus == Enums.OrderStatus.Pending && o.User.IsActive);
            return View();
        }
    }
}
