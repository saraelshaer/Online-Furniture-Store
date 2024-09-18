using FurnitureStore.Models;

namespace FurnitureStore.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Product> ProductRepository { get; }
        void Save();
    }
}
