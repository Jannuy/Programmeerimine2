using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.MeasurmentUps;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class MeasurmentUpsControllerTests : TestBase
    {
        private static MeasurmentUp CreateItem(int i)
        {
            return new MeasurmentUp
            {
                MeasurmentTypeId = 1, EmployeeId = 1, MeasuredAt = new DateTime(2026, 1, 1).AddHours(-i), Notes = "Measurment " + i
            };
        }

        private static SaveMeasurmentUpCommand CreateCommand(int id, int i)
        {
            return new SaveMeasurmentUpCommand
            {
                Id = id,
                MeasurmentTypeId = 1, EmployeeId = 1, MeasuredAt = new DateTime(2026, 1, 1).AddHours(-i), Notes = "Measurment " + i
            };
        }

        // Andmed, mida valideerija kontrollib (nt viidatud kirjed)
        private async Task AddRelatedData()
        {
            await DbContext.MeasurmentTypes.AddAsync(new MeasurmentType { Code = 1, Description = "Measurment type 1" });
            await DbContext.Employees.AddAsync(new Employee { Username = "user1", FullName = "Employee 1", PasswordHash = "hash", Email = "user1@example.com" });
            await DbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/MeasurmentUps/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<MeasurmentUp>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_measurmentUp()
        {
            // Arrange
            var item = CreateItem(1);
            await DbContext.AddAsync(item);
            await DbContext.SaveChangesAsync();

            var url = "/api/MeasurmentUps/Get/?id=" + item.Id;

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<MeasurmentUp>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.Equal(item.Id, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_measurmentUp()
        {
            // Arrange
            var url = "/api/MeasurmentUps/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_measurmentUp()
        {
            // Arrange
            var url = "/api/MeasurmentUps/Delete";
            var item = CreateItem(1);

            await DbContext.AddAsync(item);
            await DbContext.SaveChangesAsync();

            var command = new DeleteMeasurmentUpCommand { Id = item.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var deleted = await DbContext.MeasurmentUps.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_should_handle_missing_measurmentUp()
        {
            // Arrange
            var url = "/api/MeasurmentUps/Delete";

            var command = new DeleteMeasurmentUpCommand { Id = 101 };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Save_should_add_new_measurmentUp()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/MeasurmentUps/Save";
            var command = CreateCommand(0, 1);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var created = await DbContext.MeasurmentUps.AsNoTracking().FirstOrDefaultAsync(x => x.Notes == command.Notes);
            Assert.NotNull(created);
        }

        [Fact]
        public async Task Save_should_not_update_missing_measurmentUp()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/MeasurmentUps/Save";
            var command = CreateCommand(101, 1);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var created = await DbContext.MeasurmentUps.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 101);
            Assert.Null(created);
        }

        [Fact]
        public async Task Save_should_not_save_invalid_measurmentUp()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/MeasurmentUps/Save";
            var command = CreateCommand(0, 1);
            command.MeasurmentTypeId = 999;
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(0, await DbContext.MeasurmentUps.CountAsync());
        }
    }
}
