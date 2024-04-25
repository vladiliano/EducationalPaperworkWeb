using EducationalPaperworkWeb.Domain.Domain.Enums.Chat;

namespace EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities
{
    public class Chat
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long StudentId { get; set; }
        public long AdminId { get; set; }
        public DateTime TimeStamp { get; set; }
        public ChatState State { get; set; }
    }
}
