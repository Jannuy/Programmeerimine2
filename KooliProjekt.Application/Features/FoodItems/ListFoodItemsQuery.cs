using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodItems
{
    [ExcludeFromCodeCoverage]
    public class ListFoodItemsQuery : IRequest<OperationResult<PagedResult<FoodItem>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Name { get; set; }
    }
}
