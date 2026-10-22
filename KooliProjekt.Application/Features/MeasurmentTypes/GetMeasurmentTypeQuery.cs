using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    [ExcludeFromCodeCoverage]
    public class GetMeasurmentTypeQuery : IRequest<OperationResult<MeasurmentTypeDto>>
    {
        public int Id { get; set; }
    }
}
