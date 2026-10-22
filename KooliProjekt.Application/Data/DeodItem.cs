using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class DeodItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FoodEntryItem { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Label { get; set; }
    }
}
