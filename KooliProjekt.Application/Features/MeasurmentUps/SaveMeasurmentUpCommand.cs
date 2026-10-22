using System;
using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentUps
{
    [ExcludeFromCodeCoverage]
    public class SaveMeasurmentUpCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int MeasurmentTypeId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime MeasuredAt { get; set; }
        public string Notes { get; set; }
    }
}
