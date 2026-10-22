using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MealTypes
{
    public class SaveMealTypeCommandHandler : IRequestHandler<SaveMealTypeCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMealTypeCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMealTypeCommand request, CancellationToken cancellationToken)
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

            var item = new MealType();
            if (request.Id == 0)
            {
                await _dbContext.MealTypes.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.MealTypes.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find MealType with ID " + request.Id);
                    return result;
                }
            }

            item.MealTypeId = request.MealTypeId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
