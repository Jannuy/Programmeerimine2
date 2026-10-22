using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Employees
{
    [ExcludeFromCodeCoverage]
    public class ListEmployeesQuery : IRequest<OperationResult<PagedResult<Employee>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string FullName { get; set; }
    }
}
