using System.ComponentModel.DataAnnotations;

namespace EducationalPaperworkWeb.Domain.Domain.Models.User
{
    public class UserSignUp : User
    {
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(8, ErrorMessage = "Мінімальна довжина поля 8 символів!")]
        [MaxLength(50, ErrorMessage = "Максимальна довжина поля 50 символів!")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+])[A-Za-z\\d!@#$%^&*()_+]{4,}$",
        ErrorMessage = "Пароль повинен містити мінімум одну літеру в нижньому регістрі (a-z), " +
            "одну літеру у верхньому регістрі (A-Z), " +
            "одну цифру (0-9) та один із таких символів (!@#$%^&*()_+)")]
        public string RepeatPassword { get; set; }
    }
}
