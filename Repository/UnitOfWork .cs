using FurnitureStore.Data;
using FurnitureStore.IRepository;
using FurnitureStore.Models;

namespace FurnitureStore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ProductRepository = new Repository<Product>(_context);
            UserRepository = new Repository<User>(_context);
            UserRoleRepo= new Repository<UserRole>(_context);
            CategoryRepository = new Repository<Category>(_context);
            ReviewRepository = new Repository<Review>(_context);
            CartRepository = new Repository<Cart>(_context);

            RoleRepository= new Repository<Role>(_context);
            WishListRepo = new Repository<WishList>(_context);
            OrderRepo = new Repository<Order>(_context);
        }

        public IRepository<Product> ProductRepository { get; }

        public IRepository<User> UserRepository { get; }

        public IRepository<Category> CategoryRepository { get; }

        public IRepository<Review> ReviewRepository { get; }


        public IRepository<UserRole> UserRoleRepo { get; }
        public IRepository<Role> RoleRepository { get; }
        public IRepository<WishList> WishListRepo { get; }

        public IRepository<Cart> CartRepository { get; private set; }

        public IRepository<Order> OrderRepo { get; }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
    