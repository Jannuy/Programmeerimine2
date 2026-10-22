namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<FoodItem> DataSource { get; set; }
        FoodItem SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentName { get; set; }
        int CurrentServingSize { get; set; }
        int CurrentServingUnit { get; set; }
        string CurrentEnergy { get; set; }
        bool ConfirmDelete();
    }
}
