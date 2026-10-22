using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Employees
{
    // Valideerimise klass SaveEmployeeCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveEmployeeCommandValidator : AbstractValidator<SaveEmployeeCommand>
    {
        public SaveEmployeeCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password hash is required")
                .MaximumLength(256).WithMessage("Password hash cannot exceed 256 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .EmailAddress().WithMessage("Email is not a valid e-mail address");
        }
    }
}
