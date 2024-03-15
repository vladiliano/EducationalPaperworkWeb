using EducationalPaperworkWeb.Domain.Domain.Models.Chat;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;
using EducationalPaperworkWeb.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPaperworkWeb.Repository.Repositories.Implementations.ChatRepository
{
    public class MessageRepository : IBaseRepository<Message>
    {
        private readonly ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext appDbContext)
        {
            _db = appDbContext;
        }

        public async Task Create(Message message)
        {
            try
            {
                await _db.Messages.AddAsync(message);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Create)}: Помилка на етапі внесення повідомлення в таблицю.\n{ex.Message}");
            }
        }

        public async Task Delete(Message message)
        {
            try
            {
                _db.Messages.Remove(message);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Delete)}: Помилка на етапі видалення повідомлення з таблиці.\n{ex.Message}");
            }
        }

        public IQueryable<Message> GetAll()
        {
            return _db.Messages.AsQueryable();
        }

        public async Task<Message> Update(Message message)
        {
            try
            {
                _db.Messages.Update(message);
                await _db.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Update)}: Помилка на етапі оновлення повідомлення в таблиці.\n{ex.Message}");
            }
        }
    }
}
