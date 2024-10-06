using FurnitureStore.Models;

namespace FurnitureStore.ViewModels
{
    public class UserProductViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public User User { get; set; }
    }
}
