using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class WishListProduct
    {
        public int Id { get; set; }
        [ForeignKey("WishList")]
        public int WishListId { get; set; }
        public virtual WishList WishList { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
