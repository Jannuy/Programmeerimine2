namespace KooliProjekt.BlazorWasm
{
    public class MeasurmentUp
    {
        public int Id { get; set; }
        public int MeasurmentTypeId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime MeasuredAt { get; set; }
        public string Notes { get; set; }
    }
}
