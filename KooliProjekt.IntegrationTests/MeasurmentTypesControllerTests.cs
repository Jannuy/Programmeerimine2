using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.MeasurmentTypes;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class MeasurmentTypesControllerTests : TestBase
    {
        private static MeasurmentType CreateItem(int i)
        {
            return new MeasurmentType
            {
                Code = i, Description = "Measurment type " + i
            };
        }

        private static SaveMeasurmentTypeCommand CreateCommand(int id, int i)
        {
            return new SaveMeasurmentTypeCommand
            {
                Id = id,
                Code = i, Description = "Measurment type " + i
            };
        }

        // Andmed, mida valideerija kontrollib (nt viidatud kirjed)
        private async Task AddRelatedData()
        {
            
            await Task.CompletedTask;
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/MeasurmentTypes/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<MeasurmentType>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_measurmentType()
        {
            // Arrange
            var item = CreateItem(1);
            await DbContext.AddAsync(item);
            await DbContext.SaveChangesAsync();

            var url = "/api/MeasurmentTypes/Get/?id=" + item.Id;

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<MeasurmentType>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.Equal(item.Id, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_measurmentType()
        {
            // Arrange
            var url = "/api/MeasurmentTypes/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_measurmentType()
        {
            // Arrange
            var url = "/api/MeasurmentTypes/Delete";
            var item = CreateItem(1);

            await DbContext.AddAsync(item);
            await DbContext.SaveChangesAsync();

            var command = new DeleteMeasurmentTypeCommand { Id = item.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var deleted = await DbContext.MeasurmentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_should_handle_missing_measurmentType()
        {
            // Arrange
            var url = "/api/MeasurmentTypes/Delete";

            var command = new DeleteMeasurmentTypeCommand { Id = 101 };
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
        public async Task Save_should_add_new_measurmentType()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/MeasurmentTypes/Save";
            var command = CreateCommand(0, 1);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var created = await DbContext.MeasurmentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Description == command.Description);
            Assert.NotNull(created);
        }

        [Fact]
        public async Task Save_should_not_update_missing_measurmentType()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/MeasurmentTypes/Save";
            var command = CreateCommand(101, 1);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var created = await DbContext.MeasurmentTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 101);
            Assert.Null(created);
        }

        [Fact]
        public async Task Save_should_not_save_invalid_measurmentType()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/MeasurmentTypes/Save";
            var command = CreateCommand(0, 1);
            command.Code = 0;
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(0, await DbContext.MeasurmentTypes.CountAsync());
        }
    }
}
