using EducationalPaperworkWeb.Domain.Domain.Enums.UserAccount;
using System.ComponentModel.DataAnnotations;

namespace EducationalPaperworkWeb.Domain.Domain.Helpers.CustomAtributes
{
    public class FacultyNotDefaultAttribute : ValidationAttribute
    {
        public FacultyNotDefaultAttribute() : base()
        {
        }

        public override bool IsValid(object value)
        {
            if (value is Faculty faculty)
            {
                return faculty != Faculty.обрати;
            }
            return true;
        }
    }
}
