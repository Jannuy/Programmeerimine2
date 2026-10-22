using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class FoodItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServingSize { get; set; }
        public int ServingUnit { get; set; }
        public string Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G { get; set; }
    }
}
