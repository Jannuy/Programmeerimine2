using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class MeasurmentType
    {
        public int Id { get; set; }

        public int Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
