using System;
using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodEntries
{
    [ExcludeFromCodeCoverage]
    public class SaveFoodEntryCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string FoodEntryItem { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
