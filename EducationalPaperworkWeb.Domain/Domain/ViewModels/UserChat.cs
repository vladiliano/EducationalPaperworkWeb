using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;

namespace EducationalPaperworkWeb.Domain.Domain.ViewModels
{
    public class UserChat
    {
        public Chat Chat { get; set; }
        public List<Message> Messages { get; set; }
    }
}
