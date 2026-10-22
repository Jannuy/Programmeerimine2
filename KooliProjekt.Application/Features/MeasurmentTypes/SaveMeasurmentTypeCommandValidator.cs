using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    // Valideerimise klass SaveMeasurmentTypeCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveMeasurmentTypeCommandValidator : AbstractValidator<SaveMeasurmentTypeCommand>
    {
        public SaveMeasurmentTypeCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Code)
                .GreaterThan(0).WithMessage("Code must be greater than 0");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters");
        }
    }
}
