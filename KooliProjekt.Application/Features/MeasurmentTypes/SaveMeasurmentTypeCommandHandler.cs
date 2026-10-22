using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    public class SaveMeasurmentTypeCommandHandler : IRequestHandler<SaveMeasurmentTypeCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMeasurmentTypeCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMeasurmentTypeCommand request, CancellationToken cancellationToken)
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

            var item = new MeasurmentType();
            if (request.Id == 0)
            {
                await _dbContext.MeasurmentTypes.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.MeasurmentTypes.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find MeasurmentType with ID " + request.Id);
                    return result;
                }
            }

            item.Code = request.Code;
            item.Description = request.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
