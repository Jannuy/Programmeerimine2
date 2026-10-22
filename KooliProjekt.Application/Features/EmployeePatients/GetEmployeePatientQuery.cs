using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EmployeePatients
{
    [ExcludeFromCodeCoverage]
    public class GetEmployeePatientQuery : IRequest<OperationResult<EmployeePatientDto>>
    {
        public int Id { get; set; }
    }
}
