using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodEntries
{
    [ExcludeFromCodeCoverage]
    public class GetFoodEntryQuery : IRequest<OperationResult<FoodEntryDto>>
    {
        public int Id { get; set; }
    }
}
