using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.FoodItems
{
    public class GetFoodItemQueryHandler : IRequestHandler<GetFoodItemQuery, OperationResult<FoodItemDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetFoodItemQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<FoodItemDto>> Handle(GetFoodItemQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<FoodItemDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .FoodItems
                .Where(item => item.Id == request.Id)
                .Select(item => new FoodItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    ServingSize = item.ServingSize,
                    ServingUnit = item.ServingUnit,
                    Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = item.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
