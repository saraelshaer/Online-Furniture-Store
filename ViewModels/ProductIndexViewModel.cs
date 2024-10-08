using FurnitureStore.Models;

namespace FurnitureStore.ViewModels
{
    public class ProductIndexViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
