using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EmployeePatients
{
    public class SaveEmployeePatientCommandHandler : IRequestHandler<SaveEmployeePatientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveEmployeePatientCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveEmployeePatientCommand request, CancellationToken cancellationToken)
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

            var item = new EmployeePatient();
            if (request.Id == 0)
            {
                await _dbContext.EmployeePatients.AddAsync(item, cancellationToken);
            }
            else
            {
                item = await _dbContext.EmployeePatients.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    result.AddError("Cannot find EmployeePatient with ID " + request.Id);
                    return result;
                }
            }

            item.FirstName = request.FirstName;
            item.LastName = request.LastName;
            item.Gender = request.Gender;
            item.Notes = request.Notes;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
