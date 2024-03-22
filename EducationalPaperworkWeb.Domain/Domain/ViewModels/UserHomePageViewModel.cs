namespace EducationalPaperworkWeb.Domain.Domain.ViewModels
{
    public class UserHomePageViewModel
    {
        public long Id { get; set; }
        public long SelectedChatId { get; set; }
        public List<UserChat> UserChats { get; set; }
    }
}
