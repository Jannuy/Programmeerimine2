using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.EmployeePatients
{
    // Valideerimise klass SaveEmployeePatientCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveEmployeePatientCommandValidator : AbstractValidator<SaveEmployeePatientCommand>
    {
        public SaveEmployeePatientCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .MaximumLength(20).WithMessage("Gender cannot exceed 20 characters");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("Notes is required")
                .MaximumLength(100).WithMessage("Notes cannot exceed 100 characters");
        }
    }
}
