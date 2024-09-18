using FurnitureStore.Data;
using FurnitureStore.IRepository;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<T> GetAll() { return _dbSet.ToList(); }

        public T GetById(int id) { return _dbSet.Find(id); }

        public void Update(T entity) => _dbSet.Update(entity);
        public void HardDelete(T entity) => _dbSet.Remove(entity);

        public void SoftDelete(T entity)
        {
            var isActiveProp = entity.GetType().GetProperty("IsActive");
            isActiveProp.SetValue(entity, false);
            _dbSet.Update(entity);
        }

    }
}
