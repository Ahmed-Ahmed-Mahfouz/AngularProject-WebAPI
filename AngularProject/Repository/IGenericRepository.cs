namespace AngularProject.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public List<TEntity> GetAll(string includeProperties);
        public (List<TEntity> Items, int TotalCount) GetAllPaged(int pageNumber, int pageSize, string includeProperties);
        public TEntity GetById(Func<TEntity, bool> func, string? include = null);
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(int id);
        public void Save();
    }
}