using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.MeasurmentUps
{
    // Valideerimise klass SaveMeasurmentUpCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveMeasurmentUpCommandValidator : AbstractValidator<SaveMeasurmentUpCommand>
    {
        public SaveMeasurmentUpCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.MeasurmentTypeId)
                .GreaterThan(0).WithMessage("Measurment type ID must be greater than 0")
                // Oma loogikaga valideerimise reegel, mis kasutab DbContexti
                .Must(id => context.MeasurmentTypes.Any(item => item.Id == id)).WithMessage("Measurment type does not exist");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0")
                // Oma loogikaga valideerimise reegel, mis kasutab DbContexti
                .Must(id => context.Employees.Any(item => item.Id == id)).WithMessage("Employee does not exist");

            RuleFor(x => x.MeasuredAt)
                .NotEmpty().WithMessage("Measured at is required");

            RuleFor(x => x.Notes)
                .MaximumLength(200).WithMessage("Notes cannot exceed 200 characters");
        }
    }
}
