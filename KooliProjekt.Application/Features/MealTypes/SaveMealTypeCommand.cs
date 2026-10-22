using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MealTypes
{
    [ExcludeFromCodeCoverage]
    public class SaveMealTypeCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int MealTypeId { get; set; }
    }
}
