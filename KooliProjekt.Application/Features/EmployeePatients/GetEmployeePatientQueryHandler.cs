using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.EmployeePatients
{
    public class GetEmployeePatientQueryHandler : IRequestHandler<GetEmployeePatientQuery, OperationResult<EmployeePatientDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEmployeePatientQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<EmployeePatientDto>> Handle(GetEmployeePatientQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<EmployeePatientDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .EmployeePatients
                .Where(item => item.Id == request.Id)
                .Select(item => new EmployeePatientDto
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender,
                    Notes = item.Notes
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
