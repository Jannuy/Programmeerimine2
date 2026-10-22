using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.FoodEntries;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class FoodEntryTests : TestBase
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
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetFoodEntryQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetFoodEntryQuery)null;
            var handler = new GetFoodEntryQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetFoodEntryQuery { Id = id };
            var handler = new GetFoodEntryQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_foodEntry()
        {
            // Arrange
            var query = new GetFoodEntryQuery { Id = 1 };
            var handler = new GetFoodEntryQueryHandler(DbContext);

            var item = CreateItem(1);
            await DbContext.FoodEntries.AddAsync(item);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_foodEntry_does_not_exist(int id)
        {
            // Arrange
            var query = new GetFoodEntryQuery { Id = id };
            var handler = new GetFoodEntryQueryHandler(DbContext);

            await DbContext.FoodEntries.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListFoodEntriesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListFoodEntriesQuery)null;
            var handler = new ListFoodEntriesQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(4, -10)]
        [InlineData(5, -5)]
        [InlineData(0, 0)]
        [InlineData(-5, -10)]
        public async Task List_should_return_empty_result_when_page_or_page_size_is_zero_or_negative(int page, int pageSize)
        {
            // Arrange
            var query = new ListFoodEntriesQuery { Page = page, PageSize = pageSize };
            var handler = new ListFoodEntriesQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_return_page_of_foodEntries()
        {
            // Arrange
            var query = new ListFoodEntriesQuery { Page = 1, PageSize = 5 };
            var handler = new ListFoodEntriesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                await DbContext.FoodEntries.AddAsync(CreateItem(i));
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_foodEntries_dont_exist()
        {
            // Arrange
            var query = new ListFoodEntriesQuery { Page = 1, PageSize = 5 };
            var handler = new ListFoodEntriesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_filter_by_FoodEntryItem()
        {
            // Arrange
            var query = new ListFoodEntriesQuery { Page = 1, PageSize = 20, FoodEntryItem = CreateItem(3).FoodEntryItem };
            var handler = new ListFoodEntriesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 10))
            {
                await DbContext.FoodEntries.AddAsync(CreateItem(i));
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var found = Assert.Single(result.Value.Results);
            Assert.Equal(query.FoodEntryItem, found.FoodEntryItem);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteFoodEntryCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteFoodEntryCommand)null;
            var handler = new DeleteFoodEntryCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            // Arrange
            var command = new DeleteFoodEntryCommand { Id = id };
            var handler = new DeleteFoodEntryCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_foodEntry()
        {
            // Arrange
            var command = new DeleteFoodEntryCommand { Id = 1 };
            var handler = new DeleteFoodEntryCommandHandler(DbContext);

            await DbContext.FoodEntries.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.FoodEntries.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_foodEntry()
        {
            // Arrange
            var command = new DeleteFoodEntryCommand { Id = 1034 };
            var handler = new DeleteFoodEntryCommandHandler(DbContext);

            await DbContext.FoodEntries.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, DbContext.FoodEntries.Count());
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveFoodEntryCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveFoodEntryCommand)null;
            var handler = new SaveFoodEntryCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_error_if_id_is_negative()
        {
            // Arrange
            var request = CreateCommand(-10, 1);
            var handler = new SaveFoodEntryCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_foodEntry()
        {
            // Arrange
            var request = CreateCommand(0, 1);
            var handler = new SaveFoodEntryCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.FoodEntries.SingleOrDefaultAsync(item => item.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_foodEntry()
        {
            // Arrange
            var request = CreateCommand(1, 2);
            var handler = new SaveFoodEntryCommandHandler(DbContext);

            await DbContext.FoodEntries.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.FoodEntries.SingleOrDefaultAsync(item => item.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.FoodEntryItem, saved.FoodEntryItem);
            Assert.Equal(request.Quantity, saved.Quantity);
            Assert.Equal(request.CreatedAt, saved.CreatedAt);
        }

        [Fact]
        public async Task Save_should_not_update_missing_foodEntry()
        {
            // Arrange
            var request = CreateCommand(20, 2);
            var handler = new SaveFoodEntryCommandHandler(DbContext);

            await DbContext.FoodEntries.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void SaveValidator_should_return_false_when_FoodEntryItem_is_invalid(string value)
        {
            // Arrange
            var command = new SaveFoodEntryCommand { Id = 0, FoodEntryItem = value };
            var validator = new SaveFoodEntryCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveFoodEntryCommand.FoodEntryItem), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            await AddRelatedData();
            var command = CreateCommand(0, 1);
            var validator = new SaveFoodEntryCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
