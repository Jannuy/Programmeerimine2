using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    public class GetMeasurmentTypeQueryHandler : IRequestHandler<GetMeasurmentTypeQuery, OperationResult<MeasurmentTypeDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMeasurmentTypeQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<MeasurmentTypeDto>> Handle(GetMeasurmentTypeQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<MeasurmentTypeDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .MeasurmentTypes
                .Where(item => item.Id == request.Id)
                .Select(item => new MeasurmentTypeDto
                {
                    Id = item.Id,
                    Code = item.Code,
                    Description = item.Description
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
