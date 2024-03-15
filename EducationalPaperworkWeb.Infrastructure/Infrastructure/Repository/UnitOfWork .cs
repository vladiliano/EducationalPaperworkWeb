using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.GenericRepository;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;

namespace EducationalPaperworkWeb.Infrastructure.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<Chat> _chatRepository;
        private IGenericRepository<Message> _messageRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<User>(_context);
                }
                return _userRepository;
            }
        }

        public IGenericRepository<Chat> ChatRepository
        {
            get
            {
                if (_chatRepository == null)
                {
                    _chatRepository = new GenericRepository<Chat>(_context);
                }
                return _chatRepository;
            }
        }

        public IGenericRepository<Message> MessageRepository
        {
            get
            {
                if (_messageRepository == null)
                {
                    _messageRepository = new GenericRepository<Message>(_context);
                }
                return _messageRepository;
            }
        }

        public void Save() => _context.SaveChanges();

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _context != null) _context.Dispose();
            }
            _disposed = true;
        }
    }
}
