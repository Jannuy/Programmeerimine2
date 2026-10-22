using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentUps
{
    [ExcludeFromCodeCoverage]
    public class GetMeasurmentUpQuery : IRequest<OperationResult<MeasurmentUpDto>>
    {
        public int Id { get; set; }
    }
}
