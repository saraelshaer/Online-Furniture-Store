using FurnitureStore.Data;
using FurnitureStore.IRepository;
using FurnitureStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        Cart ICartRepository.GetCartByUserId(int userId)
        {
            return _context.Carts
            .Include(c => c.CartProducts)
            .FirstOrDefault(c => c.UserId == userId);
        }
    }
}
