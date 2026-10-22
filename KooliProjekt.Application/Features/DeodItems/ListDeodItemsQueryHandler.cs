using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DeodItems
{
    public class ListDeodItemsQueryHandler : IRequestHandler<ListDeodItemsQuery, OperationResult<PagedResult<DeodItem>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListDeodItemsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DeodItem>>> Handle(ListDeodItemsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<DeodItem>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<DeodItem>();
                return result;
            }

            var query = _dbContext.DeodItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(item => item.Name.Contains(request.Name));
            }

            result.Value = await query
                .OrderBy(item => item.Name)
                .ThenBy(item => item.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
