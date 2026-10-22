using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.FoodItems;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class FoodItemTests : TestBase
    {
        private static FoodItem CreateItem(int i)
        {
            return new FoodItem
            {
                Name = "Food item " + i, ServingSize = 100, ServingUnit = 1, Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = "250/10/30"
            };
        }

        private static SaveFoodItemCommand CreateCommand(int id, int i)
        {
            return new SaveFoodItemCommand
            {
                Id = id,
                Name = "Food item " + i, ServingSize = 100, ServingUnit = 1, Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = "250/10/30"
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
                new GetFoodItemQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetFoodItemQuery)null;
            var handler = new GetFoodItemQueryHandler(DbContext);

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
            var query = new GetFoodItemQuery { Id = id };
            var handler = new GetFoodItemQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_foodItem()
        {
            // Arrange
            var query = new GetFoodItemQuery { Id = 1 };
            var handler = new GetFoodItemQueryHandler(DbContext);

            var item = CreateItem(1);
            await DbContext.FoodItems.AddAsync(item);
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
        public async Task Get_should_return_null_when_foodItem_does_not_exist(int id)
        {
            // Arrange
            var query = new GetFoodItemQuery { Id = id };
            var handler = new GetFoodItemQueryHandler(DbContext);

            await DbContext.FoodItems.AddAsync(CreateItem(1));
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
                new ListFoodItemsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListFoodItemsQuery)null;
            var handler = new ListFoodItemsQueryHandler(DbContext);

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
            var query = new ListFoodItemsQuery { Page = page, PageSize = pageSize };
            var handler = new ListFoodItemsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_return_page_of_foodItems()
        {
            // Arrange
            var query = new ListFoodItemsQuery { Page = 1, PageSize = 5 };
            var handler = new ListFoodItemsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                await DbContext.FoodItems.AddAsync(CreateItem(i));
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
        public async Task List_should_return_empty_result_if_foodItems_dont_exist()
        {
            // Arrange
            var query = new ListFoodItemsQuery { Page = 1, PageSize = 5 };
            var handler = new ListFoodItemsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_filter_by_Name()
        {
            // Arrange
            var query = new ListFoodItemsQuery { Page = 1, PageSize = 20, Name = CreateItem(3).Name };
            var handler = new ListFoodItemsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 10))
            {
                await DbContext.FoodItems.AddAsync(CreateItem(i));
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var found = Assert.Single(result.Value.Results);
            Assert.Equal(query.Name, found.Name);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteFoodItemCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteFoodItemCommand)null;
            var handler = new DeleteFoodItemCommandHandler(DbContext);

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
            var command = new DeleteFoodItemCommand { Id = id };
            var handler = new DeleteFoodItemCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_foodItem()
        {
            // Arrange
            var command = new DeleteFoodItemCommand { Id = 1 };
            var handler = new DeleteFoodItemCommandHandler(DbContext);

            await DbContext.FoodItems.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.FoodItems.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_foodItem()
        {
            // Arrange
            var command = new DeleteFoodItemCommand { Id = 1034 };
            var handler = new DeleteFoodItemCommandHandler(DbContext);

            await DbContext.FoodItems.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, DbContext.FoodItems.Count());
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveFoodItemCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveFoodItemCommand)null;
            var handler = new SaveFoodItemCommandHandler(DbContext);

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
            var handler = new SaveFoodItemCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_foodItem()
        {
            // Arrange
            var request = CreateCommand(0, 1);
            var handler = new SaveFoodItemCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.FoodItems.SingleOrDefaultAsync(item => item.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_foodItem()
        {
            // Arrange
            var request = CreateCommand(1, 2);
            var handler = new SaveFoodItemCommandHandler(DbContext);

            await DbContext.FoodItems.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.FoodItems.SingleOrDefaultAsync(item => item.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Name, saved.Name);
            Assert.Equal(request.ServingSize, saved.ServingSize);
            Assert.Equal(request.ServingUnit, saved.ServingUnit);
            Assert.Equal(request.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G, saved.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G);
        }

        [Fact]
        public async Task Save_should_not_update_missing_foodItem()
        {
            // Arrange
            var request = CreateCommand(20, 2);
            var handler = new SaveFoodItemCommandHandler(DbContext);

            await DbContext.FoodItems.AddAsync(CreateItem(1));
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
        [InlineData("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void SaveValidator_should_return_false_when_Name_is_invalid(string value)
        {
            // Arrange
            var command = new SaveFoodItemCommand { Id = 0, Name = value };
            var validator = new SaveFoodItemCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveFoodItemCommand.Name), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            await AddRelatedData();
            var command = CreateCommand(0, 1);
            var validator = new SaveFoodItemCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
