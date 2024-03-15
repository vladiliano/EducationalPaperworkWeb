using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Repository.Repository.Intarfaces.GenericRepository;

namespace EducationalPaperworkWeb.Repository.Repository.Intarfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Chat> ChatRepository { get; }
        IGenericRepository<Message> MessageRepository { get; }
        void Save();
    }
}
