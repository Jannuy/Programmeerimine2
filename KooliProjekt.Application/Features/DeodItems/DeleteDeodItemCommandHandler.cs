using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DeodItems
{
    public class DeleteDeodItemCommandHandler : IRequestHandler<DeleteDeodItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteDeodItemCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDeodItemCommand request, CancellationToken cancellationToken)
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
                .DeodItems
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return result;
            }

            _dbContext.DeodItems.Remove(item);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
