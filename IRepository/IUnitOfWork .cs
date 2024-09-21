using FurnitureStore.Models;

namespace FurnitureStore.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Product> ProductRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserRole> UserRoleRepo { get; }
        void Save();
    }
}
