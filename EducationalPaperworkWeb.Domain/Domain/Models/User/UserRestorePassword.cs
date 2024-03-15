namespace EducationalPaperworkWeb.Domain.Domain.Models.User
{
    public class UserRestorePassword : UserSignIn
    {
        public string RepeatPassword { get; set; }
    }
}