namespace EducationalPaperworkWeb.Repository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task Create(T entity);

        IQueryable<T> GetAll();

        Task Delete(T entity);

        Task Update(T entity);
    }
}
