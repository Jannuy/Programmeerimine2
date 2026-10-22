using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DeodItems
{
    public class GetDeodItemQueryHandler : IRequestHandler<GetDeodItemQuery, OperationResult<DeodItemDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDeodItemQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<DeodItemDto>> Handle(GetDeodItemQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<DeodItemDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .DeodItems
                .Where(item => item.Id == request.Id)
                .Select(item => new DeodItemDto
                {
                    Id = item.Id,
                    FoodEntryItem = item.FoodEntryItem,
                    Name = item.Name,
                    Label = item.Label
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
