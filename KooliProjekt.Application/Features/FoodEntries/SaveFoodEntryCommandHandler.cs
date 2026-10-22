using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodEntries
{
    public class SaveFoodEntryCommandHandler : IRequestHandler<SaveFoodEntryCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveFoodEntryCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveFoodEntryCommand request, CancellationToken cancellationToken)
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

            var item = new FoodEntry();
            if (request.Id == 0)
            {
                await _dbContext.FoodEntries.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.FoodEntries.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find FoodEntry with ID " + request.Id);
                    return result;
                }
            }

            item.FoodEntryItem = request.FoodEntryItem;
            item.Quantity = request.Quantity;
            item.CreatedAt = request.CreatedAt;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
