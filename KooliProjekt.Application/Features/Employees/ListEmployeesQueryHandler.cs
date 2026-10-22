using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Employees
{
    public class ListEmployeesQueryHandler : IRequestHandler<ListEmployeesQuery, OperationResult<PagedResult<Employee>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListEmployeesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Employee>>> Handle(ListEmployeesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Employee>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<Employee>();
                return result;
            }

            var query = _dbContext.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                query = query.Where(item => item.FullName.Contains(request.FullName));
            }

            result.Value = await query
                .OrderBy(item => item.FullName)
                .ThenBy(item => item.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
