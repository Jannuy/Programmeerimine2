using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DeodItems
{
    [ExcludeFromCodeCoverage]
    public class GetDeodItemQuery : IRequest<OperationResult<DeodItemDto>>
    {
        public int Id { get; set; }
    }
}
