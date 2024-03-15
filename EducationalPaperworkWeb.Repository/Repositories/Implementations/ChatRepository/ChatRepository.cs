using EducationalPaperworkWeb.Domain.Domain.Models.Chat;
using EducationalPaperworkWeb.Domain.Domain.Models.User;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;
using EducationalPaperworkWeb.Repository.Repositories.Interfaces;

namespace EducationalPaperworkWeb.Repository.Repositories.Implementations.ChatRepository
{
    public class UserRepository : IBaseRepository<Chat>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext appDbContext)
        {
            _db = appDbContext;
        }

        public async Task Create(Chat chat)
        {
            try
            {
                await _db.Chats.AddAsync(chat);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Create)}: Помилка на етапі внесення чату в таблицю.\n{ex.Message}");
            }
        }

        public async Task Delete(Chat chat)
        {
            try
            {
                _db.Chats.Remove(chat);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Delete)}: Помилка на етапі видалення чату з таблиці.\n{ex.Message}");
            }
        }

        public IQueryable<Chat> GetAll()
        {
            return _db.Chats.AsQueryable();
        }

        public async Task<Chat> Update(Chat chat)
        {
            try
            {
                _db.Chats.Update(chat);
                await _db.SaveChangesAsync();

                return chat;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Update)}: Помилка на етапі оновлення чату в таблиці.\n{ex.Message}");
            }
        }
    }
}
