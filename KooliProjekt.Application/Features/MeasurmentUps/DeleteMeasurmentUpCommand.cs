using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentUps
{
    // MeasurmentUp kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteMeasurmentUpCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
