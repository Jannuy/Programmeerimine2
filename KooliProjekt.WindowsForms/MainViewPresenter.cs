using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class MainViewPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IMainView _mainView;

        private FoodItem _selectedItem;

        public MainViewPresenter(IApiClient apiClient, IMainView mainView)
        {
            _apiClient = apiClient;
            _mainView = mainView;
            _mainView.SetPresenter(this);
        }

        public async Task LoadData()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                _mainView.ShowError("Viga andmete laadimisel", response);
                _mainView.DataSource = null;
                return;
            }

            _mainView.DataSource = response.Value.Results;
        }

        public void SetSelection(FoodItem selectedItem)
        {
            _selectedItem = selectedItem;
            if (_selectedItem == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentName = "";
                _mainView.CurrentServingSize = 0;
                _mainView.CurrentServingUnit = 0;
                _mainView.CurrentEnergy = "";
            }
            else
            {
                _mainView.CurrentId = _selectedItem.Id;
                _mainView.CurrentName = _selectedItem.Name;
                _mainView.CurrentServingSize = _selectedItem.ServingSize;
                _mainView.CurrentServingUnit = _selectedItem.ServingUnit;
                _mainView.CurrentEnergy = _selectedItem.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G;
            }
        }

        public async Task Save()
        {
            var item = new FoodItem();
            item.Id = _mainView.CurrentId;
            item.Name = _mainView.CurrentName;
            item.ServingSize = _mainView.CurrentServingSize;
            item.ServingUnit = _mainView.CurrentServingUnit;
            item.Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G = _mainView.CurrentEnergy;

            var result = await _apiClient.Save(item);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga salvestamisel", result);
                return;
            }

            await LoadData();
        }

        public async Task Delete()
        {
            if (!_mainView.ConfirmDelete())
            {
                return;
            }

            var result = await _apiClient.Delete(_mainView.CurrentId);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga kustutamisel", result);
                return;
            }

            await LoadData();
        }
    }
}
