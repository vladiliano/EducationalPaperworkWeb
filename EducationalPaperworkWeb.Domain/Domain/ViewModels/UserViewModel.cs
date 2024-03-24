namespace EducationalPaperworkWeb.Domain.Domain.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public long SelectedChatId { get; set; }
        public List<UserChat> UserChats { get; set; }
    }
}
