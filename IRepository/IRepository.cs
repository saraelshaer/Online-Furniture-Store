using FurnitureStore.Consts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FurnitureStore.IRepository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> criteria = null, string[] includes = null, Expression<Func<T, object>> OrderBy = null, OrderByDirection OrderByDirection = OrderByDirection.Ascending, int? take = null);
        T GetById(int? id);
        void Add(T entity);
        void Update(T entity);
        void SoftDelete(T entity);
        void HardDelete(T entity);
        T Find(Expression<Func<T, bool>> criteria , string[] includes = null);
        IQueryable<U> FindAll<U>( Expression<Func<T, bool>> criteria, Expression<Func<T, U>> expression, string[] includes = null);
        object GetCartByUserId(int userId);
        bool Exists(Expression<Func<T, bool>> criteria);

        public int Count(Expression<Func<T, bool>> criteria = null);
        public decimal Sum(Expression<Func<T, decimal>> expression, Expression<Func<T, bool>> criteria = null);
    }
}
