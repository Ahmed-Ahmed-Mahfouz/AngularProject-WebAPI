using AngularProject.Models;
using AngularProject.Repository;

namespace AngularProject.UnitOfWork
{
    public interface IUnitOfWorks
    {
        public IGenericRepository<Product> ProductRepository { get; }
        public IGenericRepository<Category> CategoryRepository { get; }
        public void Save();
    }
}