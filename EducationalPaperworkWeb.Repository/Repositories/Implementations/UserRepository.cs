using EducationalPaperworkWeb.Domain.Domain.Models.User;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;
using EducationalPaperworkWeb.Repository.Repositories.Interfaces;

namespace EducationalPaperworkWeb.Repository.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext appDbContext)
        {
            _db = appDbContext;
        }

        public async Task Create(User user)
        {
            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Create)}: Помилка на етапі внесення користувача в таблицю.\n{ex.Message}");
            }
        }

        public async Task Delete(User user)
        {
            try
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Delete)}: Помилка на етапі видалення користувача з таблиці.\n{ex.Message}");
            }
        }

        public async Task<User> Update(User user)
        {
            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Update)}: Помилка на етапі оновлення користувача в таблиці.\n{ex.Message}");
            }
        }

        public IQueryable<User> GetAll()
        {
            return _db.Users.AsQueryable();
        }
    }
}
