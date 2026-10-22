using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.DeodItems
{
    // Valideerimise klass SaveDeodItemCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveDeodItemCommandValidator : AbstractValidator<SaveDeodItemCommand>
    {
        public SaveDeodItemCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.FoodEntryItem)
                .NotEmpty().WithMessage("Food entry item is required")
                .MaximumLength(100).WithMessage("Food entry item cannot exceed 100 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Label)
                .NotEmpty().WithMessage("Label is required")
                .MaximumLength(50).WithMessage("Label cannot exceed 50 characters");
        }
    }
}
