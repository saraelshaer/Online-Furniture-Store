using System.Linq.Expressions;

namespace FurnitureStore.IRepository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> criteria = null);
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void SoftDelete(T entity);
        void HardDelete(T entity);
        T Find(Expression<Func<T, bool>> criteria , string[] includes = null);
        IQueryable<U> FindAll<U>( Expression<Func<T, bool>> criteria, Expression<Func<T, U>> expression, string[] includes = null);
        object GetCartByUserId(int userId);
    }
}
