using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class FoodItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public int ServingSize { get; set; }

        public int ServingUnit { get; set; }

        [Required]
        [MaxLength(100)]
        public string Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G { get; set; }
    }
}
