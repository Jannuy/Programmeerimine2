using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    public class ListMeasurmentTypesQueryHandler : IRequestHandler<ListMeasurmentTypesQuery, OperationResult<PagedResult<MeasurmentType>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListMeasurmentTypesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<MeasurmentType>>> Handle(ListMeasurmentTypesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<MeasurmentType>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<MeasurmentType>();
                return result;
            }

            var query = _dbContext.MeasurmentTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                query = query.Where(item => item.Description.Contains(request.Description));
            }

            result.Value = await query
                .OrderBy(item => item.Description)
                .ThenBy(item => item.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
