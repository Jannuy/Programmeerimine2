using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EmployeePatients
{
    public class ListEmployeePatientsQueryHandler : IRequestHandler<ListEmployeePatientsQuery, OperationResult<PagedResult<EmployeePatient>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListEmployeePatientsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<EmployeePatient>>> Handle(ListEmployeePatientsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<EmployeePatient>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<EmployeePatient>();
                return result;
            }

            var query = _dbContext.EmployeePatients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                query = query.Where(item => item.LastName.Contains(request.LastName));
            }

            result.Value = await query
                .OrderBy(item => item.LastName)
                .ThenBy(item => item.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
