using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MealTypes
{
    public class ListMealTypesQueryHandler : IRequestHandler<ListMealTypesQuery, OperationResult<PagedResult<MealType>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListMealTypesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<MealType>>> Handle(ListMealTypesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<MealType>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                result.Value = new PagedResult<MealType>();
                return result;
            }

            var query = _dbContext.MealTypes.AsQueryable();

            result.Value = await query
                .OrderBy(item => item.MealTypeId)
                .ThenBy(item => item.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
