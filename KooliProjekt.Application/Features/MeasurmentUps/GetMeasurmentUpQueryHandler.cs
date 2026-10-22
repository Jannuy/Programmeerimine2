using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MeasurmentUps
{
    public class GetMeasurmentUpQueryHandler : IRequestHandler<GetMeasurmentUpQuery, OperationResult<MeasurmentUpDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMeasurmentUpQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<MeasurmentUpDto>> Handle(GetMeasurmentUpQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<MeasurmentUpDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .MeasurmentUps
                .Where(item => item.Id == request.Id)
                .Select(item => new MeasurmentUpDto
                {
                    Id = item.Id,
                    MeasurmentTypeId = item.MeasurmentTypeId,
                    EmployeeId = item.EmployeeId,
                    MeasuredAt = item.MeasuredAt,
                    Notes = item.Notes
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
