namespace EducationalPaperworkWeb.Domain.Domain.Models.Chat
{
    public class Message
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public long ChatId { get; set; }
        public object Content { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
