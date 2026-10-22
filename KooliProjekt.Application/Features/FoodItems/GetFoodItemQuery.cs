using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodItems
{
    [ExcludeFromCodeCoverage]
    public class GetFoodItemQuery : IRequest<OperationResult<FoodItemDto>>
    {
        public int Id { get; set; }
    }
}
