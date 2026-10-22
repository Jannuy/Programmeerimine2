using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MealTypes
{
    [ExcludeFromCodeCoverage]
    public class GetMealTypeQuery : IRequest<OperationResult<MealTypeDto>>
    {
        public int Id { get; set; }
    }
}
