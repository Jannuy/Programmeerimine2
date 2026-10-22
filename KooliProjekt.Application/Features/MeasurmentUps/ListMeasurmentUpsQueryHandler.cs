using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentUps
{
    public class ListMeasurmentUpsQueryHandler : IRequestHandler<ListMeasurmentUpsQuery, OperationResult<PagedResult<MeasurmentUp>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListMeasurmentUpsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<MeasurmentUp>>> Handle(ListMeasurmentUpsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<MeasurmentUp>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<MeasurmentUp>();
                return result;
            }

            var query = _dbContext.MeasurmentUps.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Notes))
            {
                query = query.Where(item => item.Notes.Contains(request.Notes));
            }

            result.Value = await query
                .OrderBy(item => item.Notes)
                .ThenBy(item => item.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
