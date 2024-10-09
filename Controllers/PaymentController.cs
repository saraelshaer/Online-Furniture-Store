using FurnitureStore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PayPalService _payPalService;

        public PaymentController(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        public async Task<IActionResult> CreatePayment(decimal amount)
        {
            var returnUrl = Url.Action("PaymentSuccess", "Payment", null, Request.Scheme);
            var cancelUrl = Url.Action("PaymentCancelled", "Payment", null, Request.Scheme);
            var approvalUrl = await _payPalService.CreatePayment(amount, returnUrl, cancelUrl);

            return Redirect(approvalUrl);
        }

        public IActionResult PaymentSuccess()
        {
            // Logic after successful payment
            return View();
        }

        public IActionResult PaymentCancelled()
        {
            // Logic after cancelled payment
            return View();
        }
    }
}
