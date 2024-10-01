using FurnitureStore.Models;

namespace FurnitureStore.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        new Cart GetCartByUserId(int userId);
    }
}
