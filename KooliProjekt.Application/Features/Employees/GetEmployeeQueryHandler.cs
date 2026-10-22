using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Employees
{
    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, OperationResult<EmployeeDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEmployeeQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<EmployeeDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<EmployeeDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .Employees
                .Where(item => item.Id == request.Id)
                .Select(item => new EmployeeDto
                {
                    Id = item.Id,
                    Username = item.Username,
                    FullName = item.FullName,
                    PasswordHash = item.PasswordHash,
                    Email = item.Email
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
