using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class WishList
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<WishListProduct> WishListProducts { get; set; }

    }
}
