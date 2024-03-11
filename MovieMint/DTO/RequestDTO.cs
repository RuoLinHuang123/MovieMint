using MovieMint.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieMint.DTO
{
    public class RequestDTO<T> : IValidatableObject
    {
        [DefaultValue(0)]
        public int PageIndex { get; set; } = 0;

        [DefaultValue(10)]
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        [DefaultValue("Id")]
        public string? SortColumn { get; set; }

        [RegularExpression("ASC|DESC")]
        [DefaultValue("ASC")]
        public string? SortOrder { get; set; } = "ASC";

        [DefaultValue(null)]
        public string? FilterQuery { get; set; } = null;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            var validator = new SortColumnValidatorAttribute(typeof(T));
            var sortColumnValidationResult = validator.GetValidationResult(SortColumn, validationContext);
            if (sortColumnValidationResult != null)
            {
                errors.Add(sortColumnValidationResult);
            }

            if (PageIndex < 0)
            {
                errors.Add(new ValidationResult("PageIndex cannot be less than 0.", new[] { nameof(PageIndex) }));
            }

            return errors;
        }
    }
}
