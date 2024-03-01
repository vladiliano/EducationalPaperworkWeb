using EducationalPaperworkWeb.Domain.Domain.Enums;

namespace EducationalPaperworkWeb.Domain.Domain.Models.User
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Faculty Faculty { get; set; }
        public string Group { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
    }
}
