using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.FoodEntries
{
    // Valideerimise klass SaveFoodEntryCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveFoodEntryCommandValidator : AbstractValidator<SaveFoodEntryCommand>
    {
        public SaveFoodEntryCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.FoodEntryItem)
                .NotEmpty().WithMessage("Food entry item is required")
                .MaximumLength(100).WithMessage("Food entry item cannot exceed 100 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Created at is required");
        }
    }
}
