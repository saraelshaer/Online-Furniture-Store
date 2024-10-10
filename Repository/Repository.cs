using FurnitureStore.Consts;
using FurnitureStore.Data;
using FurnitureStore.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FurnitureStore.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity) => _dbSet.Add(entity);

        public IQueryable<T> GetAll(
            Expression<Func<T, bool>> criteria = null,
            string[] includes = null ,
            Expression<Func<T, object>> OrderBy=null ,
            OrderByDirection OrderByDirection = OrderByDirection.Ascending,
            int? take=null) 
        {
            IQueryable<T> query = _dbSet.AsQueryable(); ;
            if (criteria != null) 
            {
                query = query.Where(criteria);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if(OrderBy != null)
            {
                if (OrderByDirection == OrderByDirection.Ascending) { query = query.OrderBy(OrderBy); }
                else
                {
                    query = query.OrderByDescending(OrderBy);
                }
            }
            if (take != null)
            {
                query = query.Take(take.Value);
            }
            return query;
        }

        public T GetById(int? id) { return _dbSet.Find(id); }

        public void Update(T entity) => _dbSet.Update(entity);
        public void HardDelete(T entity) => _dbSet.Remove(entity);

        public void SoftDelete(T entity)
        {
            var isActiveProp = entity.GetType().GetProperty("IsActive");
            isActiveProp.SetValue(entity, false);
            _dbSet.Update(entity);
        }

        public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _dbSet;
            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query=query.Include(include);
                }
            }
            return query.SingleOrDefault(criteria);
        }

        public IQueryable<U> FindAll<U>( Expression<Func<T, bool>> criteria, Expression<Func<T,U>> expression, string[] includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(criteria).Select(expression);
        }

        public object GetCartByUserId(int userId)
        {
            return _context.Carts
           .Include(c => c.CartProducts)
           .FirstOrDefault(c => c.UserId == userId);
        }

        public bool Exists(Expression<Func<T, bool>> criteria) => _dbSet.Any(criteria);

        public int Count(Expression<Func<T, bool>> criteria = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if (criteria != null)
            {
                query = query.Where(criteria);
            }
            return query.Count();
        }


        public decimal Sum(Expression<Func<T, decimal>> criteria) => _dbSet.Sum(criteria);


    }
}
