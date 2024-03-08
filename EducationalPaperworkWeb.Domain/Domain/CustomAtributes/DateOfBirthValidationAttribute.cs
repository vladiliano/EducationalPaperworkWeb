using System.ComponentModel.DataAnnotations;

namespace EducationalPaperworkWeb.Domain.Domain.CustomAtributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateOfBirthValidationAttribute : ValidationAttribute
    {
        private readonly int _minYear;

        public DateOfBirthValidationAttribute(int minYear)
        {
            _minYear = minYear;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, _minYear);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                if (dateOfBirth >= DateTime.MinValue && dateOfBirth.Year < _minYear)
                    return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
