namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<FoodItem>>> List(int page, int pageSize);
        Task<OperationResult> Save(FoodItem item);
        Task<OperationResult> Delete(int id);
    }
}
