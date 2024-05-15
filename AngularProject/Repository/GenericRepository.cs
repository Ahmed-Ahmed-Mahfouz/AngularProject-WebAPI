using AngularProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularProject.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        ApplicationDbContext db;
        public GenericRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<TEntity> GetAll(string includeProperties = "")
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }


        public TEntity GetById(Func<TEntity, bool> func, string? include = null)
        {
            if (include != null)
                return db.Set<TEntity>().Include(include).FirstOrDefault(func);

            return db.Set<TEntity>().FirstOrDefault(func);
        }

        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            TEntity entity = db.Set<TEntity>().Find(id);
            db.Set<TEntity>().Remove(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public (List<TEntity> Items, int TotalCount) GetAllPaged(int pageNumber, int pageSize, string includeProperties = "")
        {
            var totalCount = db.Set<TEntity>().Count();
            var items = db.Set<TEntity>()
                          .Include("Category")
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();
            return (items, totalCount);
        }


    }
}
