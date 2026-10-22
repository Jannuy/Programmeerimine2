using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    // SeedData klass andmete genereerimiseks
    [ExcludeFromCodeCoverage]
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IList<DeodItem> _deodItems = new List<DeodItem>();
        private readonly IList<Employee> _employees = new List<Employee>();
        private readonly IList<EmployeePatient> _employeePatients = new List<EmployeePatient>();
        private readonly IList<FoodEntry> _foodEntrys = new List<FoodEntry>();
        private readonly IList<FoodItem> _foodItems = new List<FoodItem>();
        private readonly IList<MealType> _mealTypes = new List<MealType>();
        private readonly IList<MeasurmentType> _measurmentTypes = new List<MeasurmentType>();
        private readonly IList<MeasurmentUp> _measurmentUps = new List<MeasurmentUp>();

        public SeedData(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        // Generate meetod koordineerib andmete genereerimist
        public void Generate()
        {
            // Ära tee midagi kui andmed on juba olemas
            if (_dbContext.Employees.Any())
            {
                return;
            }

            GenerateDeodItems();
            GenerateEmployees();
            GenerateEmployeePatients();
            GenerateFoodEntries();
            GenerateFoodItems();
            GenerateMealTypes();
            GenerateMeasurmentTypes();

            // Salvestame, et töötajad ja mõõtmistüübid saaksid ID-d
            _dbContext.SaveChanges();

            GenerateMeasurmentUps();

            _dbContext.SaveChanges();
        }

        private void GenerateDeodItems()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new DeodItem
                {
                    FoodEntryItem = "Food entry " + (i + 1),
                    Name = "Deod item " + (i + 1),
                    Label = "Label " + (i + 1)
                };

                _deodItems.Add(item);
            }

            _dbContext.DeodItems.AddRange(_deodItems);
        }

        private void GenerateEmployees()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new Employee
                {
                    Username = "user" + (i + 1),
                    FullName = "Employee " + (i + 1),
                    PasswordHash = "hash",
                    Email = "user" + (i + 1) + "@example.com"
                };

                _employees.Add(item);
            }

            _dbContext.Employees.AddRange(_employees);
        }

        private void GenerateEmployeePatients()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new EmployeePatient
                {
                    FirstName = "First " + (i + 1),
                    LastName = "Last " + (i + 1),
                    Gender = "Male",
                    Notes = "Notes " + (i + 1)
                };

                _employeePatients.Add(item);
            }

            _dbContext.EmployeePatients.AddRange(_employeePatients);
        }

        private void GenerateFoodEntries()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new FoodEntry
                {
                    FoodEntryItem = "Food entry " + (i + 1),
                    Quantity = (i + 1),
                    CreatedAt = DateTime.Now.AddDays(-(i + 1))
                };

                _foodEntrys.Add(item);
            }

            _dbContext.FoodEntries.AddRange(_foodEntrys);
        }

        private void GenerateFoodItems()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new FoodItem
                {
                    Name = "Food item " + (i + 1),
                    ServingSize = 100,
                    ServingUnit = 1,
                    Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = "250/10/30"
                };

                _foodItems.Add(item);
            }

            _dbContext.FoodItems.AddRange(_foodItems);
        }

        private void GenerateMealTypes()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new MealType
                {
                    MealTypeId = (i + 1)
                };

                _mealTypes.Add(item);
            }

            _dbContext.MealTypes.AddRange(_mealTypes);
        }

        private void GenerateMeasurmentTypes()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new MeasurmentType
                {
                    Code = (i + 1),
                    Description = "Measurment type " + (i + 1)
                };

                _measurmentTypes.Add(item);
            }

            _dbContext.MeasurmentTypes.AddRange(_measurmentTypes);
        }

        private void GenerateMeasurmentUps()
        {
            for (var i = 0; i < 10; i++)
            {
                var item = new MeasurmentUp
                {
                    MeasurmentTypeId = _measurmentTypes[i].Id,
                    EmployeeId = _employees[i].Id,
                    MeasuredAt = DateTime.Now.AddHours(-(i + 1)),
                    Notes = "Measurment " + (i + 1)
                };

                _measurmentUps.Add(item);
            }

            _dbContext.MeasurmentUps.AddRange(_measurmentUps);
        }
    }
}
