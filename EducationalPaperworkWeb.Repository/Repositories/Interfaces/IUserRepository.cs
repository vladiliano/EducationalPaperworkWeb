using EducationalPaperworkWeb.Domain.Domain.Models.User;

namespace EducationalPaperworkWeb.Repository.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task Create(User user);
        public IQueryable<User> GetAll();
        public Task Delete(User user);
        public Task<User> Update(User user);
    }
}
