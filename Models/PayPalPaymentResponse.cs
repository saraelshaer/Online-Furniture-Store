namespace FurnitureStore.Models
{
    public class PayPalPaymentResponse
    {
        public List<Link> links { get; set; }

        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }
    }
}
