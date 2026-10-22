using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class MeasurmentTypeDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
