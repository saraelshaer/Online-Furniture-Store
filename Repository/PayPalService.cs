using FurnitureStore.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PayPal.Api;
using System.Net.Http.Headers;
using System.Text;

namespace FurnitureStore.Repository
{
    public class PayPalService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public PayPalService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        private async Task<string> GetAccessToken()
        {
            var client = _httpClientFactory.CreateClient("PayPal");

            var byteArray = Encoding.ASCII.GetBytes($"{_config["PayPal:ClientId"]}:{_config["PayPal:ClientSecret"]}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var requestBody = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync("v1/oauth2/token", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<PayPalAccessToken>(responseContent);
                return token.access_token;
            }

            throw new Exception("Failed to retrieve PayPal access token.");
        }

        public async Task<string> CreatePayment(decimal amount, string returnUrl, string cancelUrl)
        {
            var client = _httpClientFactory.CreateClient("PayPal");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var payment = new
            {
                intent = "sale",
                payer = new { payment_method = "paypal" },
                transactions = new[]
                {
                new
                {
                    amount = new { total = amount.ToString("F"), currency = "USD" },
                    description = "Transaction description."
                }
            },
                redirect_urls = new { return_url = returnUrl, cancel_url = cancelUrl }
            };

            var response = await client.PostAsJsonAsync("v1/payments/payment", payment);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var createdPayment = JsonConvert.DeserializeObject<PayPalPaymentResponse>(jsonResponse);
                var approvalUrl = createdPayment.links.FirstOrDefault(link => link.rel == "approval_url")?.href;
                return approvalUrl;
            }

            throw new Exception("Failed to create PayPal payment.");
        }
    }

}
