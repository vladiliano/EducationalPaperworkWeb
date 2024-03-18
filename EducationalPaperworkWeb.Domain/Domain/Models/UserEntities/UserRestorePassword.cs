namespace EducationalPaperworkWeb.Domain.Domain.Models.UserEntities
{
    public class UserRestorePassword : UserSignIn
    {
        public string RepeatPassword { get; set; }
    }
}