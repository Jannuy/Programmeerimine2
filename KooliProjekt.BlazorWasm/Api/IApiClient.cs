namespace KooliProjekt.BlazorWasm
{
    // Üldine API klient, mida kasutatakse kõigi olemite jaoks
    public interface IApiClient<T> where T : class
    {
        Task<OperationResult<T>> Get(int id);
        Task<OperationResult<PagedResult<T>>> List(int page, int pageSize);
        Task<OperationResult> Save(T item);
        Task<OperationResult> Delete(int id);
    }
}
