using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class MeasurmentUp
    {
        public int Id { get; set; }

        public int MeasurmentTypeId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime MeasuredAt { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }
    }
}
