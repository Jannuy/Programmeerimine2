using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DeodItem> DeodItems { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePatient> EmployeePatients { get; set; }
        public DbSet<FoodEntry> FoodEntries { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<MeasurmentType> MeasurmentTypes { get; set; }
        public DbSet<MeasurmentUp> MeasurmentUps { get; set; }
    }
}
