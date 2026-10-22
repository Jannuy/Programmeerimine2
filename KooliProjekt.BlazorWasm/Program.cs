using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KooliProjekt.BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Iga olemi jaoks oma API klient (parameeter on kontrolleri nimi)
            builder.Services.AddScoped<IApiClient<DeodItem>>(sp => new ApiClient<DeodItem>("DeodItems"));
            builder.Services.AddScoped<IApiClient<Employee>>(sp => new ApiClient<Employee>("Employees"));
            builder.Services.AddScoped<IApiClient<EmployeePatient>>(sp => new ApiClient<EmployeePatient>("EmployeePatients"));
            builder.Services.AddScoped<IApiClient<FoodEntry>>(sp => new ApiClient<FoodEntry>("FoodEntries"));
            builder.Services.AddScoped<IApiClient<FoodItem>>(sp => new ApiClient<FoodItem>("FoodItems"));
            builder.Services.AddScoped<IApiClient<MealType>>(sp => new ApiClient<MealType>("MealTypes"));
            builder.Services.AddScoped<IApiClient<MeasurmentType>>(sp => new ApiClient<MeasurmentType>("MeasurmentTypes"));
            builder.Services.AddScoped<IApiClient<MeasurmentUp>>(sp => new ApiClient<MeasurmentUp>("MeasurmentUps"));

            await builder.Build().RunAsync();
        }
    }
}
