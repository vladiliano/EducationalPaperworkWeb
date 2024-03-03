using EducationalPaperworkWeb.Domain.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EducationalPaperworkWeb.Domain.Domain.Models.User
{
    public class User
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина поля 2 літери!")]
        [MaxLength(25, ErrorMessage = "Максимальна довжина поля 25 літер!")]
        [RegularExpression("^[a-zA-Z-]+$", ErrorMessage = "Поле має містити тільки літери!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина поля 2 літери!")]
        [MaxLength(25, ErrorMessage = "Максимальна довжина поля 25 літер!")]
        [RegularExpression("^[a-zA-Z-]+$", ErrorMessage = "Поле має містити тільки літери!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина поля 2 літери!")]
        [MaxLength(25, ErrorMessage = "Максимальна довжина поля 25 літер!")]
        [RegularExpression("^[a-zA-Z-]+$", ErrorMessage = "Поле має містити тільки літери!")]
        public string Patronymic { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        public Faculty Faculty { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина поля 2 літери!")]
        [MaxLength(10, ErrorMessage = "Максимальна довжина поля 10 літер!")]
        [RegularExpression("^[a-zA-Z0-9-]+$", ErrorMessage = "Поле має містити тільки літери і цифри!")]
        public string Group { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некоректна адреса пошти!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Довжина поля має містити 10 цифр!")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Поле має містити тільки цифри!")]
        public string Phone { get; set; }
        public Role Role { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(8, ErrorMessage = "Мінімальна довжина поля 8 символів!")]
        [MaxLength(50, ErrorMessage = "Максимальна довжина поля 50 символів!")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+])[A-Za-z\\d!@#$%^&*()_+]{4,}$",
        ErrorMessage = "Пароль повинен містити мінімум одну літеру в нижньому регістрі (a-z), " +
            "одну літеру у верхньому регістрі (A-Z), " +
            "одну цифру (0-9) та один із таких символів (!@#$%^&*()_+)")]
        public string Password { get; set; }
    }
}
