using System.ComponentModel.DataAnnotations;

namespace EducationalPaperworkWeb.Domain.Domain.Models.User
{
    public class UserSignUp : User
    {
        [Required(ErrorMessage = "Пусте поле!")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають!")]
        public string RepeatPassword { get; set; }
    }
}
