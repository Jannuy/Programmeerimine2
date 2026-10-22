using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.MealTypes
{
    // Valideerimise klass SaveMealTypeCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveMealTypeCommandValidator : AbstractValidator<SaveMealTypeCommand>
    {
        public SaveMealTypeCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.MealTypeId)
                .GreaterThan(0).WithMessage("Meal type ID must be greater than 0");
        }
    }
}
