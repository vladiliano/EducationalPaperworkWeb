using EducationalPaperworkWeb.Domain.Domain.Enums;
using EducationalPaperworkWeb.Domain.Domain.Helpers.CustomAtributes;
using System.ComponentModel.DataAnnotations;

namespace EducationalPaperworkWeb.Domain.Domain.Models.UserEntities
{
    public class User
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина поля 2 літери!")]
        [MaxLength(25, ErrorMessage = "Максимальна довжина поля 25 літер!")]
        [RegularExpression("^[а-яА-Яa-zA-Z-]+$", ErrorMessage = "Поле має містити тільки літери!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина поля 2 літери!")]
        [MaxLength(25, ErrorMessage = "Максимальна довжина поля 25 літер!")]
        [RegularExpression("^[а-яА-Яa-zA-Z-]+$", ErrorMessage = "Поле має містити тільки літери!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(2, ErrorMessage = "Мінімальна довжина поля 2 літери!")]
        [MaxLength(25, ErrorMessage = "Максимальна довжина поля 25 літер!")]
        [RegularExpression("^[а-яА-Яa-zA-Z-]+$", ErrorMessage = "Поле має містити тільки літери!")]
        public string Patronymic { get; set; }
        [Required(ErrorMessage = "Дата народження не обрана!")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [FacultyNotDefault(ErrorMessage = "Оберіть факультет!")]
        public Faculty Faculty { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [RegularExpression(@"^\d{3}-[а-яА-Яa-zA-Z-]$", ErrorMessage = "Поле має містити (3 цифри)-(літера)")]
        public string Group { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [RegularExpression(@"[A-Za-zа-яА-Я0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некоректна адреса пошти!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Довжина поля має містити 10 цифр!")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Поле має містити тільки цифри!")]
        public string Phone { get; set; }
        public Role Role { get; set; }
        [Required(ErrorMessage = "Пусте поле!")]
        [MinLength(8, ErrorMessage = "Мінімальна довжина поля 8 символів!")]
        [MaxLength(64, ErrorMessage = "Максимальна довжина поля 64 символи!")]
        [RegularExpression("^(?=.*[a-zа-я])(?=.*[A-ZА-Я])(?=.*\\d)(?=.*[!@#$%^&*()_+])[A-Za-zа-яА-Я\\d!@#$%^&*()_+]{4,}$",
        ErrorMessage = "Пароль повинен містити (a-z,а-я)(A-Z,А-Я)(0-9)(!@#$%^&*()_+)")]
        public string Password { get; set; }
    }
}