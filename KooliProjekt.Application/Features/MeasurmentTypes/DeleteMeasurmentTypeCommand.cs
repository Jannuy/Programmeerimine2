using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    // MeasurmentType kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteMeasurmentTypeCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
