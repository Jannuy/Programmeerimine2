using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.MeasurmentTypes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class MeasurmentTypeTests : TestBase
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
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetMeasurmentTypeQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetMeasurmentTypeQuery)null;
            var handler = new GetMeasurmentTypeQueryHandler(DbContext);

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
            var query = new GetMeasurmentTypeQuery { Id = id };
            var handler = new GetMeasurmentTypeQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_measurmentType()
        {
            // Arrange
            var query = new GetMeasurmentTypeQuery { Id = 1 };
            var handler = new GetMeasurmentTypeQueryHandler(DbContext);

            var item = CreateItem(1);
            await DbContext.MeasurmentTypes.AddAsync(item);
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
        public async Task Get_should_return_null_when_measurmentType_does_not_exist(int id)
        {
            // Arrange
            var query = new GetMeasurmentTypeQuery { Id = id };
            var handler = new GetMeasurmentTypeQueryHandler(DbContext);

            await DbContext.MeasurmentTypes.AddAsync(CreateItem(1));
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
                new ListMeasurmentTypesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListMeasurmentTypesQuery)null;
            var handler = new ListMeasurmentTypesQueryHandler(DbContext);

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
            var query = new ListMeasurmentTypesQuery { Page = page, PageSize = pageSize };
            var handler = new ListMeasurmentTypesQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_return_page_of_measurmentTypes()
        {
            // Arrange
            var query = new ListMeasurmentTypesQuery { Page = 1, PageSize = 5 };
            var handler = new ListMeasurmentTypesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                await DbContext.MeasurmentTypes.AddAsync(CreateItem(i));
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
        public async Task List_should_return_empty_result_if_measurmentTypes_dont_exist()
        {
            // Arrange
            var query = new ListMeasurmentTypesQuery { Page = 1, PageSize = 5 };
            var handler = new ListMeasurmentTypesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_filter_by_Description()
        {
            // Arrange
            var query = new ListMeasurmentTypesQuery { Page = 1, PageSize = 20, Description = CreateItem(3).Description };
            var handler = new ListMeasurmentTypesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 10))
            {
                await DbContext.MeasurmentTypes.AddAsync(CreateItem(i));
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var found = Assert.Single(result.Value.Results);
            Assert.Equal(query.Description, found.Description);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteMeasurmentTypeCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteMeasurmentTypeCommand)null;
            var handler = new DeleteMeasurmentTypeCommandHandler(DbContext);

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
            var command = new DeleteMeasurmentTypeCommand { Id = id };
            var handler = new DeleteMeasurmentTypeCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_measurmentType()
        {
            // Arrange
            var command = new DeleteMeasurmentTypeCommand { Id = 1 };
            var handler = new DeleteMeasurmentTypeCommandHandler(DbContext);

            await DbContext.MeasurmentTypes.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.MeasurmentTypes.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_measurmentType()
        {
            // Arrange
            var command = new DeleteMeasurmentTypeCommand { Id = 1034 };
            var handler = new DeleteMeasurmentTypeCommandHandler(DbContext);

            await DbContext.MeasurmentTypes.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, DbContext.MeasurmentTypes.Count());
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveMeasurmentTypeCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveMeasurmentTypeCommand)null;
            var handler = new SaveMeasurmentTypeCommandHandler(DbContext);

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
            var handler = new SaveMeasurmentTypeCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_measurmentType()
        {
            // Arrange
            var request = CreateCommand(0, 1);
            var handler = new SaveMeasurmentTypeCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MeasurmentTypes.SingleOrDefaultAsync(item => item.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_measurmentType()
        {
            // Arrange
            var request = CreateCommand(1, 2);
            var handler = new SaveMeasurmentTypeCommandHandler(DbContext);

            await DbContext.MeasurmentTypes.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MeasurmentTypes.SingleOrDefaultAsync(item => item.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Code, saved.Code);
            Assert.Equal(request.Description, saved.Description);
        }

        [Fact]
        public async Task Save_should_not_update_missing_measurmentType()
        {
            // Arrange
            var request = CreateCommand(20, 2);
            var handler = new SaveMeasurmentTypeCommandHandler(DbContext);

            await DbContext.MeasurmentTypes.AddAsync(CreateItem(1));
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
        public void SaveValidator_should_return_false_when_Code_is_invalid(int value)
        {
            // Arrange
            var command = new SaveMeasurmentTypeCommand { Id = 0, Code = value };
            var validator = new SaveMeasurmentTypeCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMeasurmentTypeCommand.Code), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            await AddRelatedData();
            var command = CreateCommand(0, 1);
            var validator = new SaveMeasurmentTypeCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
