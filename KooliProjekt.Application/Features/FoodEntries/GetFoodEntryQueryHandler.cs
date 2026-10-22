using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.FoodEntries
{
    public class GetFoodEntryQueryHandler : IRequestHandler<GetFoodEntryQuery, OperationResult<FoodEntryDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetFoodEntryQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<FoodEntryDto>> Handle(GetFoodEntryQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<FoodEntryDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .FoodEntries
                .Where(item => item.Id == request.Id)
                .Select(item => new FoodEntryDto
                {
                    Id = item.Id,
                    FoodEntryItem = item.FoodEntryItem,
                    Quantity = item.Quantity,
                    CreatedAt = item.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
