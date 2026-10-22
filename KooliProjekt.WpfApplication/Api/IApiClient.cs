using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<FoodItem>>> List(int page, int pageSize);
        Task<OperationResult> Save(FoodItem item);
        Task<OperationResult> Delete(int id);
    }
}