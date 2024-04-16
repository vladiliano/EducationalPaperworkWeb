using EducationalPaperworkWeb.Domain.Domain.Enums.Chat;

namespace EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities
{
    public class Message
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public long ChatId { get; set; }
        public MessageContentType Type { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
