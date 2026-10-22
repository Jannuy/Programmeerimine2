using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.MealTypes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class MealTypeTests : TestBase
    {
        private static MealType CreateItem(int i)
        {
            return new MealType
            {
                MealTypeId = i
            };
        }

        private static SaveMealTypeCommand CreateCommand(int id, int i)
        {
            return new SaveMealTypeCommand
            {
                Id = id,
                MealTypeId = i
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
                new GetMealTypeQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetMealTypeQuery)null;
            var handler = new GetMealTypeQueryHandler(DbContext);

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
            var query = new GetMealTypeQuery { Id = id };
            var handler = new GetMealTypeQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_mealType()
        {
            // Arrange
            var query = new GetMealTypeQuery { Id = 1 };
            var handler = new GetMealTypeQueryHandler(DbContext);

            var item = CreateItem(1);
            await DbContext.MealTypes.AddAsync(item);
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
        public async Task Get_should_return_null_when_mealType_does_not_exist(int id)
        {
            // Arrange
            var query = new GetMealTypeQuery { Id = id };
            var handler = new GetMealTypeQueryHandler(DbContext);

            await DbContext.MealTypes.AddAsync(CreateItem(1));
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
                new ListMealTypesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListMealTypesQuery)null;
            var handler = new ListMealTypesQueryHandler(DbContext);

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
            var query = new ListMealTypesQuery { Page = page, PageSize = pageSize };
            var handler = new ListMealTypesQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_return_page_of_mealTypes()
        {
            // Arrange
            var query = new ListMealTypesQuery { Page = 1, PageSize = 5 };
            var handler = new ListMealTypesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                await DbContext.MealTypes.AddAsync(CreateItem(i));
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
        public async Task List_should_return_empty_result_if_mealTypes_dont_exist()
        {
            // Arrange
            var query = new ListMealTypesQuery { Page = 1, PageSize = 5 };
            var handler = new ListMealTypesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteMealTypeCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteMealTypeCommand)null;
            var handler = new DeleteMealTypeCommandHandler(DbContext);

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
            var command = new DeleteMealTypeCommand { Id = id };
            var handler = new DeleteMealTypeCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_mealType()
        {
            // Arrange
            var command = new DeleteMealTypeCommand { Id = 1 };
            var handler = new DeleteMealTypeCommandHandler(DbContext);

            await DbContext.MealTypes.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.MealTypes.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_mealType()
        {
            // Arrange
            var command = new DeleteMealTypeCommand { Id = 1034 };
            var handler = new DeleteMealTypeCommandHandler(DbContext);

            await DbContext.MealTypes.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, DbContext.MealTypes.Count());
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveMealTypeCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveMealTypeCommand)null;
            var handler = new SaveMealTypeCommandHandler(DbContext);

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
            var handler = new SaveMealTypeCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_mealType()
        {
            // Arrange
            var request = CreateCommand(0, 1);
            var handler = new SaveMealTypeCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MealTypes.SingleOrDefaultAsync(item => item.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_mealType()
        {
            // Arrange
            var request = CreateCommand(1, 2);
            var handler = new SaveMealTypeCommandHandler(DbContext);

            await DbContext.MealTypes.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MealTypes.SingleOrDefaultAsync(item => item.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.MealTypeId, saved.MealTypeId);
        }

        [Fact]
        public async Task Save_should_not_update_missing_mealType()
        {
            // Arrange
            var request = CreateCommand(20, 2);
            var handler = new SaveMealTypeCommandHandler(DbContext);

            await DbContext.MealTypes.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_MealTypeId_is_invalid(int value)
        {
            // Arrange
            var command = new SaveMealTypeCommand { Id = 0, MealTypeId = value };
            var validator = new SaveMealTypeCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMealTypeCommand.MealTypeId), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            await AddRelatedData();
            var command = CreateCommand(0, 1);
            var validator = new SaveMealTypeCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
