using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EmployeePatients
{
    // EmployeePatient kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteEmployeePatientCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
