using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Repository.GenericRepository;

namespace EducationalPaperworkWeb.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Chat> ChatRepository { get; }
        IGenericRepository<Message> MessageRepository { get; }
        void Save();
    }
}
