using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class WishListProduct
    {
        [ForeignKey("WishList")]
        public int WishListId { get; set; }
        public WishList WishList { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
