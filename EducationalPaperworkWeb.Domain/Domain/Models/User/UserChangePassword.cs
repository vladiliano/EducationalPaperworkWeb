namespace EducationalPaperworkWeb.Domain.Domain.Models.User
{
    public class UserChangePassword : UserLogIn
    {
        public string RepeatPassword { get; set; }
    }
}