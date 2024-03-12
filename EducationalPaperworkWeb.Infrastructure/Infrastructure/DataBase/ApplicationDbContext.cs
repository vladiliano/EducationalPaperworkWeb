using EducationalPaperworkWeb.Domain.Domain.Models.Chat;
using EducationalPaperworkWeb.Domain.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
