using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly ObservableCollection<FoodItem> _data;
        private readonly IApiClient _apiClient;
        private readonly IDialogProvider _dialogProvider;

        public ICommand AddNewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }


        private FoodItem _selectedItem;

        public MainWindowViewModel() : this(new ApiClient(), new DialogProvider())
        {
        }

        public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogProvider)
        {
            _apiClient = apiClient;
            _dialogProvider = dialogProvider;
            _data = new ObservableCollection<FoodItem>();

            AddNewCommand = new RelayCommand<FoodItem>(
                foodItem =>
                {
                    SelectedItem = new FoodItem();
                });
            SaveCommand = new RelayCommand<FoodItem>(
                async foodItem =>
                {
                    var result = await _apiClient.Save(foodItem);
                    if(result.HasErrors)
                    {
                        ShowError("Failed to save data", result);
                        return;
                    }
                    await LoadData();
                },
                foodItem =>
                {
                    return SelectedItem != null;
                });
            DeleteCommand = new RelayCommand<FoodItem>(
                async foodItem =>
                {
                    var canDelete = _dialogProvider.Confirm("Are you sure you want to delete this item?");
                    if (!canDelete)
                    {
                        return;
                    }

                    var result = await _apiClient.Delete(foodItem.Id);
                    if(result.HasErrors) 
                    {
                        ShowError("Failed to delete data", result);
                        return;
                    }
                    await LoadData();
                },
                foodItem => 
                {                     
                    return SelectedItem != null && SelectedItem.Id != 0;
                });
        }

        public async Task LoadData()
        {
            var data = await _apiClient.List(1, 100);
            _data.Clear();

            if(data.HasErrors)
            {
                ShowError("Failed to load data", data);
                return;
            }

            foreach (var item in data.Value.Results)
            {
                _data.Add(item);
            }
        }

        public ObservableCollection<FoodItem> Data 
        { 
            get
            {
                return _data;
            }
        }

        public FoodItem SelectedItem 
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = "";
            var propertyErrors = "";

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            error = error.Trim();

            _dialogProvider.ShowError(error);
        }
    }
}