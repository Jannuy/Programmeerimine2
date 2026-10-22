using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class MeasurmentUpDto
    {
        public int Id { get; set; }
        public int MeasurmentTypeId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime MeasuredAt { get; set; }
        public string Notes { get; set; }
    }
}
