using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.EmployeePatients;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class EmployeePatientTests : TestBase
    {
        private static EmployeePatient CreateItem(int i)
        {
            return new EmployeePatient
            {
                FirstName = "First " + i, LastName = "Last " + i, Gender = "Male", Notes = "Notes " + i
            };
        }

        private static SaveEmployeePatientCommand CreateCommand(int id, int i)
        {
            return new SaveEmployeePatientCommand
            {
                Id = id,
                FirstName = "First " + i, LastName = "Last " + i, Gender = "Male", Notes = "Notes " + i
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
                new GetEmployeePatientQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetEmployeePatientQuery)null;
            var handler = new GetEmployeePatientQueryHandler(DbContext);

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
            var query = new GetEmployeePatientQuery { Id = id };
            var handler = new GetEmployeePatientQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_employeePatient()
        {
            // Arrange
            var query = new GetEmployeePatientQuery { Id = 1 };
            var handler = new GetEmployeePatientQueryHandler(DbContext);

            var item = CreateItem(1);
            await DbContext.EmployeePatients.AddAsync(item);
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
        public async Task Get_should_return_null_when_employeePatient_does_not_exist(int id)
        {
            // Arrange
            var query = new GetEmployeePatientQuery { Id = id };
            var handler = new GetEmployeePatientQueryHandler(DbContext);

            await DbContext.EmployeePatients.AddAsync(CreateItem(1));
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
                new ListEmployeePatientsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListEmployeePatientsQuery)null;
            var handler = new ListEmployeePatientsQueryHandler(DbContext);

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
            var query = new ListEmployeePatientsQuery { Page = page, PageSize = pageSize };
            var handler = new ListEmployeePatientsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_return_page_of_employeePatients()
        {
            // Arrange
            var query = new ListEmployeePatientsQuery { Page = 1, PageSize = 5 };
            var handler = new ListEmployeePatientsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                await DbContext.EmployeePatients.AddAsync(CreateItem(i));
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
        public async Task List_should_return_empty_result_if_employeePatients_dont_exist()
        {
            // Arrange
            var query = new ListEmployeePatientsQuery { Page = 1, PageSize = 5 };
            var handler = new ListEmployeePatientsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_filter_by_LastName()
        {
            // Arrange
            var query = new ListEmployeePatientsQuery { Page = 1, PageSize = 20, LastName = CreateItem(3).LastName };
            var handler = new ListEmployeePatientsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 10))
            {
                await DbContext.EmployeePatients.AddAsync(CreateItem(i));
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var found = Assert.Single(result.Value.Results);
            Assert.Equal(query.LastName, found.LastName);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteEmployeePatientCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteEmployeePatientCommand)null;
            var handler = new DeleteEmployeePatientCommandHandler(DbContext);

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
            var command = new DeleteEmployeePatientCommand { Id = id };
            var handler = new DeleteEmployeePatientCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_employeePatient()
        {
            // Arrange
            var command = new DeleteEmployeePatientCommand { Id = 1 };
            var handler = new DeleteEmployeePatientCommandHandler(DbContext);

            await DbContext.EmployeePatients.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.EmployeePatients.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_employeePatient()
        {
            // Arrange
            var command = new DeleteEmployeePatientCommand { Id = 1034 };
            var handler = new DeleteEmployeePatientCommandHandler(DbContext);

            await DbContext.EmployeePatients.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, DbContext.EmployeePatients.Count());
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveEmployeePatientCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveEmployeePatientCommand)null;
            var handler = new SaveEmployeePatientCommandHandler(DbContext);

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
            var handler = new SaveEmployeePatientCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_employeePatient()
        {
            // Arrange
            var request = CreateCommand(0, 1);
            var handler = new SaveEmployeePatientCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.EmployeePatients.SingleOrDefaultAsync(item => item.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_employeePatient()
        {
            // Arrange
            var request = CreateCommand(1, 2);
            var handler = new SaveEmployeePatientCommandHandler(DbContext);

            await DbContext.EmployeePatients.AddAsync(CreateItem(1));
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.EmployeePatients.SingleOrDefaultAsync(item => item.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.FirstName, saved.FirstName);
            Assert.Equal(request.LastName, saved.LastName);
            Assert.Equal(request.Gender, saved.Gender);
            Assert.Equal(request.Notes, saved.Notes);
        }

        [Fact]
        public async Task Save_should_not_update_missing_employeePatient()
        {
            // Arrange
            var request = CreateCommand(20, 2);
            var handler = new SaveEmployeePatientCommandHandler(DbContext);

            await DbContext.EmployeePatients.AddAsync(CreateItem(1));
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
        public void SaveValidator_should_return_false_when_FirstName_is_invalid(string value)
        {
            // Arrange
            var command = new SaveEmployeePatientCommand { Id = 0, FirstName = value };
            var validator = new SaveEmployeePatientCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveEmployeePatientCommand.FirstName), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            await AddRelatedData();
            var command = CreateCommand(0, 1);
            var validator = new SaveEmployeePatientCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
