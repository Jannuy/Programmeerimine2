using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MealTypes
{
    public class GetMealTypeQueryHandler : IRequestHandler<GetMealTypeQuery, OperationResult<MealTypeDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMealTypeQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<MealTypeDto>> Handle(GetMealTypeQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<MealTypeDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .MealTypes
                .Where(item => item.Id == request.Id)
                .Select(item => new MealTypeDto
                {
                    Id = item.Id,
                    MealTypeId = item.MealTypeId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
