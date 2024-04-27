using System.ComponentModel.DataAnnotations;

namespace PRX.Utils
{
    public class DescriptionValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string description = value.ToString();
                string[] words = description.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length < 10)
                {
                    return new ValidationResult(ErrorMessage);
                }

                int maxLength = words.Max(word => word.Length);
                if (maxLength > 100) // Adjust the maximum long word length as needed
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
