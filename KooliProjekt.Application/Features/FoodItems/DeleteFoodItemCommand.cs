using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodItems
{
    // FoodItem kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteFoodItemCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
