using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DeodItems
{
    public class SaveDeodItemCommandHandler : IRequestHandler<SaveDeodItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveDeodItemCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveDeodItemCommand request, CancellationToken cancellationToken)
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

            var item = new DeodItem();
            if (request.Id == 0)
            {
                await _dbContext.DeodItems.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.DeodItems.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find DeodItem with ID " + request.Id);
                    return result;
                }
            }

            item.FoodEntryItem = request.FoodEntryItem;
            item.Name = request.Name;
            item.Label = request.Label;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
