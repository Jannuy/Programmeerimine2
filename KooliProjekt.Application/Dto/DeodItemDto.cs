using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class DeodItemDto
    {
        public int Id { get; set; }
        public string FoodEntryItem { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
    }
}
