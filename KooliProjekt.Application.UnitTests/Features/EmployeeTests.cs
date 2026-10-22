using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Employees;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class EmployeeTests : TestBase
    {
        private static Employee CreateItem(int i)
        {
            return new Employee
            {
                Username = "user" + i, FullName = "Employee " + i, PasswordHash = "hash", Email = "user" + i + "@example.com"
            };
        }

        private static SaveEmployeeCommand CreateCommand(int id, int i)
        {
            return new SaveEmployeeCommand
            {
                Id = id,
                Username = "user" + i, FullName = "Employee " + i, PasswordHash = "hash", Email = "user" + i + "@example.com"
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
                new GetEmployeeQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetEmployeeQuery)null;
            var handler = new GetEmployeeQueryHandler(DbContext);

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
            var query = new GetEmployeeQuery { Id = id };
            var handler = new GetEmployeeQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_employee()
        {
            // Arrange
            var query = new GetEmployeeQuery { Id = 1 };
            var handler = new GetEmployeeQueryHandler(DbContext);

            var item = CreateItem(1);
            await DbContext.Employees.AddAsync(item);
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
        public async Task Get_should_return_null_when_employee_does_not_exist(int id)
        {
            // Arrange
            var query = new GetEmployeeQuery { Id = id };
            var handler = new GetEmployeeQueryHandler(DbContext);

            await DbContext.Employees.AddAsync(CreateItem(1));
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
                new ListEmployeesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListEmployeesQuery)null;
            var handler = new ListEmployeesQueryHandler(DbContext);

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
            var query = new ListEmployeesQuery { Page = page, PageSize = pageSize };
            var handler = new ListEmployeesQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_return_page_of_employees()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 5 };
            var handler = new ListEmployeesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                await DbContext.Employees.AddAsync(CreateItem(i));
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
        public async Task List_should_return_empty_result_if_employees_dont_exist()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 5 };
            var handler = new ListEmployeesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_filter_by_FullName()
        {
            // Arrange
            var query = new ListEmployeesQuery { Page = 1, PageSize = 20, FullName = CreateItem(3).FullName };
            var handler = new ListEmployeesQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 10))
            {
                await DbContext.Employees.AddAsync(CreateItem(i));
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var found = Assert.Single(result.Value.Results);
            Assert.Equal(query.FullName, found.FullName);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteEmployeeCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteEmployeeCommand)null;
            var handler = new DeleteEmployeeCommandHandler(DbContext);

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
            var command = new DeleteEmployeeCommand { Id = id };
            var handler = new DeleteEmployeeCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_employee()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { Id = 1 };
            var handler = new DeleteEmployeeCommandHandler(DbContext);

            await DbContext.Employees.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.Employees.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_employee()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { Id = 1034 };
            var handler = new DeleteEmployeeCommandHandler(DbContext);

            await DbContext.Employees.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, DbContext.Employees.Count());
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveEmployeeCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveEmployeeCommand)null;
            var handler = new SaveEmployeeCommandHandler(DbContext);

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
            var handler = new SaveEmployeeCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_employee()
        {
            // Arrange
            var request = CreateCommand(0, 1);
            var handler = new SaveEmployeeCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.Employees.SingleOrDefaultAsync(item => item.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_employee()
        {
            // Arrange
            var request = CreateCommand(1, 2);
            var handler = new SaveEmployeeCommandHandler(DbContext);

            await DbContext.Employees.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.Employees.SingleOrDefaultAsync(item => item.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Username, saved.Username);
            Assert.Equal(request.FullName, saved.FullName);
            Assert.Equal(request.PasswordHash, saved.PasswordHash);
            Assert.Equal(request.Email, saved.Email);
        }

        [Fact]
        public async Task Save_should_not_update_missing_employee()
        {
            // Arrange
            var request = CreateCommand(20, 2);
            var handler = new SaveEmployeeCommandHandler(DbContext);

            await DbContext.Employees.AddAsync(CreateItem(1));
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
        [InlineData("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void SaveValidator_should_return_false_when_Username_is_invalid(string value)
        {
            // Arrange
            var command = new SaveEmployeeCommand { Id = 0, Username = value };
            var validator = new SaveEmployeeCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveEmployeeCommand.Username), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            await AddRelatedData();
            var command = CreateCommand(0, 1);
            var validator = new SaveEmployeeCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
