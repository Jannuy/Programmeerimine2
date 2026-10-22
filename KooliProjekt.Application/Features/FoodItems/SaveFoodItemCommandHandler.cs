using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodItems
{
    public class SaveFoodItemCommandHandler : IRequestHandler<SaveFoodItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveFoodItemCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveFoodItemCommand request, CancellationToken cancellationToken)
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

            var item = new FoodItem();
            if (request.Id == 0)
            {
                await _dbContext.FoodItems.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.FoodItems.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find FoodItem with ID " + request.Id);
                    return result;
                }
            }

            item.Name = request.Name;
            item.ServingSize = request.ServingSize;
            item.ServingUnit = request.ServingUnit;
            item.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = request.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
