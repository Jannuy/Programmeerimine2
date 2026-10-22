using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DeodItems
{
    [ExcludeFromCodeCoverage]
    public class SaveDeodItemCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string FoodEntryItem { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
    }
}
