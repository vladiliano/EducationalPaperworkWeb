using EducationalPaperworkWeb.Domain.Domain.Enums.UserAccount;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;

namespace EducationalPaperworkWeb.Domain.Domain.ViewModels
{
    public class UserViewModel
    {
        public long UserId { get; set; }
        public Role UserRole { get; set; }
        public List<Chat> Chats { get; set; }
    }
}
