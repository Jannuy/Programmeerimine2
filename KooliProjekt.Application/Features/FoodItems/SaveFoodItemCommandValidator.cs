using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.FoodItems
{
    // Valideerimise klass SaveFoodItemCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveFoodItemCommandValidator : AbstractValidator<SaveFoodItemCommand>
    {
        public SaveFoodItemCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters");

            RuleFor(x => x.ServingSize)
                .GreaterThan(0).WithMessage("Serving size must be greater than 0");

            RuleFor(x => x.ServingUnit)
                .GreaterThanOrEqualTo(0).WithMessage("Serving unit cannot be negative");

            RuleFor(x => x.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G)
                .NotEmpty().WithMessage("Energy kcal / sugars g / carbs g is required")
                .MaximumLength(100).WithMessage("Energy kcal / sugars g / carbs g cannot exceed 100 characters");
        }
    }
}
