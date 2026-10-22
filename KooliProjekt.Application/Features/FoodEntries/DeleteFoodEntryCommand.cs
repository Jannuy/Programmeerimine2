using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodEntries
{
    // FoodEntry kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteFoodEntryCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
