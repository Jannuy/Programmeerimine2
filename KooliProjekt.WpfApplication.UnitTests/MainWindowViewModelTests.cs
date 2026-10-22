using Moq;

namespace KooliProjekt.WpfApplication.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IDialogProvider> _dialogProviderMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _dialogProviderMock = new Mock<IDialogProvider>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object, _dialogProviderMock.Object);
        }

        private static OperationResult<PagedResult<FoodItem>> ValidListResponse()
        {
            return new OperationResult<PagedResult<FoodItem>>
            {
                Value = new PagedResult<FoodItem>
                {
                    Results = new List<FoodItem>
                    {
                        new FoodItem { Id = 1, Name = "Test 1" },
                        new FoodItem { Id = 2, Name = "Test 2" }
                    }
                }
            };
        }

        private static OperationResult ErrorResponse()
        {
            return new OperationResult
            {
                Errors = new List<string> { "Error" }
            };
        }

        [Fact]
        public void SelectedItem_should_return_correct_item()
        {
            // Arrange
            var item = new FoodItem { Id = 1, Name = "Test" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Equal(item, _viewModel.SelectedItem);
        }

        [Fact]
        public void SelectedItem_should_call_notify_property_changed()
        {
            // Arrange
            var item = new FoodItem { Id = 1, Name = "Test" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public async Task LoadData_should_load_data_from_api_client()
        {
            // Arrange
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(ValidListResponse())
                .Verifiable();

            // Act
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Equal(2, _viewModel.Data.Count);
            Assert.Equal(1, _viewModel.Data[0].Id);
            Assert.Equal(2, _viewModel.Data[1].Id);
        }

        [Fact]
        public async Task LoadData_should_show_error_when_api_client_fails()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<FoodItem>>
            {
                Errors = new List<string> { "Error" }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);
            Assert.Empty(_viewModel.Data);
        }

        [Fact]
        public void AddNew_Command_Should_Set_Empty_SelectedItem()
        {
            // Arrange
            _viewModel.SelectedItem = new FoodItem { Id = 5, Name = "Old" };

            // Act
            _viewModel.AddNewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
            Assert.Null(_viewModel.SelectedItem.Name);
        }

        [Fact]
        public void SaveCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var itemToSave = new FoodItem { Id = 1, Name = "Test" };

            _apiClientMock.Setup(client => client.Save(itemToSave))
                .ReturnsAsync(new OperationResult())
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(ValidListResponse())
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(itemToSave);

            // Assert
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public void SaveCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var itemToSave = new FoodItem { Id = 1, Name = "Test" };

            _apiClientMock.Setup(client => client.Save(itemToSave))
                .ReturnsAsync(ErrorResponse())
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(itemToSave);

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void SaveCommand_can_execute_when_selected_item_is_not_null()
        {
            // Arrange
            var item = new FoodItem { Id = 1, Name = "Test" };

            // Act
            var canExecuteWithoutSelection = _viewModel.SaveCommand.CanExecute(null);
            _viewModel.SelectedItem = item;
            var canExecuteWithSelection = _viewModel.SaveCommand.CanExecute(item);

            // Assert
            Assert.False(canExecuteWithoutSelection);
            Assert.True(canExecuteWithSelection);
        }

        [Fact]
        public void DeleteCommand_should_return_when_no_confirmation()
        {
            // Arrange
            var item = new FoodItem { Id = 1, Name = "Test" };
            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(item);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var item = new FoodItem { Id = 1, Name = "Test" };
            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(true);
            _apiClientMock.Setup(client => client.Delete(1))
                .ReturnsAsync(new OperationResult())
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(ValidListResponse())
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(item);

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Equal(2, _viewModel.Data.Count);
        }

        [Fact]
        public void DeleteCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var item = new FoodItem { Id = 1, Name = "Test" };
            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(true);
            _apiClientMock.Setup(client => client.Delete(1))
                .ReturnsAsync(ErrorResponse())
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(item);

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteCommand_can_execute_when_selected_item_is_not_null_and_id_is_not_zero()
        {
            // Arrange
            var newItem = new FoodItem { Id = 0 };
            var existingItem = new FoodItem { Id = 1 };

            // Act
            var canExecuteWithoutSelection = _viewModel.DeleteCommand.CanExecute(null);
            _viewModel.SelectedItem = newItem;
            var canExecuteWithNewItem = _viewModel.DeleteCommand.CanExecute(newItem);
            _viewModel.SelectedItem = existingItem;
            var canExecuteWithExistingItem = _viewModel.DeleteCommand.CanExecute(existingItem);

            // Assert
            Assert.False(canExecuteWithoutSelection);
            Assert.False(canExecuteWithNewItem);
            Assert.True(canExecuteWithExistingItem);
        }
    }
}
