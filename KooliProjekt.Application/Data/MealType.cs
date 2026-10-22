using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class MealType
    {
        public int Id { get; set; }

        public int MealTypeId { get; set; }
    }
}
