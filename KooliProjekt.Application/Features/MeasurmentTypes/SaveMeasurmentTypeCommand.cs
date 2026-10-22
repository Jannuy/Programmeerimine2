using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    [ExcludeFromCodeCoverage]
    public class SaveMeasurmentTypeCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
