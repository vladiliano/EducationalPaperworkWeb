using System.ComponentModel.DataAnnotations;

namespace EducationalPaperworkWeb.Domain.Domain.Models.UserEntities
{
    public class UserSignIn
    {
        [Required(ErrorMessage = "Пусте поле!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некоректна адреса пошти!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(8, ErrorMessage = "Мінімальна довжина поля 8 символів!")]
        [MaxLength(50, ErrorMessage = "Максимальна довжина поля 50 символів!")]
        public string Password { get; set; }
    }
}
