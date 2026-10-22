using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodEntries
{
    public class ListFoodEntriesQueryHandler : IRequestHandler<ListFoodEntriesQuery, OperationResult<PagedResult<FoodEntry>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListFoodEntriesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<FoodEntry>>> Handle(ListFoodEntriesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<FoodEntry>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<FoodEntry>();
                return result;
            }

            var query = _dbContext.FoodEntries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.FoodEntryItem))
            {
                query = query.Where(item => item.FoodEntryItem.Contains(request.FoodEntryItem));
            }

            result.Value = await query
                .OrderBy(item => item.FoodEntryItem)
                .ThenBy(item => item.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
