using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Employees
{
    // Employee kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteEmployeeCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
