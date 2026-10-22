using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodItems
{
    [ExcludeFromCodeCoverage]
    public class SaveFoodItemCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServingSize { get; set; }
        public int ServingUnit { get; set; }
        public string Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G { get; set; }
    }
}
