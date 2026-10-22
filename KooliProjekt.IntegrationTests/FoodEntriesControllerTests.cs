using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.FoodEntries;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class FoodEntriesControllerTests : TestBase
    {
        private static FoodEntry CreateItem(int i)
        {
            return new FoodEntry
            {
                FoodEntryItem = "Food entry " + i, Quantity = i, CreatedAt = new DateTime(2026, 1, 1).AddDays(-i)
            };
        }

        private static SaveFoodEntryCommand CreateCommand(int id, int i)
        {
            return new SaveFoodEntryCommand
            {
                Id = id,
                FoodEntryItem = "Food entry " + i, Quantity = i, CreatedAt = new DateTime(2026, 1, 1).AddDays(-i)
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
            var url = "/api/FoodEntries/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<FoodEntry>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_foodEntry()
        {
            // Arrange
            var item = CreateItem(1);
            await DbContext.AddAsync(item);
            await DbContext.SaveChangesAsync();

            var url = "/api/FoodEntries/Get/?id=" + item.Id;

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<FoodEntry>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.Equal(item.Id, response.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_foodEntry()
        {
            // Arrange
            var url = "/api/FoodEntries/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_foodEntry()
        {
            // Arrange
            var url = "/api/FoodEntries/Delete";
            var item = CreateItem(1);

            await DbContext.AddAsync(item);
            await DbContext.SaveChangesAsync();

            var command = new DeleteFoodEntryCommand { Id = item.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var deleted = await DbContext.FoodEntries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_should_handle_missing_foodEntry()
        {
            // Arrange
            var url = "/api/FoodEntries/Delete";

            var command = new DeleteFoodEntryCommand { Id = 101 };
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
        public async Task Save_should_add_new_foodEntry()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/FoodEntries/Save";
            var command = CreateCommand(0, 1);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var created = await DbContext.FoodEntries.AsNoTracking().FirstOrDefaultAsync(x => x.FoodEntryItem == command.FoodEntryItem);
            Assert.NotNull(created);
        }

        [Fact]
        public async Task Save_should_not_update_missing_foodEntry()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/FoodEntries/Save";
            var command = CreateCommand(101, 1);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var created = await DbContext.FoodEntries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 101);
            Assert.Null(created);
        }

        [Fact]
        public async Task Save_should_not_save_invalid_foodEntry()
        {
            // Arrange
            await AddRelatedData();

            var url = "/api/FoodEntries/Save";
            var command = CreateCommand(0, 1);
            command.FoodEntryItem = "";
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(0, await DbContext.FoodEntries.CountAsync());
        }
    }
}
