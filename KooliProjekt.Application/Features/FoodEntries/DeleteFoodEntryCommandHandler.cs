using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.FoodEntries
{
    public class DeleteFoodEntryCommandHandler : IRequestHandler<DeleteFoodEntryCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteFoodEntryCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteFoodEntryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            // InMemory andmebaas ei toeta ExecuteDeleteAsync meetodit,
            // seepärast loeme kirje enne kustutamist välja
            var item = await _dbContext
                .FoodEntries
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return result;
            }

            _dbContext.FoodEntries.Remove(item);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
