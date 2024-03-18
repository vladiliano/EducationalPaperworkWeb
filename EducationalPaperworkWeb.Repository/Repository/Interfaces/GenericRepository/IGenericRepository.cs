namespace EducationalPaperworkWeb.Repository.Repository.Interfaces.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task CreateAsync(T entity);

        IQueryable<T> GetAll();

        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);
    }
}
