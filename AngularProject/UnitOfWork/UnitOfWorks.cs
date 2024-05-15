using AngularProject.Models;
using AngularProject.Repository;

namespace AngularProject.UnitOfWork
{
    public class UnitOfWorks : IUnitOfWorks
    {
        ApplicationDbContext db;
        IGenericRepository<Product> repository;
        IGenericRepository<Category> categoryRepository;
        public UnitOfWorks(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                if (repository == null)
                {
                    repository = new GenericRepository<Product>(db);
                }
                return repository;
            }
        }
        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new GenericRepository<Category>(db);
                }
                return categoryRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
