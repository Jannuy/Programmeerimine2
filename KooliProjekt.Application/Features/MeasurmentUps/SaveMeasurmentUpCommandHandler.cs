using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentUps
{
    public class SaveMeasurmentUpCommandHandler : IRequestHandler<SaveMeasurmentUpCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMeasurmentUpCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMeasurmentUpCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if (request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            var item = new MeasurmentUp();
            if (request.Id == 0)
            {
                await _dbContext.MeasurmentUps.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.MeasurmentUps.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find MeasurmentUp with ID " + request.Id);
                    return result;
                }
            }

            item.MeasurmentTypeId = request.MeasurmentTypeId;
            item.EmployeeId = request.EmployeeId;
            item.MeasuredAt = request.MeasuredAt;
            item.Notes = request.Notes;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
