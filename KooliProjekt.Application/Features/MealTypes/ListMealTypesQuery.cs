using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MealTypes
{
    [ExcludeFromCodeCoverage]
    public class ListMealTypesQuery : IRequest<OperationResult<PagedResult<MealType>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
