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
            ViewBag.NoOfCustomers = _unitOfWork.UserRepository.Count();
            ViewBag.NoOfOrders = _unitOfWork.OrderRepo.Count();
            ViewBag.TotalSales = _unitOfWork.OrderRepo.Sum(o => o.TotalAmount);
            ViewBag.PendingOrders = _unitOfWork.OrderRepo.Count(o => o.OrderStatus == Enums.OrderStatus.Pending);
            return View();
        }

   
    }
}
