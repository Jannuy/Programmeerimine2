using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Employees
{
    [ExcludeFromCodeCoverage]
    public class GetEmployeeQuery : IRequest<OperationResult<EmployeeDto>>
    {
        public int Id { get; set; }
    }
}
