using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Employees
{
    public class SaveEmployeeCommandHandler : IRequestHandler<SaveEmployeeCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveEmployeeCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveEmployeeCommand request, CancellationToken cancellationToken)
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

            var item = new Employee();
            if (request.Id == 0)
            {
                await _dbContext.Employees.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.Employees.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find Employee with ID " + request.Id);
                    return result;
                }
            }

            item.Username = request.Username;
            item.FullName = request.FullName;
            item.PasswordHash = request.PasswordHash;
            item.Email = request.Email;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
