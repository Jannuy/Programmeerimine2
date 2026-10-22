using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class MainViewPresenterTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMainView> _mainViewMock;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _mainViewMock = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClientMock.Object, _mainViewMock.Object);
        }

        private static OperationResult<PagedResult<FoodItem>> ValidListResponse()
        {
            return new OperationResult<PagedResult<FoodItem>>
            {
                Value = new PagedResult<FoodItem>
                {
                    Results = new List<FoodItem>
                    {
                        new FoodItem { Id = 1, Name = "Food item 1", ServingSize = 100, ServingUnit = 1, Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = "250/10/30" },
                        new FoodItem { Id = 2, Name = "Food item 2", ServingSize = 200, ServingUnit = 1, Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = "100/5/20" }
                    }
                }
            };
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult<PagedResult<FoodItem>>();
            faultyResponse.Errors.Add("An error occurred while fetching data.");

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = null)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            // Arrange
            var validResponse = ValidListResponse();

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = validResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_clear_fields_with_null_selection()
        {
            // Arrange
            var selectedItem = (FoodItem)null;

            _mainViewMock.SetupSet(view => view.CurrentId = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = "").Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentServingSize = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentServingUnit = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentEnergy = "").Verifiable();

            // Act
            _presenter.SetSelection(selectedItem);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            // Arrange
            var selectedItem = new FoodItem
            {
                Id = 5,
                Name = "Apple",
                ServingSize = 150,
                ServingUnit = 2,
                Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = "80/15/20"
            };

            _mainViewMock.SetupSet(view => view.CurrentId = 5).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = "Apple").Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentServingSize = 150).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentServingUnit = 2).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentEnergy = "80/15/20").Verifiable();

            // Act
            _presenter.SetSelection(selectedItem);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("An error occurred while saving data.");

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<FoodItem>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validListResponse = ValidListResponse();

            _mainViewMock.SetupGet(view => view.CurrentId).Returns(1);
            _mainViewMock.SetupGet(view => view.CurrentName).Returns("Apple");
            _apiClientMock
                .Setup(client => client.Save(It.Is<FoodItem>(item => item.Id == 1 && item.Name == "Apple")))
                .ReturnsAsync(new OperationResult())
                .Verifiable();
            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(validListResponse)
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = validListResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
            _mainViewMock.Verify(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            // Arrange
            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(false).Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("An error occurred while deleting data.");

            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(true);
            _mainViewMock.SetupGet(view => view.CurrentId).Returns(1);
            _apiClientMock
                .Setup(client => client.Delete(1))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validListResponse = ValidListResponse();

            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(true);
            _mainViewMock.SetupGet(view => view.CurrentId).Returns(1);
            _apiClientMock
                .Setup(client => client.Delete(1))
                .ReturnsAsync(new OperationResult())
                .Verifiable();
            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(validListResponse)
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.Verify(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Never);
        }
    }
}
