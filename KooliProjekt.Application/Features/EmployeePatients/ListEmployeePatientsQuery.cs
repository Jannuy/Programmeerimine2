using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EmployeePatients
{
    [ExcludeFromCodeCoverage]
    public class ListEmployeePatientsQuery : IRequest<OperationResult<PagedResult<EmployeePatient>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string LastName { get; set; }
    }
}
