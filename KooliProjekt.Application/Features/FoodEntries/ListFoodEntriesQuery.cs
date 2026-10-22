using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.FoodEntries
{
    [ExcludeFromCodeCoverage]
    public class ListFoodEntriesQuery : IRequest<OperationResult<PagedResult<FoodEntry>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string FoodEntryItem { get; set; }
    }
}
