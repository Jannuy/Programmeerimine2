using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MealTypes
{
    // MealType kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteMealTypeCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
