using FurnitureStore.Models;

namespace FurnitureStore.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Product> ProductRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Review> ReviewRepository { get; }
        IRepository<UserRole> UserRoleRepo { get; }
        IRepository<Cart> CartRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IRepository<WishList> WishListRepo{ get; }
        void Save();
    }
}
