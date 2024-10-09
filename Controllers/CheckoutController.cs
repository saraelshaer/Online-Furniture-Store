using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;
using FurnitureStore.IRepository;
using FurnitureStore.Enums;

namespace FurnitureStore.Controllers
{
    public class CheckoutController : Controller
    {
        private string PayPalClientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        private string PayPalUrl { get; set; } = "";

        private readonly IUnitOfWork _unitOfWork;
        public CheckoutController(IConfiguration configuration , IUnitOfWork unitOfWork)
        {
            PayPalClientId = configuration["PayPal:ClientId"];
            PayPalSecret = configuration["PayPal:ClientSecret"];
            PayPalUrl = configuration["PayPal:Url"];
            _unitOfWork = unitOfWork;
        }

       
        private async Task<string> GetPayPalAccessToken()
        {
            string accessToken = "";

            string url = PayPalUrl + "/v1/oauth2/token";

            using (HttpClient client = new HttpClient()) 
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PayPalClientId + ":" + PayPalSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic "+ credentials64);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials",null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();

                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null) 
                    {
                        accessToken = jsonResponse["access_token"]?.ToString()??"";
                    }
                
                }
            }
            return accessToken; 
        }


       
        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (totalAmount == null)
            {
                return new JsonResult(new { Id = "" });
            }


            // create the request body
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");
            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", totalAmount);
            JsonObject purchaseUnit1 = new JsonObject(); purchaseUnit1.Add("amount", amount);
            JsonArray purchaseUnits = new JsonArray(); purchaseUnits.Add(purchaseUnit1);
            createOrderRequest.Add("purchase_units", purchaseUnits);

            // get access token
            string accessToken = await GetPayPalAccessToken();
            // send request
            string url = PayPalUrl + "/v2/checkout/orders";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        return new JsonResult(new { Id = paypalOrderId });
                    }

                }
            }
            return new JsonResult(new { Id = "" });
        }




        [HttpPost]
        public async Task<IActionResult> CompleteOrder([FromBody] JsonObject data)
        {
            
            var orderId = data?["orderID"]?.ToString();
            if (orderId == null)
            {
                return new JsonResult("error");
            }
            // get access token
            string accessToken = await GetPayPalAccessToken();
            string url = PayPalUrl + "/v2/checkout/orders/" + orderId + "/capture";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");
                var httpResponse = await client.SendAsync(requestMessage);



                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        if (paypalOrderStatus == "COMPLETED")
                        {
                            //return  RedirectToAction("PaymentSuccess");
                            PaymentSuccess();
                            return new JsonResult("success");
                        }


                    }
                }
            }
            return new JsonResult("error");

        }

        public void PaymentSuccess()
        {
            int id = Convert.ToInt32( TempData.Peek("OrderId"));
            var order = _unitOfWork.OrderRepo.GetById(id);
            order.PaymentStatus = PaymentStatus.Completed;
            foreach (var item in order.OrderProducts) 
            {
                if (item.Product.StockQuantity >= item.Quantity)
                    item.Product.StockQuantity -= item.Quantity;
                else
                    item.Product.StockQuantity = 0;

            }
            _unitOfWork.Save();
           
        }


        public IActionResult Index(int id)
        {
            var order = _unitOfWork.OrderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.ClientId = PayPalClientId;
            ViewBag.TotalAmount = order.TotalAmount;
            TempData["OrderId"] = id;
            return View();
        }
    }
}
